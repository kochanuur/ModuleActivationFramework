namespace AppMain
{
    partial class MainWindow
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
            labelThemeParkName = new Label();
            comboBoxThemeParkName = new ComboBox();
            buttonGetWaitTime = new Button();
            textBoxWaitTime = new TextBox();
            SuspendLayout();
            // 
            // labelThemeParkName
            // 
            labelThemeParkName.AutoSize = true;
            labelThemeParkName.Location = new Point(34, 19);
            labelThemeParkName.Name = "labelThemeParkName";
            labelThemeParkName.Size = new Size(102, 15);
            labelThemeParkName.TabIndex = 0;
            labelThemeParkName.Text = "テーマパークを選択 : ";
            // 
            // comboBoxThemeParkName
            // 
            comboBoxThemeParkName.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxThemeParkName.FormattingEnabled = true;
            comboBoxThemeParkName.Location = new Point(138, 17);
            comboBoxThemeParkName.Name = "comboBoxThemeParkName";
            comboBoxThemeParkName.Size = new Size(170, 23);
            comboBoxThemeParkName.TabIndex = 1;
            // 
            // buttonGetWaitTime
            // 
            buttonGetWaitTime.Location = new Point(34, 79);
            buttonGetWaitTime.Name = "buttonGetWaitTime";
            buttonGetWaitTime.Size = new Size(274, 51);
            buttonGetWaitTime.TabIndex = 2;
            buttonGetWaitTime.Text = "待ち時間を取得";
            buttonGetWaitTime.UseVisualStyleBackColor = true;
            buttonGetWaitTime.Click += buttonGetWaitTime_Click;
            // 
            // textBoxWaitTime
            // 
            textBoxWaitTime.Location = new Point(347, 19);
            textBoxWaitTime.Multiline = true;
            textBoxWaitTime.Name = "textBoxWaitTime";
            textBoxWaitTime.ReadOnly = true;
            textBoxWaitTime.ScrollBars = ScrollBars.Vertical;
            textBoxWaitTime.Size = new Size(411, 400);
            textBoxWaitTime.TabIndex = 3;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBoxWaitTime);
            Controls.Add(buttonGetWaitTime);
            Controls.Add(comboBoxThemeParkName);
            Controls.Add(labelThemeParkName);
            Name = "MainWindow";
            Text = "Form1";
            Shown += MainWindow_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelThemeParkName;
        private ComboBox comboBoxThemeParkName;
        private Button buttonGetWaitTime;
        private TextBox textBoxWaitTime;
    }
}
