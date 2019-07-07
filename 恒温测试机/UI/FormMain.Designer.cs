namespace 恒温测试机.UI
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.leftPanel = new System.Windows.Forms.Panel();
            this.left3 = new System.Windows.Forms.Panel();
            this.waterBoxGbx = new System.Windows.Forms.GroupBox();
            this.Temp5Status = new System.Windows.Forms.Label();
            this.Temp4Status = new System.Windows.Forms.Label();
            this.Temp3Status = new System.Windows.Forms.Label();
            this.Temp2Status = new System.Windows.Forms.Label();
            this.Temp1Status = new System.Windows.Forms.Label();
            this.hslWaterBox5 = new HslControls.HslWaterBox();
            this.hslWaterBox3 = new HslControls.HslWaterBox();
            this.hslWaterBox4 = new HslControls.HslWaterBox();
            this.hslWaterBox2 = new HslControls.HslWaterBox();
            this.hslWaterBox1 = new HslControls.HslWaterBox();
            this.left2 = new System.Windows.Forms.Panel();
            this.dataGbx = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.QhShow = new System.Windows.Forms.Label();
            this.QcShow = new System.Windows.Forms.Label();
            this.QmShow = new System.Windows.Forms.Label();
            this.PhShow = new System.Windows.Forms.Label();
            this.PcShow = new System.Windows.Forms.Label();
            this.PmShow = new System.Windows.Forms.Label();
            this.ThShow = new System.Windows.Forms.Label();
            this.TcShow = new System.Windows.Forms.Label();
            this.TmShow = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.left1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.standardCbx = new System.Windows.Forms.ComboBox();
            this.ICON = new System.Windows.Forms.Label();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.right3 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.systemInfoTb = new System.Windows.Forms.TextBox();
            this.right4 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.setParamBtn = new HslControls.HslButton();
            this.hslButton12 = new HslControls.HslButton();
            this.hslButton11 = new HslControls.HslButton();
            this.hslButton10 = new HslControls.HslButton();
            this.hslButton9 = new HslControls.HslButton();
            this.autoRunBtn = new HslControls.HslButton();
            this.stopBtn = new HslControls.HslButton();
            this.clearDataBtn = new HslControls.HslButton();
            this.hslButton7 = new HslControls.HslButton();
            this.hslButton8 = new HslControls.HslButton();
            this.startBtn = new HslControls.HslButton();
            this.saveDataBtn = new HslControls.HslButton();
            this.right1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.FlowTestRbt = new System.Windows.Forms.RadioButton();
            this.tmpTestRbt = new System.Windows.Forms.RadioButton();
            this.coolTestRbt = new System.Windows.Forms.RadioButton();
            this.pressureTestRbt = new System.Windows.Forms.RadioButton();
            this.safeTestRbt = new System.Windows.Forms.RadioButton();
            this.hslCurveHistory1 = new HslControls.HslCurveHistory();
            this.centerPanel = new System.Windows.Forms.Panel();
            this.center1 = new System.Windows.Forms.Panel();
            this.Title = new System.Windows.Forms.Label();
            this.m_instantAoCtrl = new Automation.BDaq.InstantAoCtrl(this.components);
            this.instantDoCtrl1 = new Automation.BDaq.InstantDoCtrl(this.components);
            this.instantDiCtrl1 = new Automation.BDaq.InstantDiCtrl(this.components);
            this.waveformAiCtrl1 = new Automation.BDaq.WaveformAiCtrl(this.components);
            this.hslCurve1 = new HslControls.HslCurve();
            this.leftPanel.SuspendLayout();
            this.left3.SuspendLayout();
            this.waterBoxGbx.SuspendLayout();
            this.left2.SuspendLayout();
            this.dataGbx.SuspendLayout();
            this.left1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.right3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.right4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.right1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.centerPanel.SuspendLayout();
            this.center1.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.left3);
            this.leftPanel.Controls.Add(this.left2);
            this.leftPanel.Controls.Add(this.left1);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(400, 961);
            this.leftPanel.TabIndex = 0;
            // 
            // left3
            // 
            this.left3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.left3.Controls.Add(this.waterBoxGbx);
            this.left3.Location = new System.Drawing.Point(0, 353);
            this.left3.Margin = new System.Windows.Forms.Padding(0);
            this.left3.Name = "left3";
            this.left3.Size = new System.Drawing.Size(400, 608);
            this.left3.TabIndex = 4;
            // 
            // waterBoxGbx
            // 
            this.waterBoxGbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.waterBoxGbx.Controls.Add(this.Temp5Status);
            this.waterBoxGbx.Controls.Add(this.Temp4Status);
            this.waterBoxGbx.Controls.Add(this.Temp3Status);
            this.waterBoxGbx.Controls.Add(this.Temp2Status);
            this.waterBoxGbx.Controls.Add(this.Temp1Status);
            this.waterBoxGbx.Controls.Add(this.hslWaterBox5);
            this.waterBoxGbx.Controls.Add(this.hslWaterBox3);
            this.waterBoxGbx.Controls.Add(this.hslWaterBox4);
            this.waterBoxGbx.Controls.Add(this.hslWaterBox2);
            this.waterBoxGbx.Controls.Add(this.hslWaterBox1);
            this.waterBoxGbx.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.waterBoxGbx.Location = new System.Drawing.Point(9, 9);
            this.waterBoxGbx.Name = "waterBoxGbx";
            this.waterBoxGbx.Size = new System.Drawing.Size(381, 584);
            this.waterBoxGbx.TabIndex = 7;
            this.waterBoxGbx.TabStop = false;
            this.waterBoxGbx.Text = "水箱状态";
            // 
            // Temp5Status
            // 
            this.Temp5Status.AutoSize = true;
            this.Temp5Status.Location = new System.Drawing.Point(252, 544);
            this.Temp5Status.Name = "Temp5Status";
            this.Temp5Status.Size = new System.Drawing.Size(104, 24);
            this.Temp5Status.TabIndex = 16;
            this.Temp5Status.Text = "温度：10℃";
            // 
            // Temp4Status
            // 
            this.Temp4Status.AutoSize = true;
            this.Temp4Status.Location = new System.Drawing.Point(131, 544);
            this.Temp4Status.Name = "Temp4Status";
            this.Temp4Status.Size = new System.Drawing.Size(104, 24);
            this.Temp4Status.TabIndex = 15;
            this.Temp4Status.Text = "温度：10℃";
            // 
            // Temp3Status
            // 
            this.Temp3Status.AutoSize = true;
            this.Temp3Status.Location = new System.Drawing.Point(8, 544);
            this.Temp3Status.Name = "Temp3Status";
            this.Temp3Status.Size = new System.Drawing.Size(104, 24);
            this.Temp3Status.TabIndex = 14;
            this.Temp3Status.Text = "温度：10℃";
            // 
            // Temp2Status
            // 
            this.Temp2Status.AutoSize = true;
            this.Temp2Status.Location = new System.Drawing.Point(202, 242);
            this.Temp2Status.Name = "Temp2Status";
            this.Temp2Status.Size = new System.Drawing.Size(104, 24);
            this.Temp2Status.TabIndex = 13;
            this.Temp2Status.Text = "温度：10℃";
            // 
            // Temp1Status
            // 
            this.Temp1Status.AutoSize = true;
            this.Temp1Status.Location = new System.Drawing.Point(59, 242);
            this.Temp1Status.Name = "Temp1Status";
            this.Temp1Status.Size = new System.Drawing.Size(104, 24);
            this.Temp1Status.TabIndex = 11;
            this.Temp1Status.Text = "温度：10℃";
            // 
            // hslWaterBox5
            // 
            this.hslWaterBox5.BorderColor = System.Drawing.Color.Gray;
            this.hslWaterBox5.EdgeColor = System.Drawing.Color.Silver;
            this.hslWaterBox5.Location = new System.Drawing.Point(246, 354);
            this.hslWaterBox5.Margin = new System.Windows.Forms.Padding(0);
            this.hslWaterBox5.Name = "hslWaterBox5";
            this.hslWaterBox5.Size = new System.Drawing.Size(120, 180);
            this.hslWaterBox5.TabIndex = 10;
            this.hslWaterBox5.Text = "常温水箱";
            this.hslWaterBox5.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(235)))), ((int)(((byte)(246)))));
            // 
            // hslWaterBox3
            // 
            this.hslWaterBox3.BorderColor = System.Drawing.Color.Gray;
            this.hslWaterBox3.EdgeColor = System.Drawing.Color.Silver;
            this.hslWaterBox3.Location = new System.Drawing.Point(6, 354);
            this.hslWaterBox3.Margin = new System.Windows.Forms.Padding(0);
            this.hslWaterBox3.Name = "hslWaterBox3";
            this.hslWaterBox3.Size = new System.Drawing.Size(120, 180);
            this.hslWaterBox3.TabIndex = 8;
            this.hslWaterBox3.Text = "高温水箱";
            this.hslWaterBox3.WaterColor = System.Drawing.Color.Salmon;
            // 
            // hslWaterBox4
            // 
            this.hslWaterBox4.BorderColor = System.Drawing.Color.Gray;
            this.hslWaterBox4.EdgeColor = System.Drawing.Color.Silver;
            this.hslWaterBox4.Location = new System.Drawing.Point(126, 354);
            this.hslWaterBox4.Margin = new System.Windows.Forms.Padding(0);
            this.hslWaterBox4.Name = "hslWaterBox4";
            this.hslWaterBox4.Size = new System.Drawing.Size(120, 180);
            this.hslWaterBox4.TabIndex = 9;
            this.hslWaterBox4.Text = "中温水箱";
            this.hslWaterBox4.WaterColor = System.Drawing.Color.MistyRose;
            // 
            // hslWaterBox2
            // 
            this.hslWaterBox2.BorderColor = System.Drawing.Color.Gray;
            this.hslWaterBox2.EdgeColor = System.Drawing.Color.Silver;
            this.hslWaterBox2.Location = new System.Drawing.Point(181, 48);
            this.hslWaterBox2.Margin = new System.Windows.Forms.Padding(0);
            this.hslWaterBox2.Name = "hslWaterBox2";
            this.hslWaterBox2.Size = new System.Drawing.Size(140, 180);
            this.hslWaterBox2.TabIndex = 5;
            this.hslWaterBox2.Text = "热水箱";
            this.hslWaterBox2.WaterColor = System.Drawing.Color.Tomato;
            // 
            // hslWaterBox1
            // 
            this.hslWaterBox1.BorderColor = System.Drawing.Color.Gray;
            this.hslWaterBox1.EdgeColor = System.Drawing.Color.Silver;
            this.hslWaterBox1.Location = new System.Drawing.Point(41, 48);
            this.hslWaterBox1.Margin = new System.Windows.Forms.Padding(0);
            this.hslWaterBox1.Name = "hslWaterBox1";
            this.hslWaterBox1.Size = new System.Drawing.Size(140, 180);
            this.hslWaterBox1.TabIndex = 4;
            this.hslWaterBox1.Text = "冷水箱";
            this.hslWaterBox1.WaterColor = System.Drawing.Color.DeepSkyBlue;
            // 
            // left2
            // 
            this.left2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.left2.Controls.Add(this.dataGbx);
            this.left2.Location = new System.Drawing.Point(0, 117);
            this.left2.Margin = new System.Windows.Forms.Padding(0);
            this.left2.Name = "left2";
            this.left2.Size = new System.Drawing.Size(400, 236);
            this.left2.TabIndex = 3;
            // 
            // dataGbx
            // 
            this.dataGbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGbx.Controls.Add(this.label14);
            this.dataGbx.Controls.Add(this.label15);
            this.dataGbx.Controls.Add(this.label16);
            this.dataGbx.Controls.Add(this.label17);
            this.dataGbx.Controls.Add(this.QhShow);
            this.dataGbx.Controls.Add(this.QcShow);
            this.dataGbx.Controls.Add(this.QmShow);
            this.dataGbx.Controls.Add(this.PhShow);
            this.dataGbx.Controls.Add(this.PcShow);
            this.dataGbx.Controls.Add(this.PmShow);
            this.dataGbx.Controls.Add(this.ThShow);
            this.dataGbx.Controls.Add(this.TcShow);
            this.dataGbx.Controls.Add(this.TmShow);
            this.dataGbx.Controls.Add(this.label4);
            this.dataGbx.Controls.Add(this.label3);
            this.dataGbx.Controls.Add(this.label2);
            this.dataGbx.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.dataGbx.Location = new System.Drawing.Point(9, 6);
            this.dataGbx.Name = "dataGbx";
            this.dataGbx.Size = new System.Drawing.Size(381, 214);
            this.dataGbx.TabIndex = 13;
            this.dataGbx.TabStop = false;
            this.dataGbx.Text = "数据监控区";
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(281, 36);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 40);
            this.label14.TabIndex = 15;
            this.label14.Text = "流量 L/Min";
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(181, 36);
            this.label15.Margin = new System.Windows.Forms.Padding(0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 40);
            this.label15.TabIndex = 14;
            this.label15.Text = "压力 Bar";
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(81, 36);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 40);
            this.label16.TabIndex = 13;
            this.label16.Text = "温度 ℃";
            // 
            // label17
            // 
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label17.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(1, 36);
            this.label17.Margin = new System.Windows.Forms.Padding(0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(80, 40);
            this.label17.TabIndex = 12;
            // 
            // QhShow
            // 
            this.QhShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.QhShow.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.QhShow.Location = new System.Drawing.Point(281, 156);
            this.QhShow.Margin = new System.Windows.Forms.Padding(0);
            this.QhShow.Name = "QhShow";
            this.QhShow.Size = new System.Drawing.Size(100, 40);
            this.QhShow.TabIndex = 11;
            this.QhShow.Text = "label8";
            // 
            // QcShow
            // 
            this.QcShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.QcShow.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.QcShow.Location = new System.Drawing.Point(281, 116);
            this.QcShow.Margin = new System.Windows.Forms.Padding(0);
            this.QcShow.Name = "QcShow";
            this.QcShow.Size = new System.Drawing.Size(100, 40);
            this.QcShow.TabIndex = 10;
            this.QcShow.Text = "label9";
            // 
            // QmShow
            // 
            this.QmShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.QmShow.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.QmShow.Location = new System.Drawing.Point(281, 76);
            this.QmShow.Margin = new System.Windows.Forms.Padding(0);
            this.QmShow.Name = "QmShow";
            this.QmShow.Size = new System.Drawing.Size(100, 40);
            this.QmShow.TabIndex = 9;
            this.QmShow.Text = "label10";
            // 
            // PhShow
            // 
            this.PhShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PhShow.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PhShow.Location = new System.Drawing.Point(181, 156);
            this.PhShow.Margin = new System.Windows.Forms.Padding(0);
            this.PhShow.Name = "PhShow";
            this.PhShow.Size = new System.Drawing.Size(100, 40);
            this.PhShow.TabIndex = 8;
            this.PhShow.Text = "label11";
            // 
            // PcShow
            // 
            this.PcShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PcShow.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PcShow.Location = new System.Drawing.Point(181, 116);
            this.PcShow.Margin = new System.Windows.Forms.Padding(0);
            this.PcShow.Name = "PcShow";
            this.PcShow.Size = new System.Drawing.Size(100, 40);
            this.PcShow.TabIndex = 7;
            this.PcShow.Text = "label12";
            // 
            // PmShow
            // 
            this.PmShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PmShow.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PmShow.Location = new System.Drawing.Point(181, 76);
            this.PmShow.Margin = new System.Windows.Forms.Padding(0);
            this.PmShow.Name = "PmShow";
            this.PmShow.Size = new System.Drawing.Size(100, 40);
            this.PmShow.TabIndex = 6;
            this.PmShow.Text = "label13";
            // 
            // ThShow
            // 
            this.ThShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ThShow.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ThShow.Location = new System.Drawing.Point(81, 156);
            this.ThShow.Margin = new System.Windows.Forms.Padding(0);
            this.ThShow.Name = "ThShow";
            this.ThShow.Size = new System.Drawing.Size(100, 40);
            this.ThShow.TabIndex = 5;
            this.ThShow.Text = "label5";
            // 
            // TcShow
            // 
            this.TcShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TcShow.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TcShow.Location = new System.Drawing.Point(81, 116);
            this.TcShow.Margin = new System.Windows.Forms.Padding(0);
            this.TcShow.Name = "TcShow";
            this.TcShow.Size = new System.Drawing.Size(100, 40);
            this.TcShow.TabIndex = 4;
            this.TcShow.Text = "label6";
            // 
            // TmShow
            // 
            this.TmShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TmShow.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TmShow.Location = new System.Drawing.Point(81, 76);
            this.TmShow.Margin = new System.Windows.Forms.Padding(0);
            this.TmShow.Name = "TmShow";
            this.TmShow.Size = new System.Drawing.Size(100, 40);
            this.TmShow.TabIndex = 3;
            this.TmShow.Text = "label7";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(1, 156);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 40);
            this.label4.TabIndex = 2;
            this.label4.Text = "热水";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(1, 116);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 40);
            this.label3.TabIndex = 1;
            this.label3.Text = "冷水";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(1, 76);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "出水";
            // 
            // left1
            // 
            this.left1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.left1.Controls.Add(this.groupBox3);
            this.left1.Controls.Add(this.ICON);
            this.left1.Location = new System.Drawing.Point(0, 0);
            this.left1.Margin = new System.Windows.Forms.Padding(0);
            this.left1.Name = "left1";
            this.left1.Size = new System.Drawing.Size(400, 117);
            this.left1.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.standardCbx);
            this.groupBox3.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(10, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(180, 100);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "测试标准";
            // 
            // standardCbx
            // 
            this.standardCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.standardCbx.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.standardCbx.FormattingEnabled = true;
            this.standardCbx.Items.AddRange(new object[] {
            "EN1111-2017",
            "自定义"});
            this.standardCbx.Location = new System.Drawing.Point(11, 31);
            this.standardCbx.Name = "standardCbx";
            this.standardCbx.Size = new System.Drawing.Size(160, 31);
            this.standardCbx.TabIndex = 2;
            this.standardCbx.TextChanged += new System.EventHandler(this.StandardCbx_TextChanged);
            // 
            // ICON
            // 
            this.ICON.Image = ((System.Drawing.Image)(resources.GetObject("ICON.Image")));
            this.ICON.Location = new System.Drawing.Point(207, 26);
            this.ICON.Name = "ICON";
            this.ICON.Size = new System.Drawing.Size(183, 62);
            this.ICON.TabIndex = 0;
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.right3);
            this.rightPanel.Controls.Add(this.right4);
            this.rightPanel.Controls.Add(this.right1);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point(984, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(500, 961);
            this.rightPanel.TabIndex = 1;
            // 
            // right3
            // 
            this.right3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.right3.Controls.Add(this.groupBox4);
            this.right3.Location = new System.Drawing.Point(10, 117);
            this.right3.Margin = new System.Windows.Forms.Padding(0);
            this.right3.Name = "right3";
            this.right3.Size = new System.Drawing.Size(490, 645);
            this.right3.TabIndex = 7;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.systemInfoTb);
            this.groupBox4.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(11, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(474, 631);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "数据结果";
            // 
            // systemInfoTb
            // 
            this.systemInfoTb.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.systemInfoTb.Location = new System.Drawing.Point(7, 29);
            this.systemInfoTb.Multiline = true;
            this.systemInfoTb.Name = "systemInfoTb";
            this.systemInfoTb.ReadOnly = true;
            this.systemInfoTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.systemInfoTb.Size = new System.Drawing.Size(459, 596);
            this.systemInfoTb.TabIndex = 22;
            // 
            // right4
            // 
            this.right4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.right4.Controls.Add(this.groupBox1);
            this.right4.Location = new System.Drawing.Point(10, 762);
            this.right4.Margin = new System.Windows.Forms.Padding(0);
            this.right4.Name = "right4";
            this.right4.Size = new System.Drawing.Size(490, 199);
            this.right4.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.groupBox1.Location = new System.Drawing.Point(6, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(485, 185);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统控制区";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.setParamBtn, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.hslButton12, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.hslButton11, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.hslButton10, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.hslButton9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.autoRunBtn, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.stopBtn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.clearDataBtn, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.hslButton7, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.hslButton8, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.startBtn, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.saveDataBtn, 1, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 26);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(473, 153);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // setParamBtn
            // 
            this.setParamBtn.CustomerInformation = null;
            this.setParamBtn.Location = new System.Drawing.Point(239, 3);
            this.setParamBtn.Name = "setParamBtn";
            this.setParamBtn.Size = new System.Drawing.Size(112, 44);
            this.setParamBtn.TabIndex = 2;
            this.setParamBtn.Text = "设置系统参数";
            this.setParamBtn.Click += new System.EventHandler(this.SetParamBtn_Click);
            // 
            // hslButton12
            // 
            this.hslButton12.CustomerInformation = null;
            this.hslButton12.Location = new System.Drawing.Point(357, 105);
            this.hslButton12.Name = "hslButton12";
            this.hslButton12.Size = new System.Drawing.Size(113, 44);
            this.hslButton12.TabIndex = 12;
            this.hslButton12.Text = "hslButton12";
            // 
            // hslButton11
            // 
            this.hslButton11.CustomerInformation = null;
            this.hslButton11.Location = new System.Drawing.Point(239, 105);
            this.hslButton11.Name = "hslButton11";
            this.hslButton11.Size = new System.Drawing.Size(110, 44);
            this.hslButton11.TabIndex = 11;
            this.hslButton11.Text = "hslButton11";
            // 
            // hslButton10
            // 
            this.hslButton10.CustomerInformation = null;
            this.hslButton10.Location = new System.Drawing.Point(3, 105);
            this.hslButton10.Name = "hslButton10";
            this.hslButton10.Size = new System.Drawing.Size(112, 44);
            this.hslButton10.TabIndex = 10;
            this.hslButton10.Text = "加热";
            this.hslButton10.Click += new System.EventHandler(this.HeatingBtn_Click);
            // 
            // hslButton9
            // 
            this.hslButton9.CustomerInformation = null;
            this.hslButton9.Location = new System.Drawing.Point(121, 105);
            this.hslButton9.Name = "hslButton9";
            this.hslButton9.Size = new System.Drawing.Size(112, 44);
            this.hslButton9.TabIndex = 9;
            this.hslButton9.Text = "制冷";
            this.hslButton9.Click += new System.EventHandler(this.CoolingBtn_Click);
            // 
            // autoRunBtn
            // 
            this.autoRunBtn.CustomerInformation = null;
            this.autoRunBtn.Location = new System.Drawing.Point(357, 3);
            this.autoRunBtn.Name = "autoRunBtn";
            this.autoRunBtn.Size = new System.Drawing.Size(113, 44);
            this.autoRunBtn.TabIndex = 4;
            this.autoRunBtn.Text = "自动测试运行";
            // 
            // stopBtn
            // 
            this.stopBtn.CustomerInformation = null;
            this.stopBtn.Location = new System.Drawing.Point(3, 54);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(110, 44);
            this.stopBtn.TabIndex = 5;
            this.stopBtn.Text = "停止";
            this.stopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // clearDataBtn
            // 
            this.clearDataBtn.CustomerInformation = null;
            this.clearDataBtn.Location = new System.Drawing.Point(121, 54);
            this.clearDataBtn.Name = "clearDataBtn";
            this.clearDataBtn.Size = new System.Drawing.Size(112, 44);
            this.clearDataBtn.TabIndex = 6;
            this.clearDataBtn.Text = "清空数据";
            this.clearDataBtn.Click += new System.EventHandler(this.ClearDataBtn_Click);
            // 
            // hslButton7
            // 
            this.hslButton7.CustomerInformation = null;
            this.hslButton7.Location = new System.Drawing.Point(239, 54);
            this.hslButton7.Name = "hslButton7";
            this.hslButton7.Size = new System.Drawing.Size(112, 44);
            this.hslButton7.TabIndex = 7;
            this.hslButton7.Text = "保存图像";
            // 
            // hslButton8
            // 
            this.hslButton8.CustomerInformation = null;
            this.hslButton8.Location = new System.Drawing.Point(357, 54);
            this.hslButton8.Name = "hslButton8";
            this.hslButton8.Size = new System.Drawing.Size(113, 44);
            this.hslButton8.TabIndex = 8;
            this.hslButton8.Text = "导出测试报告";
            this.hslButton8.Click += new System.EventHandler(this.ExportReportBtn_Click);
            // 
            // startBtn
            // 
            this.startBtn.CustomerInformation = null;
            this.startBtn.Location = new System.Drawing.Point(3, 3);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(110, 44);
            this.startBtn.TabIndex = 1;
            this.startBtn.Text = "开始";
            this.startBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // saveDataBtn
            // 
            this.saveDataBtn.CustomerInformation = null;
            this.saveDataBtn.Location = new System.Drawing.Point(121, 3);
            this.saveDataBtn.Name = "saveDataBtn";
            this.saveDataBtn.Size = new System.Drawing.Size(112, 44);
            this.saveDataBtn.TabIndex = 3;
            this.saveDataBtn.Text = "保存数据";
            this.saveDataBtn.Click += new System.EventHandler(this.SaveDataBtn_Click);
            // 
            // right1
            // 
            this.right1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.right1.Controls.Add(this.groupBox2);
            this.right1.Location = new System.Drawing.Point(10, 0);
            this.right1.Margin = new System.Windows.Forms.Padding(0);
            this.right1.Name = "right1";
            this.right1.Size = new System.Drawing.Size(490, 117);
            this.right1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.FlowTestRbt);
            this.groupBox2.Controls.Add(this.tmpTestRbt);
            this.groupBox2.Controls.Add(this.coolTestRbt);
            this.groupBox2.Controls.Add(this.pressureTestRbt);
            this.groupBox2.Controls.Add(this.safeTestRbt);
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(11, 8);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(474, 103);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "子操作界面选择";
            // 
            // FlowTestRbt
            // 
            this.FlowTestRbt.AutoSize = true;
            this.FlowTestRbt.Location = new System.Drawing.Point(166, 64);
            this.FlowTestRbt.Name = "FlowTestRbt";
            this.FlowTestRbt.Size = new System.Drawing.Size(130, 27);
            this.FlowTestRbt.TabIndex = 9;
            this.FlowTestRbt.Text = "流量减少测试";
            this.FlowTestRbt.UseVisualStyleBackColor = true;
            this.FlowTestRbt.CheckedChanged += new System.EventHandler(this.RadioBtn_CheckedChange);
            // 
            // tmpTestRbt
            // 
            this.tmpTestRbt.AutoSize = true;
            this.tmpTestRbt.Location = new System.Drawing.Point(7, 63);
            this.tmpTestRbt.Name = "tmpTestRbt";
            this.tmpTestRbt.Size = new System.Drawing.Size(147, 27);
            this.tmpTestRbt.TabIndex = 8;
            this.tmpTestRbt.Text = "温度稳定性测试";
            this.tmpTestRbt.UseVisualStyleBackColor = true;
            this.tmpTestRbt.CheckedChanged += new System.EventHandler(this.RadioBtn_CheckedChange);
            // 
            // coolTestRbt
            // 
            this.coolTestRbt.AutoSize = true;
            this.coolTestRbt.Location = new System.Drawing.Point(314, 31);
            this.coolTestRbt.Name = "coolTestRbt";
            this.coolTestRbt.Size = new System.Drawing.Size(96, 27);
            this.coolTestRbt.TabIndex = 7;
            this.coolTestRbt.Text = "降温测试";
            this.coolTestRbt.UseVisualStyleBackColor = true;
            this.coolTestRbt.CheckedChanged += new System.EventHandler(this.RadioBtn_CheckedChange);
            // 
            // pressureTestRbt
            // 
            this.pressureTestRbt.AutoSize = true;
            this.pressureTestRbt.Location = new System.Drawing.Point(166, 30);
            this.pressureTestRbt.Name = "pressureTestRbt";
            this.pressureTestRbt.Size = new System.Drawing.Size(130, 27);
            this.pressureTestRbt.TabIndex = 6;
            this.pressureTestRbt.Text = "压力变化测试";
            this.pressureTestRbt.UseVisualStyleBackColor = true;
            this.pressureTestRbt.CheckedChanged += new System.EventHandler(this.RadioBtn_CheckedChange);
            // 
            // safeTestRbt
            // 
            this.safeTestRbt.AutoSize = true;
            this.safeTestRbt.Checked = true;
            this.safeTestRbt.Location = new System.Drawing.Point(7, 29);
            this.safeTestRbt.Name = "safeTestRbt";
            this.safeTestRbt.Size = new System.Drawing.Size(113, 27);
            this.safeTestRbt.TabIndex = 5;
            this.safeTestRbt.TabStop = true;
            this.safeTestRbt.Text = "安全性测试";
            this.safeTestRbt.UseVisualStyleBackColor = true;
            this.safeTestRbt.CheckedChanged += new System.EventHandler(this.RadioBtn_CheckedChange);
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
            this.hslCurveHistory1.Location = new System.Drawing.Point(3, 426);
            this.hslCurveHistory1.MarkTextColor = System.Drawing.Color.Yellow;
            this.hslCurveHistory1.Name = "hslCurveHistory1";
            this.hslCurveHistory1.Size = new System.Drawing.Size(591, 414);
            this.hslCurveHistory1.TabIndex = 4;
            this.hslCurveHistory1.Text = "hslCurveHistory1";
            this.hslCurveHistory1.ValueMaxLeft = 10F;
            this.hslCurveHistory1.ValueMaxRight = 10F;
            this.hslCurveHistory1.ValueSegment = 20;
            // 
            // centerPanel
            // 
            this.centerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.centerPanel.Controls.Add(this.center1);
            this.centerPanel.Controls.Add(this.Title);
            this.centerPanel.Location = new System.Drawing.Point(400, 0);
            this.centerPanel.Name = "centerPanel";
            this.centerPanel.Size = new System.Drawing.Size(604, 961);
            this.centerPanel.TabIndex = 2;
            // 
            // center1
            // 
            this.center1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.center1.Controls.Add(this.hslCurve1);
            this.center1.Controls.Add(this.hslCurveHistory1);
            this.center1.Location = new System.Drawing.Point(-1, 116);
            this.center1.Margin = new System.Windows.Forms.Padding(0);
            this.center1.Name = "center1";
            this.center1.Size = new System.Drawing.Size(604, 844);
            this.center1.TabIndex = 2;
            // 
            // Title
            // 
            this.Title.Font = new System.Drawing.Font("微软雅黑", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(105, 8);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(393, 80);
            this.Title.TabIndex = 1;
            this.Title.Text = "恒温测试界面";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_instantAoCtrl
            // 
            this.m_instantAoCtrl._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("m_instantAoCtrl._StateStream")));
            // 
            // instantDoCtrl1
            // 
            this.instantDoCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantDoCtrl1._StateStream")));
            // 
            // instantDiCtrl1
            // 
            this.instantDiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantDiCtrl1._StateStream")));
            // 
            // waveformAiCtrl1
            // 
            this.waveformAiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("waveformAiCtrl1._StateStream")));
            // 
            // hslCurve1
            // 
            this.hslCurve1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.hslCurve1.ColorDashLines = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.hslCurve1.ColorLinesAndText = System.Drawing.Color.LightGray;
            this.hslCurve1.Location = new System.Drawing.Point(5, 3);
            this.hslCurve1.Name = "hslCurve1";
            this.hslCurve1.Size = new System.Drawing.Size(589, 420);
            this.hslCurve1.TabIndex = 7;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 961);
            this.Controls.Add(this.centerPanel);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.leftPanel);
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.leftPanel.ResumeLayout(false);
            this.left3.ResumeLayout(false);
            this.waterBoxGbx.ResumeLayout(false);
            this.waterBoxGbx.PerformLayout();
            this.left2.ResumeLayout(false);
            this.dataGbx.ResumeLayout(false);
            this.left1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.right3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.right4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.right1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.centerPanel.ResumeLayout(false);
            this.center1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel left1;
        private System.Windows.Forms.ComboBox standardCbx;
        private System.Windows.Forms.Panel left2;
        private System.Windows.Forms.Panel left3;
        private System.Windows.Forms.Panel right1;
        private System.Windows.Forms.Panel right3;
        private System.Windows.Forms.Panel right4;
        private System.Windows.Forms.GroupBox dataGbx;
        private System.Windows.Forms.Panel centerPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ThShow;
        private System.Windows.Forms.Label TcShow;
        private System.Windows.Forms.Label TmShow;
        private System.Windows.Forms.Label QhShow;
        private System.Windows.Forms.Label QcShow;
        private System.Windows.Forms.Label QmShow;
        private System.Windows.Forms.Label PhShow;
        private System.Windows.Forms.Label PcShow;
        private System.Windows.Forms.Label PmShow;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox waterBoxGbx;
        private HslControls.HslWaterBox hslWaterBox1;
        private HslControls.HslWaterBox hslWaterBox2;
        private HslControls.HslWaterBox hslWaterBox5;
        private HslControls.HslWaterBox hslWaterBox3;
        private HslControls.HslWaterBox hslWaterBox4;
        private System.Windows.Forms.Label Temp1Status;
        private System.Windows.Forms.Label Temp3Status;
        private System.Windows.Forms.Label Temp2Status;
        private System.Windows.Forms.Label Temp4Status;
        private System.Windows.Forms.Label Temp5Status;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private HslControls.HslButton hslButton12;
        private HslControls.HslButton hslButton11;
        private HslControls.HslButton hslButton10;
        private HslControls.HslButton hslButton9;
        private HslControls.HslButton startBtn;
        private HslControls.HslButton saveDataBtn;
        private HslControls.HslButton autoRunBtn;
        private HslControls.HslButton stopBtn;
        private HslControls.HslButton clearDataBtn;
        private HslControls.HslButton hslButton7;
        private HslControls.HslButton hslButton8;
        private HslControls.HslButton setParamBtn;
        private System.Windows.Forms.Label ICON;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Panel center1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton FlowTestRbt;
        private System.Windows.Forms.RadioButton tmpTestRbt;
        private System.Windows.Forms.RadioButton coolTestRbt;
        private System.Windows.Forms.RadioButton pressureTestRbt;
        private System.Windows.Forms.RadioButton safeTestRbt;
        private System.Windows.Forms.GroupBox groupBox3;
        private HslControls.HslCurveHistory hslCurveHistory1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox systemInfoTb;
        private Automation.BDaq.InstantAoCtrl m_instantAoCtrl;
        private Automation.BDaq.InstantDoCtrl instantDoCtrl1;
        private Automation.BDaq.InstantDiCtrl instantDiCtrl1;
        private Automation.BDaq.WaveformAiCtrl waveformAiCtrl1;
        private HslControls.HslCurve hslCurve1;
    }
}