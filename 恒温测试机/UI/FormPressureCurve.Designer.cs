namespace 恒温测试机
{
    partial class FormPressureCurve
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
            this.hslCurveHistory1 = new HslControls.HslCurveHistory();
            this.hslButton1 = new HslControls.HslButton();
            this.hslButton3 = new HslControls.HslButton();
            this.hslButton4 = new HslControls.HslButton();
            this.SuspendLayout();
            // 
            // hslCurveHistory1
            // 
            this.hslCurveHistory1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hslCurveHistory1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.hslCurveHistory1.CurveNameWidth = 120;
            this.hslCurveHistory1.DashCoordinateColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.hslCurveHistory1.DataTipWidth = 200;
            this.hslCurveHistory1.Location = new System.Drawing.Point(21, 23);
            this.hslCurveHistory1.MarkTextColor = System.Drawing.Color.Yellow;
            this.hslCurveHistory1.Name = "hslCurveHistory1";
            this.hslCurveHistory1.Size = new System.Drawing.Size(1536, 653);
            this.hslCurveHistory1.TabIndex = 3;
            this.hslCurveHistory1.Text = "hslCurveHistory1";
            this.hslCurveHistory1.ValueMaxLeft = 10F;
            this.hslCurveHistory1.ValueMaxRight = 10F;
            this.hslCurveHistory1.ValueSegment = 20;
            // 
            // hslButton1
            // 
            this.hslButton1.ActiveColor = System.Drawing.Color.Cyan;
            this.hslButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hslButton1.CustomerInformation = null;
            this.hslButton1.EnableColor = System.Drawing.Color.DodgerBlue;
            this.hslButton1.Location = new System.Drawing.Point(21, 692);
            this.hslButton1.Name = "hslButton1";
            this.hslButton1.OriginalColor = System.Drawing.Color.PaleGreen;
            this.hslButton1.Size = new System.Drawing.Size(78, 32);
            this.hslButton1.TabIndex = 4;
            this.hslButton1.Text = "保存图像";
            this.hslButton1.Click += new System.EventHandler(this.HslButton1_Click);
            // 
            // hslButton3
            // 
            this.hslButton3.ActiveColor = System.Drawing.Color.Cyan;
            this.hslButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hslButton3.CustomerInformation = null;
            this.hslButton3.EnableColor = System.Drawing.Color.DodgerBlue;
            this.hslButton3.Location = new System.Drawing.Point(259, 692);
            this.hslButton3.Name = "hslButton3";
            this.hslButton3.OriginalColor = System.Drawing.Color.PaleGreen;
            this.hslButton3.Size = new System.Drawing.Size(78, 32);
            this.hslButton3.TabIndex = 9;
            this.hslButton3.Text = "放大+";
            this.hslButton3.Click += new System.EventHandler(this.HslButton3_Click);
            // 
            // hslButton4
            // 
            this.hslButton4.ActiveColor = System.Drawing.Color.Cyan;
            this.hslButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hslButton4.CustomerInformation = null;
            this.hslButton4.EnableColor = System.Drawing.Color.DodgerBlue;
            this.hslButton4.Location = new System.Drawing.Point(156, 692);
            this.hslButton4.Name = "hslButton4";
            this.hslButton4.OriginalColor = System.Drawing.Color.PaleGreen;
            this.hslButton4.Size = new System.Drawing.Size(78, 32);
            this.hslButton4.TabIndex = 8;
            this.hslButton4.Text = "缩小-";
            this.hslButton4.Click += new System.EventHandler(this.HslButton4_Click);
            // 
            // FormPressureCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1569, 748);
            this.Controls.Add(this.hslButton3);
            this.Controls.Add(this.hslButton4);
            this.Controls.Add(this.hslButton1);
            this.Controls.Add(this.hslCurveHistory1);
            this.Name = "FormPressureCurve";
            this.Text = "FormPressureCurve";
            this.Load += new System.EventHandler(this.FormPressureCurve_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private HslControls.HslCurveHistory hslCurveHistory1;
        private HslControls.HslButton hslButton1;
        private HslControls.HslButton hslButton3;
        private HslControls.HslButton hslButton4;
    }
}