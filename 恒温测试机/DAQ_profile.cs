using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Automation.BDaq;
namespace 恒温测试机
{
    internal struct config
    {
        public string deviceDescription;
        public string profilePath;
        public int startChannel;
        public int channelCount;
        public int sectionLength;
        public int sectionCount;
        public double convertClkRate;
    }
    internal class DAQ_profile
    {
        private Automation.BDaq.WaveformAiCtrl waveformAiCtrl1;
        private Automation.BDaq.InstantAiCtrl instantAiCtrl1;
        private Automation.BDaq.InstantAoCtrl m_instantAoCtrl;
        private Automation.BDaq.EventCounterCtrl m_eventCounterCtrl;
        private Automation.BDaq.InstantDiCtrl instantDiCtrl1;
        private Automation.BDaq.InstantDoCtrl instantDoCtrl1;
        private System.ComponentModel.ComponentResourceManager resources;
        private int deviceNumber;
        config conf;
        public const int CHANNEL_COUNT_MAX = 16;
        private double[] m_dataScaled = new double[CHANNEL_COUNT_MAX];
        private byte[] portData = new byte[10];

        //double[] dataAO = new double[2];
        private int channelStart;
        private int channelCount;
        //构造函数 需要提供窗体里面的resource 
        public DAQ_profile(int deviceNumber, config conf, System.ComponentModel.ComponentResourceManager resources = null)
        {
            this.conf = conf;
            this.deviceNumber = deviceNumber;
            this.resources = resources;
        }

        public void InstantDo_Write(byte[] outData)
        {
            ErrorCode err = ErrorCode.Success;
            err = instantDoCtrl1.Write(0, outData.Length, outData);
            if (err != ErrorCode.Success)
            {
                HandleError(err);
            }
        }
        public void InstantDo(System.ComponentModel.IContainer components)
        {
            this.instantDoCtrl1 = new Automation.BDaq.InstantDoCtrl(components);
            this.instantDoCtrl1.SelectedDevice = new DeviceInformation(conf.deviceDescription);
        }
        public void InstantDo()
        {
            this.instantDoCtrl1 = new Automation.BDaq.InstantDoCtrl();
            this.instantDoCtrl1.SelectedDevice = new DeviceInformation(conf.deviceDescription);
        }
        public byte InstantDi_Read()
        {
            byte data = 0;
            int m_startPort = 0;
            ErrorCode err = ErrorCode.Success;
            for (int i = 0; (i + m_startPort) < instantDiCtrl1.Features.PortCount; ++i)
            {
                err = instantDiCtrl1.Read(i + m_startPort, out data);
                if (err != ErrorCode.Success)
                {

                    HandleError(err);
                    return 0;
                }
            }
            return data;
        }

        public void InstantDi(System.ComponentModel.IContainer components)
        {
            for (int i = 0; i < 10; i++)
            {
                portData[i] = 0;
            }
            this.instantDiCtrl1 = new Automation.BDaq.InstantDiCtrl(components);
            this.instantDiCtrl1.SelectedDevice = new DeviceInformation(conf.deviceDescription);
        }
        public void InstantDi()
        {
            for (int i = 0; i < 10; i++)
            {
                portData[i] = 0;
            }
            this.instantDiCtrl1 = new Automation.BDaq.InstantDiCtrl();
            this.instantDiCtrl1.SelectedDevice = new DeviceInformation(conf.deviceDescription);
        }
        public void EventCounter(System.ComponentModel.IContainer components)
        {
            this.m_eventCounterCtrl = new Automation.BDaq.EventCounterCtrl(components);
            this.m_eventCounterCtrl.SelectedDevice = new DeviceInformation(conf.deviceDescription);
        }

        public void EventCounter()
        {
            this.m_eventCounterCtrl = new Automation.BDaq.EventCounterCtrl();
            this.m_eventCounterCtrl.SelectedDevice = new DeviceInformation(conf.deviceDescription);
        }

        public void EventCount_Start()
        {
            m_eventCounterCtrl.Enabled = true;

        }

        public void EventCount_Stop()
        {
            m_eventCounterCtrl.Enabled = false;
        }

        public int EventCount_Read()
        {
            int value = 0;
            try
            {
                m_eventCounterCtrl.Enabled = true;
                // Show event counting value
                m_eventCounterCtrl.Read(out value);
                System.Console.WriteLine(m_eventCounterCtrl.SelectedDevice.Description + value);
                return value;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                return -1;
            }
        }

        public void InstantAo()
        {
            this.m_instantAoCtrl = new Automation.BDaq.InstantAoCtrl();
            this.m_instantAoCtrl.SelectedDevice = new DeviceInformation(conf.deviceDescription);
           // this.m_instantAoCtrl._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("m_instantAoCtrl._StateStream")));
        }

        public void InstantAo(System.ComponentModel.IContainer components)
        {
            this.m_instantAoCtrl = new Automation.BDaq.InstantAoCtrl(components);
            this.m_instantAoCtrl.SelectedDevice = new DeviceInformation(conf.deviceDescription);
            //this.m_instantAoCtrl._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("m_instantAoCtrl._StateStream")));
        }

