namespace 恒温测试机.UI
{
    partial class FormConnectValueSetting
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.shutdownBtn = new System.Windows.Forms.Button();
            this.autoRunBtn = new System.Windows.Forms.Button();
            this.backOrignBtn = new System.Windows.Forms.Button();
            this.orignBtn = new System.Windows.Forms.Button();
            this.angelLb = new System.Windows.Forms.Label();
            this.radioLb = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.noForwardBtn = new System.Windows.Forms.Button();
            this.forwardBtn = new System.Windows.Forms.Button();
            this.electCbx = new System.Windows.Forms.ComboBox();
            this.powerBtn = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 108);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "变频器通讯";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 17);
            this.label2.TabIndex = 34;
            this.label2.Text = "值";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(58, 78);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(73, 21);
            this.textBox3.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 32;
            this.label1.Text = "站号";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(164, 28);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(74, 29);
            this.button2.TabIndex = 31;
            this.button2.Text = "读取";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ReadBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(13, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 17);
            this.label5.TabIndex = 30;
            this.label5.Text = "地址";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(58, 24);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(73, 21);
            this.textBox2.TabIndex = 29;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(164, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 29);
            this.button1.TabIndex = 28;
            this.button1.Text = "写入";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.WriteBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.shutdownBtn);
            this.groupBox2.Controls.Add(this.autoRunBtn);
            this.groupBox2.Controls.Add(this.backOrignBtn);
            this.groupBox2.Controls.Add(this.orignBtn);
            this.groupBox2.Controls.Add(this.angelLb);
            this.groupBox2.Controls.Add(this.radioLb);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.noForwardBtn);
            this.groupBox2.Controls.Add(this.forwardBtn);
            this.groupBox2.Controls.Add(this.electCbx);
            this.groupBox2.Controls.Add(this.powerBtn);
            this.groupBox2.Location = new System.Drawing.Point(12, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 185);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "电机通讯";
            // 
            // shutdownBtn
            // 
            this.shutdownBtn.Location = new System.Drawing.Point(202, 145);
            this.shutdownBtn.Name = "shutdownBtn";
            this.shutdownBtn.Size = new System.Drawing.Size(74, 29);
            this.shutdownBtn.TabIndex = 41;
            this.shutdownBtn.Text = "紧急停止";
            this.shutdownBtn.UseVisualStyleBackColor = true;
            this.shutdownBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ShutdownBtn_MouseDown);
            this.shutdownBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ShutdownBtn_MouseUp);
            // 
            // autoRunBtn
            // 
            this.autoRunBtn.Location = new System.Drawing.Point(122, 145);
            this.autoRunBtn.Name = "autoRunBtn";
            this.autoRunBtn.Size = new System.Drawing.Size(74, 29);
            this.autoRunBtn.TabIndex = 40;
            this.autoRunBtn.Text = "自动运行";
            this.autoRunBtn.UseVisualStyleBackColor = true;
            this.autoRunBtn.Click += new System.EventHandler(this.AutoRunBtn_Click);
            // 
            // backOrignBtn
            // 
            this.backOrignBtn.Location = new System.Drawing.Point(202, 110);
            this.backOrignBtn.Name = "backOrignBtn";
            this.backOrignBtn.Size = new System.Drawing.Size(74, 29);
            this.backOrignBtn.TabIndex = 39;
            this.backOrignBtn.Text = "回原点";
            this.backOrignBtn.UseVisualStyleBackColor = true;
            this.backOrignBtn.Click += new System.EventHandler(this.BackOrignBtn_Click);
            // 
            // orignBtn
            // 
            this.orignBtn.Location = new System.Drawing.Point(122, 110);
            this.orignBtn.Name = "orignBtn";
            this.orignBtn.Size = new System.Drawing.Size(74, 29);
            this.orignBtn.TabIndex = 38;
            this.orignBtn.Text = "原点";
            this.orignBtn.UseVisualStyleBackColor = true;
            this.orignBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OrignBtn_MouseDown);
            this.orignBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OrignBtn_MouseUp);
            // 
            // angelLb
            // 
            this.angelLb.AutoSize = true;
            this.angelLb.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.angelLb.Location = new System.Drawing.Point(81, 116);
            this.angelLb.Name = "angelLb";
            this.angelLb.Size = new System.Drawing.Size(15, 17);
            this.angelLb.TabIndex = 37;
            this.angelLb.Text = "0";
            // 
            // radioLb
            // 
            this.radioLb.AutoSize = true;
            this.radioLb.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioLb.Location = new System.Drawing.Point(81, 87);
            this.radioLb.Name = "radioLb";
            this.radioLb.Size = new System.Drawing.Size(15, 17);
            this.radioLb.TabIndex = 36;
            this.radioLb.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(6, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 35;
            this.label4.Text = "角度(度)：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(6, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 34;
            this.label3.Text = "转速(度/秒)：";
            // 
            // noForwardBtn
            // 
            this.noForwardBtn.Location = new System.Drawing.Point(202, 75);
            this.noForwardBtn.Name = "noForwardBtn";
            this.noForwardBtn.Size = new System.Drawing.Size(74, 29);
            this.noForwardBtn.TabIndex = 33;
            this.noForwardBtn.Text = "反传";
            this.noForwardBtn.UseVisualStyleBackColor = true;
            this.noForwardBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NoForwardBtn_MouseDown);
            this.noForwardBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NoForwardBtn_MouseUp);
            // 
            // forwardBtn
            // 
            this.forwardBtn.Location = new System.Drawing.Point(122, 75);
            this.forwardBtn.Name = "forwardBtn";
            this.forwardBtn.Size = new System.Drawing.Size(74, 29);
            this.forwardBtn.TabIndex = 32;
            this.forwardBtn.Text = "正传";
            this.forwardBtn.UseVisualStyleBackColor = true;
            this.forwardBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ForwardBtn_MouseDown);
            this.forwardBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ForwardBtn_MouseUp);
            // 
            // electCbx
            // 
            this.electCbx.FormattingEnabled = true;
            this.electCbx.Items.AddRange(new object[] {
            "按键恒温电机",
            "按键流量电机"});
            this.electCbx.Location = new System.Drawing.Point(6, 45);
            this.electCbx.Name = "electCbx";
            this.electCbx.Size = new System.Drawing.Size(110, 20);
            this.electCbx.TabIndex = 30;
            this.electCbx.Text = "请选择伺服电机";
            this.electCbx.TextChanged += new System.EventHandler(this.ElectCbx_TextChanged);
            // 
            // powerBtn
            // 
            this.powerBtn.BackColor = System.Drawing.SystemColors.Control;
            this.powerBtn.Location = new System.Drawing.Point(122, 40);
            this.powerBtn.Name = "powerBtn";
            this.powerBtn.Size = new System.Drawing.Size(74, 29);
            this.powerBtn.TabIndex = 29;
            this.powerBtn.Text = "伺服开关";
            this.powerBtn.UseVisualStyleBackColor = false;
            this.powerBtn.Click += new System.EventHandler(this.PowerSwitchBtn_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBox1.Location = new System.Drawing.Point(58, 52);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(73, 20);
            this.comboBox1.TabIndex = 35;
            this.comboBox1.Text = "1";
            // 
            // FormConnectValueSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 344);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormConnectValueSetting";
            this.ShowIcon = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ComboBox electCbx;
        private System.Windows.Forms.Button powerBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button noForwardBtn;
        private System.Windows.Forms.Button forwardBtn;
        private System.Windows.Forms.Label angelLb;
        private System.Windows.Forms.Label radioLb;
        private System.Windows.Forms.Button orignBtn;
        private System.Windows.Forms.Button backOrignBtn;
        private System.Windows.Forms.Button shutdownBtn;
        private System.Windows.Forms.Button autoRunBtn;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}