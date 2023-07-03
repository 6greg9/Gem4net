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
        richTextBox2 = new RichTextBox();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(936, 63);
        button1.Name = "button1";
        button1.Size = new Size(127, 58);
        button1.TabIndex = 0;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // button2
        // 
        button2.Location = new Point(936, 163);
        button2.Name = "button2";
        button2.Size = new Size(120, 39);
        button2.TabIndex = 1;
        button2.Text = "button2";
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
        // richTextBox2
        // 
        richTextBox2.Location = new Point(64, 35);
        richTextBox2.Name = "richTextBox2";
        richTextBox2.Size = new Size(841, 167);
        richTextBox2.TabIndex = 4;
        richTextBox2.Text = "";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(11F, 23F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1205, 584);
        Controls.Add(richTextBox2);
        Controls.Add(richTextBox1);
        Controls.Add(button2);
        Controls.Add(button1);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
    }

    #endregion

    private Button button1;
    private Button button2;
    private RichTextBox richTextBox1;
    private RichTextBox richTextBox2;
}
