using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Automation.BDaq;

namespace 恒温测试机
{
    public partial class FormSafeTest : Form
    {
        public FormSafeTest()
        {
            InitializeComponent();
        }



        DAQ_profile collectData;
        DAQ_profile control;
        private byte[] doData = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        private byte[] diData = new byte[4];
        double[] aoData = new double[2];

        private config collectConfig;
        private config controlConfig;
        public const int CHANNEL_COUNT_MAX = 16;
        private double[] m_dataScaled = new double[CHANNEL_COUNT_MAX];
        private void Form1_Load(object sender, EventArgs e)
        {
            Temp1Status.Text = "温度：10℃\n" + "状态：制冷中.";
            Temp2Status.Text = "温度：10℃\n" + "状态：加热中.";
            Temp3Status.Text = "温度：10℃\n" + "状态：加热中.";
            Temp4Status.Text = "温度：10℃\n" + "状态：加热中.";
            Temp5Status.Text = "温度：10℃\n" + "状态：无";
            collectConfig = new config();
            collectConfig.channelCount = 15;
            collectConfig.convertClkRate = 100;
            collectConfig.deviceDescription = "PCI-1710HG,BID#0";//选择设备以这个为准，不用考虑设备序号            
            collectConfig.sectionCount = 0;//The 0 means setting 'streaming' mode.
            collectConfig.sectionLength = 100;
            collectConfig.startChannel = 0;

            controlConfig = new config();
            controlConfig.deviceDescription = "PCI-1756,BID#0";
            controlConfig.sectionCount = 0;//The 0 means setting 'streaming' mode.

            collectData = new DAQ_profile(0, collectConfig);
            collectData.InstantAo();
            collectData.InstantAo_Write(aoData);//输出模拟量函数

            control = new DAQ_profile(0, controlConfig);
            control.InstantDo();
            control.InstantDi();
            for (int i = 0; i < 4; i++)
            {
                doData[i] = 0x00;
            }//初始化数字量输出
            control.InstantDo_Write(doData);//输出数字量函数
            control.InstantDi();
            diData[0] = control.InstantDi_Read();//读取数字量函数
            Console.WriteLine("didata:" + diData[0]);
            WaveformAi();//
            waveformAiCtrl1_Start();//开始高速读取模拟量数据

            loadData();
        }
        void loadData()
        {
            QmMax.Text = "上限:" + Properties.Settings.Default.QmMax;
            QcMax.Text = "上限:" + Properties.Settings.Default.QcMax;
            QhMax.Text = "上限:" + Properties.Settings.Default.QhMax;

            TmMax.Text = "上限:" + Properties.Settings.Default.TmMax;
            TcMax.Text = "上限:" + Properties.Settings.Default.TcMax;
            ThMax.Text = "上限:" + Properties.Settings.Default.ThMax;

            PmMax.Text = "上限:" + Properties.Settings.Default.PmMax;
            PcMax.Text = "上限:" + Properties.Settings.Default.PcMax;
            PhMax.Text = "上限:" + Properties.Settings.Default.PhMax;

            QmMin.Text = "下限:" + Properties.Settings.Default.QmMin;
            QcMin.Text = "下限:" + Properties.Settings.Default.QcMin;
            QhMin.Text = "下限:" + Properties.Settings.Default.QhMin;

            TmMin.Text = "下限:" + Properties.Settings.Default.TmMin;
            TcMin.Text = "下限:" + Properties.Settings.Default.TcMin;
            ThMin.Text = "下限:" + Properties.Settings.Default.ThMin;

            PmMin.Text = "下限:" + Properties.Settings.Default.PmMin;
            PcMin.Text = "下限:" + Properties.Settings.Default.PcMin;
            PhMin.Text = "下限:" + Properties.Settings.Default.PhMin;

        }

        private Automation.BDaq.WaveformAiCtrl waveformAiCtrl1;
        public void WaveformAi()
        {
            waveformAiCtrl1 = new Automation.BDaq.WaveformAiCtrl();
            waveformAiCtrl1.SelectedDevice = new DeviceInformation(collectConfig.deviceDescription);

            // waveformAiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("waveformAiCtrl1._StateStream")));

            Conversion conversion = waveformAiCtrl1.Conversion;

            conversion.ChannelStart = collectConfig.startChannel;
            conversion.ChannelCount = collectConfig.channelCount;
            conversion.ClockRate = collectConfig.convertClkRate;
            Record record = waveformAiCtrl1.Record;
            record.SectionCount = collectConfig.sectionCount;//The 0 means setting 'streaming' mode.
            record.SectionLength = collectConfig.sectionLength;

            this.waveformAiCtrl1.Overrun += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.waveformAiCtrl1_Overrun);
            this.waveformAiCtrl1.CacheOverflow += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.waveformAiCtrl1_CacheOverflow);
            this.waveformAiCtrl1.DataReady += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.waveformAiCtrl1_DataReady);

        }
        public void waveformAiCtrl1_Start()
        {
            ErrorCode err = ErrorCode.Success;
            err = waveformAiCtrl1.Prepare();//准备缓存区
            if (err == ErrorCode.Success)
            {
                err = waveformAiCtrl1.Start();
            }

            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }
            //System.Console.WriteLine(m_dataScaled.Length.ToString());
        }
        private void HandleError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("Sorry ! some errors happened, the error code is: " + err.ToString(), "AI_InstantAI");
            }
        }
        public void waveformAiCtrl1_Stop()
        {
            ErrorCode err = ErrorCode.Success;
            err = waveformAiCtrl1.Stop();
            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }
            Array.Clear(m_dataScaled, 0, m_dataScaled.Length);
        }
        private void waveformAiCtrl1_DataReady(object sender, BfdAiEventArgs args)
        {
            ErrorCode err = ErrorCode.Success;
            try
            {
                //The WaveformAiCtrl has been disposed.
                if (waveformAiCtrl1.State == ControlState.Idle)
                {
                    return;
                }
                if (m_dataScaled.Length < args.Count)
                {
                    m_dataScaled = new double[args.Count];
                }

                int chanCount = waveformAiCtrl1.Conversion.ChannelCount;
                int sectionLength = waveformAiCtrl1.Record.SectionLength;
                err = waveformAiCtrl1.GetData(args.Count, m_dataScaled);//读取数据     
                                                                        // Console.WriteLine("length:"+m_dataScaled.Length);
                DateTime t = DateTime.Now;
                //
                t.ToString("yyyy-MM-dd hh:mm:ss:fff");
                for (int i = 0; i < m_dataScaled.Length; i += 4)
                {

                    t = t.AddMilliseconds(10.0);
                }



                if (err != ErrorCode.Success && err != ErrorCode.WarningRecordEnd)
                {
                    HandleError(err);
                    return;
                }
                // System.Diagnostics.Debug.WriteLine("读取数据长度"+args.Count.ToString());

            }
            catch (System.Exception) { HandleError(err); }
        }

        private void waveformAiCtrl1_Overrun(object sender, BfdAiEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void waveformAiCtrl1_CacheOverflow(object sender, BfdAiEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void HslButton2_Click(object sender, EventArgs e)
        {
            Hide();
            System.Threading.Thread.Sleep(100);
            using (FormValueRangeSet form = new FormValueRangeSet())
            {
                form.ShowDialog();
            }
            System.Threading.Thread.Sleep(100);
            Show();
            loadData();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
