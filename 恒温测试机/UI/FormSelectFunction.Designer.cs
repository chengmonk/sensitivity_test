namespace 恒温测试机
{
    partial class FormSelectFunction
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.hslButton1 = new HslControls.HslButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(301, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "测试功能模块选择";
            // 
            // hslButton1
            // 
            this.hslButton1.CustomerInformation = null;
            this.hslButton1.Location = new System.Drawing.Point(269, 156);
            this.hslButton1.Name = "hslButton1";
            this.hslButton1.Size = new System.Drawing.Size(114, 46);
            this.hslButton1.TabIndex = 1;
            this.hslButton1.Text = "hslButton1";
            this.hslButton1.Click += new System.EventHandler(this.HslButton1_Click);
            // 
            // FormSelectFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 626);
            this.Controls.Add(this.hslButton1);
            this.Controls.Add(this.label1);
            this.Name = "FormSelectFunction";
            this.Text = "FormSelectFunction";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private HslControls.HslButton hslButton1;
    }
}