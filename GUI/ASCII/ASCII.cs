using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using x86CS.Devices;

namespace x86CS.GUI.ASCII
{
    public class ASCII : UI
    {
        private Form UIForm;
        private byte[] Memory;
        private int Width = 640;
        private int Height = 400;

        private int SkipCount = 0;

        // NOTE: Detect all ASCII characters based on its pixel values and print text in a default console window?

        public ASCII(Form uiForm, VGA device)
            : base(uiForm, device)
        {
            UIForm = uiForm;
            UIForm.Close();

            // spawn console window
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
            {
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process p = Process.Start(psi);

            StreamWriter sw = p.StandardInput;
            StreamReader sr = p.StandardOutput;

            sw.WriteLine("Initialising ASCII UI..");
            sr.Close();
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
                int charIndex = displayBuffer.Length / 16 / 8;

                Color foreColour = vgaDevice.GetColour(attribute & 0xf);
                Color backColour = vgaDevice.GetColour((attribute >> 4) & 0xf);

                for (var f = fontOffset; f < fontOffset + 16; f++)
                {
                    for (var a = 0; a < Keymap.KeyTableA.Length; a++)
                    {
                        bool isMatch = true;
                        byte[] pixelkey = (byte[])Keymap.KeyTableB[a];
                        for (var j = 0; j < 16; j++)
                        {
                            if (pixelkey[j] != BitConverter.GetBytes(fontBuffer[f])[0])
                            {
                                isMatch = false;
                                break;
                            }
                        }

                        if (isMatch)
                        {
                            Memory[f] = (byte)Keymap.KeyTableA[a];
                            if (f % (160 / 8) == 0)
                                Console.WriteLine(Keymap.KeyTableA[a]); // EOL
                            else
                                Console.Write(Keymap.KeyTableA[a]);
                            break;
                        }
                        else if (a == Keymap.KeyTableB.Length - 1)
                        {
                            // end of keys?
                            Memory[f] = (byte)'?';
                            Console.Write('?');
                        }
                    }
                }
            }

            // copy buffer to device
            //BitmapBuffer.CopyFromMemory(Memory, Width * 4);

            // Draw bitmap to device
        }
    }
}
