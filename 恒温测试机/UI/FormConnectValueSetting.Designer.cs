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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
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
            this.powerBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.button10);
            this.groupBox2.Controls.Add(this.textBox1);
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
            this.groupBox2.Controls.Add(this.powerBtn);
            this.groupBox2.Location = new System.Drawing.Point(12, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 232);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "旋转电机通讯";
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
            this.angelLb.Location = new System.Drawing.Point(81, 71);
            this.angelLb.Name = "angelLb";
            this.angelLb.Size = new System.Drawing.Size(15, 17);
            this.angelLb.TabIndex = 37;
            this.angelLb.Text = "0";
            // 
            // radioLb
            // 
            this.radioLb.AutoSize = true;
            this.radioLb.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioLb.Location = new System.Drawing.Point(81, 42);
            this.radioLb.Name = "radioLb";
            this.radioLb.Size = new System.Drawing.Size(15, 17);
            this.radioLb.TabIndex = 36;
            this.radioLb.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(6, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 35;
            this.label4.Text = "角度(度)：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(6, 42);
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.button11);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.button9);
            this.groupBox3.Location = new System.Drawing.Point(10, 401);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(314, 229);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "升降电机通讯";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(202, 145);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(74, 29);
            this.button3.TabIndex = 41;
            this.button3.Text = "紧急停止";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(122, 145);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(74, 29);
            this.button4.TabIndex = 40;
            this.button4.Text = "自动运行";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(202, 110);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(74, 29);
            this.button5.TabIndex = 39;
            this.button5.Text = "回原点";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(122, 110);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(74, 29);
            this.button6.TabIndex = 38;
            this.button6.Text = "原点";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(81, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 17);
            this.label6.TabIndex = 37;
            this.label6.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(81, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 17);
            this.label7.TabIndex = 36;
            this.label7.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(6, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 17);
            this.label8.TabIndex = 35;
            this.label8.Text = "角度(度)：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(6, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 17);
            this.label9.TabIndex = 34;
            this.label9.Text = "转速(度/秒)：";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(202, 75);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(74, 29);
            this.button7.TabIndex = 33;
            this.button7.Text = "反传";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(122, 75);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(74, 29);
            this.button8.TabIndex = 32;
            this.button8.Text = "正传";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.SystemColors.Control;
            this.button9.Location = new System.Drawing.Point(122, 40);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(74, 29);
            this.button9.TabIndex = 29;
            this.button9.Text = "伺服开关";
            this.button9.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(123, 185);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(73, 21);
            this.textBox1.TabIndex = 42;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(202, 180);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(74, 29);
            this.button10.TabIndex = 43;
            this.button10.Text = "写入";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(6, 185);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 17);
            this.label10.TabIndex = 44;
            this.label10.Text = "转速设置：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(8, 185);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 17);
            this.label11.TabIndex = 47;
            this.label11.Text = "转速设置：";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(202, 180);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(74, 29);
            this.button11.TabIndex = 46;
            this.button11.Text = "写入";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(123, 185);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(73, 21);
            this.textBox4.TabIndex = 45;
            // 
            // FormConnectValueSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 642);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormConnectValueSetting";
            this.ShowIcon = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.TextBox textBox4;
    }
}