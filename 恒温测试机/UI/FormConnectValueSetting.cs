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

namespace 恒温测试机.UI
{
    public partial class FormConnectValueSetting : Form
    {
        private FormMain formMain;

        public FormConnectValueSetting(FormMain formMain)
        {
            InitializeComponent();
            InitControl();
            InitTimer();
            this.formMain = formMain;
        }

        private byte station = 1;

        private void InitControl()
        {
            //默认按键恒温电机
            type = ElectricalMachineryType.tempType;
            powerAddress = "2056";                  //伺服开关
            forwardWriteAddress = "2048";            //正传写入地址
            forwardReadAddress = "2053";             //正传读取地址
            noForwardWriteAddress = "2049";         //反传写入地址
            noForwardReadAddress = "2054";          //反传读取地址
            orignWriteAddress = "2050";             //原点写入地址
            orignReadAddress = "2055";              //原点读取地址
            autoRunAddress = "2051";                //自动运行地址
            backOrignAddress = "2052";              //回到原点地址
            shutdownAddress = "2057";               //紧急停止地址
            radioAddress = "4296";                  //转速
            angleAddress = "5432";                  //角度
        }

        private void InitTimer()
        {
            monitorTimer = new System.Timers.Timer(1000);
            monitorTimer.Elapsed += (o, a) =>
            {
                MonitorActive();
            };//到达时间的时候执行事件；
            monitorTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            monitorTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            monitorDTimer = new System.Timers.Timer(200);
            monitorDTimer.Elapsed += (o, a) =>
            {
                MonitorDActive();
            };//到达时间的时候执行事件；
            monitorDTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            monitorDTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

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
            formMain.Read(textBox2.Text,station);
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
            int val2 = Convert.ToInt32(textBox3.Text)*500;
            
            if (val2 < 0 || val2 > 5000)
            {
                MessageBox.Show("请输入0-10 范围内的值");
                textBox3.Text = "";
            }
            else
            {
                short val3 = Convert.ToInt16("" + val2);
                formMain.Write(textBox2.Text, val3, station);
            }
        }



        #region 电机通信

        private ElectricalMachineryType type;
        private string powerAddress = "";   //M8-2056   M18-2066
        private bool powerState = false;

        private string forwardWriteAddress = "";
        private string forwardReadAddress = "";
        private bool forwardState = false;

        private string noForwardWriteAddress = "";
        private string noForwardReadAddress = "";
        private bool noForwadState = false;

        private string orignWriteAddress = "";
        private string orignReadAddress = "";
        private bool orignState = false;

        private string autoRunAddress = "";
        private string backOrignAddress = "";
        private string shutdownAddress = "";

        private string radioAddress = "";
        private uint radioValue = 0;
        private string angleAddress = "";
        private int angleValue = 0;

        System.Timers.Timer monitorTimer;            //监控M寄存器定时器
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
                    powerState = formMain.bpq.read_coil(powerAddress,5);
                    forwardState = formMain.bpq.read_coil(forwardReadAddress, 5);
                    noForwadState = formMain.bpq.read_coil(noForwardReadAddress, 5);
                    orignState = formMain.bpq.read_coil(orignReadAddress, 5);
                    //radioValue = formMain.bpq.read_uint(radioAddress, 5);
                    //angleValue = formMain.bpq.read_int(angleAddress, 5);

                    powerBtn.BackColor = powerState ? Color.Green : DefaultBackColor;
                    powerBtn.Text = powerState ? "伺服开" : "伺服关";
                    forwardBtn.BackColor = forwardState?Color.Green:DefaultBackColor;
                    noForwardBtn.BackColor = noForwadState ? Color.Green : DefaultBackColor;
                    orignBtn.BackColor = orignState ? Color.Green : DefaultBackColor;
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }

        System.Timers.Timer monitorDTimer;            //监控D寄存器定时器
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
                    radioValue = formMain.bpq.read_uint(radioAddress, 5);   //转速  读取 uint
                    angleValue = formMain.bpq.read_int(angleAddress, 5);    //角度  读取 int

                    var temp1 = (radioValue * 0.0001);
                    var temp2 = (angleValue * 0.0001);
                    radioLb.Text = "" + temp1;
                    angelLb.Text = "" + temp2;
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }
        #endregion

        /// <summary>
        /// 伺服开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PowerSwitchBtn_Click(object sender, EventArgs e)
        {
            if (powerState)
                formMain.bpq.write_coil(powerAddress, false, 5);
            else
                formMain.bpq.write_coil(powerAddress, true, 5);
        }

        private void ElectCbx_TextChanged(object sender, EventArgs e)
        {
            switch (((ComboBox)sender).Text.ToString())
            {
                case "按键恒温电机":
                    type = ElectricalMachineryType.tempType;
                    powerAddress = "2056";              //私服开关
                    forwardWriteAddress = "2048";       //正传（写）
                    forwardReadAddress = "2053";        //正传（反）
                    noForwardWriteAddress = "2049";     //反传（写）
                    noForwardReadAddress = "2054";      //反传（读）
                    orignWriteAddress = "2050";         //原点（写）
                    orignReadAddress = "2055";          //原点（读）
                    autoRunAddress = "2051";            //自动运行
                    backOrignAddress = "2052";          //回到原点
                    shutdownAddress = "2057";           //紧急停止
                    radioAddress = "4296";              //转速
                    angleAddress = "2616";              //角度
                    break;
                case "按键流量电机":
                    type = ElectricalMachineryType.flowType;
                    powerAddress = "2066";
                    forwardWriteAddress = "2058";
                    forwardReadAddress = "2063";
                    noForwardWriteAddress = "2059";
                    noForwardReadAddress = "2064";
                    orignWriteAddress = "2060";
                    orignReadAddress = "2065";
                    autoRunAddress = "2061";
                    backOrignAddress = "2062";
                    shutdownAddress = "2067";
                    radioAddress = "4298";
                    angleAddress = "2618";
                    break;
            }
        }

        private void ForwardBtn_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(forwardWriteAddress, true, 5);
            }
        }

        private void ForwardBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(forwardWriteAddress, false, 5);
            }
        }

        private void NoForwardBtn_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(noForwardWriteAddress, true, 5);
            }
        }

        private void NoForwardBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(noForwardWriteAddress, false, 5);
            }
        }

        private void OrignBtn_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(orignWriteAddress, true, 5);
            }
        }

        private void OrignBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(orignWriteAddress, false, 5);
            }
        }

        private void ShutdownBtn_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(shutdownAddress, true, 5);
            }
        }

        private void ShutdownBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(shutdownAddress, false, 5);
            }
        }

        private void BackOrignBtn_Click(object sender, EventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(backOrignAddress, true, 5);
            }
        }

        private void AutoRunBtn_Click(object sender, EventArgs e)
        {
            if (!powerState)
            {
                MessageBox.Show("请开启伺服电机");
            }
            else
            {
                formMain.bpq.write_coil(autoRunAddress, true, 5);
            }
        }

    }
}
