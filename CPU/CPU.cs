﻿using System;
using System.Runtime.InteropServices;
using log4net;
using Bea;
using System.Collections.Generic;

namespace x86CS.CPU
{
    public enum RepeatPrefix
    {
        None = 0,
        Repeat,
        RepeatNotZero
    }

    public partial class CPU
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CPU));

        private readonly Segment[] segments;
        private readonly Register[] registers;
        private readonly uint[] controlRegisters;
        private CPUFlags eFlags;
        public event ReadCallback IORead;
        public event WriteCallback IOWrite;
        private TableRegister idtRegister, gdtRegister;
        private readonly GDTEntry realModeEntry;
        private bool inInterrupt;
        private byte interruptToRun;
        private Disasm currentInstruction;
        private RepeatPrefix repeatPrefix = RepeatPrefix.None;
        private int opSize = 16;
        private int addressSize = 16;

        public bool Halted { get; private set; }
        public uint CurrentAddr { get; private set; }
        public bool PMode { get; private set; }
        public int InterruptLevel { get; private set; }

        #region Registers

        public uint CR0
        {
            get { return controlRegisters[0]; }
            set { controlRegisters[0] = value; }
        }

        public uint CR1
        {
            get { return controlRegisters[1]; }
            set { controlRegisters[1] = value; }
        }

        public uint CR2
        {
            get { return controlRegisters[2]; }
            set { controlRegisters[2] = value; }
        }

        public uint CR3
        {
            get { return controlRegisters[3]; }
            set { controlRegisters[3] = value; }
        }

        public uint CR4
        {
            get { return controlRegisters[4]; }
            set { controlRegisters[4] = value; }
        }

        public uint EAX
        {
            get { return registers[(int)CPURegister.EAX].DWord; }
            set { registers[(int)CPURegister.EAX].DWord = value; }
        }

        public ushort AX
        {
            get { return registers[(int)CPURegister.EAX].Word; }
            set { registers[(int)CPURegister.EAX].Word = value; }
        }

        public byte AL
        {
            get { return registers[(int)CPURegister.EAX].LowByte; }
            set { registers[(int)CPURegister.EAX].LowByte = value; }
        }

        public byte AH
        {
            get { return registers[(int)CPURegister.EAX].HighByte; }
            set { registers[(int)CPURegister.EAX].HighByte = value; }
        }

        public uint EBX
        {
            get { return registers[(int)CPURegister.EBX].DWord; }
            set { registers[(int)CPURegister.EBX].DWord = value; }
        }

        public ushort BX
        {
            get { return registers[(int)CPURegister.EBX].Word; }
            set { registers[(int)CPURegister.EBX].Word = value; }
        }

        public byte BL
        {
            get { return registers[(int)CPURegister.EBX].LowByte; }
            set { registers[(int)CPURegister.EBX].LowByte = value; }
        }

        public byte BH
        {
            get { return registers[(int)CPURegister.EBX].HighByte; }
            set { registers[(int)CPURegister.EBX].HighByte = value; }
        }

        public uint ECX
        {
            get { return registers[(int)CPURegister.ECX].DWord; }
            set { registers[(int)CPURegister.ECX].DWord = value; }
        }

        public ushort CX
        {
            get { return registers[(int)CPURegister.ECX].Word; }
            set { registers[(int)CPURegister.ECX].Word = value; }
        }

        public byte CL
        {
            get { return registers[(int)CPURegister.ECX].LowByte; }
            set { registers[(int)CPURegister.ECX].LowByte = value; }
        }

        public byte CH
        {
            get { return registers[(int)CPURegister.ECX].HighByte; }
            set { registers[(int)CPURegister.ECX].HighByte = value; }
        }

        public uint EDX
        {
            get { return registers[(int)CPURegister.EDX].DWord; }
            set { registers[(int)CPURegister.EDX].DWord = value; }
        }

        public ushort DX
        {
            get { return registers[(int)CPURegister.EDX].Word; }
            set { registers[(int)CPURegister.EDX].Word = value; }
        }

        public byte DL
        {
            get { return registers[(int)CPURegister.EDX].LowByte; }
            set { registers[(int)CPURegister.EDX].LowByte = value; }
        }

        public byte DH
        {
            get { return registers[(int)CPURegister.EDX].HighByte; }
            set { registers[(int)CPURegister.EDX].HighByte = value; }
        }

        public uint ESI
        {
            get { return registers[(int)CPURegister.ESI].DWord; }
            set { registers[(int)CPURegister.ESI].DWord = value; }
        }

        public ushort SI
        {
            get { return registers[(int)CPURegister.ESI].Word; }
            set { registers[(int)CPURegister.ESI].Word = value; }
        }

        public uint EDI
        {
            get { return registers[(int)CPURegister.EDI].DWord; }
            set { registers[(int)CPURegister.EDI].DWord = value; }
        }

        public ushort DI
        {
            get { return registers[(int)CPURegister.EDI].Word; }
            set { registers[(int)CPURegister.EDI].Word = value; }
        }

        public uint EBP
        {
            get { return registers[(int)CPURegister.EBP].DWord; }
            set { registers[(int)CPURegister.EBP].DWord = value; }
        }

        public ushort BP
        {
            get { return registers[(int)CPURegister.EBP].Word; }
            set { registers[(int)CPURegister.EBP].Word = value; }
        }

        public uint EIP
        {
            get { return registers[(int)CPURegister.EIP].DWord; }
            set { registers[(int)CPURegister.EIP].DWord = value; }
        }

        public ushort IP
        {
            get { return registers[(int)CPURegister.EIP].Word; }
            set { registers[(int)CPURegister.EIP].Word = value; }
        }

        public uint ESP
        {
            get { return registers[(int)CPURegister.ESP].DWord; }
            set { registers[(int)CPURegister.ESP].DWord = value; }
        }

        public ushort SP
        {
            get { return registers[(int)CPURegister.ESP].Word; }
            set { registers[(int)CPURegister.ESP].Word = value; }
        }

        #endregion
        #region Segments
        public uint CS
        {
            get { return (ushort)segments[(int)SegmentRegister.CS].Selector; }
            set { SetSelector(SegmentRegister.CS, value); }
        }

        public ushort DS
        {
            get { return (ushort)segments[(int)SegmentRegister.DS].Selector; }
            set { SetSelector(SegmentRegister.DS, value); }
        }
        public ushort ES
        {
            get { return (ushort)segments[(int)SegmentRegister.ES].Selector; }
            set { SetSelector(SegmentRegister.ES, value); }
        }
        public ushort SS
        {
            get { return (ushort)segments[(int)SegmentRegister.SS].Selector; }
            set { SetSelector(SegmentRegister.SS, value); }
        }
        public ushort FS
        {
            get { return (ushort)segments[(int)SegmentRegister.FS].Selector; }
            set { SetSelector(SegmentRegister.FS, value); }
        }
        public ushort GS
        {
            get { return (ushort)segments[(int)SegmentRegister.GS].Selector; }
            set { SetSelector(SegmentRegister.GS, value); }
        }
        #endregion
        #region Flags
        public ushort EFlags
        {
            get { return (ushort)eFlags; }
            set { eFlags = (CPUFlags)value; }
        }

        public CPUFlags Flags
        {
            get { return eFlags; }
        }

        public bool CF
        {
            get { return GetFlag(CPUFlags.CF); }
            set { SetFlag(CPUFlags.CF, value); }
        }

        public bool PF
        {
            get { return GetFlag(CPUFlags.PF); }
            set { SetFlag(CPUFlags.PF, value); }
        }

        public bool AF
        {
            get { return GetFlag(CPUFlags.AF); }
            set { SetFlag(CPUFlags.AF, value); }
        }

        public bool ZF
        {
            get { return GetFlag(CPUFlags.ZF); }
            set { SetFlag(CPUFlags.ZF, value); }
        }

        public bool SF
        {
            get { return GetFlag(CPUFlags.SF); }
            set { SetFlag(CPUFlags.SF, value); }
        }

        public bool TF
        {
            get { return GetFlag(CPUFlags.TF); }
            set { SetFlag(CPUFlags.TF, value); }
        }

        public bool IF
        {
            get { return GetFlag(CPUFlags.IF); }
            set { SetFlag(CPUFlags.IF, value); }
        }

        public bool DF
        {
            get { return GetFlag(CPUFlags.DF); }
            set { SetFlag(CPUFlags.DF, value); }
        }

        public bool OF
        {
            get { return GetFlag(CPUFlags.OF); }
            set { SetFlag(CPUFlags.OF, value); }
        }

        public byte IOPL
        {
            get { return (byte)(((int)eFlags & 0x3000) >> 12); }
            set { eFlags = (CPUFlags)(value & 0x3000); }
        }

        public bool NT
        {
            get { return GetFlag(CPUFlags.NT); }
            set { SetFlag(CPUFlags.NT, value); }
        }

        public bool RF
        {
            get { return GetFlag(CPUFlags.RF); }
            set { SetFlag(CPUFlags.RF, value); }
        }

        public bool VM
        {
            get { return GetFlag(CPUFlags.VM); }
            set { SetFlag(CPUFlags.VM, value); }
        }

        public bool AC
        {
            get { return GetFlag(CPUFlags.AC); }
            set { SetFlag(CPUFlags.AC, value); }
        }

        public bool VIF
        {
            get { return GetFlag(CPUFlags.VIF); }
            set { SetFlag(CPUFlags.VIF, value); }
        }

        public bool VIP
        {
            get { return GetFlag(CPUFlags.VIP); }
            set { SetFlag(CPUFlags.VIP, value); }
        }

        public bool ID
        {
            get { return GetFlag(CPUFlags.ID); }
            set { SetFlag(CPUFlags.ID, value); }
        }

        #endregion

        public CPU()
        {
            PMode = false;
            segments = new Segment[6];
            registers = new Register[9];
            controlRegisters = new uint[5];
            idtRegister = new TableRegister();
            gdtRegister = new TableRegister();
            realModeEntry = new GDTEntry
                                {
                                    BaseAddress = 0,
                                    Is32Bit = false,
                                    IsAccessed = true,
                                    IsCode = false,
                                    Limit = 0xffff,
                                    IsWritable = true
                                };

            Halted = false;
            Reset();
        }

        public void Reset()
        {
            eFlags = CPUFlags.ZF | CPUFlags.IF;

            EIP = 0;
            CS = 0;
            EAX = 0;
            EBX = 0;
            ECX = 0;
            EDX = 0;
            EBP = 0;
            ESP = 0;
            DS = 0;
            ES = 0;
            FS = 0;
            GS = 0;
            inInterrupt = false;
        }

        private bool GetFlag(CPUFlags flag)
        {
            return (eFlags & flag) == flag;
        }

        private void SetFlag(CPUFlags flag, bool value)
        {
            if (value)
                eFlags |= flag;
            else
                eFlags &= ~flag;
        }

        private uint GetVirtualAddress(SegmentRegister segment, uint offset)
        {
            Segment seg = segments[(int)segment];

            return seg.GDTEntry.BaseAddress + offset;
        }

        private byte SegReadByte(SegmentRegister segment, uint offset)
        {
            uint virtAddr = GetVirtualAddress(segment, offset);
            byte ret = Memory.ReadByte(virtAddr);

            return ret;
        }

        private ushort SegReadWord(SegmentRegister segment, uint offset)
        {
            uint virtAddr = GetVirtualAddress(segment, offset);
            ushort ret = Memory.ReadWord(virtAddr);

            return ret;
        }

        private uint SegReadDWord(SegmentRegister segment, uint offset)
        {
            uint virtAddr = GetVirtualAddress(segment, offset);
            uint ret = Memory.ReadDWord(virtAddr);

            return ret;
        }

        private void SegWriteByte(SegmentRegister segment, uint offset, byte value)
        {
            uint virtAddr = GetVirtualAddress(segment, offset);

            Memory.WriteByte(virtAddr, value);
        }

        private void SegWriteWord(SegmentRegister segment, uint offset, ushort value)
        {
            uint virtAddr = GetVirtualAddress(segment, offset);

            Memory.WriteWord(virtAddr, value);
        }

        private void SegWriteDWord(SegmentRegister segment, uint offset, uint value)
        {
            uint virtAddr = GetVirtualAddress(segment, offset);

            Memory.WriteDWord(virtAddr, value);
        }

        public uint StackPop()
        {
            uint ret;

            if (PMode)
            {
                if (opSize == 32)
                {
                    ret = SegReadWord(SegmentRegister.SS, ESP);
                    ESP += 2;
                }
                else
                {
                    ret = SegReadDWord(SegmentRegister.SS, ESP);
                    ESP += 4;
                }
            }
            else
            {
                if (opSize == 32)
                {
                    ret = SegReadDWord(SegmentRegister.SS, SP);
                    SP += 4;
                }
                else
                {
                    ret = SegReadWord(SegmentRegister.SS, SP);
                    SP += 2;
                }
            }
            return ret;
        }

        /*public void StackPush(ushort value)
        {
            if (opSize == 32)
                ESP -= 2;
            else
                SP -= 2;

            SegWriteWord(SegmentRegister.SS, SP, value);
        }*/

        public void StackPush(uint value)
        {
            if (opSize == 32)
            {
                ESP -= 4;
                SegWriteDWord(SegmentRegister.SS, ESP, value);
            }
            else
            {
                SP -= 2;
                SegWriteWord(SegmentRegister.SS, SP, (ushort) value);
            }
        }

        private GDTEntry GetSelectorEntry(uint selector)
        {
            int entrySize = Marshal.SizeOf(typeof(GDTEntry));
            var gdtBytes = new byte[entrySize];

            Memory.BlockRead(gdtRegister.Base + selector, gdtBytes, gdtBytes.Length);
            IntPtr p = Marshal.AllocHGlobal(entrySize);
            Marshal.Copy(gdtBytes, 0, p, entrySize);
            var entry = (GDTEntry)Marshal.PtrToStructure(p, typeof(GDTEntry));
            Marshal.FreeHGlobal(p);

            return entry;
        }

        private void SetSelector(SegmentRegister segment, uint selector)
        {
            if (PMode)
            {
                segments[(int)segment].Selector = selector;
                segments[(int)segment].GDTEntry = GetSelectorEntry(selector);
            }
            else
            {
                segments[(int)segment].Selector = selector;
                segments[(int)segment].GDTEntry = realModeEntry;
                segments[(int)segment].GDTEntry.BaseAddress = selector << 4;
            }
        }

        private void ProcedureEnter(ushort size, byte level)
        {
            var nestingLevel = (ushort)(level % 32);

            StackPush(BP);
            ushort frameTemp = SP;

            if (nestingLevel > 0)
            {
                for (int i = 1; i < nestingLevel - 1; i++)
                {
                    BP -= 2;
                    StackPush(SegReadWord(SegmentRegister.SS, BP));
                }
                StackPush(frameTemp);
            }

            BP = frameTemp;
            SP = (ushort)(BP - size);
        }

        private void ProcedureLeave()
        {
            SP = BP;
            BP = (ushort)StackPop();
        }

        private void DoJump(uint segment, uint offset, bool relative)
        {
            Segment codeSegment = segments[(int)SegmentRegister.CS];
            uint tempEIP;

            if (PMode == false && ((CR0 & 0x1) == 0x1))
                PMode = true;
            else if (PMode && ((CR0 & 0x1) == 0))
                PMode = false;

            if (segment == CS)
            {
                if (relative)
                {
                    var relOffset = (int)offset;

                    tempEIP = (uint)(EIP + relOffset);
                }
                else
                    tempEIP = offset;

                if (tempEIP > codeSegment.GDTEntry.Limit)
                    throw new Exception("General Fault Code 0");
            }
            else
            {
                if (PMode)
                {
                    if (segment == 0)
                        throw new Exception("Null segment selector");

                    if (segment > (gdtRegister.Limit))
                        throw new Exception("Selector out of range");

                    GDTEntry newEntry = GetSelectorEntry(segment);

                    if (!newEntry.IsCode)
                        throw new Exception("Segment is not code");

                    CS = segment;
                    if (relative)
                    {
                        var relOffset = (int)offset;

                        tempEIP = (uint)(EIP + relOffset);
                    }
                    else
                        tempEIP = offset;                    
                }
                else
                {
                    if (relative)
                        tempEIP = EIP + offset;
                    else
                        tempEIP = offset;
                    if (tempEIP > codeSegment.GDTEntry.Limit)
                        throw new Exception("EIP Out of range");

                    CS = segment;

                    if (opSize == 32)
                        EIP = tempEIP;
                    else
                        EIP = (ushort)tempEIP;
                }
            }
            if (opSize == 32)
                EIP = tempEIP;
            else
                EIP = (ushort)tempEIP;
        }

        private void CallInterrupt(byte vector)
        {
            //Logger.Debug("INT" + vector.ToString("X"));
            StackPush((ushort)Flags);
            IF = false;
            TF = false;
            AC = false;
            StackPush(CS);
            StackPush(IP);

            CS = Memory.ReadWord((uint)(vector * 4) + 2);
            EIP = Memory.ReadWord((uint)(vector * 4));

            InterruptLevel++;
        }

        public void Interrupt(int vector, int irq)
        {
            inInterrupt = true;
            interruptToRun = (byte)vector;
        }

        private void DumpRegisters()
        {
            Logger.Debug(String.Format("AX {0:X4} BX {1:X4} CX {2:X4} DX {3:X4}", AX, BX, CX, DX));
            Logger.Debug(String.Format("SI {0:X4} DI {1:X4} SP {2:X4} BP {3:X4}", SI, DI, SP, BP));
            Logger.Debug(String.Format("CS {0:X4} DS {1:X4} ES {2:X4} SS {3:X4}", CS, DS, ES, SS));
        }

        [Flags]
        private enum RegisterType
        {
            NoRegister = 0x0000,
            MMX = 0x10000,
            General = 0x20000,
            FPU = 0x40000,
            SSE = 0x80000,
            CR = 0x100000,
            DR = 0x200000,
            Special = 0x400000,
            MemoryManagement = 0x800000,
            Segment = 0x1000000
        }

        private enum OperandType
        {
            Memory,
            Register,
            Immediate
        }

        private struct Operand
        {
            public uint Value;
            public uint Register;
            public RegisterType RegisterType;
            public uint Address;
            public SegmentRegister Segment;
            public uint OperandSize;
            public bool High;
            public OperandType Type;
        }

        private uint ReadGeneralRegsiter(uint register, int size, bool high)
        {
            if (size == 32)
                return registers[register].DWord;
            else if (size == 16)
                return registers[register].Word;
            else if (size == 8 && high)
                return registers[register].HighByte;
            else if (size == 8)
                return registers[register].LowByte;
            else
                System.Diagnostics.Debugger.Break();

            return 0xffffffff;
        }

        private uint ReadRegister(Operand operand)
        {
            switch (operand.RegisterType)
            {
                case RegisterType.General:
                    return ReadGeneralRegsiter(operand.Register, (int)operand.OperandSize, operand.High);
                default:
                    break;
            }

            return 0;
        }

        private void WriteRegister(Operand operand, uint value)
        {
            switch (operand.RegisterType)
            {
                case RegisterType.General:
                    if (operand.OperandSize == 32)
                        registers[operand.Register].DWord = value;
                    else if (operand.OperandSize == 16)
                        registers[operand.Register].Word = (ushort)value;
                    else if (operand.OperandSize == 8 && operand.High)
                        registers[operand.Register].HighByte = (byte)value;
                    else if (operand.OperandSize == 8)
                        registers[operand.Register].LowByte = (byte)value;
                    else
                        System.Diagnostics.Debugger.Break();
                    break;
                case RegisterType.Segment:
                    segments[operand.Register].Selector = value;
                    break;
                default:
                    break;
            }
        }

        private uint GetOperandValue(Operand operand)
        {
            if (operand.Type == OperandType.Register)
                return ReadRegister(operand);
            else if (operand.Type == OperandType.Immediate)
                return operand.Value;

            return 0;
        }

        private void SetOperandValue(Operand operand, uint value)
        {
            if (operand.Type == OperandType.Register)
                WriteRegister(operand, value);
            else if (operand.Type == OperandType.Memory)
            {
                SegmentRegister segment;

                if (operand.Segment == SegmentRegister.Default)
                    segment = SegmentRegister.DS;
                else
                    segment = operand.Segment;

                if (operand.OperandSize == 8)
                    SegWriteByte(segment, operand.Address, (byte)value);
                else if (operand.OperandSize == 16)
                    SegWriteWord(segment, operand.Address, (ushort)value);
                else if (operand.OperandSize == 32)
                    SegWriteDWord(segment, operand.Address, value);
            }
            else
                System.Diagnostics.Debugger.Break();
        }

        private uint RegisterFromBeaRegister(int register)
        {
            switch (register)
            {
                case 0x1:
                    return 0;
                case 0x2:
                    return 1;
                case 0x4:
                    return 2;
                case 0x8:
                    return 3;
                case 0x10:
                    return 4;
                case 0x20:
                    return 5;
                case 0x40:
                    return 6;
                case 0x80:
                    return 7;
                default:
                    return 0xffffffff;
            }
        }

        private Operand ProcessArgument(ArgumentType argument)
        {
            Operand operand = new Operand();
            BeaConstants.ArgumentType argType;

            operand.OperandSize = (uint)argument.ArgSize;

            argType = (BeaConstants.ArgumentType)(argument.ArgType & 0xffff0000);
            if ((argType & BeaConstants.ArgumentType.MEMORY_TYPE) == BeaConstants.ArgumentType.MEMORY_TYPE)
            {
                uint baseRegister = 0;

                if (argument.Memory.IndexRegister != 0 || argument.Memory.Scale != 0)
                    System.Diagnostics.Debugger.Break();
                if (argument.Memory.BaseRegister != 0)
                {
                    baseRegister = RegisterFromBeaRegister(argument.Memory.BaseRegister);
                    operand.Address = ReadGeneralRegsiter(baseRegister, argument.ArgSize, false);
                }

                if (argument.Memory.Displacement < 0)
                    System.Diagnostics.Debugger.Break();
                operand.Address += (uint)argument.Memory.Displacement;
                operand.Type = OperandType.Memory;
            }
            else if ((argType & BeaConstants.ArgumentType.REGISTER_TYPE) == BeaConstants.ArgumentType.REGISTER_TYPE)
            {
                operand.RegisterType = (RegisterType)((int)argType & ~0xf0000000);
                operand.Register = RegisterFromBeaRegister(argument.ArgType & 0x0000ffff);
                
                if (argument.ArgPosition == 1)
                    operand.High = true;
                else
                    operand.High = false;

                operand.Type = OperandType.Register;
            }
            else if ((argType & BeaConstants.ArgumentType.CONSTANT_TYPE) == BeaConstants.ArgumentType.CONSTANT_TYPE)
            {
                operand.Value = (uint)currentInstruction.Instruction.Immediate;
                operand.Type = OperandType.Immediate;
            }

            if (currentInstruction.Prefix.CSPrefix == 1)
                operand.Segment = SegmentRegister.CS;
            else if (currentInstruction.Prefix.DSPrefix == 1)
                operand.Segment = SegmentRegister.DS;
            else if (currentInstruction.Prefix.ESPrefix == 1)
                operand.Segment = SegmentRegister.ES;
            else if (currentInstruction.Prefix.FSPrefix == 1)
                operand.Segment = SegmentRegister.FS;
            else if (currentInstruction.Prefix.GSPrefix == 1)
                operand.Segment = SegmentRegister.GS;
            else if (currentInstruction.Prefix.SSPrefix == 1)
                operand.Segment = SegmentRegister.SS;
            else
                operand.Segment = SegmentRegister.Default;

            return operand;
        }

        private Operand[] ProcessOperands()
        {
            List<Operand> operands = new List<Operand>();

            if (currentInstruction.Argument1.ArgType != (int)BeaConstants.ArgumentType.NO_ARGUMENT)
                operands.Add(ProcessArgument(currentInstruction.Argument1));
            if (currentInstruction.Argument2.ArgType != (int)BeaConstants.ArgumentType.NO_ARGUMENT)
                operands.Add(ProcessArgument(currentInstruction.Argument2));
            if (currentInstruction.Argument3.ArgType != (int)BeaConstants.ArgumentType.NO_ARGUMENT)
                operands.Add(ProcessArgument(currentInstruction.Argument3));

            return operands.ToArray();
        }

        public void Cycle(Disasm instruction, int len)
        {
            Operand[] operands;

            currentInstruction = instruction;

            if(inInterrupt)
            {
                CallInterrupt(interruptToRun);
                inInterrupt = false;
                Halted = false;
                return;
            }

            if (Halted)
                return;

            if (currentInstruction.Prefix.AddressSize != 0)
                System.Diagnostics.Debugger.Break();
            if (currentInstruction.Prefix.OperandSize != 0)
                System.Diagnostics.Debugger.Break();

            if (currentInstruction.Prefix.RepPrefix != 0)
                repeatPrefix = RepeatPrefix.Repeat;
            if (currentInstruction.Prefix.RepnePrefix != 0)
                repeatPrefix = RepeatPrefix.RepeatNotZero;

            operands = ProcessOperands();

           // if(InterruptLevel == 0)
                Logger.Debug(String.Format("{0:X}:{1:X}    {2}", CS, EIP, instruction.CompleteInstr));

            EIP += (uint)len;

            #region old code
            /*            if (extPrefix)
            {
                #region extended opcodes
                switch (opCode)
                {
                    case 0x01:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        memAddress = ProcessRegMem(rmData, out tempDWord, out sourceDWord);
                        switch (rmData.Register)
                        {
                            case 2:
                                sourceWord = SegReadWord(overrideSegment, memAddress);
                                sourceDWord = SegReadDWord(overrideSegment, memAddress + 2);
                                if (opSize == 32)
                                {
                                    gdtRegister.Limit = sourceWord;
                                    gdtRegister.Base = sourceDWord;
                                }
                                else
                                {
                                    gdtRegister.Limit = sourceWord;
                                    gdtRegister.Base = (sourceDWord & 0x00ffffff);
                                }
                                break;
                            case 3:
                                sourceWord = SegReadWord(overrideSegment, memAddress);
                                sourceDWord = SegReadDWord(overrideSegment, memAddress + 2);
                                if (opSize == 32)
                                {
                                    idtRegister.Limit = sourceWord;
                                    idtRegister.Base = sourceDWord;
                                }
                                else
                                {
                                    idtRegister.Limit = sourceWord;
                                    idtRegister.Base = (sourceDWord & 0x00ffffff);
                                }
                                break;
                        }
                        break;
                    case 0x20:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        registers[rmData.Register].DWord = controlRegisters[rmData.RegMem];
                        break;
                    case 0x22:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        controlRegisters[rmData.RegMem] = registers[rmData.Register].DWord;
                        break;
                    case 0x80:
                        if (OF)
                            EIP = offset;
                        break;
                    case 0x81:
                        if (!OF)
                            EIP = offset;
                        break;
                    case 0x82:
                        if (CF)
                            EIP = offset;
                        break;
                    case 0x83:
                        if (!CF)
                            EIP = offset;
                        break;
                    case 0x84:
                        if (ZF)
                            EIP = offset;
                        break;
                    case 0x85:
                        if (!ZF)
                            EIP = offset;
                        break;
                    case 0x86:
                        if (CF || ZF)
                            EIP = offset;
                        break;
                    case 0x87:
                        if (!CF && !ZF)
                            EIP = offset;
                        break;
                    case 0x88:
                        if (SF)
                            EIP = offset;
                        break;
                    case 0x89:
                        if (!SF)
                            EIP = offset;
                        break;
                    case 0x8a:
                        if (PF)
                            EIP = offset;
                        break;
                    case 0x8b:
                        if (!PF)
                            EIP = offset;
                        break;
                    case 0x8c:
                        if (SF != OF)
                            EIP = offset;
                        break;
                    case 0x8d:
                        if (SF == OF)
                            EIP = offset;
                        break;
                    case 0x8e:
                        if (ZF || (SF != OF))
                            EIP = offset;
                        break;
                    case 0x8f:
                        if (!ZF && SF == OF)
                            EIP = offset;
                        break;
                    case 0x90:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (OF ? 1 : 0));
                        break;
                    case 0x91:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (!OF ? 1 : 0));
                        break;
                    case 0x92:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (CF ? 1 : 0));
                        break;
                    case 0x93:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (!CF ? 1 : 0));
                        break;
                    case 0x94:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (ZF ? 1 : 0));
                        break;
                    case 0x95:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (!ZF ? 1 : 0));
                        break;
                    case 0x96:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        if (ZF || CF)
                            WriteRegMem(rmData, memAddress, 1);
                        else
                            WriteRegMem(rmData, memAddress, 0);
                        break;
                    case 0x97:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        if (!CF && !ZF)
                            WriteRegMem(rmData, memAddress, 1);
                        else
                            WriteRegMem(rmData, memAddress, 0);
                        break;
                    case 0x98:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (SF ? 1 : 0));
                        break;
                    case 0x99:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (!SF ? 1 : 0));
                        break;
                    case 0x9a:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (PF ? 1 : 0));
                        break;
                    case 0x9b:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (!PF ? 1 : 0));
                        break;
                    case 0x9c:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (SF != OF ? 1 : 0));
                        break;
                    case 0x9d:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte) (SF == OF ? 1 : 0));
                        break;
                    case 0x9e:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        if (ZF || SF != OF)
                            WriteRegMem(rmData, memAddress, 1);
                        else
                            WriteRegMem(rmData, memAddress, 0);
                        break;
                    case 0x9f:
                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        if (!ZF && SF == OF)
                            WriteRegMem(rmData, memAddress, 1);
                        else
                            WriteRegMem(rmData, memAddress, 0);
                        break;
                    case 0xb6:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        if (opSize == 32)
                            registers[rmData.Register].DWord = (uint)sourceByte;
                        else
                            registers[rmData.Register].Word = (ushort)sourceByte;
                        break;
                    case 0xb7:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        ProcessRegMem(rmData, out destWord, out sourceWord);
                        registers[rmData.Register].DWord = sourceWord;
                        break;
                    case 0xa0:
                        StackPush(FS);
                        break;
                    case 0xa1:
                        FS = (ushort) StackPop();
                        break;
                    case 0xa8:
                        StackPush(GS);
                        break;
                    case 0xa9:
                        GS = (ushort)StackPop();
                        break;
                    case 0xaf:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            registers[rmData.Register].DWord = SignedMultiply(destDWord, sourceDWord);
                        }
                        else
                        {
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            registers[rmData.Register].Word = SignedMultiply(destWord, sourceWord);
                        }
                        break;
                    case 0xb4:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            registers[rmData.Register].DWord = destDWord;
                            FS = ReadRegMemWord(rmData, memAddress + 2);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            registers[rmData.Register].Word = destWord;
                            FS = ReadRegMemWord(rmData, memAddress + 2);
                        }
                        break;
                    case 0xb5:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            registers[rmData.Register].DWord = destDWord;
                            GS = ReadRegMemWord(rmData, memAddress + 2);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            registers[rmData.Register].Word = destWord;
                            GS = ReadRegMemWord(rmData, memAddress + 2);
                        }
                        break;
                    default:
                        System.Diagnostics.Debugger.Break();
                        break;
                }
                #endregion
            }
            else
            {
                switch (opCode)
                {
                    #region Add with carry
                    case 0x10:
                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        destByte = AddWithCarry(destByte, sourceByte);
                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0x11:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            destDWord = AddWithCarry(destDWord, sourceDWord);
                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            destWord = AddWithCarry(destWord, sourceWord);
                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0x12:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        destByte = AddWithCarry(destByte, sourceByte);
                        SetByteReg(rmData.Register, destByte);
                        break;
                    case 0x13:
                        if (opSize == 32)
                        {
                            System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            destDWord = AddWithCarry(destDWord, sourceDWord);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            destWord = AddWithCarry(destWord, sourceWord);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x14:
                        AddWithCarry((byte)operands[0]);
                        break;
                    case 0x15:
                        if (opSize == 32)
                            AddWithCarry((uint)operands[0]);
                        else
                            AddWithCarry((ushort)operands[0]);
                        break;
                    #endregion
                    #region Add
                    case 0x00:
                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        destByte = Add(destByte, sourceByte);
                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0x01:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            destDWord = Add(destDWord, sourceDWord);
                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            destWord = Add(destWord, sourceWord);
                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0x02:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        destByte = Add(destByte, sourceByte);
                        SetByteReg(rmData.Register, destByte);
                        break;
                    case 0x03:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            destDWord = Add(destDWord, sourceDWord);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            destWord = Add(destWord, sourceWord);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x04:
                        Add((byte)operands[0]);
                        break;
                    case 0x05:
                        if(opSize == 32)
                            Add((uint)operands[0]);
                        else
                            Add((ushort)operands[0]);
                        break;
                    #endregion
                    #region Sub With Borrow
                    case 0x18:
                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        destByte = SubWithBorrow(destByte, sourceByte);
                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0x19:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            destDWord = SubWithBorrow(destDWord, sourceDWord);
                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            destWord = SubWithBorrow(destWord, sourceWord);
                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0x1a:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        destByte = SubWithBorrow(destByte, sourceByte);
                        SetByteReg(rmData.Register, destByte);
                        break;
                    case 0x1b:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            destDWord = SubWithBorrow(destDWord, sourceDWord);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            destWord = SubWithBorrow(destWord, sourceWord);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x1c:
                        SubWithBorrow((byte)operands[0]);
                        break;
                    case 0x1d:
                        SubWithBorrow((ushort)operands[0]);
                        break;
                    #endregion
                    #region Sub
                    case 0x28:
                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        destByte = Subtract(destByte, sourceByte);
                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0x29:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            destDWord = Subtract(destDWord, sourceDWord);
                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            destWord = Subtract(destWord, sourceWord);
                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0x2a:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        destByte = Subtract(destByte, sourceByte);
                        SetByteReg(rmData.Register, destByte);
                        break;
                    case 0x2b:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            destDWord = Subtract(destDWord, sourceDWord);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            destWord = Subtract(destWord, sourceWord);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x2c:
                        Subtract((byte)operands[0]);
                        break;
                    case 0x2d:
                        if (opSize == 32)
                            Subtract((uint)operands[0]);
                        else
                            Subtract((ushort)operands[0]);
                        break;
                    #endregion
                    #region And
                    case 0x20:
                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        destByte = And(destByte, sourceByte);
                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0x21:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            destDWord = And(destDWord, sourceDWord);
                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            destWord = And(destWord, sourceWord);
                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0x22:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        destByte = And(destByte, sourceByte);
                        SetByteReg(rmData.Register, destByte);
                        break;
                    case 0x23:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            destDWord = And(destDWord, sourceDWord);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            destWord = And(destWord, sourceWord);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x24:
                        AL = And((byte)operands[0]);
                        break;
                    case 0x25:
                        if (opSize == 32)
                            EAX = And((uint)operands[0]);
                        else
                            AX = And((ushort)operands[0]);
                        break;
                    #endregion
                    #region Or
                    case 0x08:
                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        destByte = Or(destByte, sourceByte);
                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0x09:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            destDWord = Or(destDWord, sourceDWord);
                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            destWord = Or(destWord, sourceWord);
                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0x0a:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        destByte = Or(destByte, sourceByte);
                        SetByteReg(rmData.Register, destByte);
                        break;
                    case 0x0b:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            destDWord = Or(destDWord, sourceDWord);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            destWord = Or(destWord, sourceWord);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x0c:
                        Or((byte)operands[0]);
                        break;
                    case 0x0d:
                        if (opSize == 32)
                            Or((uint)operands[0]);
                        else
                            Or((ushort)operands[0]);
                        break;
                    #endregion
                    #region Xor
                    case 0x30:
                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        destByte = Xor(destByte, sourceByte);
                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0x31:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            destDWord = Xor(destDWord, sourceDWord);
                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            destWord = Xor(destWord, sourceWord);
                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0x32:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        destByte = Xor(destByte, sourceByte);
                        SetByteReg(rmData.Register, destByte);
                        break;
                    case 0x33:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            destDWord = Xor(destDWord, sourceDWord);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            destWord = Xor(destWord, sourceWord);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x34:
                        Xor((byte)operands[0]);
                        break;
                    case 0x35:
                        if (opSize == 32)
                            Xor((uint)operands[0]);
                        else
                            Xor((ushort)operands[0]);
                        break;
                    #endregion
                    #region Compare
                    case 0x38:
                        ProcessRegMem(rmData, out sourceByte, out destByte);
                        Subtract(destByte, sourceByte);
                        break;
                    case 0x39:
                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            Subtract(destDWord, sourceDWord);
                        }
                        else
                        {
                            ProcessRegMem(rmData, out sourceWord, out destWord);
                            Subtract(destWord, sourceWord);
                        }
                        break;
                    case 0x3a:
                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        Subtract(destByte, sourceByte);
                        break;
                    case 0x3b:
                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            Subtract(destDWord, sourceDWord);
                        }
                        else
                        {
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            Subtract(destWord, sourceWord);
                        }
                        break;
                    case 0x3c:
                        DoSub(AL, (byte)operands[0], false);
                        break;
                    case 0x3d:
                        if (opSize == 32)
                            DoSub(EAX, (uint)operands[0], false);
                        else
                            DoSub(AX, (ushort)operands[0], false);
                        break;
                    #endregion
                    #region Test
                    case 0x84:
                        ProcessRegMem(rmData, out sourceByte, out destByte);
                        And(destByte, sourceByte);
                        break;
                    case 0x85:
                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            And(destDWord, sourceDWord);
                        }
                        else
                        {
                            ProcessRegMem(rmData, out sourceWord, out destWord);
                            And(destWord, sourceWord);
                        }
                        break;
                    case 0xa8:
                        DoAnd(AL, (byte)operands[0]);
                        break;
                    case 0xa9:
                        if (opSize == 32)
                            DoAnd(EAX, (uint)operands[0]);
                        else
                            DoAnd(AX, (ushort)operands[0]);
                        break;
                    #endregion
                    #region Move
                    case 0x88:
                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        destByte = sourceByte;
                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0x89:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            destDWord = sourceDWord;
                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            destWord = sourceWord;
                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0x8a:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        ProcessRegMem(rmData, out destByte, out sourceByte);
                        destByte = sourceByte;
                        SetByteReg(rmData.Register, destByte);
                        break;
                    case 0x8b:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            destDWord = sourceDWord;
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            ProcessRegMem(rmData, out destWord, out sourceWord);
                            destWord = sourceWord;
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x8c:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        memAddress = ProcessRegMem(rmData, out destWord, out sourceWord);
                        WriteRegMem(rmData, memAddress, (ushort)segments[rmData.Register].Selector);
                        break;
                    case 0x8e:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        ProcessRegMem(rmData, out destWord, out sourceWord);
                        SetSelector((SegmentRegister)rmData.Register, sourceWord);
                        break;
                    case 0xa0:
                        if(memSize == 32)
                        {
                            sourceDWord = (uint)operands[0];
                            AL = SegReadByte(overrideSegment, sourceDWord);   
                        }
                        else
                        {
                            sourceWord = (ushort)operands[0];
                            AL = SegReadByte(overrideSegment, sourceWord);                            
                        }
                        break;
                    case 0xa1:
                        if (memSize == 32)
                        {
                            sourceDWord = (uint)operands[0];
                            EAX = SegReadDWord(overrideSegment, sourceDWord);
                        }
                        else
                        {
                            sourceWord = (ushort)operands[0];
                            AX = SegReadWord(overrideSegment, sourceWord);
                        }
                        break;
                    case 0xa2:
                        if (memSize == 32)
                        {
                            sourceDWord = (uint)operands[0];
                            SegWriteByte(overrideSegment, sourceDWord, AL);
                        }
                        else
                        {
                            sourceWord = (ushort)operands[0];
                            SegWriteByte(overrideSegment, sourceWord, AL);
                        }
                        break;
                    case 0xa3:
                        if (memSize == 32)
                        {
                            sourceDWord = (uint)operands[0];
                            SegWriteDWord(overrideSegment, sourceDWord, EAX);
                        }
                        else
                        {
                            sourceWord = (ushort)operands[0];
                            SegWriteWord(overrideSegment, sourceWord, AX);
                        }
                        break;
                    case 0xb0:
                    case 0xb1:
                    case 0xb2:
                    case 0xb3:
                    case 0xb4:
                    case 0xb5:
                    case 0xb6:
                    case 0xb7:
                        SetByteReg((byte)(opCode - 0xb0), (byte)operands[0]);
                        break;
                    case 0xb8:
                    case 0xb9:
                    case 0xba:
                    case 0xbb:
                    case 0xbc:
                    case 0xbd:
                    case 0xbe:
                    case 0xbf:
                        if (opSize == 32)
                            registers[opCode - 0xb8].DWord = (uint)operands[0];
                        else
                            registers[opCode - 0xb8].Word = (ushort)operands[0];
                        break;
                    case 0xc6:
                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        WriteRegMem(rmData, memAddress, (byte)operands[1]);
                        break;
                    case 0xc7:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            WriteRegMem(rmData, memAddress, (uint)operands[1]);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            WriteRegMem(rmData, memAddress, (ushort)operands[1]);
                        }
                        break;
                    #endregion
                    #region Exchange
                    case 0x86:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        memAddress = ProcessRegMem(rmData, out sourceByte, out destByte);
                        WriteRegMem(rmData, memAddress, sourceByte);
                        SetByteReg(rmData.Register, destByte);
                        break;
                    case 0x87:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            WriteRegMem(rmData, memAddress, sourceDWord);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            WriteRegMem(rmData, memAddress, sourceWord);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x90:
                        break;
                    case 0x91:
                    case 0x92:
                    case 0x93:
                    case 0x94:
                    case 0x95:
                    case 0x96:
                    case 0x97:
                        if (opSize == 32)
                        {
                            destDWord = registers[opCode - 0x90].DWord;
                            registers[opCode - 0x90].DWord = EAX;
                            EAX = destDWord;
                        }
                        else
                        {
                            destWord = registers[opCode - 0x90].Word;
                            registers[opCode - 0x90].Word = AX;
                            AX = destWord;
                        }
                        break;
                    #endregion
                    #region Call Procedure
                    case 0x9a:
                        CallProcedure((ushort)operands[1], (ushort)operands[0], true, false);
                        break;
                    case 0xe8:
                        CallProcedure(CS, offset, false, false);
                        break;
                    #endregion
                    #region BCD
                    case 0x27:
                        DecAdjustAfterAddition();
                        break;
                    case 0x37:
                        ASCIIAdjustAfterAdd();
                        break;
                    case 0x2f:
                        DecAdjustAfterSubtract();
                        break;
                    case 0x3f:
                        ASCIIAdjustAfterSubtract();
                        break;
                    case 0xd4:
                        ASCIIAdjustAfterMultiply((byte)operands[0]);
                        break;
                    case 0xd5:
                        ASCIIAdjustAfterDivide((byte)operands[0]);
                        break;
                    #endregion
                    #region Inc/Dec
                    case 0x40:
                    case 0x41:
                    case 0x42:
                    case 0x43:
                    case 0x44:
                    case 0x45:
                    case 0x46:
                    case 0x47:
                        if(opSize == 32)
                            registers[opCode - 0x40].DWord = Increment(registers[opCode - 0x40].DWord);
                        else
                            registers[opCode - 0x40].Word = Increment(registers[opCode - 0x40].Word);
                        break;
                    case 0x48:
                    case 0x49:
                    case 0x4a:
                    case 0x4b:
                    case 0x4c:
                    case 0x4d:
                    case 0x4e:
                    case 0x4f:
                        if(opSize == 32)
                            registers[opCode - 0x48].DWord = Decrement(registers[opCode - 0x48].DWord);
                        else
                            registers[opCode - 0x48].Word = Decrement(registers[opCode - 0x48].Word);
                        break;
                    #endregion
                    #region Multiply
                    case 0x69:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            tempDWord = (uint)operands[1];

                            destDWord = SignedMultiply(sourceDWord, tempDWord);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out destWord, out sourceWord);
                            tempWord = (ushort)operands[1];

                            destWord = SignedMultiply(sourceWord, tempWord);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    case 0x6b:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out destDWord, out sourceDWord);
                            sourceByte = (byte)operands[1];

                            destDWord = SignedMultiply(sourceDWord, sourceByte);
                            registers[rmData.Register].DWord = destDWord;
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out destWord, out sourceWord);
                            sourceByte = (byte)operands[1];

                            destWord = SignedMultiply(sourceWord, sourceByte);
                            registers[rmData.Register].Word = destWord;
                        }
                        break;
                    #endregion
                    #region String ops
                    case 0x6c:
                        StringInByte();
                        break;
                    case 0x6d:
                        if (opSize == 32)
                            StringInDWord();
                        else
                            StringInWord();
                        break;
                    case 0x6e:
                        StringOutByte();
                        break;
                    case 0x6f:
                        if (opSize == 32)
                            StringOutDWord();
                        else
                            StringOutWord();
                        break;
                    case 0xa4:
                        StringCopyByte();
                        break;
                    case 0xa5:
                        if (opSize == 32)
                            StringCopyDWord();
                        else
                            StringCopyWord();
                        break;
                    case 0xa6:
                        StringCompareByte();
                        break;
                    case 0xa7:
                        if(opSize == 32)
                            StringCompareWord();
                        else
                            StringCompareWord();
                        break;
                    case 0xaa:
                        StringWriteByte();
                        break;
                    case 0xab:
                        if(opSize == 32)
                            StringWriteDWord();
                        else
                            StringWriteWord();
                        break;
                    case 0xac:
                        StringReadByte();
                        break;
                    case 0xad:
                        if (opSize == 32)
                            StringReadDWord();
                        else
                            StringReadWord();
                        break;
                    case 0xae:
                        StringScanByte();
                        break;
                    case 0xaf:
                        if (opSize == 32)
                            StringScanDWord();
                        else
                            StringScanWord();
                        break;
                    #endregion
                    #region Jumps
                    case 0x70:
                        if (OF)
                            EIP = offset;
                        break;
                    case 0x71:
                        if (!OF)
                            EIP = offset;
                        break;
                    case 0x72:
                        if (CF)
                            EIP = offset;
                        break;
                    case 0x73:
                        if (!CF)
                            EIP = offset;
                        break;
                    case 0x74:
                        if (ZF)
                            EIP = offset;
                        break;
                    case 0x75:
                        if (!ZF)
                            EIP = offset;
                        break;
                    case 0x76:
                        if (CF || ZF)
                            EIP = offset;
                        break;
                    case 0x77:
                        if (!CF && !ZF)
                            EIP = offset;
                        break;
                    case 0x78:
                        if (SF)
                            EIP = offset;
                        break;
                    case 0x79:
                        if (!SF)
                            EIP = offset;
                        break;
                    case 0x7a:
                        if (PF)
                            EIP = offset;
                        break;
                    case 0x7b:
                        if (!PF)
                            EIP = offset;
                        break;
                    case 0x7c:
                        if (SF != OF)
                            EIP = offset;
                        break;
                    case 0x7d:
                        if (SF == OF)
                            EIP = offset;
                        break;
                    case 0x7e:
                        if (ZF || (SF != OF))
                            EIP = offset;
                        break;
                    case 0x7f:
                        if (!ZF && SF == OF)
                            EIP = offset;
                        break;
                    case 0xe3:
                        if (CX == 0)
                            EIP = offset;
                        break;
                    #endregion
                    #region Stack ops
                    case 0x50:
                    case 0x51:
                    case 0x52:
                    case 0x53:
                    case 0x54:
                    case 0x55:
                    case 0x56:
                    case 0x57:
                        if(opSize == 32)
                            StackPush(registers[opCode - 0x50].DWord);
                        else
                            StackPush(registers[opCode - 0x50].Word);
                        break;
                    case 0x58:
                    case 0x59:
                    case 0x5a:
                    case 0x5b:
                    case 0x5c:
                    case 0x5d:
                    case 0x5e:
                    case 0x5f:
                        if(opSize == 32)
                            registers[opCode - 0x58].DWord = StackPop();
                        else
                            registers[opCode - 0x58].Word = (ushort)StackPop();
                        break;
                    case 0x60:
                        if (opSize == 32)
                        {
                            tempDWord = SegReadDWord(SegmentRegister.SS, ESP);
                            StackPush(EAX);
                            StackPush(ECX);
                            StackPush(EDX);
                            StackPush(EBX);
                            StackPush(tempDWord);
                            StackPush(EBP);
                            StackPush(ESI);
                            StackPush(EDI);
                        }
                        else
                        {
                            tempWord = SegReadWord(SegmentRegister.SS, SP);
                            StackPush(AX);
                            StackPush(CX);
                            StackPush(DX);
                            StackPush(BX);
                            StackPush(tempWord);
                            StackPush(BP);
                            StackPush(SI);
                            StackPush(DI);
                        }
                        break;
                    case 0x61:
                        if (opSize == 32)
                        {
                            EDI = StackPop();
                            ESI = StackPop();
                            EBP = StackPop();
                            ESP += 4;
                            EBX = StackPop();
                            EDX = StackPop();
                            ECX = StackPop();
                            EAX = StackPop();
                        }
                        else
                        {
                            DI = (ushort)StackPop();
                            SI = (ushort)StackPop();
                            BP = (ushort)StackPop();
                            SP += 2;
                            BX = (ushort)StackPop();
                            DX = (ushort)StackPop();
                            CX = (ushort)StackPop();
                            AX = (ushort)StackPop();
                        }
                        break;
                    case 0x06:
                        StackPush(ES);
                        break;
                    case 0x07:
                        ES = (ushort)StackPop();
                        break;
                    case 0x16:
                        StackPush(SS);
                        break;
                    case 0x17:
                        SS = (ushort)StackPop();
                        break;
                    case 0x0e:
                        StackPush(CS);
                        break;
                    case 0x1e:
                        StackPush(DS);
                        break;
                    case 0x1f:
                        DS = (ushort)StackPop();
                        break;
                    case 0x6a:
                        StackPush((byte)operands[0]);
                        break;
                    case 0x68:
                        if (opSize == 32)
                            StackPush((uint)operands[0]);
                        else
                            StackPush((ushort)operands[0]);
                        break;
                    case 0x8f:
                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            sourceDWord = StackPop();
                            WriteRegMem(rmData, memAddress, sourceDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            sourceWord = (ushort)StackPop();
                            WriteRegMem(rmData, memAddress, sourceWord);
                        }
                        break;
                    case 0x9c:
                        if (opSize == 32)
                            StackPush((uint)eFlags);
                        else
                            StackPush((ushort)Flags);
                        break;
                    case 0x9d:
                        if (opSize == 32)
                            eFlags = (CPUFlags)StackPop();
                        else
                            eFlags = (CPUFlags)(ushort)StackPop();
                        break;
                    #endregion
                    #region Rotate/shift
                    case 0xc0:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        sourceByte = (byte)operands[1];

                        switch (rmData.Register)
                        {
                            case 0:
                                destByte = Rotate(destByte, sourceByte, RotateType.Left);
                                break;
                            case 1:
                                destByte = Rotate(destByte, sourceByte, RotateType.Right);
                                break;
                            case 2:
                                destByte = Rotate(destByte, sourceByte, RotateType.LeftWithCarry);
                                break;
                            case 3:
                                destByte = Rotate(destByte, sourceByte, RotateType.RightWithCarry);
                                break;
                            case 4:
                            case 6:
                                destByte = Shift(destByte, sourceByte, ShiftType.Left);
                                break;
                            case 5:
                                destByte = Shift(destByte, sourceByte, ShiftType.Right);
                                break;
                            case 7:
                                destByte = Shift(destByte, sourceByte, ShiftType.ArithmaticRight);
                                break;
                        }

                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0xc1:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out tempDWord, out destDWord);
                            sourceDWord = (byte)operands[1];

                            switch (rmData.Register)
                            {
                                case 0:
                                    destDWord = Rotate(destDWord, sourceDWord, RotateType.Left);
                                    break;
                                case 1:
                                    destDWord = Rotate(destDWord, sourceDWord, RotateType.Right);
                                    break;
                                case 2:
                                    destDWord = Rotate(destDWord, sourceDWord, RotateType.LeftWithCarry);
                                    break;
                                case 3:
                                    destDWord = Rotate(destDWord, sourceDWord, RotateType.RightWithCarry);
                                    break;
                                case 4:
                                case 6:
                                    destDWord = Shift(destDWord, sourceDWord, ShiftType.Left);
                                    break;
                                case 5:
                                    destDWord = Shift(destDWord, sourceDWord, ShiftType.Right);
                                    break;
                                case 7:
                                    destDWord = Shift(destDWord, sourceDWord, ShiftType.ArithmaticRight);
                                    break;
                            }

                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out tempWord, out destWord);
                            sourceWord = (byte)operands[1];

                            switch (rmData.Register)
                            {
                                case 0:
                                    destWord = Rotate(destWord, sourceWord, RotateType.Left);
                                    break;
                                case 1:
                                    destWord = Rotate(destWord, sourceWord, RotateType.Right);
                                    break;
                                case 2:
                                    destWord = Rotate(destWord, sourceWord, RotateType.LeftWithCarry);
                                    break;
                                case 3:
                                    destWord = Rotate(destWord, sourceWord, RotateType.RightWithCarry);
                                    break;
                                case 4:
                                case 6:
                                    destWord = Shift(destWord, sourceWord, ShiftType.Left);
                                    break;
                                case 5:
                                    destWord = Shift(destWord, sourceWord, ShiftType.Right);
                                    break;
                                case 7:
                                    destWord = Shift(destWord, sourceWord, ShiftType.ArithmaticRight);
                                    break;
                            }

                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0xd0:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);

                        switch (rmData.Register)
                        {
                            case 0:
                                destByte = Rotate(destByte, 1, RotateType.Left);
                                break;
                            case 1:
                                destByte = Rotate(destByte, 1, RotateType.Right);
                                break;
                            case 2:
                                destByte = Rotate(destByte, 1, RotateType.LeftWithCarry);
                                break;
                            case 3:
                                destByte = Rotate(destByte, 1, RotateType.RightWithCarry);
                                break;
                            case 4:
                            case 6:
                                destByte = Shift(destByte, 1, ShiftType.Left);
                                break;
                            case 5:
                                destByte = Shift(destByte, 1, ShiftType.Right);
                                break;
                            case 7:
                                destByte = Shift(destByte, 1, ShiftType.ArithmaticRight);
                                break;
                        }

                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0xd1:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out tempDWord, out destDWord);

                            switch (rmData.Register)
                            {
                                case 0:
                                    destDWord = Rotate(destDWord, 1, RotateType.Left);
                                    break;
                                case 1:
                                    destDWord = Rotate(destDWord, 1, RotateType.Right);
                                    break;
                                case 2:
                                    destDWord = Rotate(destDWord, 1, RotateType.LeftWithCarry);
                                    break;
                                case 3:
                                    destDWord = Rotate(destDWord, 1, RotateType.RightWithCarry);
                                    break;
                                case 4:
                                case 6:
                                    destDWord = Shift(destDWord, 1, ShiftType.Left);
                                    break;
                                case 5:
                                    destDWord = Shift(destDWord, 1, ShiftType.Right);
                                    break;
                                case 7:
                                    destDWord = Shift(destDWord, 1, ShiftType.ArithmaticRight);
                                    break;
                            }

                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out tempWord, out destWord);

                            switch (rmData.Register)
                            {
                                case 0:
                                    destWord = Rotate(destWord, 1, RotateType.Left);
                                    break;
                                case 1:
                                    destWord = Rotate(destWord, 1, RotateType.Right);
                                    break;
                                case 2:
                                    destWord = Rotate(destWord, 1, RotateType.LeftWithCarry);
                                    break;
                                case 3:
                                    destWord = Rotate(destWord, 1, RotateType.RightWithCarry);
                                    break;
                                case 4:
                                case 6:
                                    destWord = Shift(destWord, 1, ShiftType.Left);
                                    break;
                                case 5:
                                    destWord = Shift(destWord, 1, ShiftType.Right);
                                    break;
                                case 7:
                                    destWord = Shift(destWord, 1, ShiftType.ArithmaticRight);
                                    break;
                            }

                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0xd2:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);

                        switch (rmData.Register)
                        {
                            case 0:
                                destByte = Rotate(destByte, CL, RotateType.Left);
                                break;
                            case 1:
                                destByte = Rotate(destByte, CL, RotateType.Right);
                                break;
                            case 2:
                                destByte = Rotate(destByte, CL, RotateType.LeftWithCarry);
                                break;
                            case 3:
                                destByte = Rotate(destByte, CL, RotateType.RightWithCarry);
                                break;
                            case 4:
                            case 6:
                                destByte = Shift(destByte, CL, ShiftType.Left);
                                break;
                            case 5:
                                destByte = Shift(destByte, CL, ShiftType.Right);
                                break;
                            case 7:
                                destByte = Shift(destByte, CL, ShiftType.ArithmaticRight);
                                break;
                        }

                        WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0xd3:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out tempDWord, out destDWord);

                            switch (rmData.Register)
                            {
                                case 0:
                                    destDWord = Rotate(destDWord, CL, RotateType.Left);
                                    break;
                                case 1:
                                    destDWord = Rotate(destDWord, CL, RotateType.Right);
                                    break;
                                case 2:
                                    destDWord = Rotate(destDWord, CL, RotateType.LeftWithCarry);
                                    break;
                                case 3:
                                    destDWord = Rotate(destDWord, CL, RotateType.RightWithCarry);
                                    break;
                                case 4:
                                case 6:
                                    destDWord = Shift(destDWord, CL, ShiftType.Left);
                                    break;
                                case 5:
                                    destDWord = Shift(destDWord, CL, ShiftType.Right);
                                    break;
                                case 7:
                                    destDWord = Shift(destDWord, CL, ShiftType.ArithmaticRight);
                                    break;
                            }

                            WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out tempWord, out destWord);

                            switch (rmData.Register)
                            {
                                case 0:
                                    destWord = Rotate(destWord, CL, RotateType.Left);
                                    break;
                                case 1:
                                    destWord = Rotate(destWord, CL, RotateType.Right);
                                    break;
                                case 2:
                                    destWord = Rotate(destWord, CL, RotateType.LeftWithCarry);
                                    break;
                                case 3:
                                    destWord = Rotate(destWord, CL, RotateType.RightWithCarry);
                                    break;
                                case 4:
                                case 6:
                                    destWord = Shift(destWord, CL, ShiftType.Left);
                                    break;
                                case 5:
                                    destWord = Shift(destWord, CL, ShiftType.Right);
                                    break;
                                case 7:
                                    destWord = Shift(destWord, CL, ShiftType.ArithmaticRight);
                                    break;
                            }

                            WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    #endregion
                    #region Misc
                    case 0x8d:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            registers[rmData.Register].DWord = memAddress;
                        
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            registers[rmData.Register].Word = (ushort)memAddress;
                        }
                        break;
                    case 0x9e:
                        eFlags |= (CPUFlags)0x02;
                        CF = (AH & 0x1) == 0x1;
                        PF = (AH & 0x4) == 0x4;
                        AF = (AH & 0x10) == 0x10;
                        ZF = (AH & 0x40) == 0x40;
                        SF = (AH & 0x80) == 0x80;
                        break;
                    case 0x9f:
                        AH = 0x02;
                        AH |= (byte)((byte)eFlags & 0x1);
                        AH |= (byte)((byte)eFlags & 0x4);
                        AH |= (byte)((byte)eFlags & 0x10);
                        AH |= (byte)((byte)eFlags & 0x40);
                        AH |= (byte)((byte)eFlags & 0x80);
                        break;
                    case 0xc2:
                        if (opSize == 32)
                            EIP = StackPop();
                        else
                            EIP = (ushort)StackPop();
                        SP += (ushort)operands[0];
                        break;
                    case 0xc3:
                        if (opSize == 32)
                            EIP = StackPop();
                        else
                            EIP = (ushort)StackPop();
                        break;
                    case 0xc4:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            registers[rmData.Register].DWord = destDWord;
                            ES = ReadRegMemWord(rmData, memAddress + 2);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            registers[rmData.Register].Word = destWord;
                            ES = ReadRegMemWord(rmData, memAddress + 2);
                        }
                        break;
                    case 0xc5:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out sourceDWord, out destDWord);
                            registers[rmData.Register].DWord = destDWord;
                            DS = ReadRegMemWord(rmData, memAddress + 2);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out sourceWord, out destWord);
                            registers[rmData.Register].Word = destWord;
                            DS = ReadRegMemWord(rmData, memAddress + 2);
                        }
                        break;
                    case 0xc9:
                        ProcedureLeave();
                        break;
                    case 0xca:
                        if (opSize == 32)
                            EIP = StackPop();
                        else
                            EIP = (ushort)StackPop();
                        CS = (ushort)StackPop();
                        SP += (ushort)operands[0];
                        break;
                    case 0xcb:
                        if (opSize == 32)
                            EIP = StackPop();
                        else
                            EIP = (ushort)StackPop();
                        CS = (ushort)StackPop();
                        break;
                    case 0xcc:
                        CallInterrupt(3);
                        break;
                    case 0xcd:
                        CallInterrupt((byte)operands[0]);
                        break;
                    case 0xce:
                        if (OF)
                            CallInterrupt(4);
                        break;
                    case 0xcf:
                        if (opSize == 32)
                            EIP = StackPop();
                        else
                            EIP = (ushort)StackPop();
                        CS = (ushort)StackPop();
                        eFlags = (CPUFlags)StackPop();
                        IF = true;
                        if(InterruptLevel > 0)
                            InterruptLevel--;
//                        Logger.Debug("IRET");
                        break;
                    case 0xd7:
                        if (memSize == 32)
                            AL = SegReadByte(overrideSegment, EBX + AL);
                        else
                            AL = SegReadByte(overrideSegment, (uint)(BX + AL));
                        break;
                    case 0xe0:
                        CX--;
                        if (!ZF && CX != 0)
                            EIP = offset;
                        break;
                    case 0xe1:
                        CX--;
                        if (ZF && CX != 0)
                            EIP = offset;
                        break;
                    case 0xe2:
                        CX--;
                        if (CX != 0)
                            EIP = offset;
                        break;
                    case 0xe4:
                        AL = DoIORead((byte)operands[0]);
                        break;
                    case 0xe5:
                        if(opSize == 32)
                            EAX = DoIORead((ushort)(byte)operands[0]);
                        else
                            AX = DoIORead((ushort)(byte)operands[0]);
                        break;
                    case 0xe6:
                        DoIOWrite((byte)operands[0], AL);
                        break;
                    case 0xe7:
                        if(opSize == 32)
                            DoIOWrite((byte)operands[0], (ushort)EAX);
                        else
                            DoIOWrite((byte)operands[0], AX);
                        break;
                    case 0xea:
                        if (opSize == 32)
                            DoJump((ushort)operands[1], (uint)operands[0], false);
                        else
                            DoJump((ushort)operands[1], (ushort)operands[0], false);
                        break;
                    case 0xe9:
                    case 0xeb:
                        EIP = offset;
                        break;
                    case 0xec:
                        AL = (byte)DoIORead(DX);
                        break;
                    case 0xed:
                        if (opSize == 32)
                            EAX = DoIORead(DX);
                        else
                            AX = DoIORead(DX);
                        break;
                    case 0xee:
                        DoIOWrite(DX, AL);
                        break;
                    case 0xef:
                        if(opSize == 32)
                            DoIOWrite(DX, (ushort)EAX);
                        else
                            DoIOWrite(DX, AX);
                        break;
                    case 0x98:
                        if(opSize == 32)
                            EAX = (uint)(short)AX;
                        else
                            AX = (ushort)(sbyte)AL;
                        break;
                    case 0x99:
                        if (opSize == 32)
                        {
                            tempDWord = (uint)(short)EAX;
                            DX = (ushort)(tempDWord >> 16);
                        }
                        else
                        {
                            tempQWord = (ulong)(int)EAX;
                            EDX = (uint)(tempQWord >> 16);
                        }
                        break;
                    case 0xc8:
                        destWord = (ushort)operands[0];
                        sourceByte = (byte)operands[1];

                        ProcedureEnter(destWord, sourceByte);
                        break;
                    case 0xf4:
                        if(!IF)
                            throw new Exception("HLT with interrupt disabled");
                        Halted = true;
                        break;
                    case 0xf5:
                        CF = !CF;
                        break;
                    case 0xf8:
                        CF = false;
                        break;
                    case 0xf9:
                        CF = true;
                        break;
                    case 0xfb:
                        IF = true;
                        break;
                    case 0xfc:
                        DF = false;
                        break;
                    case 0xfd:
                        DF = true;
                        break;
                    case 0xfa:
                        IF = false;
                        break;
                    #endregion
                    #region Groups
                    case 0x80:
                    case 0x82:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);
                        sourceByte = (byte)operands[1];

                        switch (rmData.Register)
                        {
                            case 0:
                                destByte = Add(destByte, sourceByte);
                                break;
                            case 1:
                                destByte = Or(destByte, sourceByte);
                                break;
                            case 2:
                                destByte = AddWithCarry(destByte, sourceByte);
                                break;
                            case 3:
                                destByte = SubWithBorrow(destByte, sourceByte);
                                break;
                            case 4:
                                destByte = And(destByte, sourceByte);
                                break;
                            case 5:
                                destByte = Subtract(destByte, sourceByte);
                                break;
                            case 6:
                                destByte = Xor(destByte, sourceByte);
                                break;
                            case 7:
                                Subtract(destByte, sourceByte);
                                break;
                        }

                        if(rmData.Register != 7)
                            WriteRegMem(rmData, memAddress, destByte);
                        break;
                    case 0x81:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out tempDWord, out destDWord);
                            sourceDWord = (uint)operands[1];

                            switch (rmData.Register)
                            {
                                case 0:
                                    destDWord = Add(destDWord, sourceDWord);
                                    break;
                                case 1:
                                    destDWord = Or(destDWord, sourceDWord);
                                    break;
                                case 2:
                                    destDWord = AddWithCarry(destDWord, sourceDWord);
                                    break;
                                case 3:
                                    destDWord = SubWithBorrow(destDWord, sourceDWord);
                                    break;
                                case 4:
                                    destDWord = And(destDWord, sourceDWord);
                                    break;
                                case 5:
                                    destDWord = Subtract(destDWord, sourceDWord);
                                    break;
                                case 6:
                                    destDWord = Xor(destDWord, sourceDWord);
                                    break;
                                case 7:
                                    Subtract(destDWord, sourceDWord);
                                    break;
                            }

                            if (rmData.Register != 7)
                                WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out tempWord, out destWord);
                            sourceWord = (ushort)operands[1];

                            switch (rmData.Register)
                            {
                                case 0:
                                    destWord = Add(destWord, sourceWord);
                                    break;
                                case 1:
                                    destWord = Or(destWord, sourceWord);
                                    break;
                                case 2:
                                    destWord = AddWithCarry(destWord, sourceWord);
                                    break;
                                case 3:
                                    destWord = SubWithBorrow(destWord, sourceWord);
                                    break;
                                case 4:
                                    destWord = And(destWord, sourceWord);
                                    break;
                                case 5:
                                    destWord = Subtract(destWord, sourceWord);
                                    break;
                                case 6:
                                    destWord = Xor(destWord, sourceWord);
                                    break;
                                case 7:
                                    Subtract(destWord, sourceWord);
                                    break;
                            }

                            if(rmData.Register != 7)
                                WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0x83:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out tempDWord, out destDWord);
                            sourceByte = (byte)operands[1];

                            switch (rmData.Register)
                            {
                                case 0:
                                    destDWord = Add(destDWord, sourceByte);
                                    break;
                                case 1:
                                    destDWord = Or(destDWord, sourceByte);
                                    break;
                                case 2:
                                    destDWord = AddWithCarry(destDWord, sourceByte);
                                    break;
                                case 3:
                                    destDWord = SubWithBorrow(destDWord, sourceByte);
                                    break;
                                case 4:
                                    destDWord = And(destDWord, sourceByte);
                                    break;
                                case 5:
                                    destDWord = Subtract(destDWord, sourceByte);
                                    break;
                                case 6:
                                    destDWord = Xor(destDWord, sourceByte);
                                    break;
                                case 7:  
                                    Subtract(destDWord, sourceByte);
                                    break;
                            }

                            if (rmData.Register != 7)
                                WriteRegMem(rmData, memAddress, destDWord);
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out tempWord, out destWord);
                            sourceByte = (byte)operands[1];

                            switch (rmData.Register)
                            {
                                case 0:
                                    destWord = Add(destWord, sourceByte);
                                    break;
                                case 1:
                                    destWord = Or(destWord, sourceByte);
                                    break;
                                case 2:
                                    destWord = AddWithCarry(destWord, sourceByte);
                                    break;
                                case 3:
                                    destWord = SubWithBorrow(destWord, sourceByte);
                                    break;
                                case 4:
                                    destWord = And(destWord, sourceByte);
                                    break;
                                case 5:
                                    destWord = Subtract(destWord, sourceByte);
                                    break;
                                case 6:
                                    destWord = Xor(destWord, sourceByte);
                                    break;
                                case 7:
                                    Subtract(destWord, sourceByte);
                                    break;
                            }

                            if(rmData.Register != 7)
                                WriteRegMem(rmData, memAddress, destWord);
                        }
                        break;
                    case 0xf6:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        memAddress = ProcessRegMem(rmData, out tempByte, out sourceByte);

                        switch (rmData.Register)
                        {
                            case 0:
                                And(sourceByte, (byte)operands[1]);
                                break;
                            case 2:
                                sourceByte = (byte)~sourceByte;
                                WriteRegMem(rmData, memAddress, sourceByte);

                                break;
                            case 3:
                                CF = sourceByte != 0;

                                sourceByte = (byte)-sourceByte;
                                WriteRegMem(rmData, memAddress, sourceByte);
                                break;
                            case 4:
                                Multiply(sourceByte);
                                break;
                            case 5:
                                SignedMultiply(sourceByte);
                                break;
                            case 6:
                                Divide(sourceByte);
                                break;
                            case 7:
                                SDivide(sourceByte);
                                break;
                        }
                        break;
                    case 0xf7:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out tempDWord, out sourceDWord);

                            switch (rmData.Register)
                            {
                                case 0:
                                case 1:
                                    And(sourceDWord, (uint)operands[1]);
                                    break;
                                case 2:
                                    sourceDWord = ~sourceDWord;
                                    WriteRegMem(rmData, memAddress, sourceDWord);

                                    break;
                                case 3:
                                    CF = sourceDWord != 0;

                                    sourceDWord = (uint)-sourceDWord;
                                    WriteRegMem(rmData, memAddress, sourceDWord);
                                    break;
                                case 4:
                                    Multiply(sourceDWord);
                                    break;
                                case 5:
                                    SignedMultiply(sourceDWord);
                                    break;
                                case 6:
                                    Divide(sourceDWord);
                                    break;
                                case 7:
                                    SDivide(sourceDWord);
                                    break;
                            }
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out tempWord, out sourceWord);

                            switch (rmData.Register)
                            {
                                case 0:
                                case 1:
                                    And(sourceWord, (ushort)operands[1]);
                                    break;
                                case 2:
                                    sourceWord = (ushort)~sourceWord;
                                    WriteRegMem(rmData, memAddress, sourceWord);

                                    break;
                                case 3:
                                    CF = sourceWord != 0;

                                    sourceWord = (ushort)-sourceWord;
                                    WriteRegMem(rmData, memAddress, sourceWord);
                                    break;
                                case 4:
                                    Multiply(sourceWord);
                                    break;
                                case 5:
                                    SignedMultiply(sourceWord);
                                    break;
                                case 6:
                                    Divide(sourceWord);
                                    break;
                                case 7:
                                    SDivide(sourceWord);
                                    break;
                            }
                        }
                        break;
                    case 0xfe:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        memAddress = ProcessRegMem(rmData, out tempByte, out destByte);

                        switch (rmData.Register)
                        {
                            case 0:
                                destByte = Increment(destByte);
                                WriteRegMem(rmData, memAddress, destByte);
                                break;
                            case 1:
                                destByte = Decrement(destByte);
                                WriteRegMem(rmData, memAddress, destByte);
                                break;
                        }
                        break;
                    case 0xff:
                        System.Diagnostics.Debug.Assert(rmData != null, "rmData != null");

                        if (opSize == 32)
                        {
                            memAddress = ProcessRegMem(rmData, out tempDWord, out destDWord);

                            switch (rmData.Register)
                            {
                                case 0:
                                    destDWord = Increment(destDWord);
                                    WriteRegMem(rmData, memAddress, destDWord);
                                    break;
                                case 1:
                                    destDWord = Decrement(destDWord);
                                    WriteRegMem(rmData, memAddress, destDWord);
                                    break;
                                case 2:
                                    CallRegMem(rmData, memAddress, false, true);
                                    break;
                                case 3:
                                    CallRegMem(rmData, memAddress, true, true);
                                    break;
                                case 4:
                                    CallRegMem(rmData, memAddress, false, false);
                                    break;
                                case 5:
                                    CallRegMem(rmData, memAddress, true, false);
                                    break;
                                case 6:
                                    StackPush(destDWord);
                                    break;
                            }
                        }
                        else
                        {
                            memAddress = ProcessRegMem(rmData, out tempWord, out destWord);

                            switch (rmData.Register)
                            {
                                case 0:
                                    destWord = Increment(destWord);
                                    WriteRegMem(rmData, memAddress, destWord);
                                    break;
                                case 1:
                                    destWord = Decrement(destWord);
                                    WriteRegMem(rmData, memAddress, destWord);
                                    break;
                                case 2:
                                    CallRegMem(rmData, memAddress, false, true);
                                    break;
                                case 3:
                                    CallRegMem(rmData, memAddress, true, true);
                                    break;
                                case 4:
                                    CallRegMem(rmData, memAddress, false, false);
                                    break;
                                case 5:
                                    CallRegMem(rmData, memAddress, true, false);
                                    break;
                                case 6:
                                    StackPush(destWord);
                                    break;
                            }
                        }
                        break;
                    default:
                        System.Diagnostics.Debugger.Break();
                        break;

                        #endregion
                }
            }*/
            #endregion

            switch ((BeaConstants.InstructionType)(currentInstruction.Instruction.Category & 0x0000FFFF))
            {
                case BeaConstants.InstructionType.CONTROL_TRANSFER:
                    ProcessControlTransfer();
                    break;
                case BeaConstants.InstructionType.LOGICAL_INSTRUCTION:
                    ProcessLogic(operands);
                    break;
                case BeaConstants.InstructionType.InOutINSTRUCTION:
                    ProcessInputOutput(operands);
                    break;
                case BeaConstants.InstructionType.DATA_TRANSFER:
                    ProcessDataTransfer(operands);
                    break;
                case BeaConstants.InstructionType.ARITHMETIC_INSTRUCTION:
                    ProcessArithmetic(operands);
                    break;
                case BeaConstants.InstructionType.FLAG_CONTROL_INSTRUCTION:
                    ProcessFlagControl(operands);
                    break;
                case BeaConstants.InstructionType.STRING_INSTRUCTION:
                    ProcessString(operands);
                    break;
                default:
                    break;
            }

            CurrentAddr = segments[(int)SegmentRegister.CS].GDTEntry.BaseAddress + EIP;
        }
    }
}
