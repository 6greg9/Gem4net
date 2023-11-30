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
        tabControl1 = new TabControl();
        tabPage1 = new TabPage();
        tabPage2 = new TabPage();
        Tab_Variables = new TabPage();
        button4 = new Button();
        panel1.SuspendLayout();
        tabControl1.SuspendLayout();
        tabPage1.SuspendLayout();
        tabPage2.SuspendLayout();
        Tab_Variables.SuspendLayout();
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
        rtbx_Comm.Size = new Size(371, 148);
        rtbx_Comm.TabIndex = 9;
        rtbx_Comm.Text = "";
        // 
        // rtbx_Ctrl
        // 
        rtbx_Ctrl.Location = new Point(566, 49);
        rtbx_Ctrl.Name = "rtbx_Ctrl";
        rtbx_Ctrl.Size = new Size(426, 148);
        rtbx_Ctrl.TabIndex = 10;
        rtbx_Ctrl.Text = "";
        // 
        // lbl_Control
        // 
        lbl_Control.AutoSize = true;
        lbl_Control.Location = new Point(561, 14);
        lbl_Control.Name = "lbl_Control";
        lbl_Control.Size = new Size(74, 23);
        lbl_Control.TabIndex = 11;
        lbl_Control.Text = "Control";
        // 
        // Btn_GoOffLine
        // 
        Btn_GoOffLine.Location = new Point(566, 248);
        Btn_GoOffLine.Name = "Btn_GoOffLine";
        Btn_GoOffLine.Size = new Size(120, 39);
        Btn_GoOffLine.TabIndex = 12;
        Btn_GoOffLine.Text = "Off-Line";
        Btn_GoOffLine.UseVisualStyleBackColor = true;
        Btn_GoOffLine.Click += Btn_GoOffLine_Click;
        // 
        // Btn_GoOnLine
        // 
        Btn_GoOnLine.Location = new Point(566, 203);
        Btn_GoOnLine.Name = "Btn_GoOnLine";
        Btn_GoOnLine.Size = new Size(120, 39);
        Btn_GoOnLine.TabIndex = 13;
        Btn_GoOnLine.Text = "On-Line";
        Btn_GoOnLine.UseVisualStyleBackColor = true;
        Btn_GoOnLine.Click += Btn_GoOnLine_Click;
        // 
        // Btn_GoLocal
        // 
        Btn_GoLocal.Location = new Point(692, 248);
        Btn_GoLocal.Name = "Btn_GoLocal";
        Btn_GoLocal.Size = new Size(120, 39);
        Btn_GoLocal.TabIndex = 14;
        Btn_GoLocal.Text = "Local";
        Btn_GoLocal.UseVisualStyleBackColor = true;
        Btn_GoLocal.Click += Btn_GoLocal_Click;
        // 
        // Btn_GoRemote
        // 
        Btn_GoRemote.Location = new Point(692, 203);
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
        // tabControl1
        // 
        tabControl1.Controls.Add(tabPage1);
        tabControl1.Controls.Add(tabPage2);
        tabControl1.Controls.Add(Tab_Variables);
        tabControl1.Location = new Point(12, 228);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new Size(1353, 434);
        tabControl1.TabIndex = 17;
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
        Tab_Variables.Controls.Add(button4);
        Tab_Variables.Location = new Point(4, 32);
        Tab_Variables.Name = "Tab_Variables";
        Tab_Variables.Size = new Size(1345, 398);
        Tab_Variables.TabIndex = 2;
        Tab_Variables.Text = "Variables";
        Tab_Variables.UseVisualStyleBackColor = true;
        // 
        // button4
        // 
        button4.Location = new Point(448, 108);
        button4.Name = "button4";
        button4.Size = new Size(112, 34);
        button4.TabIndex = 0;
        button4.Text = "button4";
        button4.UseVisualStyleBackColor = true;
        button4.Click += button4_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(11F, 23F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1285, 648);
        Controls.Add(tabControl1);
        Name = "Form1";
        Text = "Form1";
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        tabControl1.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        tabPage2.ResumeLayout(false);
        Tab_Variables.ResumeLayout(false);
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
    private TabControl tabControl1;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private TabPage Tab_Variables;
    private Button button4;
}
