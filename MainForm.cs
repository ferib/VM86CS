﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace x86CS
{
    public partial class MainForm : Form
    {
        private readonly Machine machine;
        private readonly string[] screenText = new string[25];
        private int currLine, currPos;
        readonly Font panelFont = new Font("Courier New", 9.64f);
        bool stepping;
        readonly Thread machineThread;
        readonly Breakpoints breakpoints = new Breakpoints();

        public MainForm()
        {
            machine = new Machine();
            machine.WriteText += MachineWriteText;
            machine.WriteChar += MachineWriteChar;

            breakpoints.ItemAdded += BreakpointsItemAdded;
            breakpoints.ItemDeleted += BreakpointsItemDeleted;

            currLine = currPos = 0;

            InitializeComponent();

            PrintRegisters();

            mainPanel.Select();

            machineThread = new Thread(RunMachine);
            machineThread.Start();

            for (int i = 0; i < screenText.Length; i++)
            {
                screenText[i] = new string(' ', 80);
            }
        }

        void BreakpointsItemDeleted(object sender, IntEventArgs e)
        {
            machine.ClearBreakpoint(e.Number);
        }

        void BreakpointsItemAdded(object sender, IntEventArgs e)
        {
            machine.SetBreakpoint(e.Number);
        }

        private void RunMachine()
        {
            while (true)
            {
                if (machine.Running && !stepping)
                {
                    machine.RunCycle();
                    if (machine.CheckBreakpoint())
                    {
                        stepping = true;
                        machine.CPU.Debug = true;
                        Invoke((MethodInvoker)delegate { PrintRegisters(); SetCPULabel(machine.Operation); });
                    }
                    else
                        machine.CPU.Debug = false;

                }
            }
        }

        void MachineWriteChar(object sender, CharEventArgs e)
        {
            switch (e.Char)
            {
                case '\r':
                    currPos = 0;
                    break;
                case '\n':
                    currLine++;
                    break;
                default:
                    char[] chars = screenText[currLine].ToCharArray();

                    chars[currPos] = e.Char;

                    screenText[currLine] = new string(chars);
                    currPos++;
                    break;
            }

            if (currPos == 80)
            {
                currPos = 0;
                currLine++;
            }

            if (currLine >= 24)
            {
                currLine = 0;
                currPos = 0;
            }

            mainPanel.Invalidate();
        }

        private void SetCPULabel(string text)
        {
            cpuLabel.Text = text;
        }

        void MainPanelPaint(object sender, PaintEventArgs e)
        {
            if (e.ClipRectangle.Height == 0)
                return;
            for (int i = 0; i < 25; i++)
            {
                string line = screenText[i];
                if (String.IsNullOrEmpty(line))
                    continue;
                e.Graphics.DrawString(line, panelFont, Brushes.White, new PointF(0, i * panelFont.Height * 1.06f));
            }
        }

        void MachineWriteText(object sender, TextEventArgs e)
        {
            screenText[currLine++] = e.Text;
            if (currLine >= 25)
                currLine = 0;

            mainPanel.Invalidate();
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            machine.Stop();
            machineThread.Abort();
            Application.Exit();
        }

        private void PrintRegisters()
        {
            CPU.CPU cpu = machine.CPU;

            EAX.Text = cpu.EAX.ToString("X8");
            EBX.Text = cpu.EBX.ToString("X8");
            ECX.Text = cpu.ECX.ToString("X8");
            EDX.Text = cpu.EDX.ToString("X8");
            ESI.Text = cpu.ESI.ToString("X8");
            EDI.Text = cpu.EDI.ToString("X8");
            EBP.Text = cpu.EBP.ToString("X8");
            ESP.Text = cpu.ESP.ToString("X8");
            CS.Text = cpu.CS.ToString("X4");
            DS.Text = cpu.DS.ToString("X4");
            ES.Text = cpu.ES.ToString("X4");
            FS.Text = cpu.FS.ToString("X4");
            GS.Text = cpu.GS.ToString("X4");
            SS.Text = cpu.SS.ToString("X4");

            CF.Text = cpu.CF ? "CF" : "cf";
            PF.Text = cpu.PF ? "PF" : "pf";
            AF.Text = cpu.AF ? "AF" : "af";
            ZF.Text = cpu.ZF ? "ZF" : "zf";
            SF.Text = cpu.SF ? "SF" : "sf";
            TF.Text = cpu.TF ? "TF" : "tf";
            IF.Text = cpu.IF ? "IF" : "if";
            DF.Text = cpu.DF ? "DF" : "df";
            OF.Text = cpu.OF ? "OF" : "of";
            IOPL.Text = cpu.IOPL.ToString("X2");
            AC.Text = cpu.AC ? "AC" : "ac";
            NT.Text = cpu.NT ? "NT" : "nt";
            RF.Text = cpu.RF ? "RF" : "rf";
            VM.Text = cpu.VM ? "VM" : "vm";
            VIF.Text = cpu.VIF ? "VIF" : "vif";
            VIP.Text = cpu.VIP ? "VIP" : "vip";   
        }

        private void RunToolStripMenuItemClick(object sender, EventArgs e)
        {
            runToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
            machine.Start();
        }

        private void StopToolStripMenuItemClick(object sender, EventArgs e)
        {
            stopToolStripMenuItem.Enabled = false;
            runToolStripMenuItem.Enabled = true;
            machine.Stop();
        }

        private void MountToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (floppyOpen.ShowDialog() != DialogResult.OK)
                return;

            machine.FloppyDrive.MountImage(floppyOpen.FileName);
        }

        private void StepButtonClick(object sender, EventArgs e)
        {
            stepping = true;

            if (!machine.Running)
            {
                machine.Start();
                SetCPULabel(machine.Operation);
                PrintRegisters();
                return;
            }

            machine.CPU.Debug = true;
            machine.RunCycle();
            SetCPULabel(machine.Operation);
            PrintRegisters();
        }

        private void GoButtonClick(object sender, EventArgs e)
        {
            if (!machine.Running)
                machine.Start();

            machine.CPU.Debug = false;
            stepping = false;
        }

        private void MainFormFormClosed(object sender, FormClosedEventArgs e)
        {
            machineThread.Abort();
        }

        private void MainPanelPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            machine.KeyPresses.Push((char)e.KeyValue);
        }

        private void MainPanelClick(object sender, EventArgs e)
        {
            mainPanel.Select();
        }

        private void MemoryButtonClick(object sender, EventArgs e)
        {
            ushort seg = 0;
            ushort off = 0;

            try
            {
                seg = ushort.Parse(memSegment.Text, NumberStyles.HexNumber);
                off = ushort.Parse(memOffset.Text, NumberStyles.HexNumber);
            }
            catch
            {
            }
                
            var addr = (uint)((seg << 4) + off);

            memByte.Text = Memory.ReadByte(addr).ToString("X2");
            memWord.Text = Memory.ReadWord(addr).ToString("X4");
        }

        private void BreakpointsToolStripMenuItemClick(object sender, EventArgs e)
        {
            breakpoints.ShowDialog();
        }

        private void RestartToolStripMenuItemClick(object sender, EventArgs e)
        {
            machine.Restart();

            machine.CPU.Debug = false;
            stepping = false;
        }
    }
}
