using System.Windows.Forms;
using System.Drawing;
using x86CS.Devices;

namespace x86CS.GUI.TEST
{
    public class TEST : UI
    {
        private Form UIForm;


        public TEST(Form uiForm, VGA device)
            : base(uiForm, device)
        {
            UIForm = uiForm;
            UIForm.Close();
        }

        public override void Init()
        {

        }

        //void EventsQuit(object sender, QuitEventArgs e)
        //{
        //    Application.Exit();
        //}

        void EventsKeyboardUp(object sender, SdlDotNet.Input.KeyboardEventArgs e)
        {
            OnKeyUp(e.Scancode);
        }

        void EventsKeyboardDown(object sender, SdlDotNet.Input.KeyboardEventArgs e)
        {
            OnKeyDown(e.Scancode);
        }

        public override void Cycle()
        {
            // TODO implement this logic with SharpDX?
            var fontBuffer = new byte[0x2000];
            var displayBuffer = new byte[0xfa0];
            //Color[] data = new Color[screenBitmap.Width * screenBitmap.Height];
            Color[] data = new Color[520 * 520];

            Memory.BlockRead(0xa0000, fontBuffer, fontBuffer.Length);
            Memory.BlockRead(0xb8000, displayBuffer, displayBuffer.Length);

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
                        //if (((fontBuffer[f] >> j) & 0x1) != 0)
                        //    screenBitmap.Draw(new Point(x, y), foreColour);
                        //else
                        //    screenBitmap.Draw(new Point(x, y), backColour);
                        x++;
                    }
                    y++;
                }
            }
            //screen.Blit(screenBitmap);
            //screen.Update();
        }
    }
}
