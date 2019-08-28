using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 恒温测试机.Model.Enum;
using 恒温测试机.Utils;

namespace 恒温测试机.UI
{
    public partial class FormConnectValueSetting : Form
    {
        private FormMain formMain;
        private byte station = 1;
        //private ElectricalMachineryType type;

        System.Timers.Timer monitorTimer;            //监控M寄存器定时器
        System.Timers.Timer monitorDTimer;            //监控D寄存器定时器

        public double autoFindAngle_spin = 0;         //自动找点角度A
        public double autoFindAngle_upDown = 0;         //自动找点角度L    

        private bool isAutoFindAngle = false;
        public DataTable timeAngleDt;
        public Dictionary<double, double> tempAngleDict = new Dictionary<double, double>();
        public Dictionary<double, DateTime> tmTimeDict = new Dictionary<double, DateTime>();
        public Dictionary<DateTime, double> timeAngleDict = new Dictionary<DateTime, double>();

        #region 旋转电机相关地址
        public string powerAddress_spin = "2056";   //M8-2056   M18-2066
        public bool powerState_spin = false;

        public string forwardWriteAddress_spin = "2048";
        private string forwardReadAddress_spin = "2053";
        public bool forwardState_spin = false;

        public string noForwardWriteAddress_spin = "2049";
        private string noForwardReadAddress_spin = "2054";
        private bool noForwadState_spin = false;

        private string orignWriteAddress_spin = "2050";
        private string orignReadAddress_spin = "2055";
        private bool orignState_spin = false;

        private string autoRunAddress_spin = "2051";
        private string backOrignAddress_spin = "2052";
        public string shutdownAddress_spin = "2057";

        private string radioAddress_spin = "6096";
        private uint radioValue_spin = 0;
        public string angleAddress_spin = "4100";//"";
        public int angleValue_spin = 0;
        #endregion

        #region 升降电机相关地址
        public string powerAddress_upDown = "2066";   //M8-2056   M18-2066
        public bool powerState_upDown = false;

        public string forwardWriteAddress_upDown = "2058";
        private string forwardReadAddress_upDown = "2063";
        private bool forwardState_upDown = false;

        public string noForwardWriteAddress_upDown = "2059";
        private string noForwardReadAddress_upDown = "2064";
        private bool noForwadState_upDown = false;

        private string orignWriteAddress_upDown = "2060";
        private string orignReadAddress_upDown = "2065";
        private bool orignState_upDown = false;

        private string autoRunAddress_upDown = "2061";
        private string backOrignAddress_upDown = "2062";
        public string shutdownAddress_upDown = "2067";

        private string radioAddress_upDown = "6098";
        private uint radioValue_upDown = 0;
        private string angleAddress_upDown = "4102";//"";
        private int angleValue_upDown = 0;
        #endregion

        public FormConnectValueSetting(FormMain formMain)
        {
            InitializeComponent();
            InitTimer();
            this.formMain = formMain;
            timeAngleDt = new DataTable();
            timeAngleDt.Columns.Add("时间", typeof(string));
            timeAngleDt.Columns.Add("角度", typeof(double));
        }

        #region 电机相关定时器
        private void InitTimer()
        {
            monitorTimer = new System.Timers.Timer(1000);
            monitorTimer.Elapsed += (o, a) =>
            {
                MonitorActive();
            };//到达时间的时候执行事件；
            monitorTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            monitorTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            monitorDTimer = new System.Timers.Timer(1000);
            monitorDTimer.Elapsed += (o, a) =>
            {
                MonitorDActive();
            };//到达时间的时候执行事件；
            monitorDTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            monitorDTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }


