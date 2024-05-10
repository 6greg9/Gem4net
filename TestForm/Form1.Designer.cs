namespace TestForm;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        button1 = new Button();
        button2 = new Button();
        richTextBox1 = new RichTextBox();
        button3 = new Button();
        rtbx_HSMS = new RichTextBox();
        lbl_HSMS = new Label();
        lbl_Communication = new Label();
        rtbx_Comm = new RichTextBox();
        rtbx_Ctrl = new RichTextBox();
        lbl_Control = new Label();
        Btn_GoOffLine = new Button();
        Btn_GoOnLine = new Button();
        Btn_GoLocal = new Button();
        Btn_GoRemote = new Button();
        panel1 = new Panel();
        Tab_Events = new TabControl();
        Tab_GemState = new TabPage();
        tabPage2 = new TabPage();
        Tab_Variables = new TabPage();
        Btn_GetSvById = new Button();
        Tbx_InputVarValue = new TextBox();
        label1 = new Label();
        Tbx_InputVid = new TextBox();
        Lbl_VID = new Label();
        Btn_UpdateEC = new Button();
        Page_Events = new TabPage();
        Btn_TestSendS6F11 = new Button();
        label2 = new Label();
        Btn_TestGetEvents = new Button();
        Tbx_InputECID = new TextBox();
        Page_Terminal = new TabPage();
        Btn_S10F1TerminalRequest = new Button();
        Tbx_TerminalInput = new RichTextBox();
        Tbx_Terminal = new RichTextBox();
        Page_ProcessProgram = new TabPage();
        richTextBox2 = new RichTextBox();
        Btn_DeletePP = new Button();
        Btn_InsertPP = new Button();
        Btn_SelectAllPP = new Button();
        Page_Alarm = new TabPage();
        Num_AlarmId = new NumericUpDown();
        Cbx_SetAlarm = new CheckBox();
        Tbx_AlarmText = new TextBox();
        Btn_SendAlarm = new Button();
        timer1 = new System.Windows.Forms.Timer(components);
        panel1.SuspendLayout();
        Tab_Events.SuspendLayout();
        Tab_GemState.SuspendLayout();
        tabPage2.SuspendLayout();
        Tab_Variables.SuspendLayout();
        Page_Events.SuspendLayout();
        Page_Terminal.SuspendLayout();
        Page_ProcessProgram.SuspendLayout();
        Page_Alarm.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)Num_AlarmId).BeginInit();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(1575, 53);
        button1.Margin = new Padding(5);
        button1.Name = "button1";
        button1.Size = new Size(196, 91);
        button1.TabIndex = 0;
        button1.Text = "VarRepo";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // button2
        // 
        button2.Location = new Point(1575, 153);
        button2.Margin = new Padding(5);
        button2.Name = "button2";
        button2.Size = new Size(185, 61);
        button2.TabIndex = 1;
        button2.Text = "EnableGem";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // richTextBox1
        // 
        richTextBox1.Location = new Point(74, 171);
        richTextBox1.Margin = new Padding(5);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(1673, 416);
        richTextBox1.TabIndex = 3;
        richTextBox1.Text = "";
        // 
        // button3
        // 
        button3.Location = new Point(1583, 243);
        button3.Margin = new Padding(5);
        button3.Name = "button3";
        button3.Size = new Size(173, 53);
        button3.TabIndex = 5;
        button3.Text = "S1F1";
        button3.UseVisualStyleBackColor = true;
        button3.Click += button3_Click;
        // 
        // rtbx_HSMS
        // 
        rtbx_HSMS.Location = new Point(31, 77);
        rtbx_HSMS.Margin = new Padding(5);
        rtbx_HSMS.Name = "rtbx_HSMS";
        rtbx_HSMS.Size = new Size(220, 229);
        rtbx_HSMS.TabIndex = 4;
        rtbx_HSMS.Text = "";
        // 
        // lbl_HSMS
        // 
        lbl_HSMS.AutoSize = true;
        lbl_HSMS.Location = new Point(31, 22);
        lbl_HSMS.Margin = new Padding(5, 0, 5, 0);
        lbl_HSMS.Name = "lbl_HSMS";
        lbl_HSMS.Size = new Size(95, 36);
        lbl_HSMS.TabIndex = 6;
        lbl_HSMS.Text = "HSMS";
        // 
        // lbl_Communication
        // 
        lbl_Communication.AutoSize = true;
        lbl_Communication.Location = new Point(257, 22);
        lbl_Communication.Margin = new Padding(5, 0, 5, 0);
        lbl_Communication.Name = "lbl_Communication";
        lbl_Communication.Size = new Size(104, 36);
        lbl_Communication.TabIndex = 8;
        lbl_Communication.Text = "Comm";
        // 
        // rtbx_Comm
        // 
        rtbx_Comm.Location = new Point(261, 77);
        rtbx_Comm.Margin = new Padding(5);
        rtbx_Comm.Name = "rtbx_Comm";
        rtbx_Comm.Size = new Size(509, 229);
        rtbx_Comm.TabIndex = 9;
        rtbx_Comm.Text = "";
        // 
        // rtbx_Ctrl
        // 
        rtbx_Ctrl.Location = new Point(782, 77);
        rtbx_Ctrl.Margin = new Padding(5);
        rtbx_Ctrl.Name = "rtbx_Ctrl";
        rtbx_Ctrl.Size = new Size(781, 229);
        rtbx_Ctrl.TabIndex = 10;
        rtbx_Ctrl.Text = "";
        // 
        // lbl_Control
        // 
        lbl_Control.AutoSize = true;
        lbl_Control.Location = new Point(782, 22);
        lbl_Control.Margin = new Padding(5, 0, 5, 0);
        lbl_Control.Name = "lbl_Control";
        lbl_Control.Size = new Size(114, 36);
        lbl_Control.TabIndex = 11;
        lbl_Control.Text = "Control";
        // 
        // Btn_GoOffLine
        // 
        Btn_GoOffLine.Location = new Point(782, 388);
        Btn_GoOffLine.Margin = new Padding(5);
        Btn_GoOffLine.Name = "Btn_GoOffLine";
        Btn_GoOffLine.Size = new Size(185, 61);
        Btn_GoOffLine.TabIndex = 12;
        Btn_GoOffLine.Text = "Off-Line";
        Btn_GoOffLine.UseVisualStyleBackColor = true;
        Btn_GoOffLine.Click += Btn_GoOffLine_Click;
        // 
        // Btn_GoOnLine
        // 
        Btn_GoOnLine.Location = new Point(782, 318);
        Btn_GoOnLine.Margin = new Padding(5);
        Btn_GoOnLine.Name = "Btn_GoOnLine";
        Btn_GoOnLine.Size = new Size(185, 61);
        Btn_GoOnLine.TabIndex = 13;
        Btn_GoOnLine.Text = "On-Line";
        Btn_GoOnLine.UseVisualStyleBackColor = true;
        Btn_GoOnLine.Click += Btn_GoOnLine_Click;
        // 
        // Btn_GoLocal
        // 
        Btn_GoLocal.Location = new Point(977, 388);
        Btn_GoLocal.Margin = new Padding(5);
        Btn_GoLocal.Name = "Btn_GoLocal";
        Btn_GoLocal.Size = new Size(185, 61);
        Btn_GoLocal.TabIndex = 14;
        Btn_GoLocal.Text = "Local";
        Btn_GoLocal.UseVisualStyleBackColor = true;
        Btn_GoLocal.Click += Btn_GoLocal_Click;
        // 
        // Btn_GoRemote
        // 
        Btn_GoRemote.Location = new Point(977, 318);
        Btn_GoRemote.Margin = new Padding(5);
        Btn_GoRemote.Name = "Btn_GoRemote";
        Btn_GoRemote.Size = new Size(185, 61);
        Btn_GoRemote.TabIndex = 15;
        Btn_GoRemote.Text = "Remote";
        Btn_GoRemote.UseVisualStyleBackColor = true;
        Btn_GoRemote.Click += Btn_GoRemote_Click;
        // 
        // panel1
        // 
        panel1.Controls.Add(lbl_HSMS);
        panel1.Controls.Add(Btn_GoLocal);
        panel1.Controls.Add(Btn_GoOffLine);
        panel1.Controls.Add(Btn_GoRemote);
        panel1.Controls.Add(rtbx_HSMS);
        panel1.Controls.Add(lbl_Communication);
        panel1.Controls.Add(Btn_GoOnLine);
        panel1.Controls.Add(rtbx_Comm);
        panel1.Controls.Add(lbl_Control);
        panel1.Controls.Add(button3);
        panel1.Controls.Add(rtbx_Ctrl);
        panel1.Controls.Add(button1);
        panel1.Controls.Add(button2);
        panel1.Location = new Point(85, 88);
        panel1.Margin = new Padding(5);
        panel1.Name = "panel1";
        panel1.Size = new Size(1793, 501);
        panel1.TabIndex = 16;
        // 
        // Tab_Events
        // 
        Tab_Events.Controls.Add(Tab_GemState);
        Tab_Events.Controls.Add(tabPage2);
        Tab_Events.Controls.Add(Tab_Variables);
        Tab_Events.Controls.Add(Page_Events);
        Tab_Events.Controls.Add(Page_Terminal);
        Tab_Events.Controls.Add(Page_ProcessProgram);
        Tab_Events.Controls.Add(Page_Alarm);
        Tab_Events.Location = new Point(19, 357);
        Tab_Events.Margin = new Padding(5);
        Tab_Events.Name = "Tab_Events";
        Tab_Events.SelectedIndex = 0;
        Tab_Events.Size = new Size(2091, 679);
        Tab_Events.TabIndex = 17;
        // 
        // Tab_GemState
        // 
        Tab_GemState.BackColor = Color.Gainsboro;
        Tab_GemState.Controls.Add(panel1);
        Tab_GemState.Location = new Point(4, 45);
        Tab_GemState.Margin = new Padding(5);
        Tab_GemState.Name = "Tab_GemState";
        Tab_GemState.Padding = new Padding(5);
        Tab_GemState.Size = new Size(2083, 630);
        Tab_GemState.TabIndex = 0;
        Tab_GemState.Text = "State";
        // 
        // tabPage2
        // 
        tabPage2.BackColor = Color.LightGray;
        tabPage2.Controls.Add(richTextBox1);
        tabPage2.Location = new Point(4, 32);
        tabPage2.Margin = new Padding(5);
        tabPage2.Name = "tabPage2";
        tabPage2.Padding = new Padding(5);
        tabPage2.Size = new Size(2083, 643);
        tabPage2.TabIndex = 1;
        tabPage2.Text = "Log";
        // 
        // Tab_Variables
        // 
        Tab_Variables.Controls.Add(Btn_GetSvById);
        Tab_Variables.Controls.Add(Tbx_InputVarValue);
        Tab_Variables.Controls.Add(label1);
        Tab_Variables.Controls.Add(Tbx_InputVid);
        Tab_Variables.Controls.Add(Lbl_VID);
        Tab_Variables.Controls.Add(Btn_UpdateEC);
        Tab_Variables.Location = new Point(4, 32);
        Tab_Variables.Margin = new Padding(5);
        Tab_Variables.Name = "Tab_Variables";
        Tab_Variables.Size = new Size(2083, 643);
        Tab_Variables.TabIndex = 2;
        Tab_Variables.Text = "Variables";
        Tab_Variables.UseVisualStyleBackColor = true;
        // 
        // Btn_GetSvById
        // 
        Btn_GetSvById.Location = new Point(399, 119);
        Btn_GetSvById.Margin = new Padding(5);
        Btn_GetSvById.Name = "Btn_GetSvById";
        Btn_GetSvById.Size = new Size(275, 72);
        Btn_GetSvById.TabIndex = 5;
        Btn_GetSvById.Text = "TestGetSV";
        Btn_GetSvById.UseVisualStyleBackColor = true;
        Btn_GetSvById.Click += Btn_GetSvById_Click;
        // 
        // Tbx_InputVarValue
        // 
        Tbx_InputVarValue.Location = new Point(116, 255);
        Tbx_InputVarValue.Margin = new Padding(5);
        Tbx_InputVarValue.Name = "Tbx_InputVarValue";
        Tbx_InputVarValue.Size = new Size(222, 43);
        Tbx_InputVarValue.TabIndex = 4;
        // 
        // label1
        // 
        label1.Location = new Point(116, 211);
        label1.Margin = new Padding(5, 0, 5, 0);
        label1.Name = "label1";
        label1.Size = new Size(224, 39);
        label1.TabIndex = 3;
        label1.Text = "UpdateValue";
        // 
        // Tbx_InputVid
        // 
        Tbx_InputVid.Location = new Point(116, 133);
        Tbx_InputVid.Margin = new Padding(5);
        Tbx_InputVid.Name = "Tbx_InputVid";
        Tbx_InputVid.Size = new Size(222, 43);
        Tbx_InputVid.TabIndex = 2;
        // 
        // Lbl_VID
        // 
        Lbl_VID.Location = new Point(116, 89);
        Lbl_VID.Margin = new Padding(5, 0, 5, 0);
        Lbl_VID.Name = "Lbl_VID";
        Lbl_VID.Size = new Size(224, 39);
        Lbl_VID.TabIndex = 1;
        Lbl_VID.Text = "VID";
        // 
        // Btn_UpdateEC
        // 
        Btn_UpdateEC.Location = new Point(399, 241);
        Btn_UpdateEC.Margin = new Padding(5);
        Btn_UpdateEC.Name = "Btn_UpdateEC";
        Btn_UpdateEC.Size = new Size(275, 72);
        Btn_UpdateEC.TabIndex = 0;
        Btn_UpdateEC.Text = "TestUpdateSV";
        Btn_UpdateEC.UseVisualStyleBackColor = true;
        Btn_UpdateEC.Click += Btn_UpdateSV_Click;
        // 
        // Page_Events
        // 
        Page_Events.Controls.Add(Btn_TestSendS6F11);
        Page_Events.Controls.Add(label2);
        Page_Events.Controls.Add(Btn_TestGetEvents);
        Page_Events.Controls.Add(Tbx_InputECID);
        Page_Events.Location = new Point(4, 32);
        Page_Events.Margin = new Padding(5);
        Page_Events.Name = "Page_Events";
        Page_Events.Size = new Size(2083, 643);
        Page_Events.TabIndex = 3;
        Page_Events.Text = "Events";
        Page_Events.UseVisualStyleBackColor = true;
        // 
        // Btn_TestSendS6F11
        // 
        Btn_TestSendS6F11.Location = new Point(451, 194);
        Btn_TestSendS6F11.Margin = new Padding(5);
        Btn_TestSendS6F11.Name = "Btn_TestSendS6F11";
        Btn_TestSendS6F11.Size = new Size(258, 77);
        Btn_TestSendS6F11.TabIndex = 3;
        Btn_TestSendS6F11.Text = "SendS6F11";
        Btn_TestSendS6F11.UseVisualStyleBackColor = true;
        Btn_TestSendS6F11.Click += Btn_TestSendS6F11_Click;
        // 
        // label2
        // 
        label2.Location = new Point(189, 80);
        label2.Margin = new Padding(5, 0, 5, 0);
        label2.Name = "label2";
        label2.Size = new Size(224, 39);
        label2.TabIndex = 2;
        label2.Text = "CEID";
        // 
        // Btn_TestGetEvents
        // 
        Btn_TestGetEvents.Location = new Point(451, 108);
        Btn_TestGetEvents.Margin = new Padding(5);
        Btn_TestGetEvents.Name = "Btn_TestGetEvents";
        Btn_TestGetEvents.Size = new Size(258, 77);
        Btn_TestGetEvents.TabIndex = 1;
        Btn_TestGetEvents.Text = "TestGetEvents";
        Btn_TestGetEvents.UseVisualStyleBackColor = true;
        Btn_TestGetEvents.Click += Btn_TestGetEvents_Click;
        // 
        // Tbx_InputECID
        // 
        Tbx_InputECID.Location = new Point(189, 124);
        Tbx_InputECID.Margin = new Padding(5);
        Tbx_InputECID.Name = "Tbx_InputECID";
        Tbx_InputECID.Size = new Size(230, 43);
        Tbx_InputECID.TabIndex = 0;
        // 
        // Page_Terminal
        // 
        Page_Terminal.Controls.Add(Btn_S10F1TerminalRequest);
        Page_Terminal.Controls.Add(Tbx_TerminalInput);
        Page_Terminal.Controls.Add(Tbx_Terminal);
        Page_Terminal.Location = new Point(4, 32);
        Page_Terminal.Margin = new Padding(5);
        Page_Terminal.Name = "Page_Terminal";
        Page_Terminal.Size = new Size(2083, 643);
        Page_Terminal.TabIndex = 4;
        Page_Terminal.Text = "Terminal";
        Page_Terminal.UseVisualStyleBackColor = true;
        // 
        // Btn_S10F1TerminalRequest
        // 
        Btn_S10F1TerminalRequest.Location = new Point(967, 366);
        Btn_S10F1TerminalRequest.Margin = new Padding(5);
        Btn_S10F1TerminalRequest.Name = "Btn_S10F1TerminalRequest";
        Btn_S10F1TerminalRequest.Size = new Size(294, 99);
        Btn_S10F1TerminalRequest.TabIndex = 13;
        Btn_S10F1TerminalRequest.Text = "S10F1TermialReq";
        Btn_S10F1TerminalRequest.UseVisualStyleBackColor = true;
        Btn_S10F1TerminalRequest.Click += Btn_S10F1TerminalRequest_Click;
        // 
        // Tbx_TerminalInput
        // 
        Tbx_TerminalInput.Location = new Point(155, 357);
        Tbx_TerminalInput.Margin = new Padding(5);
        Tbx_TerminalInput.Name = "Tbx_TerminalInput";
        Tbx_TerminalInput.Size = new Size(781, 106);
        Tbx_TerminalInput.TabIndex = 12;
        Tbx_TerminalInput.Text = "";
        // 
        // Tbx_Terminal
        // 
        Tbx_Terminal.Location = new Point(155, 130);
        Tbx_Terminal.Margin = new Padding(5);
        Tbx_Terminal.Name = "Tbx_Terminal";
        Tbx_Terminal.Size = new Size(781, 229);
        Tbx_Terminal.TabIndex = 11;
        Tbx_Terminal.Text = "";
        // 
        // Page_ProcessProgram
        // 
        Page_ProcessProgram.Controls.Add(richTextBox2);
        Page_ProcessProgram.Controls.Add(Btn_DeletePP);
        Page_ProcessProgram.Controls.Add(Btn_InsertPP);
        Page_ProcessProgram.Controls.Add(Btn_SelectAllPP);
        Page_ProcessProgram.Location = new Point(4, 32);
        Page_ProcessProgram.Margin = new Padding(5);
        Page_ProcessProgram.Name = "Page_ProcessProgram";
        Page_ProcessProgram.Size = new Size(2083, 643);
        Page_ProcessProgram.TabIndex = 5;
        Page_ProcessProgram.Text = "ProcessProgram";
        Page_ProcessProgram.UseVisualStyleBackColor = true;
        // 
        // richTextBox2
        // 
        richTextBox2.Location = new Point(224, 345);
        richTextBox2.Name = "richTextBox2";
        richTextBox2.Size = new Size(348, 144);
        richTextBox2.TabIndex = 3;
        richTextBox2.Text = "";
        // 
        // Btn_DeletePP
        // 
        Btn_DeletePP.Location = new Point(876, 419);
        Btn_DeletePP.Name = "Btn_DeletePP";
        Btn_DeletePP.Size = new Size(252, 65);
        Btn_DeletePP.TabIndex = 2;
        Btn_DeletePP.Text = "DeletePP";
        Btn_DeletePP.UseVisualStyleBackColor = true;
        Btn_DeletePP.Click += Btn_DeletePP_Click;
        // 
        // Btn_InsertPP
        // 
        Btn_InsertPP.Location = new Point(888, 308);
        Btn_InsertPP.Margin = new Padding(5);
        Btn_InsertPP.Name = "Btn_InsertPP";
        Btn_InsertPP.Size = new Size(227, 72);
        Btn_InsertPP.TabIndex = 1;
        Btn_InsertPP.Text = "InsertPP";
        Btn_InsertPP.UseVisualStyleBackColor = true;
        Btn_InsertPP.Click += Btn_InsertPP_Click;
        // 
        // Btn_SelectAllPP
        // 
        Btn_SelectAllPP.Location = new Point(876, 167);
        Btn_SelectAllPP.Margin = new Padding(5);
        Btn_SelectAllPP.Name = "Btn_SelectAllPP";
        Btn_SelectAllPP.Size = new Size(227, 72);
        Btn_SelectAllPP.TabIndex = 0;
        Btn_SelectAllPP.Text = "SelectAllPP";
        Btn_SelectAllPP.UseVisualStyleBackColor = true;
        Btn_SelectAllPP.Click += Btn_SelectAllPP_Click;
        // 
        // Page_Alarm
        // 
        Page_Alarm.Controls.Add(Num_AlarmId);
        Page_Alarm.Controls.Add(Cbx_SetAlarm);
        Page_Alarm.Controls.Add(Tbx_AlarmText);
        Page_Alarm.Controls.Add(Btn_SendAlarm);
        Page_Alarm.Location = new Point(4, 32);
        Page_Alarm.Margin = new Padding(5);
        Page_Alarm.Name = "Page_Alarm";
        Page_Alarm.Size = new Size(2083, 643);
        Page_Alarm.TabIndex = 6;
        Page_Alarm.Text = "Alarm";
        Page_Alarm.UseVisualStyleBackColor = true;
        // 
        // Num_AlarmId
        // 
        Num_AlarmId.Location = new Point(156, 136);
        Num_AlarmId.Margin = new Padding(5);
        Num_AlarmId.Name = "Num_AlarmId";
        Num_AlarmId.Size = new Size(172, 43);
        Num_AlarmId.TabIndex = 3;
        // 
        // Cbx_SetAlarm
        // 
        Cbx_SetAlarm.AutoSize = true;
        Cbx_SetAlarm.Location = new Point(151, 86);
        Cbx_SetAlarm.Margin = new Padding(5);
        Cbx_SetAlarm.Name = "Cbx_SetAlarm";
        Cbx_SetAlarm.Size = new Size(160, 40);
        Cbx_SetAlarm.TabIndex = 2;
        Cbx_SetAlarm.Text = "SetAlarm";
        Cbx_SetAlarm.UseVisualStyleBackColor = true;
        // 
        // Tbx_AlarmText
        // 
        Tbx_AlarmText.Location = new Point(151, 193);
        Tbx_AlarmText.Margin = new Padding(5);
        Tbx_AlarmText.Name = "Tbx_AlarmText";
        Tbx_AlarmText.Size = new Size(296, 43);
        Tbx_AlarmText.TabIndex = 1;
        // 
        // Btn_SendAlarm
        // 
        Btn_SendAlarm.Location = new Point(496, 163);
        Btn_SendAlarm.Margin = new Padding(5);
        Btn_SendAlarm.Name = "Btn_SendAlarm";
        Btn_SendAlarm.Size = new Size(209, 77);
        Btn_SendAlarm.TabIndex = 0;
        Btn_SendAlarm.Text = "SendAlarm";
        Btn_SendAlarm.UseVisualStyleBackColor = true;
        Btn_SendAlarm.Click += Btn_SendAlarm_Click;
        // 
        // timer1
        // 
        timer1.Tick += timer1_Tick;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(17F, 36F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1924, 1014);
        Controls.Add(Tab_Events);
        Font = new Font("Microsoft JhengHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
        Margin = new Padding(5);
        Name = "Form1";
        Text = "Form1";
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        Tab_Events.ResumeLayout(false);
        Tab_GemState.ResumeLayout(false);
        tabPage2.ResumeLayout(false);
        Tab_Variables.ResumeLayout(false);
        Tab_Variables.PerformLayout();
        Page_Events.ResumeLayout(false);
        Page_Events.PerformLayout();
        Page_Terminal.ResumeLayout(false);
        Page_ProcessProgram.ResumeLayout(false);
        Page_Alarm.ResumeLayout(false);
        Page_Alarm.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)Num_AlarmId).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Button button1;
    private Button button2;
    private RichTextBox richTextBox1;
    private Button button3;
    private RichTextBox rtbx_HSMS;
    private Label lbl_HSMS;
    private Label lbl_Communication;
    private RichTextBox rtbx_Comm;
    private RichTextBox rtbx_Ctrl;
    private Label lbl_Control;
    private Button Btn_GoOffLine;
    private Button Btn_GoOnLine;
    private Button Btn_GoLocal;
    private Button Btn_GoRemote;
    private Panel panel1;
    private TabControl Tab_Events;
    private TabPage Tab_GemState;
    private TabPage tabPage2;
    private TabPage Tab_Variables;
    private Button Btn_UpdateEC;
    private TabPage Page_Events;
    private Button Btn_TestGetEvents;
    private TextBox Tbx_InputECID;
    private TextBox Tbx_InputVid;
    private Label Lbl_VID;
    private Button Btn_GetSvById;
    private TextBox Tbx_InputVarValue;
    private Label label1;
    private Label label2;
    private Button Btn_TestSendS6F11;
    private TabPage Page_Terminal;
    private Button Btn_S10F1TerminalRequest;
    private RichTextBox Tbx_TerminalInput;
    private RichTextBox Tbx_Terminal;
    private TabPage Page_ProcessProgram;
    private Button Btn_InsertPP;
    private Button Btn_SelectAllPP;
    private TabPage Page_Alarm;
    private Button Btn_SendAlarm;
    private CheckBox Cbx_SetAlarm;
    private TextBox Tbx_AlarmText;
    private NumericUpDown Num_AlarmId;
    private System.Windows.Forms.Timer timer1;
    private RichTextBox richTextBox2;
    private Button Btn_DeletePP;
}
