namespace ToolForm;

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
        Btn_InsertVariableTable = new Button();
        SuspendLayout();
        // 
        // Btn_InsertVariableTable
        // 
        Btn_InsertVariableTable.Location = new Point(121, 132);
        Btn_InsertVariableTable.Name = "Btn_InsertVariableTable";
        Btn_InsertVariableTable.Size = new Size(221, 51);
        Btn_InsertVariableTable.TabIndex = 1;
        Btn_InsertVariableTable.Text = "InsertVarTable";
        Btn_InsertVariableTable.UseVisualStyleBackColor = true;
        Btn_InsertVariableTable.Click += Btn_InsertVariableTable_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(11F, 23F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(Btn_InsertVariableTable);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
    }

    #endregion

    private Button Btn_InsertVariableTable;
}
