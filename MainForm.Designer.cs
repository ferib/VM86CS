﻿namespace x86CS
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floppyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registersGroup = new System.Windows.Forms.GroupBox();
            this.EBP = new System.Windows.Forms.TextBox();
            this.ebpLabel = new System.Windows.Forms.Label();
            this.ESP = new System.Windows.Forms.TextBox();
            this.ebxLabel = new System.Windows.Forms.Label();
            this.esiLabel = new System.Windows.Forms.Label();
            this.espLabel = new System.Windows.Forms.Label();
            this.EBX = new System.Windows.Forms.TextBox();
            this.ESI = new System.Windows.Forms.TextBox();
            this.ediLabel = new System.Windows.Forms.Label();
            this.EDI = new System.Windows.Forms.TextBox();
            this.edxLabel = new System.Windows.Forms.Label();
            this.ecxLabel = new System.Windows.Forms.Label();
            this.eaxLabel = new System.Windows.Forms.Label();
            this.EDX = new System.Windows.Forms.TextBox();
            this.ECX = new System.Windows.Forms.TextBox();
            this.EAX = new System.Windows.Forms.TextBox();
            this.segmentGroup = new System.Windows.Forms.GroupBox();
            this.ssLabel = new System.Windows.Forms.Label();
            this.SS = new System.Windows.Forms.TextBox();
            this.gsLabel = new System.Windows.Forms.Label();
            this.GS = new System.Windows.Forms.TextBox();
            this.fsLabel = new System.Windows.Forms.Label();
            this.FS = new System.Windows.Forms.TextBox();
            this.esLabel = new System.Windows.Forms.Label();
            this.ES = new System.Windows.Forms.TextBox();
            this.dsLabel = new System.Windows.Forms.Label();
            this.DS = new System.Windows.Forms.TextBox();
            this.csLabel = new System.Windows.Forms.Label();
            this.CS = new System.Windows.Forms.TextBox();
            this.flagsGroup = new System.Windows.Forms.GroupBox();
            this.VIP = new System.Windows.Forms.TextBox();
            this.VIF = new System.Windows.Forms.TextBox();
            this.AC = new System.Windows.Forms.TextBox();
            this.VM = new System.Windows.Forms.TextBox();
            this.RF = new System.Windows.Forms.TextBox();
            this.NT = new System.Windows.Forms.TextBox();
            this.IOPL = new System.Windows.Forms.TextBox();
            this.OF = new System.Windows.Forms.TextBox();
            this.DF = new System.Windows.Forms.TextBox();
            this.IF = new System.Windows.Forms.TextBox();
            this.TF = new System.Windows.Forms.TextBox();
            this.SF = new System.Windows.Forms.TextBox();
            this.ZF = new System.Windows.Forms.TextBox();
            this.AF = new System.Windows.Forms.TextBox();
            this.PF = new System.Windows.Forms.TextBox();
            this.CF = new System.Windows.Forms.TextBox();
            this.floppyOpen = new System.Windows.Forms.OpenFileDialog();
            this.stepButton = new System.Windows.Forms.Button();
            this.goButton = new System.Windows.Forms.Button();
            this.cpuGroup = new System.Windows.Forms.GroupBox();
            this.stepOverButton = new System.Windows.Forms.Button();
            this.cpuLabel = new System.Windows.Forms.Label();
            this.memoryGroup = new System.Windows.Forms.GroupBox();
            this.memoryCharList = new System.Windows.Forms.ListBox();
            this.memoryChar = new System.Windows.Forms.TextBox();
            this.memoryByte = new System.Windows.Forms.TextBox();
            this.memoryWord = new System.Windows.Forms.TextBox();
            this.memoryDWord = new System.Windows.Forms.TextBox();
            this.memoryList = new System.Windows.Forms.ListBox();
            this.memOffset = new System.Windows.Forms.TextBox();
            this.memoryButton = new System.Windows.Forms.Button();
            this.mainStatus = new System.Windows.Forms.StatusStrip();
            this.tpsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.stackGroup = new System.Windows.Forms.GroupBox();
            this.baseList = new System.Windows.Forms.ListBox();
            this.stackList = new System.Windows.Forms.ListBox();
            this.memoryDisasmList = new System.Windows.Forms.ListBox();
            this.mainMenu.SuspendLayout();
            this.registersGroup.SuspendLayout();
            this.segmentGroup.SuspendLayout();
            this.flagsGroup.SuspendLayout();
            this.cpuGroup.SuspendLayout();
            this.memoryGroup.SuspendLayout();
            this.mainStatus.SuspendLayout();
            this.stackGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.floppyToolStripMenuItem,
            this.configToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(656, 24);
            this.mainMenu.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.runToolStripMenuItem.Text = "&Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.RunToolStripMenuItemClick);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Enabled = false;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.stopToolStripMenuItem.Text = "&Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.StopToolStripMenuItemClick);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.restartToolStripMenuItem.Text = "R&estart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.RestartToolStripMenuItemClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // floppyToolStripMenuItem
            // 
            this.floppyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mountToolStripMenuItem});
            this.floppyToolStripMenuItem.Name = "floppyToolStripMenuItem";
            this.floppyToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.floppyToolStripMenuItem.Text = "F&loppy";
            // 
            // mountToolStripMenuItem
            // 
            this.mountToolStripMenuItem.Name = "mountToolStripMenuItem";
            this.mountToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.mountToolStripMenuItem.Text = "&Mount";
            this.mountToolStripMenuItem.Click += new System.EventHandler(this.MountToolStripMenuItemClick);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.configToolStripMenuItem.Text = "&Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.breakpointsToolStripMenuItem,
            this.loggingToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "&Debug";
            // 
            // breakpointsToolStripMenuItem
            // 
            this.breakpointsToolStripMenuItem.Name = "breakpointsToolStripMenuItem";
            this.breakpointsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.breakpointsToolStripMenuItem.Text = "&Breakpoints";
            this.breakpointsToolStripMenuItem.Click += new System.EventHandler(this.BreakpointsToolStripMenuItemClick);
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.CheckOnClick = true;
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            this.loggingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.loggingToolStripMenuItem.Text = "&Logging";
            // 
            // registersGroup
            // 
            this.registersGroup.Controls.Add(this.EBP);
            this.registersGroup.Controls.Add(this.ebpLabel);
            this.registersGroup.Controls.Add(this.ESP);
            this.registersGroup.Controls.Add(this.ebxLabel);
            this.registersGroup.Controls.Add(this.esiLabel);
            this.registersGroup.Controls.Add(this.espLabel);
            this.registersGroup.Controls.Add(this.EBX);
            this.registersGroup.Controls.Add(this.ESI);
            this.registersGroup.Controls.Add(this.ediLabel);
            this.registersGroup.Controls.Add(this.EDI);
            this.registersGroup.Controls.Add(this.edxLabel);
            this.registersGroup.Controls.Add(this.ecxLabel);
            this.registersGroup.Controls.Add(this.eaxLabel);
            this.registersGroup.Controls.Add(this.EDX);
            this.registersGroup.Controls.Add(this.ECX);
            this.registersGroup.Controls.Add(this.EAX);
            this.registersGroup.Location = new System.Drawing.Point(0, 27);
            this.registersGroup.Name = "registersGroup";
            this.registersGroup.Size = new System.Drawing.Size(205, 128);
            this.registersGroup.TabIndex = 2;
            this.registersGroup.TabStop = false;
            this.registersGroup.Text = "Registers:";
            // 
            // EBP
            // 
            this.EBP.Location = new System.Drawing.Point(131, 71);
            this.EBP.Name = "EBP";
            this.EBP.ReadOnly = true;
            this.EBP.Size = new System.Drawing.Size(70, 20);
            this.EBP.TabIndex = 12;
            // 
            // ebpLabel
            // 
            this.ebpLabel.AutoSize = true;
            this.ebpLabel.Location = new System.Drawing.Point(104, 74);
            this.ebpLabel.Name = "ebpLabel";
            this.ebpLabel.Size = new System.Drawing.Size(28, 13);
            this.ebpLabel.TabIndex = 17;
            this.ebpLabel.Text = "EBP";
            // 
            // ESP
            // 
            this.ESP.Location = new System.Drawing.Point(33, 71);
            this.ESP.Name = "ESP";
            this.ESP.ReadOnly = true;
            this.ESP.Size = new System.Drawing.Size(70, 20);
            this.ESP.TabIndex = 13;
            // 
            // ebxLabel
            // 
            this.ebxLabel.AutoSize = true;
            this.ebxLabel.Location = new System.Drawing.Point(103, 47);
            this.ebxLabel.Name = "ebxLabel";
            this.ebxLabel.Size = new System.Drawing.Size(28, 13);
            this.ebxLabel.TabIndex = 8;
            this.ebxLabel.Text = "EBX";
            // 
            // esiLabel
            // 
            this.esiLabel.AutoSize = true;
            this.esiLabel.Location = new System.Drawing.Point(6, 102);
            this.esiLabel.Name = "esiLabel";
            this.esiLabel.Size = new System.Drawing.Size(24, 13);
            this.esiLabel.TabIndex = 15;
            this.esiLabel.Text = "ESI";
            // 
            // espLabel
            // 
            this.espLabel.AutoSize = true;
            this.espLabel.Location = new System.Drawing.Point(2, 74);
            this.espLabel.Name = "espLabel";
            this.espLabel.Size = new System.Drawing.Size(28, 13);
            this.espLabel.TabIndex = 18;
            this.espLabel.Text = "ESP";
            // 
            // EBX
            // 
            this.EBX.Location = new System.Drawing.Point(131, 44);
            this.EBX.Name = "EBX";
            this.EBX.ReadOnly = true;
            this.EBX.Size = new System.Drawing.Size(70, 20);
            this.EBX.TabIndex = 6;
            // 
            // ESI
            // 
            this.ESI.Location = new System.Drawing.Point(33, 99);
            this.ESI.Name = "ESI";
            this.ESI.ReadOnly = true;
            this.ESI.Size = new System.Drawing.Size(70, 20);
            this.ESI.TabIndex = 11;
            // 
            // ediLabel
            // 
            this.ediLabel.AutoSize = true;
            this.ediLabel.Location = new System.Drawing.Point(106, 102);
            this.ediLabel.Name = "ediLabel";
            this.ediLabel.Size = new System.Drawing.Size(25, 13);
            this.ediLabel.TabIndex = 16;
            this.ediLabel.Text = "EDI";
            // 
            // EDI
            // 
            this.EDI.Location = new System.Drawing.Point(131, 99);
            this.EDI.Name = "EDI";
            this.EDI.ReadOnly = true;
            this.EDI.Size = new System.Drawing.Size(70, 20);
            this.EDI.TabIndex = 14;
            // 
            // edxLabel
            // 
            this.edxLabel.AutoSize = true;
            this.edxLabel.Location = new System.Drawing.Point(1, 47);
            this.edxLabel.Name = "edxLabel";
            this.edxLabel.Size = new System.Drawing.Size(29, 13);
            this.edxLabel.TabIndex = 10;
            this.edxLabel.Text = "EDX";
            // 
            // ecxLabel
            // 
            this.ecxLabel.AutoSize = true;
            this.ecxLabel.Location = new System.Drawing.Point(103, 20);
            this.ecxLabel.Name = "ecxLabel";
            this.ecxLabel.Size = new System.Drawing.Size(28, 13);
            this.ecxLabel.TabIndex = 9;
            this.ecxLabel.Text = "ECX";
            // 
            // eaxLabel
            // 
            this.eaxLabel.AutoSize = true;
            this.eaxLabel.Location = new System.Drawing.Point(2, 20);
            this.eaxLabel.Name = "eaxLabel";
            this.eaxLabel.Size = new System.Drawing.Size(28, 13);
            this.eaxLabel.TabIndex = 7;
            this.eaxLabel.Text = "EAX";
            // 
            // EDX
            // 
            this.EDX.Location = new System.Drawing.Point(33, 44);
            this.EDX.Name = "EDX";
            this.EDX.ReadOnly = true;
            this.EDX.Size = new System.Drawing.Size(70, 20);
            this.EDX.TabIndex = 5;
            // 
            // ECX
            // 
            this.ECX.Location = new System.Drawing.Point(131, 16);
            this.ECX.Name = "ECX";
            this.ECX.ReadOnly = true;
            this.ECX.Size = new System.Drawing.Size(70, 20);
            this.ECX.TabIndex = 4;
            // 
            // EAX
            // 
            this.EAX.Location = new System.Drawing.Point(33, 16);
            this.EAX.Name = "EAX";
            this.EAX.ReadOnly = true;
            this.EAX.Size = new System.Drawing.Size(70, 20);
            this.EAX.TabIndex = 3;
            // 
            // segmentGroup
            // 
            this.segmentGroup.Controls.Add(this.ssLabel);
            this.segmentGroup.Controls.Add(this.SS);
            this.segmentGroup.Controls.Add(this.gsLabel);
            this.segmentGroup.Controls.Add(this.GS);
            this.segmentGroup.Controls.Add(this.fsLabel);
            this.segmentGroup.Controls.Add(this.FS);
            this.segmentGroup.Controls.Add(this.esLabel);
            this.segmentGroup.Controls.Add(this.ES);
            this.segmentGroup.Controls.Add(this.dsLabel);
            this.segmentGroup.Controls.Add(this.DS);
            this.segmentGroup.Controls.Add(this.csLabel);
            this.segmentGroup.Controls.Add(this.CS);
            this.segmentGroup.Location = new System.Drawing.Point(211, 27);
            this.segmentGroup.Name = "segmentGroup";
            this.segmentGroup.Size = new System.Drawing.Size(205, 59);
            this.segmentGroup.TabIndex = 3;
            this.segmentGroup.TabStop = false;
            this.segmentGroup.Text = "Segments:";
            // 
            // ssLabel
            // 
            this.ssLabel.AutoSize = true;
            this.ssLabel.Location = new System.Drawing.Point(133, 35);
            this.ssLabel.Name = "ssLabel";
            this.ssLabel.Size = new System.Drawing.Size(21, 13);
            this.ssLabel.TabIndex = 11;
            this.ssLabel.Text = "SS";
            // 
            // SS
            // 
            this.SS.Location = new System.Drawing.Point(154, 32);
            this.SS.Name = "SS";
            this.SS.ReadOnly = true;
            this.SS.Size = new System.Drawing.Size(34, 20);
            this.SS.TabIndex = 10;
            // 
            // gsLabel
            // 
            this.gsLabel.AutoSize = true;
            this.gsLabel.Location = new System.Drawing.Point(75, 35);
            this.gsLabel.Name = "gsLabel";
            this.gsLabel.Size = new System.Drawing.Size(22, 13);
            this.gsLabel.TabIndex = 9;
            this.gsLabel.Text = "GS";
            // 
            // GS
            // 
            this.GS.Location = new System.Drawing.Point(97, 32);
            this.GS.Name = "GS";
            this.GS.ReadOnly = true;
            this.GS.Size = new System.Drawing.Size(34, 20);
            this.GS.TabIndex = 8;
            // 
            // fsLabel
            // 
            this.fsLabel.AutoSize = true;
            this.fsLabel.Location = new System.Drawing.Point(17, 35);
            this.fsLabel.Name = "fsLabel";
            this.fsLabel.Size = new System.Drawing.Size(20, 13);
            this.fsLabel.TabIndex = 7;
            this.fsLabel.Text = "FS";
            // 
            // FS
            // 
            this.FS.Location = new System.Drawing.Point(37, 32);
            this.FS.Name = "FS";
            this.FS.ReadOnly = true;
            this.FS.Size = new System.Drawing.Size(34, 20);
            this.FS.TabIndex = 6;
            // 
            // esLabel
            // 
            this.esLabel.AutoSize = true;
            this.esLabel.Location = new System.Drawing.Point(133, 16);
            this.esLabel.Name = "esLabel";
            this.esLabel.Size = new System.Drawing.Size(21, 13);
            this.esLabel.TabIndex = 5;
            this.esLabel.Text = "ES";
            // 
            // ES
            // 
            this.ES.Location = new System.Drawing.Point(154, 13);
            this.ES.Name = "ES";
            this.ES.ReadOnly = true;
            this.ES.Size = new System.Drawing.Size(34, 20);
            this.ES.TabIndex = 4;
            // 
            // dsLabel
            // 
            this.dsLabel.AutoSize = true;
            this.dsLabel.Location = new System.Drawing.Point(75, 16);
            this.dsLabel.Name = "dsLabel";
            this.dsLabel.Size = new System.Drawing.Size(22, 13);
            this.dsLabel.TabIndex = 3;
            this.dsLabel.Text = "DS";
            // 
            // DS
            // 
            this.DS.Location = new System.Drawing.Point(97, 13);
            this.DS.Name = "DS";
            this.DS.ReadOnly = true;
            this.DS.Size = new System.Drawing.Size(34, 20);
            this.DS.TabIndex = 2;
            // 
            // csLabel
            // 
            this.csLabel.AutoSize = true;
            this.csLabel.Location = new System.Drawing.Point(16, 16);
            this.csLabel.Name = "csLabel";
            this.csLabel.Size = new System.Drawing.Size(21, 13);
            this.csLabel.TabIndex = 1;
            this.csLabel.Text = "CS";
            // 
            // CS
            // 
            this.CS.Location = new System.Drawing.Point(37, 13);
            this.CS.Name = "CS";
            this.CS.ReadOnly = true;
            this.CS.Size = new System.Drawing.Size(34, 20);
            this.CS.TabIndex = 0;
            // 
            // flagsGroup
            // 
            this.flagsGroup.Controls.Add(this.VIP);
            this.flagsGroup.Controls.Add(this.VIF);
            this.flagsGroup.Controls.Add(this.AC);
            this.flagsGroup.Controls.Add(this.VM);
            this.flagsGroup.Controls.Add(this.RF);
            this.flagsGroup.Controls.Add(this.NT);
            this.flagsGroup.Controls.Add(this.IOPL);
            this.flagsGroup.Controls.Add(this.OF);
            this.flagsGroup.Controls.Add(this.DF);
            this.flagsGroup.Controls.Add(this.IF);
            this.flagsGroup.Controls.Add(this.TF);
            this.flagsGroup.Controls.Add(this.SF);
            this.flagsGroup.Controls.Add(this.ZF);
            this.flagsGroup.Controls.Add(this.AF);
            this.flagsGroup.Controls.Add(this.PF);
            this.flagsGroup.Controls.Add(this.CF);
            this.flagsGroup.Location = new System.Drawing.Point(212, 85);
            this.flagsGroup.Name = "flagsGroup";
            this.flagsGroup.Size = new System.Drawing.Size(204, 70);
            this.flagsGroup.TabIndex = 4;
            this.flagsGroup.TabStop = false;
            this.flagsGroup.Text = "Flags";
            // 
            // VIP
            // 
            this.VIP.Location = new System.Drawing.Point(169, 40);
            this.VIP.Name = "VIP";
            this.VIP.ReadOnly = true;
            this.VIP.Size = new System.Drawing.Size(23, 20);
            this.VIP.TabIndex = 15;
            // 
            // VIF
            // 
            this.VIF.Location = new System.Drawing.Point(146, 40);
            this.VIF.Name = "VIF";
            this.VIF.ReadOnly = true;
            this.VIF.Size = new System.Drawing.Size(23, 20);
            this.VIF.TabIndex = 14;
            // 
            // AC
            // 
            this.AC.Location = new System.Drawing.Point(123, 40);
            this.AC.Name = "AC";
            this.AC.ReadOnly = true;
            this.AC.Size = new System.Drawing.Size(23, 20);
            this.AC.TabIndex = 13;
            // 
            // VM
            // 
            this.VM.Location = new System.Drawing.Point(100, 40);
            this.VM.Name = "VM";
            this.VM.ReadOnly = true;
            this.VM.Size = new System.Drawing.Size(23, 20);
            this.VM.TabIndex = 12;
            // 
            // RF
            // 
            this.RF.Location = new System.Drawing.Point(77, 40);
            this.RF.Name = "RF";
            this.RF.ReadOnly = true;
            this.RF.Size = new System.Drawing.Size(23, 20);
            this.RF.TabIndex = 11;
            // 
            // NT
            // 
            this.NT.Location = new System.Drawing.Point(54, 40);
            this.NT.Name = "NT";
            this.NT.ReadOnly = true;
            this.NT.Size = new System.Drawing.Size(23, 20);
            this.NT.TabIndex = 10;
            // 
            // IOPL
            // 
            this.IOPL.Location = new System.Drawing.Point(31, 40);
            this.IOPL.Name = "IOPL";
            this.IOPL.ReadOnly = true;
            this.IOPL.Size = new System.Drawing.Size(23, 20);
            this.IOPL.TabIndex = 9;
            // 
            // OF
            // 
            this.OF.Location = new System.Drawing.Point(8, 40);
            this.OF.Name = "OF";
            this.OF.ReadOnly = true;
            this.OF.Size = new System.Drawing.Size(23, 20);
            this.OF.TabIndex = 8;
            // 
            // DF
            // 
            this.DF.Location = new System.Drawing.Point(169, 20);
            this.DF.Name = "DF";
            this.DF.ReadOnly = true;
            this.DF.Size = new System.Drawing.Size(23, 20);
            this.DF.TabIndex = 7;
            // 
            // IF
            // 
            this.IF.Location = new System.Drawing.Point(146, 20);
            this.IF.Name = "IF";
            this.IF.ReadOnly = true;
            this.IF.Size = new System.Drawing.Size(23, 20);
            this.IF.TabIndex = 6;
            // 
            // TF
            // 
            this.TF.Location = new System.Drawing.Point(123, 20);
            this.TF.Name = "TF";
            this.TF.ReadOnly = true;
            this.TF.Size = new System.Drawing.Size(23, 20);
            this.TF.TabIndex = 5;
            // 
            // SF
            // 
            this.SF.Location = new System.Drawing.Point(100, 20);
            this.SF.Name = "SF";
            this.SF.ReadOnly = true;
            this.SF.Size = new System.Drawing.Size(23, 20);
            this.SF.TabIndex = 4;
            // 
            // ZF
            // 
            this.ZF.Location = new System.Drawing.Point(77, 20);
            this.ZF.Name = "ZF";
            this.ZF.ReadOnly = true;
            this.ZF.Size = new System.Drawing.Size(23, 20);
            this.ZF.TabIndex = 3;
            // 
            // AF
            // 
            this.AF.Location = new System.Drawing.Point(54, 20);
            this.AF.Name = "AF";
            this.AF.ReadOnly = true;
            this.AF.Size = new System.Drawing.Size(23, 20);
            this.AF.TabIndex = 2;
            // 
            // PF
            // 
            this.PF.Location = new System.Drawing.Point(31, 20);
            this.PF.Name = "PF";
            this.PF.ReadOnly = true;
            this.PF.Size = new System.Drawing.Size(23, 20);
            this.PF.TabIndex = 1;
            // 
            // CF
            // 
            this.CF.Location = new System.Drawing.Point(8, 20);
            this.CF.Name = "CF";
            this.CF.ReadOnly = true;
            this.CF.Size = new System.Drawing.Size(23, 20);
            this.CF.TabIndex = 0;
            // 
            // floppyOpen
            // 
            this.floppyOpen.Filter = "All files|*.*";
            this.floppyOpen.Title = "Open Floppy Image File";
            // 
            // stepButton
            // 
            this.stepButton.Location = new System.Drawing.Point(15, 46);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(75, 23);
            this.stepButton.TabIndex = 0;
            this.stepButton.Text = "&Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.StepButtonClick);
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(329, 46);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 1;
            this.goButton.Text = "&Go";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.GoButtonClick);
            // 
            // cpuGroup
            // 
            this.cpuGroup.Controls.Add(this.stepOverButton);
            this.cpuGroup.Controls.Add(this.cpuLabel);
            this.cpuGroup.Controls.Add(this.stepButton);
            this.cpuGroup.Controls.Add(this.goButton);
            this.cpuGroup.Location = new System.Drawing.Point(0, 161);
            this.cpuGroup.Name = "cpuGroup";
            this.cpuGroup.Size = new System.Drawing.Size(416, 75);
            this.cpuGroup.TabIndex = 5;
            this.cpuGroup.TabStop = false;
            this.cpuGroup.Text = "CPU";
            // 
            // stepOverButton
            // 
            this.stepOverButton.Location = new System.Drawing.Point(124, 46);
            this.stepOverButton.Name = "stepOverButton";
            this.stepOverButton.Size = new System.Drawing.Size(75, 23);
            this.stepOverButton.TabIndex = 3;
            this.stepOverButton.Text = "&Step Over";
            this.stepOverButton.UseVisualStyleBackColor = true;
            this.stepOverButton.Click += new System.EventHandler(this.stepOverButton_Click);
            // 
            // cpuLabel
            // 
            this.cpuLabel.AutoSize = true;
            this.cpuLabel.Location = new System.Drawing.Point(6, 20);
            this.cpuLabel.Name = "cpuLabel";
            this.cpuLabel.Size = new System.Drawing.Size(0, 13);
            this.cpuLabel.TabIndex = 2;
            // 
            // memoryGroup
            // 
            this.memoryGroup.Controls.Add(this.memoryDisasmList);
            this.memoryGroup.Controls.Add(this.memoryCharList);
            this.memoryGroup.Controls.Add(this.memoryChar);
            this.memoryGroup.Controls.Add(this.memoryByte);
            this.memoryGroup.Controls.Add(this.memoryWord);
            this.memoryGroup.Controls.Add(this.memoryDWord);
            this.memoryGroup.Controls.Add(this.memoryList);
            this.memoryGroup.Controls.Add(this.memOffset);
            this.memoryGroup.Controls.Add(this.memoryButton);
            this.memoryGroup.Location = new System.Drawing.Point(0, 236);
            this.memoryGroup.Name = "memoryGroup";
            this.memoryGroup.Size = new System.Drawing.Size(650, 374);
            this.memoryGroup.TabIndex = 6;
            this.memoryGroup.TabStop = false;
            this.memoryGroup.Text = "Memory:";
            // 
            // memoryCharList
            // 
            this.memoryCharList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoryCharList.FormattingEnabled = true;
            this.memoryCharList.ItemHeight = 14;
            this.memoryCharList.Location = new System.Drawing.Point(387, 45);
            this.memoryCharList.Name = "memoryCharList";
            this.memoryCharList.Size = new System.Drawing.Size(257, 144);
            this.memoryCharList.TabIndex = 8;
            // 
            // memoryChar
            // 
            this.memoryChar.Location = new System.Drawing.Point(319, 20);
            this.memoryChar.Name = "memoryChar";
            this.memoryChar.ReadOnly = true;
            this.memoryChar.Size = new System.Drawing.Size(23, 20);
            this.memoryChar.TabIndex = 7;
            // 
            // memoryByte
            // 
            this.memoryByte.Location = new System.Drawing.Point(290, 20);
            this.memoryByte.Name = "memoryByte";
            this.memoryByte.ReadOnly = true;
            this.memoryByte.Size = new System.Drawing.Size(23, 20);
            this.memoryByte.TabIndex = 6;
            // 
            // memoryWord
            // 
            this.memoryWord.Location = new System.Drawing.Point(250, 20);
            this.memoryWord.MaxLength = 4;
            this.memoryWord.Name = "memoryWord";
            this.memoryWord.ReadOnly = true;
            this.memoryWord.Size = new System.Drawing.Size(34, 20);
            this.memoryWord.TabIndex = 5;
            // 
            // memoryDWord
            // 
            this.memoryDWord.Location = new System.Drawing.Point(174, 20);
            this.memoryDWord.Name = "memoryDWord";
            this.memoryDWord.ReadOnly = true;
            this.memoryDWord.Size = new System.Drawing.Size(70, 20);
            this.memoryDWord.TabIndex = 4;
            // 
            // memoryList
            // 
            this.memoryList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoryList.FormattingEnabled = true;
            this.memoryList.ItemHeight = 14;
            this.memoryList.Location = new System.Drawing.Point(4, 45);
            this.memoryList.Name = "memoryList";
            this.memoryList.Size = new System.Drawing.Size(380, 144);
            this.memoryList.TabIndex = 3;
            // 
            // memOffset
            // 
            this.memOffset.Location = new System.Drawing.Point(6, 20);
            this.memOffset.MaxLength = 8;
            this.memOffset.Name = "memOffset";
            this.memOffset.Size = new System.Drawing.Size(84, 20);
            this.memOffset.TabIndex = 1;
            // 
            // memoryButton
            // 
            this.memoryButton.Location = new System.Drawing.Point(96, 18);
            this.memoryButton.Name = "memoryButton";
            this.memoryButton.Size = new System.Drawing.Size(75, 23);
            this.memoryButton.TabIndex = 2;
            this.memoryButton.Text = "&Lookup";
            this.memoryButton.UseVisualStyleBackColor = true;
            this.memoryButton.Click += new System.EventHandler(this.MemoryButtonClick);
            // 
            // mainStatus
            // 
            this.mainStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tpsLabel});
            this.mainStatus.Location = new System.Drawing.Point(0, 613);
            this.mainStatus.Name = "mainStatus";
            this.mainStatus.Size = new System.Drawing.Size(656, 22);
            this.mainStatus.TabIndex = 8;
            // 
            // tpsLabel
            // 
            this.tpsLabel.Name = "tpsLabel";
            this.tpsLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // stackGroup
            // 
            this.stackGroup.Controls.Add(this.baseList);
            this.stackGroup.Controls.Add(this.stackList);
            this.stackGroup.Location = new System.Drawing.Point(422, 27);
            this.stackGroup.Name = "stackGroup";
            this.stackGroup.Size = new System.Drawing.Size(228, 209);
            this.stackGroup.TabIndex = 9;
            this.stackGroup.TabStop = false;
            this.stackGroup.Text = "Stack:";
            // 
            // baseList
            // 
            this.baseList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseList.FormattingEnabled = true;
            this.baseList.ItemHeight = 14;
            this.baseList.Location = new System.Drawing.Point(115, 16);
            this.baseList.Name = "baseList";
            this.baseList.Size = new System.Drawing.Size(108, 186);
            this.baseList.TabIndex = 1;
            // 
            // stackList
            // 
            this.stackList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stackList.FormattingEnabled = true;
            this.stackList.ItemHeight = 14;
            this.stackList.Location = new System.Drawing.Point(6, 16);
            this.stackList.Name = "stackList";
            this.stackList.Size = new System.Drawing.Size(108, 186);
            this.stackList.TabIndex = 0;
            // 
            // memoryDisasmList
            // 
            this.memoryDisasmList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoryDisasmList.FormattingEnabled = true;
            this.memoryDisasmList.ItemHeight = 14;
            this.memoryDisasmList.Location = new System.Drawing.Point(4, 195);
            this.memoryDisasmList.Name = "memoryDisasmList";
            this.memoryDisasmList.Size = new System.Drawing.Size(640, 172);
            this.memoryDisasmList.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 635);
            this.Controls.Add(this.stackGroup);
            this.Controls.Add(this.mainStatus);
            this.Controls.Add(this.memoryGroup);
            this.Controls.Add(this.cpuGroup);
            this.Controls.Add(this.flagsGroup);
            this.Controls.Add(this.segmentGroup);
            this.Controls.Add(this.registersGroup);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "x86 CS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.registersGroup.ResumeLayout(false);
            this.registersGroup.PerformLayout();
            this.segmentGroup.ResumeLayout(false);
            this.segmentGroup.PerformLayout();
            this.flagsGroup.ResumeLayout(false);
            this.flagsGroup.PerformLayout();
            this.cpuGroup.ResumeLayout(false);
            this.cpuGroup.PerformLayout();
            this.memoryGroup.ResumeLayout(false);
            this.memoryGroup.PerformLayout();
            this.mainStatus.ResumeLayout(false);
            this.mainStatus.PerformLayout();
            this.stackGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.GroupBox registersGroup;
        private System.Windows.Forms.TextBox EAX;
        private System.Windows.Forms.Label ecxLabel;
        private System.Windows.Forms.Label ebxLabel;
        private System.Windows.Forms.Label eaxLabel;
        private System.Windows.Forms.TextBox EBX;
        private System.Windows.Forms.TextBox EDX;
        private System.Windows.Forms.TextBox ECX;
        private System.Windows.Forms.Label edxLabel;
        private System.Windows.Forms.Label espLabel;
        private System.Windows.Forms.Label ebpLabel;
        private System.Windows.Forms.Label ediLabel;
        private System.Windows.Forms.Label esiLabel;
        private System.Windows.Forms.TextBox EDI;
        private System.Windows.Forms.TextBox ESP;
        private System.Windows.Forms.TextBox EBP;
        private System.Windows.Forms.TextBox ESI;
        private System.Windows.Forms.GroupBox segmentGroup;
        private System.Windows.Forms.Label csLabel;
        private System.Windows.Forms.TextBox CS;
        private System.Windows.Forms.Label ssLabel;
        private System.Windows.Forms.TextBox SS;
        private System.Windows.Forms.Label gsLabel;
        private System.Windows.Forms.TextBox GS;
        private System.Windows.Forms.Label fsLabel;
        private System.Windows.Forms.TextBox FS;
        private System.Windows.Forms.Label esLabel;
        private System.Windows.Forms.TextBox ES;
        private System.Windows.Forms.Label dsLabel;
        private System.Windows.Forms.TextBox DS;
        private System.Windows.Forms.GroupBox flagsGroup;
        private System.Windows.Forms.TextBox SF;
        private System.Windows.Forms.TextBox ZF;
        private System.Windows.Forms.TextBox AF;
        private System.Windows.Forms.TextBox PF;
        private System.Windows.Forms.TextBox CF;
        private System.Windows.Forms.TextBox VM;
        private System.Windows.Forms.TextBox RF;
        private System.Windows.Forms.TextBox NT;
        private System.Windows.Forms.TextBox IOPL;
        private System.Windows.Forms.TextBox OF;
        private System.Windows.Forms.TextBox DF;
        private System.Windows.Forms.TextBox IF;
        private System.Windows.Forms.TextBox TF;
        private System.Windows.Forms.TextBox VIP;
        private System.Windows.Forms.TextBox VIF;
        private System.Windows.Forms.TextBox AC;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem floppyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mountToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog floppyOpen;
        private System.Windows.Forms.Button stepButton;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.GroupBox cpuGroup;
        private System.Windows.Forms.Label cpuLabel;
        private System.Windows.Forms.GroupBox memoryGroup;
        private System.Windows.Forms.Button memoryButton;
        private System.Windows.Forms.TextBox memOffset;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breakpointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.StatusStrip mainStatus;
        private System.Windows.Forms.ToolStripStatusLabel tpsLabel;
        private System.Windows.Forms.Button stepOverButton;
        private System.Windows.Forms.ListBox memoryCharList;
        private System.Windows.Forms.TextBox memoryChar;
        private System.Windows.Forms.TextBox memoryByte;
        private System.Windows.Forms.TextBox memoryWord;
        private System.Windows.Forms.TextBox memoryDWord;
        private System.Windows.Forms.ListBox memoryList;
        private System.Windows.Forms.GroupBox stackGroup;
        private System.Windows.Forms.ListBox stackList;
        private System.Windows.Forms.ListBox baseList;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ListBox memoryDisasmList;
    }
}

