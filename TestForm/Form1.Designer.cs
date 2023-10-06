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
        richTextBox2 = new RichTextBox();
        richTextBox3 = new RichTextBox();
        lbl_Control = new Label();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(936, 35);
        button1.Name = "button1";
        button1.Size = new Size(127, 58);
        button1.TabIndex = 0;
        button1.Text = "VarRepo";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // button2
        // 
        button2.Location = new Point(936, 99);
        button2.Name = "button2";
        button2.Size = new Size(120, 39);
        button2.TabIndex = 1;
        button2.Text = "EnableGem";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // richTextBox1
        // 
        richTextBox1.Location = new Point(59, 258);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(1021, 267);
        richTextBox1.TabIndex = 3;
        richTextBox1.Text = "";
        // 
        // button3
        // 
        button3.Location = new Point(941, 156);
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
        // richTextBox2
        // 
        richTextBox2.Location = new Point(217, 54);
        richTextBox2.Name = "richTextBox2";
        richTextBox2.Size = new Size(144, 148);
        richTextBox2.TabIndex = 9;
        richTextBox2.Text = "";
        // 
        // richTextBox3
        // 
        richTextBox3.Location = new Point(367, 54);
        richTextBox3.Name = "richTextBox3";
        richTextBox3.Size = new Size(144, 148);
        richTextBox3.TabIndex = 10;
        richTextBox3.Text = "";
        // 
        // lbl_Control
        // 
        lbl_Control.AutoSize = true;
        lbl_Control.Location = new Point(367, 19);
        lbl_Control.Name = "lbl_Control";
        lbl_Control.Size = new Size(74, 23);
        lbl_Control.TabIndex = 11;
        lbl_Control.Text = "Control";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(11F, 23F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1205, 584);
        Controls.Add(lbl_Control);
        Controls.Add(richTextBox3);
        Controls.Add(richTextBox2);
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
    private RichTextBox richTextBox2;
    private RichTextBox richTextBox3;
    private Label lbl_Control;
}
