namespace Client
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
            this.active_processes_groupBox = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.active_processes_textBox = new System.Windows.Forms.RichTextBox();
            this.active_processes_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // active_processes_groupBox
            // 
            this.active_processes_groupBox.Controls.Add(this.active_processes_textBox);
            this.active_processes_groupBox.Location = new System.Drawing.Point(219, 12);
            this.active_processes_groupBox.Name = "active_processes_groupBox";
            this.active_processes_groupBox.Size = new System.Drawing.Size(326, 701);
            this.active_processes_groupBox.TabIndex = 0;
            this.active_processes_groupBox.TabStop = false;
            this.active_processes_groupBox.Text = "Active Processes";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(591, 188);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // active_processes_textBox
            // 
            this.active_processes_textBox.Location = new System.Drawing.Point(6, 21);
            this.active_processes_textBox.Name = "active_processes_textBox";
            this.active_processes_textBox.ReadOnly = true;
            this.active_processes_textBox.Size = new System.Drawing.Size(314, 674);
            this.active_processes_textBox.TabIndex = 166;
            this.active_processes_textBox.TabStop = false;
            this.active_processes_textBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 725);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.active_processes_groupBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.active_processes_groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox active_processes_groupBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox active_processes_textBox;
    }
}

