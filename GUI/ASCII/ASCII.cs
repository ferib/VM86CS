using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using x86CS.Devices;

namespace x86CS.GUI.ASCII
{
    public class ASCII : UI
    {
        private Form UIForm;
        private byte[] Memory;
        private SharpDX.Direct2D1.Bitmap BitmapBuffer;
        private int Width = 640;
        private int Height = 400;

        private int SkipCount = 0;

        // NOTE: Detect all ASCII characters based on its pixel values and print text in a default console window?

        public ASCII(Form uiForm, VGA device)
            : base(uiForm, device)
        {
            UIForm = uiForm;
            //UIForm.Close();
            UIForm.Show();
        }

        public override void Init()
        {
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

            // convert console display buffer to ASCII characters
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
                        if (((fontBuffer[f] >> j) & 0x1) != 0)
                        {
                            // front
                        }
                        else
                        {
                           // back
                        }
                        x++;
                    }
                    y++;
                }
            }

            // copy buffer to device
            BitmapBuffer.CopyFromMemory(Memory, Width * 4);

            // Draw bitmap to device
        }
    }
}
