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
            this.hslCurveHistory1.Size = new System.Drawing.Size(1543, 721);
            this.hslCurveHistory1.TabIndex = 3;
            this.hslCurveHistory1.Text = "hslCurveHistory1";
            this.hslCurveHistory1.ValueMaxLeft = 10F;
            this.hslCurveHistory1.ValueMaxRight = 10F;
            this.hslCurveHistory1.ValueSegment = 20;
            // 
            // FormPressureCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1569, 748);
            this.Controls.Add(this.hslCurveHistory1);
            this.Name = "FormPressureCurve";
            this.Text = "FormPressureCurve";
            this.Load += new System.EventHandler(this.FormPressureCurve_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private HslControls.HslCurveHistory hslCurveHistory1;
    }
}