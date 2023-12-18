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
        tabPage1 = new TabPage();
        tabPage2 = new TabPage();
        Tab_Variables = new TabPage();
        Btn_GetSvById = new Button();
        Tbx_InputVarValue = new TextBox();
        label1 = new Label();
        Tbx_InputVid = new TextBox();
        Lbl_VID = new Label();
        Btn_UpdateEC = new Button();
        Page_Events = new TabPage();
        Btn_TestGetEvents = new Button();
        Tbx_InputECID = new TextBox();
        label2 = new Label();
        Btn_TestSendS6F11 = new Button();
        panel1.SuspendLayout();
        Tab_Events.SuspendLayout();
        tabPage1.SuspendLayout();
        tabPage2.SuspendLayout();
        Tab_Variables.SuspendLayout();
        Page_Events.SuspendLayout();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(1019, 34);
        button1.Name = "button1";
        button1.Size = new Size(127, 58);
        button1.TabIndex = 0;
        button1.Text = "VarRepo";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // button2
        // 
        button2.Location = new Point(1019, 98);
        button2.Name = "button2";
        button2.Size = new Size(120, 39);
        button2.TabIndex = 1;
        button2.Text = "EnableGem";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // richTextBox1
        // 
        richTextBox1.Location = new Point(48, 109);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(1084, 267);
        richTextBox1.TabIndex = 3;
        richTextBox1.Text = "";
        // 
        // button3
        // 
        button3.Location = new Point(1024, 155);
        button3.Name = "button3";
        button3.Size = new Size(112, 34);
        button3.TabIndex = 5;
        button3.Text = "S1F1";
        button3.UseVisualStyleBackColor = true;
        button3.Click += button3_Click;
        // 
        // rtbx_HSMS
        // 
        rtbx_HSMS.Location = new Point(20, 49);
        rtbx_HSMS.Name = "rtbx_HSMS";
        rtbx_HSMS.Size = new Size(144, 148);
        rtbx_HSMS.TabIndex = 4;
        rtbx_HSMS.Text = "";
        // 
        // lbl_HSMS
        // 
        lbl_HSMS.AutoSize = true;
        lbl_HSMS.Location = new Point(20, 14);
        lbl_HSMS.Name = "lbl_HSMS";
        lbl_HSMS.Size = new Size(61, 23);
        lbl_HSMS.TabIndex = 6;
        lbl_HSMS.Text = "HSMS";
        // 
        // lbl_Communication
        // 
        lbl_Communication.AutoSize = true;
        lbl_Communication.Location = new Point(166, 14);
        lbl_Communication.Name = "lbl_Communication";
        lbl_Communication.Size = new Size(67, 23);
        lbl_Communication.TabIndex = 8;
        lbl_Communication.Text = "Comm";
        // 
        // rtbx_Comm
        // 
        rtbx_Comm.Location = new Point(169, 49);
        rtbx_Comm.Name = "rtbx_Comm";
        rtbx_Comm.Size = new Size(331, 148);
        rtbx_Comm.TabIndex = 9;
        rtbx_Comm.Text = "";
        // 
        // rtbx_Ctrl
        // 
        rtbx_Ctrl.Location = new Point(506, 49);
        rtbx_Ctrl.Name = "rtbx_Ctrl";
        rtbx_Ctrl.Size = new Size(507, 148);
        rtbx_Ctrl.TabIndex = 10;
        rtbx_Ctrl.Text = "";
        // 
        // lbl_Control
        // 
        lbl_Control.AutoSize = true;
        lbl_Control.Location = new Point(506, 14);
        lbl_Control.Name = "lbl_Control";
        lbl_Control.Size = new Size(74, 23);
        lbl_Control.TabIndex = 11;
        lbl_Control.Text = "Control";
        // 
        // Btn_GoOffLine
        // 
        Btn_GoOffLine.Location = new Point(506, 248);
        Btn_GoOffLine.Name = "Btn_GoOffLine";
        Btn_GoOffLine.Size = new Size(120, 39);
        Btn_GoOffLine.TabIndex = 12;
        Btn_GoOffLine.Text = "Off-Line";
        Btn_GoOffLine.UseVisualStyleBackColor = true;
        Btn_GoOffLine.Click += Btn_GoOffLine_Click;
        // 
        // Btn_GoOnLine
        // 
        Btn_GoOnLine.Location = new Point(506, 203);
        Btn_GoOnLine.Name = "Btn_GoOnLine";
        Btn_GoOnLine.Size = new Size(120, 39);
        Btn_GoOnLine.TabIndex = 13;
        Btn_GoOnLine.Text = "On-Line";
        Btn_GoOnLine.UseVisualStyleBackColor = true;
        Btn_GoOnLine.Click += Btn_GoOnLine_Click;
        // 
        // Btn_GoLocal
        // 
        Btn_GoLocal.Location = new Point(632, 248);
        Btn_GoLocal.Name = "Btn_GoLocal";
        Btn_GoLocal.Size = new Size(120, 39);
        Btn_GoLocal.TabIndex = 14;
        Btn_GoLocal.Text = "Local";
        Btn_GoLocal.UseVisualStyleBackColor = true;
        Btn_GoLocal.Click += Btn_GoLocal_Click;
        // 
        // Btn_GoRemote
        // 
        Btn_GoRemote.Location = new Point(632, 203);
        Btn_GoRemote.Name = "Btn_GoRemote";
        Btn_GoRemote.Size = new Size(120, 39);
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
        panel1.Location = new Point(55, 56);
        panel1.Name = "panel1";
        panel1.Size = new Size(1160, 320);
        panel1.TabIndex = 16;
        // 
        // Tab_Events
        // 
        Tab_Events.Controls.Add(tabPage1);
        Tab_Events.Controls.Add(tabPage2);
        Tab_Events.Controls.Add(Tab_Variables);
        Tab_Events.Controls.Add(Page_Events);
        Tab_Events.Location = new Point(12, 228);
        Tab_Events.Name = "Tab_Events";
        Tab_Events.SelectedIndex = 0;
        Tab_Events.Size = new Size(1353, 434);
        Tab_Events.TabIndex = 17;
        // 
        // tabPage1
        // 
        tabPage1.BackColor = Color.Gainsboro;
        tabPage1.Controls.Add(panel1);
        tabPage1.Location = new Point(4, 32);
        tabPage1.Name = "tabPage1";
        tabPage1.Padding = new Padding(3);
        tabPage1.Size = new Size(1345, 398);
        tabPage1.TabIndex = 0;
        tabPage1.Text = "tabPage1";
        // 
        // tabPage2
        // 
        tabPage2.BackColor = Color.LightGray;
        tabPage2.Controls.Add(richTextBox1);
        tabPage2.Location = new Point(4, 32);
        tabPage2.Name = "tabPage2";
        tabPage2.Padding = new Padding(3);
        tabPage2.Size = new Size(1345, 398);
        tabPage2.TabIndex = 1;
        tabPage2.Text = "tabPage2";
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
        Tab_Variables.Name = "Tab_Variables";
        Tab_Variables.Size = new Size(1345, 398);
        Tab_Variables.TabIndex = 2;
        Tab_Variables.Text = "Variables";
        Tab_Variables.UseVisualStyleBackColor = true;
        // 
        // Btn_GetSvById
        // 
        Btn_GetSvById.Location = new Point(258, 76);
        Btn_GetSvById.Name = "Btn_GetSvById";
        Btn_GetSvById.Size = new Size(178, 46);
        Btn_GetSvById.TabIndex = 5;
        Btn_GetSvById.Text = "TestGetSV";
        Btn_GetSvById.UseVisualStyleBackColor = true;
        Btn_GetSvById.Click += Btn_GetSvById_Click;
        // 
        // Tbx_InputVarValue
        // 
        Tbx_InputVarValue.Location = new Point(75, 163);
        Tbx_InputVarValue.Name = "Tbx_InputVarValue";
        Tbx_InputVarValue.Size = new Size(145, 30);
        Tbx_InputVarValue.TabIndex = 4;
        // 
        // label1
        // 
        label1.Location = new Point(75, 135);
        label1.Name = "label1";
        label1.Size = new Size(145, 25);
        label1.TabIndex = 3;
        label1.Text = "UpdateValue";
        // 
        // Tbx_InputVid
        // 
        Tbx_InputVid.Location = new Point(75, 85);
        Tbx_InputVid.Name = "Tbx_InputVid";
        Tbx_InputVid.Size = new Size(145, 30);
        Tbx_InputVid.TabIndex = 2;
        // 
        // Lbl_VID
        // 
        Lbl_VID.Location = new Point(75, 57);
        Lbl_VID.Name = "Lbl_VID";
        Lbl_VID.Size = new Size(145, 25);
        Lbl_VID.TabIndex = 1;
        Lbl_VID.Text = "VID";
        // 
        // Btn_UpdateEC
        // 
        Btn_UpdateEC.Location = new Point(258, 154);
        Btn_UpdateEC.Name = "Btn_UpdateEC";
        Btn_UpdateEC.Size = new Size(178, 46);
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
        Page_Events.Name = "Page_Events";
        Page_Events.Size = new Size(1345, 398);
        Page_Events.TabIndex = 3;
        Page_Events.Text = "Events";
        Page_Events.UseVisualStyleBackColor = true;
        // 
        // Btn_TestGetEvents
        // 
        Btn_TestGetEvents.Location = new Point(292, 69);
        Btn_TestGetEvents.Name = "Btn_TestGetEvents";
        Btn_TestGetEvents.Size = new Size(167, 49);
        Btn_TestGetEvents.TabIndex = 1;
        Btn_TestGetEvents.Text = "TestGetEvents";
        Btn_TestGetEvents.UseVisualStyleBackColor = true;
        Btn_TestGetEvents.Click += Btn_TestGetEvents_Click;
        // 
        // Tbx_InputECID
        // 
        Tbx_InputECID.Location = new Point(122, 79);
        Tbx_InputECID.Name = "Tbx_InputECID";
        Tbx_InputECID.Size = new Size(150, 30);
        Tbx_InputECID.TabIndex = 0;
        // 
        // label2
        // 
        label2.Location = new Point(122, 51);
        label2.Name = "label2";
        label2.Size = new Size(145, 25);
        label2.TabIndex = 2;
        label2.Text = "CEID";
        // 
        // Btn_TestSendS6F11
        // 
        Btn_TestSendS6F11.Location = new Point(292, 124);
        Btn_TestSendS6F11.Name = "Btn_TestSendS6F11";
        Btn_TestSendS6F11.Size = new Size(167, 49);
        Btn_TestSendS6F11.TabIndex = 3;
        Btn_TestSendS6F11.Text = "SendS6F11";
        Btn_TestSendS6F11.UseVisualStyleBackColor = true;
        Btn_TestSendS6F11.Click += Btn_TestSendS6F11_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(11F, 23F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1285, 648);
        Controls.Add(Tab_Events);
        Name = "Form1";
        Text = "Form1";
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        Tab_Events.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        tabPage2.ResumeLayout(false);
        Tab_Variables.ResumeLayout(false);
        Tab_Variables.PerformLayout();
        Page_Events.ResumeLayout(false);
        Page_Events.PerformLayout();
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
    private TabPage tabPage1;
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
}
