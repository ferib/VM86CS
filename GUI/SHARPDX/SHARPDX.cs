﻿using SharpDX;
using SharpDX.Direct2D1;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using x86CS.Devices;

namespace x86CS.GUI.SHARPDX
{
    public class SHARPDX : UI
    {
        private Form UIForm;
        private byte[] Memory;
        private SharpDX.Direct2D1.Bitmap BitmapBuffer;
        private int Width = 640;
        private int Height = 400;

        private int SkipCount = 0;

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
                PixelFormat = new PixelFormat(SharpDX.DXGI.Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied),
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
            BitmapBuffer = new SharpDX.Direct2D1.Bitmap(DrawTarget, new Size2(Width, Height), new BitmapProperties(DrawTarget.PixelFormat));

            //Start to draw
            DrawTarget.BeginDraw();
            DrawTarget.Clear(new SharpDX.Mathematics.Interop.RawColor4(0f, 0f, 0f, 1f));
            DrawTarget.EndDraw();

            // init events
            UIForm.KeyDown += new KeyEventHandler(EventsKeyboardDown);
            UIForm.KeyUp += new KeyEventHandler(EventsKeyboardUp);
            UIForm.FormClosed += new FormClosedEventHandler(EventsQuit);
        }

        void EventsQuit(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        void EventsKeyboardUp(object sender, KeyEventArgs e)
        {
            OnKeyUp((uint)e.KeyData); // TODO: replace with scancode
        }

        void EventsKeyboardDown(object sender, KeyEventArgs e)
        {
            OnKeyDown((uint)e.KeyValue); // TODO: replace with scancode
        }

        public override void Cycle()
        {
            // lets not fry my cpu
            SkipCount++;
            if (SkipCount % 10 == 0)
                SkipCount = 0;
            else
                return;

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
                int y = i / 160 * 16; // height pixels

                Color foreColour = vgaDevice.GetColour(attribute & 0xf);
                Color backColour = vgaDevice.GetColour((attribute >> 4) & 0xf);

                for (var f = fontOffset; f < fontOffset + 16; f++)
                {
                    int x = ((i % 160) / 2) * 8; // width pixels

                    for (var j = 7; j >= 0; j--)
                    {
                        var pixelIndex = Width * 4 * y + x * 4;
                        if (((fontBuffer[f] >> j) & 0x1) != 0)
                        {
                            Memory[pixelIndex + 0] = foreColour.R;
                            Memory[pixelIndex + 1] = foreColour.G;
                            Memory[pixelIndex + 2] = foreColour.B;
                            Memory[pixelIndex + 3] = foreColour.A;
                            var d = fontBuffer[f];
                        }
                        else
                        {
                            Memory[pixelIndex + 0] = backColour.R;
                            Memory[pixelIndex + 1] = backColour.G;
                            Memory[pixelIndex + 2] = backColour.B;
                            Memory[pixelIndex + 3] = backColour.A;
                        }
                        x++;
                    }
                    y++;
                }
            }

            // copy buffer to device
            BitmapBuffer.CopyFromMemory(Memory, Width * 4);

            // Draw bitmap to device
            DrawTarget.BeginDraw();
            DrawTarget.Clear(new SharpDX.Mathematics.Interop.RawColor4(1f, 0f, 1f, 1f));
            DrawTarget.DrawBitmap(BitmapBuffer, 1.0f, BitmapInterpolationMode.Linear);
            DrawTarget.EndDraw();
        }
    }
}
