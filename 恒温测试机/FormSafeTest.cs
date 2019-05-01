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

        bool startFlag = false;

        double Temp1;
        double Temp2;
        double Temp3;
        double Temp4;
        double Temp5;
        double Pm;
        double Pc;
        double Ph;
        double Tm;
        double Tc;
        double Th;
        double Qm;
        double Qc;
        double Qh;
        double Qm5;
        
        private DataTable dt;
        private delegate void myDelegate();//声明委托   
        private void Form1_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("时间", typeof(string));
            dt.Columns.Add("冷水流量Qc", typeof(double));   //新建第一列 通道0
            dt.Columns.Add("热水流量Qh", typeof(double));   //1
            dt.Columns.Add("出水流量Qm", typeof(double));   //2
            dt.Columns.Add("冷水温度Tc", typeof(double));   //3
            dt.Columns.Add("热水温度Th", typeof(double));   //4
            dt.Columns.Add("出水温度Tm", typeof(double));   //5
            dt.Columns.Add("冷水压力Pc", typeof(double));   //6
            dt.Columns.Add("热水压力Ph", typeof(double));   //7
            dt.Columns.Add("出水压力Pm", typeof(double));   //8
            dt.Columns.Add("出水重量Qm5", typeof(double));   //9
            //Temp1Status.Text = "温度：10℃\n" + "状态：制冷中.";
            //Temp2Status.Text = "温度：10℃\n" + "状态：加热中.";
            //Temp3Status.Text = "温度：10℃\n" + "状态：加热中.";
            //Temp4Status.Text = "温度：10℃\n" + "状态：加热中.";
            //Temp5Status.Text = "温度：10℃\n" + "状态：无";
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
                Console.WriteLine("length:" + m_dataScaled.Length);

                DateTime t = DateTime.Now;
                //
                //t = t.AddSeconds(-);
                t.ToString("yyyy-MM-dd hh:mm:ss:fff");
                Console.WriteLine("time:" + t.ToString("yyyy-MM-dd hh:mm:ss:fff"));
                for (int i = 0; i < m_dataScaled.Length; i += 15)
                {
                    Qc = Math.Round(m_dataScaled[i + 0], 2);
                    Qh = Math.Round(m_dataScaled[i + 1], 2);
                    Qm = Math.Round(m_dataScaled[i + 2], 2);
                    Tc = Math.Round(m_dataScaled[i + 3], 2);
                    Th = Math.Round(m_dataScaled[i + 4], 2);
                    Tm = Math.Round(m_dataScaled[i + 5], 2);
                    Pc = Math.Round(m_dataScaled[i + 6], 2);
                    Ph = Math.Round(m_dataScaled[i + 7], 2);
                    Pm = Math.Round(m_dataScaled[i + 8], 2);
                    Qm5 = Math.Round(m_dataScaled[i + 9], 2);
                    Temp1 = Math.Round(m_dataScaled[i + 10], 2);
                    Temp2 = Math.Round(m_dataScaled[i + 11], 2);
                    Temp3 = Math.Round(m_dataScaled[i + 12], 2);
                    Temp4 = Math.Round(m_dataScaled[i + 13], 2);
                    Temp5 = Math.Round(m_dataScaled[i + 14], 2);
                    if (startFlag)
                    {
                        dt.Rows.Add(t.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                            Qc,
                            Qh,
                            Qm,
                            Tc,
                            Th,
                            Tm,
                            Pc,
                            Ph,
                            Pm,
                            Qm5);
                    }

                    t = t.AddMilliseconds(10.0);
                }

                myDelegate md = new myDelegate(dataReadyUpdateForm);
                this.Invoke(md);

                if (err != ErrorCode.Success && err != ErrorCode.WarningRecordEnd)
                {
                    HandleError(err);
                    return;
                }
                // System.Diagnostics.Debug.WriteLine("读取数据长度"+args.Count.ToString());

            }
            catch (System.Exception) { HandleError(err); }
        }

        private void dataReadyUpdateForm()
        {
            QmShow.Text = Qm.ToString();
            QcShow.Text = Qc.ToString();
            QhShow.Text = Qh.ToString();
            TmShow.Text = Tm.ToString();
            TcShow.Text = Tc.ToString();
            ThShow.Text = Th.ToString();
            PmShow.Text = Pm.ToString();
            PcShow.Text = Pc.ToString();
            PhShow.Text = Ph.ToString();
            if (Qm > (double)Properties.Settings.Default.QmMax || Qm < (double)Properties.Settings.Default.QmMin)
            {
                QmAlarm.LanternBackground = Color.Red;
            }
            

            if (get_bit(doData[0], 0) == 0)//制冷控温
            {
                if (Temp1 <= (double)(Properties.Settings.Default.Temp1Set + Properties.Settings.Default.Temp1Range))
                    Temp1Status.Text = "水温:" + Temp1 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp1Status.Text = "水温:" + Temp1 + "℃\n" + "状态:" + "制冷中";
                    set_bit(doData[0], 0, true);
                }
            }
            else
            {
                if (Temp1 <= (double)(Properties.Settings.Default.Temp1Set))
                {
                    Temp1Status.Text = "水温:" + Temp1 + "℃\n" + "状态:" + "保持温度";
                    set_bit(doData[0], 0, false);
                }
                else
                {
                    Temp1Status.Text = "水温:" + Temp1 + "℃\n" + "状态:" + "制冷中";

                }
            }
            if (get_bit(doData[0], 1) == 0)//制热控温
            {
                if (Temp2 >= (double)(Properties.Settings.Default.Temp2Set - Properties.Settings.Default.Temp2Range))
                    Temp2Status.Text = "水温:" + Temp2 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp2Status.Text = "水温:" + Temp2 + "℃\n" + "状态:" + "加热中";
                    set_bit(doData[0], 1, true);
                }
            }
            else
            {
                if (Temp2 >= (double)(Properties.Settings.Default.Temp2Set))
                {
                    Temp2Status.Text = "水温:" + Temp2 + "℃\n" + "状态:" + "保持温度";
                    set_bit(doData[0], 1, false);
                }
                else
                {
                    Temp2Status.Text = "水温:" + Temp2 + "℃\n" + "状态:" + "加热中";

                }
            }
            if (get_bit(doData[0], 2) == 0)//制热控温
            {
                if (Temp3 >= (double)(Properties.Settings.Default.Temp3Set - Properties.Settings.Default.Temp3Range))
                    Temp3Status.Text = "水温:" + Temp3 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp3Status.Text = "水温:" + Temp3 + "℃\n" + "状态:" + "加热中";
                    set_bit(doData[0], 2, true);
                }
            }
            else
            {
                if (Temp3 >= (double)(Properties.Settings.Default.Temp3Set))
                {
                    Temp3Status.Text = "水温:" + Temp3 + "℃\n" + "状态:" + "保持温度";
                    set_bit(doData[0], 2, false);
                }
                else
                {
                    Temp3Status.Text = "水温:" + Temp3 + "℃\n" + "状态:" + "加热中";

                }
            }
            if (get_bit(doData[0], 3) == 0)//制热控温
            {
                if (Temp4 >= (double)(Properties.Settings.Default.Temp4Set - Properties.Settings.Default.Temp4Range))
                    Temp4Status.Text = "水温:" + Temp4 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp4Status.Text = "水温:" + Temp4 + "℃\n" + "状态:" + "加热中";
                    set_bit(doData[0], 3, true);
                }
            }
            else
            {
                if (Temp4 >= (double)(Properties.Settings.Default.Temp4Set))
                {
                    Temp4Status.Text = "水温:" + Temp4 + "℃\n" + "状态:" + "保持温度";
                    set_bit(doData[0], 3, false);
                }
                else
                {
                    Temp4Status.Text = "水温:" + Temp4 + "℃\n" + "状态:" + "加热中";

                }
            }
            if (get_bit(doData[0], 4) == 0)//制冷控温
            {
                if (Temp5 <= (double)(Properties.Settings.Default.Temp5Set + Properties.Settings.Default.Temp5Range))
                    Temp5Status.Text = "水温:" + Temp5 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp5Status.Text = "水温:" + Temp5 + "℃\n" + "状态:" + "制冷中";
                    set_bit(doData[0], 4, true);
                }
            }
            else
            {
                if (Temp5 <= (double)(Properties.Settings.Default.Temp5Set))
                {
                    Temp5Status.Text = "水温:" + Temp5 + "℃\n" + "状态:" + "保持温度";
                    set_bit(doData[0], 4, false);
                }
                else
                {
                    Temp5Status.Text = "水温:" + Temp5 + "℃\n" + "状态:" + "制冷中";

                }
            }
            control.InstantDo_Write(doData);
        }
        /// <summary>
        /// 设置某一位的值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index">要设置的位， 值从低到高为 0-7</param>
        /// <param name="flag">要设置的值 true / false</param>
        /// 
        /// <returns></returns>
        byte set_bit(byte data, int index, bool flag)
        {
            index++;
            if (index > 8 || index < 1)
                throw new ArgumentOutOfRangeException();
            int v = index < 2 ? index : (2 << (index - 2));
            return flag ? (byte)(data | v) : (byte)(data & ~v);
        }
        /// <summary>
        /// 获取数据中某一位的值
        /// </summary>
        /// <param name="input">传入的数据类型,可换成其它数据类型,比如Int</param>
        /// <param name="index">要获取的第几位的序号,从0开始 0-7</param>
        /// <returns>返回值为-1表示获取值失败</returns>
        int get_bit(byte input, int index)
        {
            //if (index > sizeof(byte))
            //{
            //    return -1;
            //}

            return ((input & (1 << index)) > 0) ? 1 : 0;
        }
        /// <summary>
        /// DataTable导出为CSV
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="strFilePath">路径</param>
        public static void dataTableToCsvT(System.Data.DataTable dt, string strFilePath)
        {
            if (dt == null || dt.Rows.Count == 0)   //确保DataTable中有数据
                return;
            string strBufferLine = "";
            StreamWriter strmWriterObj = new StreamWriter(strFilePath, false, System.Text.Encoding.Default);
            //写入列头
            foreach (System.Data.DataColumn col in dt.Columns)
                strBufferLine += col.ColumnName + ",";
            strBufferLine = strBufferLine.Substring(0, strBufferLine.Length - 1);
            strmWriterObj.WriteLine(strBufferLine);
            //写入记录
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strBufferLine = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j > 0)
                        strBufferLine += ",";
                    strBufferLine += dt.Rows[i][j].ToString().Replace(",", "");   //因为CSV文件以逗号分割，在这里替换为空
                }
                strmWriterObj.WriteLine(strBufferLine);
            }
            strmWriterObj.Close();
        }

        /// <summary>
        /// 读取CSV
        /// </summary>
        /// <param name="filePath">CSV路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        /// <returns></returns>
        public static System.Data.DataTable CsvToDataTable(string filePath, int n)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default, false);
            int m = 0;

            while (!reader.EndOfStream)
            {
                m = m + 1;
                string str = reader.ReadLine();
                string[] split = str.Split(',');
                if (m == n)
                {
                    System.Data.DataColumn column; //列名
                    for (int c = 0; c < split.Length; c++)
                    {
                        column = new System.Data.DataColumn();
                        column.DataType = System.Type.GetType("System.String");
                        column.ColumnName = split[c];
                        if (dt.Columns.Contains(split[c]))                 //重复列名处理
                            column.ColumnName = split[c] + c;
                        dt.Columns.Add(column);
                    }
                }
                if (m >= n + 1)
                {
                    System.Data.DataRow dr = dt.NewRow();
                    for (int i = 0; i < split.Length; i++)
                    {
                        dr[i] = split[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            reader.Close();
            return dt;
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
            System.Threading.Thread.Sleep(10);
            using (FormValueRangeSet form = new FormValueRangeSet())
            {
                form.ShowDialog();
            }
            System.Threading.Thread.Sleep(10);
            Show();
            loadData();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void HslButton1_Click(object sender, EventArgs e)
        {
            startFlag = true;
        }

        private void HslButton5_Click(object sender, EventArgs e)
        {
            startFlag = false;
        }

        private void HslButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "文档|*.csv";
            fileDialog.InitialDirectory = Application.StartupPath;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {              
                dataTableToCsvT(dt, fileDialog.FileName);
                MessageBox.Show("保存成功!");
            }
            fileDialog.Dispose();
        }
    }
}
