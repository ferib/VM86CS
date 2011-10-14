﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using log4net;
using x86CS.Devices;
using System.Windows.Forms;

namespace x86CS
{
    public delegate void InteruptHandler();

    public class Machine
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Machine));

        private readonly Dictionary<int, int> breakpoints = new Dictionary<int, int>();
        private readonly MachineForm machineForm = new MachineForm();
        private readonly IDevice[] devices;
        private readonly PIC8259 picDevice;
        private readonly VGA vgaDevice;
        private readonly DMAController dmaController;
        private readonly Keyboard keyboard;

        private Dictionary<ushort, IOEntry> ioPorts;

        public Floppy FloppyDrive { get; private set; }
        public CPU.CPU CPU { get; private set; }

        public bool Running;
        public bool Stepping;

        public Machine()
        {
            picDevice = new PIC8259();
            vgaDevice = new VGA();
            FloppyDrive = new Floppy();
            dmaController = new DMAController();
            keyboard = new Keyboard();

            devices = new IDevice[]
                          {
                              FloppyDrive, new CMOS(), new Misc(), new PIT8253(), picDevice, keyboard, dmaController,
                              vgaDevice, new ATA()
                          };

            CPU = new CPU.CPU();
            
            SetupSystem();

            CPU.IORead += CPUIORead;
            CPU.IOWrite += CPUIOWrite;

            machineForm.Paint += MachineFormPaint;
            machineForm.KeyDown += MachineFormKeyDown;
            machineForm.KeyUp += MachineFormKeyUp;
            machineForm.Show();
            machineForm.BringToFront();
            machineForm.Select();
        }

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        void MachineFormKeyDown(object sender, KeyEventArgs e)
        {
            uint scanCode = MapVirtualKey((uint)e.KeyCode, 0);
            keyboard.KeyPress(scanCode);
        }

        void MachineFormKeyUp(object sender, KeyEventArgs e)
        {
            uint scanCode = MapVirtualKey((uint)e.KeyCode, 0);

            keyboard.KeyUp(scanCode);
        }

        void DMARaised(object sender, ByteArrayEventArgs e)
        {
            var device = sender as INeedsDMA;

            if (device == null)
                return;

            dmaController.DoTransfer(device.DMAChannel, e.ByteArray);
        }

        void IRQRaised(object sender, EventArgs e)
        {
            var device = sender as INeedsIRQ;

            if (device == null)
                return;

            picDevice.RequestInterrupt((byte)device.IRQNumber);
        }

        private void MachineFormPaint(object sender, PaintEventArgs e)
        {
           vgaDevice.GDIDraw(e.Graphics);
        }

        private void SetupIOEntry(ushort port, ReadCallback read, WriteCallback write)
        {
            var entry = new IOEntry {Read = read, Write = write};

            ioPorts.Add(port, entry);
        }

        private uint CPUIORead(ushort addr, int size)
        {
            IOEntry entry;

            var ret = (ushort) (!ioPorts.TryGetValue(addr, out entry) ? 0xffff : entry.Read(addr, size));
            Logger.Debug(String.Format("IO Read Port {0:X}, Value {1:X}", addr, ret));

            return ret;
        }

        private void CPUIOWrite(ushort addr, uint value, int size)
        {
            IOEntry entry;

            if (ioPorts.TryGetValue(addr, out entry))
                entry.Write(addr, value, size);

            Logger.Debug(String.Format("IO Write Port {0:X}, Value {1:X}", addr, value));
        }

        private void LoadBIOS()
        {
            FileStream biosStream = File.OpenRead("BIOS-bochs-legacy");
            var buffer = new byte[biosStream.Length];

            uint startAddr = (uint)(0xfffff - buffer.Length) + 1;

            biosStream.Read(buffer, 0, buffer.Length);
            Memory.BlockWrite(startAddr, buffer, buffer.Length);
            
            biosStream.Close();
            biosStream.Dispose();
        }

        private void LoadVGABios()
        {
            FileStream biosStream = File.OpenRead("VGABIOS-lgpl-latest");
            var buffer = new byte[biosStream.Length];

            biosStream.Read(buffer, 0, buffer.Length);
            Memory.BlockWrite(0xc0000, buffer, buffer.Length);

            biosStream.Close();
            biosStream.Dispose();
        }

        private void SetupSystem()
        {
            ioPorts = new Dictionary<ushort, IOEntry>();

            LoadBIOS();
            LoadVGABios();

            foreach(IDevice device in devices)
            {
                INeedsIRQ irqDevice = device as INeedsIRQ;
                INeedsDMA dmaDevice = device as INeedsDMA;

                if(irqDevice != null)
                    irqDevice.IRQ += IRQRaised;

                if(dmaDevice != null)
                    dmaDevice.DMA += DMARaised;

                foreach(int port in device.PortsUsed)
                    SetupIOEntry((ushort)port, device.Read, device.Write);
            }

            CPU.CS = 0xf000;
            CPU.IP = 0xfff0;
        }

        public void Restart()
        {
            Running = false;
            CPU.Reset();
            SetupSystem();
            Running = true;
        }

        public void SetBreakpoint(int addr)
        {
            if (breakpoints.ContainsKey(addr))
                return;

            breakpoints.Add(addr, addr);
        }

        public void ClearBreakpoint(int addr)
        {
            if (!breakpoints.ContainsKey(addr))
                return;

            breakpoints.Remove(addr);
        }

        public bool CheckBreakpoint()
        {
            //var cpuAddr = (uint)(currentInstruction.VirtualAddr);
          //  bool bpHit = breakpoints.Any(kvp => kvp.Value == cpuAddr);

          //  return bpHit;
            return false;
        }

        public void Start()
        {
            int addr = (int)((CPU.CS << 4) + CPU.IP);

            CPU.Fetch();
            
            Running = true;
        }

        public void Stop()
        {
            Running = false;
        }

        public void RunCycle(double frequency, ulong timerTicks)
        {
            if (Running)
            {
                CPU.Cycle();
                CPU.Fetch();
            }
        }
    }

    public struct IOEntry
    {
        public ReadCallback Read;
        public WriteCallback Write;
    }
}