        public void InstantAo_Write(double[] dataAO)
        {
            ErrorCode err = m_instantAoCtrl.Write(0, 1, dataAO);
            if (err != ErrorCode.Success)
            {
                HandleError(err);
            }
        }
        //动态读取数据初始化方法
        public void WaveformAi()
        {
            waveformAiCtrl1 = new Automation.BDaq.WaveformAiCtrl();
            waveformAiCtrl1.SelectedDevice = new DeviceInformation(conf.deviceDescription);
            waveformAiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("waveformAiCtrl1._StateStream")));

            Conversion conversion = waveformAiCtrl1.Conversion;
            conversion.ChannelStart = conf.startChannel;
            conversion.ChannelCount = conf.channelCount;
            conversion.ClockRate = conf.convertClkRate;
            Record record = waveformAiCtrl1.Record;
            record.SectionCount = conf.sectionCount;//The 0 means setting 'streaming' mode.
            record.SectionLength = conf.sectionLength;

            this.waveformAiCtrl1.Overrun += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.waveformAiCtrl1_Overrun);
            this.waveformAiCtrl1.CacheOverflow += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.waveformAiCtrl1_CacheOverflow);
            this.waveformAiCtrl1.DataReady += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.waveformAiCtrl1_DataReady);

        }

        public void WaveformAi(System.ComponentModel.IContainer components)
        {
            waveformAiCtrl1 = new Automation.BDaq.WaveformAiCtrl(components);
            waveformAiCtrl1.SelectedDevice = new DeviceInformation(conf.deviceDescription);
           // waveformAiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("waveformAiCtrl1._StateStream")));
            // Step 4: Set necessary parameter
            Conversion conversion = waveformAiCtrl1.Conversion;
            conversion.ChannelStart = conf.startChannel;
            conversion.ChannelCount = conf.channelCount;
            conversion.ClockRate = conf.convertClkRate;
            Record record = waveformAiCtrl1.Record;
            record.SectionCount = conf.sectionCount;//The 0 means setting 'streaming' mode.
            record.SectionLength = conf.sectionLength;
            this.waveformAiCtrl1.Overrun += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.waveformAiCtrl1_Overrun);
            this.waveformAiCtrl1.CacheOverflow += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.waveformAiCtrl1_CacheOverflow);
            this.waveformAiCtrl1.DataReady += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.waveformAiCtrl1_DataReady);

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
        }
        public void waveformAiCtrl1_DataReady(object sender, BfdAiEventArgs args)
        {
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

                ErrorCode err = ErrorCode.Success;
                int chanCount = waveformAiCtrl1.Conversion.ChannelCount;
                int sectionLength = waveformAiCtrl1.Record.SectionLength;
                err = waveformAiCtrl1.GetData(args.Count, m_dataScaled);//读取数据
                                                                        // Test.emmmm();//在此处调用一个其他类的静态函数来对数据进行处理。
                if (err != ErrorCode.Success && err != ErrorCode.WarningRecordEnd)
                {
                    HandleError(err);
                    return;
                }
                System.Diagnostics.Debug.WriteLine(args.Count.ToString());

            }
            catch (System.Exception) { }
        }

        public void waveformAiCtrl1_CacheOverflow(object sender, BfdAiEventArgs e)
        {

        }

        public void waveformAiCtrl1_Overrun(object sender, BfdAiEventArgs e)
        {

        }

        public void InstantAi()
        {
            this.instantAiCtrl1 = new Automation.BDaq.InstantAiCtrl();
            //this.instantAiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantAiCtrl1._StateStream")));
            this.instantAiCtrl1.SelectedDevice = new DeviceInformation(conf.deviceDescription);
        }
      public void HandleError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("Sorry ! some errors happened, the error code is: " + err.ToString(), "AI_InstantAI");
            }
        }
        public double[] InstantAi_Read(int chanStart, int chanCount)
        {
            // ErrorCode err = instantAiCtrl1.Read(comboBox_chanStart.SelectedIndex, chanCountSet, m_dataScaled);
            ErrorCode err = instantAiCtrl1.Read(chanStart, chanCount, m_dataScaled);
            if (err != ErrorCode.Success)
            {
                HandleError(err);
            }

            return m_dataScaled;
        }

        public void InstantAi(System.ComponentModel.IContainer components)
        {
            this.instantAiCtrl1 = new Automation.BDaq.InstantAiCtrl(components);
            //this.instantAiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantAiCtrl1._StateStream")));
            instantAiCtrl1.SelectedDevice = new DeviceInformation(conf.deviceDescription);
        }
        private static void ShowErrorMessage(Exception e)
        {
            string errorInfo;
            errorInfo = "There's some error happened, the error information: ";
            MessageBox.Show(errorInfo + e.Message);
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
        

    }
}
