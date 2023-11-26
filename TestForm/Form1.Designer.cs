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
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(1026, 39);
        button1.Name = "button1";
        button1.Size = new Size(127, 58);
        button1.TabIndex = 0;
        button1.Text = "VarRepo";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // button2
        // 
        button2.Location = new Point(1026, 103);
        button2.Name = "button2";
        button2.Size = new Size(120, 39);
        button2.TabIndex = 1;
        button2.Text = "EnableGem";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // richTextBox1
        // 
        richTextBox1.Location = new Point(59, 298);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(1084, 267);
        richTextBox1.TabIndex = 3;
        richTextBox1.Text = "";
        // 
        // button3
        // 
        button3.Location = new Point(1031, 160);
        button3.Name = "button3";
        button3.Size = new Size(112, 34);
        button3.TabIndex = 5;
        button3.Text = "S1F1";
        button3.UseVisualStyleBackColor = true;
        button3.Click += button3_Click;
        // 
        // rtbx_HSMS
        // 
        rtbx_HSMS.Location = new Point(64, 54);
        rtbx_HSMS.Name = "rtbx_HSMS";
        rtbx_HSMS.Size = new Size(144, 148);
        rtbx_HSMS.TabIndex = 4;
        rtbx_HSMS.Text = "";
        // 
        // lbl_HSMS
        // 
        lbl_HSMS.AutoSize = true;
        lbl_HSMS.Location = new Point(64, 19);
        lbl_HSMS.Name = "lbl_HSMS";
        lbl_HSMS.Size = new Size(61, 23);
        lbl_HSMS.TabIndex = 6;
        lbl_HSMS.Text = "HSMS";
        // 
        // lbl_Communication
        // 
        lbl_Communication.AutoSize = true;
        lbl_Communication.Location = new Point(214, 19);
        lbl_Communication.Name = "lbl_Communication";
        lbl_Communication.Size = new Size(67, 23);
        lbl_Communication.TabIndex = 8;
        lbl_Communication.Text = "Comm";
        // 
        // rtbx_Comm
        // 
        rtbx_Comm.Location = new Point(217, 54);
        rtbx_Comm.Name = "rtbx_Comm";
        rtbx_Comm.Size = new Size(371, 148);
        rtbx_Comm.TabIndex = 9;
        rtbx_Comm.Text = "";
        // 
        // rtbx_Ctrl
        // 
        rtbx_Ctrl.Location = new Point(594, 54);
        rtbx_Ctrl.Name = "rtbx_Ctrl";
        rtbx_Ctrl.Size = new Size(426, 148);
        rtbx_Ctrl.TabIndex = 10;
        rtbx_Ctrl.Text = "";
        // 
        // lbl_Control
        // 
        lbl_Control.AutoSize = true;
        lbl_Control.Location = new Point(594, 19);
        lbl_Control.Name = "lbl_Control";
        lbl_Control.Size = new Size(74, 23);
        lbl_Control.TabIndex = 11;
        lbl_Control.Text = "Control";
        // 
        // Btn_GoOffLine
        // 
        Btn_GoOffLine.Location = new Point(594, 208);
        Btn_GoOffLine.Name = "Btn_GoOffLine";
        Btn_GoOffLine.Size = new Size(120, 39);
        Btn_GoOffLine.TabIndex = 12;
        Btn_GoOffLine.Text = "Off-Line";
        Btn_GoOffLine.UseVisualStyleBackColor = true;
        Btn_GoOffLine.Click += Btn_GoOffLine_Click;
        // 
        // Btn_GoOnLine
        // 
        Btn_GoOnLine.Location = new Point(594, 253);
        Btn_GoOnLine.Name = "Btn_GoOnLine";
        Btn_GoOnLine.Size = new Size(120, 39);
        Btn_GoOnLine.TabIndex = 13;
        Btn_GoOnLine.Text = "On-Line";
        Btn_GoOnLine.UseVisualStyleBackColor = true;
        Btn_GoOnLine.Click += Btn_GoOnLine_Click;
        // 
        // Btn_GoLocal
        // 
        Btn_GoLocal.Location = new Point(720, 208);
        Btn_GoLocal.Name = "Btn_GoLocal";
        Btn_GoLocal.Size = new Size(120, 39);
        Btn_GoLocal.TabIndex = 14;
        Btn_GoLocal.Text = "Local";
        Btn_GoLocal.UseVisualStyleBackColor = true;
        Btn_GoLocal.Click += Btn_GoLocal_Click;
        // 
        // Btn_GoRemote
        // 
        Btn_GoRemote.Location = new Point(720, 253);
        Btn_GoRemote.Name = "Btn_GoRemote";
        Btn_GoRemote.Size = new Size(120, 39);
        Btn_GoRemote.TabIndex = 15;
        Btn_GoRemote.Text = "Remote";
        Btn_GoRemote.UseVisualStyleBackColor = true;
        Btn_GoRemote.Click += Btn_GoRemote_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(11F, 23F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1205, 648);
        Controls.Add(Btn_GoRemote);
        Controls.Add(Btn_GoLocal);
        Controls.Add(Btn_GoOnLine);
        Controls.Add(Btn_GoOffLine);
        Controls.Add(lbl_Control);
        Controls.Add(rtbx_Ctrl);
        Controls.Add(rtbx_Comm);
        Controls.Add(lbl_Communication);
        Controls.Add(lbl_HSMS);
        Controls.Add(button3);
        Controls.Add(rtbx_HSMS);
        Controls.Add(richTextBox1);
        Controls.Add(button2);
        Controls.Add(button1);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
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
}
