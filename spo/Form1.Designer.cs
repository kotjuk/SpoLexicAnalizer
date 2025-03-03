namespace spo
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.AnalyzeButton = new System.Windows.Forms.Button();
            this.TreeButton = new System.Windows.Forms.Button();
            this.syntaxTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();

            // inputTextBox
            this.inputTextBox.Location = new System.Drawing.Point(12, 12);
            this.inputTextBox.Multiline = true;
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(360, 100);
            this.inputTextBox.TabIndex = 0;

            // AnalyzeButton
            this.AnalyzeButton.Location = new System.Drawing.Point(12, 120);
            this.AnalyzeButton.Name = "AnalyzeButton";
            this.AnalyzeButton.Size = new System.Drawing.Size(75, 23);
            this.AnalyzeButton.TabIndex = 1;
            this.AnalyzeButton.Text = "Анализ";
            this.AnalyzeButton.UseVisualStyleBackColor = true;
            this.AnalyzeButton.Click += new System.EventHandler(this.AnalyzeButton_Click);

            // TreeButton
            this.TreeButton.Location = new System.Drawing.Point(100, 120);
            this.TreeButton.Name = "TreeButton";
            this.TreeButton.Size = new System.Drawing.Size(100, 23);
            this.TreeButton.TabIndex = 2;
            //this.TreeButton.Text = "Синтаксическое дерево";
            this.TreeButton.UseVisualStyleBackColor = true;
            //this.TreeButton.Click += new System.EventHandler(this.TreeButton_Click);

            // syntaxTreeView
            this.syntaxTreeView.Location = new System.Drawing.Point(12, 150);
            this.syntaxTreeView.Name = "syntaxTreeView";
            this.syntaxTreeView.Size = new System.Drawing.Size(360, 200);
            this.syntaxTreeView.TabIndex = 3;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.syntaxTreeView);
            this.Controls.Add(this.TreeButton);
            this.Controls.Add(this.AnalyzeButton);
            this.Controls.Add(this.inputTextBox);
            this.Name = "Form1";
            this.Text = "Лексический и синтаксический анализатор";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Button AnalyzeButton;
        private System.Windows.Forms.Button TreeButton;
        private System.Windows.Forms.TreeView syntaxTreeView;
    }
}