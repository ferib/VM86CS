using SharpDX;
using SharpDX.Direct2D1;
using System.Drawing;
using System.Windows.Forms;
using x86CS.Devices;

namespace x86CS.GUI.SHARPDX
{
    public class SHARPDX : UI
    {
        private Form UIForm;
        private byte[] Memory;
        private SharpDX.Direct2D1.Bitmap BackBufferBmp;
        private int Width = 160 * 16;
        private int Height = 160 * 8;

        private WindowRenderTarget DrawTarget;
        private Factory factory;


        public SHARPDX(Form uiForm, VGA device)
            : base(uiForm, device)
        {
            UIForm = uiForm;
            //UIForm.Close();
            UIForm.Show();

            Memory = new byte[Width * Height * 4]; // BGRA
        }

        public override void Init()
        {
            // create Direct2D1 Factory
            factory = new Factory();

            //Set Rendering properties
            RenderTargetProperties renderProp = new RenderTargetProperties()
            {
                PixelFormat = new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
            };

            //set hwnd target properties (permit to attach Direct2D to window)
            HwndRenderTargetProperties winProp = new HwndRenderTargetProperties()
            {
                Hwnd = UIForm.Handle,
                PixelSize = new Size2(Width, Height),
                PresentOptions = PresentOptions.None
            };

            //target creation
            DrawTarget = new WindowRenderTarget(factory, renderProp, winProp);

            // create Bitmap
            BackBufferBmp = new SharpDX.Direct2D1.Bitmap(DrawTarget, new Size2(Width, Height), new BitmapProperties(DrawTarget.PixelFormat));

            //Start to draw
            DrawTarget.BeginDraw();
            DrawTarget.Clear(new SharpDX.Mathematics.Interop.RawColor4(0f, 0f, 0f, 1f));
            DrawTarget.EndDraw();
        }

        //void EventsQuit(object sender, QuitEventArgs e)
        //{
        //    Application.Exit();
        //}

        //void EventsKeyboardUp(object sender, SdlDotNet.Input.KeyboardEventArgs e)
        //{
        //    OnKeyUp(e.Scancode);
        //}

        //void EventsKeyboardDown(object sender, SdlDotNet.Input.KeyboardEventArgs e)
        //{
        //    OnKeyDown(e.Scancode);
        //}

        public override void Cycle()
        {
            var fontBuffer = new byte[0x2000];
            var displayBuffer = new byte[0xfa0];

            x86CS.Memory.BlockRead(0xa0000, fontBuffer, fontBuffer.Length);
            x86CS.Memory.BlockRead(0xb8000, displayBuffer, displayBuffer.Length);

            // convert console display buffer to SharpDX window
            for (var i = 0; i < displayBuffer.Length; i += 2)
            {
                int currChar = displayBuffer[i];
                int fontOffset = currChar * 32;
                byte attribute = displayBuffer[i + 1];
                int y = i / 160 * 16;

                Color foreColour = vgaDevice.GetColour(attribute & 0xf);
                Color backColour = vgaDevice.GetColour((attribute >> 4) & 0xf);

                for (var f = fontOffset; f < fontOffset + 16; f++)
                {
                    int x = ((i % 160) / 2) * 8;

                    for (var j = 7; j >= 0; j--)
                    {
                        var pixelIndex = Width * 4 * y + x * 4;
                        if (((fontBuffer[f] >> j) & 0x1) != 0)
                        {
                            Memory[pixelIndex + 0] = foreColour.B;
                            Memory[pixelIndex + 1] = foreColour.G;
                            Memory[pixelIndex + 2] = foreColour.R;
                            Memory[pixelIndex + 3] = foreColour.A;
                        }
                        else
                        {
                            Memory[pixelIndex + 0] = backColour.B;
                            Memory[pixelIndex + 1] = backColour.G;
                            Memory[pixelIndex + 2] = backColour.R;
                            Memory[pixelIndex + 3] = backColour.A;
                        }
                        x++;
                    }
                    y++;
                }
            }
            // copy buffer to device
            BackBufferBmp.CopyFromMemory(Memory, Width * 4);

            // Draw bitmap to device
            DrawTarget.BeginDraw();
            DrawTarget.Clear(new SharpDX.Mathematics.Interop.RawColor4(0f, 0f, 0f, 1f));
            DrawTarget.DrawBitmap(BackBufferBmp, 1.0f, BitmapInterpolationMode.Linear);
            DrawTarget.EndDraw();
        }
    }
}