        private delegate void MonitorActiveDelegate();
        private void MonitorActive()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MonitorActiveDelegate monitorActiveDelegate = MonitorActive;
                    this.Invoke(monitorActiveDelegate);
                }
                else
                {
                    #region 旋转电机状态
                    powerState_spin = formMain.bpq.read_coil(powerAddress_spin, 5);
                    forwardState_spin = formMain.bpq.read_coil(forwardReadAddress_spin, 5);
                    noForwadState_spin = formMain.bpq.read_coil(noForwardReadAddress_spin, 5);
                    orignState_spin = formMain.bpq.read_coil(orignReadAddress_spin, 5);

                    powerBtn_spin.BackColor = powerState_spin ? Color.Green : DefaultBackColor;
                    powerBtn_spin.Text = powerState_spin ? "伺服开" : "伺服关";
                    forwardBtn_spin.BackColor = forwardState_spin ? Color.Green : DefaultBackColor;
                    noForwardBtn_spin.BackColor = noForwadState_spin ? Color.Green : DefaultBackColor;
                    orignBtn_spin.BackColor = orignState_spin ? Color.Green : DefaultBackColor;
                    #endregion

                    #region 升降电机状态
                    powerState_upDown = formMain.bpq.read_coil(powerAddress_upDown, 5);
                    forwardState_upDown = formMain.bpq.read_coil(forwardReadAddress_upDown, 5);
                    noForwadState_upDown = formMain.bpq.read_coil(noForwardReadAddress_upDown, 5);
                    orignState_upDown = formMain.bpq.read_coil(orignReadAddress_upDown, 5);

                    powerBtn_upDown.BackColor = powerState_upDown ? Color.Green : DefaultBackColor;
                    powerBtn_upDown.Text = powerState_upDown ? "伺服开" : "伺服关";
                    forwardBtn_upDown.BackColor = forwardState_upDown ? Color.Green : DefaultBackColor;
                    noForwardBtn_upDown.BackColor = noForwadState_upDown ? Color.Green : DefaultBackColor;
                    orignBtn_upDown.BackColor = orignState_upDown ? Color.Green : DefaultBackColor;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Log.Error("读取电机状态异常：" + ex.ToString());
                return;
            }

        }

        private delegate void MonitorDActiveDelegate();
        private void MonitorDActive()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MonitorDActiveDelegate monitorActiveDelegate = MonitorDActive;
                    this.Invoke(monitorActiveDelegate);
                }
                else
                {
                    #region 旋转电机
                    radioValue_spin = formMain.bpq.read_uint(radioAddress_spin, 5);   //转速  读取 uint
                    angleValue_spin = formMain.bpq.read_int(angleAddress_spin, 5);    //角度  读取 int

                    radioLb_spin.Text = "" + Math.Round(radioValue_spin / 3200.0, 1);
                    angelLb_spin.Text = "" + Math.Round(angleValue_spin / 3200.0, 1);
                    if (isAutoFindAngle)
                    {
                        timeAngleDt.Rows.Add(
                            DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                            Math.Round(angleValue_spin / 10000.0, 1));
                    }
                    #endregion

                    radioValue_upDown = formMain.bpq.read_uint(radioAddress_upDown, 5);   //转速  读取 uint
                    angleValue_upDown = formMain.bpq.read_int(angleAddress_upDown, 5);    //角度  读取 int

                    var temp3 = Math.Round((radioValue_upDown / 4000.0), 1);
                    var temp4 = Math.Round((angleValue_upDown / 4000.0), 1);
                    radioLb_upDown.Text = "" + temp3;
                    angelLb_upDown.Text = "" + temp4;
                }
            }
            catch (Exception ex)
            {
                Log.Error("电机角度转速读取异常：" + ex.ToString());
                return;
            }

        }

        private delegate void SystemInfoPrintDelegate(string s);//记录数据  
        private void SystemInfoPrint(string msg)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    SystemInfoPrintDelegate systemInfoDelegate = SystemInfoPrint;
                    this.Invoke(systemInfoDelegate, new object[] { msg });
                }
                else
                {
                    outInfoTb.AppendText(msg);
                    outInfoTb.AppendText("\n");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return;
            }
        }
        #endregion



        #region 变频器通信

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadBtn_Click(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "1":
                    station = 1;
                    break;
                case "2":
                    station = 2;
                    break;
                case "3":
                    station = 3;
                    break;
                case "4":
                    station = 4;
                    break;
            }
            //Read
            var val = formMain.Read(textBox2.Text, station);
            SystemInfoPrint("读取：【" + textBox2.Text + "】【" + val + "】\n");
        }


        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WriteBtn_Click(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "1":
                    station = 1;
                    break;
                case "2":
                    station = 2;
                    break;
                    //case "3":
                    //    station = 3;
                    //    break;
                    //case "4":
                    //    station = 4;
                    //    break;
            }
            int val2 = Convert.ToInt32(textBox3.Text) * 500;

            if (val2 < 0 || val2 > 5000)
            {
                MessageBox.Show("请输入0-10 范围内的值");
                textBox3.Text = "";
            }
            else
            {
                short val3 = Convert.ToInt16("" + val2);
                formMain.Write(textBox2.Text, val3, station);
                SystemInfoPrint("写入：【" + textBox2.Text + "】【" + val3 + "】\n");
            }
        }

        #endregion

        #region 旋转电机按钮

        /// <summary>
        /// 伺服开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PowerSwitchBtn_spin_Click(object sender, EventArgs e)
        {
            if (powerState_spin)
            {
                formMain.bpq.write_coil(powerAddress_spin, false, 5);
                isAutoFindAngle = false;
            }
            else
                formMain.bpq.write_coil(powerAddress_spin, true, 5);
        }

        private void ForwardBtn_spin_MouseClick(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else if (isAutoFindAngle)
            {
                return;
            }
            else
            {
                Console.WriteLine("click");
                if (forwardState_spin)
                    formMain.bpq.write_coil(forwardWriteAddress_spin, false, 5);
                else
                    formMain.bpq.write_coil(forwardWriteAddress_spin, true, 5);
            }
        }

        private void NoForwardBtn_spin_MouseClick(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else if (isAutoFindAngle)
            {
                return;
            }
            else
            {
                if (noForwadState_spin)
                    formMain.bpq.write_coil(noForwardWriteAddress_spin, false, 5);
                else
                    formMain.bpq.write_coil(noForwardWriteAddress_spin, true, 5);
            }
        }

        private void OrignBtn_spin_MouseClick(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else if (isAutoFindAngle)
            {
                return;
            }
            else
            {
                if (orignState_spin)
                    formMain.bpq.write_coil(orignWriteAddress_spin, false, 5);
                else
                    formMain.bpq.write_coil(orignWriteAddress_spin, true, 5);
            }
        }

        private void ShutdownBtn_spin_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else
            {
                formMain.bpq.write_coil(shutdownAddress_spin, true, 5);
                isAutoFindAngle = false;
            }
        }

        private void ShutdownBtn_spin_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else
            {
                formMain.bpq.write_coil(shutdownAddress_spin, false, 5);
                isAutoFindAngle = false;
            }
        }

        private void BackOrignBtn_spin_Click(object sender, EventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else if (isAutoFindAngle)
            {
                return;
            }
            else
            {
                formMain.bpq.write_coil(backOrignAddress_spin, true, 5);
            }
        }

        private void AutoRunBtn_spin_Click(object sender, EventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else if (isAutoFindAngle)
            {
                return;
            }
            else
            {
                formMain.bpq.write_coil(autoRunAddress_spin, true, 5);
            }
        }
        private void Radio_spin_Click(object sender, EventArgs e)
        {
            uint val2 = (uint)(double.Parse(radioTb_spin.Text) * 3200);

            if (val2 < 0 || val2 > 20000)
            {
                MessageBox.Show("请输入0-6.25 范围内的值");
                radioTb_spin.Text = "";
            }
            else if (isAutoFindAngle)
            {
                return;
            }
            else
            {
                formMain.Write_uint(radioAddress_spin, val2, 5);
                SystemInfoPrint("写入：【" + radioAddress_spin + "】【" + val2 + "】\n");
            }
        }

        private void AngleBtn_spin_Click(object sender, EventArgs e)
        {
            try
            {
                if (isAutoFindAngle)
                {
                    return;
                }
                autoFindAngle_spin = angleTb_spin.Text.ToDouble();
                SystemInfoPrint("写入自动找点预转动角度值：【" + autoFindAngle_spin + "】【" + DateTime.Now.ToString() + "】\n");

            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入正确的格式");
                return;
            }
        }
        private void autoFindBtn_spin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!powerState_spin)
                {
                    MessageBox.Show("请开启旋转电机");
                }
                if (autoFindAngle_spin == 0)
                {
                    MessageBox.Show("请先设置自动找点，电机预转动的角度");
                    return;
                }
                if (MessageBox.Show("请确认是否设置好原点、转速等相关参数", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    timeAngleDt.Clear();
                    formMain.ElectDt.Clear();
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始找点");
                    isAutoFindAngle = true;
                    formMain.electDataFlag = true;
                    System.Threading.Thread.Sleep(100);
                    //正传
                    formMain.bpq.write_coil(forwardWriteAddress_spin, true, 5);
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始正传...");
                    while (isAutoFindAngle && (angleValue_spin <= autoFindAngle_spin))
                    {
                        //等待角度达到 预设角度
                    }
                    formMain.bpq.write_coil(forwardWriteAddress_spin, false, 5);
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "结束正传...");
                    if (isAutoFindAngle == false)
                    {
                        SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "当前已手动停止找点...");
                        timeAngleDt.Clear();
                        formMain.ElectDt.Clear();
                        return;
                    }
                    //反传
                    formMain.bpq.write_coil(noForwardWriteAddress_spin, true, 5);
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始反传...");
                    while (isAutoFindAngle && (angleValue_spin == 0))
                    {
                        //等待角度达到 原点
                    }
                    formMain.bpq.write_coil(noForwardWriteAddress_spin, false, 5);
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "结束反传...");
                    if (isAutoFindAngle == false)
                    {
                        SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "当前已手动停止找点...");
                        timeAngleDt.Clear();
                        formMain.ElectDt.Clear();
                        return;
                    }

                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "数据采集完毕，开始分析...");
                    AnalyseElect();
                    if (tempAngleDict.Count > 0)
                    {
                        SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "采集的点位如下...");
                        foreach (var dic in tempAngleDict)
                        {
                            SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "温度：" + dic.Key + "-----> 角度：" + dic.Value);
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error("自动找点异常：" + ex.ToString());
                return;
            }
            finally
            {
                isAutoFindAngle = false;
                formMain.electDataFlag = false;
            }
        }

        /// <summary>
        /// 分析温度与角度对应关系
        /// </summary>
        private void AnalyseElect()
        {
            tmTimeDict = new Dictionary<double, DateTime>();
            timeAngleDict = new Dictionary<DateTime, double>();
            tempAngleDict = new Dictionary<double, double>();
            foreach (DataRow tmRow in formMain.ElectDt.Rows)
            {
                if (tmTimeDict.Keys.Count == 3)
                {
                    break;
                }
                var tm = tmRow["出水温度Tm"].AsDouble();
                var tmTime = tmRow["时间"].AsDateTime();
                if (tm == 36)
                {
                    if (tmTimeDict.ContainsKey(36))
                        continue;
                    else
                        tmTimeDict.Add(36, tmTime);
                }
                if (tm == 38)
                {
                    if (tmTimeDict.ContainsKey(38))
                        continue;
                    else
                        tmTimeDict.Add(38, tmTime);
                }
                if (tm == 40)
                {
                    if (tempAngleDict.ContainsKey(40))
                        continue;
                    else
                        tmTimeDict.Add(40, tmTime);
                }
            }
            foreach (DataRow eleRow in timeAngleDt.Rows)
            {
                if (timeAngleDict.Keys.Count == 3)
                {
                    break;
                }
                var angle = eleRow["角度"].AsDouble();
                var angleTime = eleRow["时间"].AsDateTime();

                if (timeAngleDict.ContainsKey(tmTimeDict[36]) == false && (angleTime - tmTimeDict[36]).TotalMilliseconds < 0.2)
                {
                    timeAngleDict.Add(tmTimeDict[36], angle);
                }

                if (timeAngleDict.ContainsKey(tmTimeDict[38]) == false && (angleTime - tmTimeDict[38]).TotalMilliseconds < 0.2)
                {
                    timeAngleDict.Add(tmTimeDict[38], angle);
                }

                if (timeAngleDict.ContainsKey(tmTimeDict[40]) == false && (angleTime - tmTimeDict[40]).TotalMilliseconds < 0.2)
                {
                    timeAngleDict.Add(tmTimeDict[40], angle);
                }
            }

            tempAngleDict.Add(36, timeAngleDict[tmTimeDict[36]]);
            tempAngleDict.Add(38, timeAngleDict[tmTimeDict[38]]);
            tempAngleDict.Add(40, timeAngleDict[tmTimeDict[40]]);
        }
        #endregion


        #region 升降电机按钮
        /// <summary>
        /// 伺服开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PowerSwitchBtn_upDown_Click(object sender, EventArgs e)
        {
            if (powerState_upDown)
                formMain.bpq.write_coil(powerAddress_upDown, false, 5);
            else
                formMain.bpq.write_coil(powerAddress_upDown, true, 5);
        }

        private void ForwardBtn_upDown_MouseClick(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                if (forwardState_upDown)
                    formMain.bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                else
                    formMain.bpq.write_coil(forwardWriteAddress_upDown, true, 5);
            }
        }

        private void NoForwardBtn_upDown_MouseClick(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                if (noForwadState_upDown)
                    formMain.bpq.write_coil(noForwardWriteAddress_upDown, false, 5);
                else
                    formMain.bpq.write_coil(noForwardWriteAddress_upDown, true, 5);
            }
        }

        private void OrignBtn_upDown_MouseClick(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                if (orignState_upDown)
                    formMain.bpq.write_coil(orignWriteAddress_upDown, false, 5);
                else
                    formMain.bpq.write_coil(orignWriteAddress_upDown, true, 5);
            }
        }

        private void ShutdownBtn_upDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                formMain.bpq.write_coil(shutdownAddress_upDown, true, 5);
            }
        }

        private void ShutdownBtn_upDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                formMain.bpq.write_coil(shutdownAddress_upDown, false, 5);
            }
        }

        private void BackOrignBtn_upDown_Click(object sender, EventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                formMain.bpq.write_coil(backOrignAddress_upDown, true, 5);
            }
        }

        private void AutoRunBtn_upDown_Click(object sender, EventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                formMain.bpq.write_coil(autoRunAddress_upDown, true, 5);
            }
        }


        private void RadioBtn_upDown_Click(object sender, EventArgs e)
        {
            uint val2 = (uint)(double.Parse(radioTb_upDown.Text) * 4000);
            if (val2 < 0 || val2 > 20000)
            {
                MessageBox.Show("请输入0-5 范围内的值");
                radioTb_upDown.Text = "";
            }
            else
            {

                formMain.Write_uint(radioAddress_upDown, val2, 5);
                SystemInfoPrint("写入：【" + radioAddress_upDown + "】【" + val2 + "】\n");
            }
        }
        private void AngleBtn_upDown_Click(object sender, EventArgs e)
        {
            try
            {
                autoFindAngle_upDown = angleTb_upDown.Text.ToDouble();
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入正确的格式");
                return;
            }
        }


        #endregion

        /// <summary>
        /// 角度与时间的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "文档|*.csv";
            fileDialog.InitialDirectory = Application.StartupPath;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTableUtils.DataTableToCsvT(timeAngleDt, fileDialog.FileName);
                MessageBox.Show("保存成功!");

            }
            fileDialog.Dispose();
        }

        /// <summary>
        /// 温度与时间的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "文档|*.csv";
            fileDialog.InitialDirectory = Application.StartupPath;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTableUtils.DataTableToCsvT(formMain.ElectDt, fileDialog.FileName);
                MessageBox.Show("保存成功!");
            }
            fileDialog.Dispose();
        }

    }
}
