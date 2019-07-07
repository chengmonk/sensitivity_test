using Automation.BDaq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 恒温测试机.App;
using 恒温测试机.Model.Enum;
using 恒温测试机.Utils;

namespace 恒温测试机.UI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            InitControl();
            InitData();
            InitTimer();
        }

        #region 变量
        LogicTypeEnum logicType = LogicTypeEnum.safeTest;
        TestStandardEnum testStandard = TestStandardEnum.default1711;

        System.Timers.Timer safetyTimer;             //安全性测试 定时器
        System.Timers.Timer pressureTimer;           //压力变化测试 定时器
        System.Timers.Timer coolTimer;               //降温测试 定时器
        System.Timers.Timer steadyTimer;             //温度稳定性测试 定时器
        System.Timers.Timer flowTimer;               //流量减少测试 定时器

        System.Timers.Timer monitorTimer;            //监控阀门定时器
        DAQ_profile collectData;
        DAQ_profile control;
        private byte[] doData = new byte[4];    //数字量输出数据
        private byte[] diData = new byte[4];    //数字量输入数据
        double[] aoData = new double[2];           //模拟量输出数据

        private config collectConfig;
        private config controlConfig;
        public const int CHANNEL_COUNT_MAX = 16;
        private double[] m_dataScaled = new double[CHANNEL_COUNT_MAX];

        bool collectDataFlag = false;
        bool runFlag = false;
        bool heatFlag = false;
        bool coolFlag = false;
        bool graphFlag = true;

        public static DataTable dt;
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

        int index;
        int startIndex;
        int endIndex;

        public Dictionary<string, DataTable> analyseDataDic;
        public Dictionary<LogicTypeEnum, string> analyseReportDic = new Dictionary<LogicTypeEnum, string>();
        public DataReportAnalyseApp dataReportAnalyseApp;

        #endregion

        #region 委托

        private delegate void MonitorActiveDelegate(byte[] data);//控制阀的状态监控
        private void MonitorActive(byte[] data)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MonitorActiveDelegate monitorActiveDelegate = MonitorActive;
                    this.Invoke(monitorActiveDelegate, new object[] { data });
                }
                else
                {
                    if (doData[0].get_bit(0) == 1)
                        Temp1Status.ForeColor = Color.Red;
                    else
                        Temp1Status.ForeColor = Color.Black;

                    if (doData[0].get_bit(1) == 1)
                        Temp2Status.ForeColor = Color.Red;
                    else
                        Temp2Status.ForeColor = Color.Black;
                    if (doData[0].get_bit(2) == 1)
                        Temp3Status.ForeColor = Color.Red;
                    else
                        Temp3Status.ForeColor = Color.Black;
                    if (doData[0].get_bit(3) == 1)
                        Temp4Status.ForeColor = Color.Red;
                    else
                        Temp4Status.ForeColor = Color.Black;
                    if (doData[0].get_bit(4) == 1)
                        Temp5Status.ForeColor = Color.Red;
                    else
                        Temp5Status.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
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
                    systemInfoTb.AppendText("[时间:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "] " + msg);
                    systemInfoTb.AppendText("\n");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return;
            }

        }

        private delegate void DataReadyDelegate();//数据采集委托  
        private void DataReadyToUpdateStatus()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    DataReadyDelegate dataReadyDelegate = DataReadyToUpdateStatus;
                    this.Invoke(dataReadyDelegate);
                }
                else
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

                    if (logicType == LogicTypeEnum.safeTest || logicType == LogicTypeEnum.PressureTest)
                    {
                        if (Qm > (double)Properties.Settings.Default.QmMax || Qm < (double)Properties.Settings.Default.QmMin)
                        {
                            QmShow.ForeColor = Color.Red;
                            systemInfoTb.AppendText("[时间:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "] " + "[出水流量Qm" + "超出上下限！]");
                            systemInfoTb.AppendText("\n");
                        }
                        else
                        {
                            QmShow.ForeColor = Color.Black;
                        }
                    }
                    DataReadyToControlTemp();
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
                return;
            }
            
        }
        private void DataReadyToControlTemp()
        {
            if (doData[0].get_bit(0) == 0)//制冷控温
            {
                if (Temp1 <= (double)(Properties.Settings.Default.Temp1Set + Properties.Settings.Default.Temp1Range))
                    Temp1Status.Text = "水温:" + Temp1 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp1Status.Text = "水温:" + Temp1 + "℃\n" + "状态:" + "制冷中";
                    doData[0].set_bit(0, true);
                }
            }
            else
            {
                if (Temp1 <= (double)(Properties.Settings.Default.Temp1Set))
                {
                    Temp1Status.Text = "水温:" + Temp1 + "℃\n" + "状态:" + "保持温度";
                    doData[0].set_bit(0, false);
                }
                else
                {
                    Temp1Status.Text = "水温:" + Temp1 + "℃\n" + "状态:" + "制冷中";
                }
            }

            if (doData[0].get_bit(1) == 0)//制热控温
            {
                if (Temp2 >= (double)(Properties.Settings.Default.Temp2Set - Properties.Settings.Default.Temp2Range))
                    Temp2Status.Text = "水温:" + Temp2 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp2Status.Text = "水温:" + Temp2 + "℃\n" + "状态:" + "加热中";
                    doData[0].set_bit(1, true);
                }
            }
            else
            {
                if (Temp2 >= (double)(Properties.Settings.Default.Temp2Set))
                {
                    Temp2Status.Text = "水温:" + Temp2 + "℃\n" + "状态:" + "保持温度";
                    doData[0].set_bit(1, false);
                }
                else
                {
                    Temp2Status.Text = "水温:" + Temp2 + "℃\n" + "状态:" + "加热中";
                }
            }

            if (doData[0].get_bit(2) == 0)//制热控温
            {
                if (Temp3 >= (double)(Properties.Settings.Default.Temp3Set - Properties.Settings.Default.Temp3Range))
                    Temp3Status.Text = "水温:" + Temp3 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp3Status.Text = "水温:" + Temp3 + "℃\n" + "状态:" + "加热中";
                    doData[0].set_bit(2, true);
                }
            }
            else
            {
                if (Temp3 >= (double)(Properties.Settings.Default.Temp3Set))
                {
                    Temp3Status.Text = "水温:" + Temp3 + "℃\n" + "状态:" + "保持温度";
                    doData[0].set_bit(2, false);
                }
                else
                {
                    Temp3Status.Text = "水温:" + Temp3 + "℃\n" + "状态:" + "加热中";
                }
            }

            if (doData[0].get_bit(3) == 0)//制热控温
            {
                if (Temp4 >= (double)(Properties.Settings.Default.Temp4Set - Properties.Settings.Default.Temp4Range))
                    Temp4Status.Text = "水温:" + Temp4 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp4Status.Text = "水温:" + Temp4 + "℃\n" + "状态:" + "加热中";
                    doData[0].set_bit(3, true);
                }
            }
            else
            {
                if (Temp4 >= (double)(Properties.Settings.Default.Temp4Set))
                {
                    Temp4Status.Text = "水温:" + Temp4 + "℃\n" + "状态:" + "保持温度";
                    doData[0].set_bit(3, false);
                }
                else
                {
                    Temp4Status.Text = "水温:" + Temp4 + "℃\n" + "状态:" + "加热中";
                }
            }

            if (doData[0].get_bit(4) == 0)//制冷控温
            {
                if (Temp5 <= (double)(Properties.Settings.Default.Temp5Set + Properties.Settings.Default.Temp5Range))
                    Temp5Status.Text = "水温:" + Temp5 + "℃\n" + "状态:" + "保持温度";
                else
                {
                    Temp5Status.Text = "水温:" + Temp5 + "℃\n" + "状态:" + "制冷中";
                    doData[0].set_bit(4, true);
                }
            }
            else
            {
                if (Temp5 <= (double)(Properties.Settings.Default.Temp5Set))
                {
                    Temp5Status.Text = "水温:" + Temp5 + "℃\n" + "状态:" + "保持温度";
                    doData[0].set_bit(4, false);
                }
                else
                {
                    Temp5Status.Text = "水温:" + Temp5 + "℃\n" + "状态:" + "制冷中";
                }
            }

            control.InstantDo_Write(doData);
        }

        private delegate void DataGraphDelegate();//实时曲线委托
        private void DataGraph()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    DataGraphDelegate dataGranphDelegate = DataGraph;
                    this.Invoke(dataGranphDelegate);
                }
                else
                {
                    hslCurve1.AddCurveData(
                                new string[] { "冷水流量Qc", "热水流量Qh", "出水流量Qm", "冷水温度Tc", "热水温度Th", "出水温度Tm",
                                    "冷水压力Pc","热水压力Ph","出水压力Pm","出水重量Qm5" },
                                new float[]
                                {
                        (float)Qc,(float)Qm,(float)Tc,(float)Th,(float)Tm,(float)Pc,(float)Ph,(float)Pm,(float)Qm5
                                }
                            );
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
                return;
            }
        }

        private delegate void HideOrShowCurveDelegate();
        private void HideOrShowCurve()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    HideOrShowCurveDelegate hideOrShowCurveDelegate = HideOrShowCurve;
                    this.Invoke(hideOrShowCurveDelegate);
                }
                else
                {
                    if (graphFlag)
                    {
                        hslCurve1.RemoveAllCurveData();
                        hslCurve1.Show();
                    }
                    else
                    {
                        hslCurve1.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return;
            }
        }

        #endregion

        #region UI相关
        AutoSizeFormClass asc = new AutoSizeFormClass();

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            standardCbx.Text = "EN1111-2017";
            //safeTestRbt.Checked = true;
            hslCurve1.SetLeftCurve("冷水流量Qc", null, Color.DodgerBlue);
            hslCurve1.SetLeftCurve("热水流量Qh", null, Color.DarkOrange);
            hslCurve1.SetLeftCurve("出水流量Qm", null, Color.Green);
            hslCurve1.SetLeftCurve("冷水温度Tc", null, Color.DarkOrange);
            hslCurve1.SetLeftCurve("热水温度Th", null, Color.DodgerBlue);
            hslCurve1.SetLeftCurve("出水温度Tm", null, Color.DarkOrange);
            hslCurve1.SetLeftCurve("冷水压力Pc", null, Color.DodgerBlue);
            hslCurve1.SetLeftCurve("热水压力Ph", null, Color.DarkOrange);
            hslCurve1.SetLeftCurve("出水压力Pm", null, Color.DodgerBlue);
            hslCurve1.SetLeftCurve("出水重量Qm5", null, Color.DarkOrange);
            hslCurve1.TextAddFormat = "hh:mm:ss:fff";
        }

        /// <summary>
        /// 初始化数据采集
        /// </summary>
        private void InitData()
        {
            systemInfoTb.Text = "";
            dt = new DataTable();
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
            dt.Columns.Add("索引", typeof(int));
            collectConfig = new config();
            collectConfig.channelCount = 16;
            collectConfig.convertClkRate = 100;
            collectConfig.deviceDescription = "DemoDevice,BID#0";//"PCI-1710HG,BID#0";//选择设备以这个为准，不用考虑设备序号            
            collectConfig.sectionCount = 0;//The 0 means setting 'streaming' mode.
            collectConfig.sectionLength = 100;
            collectConfig.startChannel = 0;

            controlConfig = new config();
            controlConfig.deviceDescription = "DemoDevice,BID#0";
            controlConfig.sectionCount = 0;//The 0 means setting 'streaming' mode.

            collectData = new DAQ_profile(0, collectConfig);
            collectData.InstantAo();
            collectData.InstantAo_Write(aoData);//输出模拟量函数

            control = new DAQ_profile(0, controlConfig);
            control.InstantDo();
            control.InstantDi();

            for (int i = 0; i < 4; i++)     //初始化数字量输出
            {
                doData[i] = 0x00;
            }
            control.InstantDo_Write(doData);//输出数字量函数
            control.InstantDi();
            diData[0] = control.InstantDi_Read();//读取数字量函数
            //Log.Info("diData:" + diData[0]);
            WaveformAi();//
            WaveformAiCtrl1_Start();//开始高速读取模拟量数据

        }

        /// <summary>
        /// 初始化计时器
        /// </summary>
        private void InitTimer()
        {
            monitorTimer = new System.Timers.Timer(100);
            monitorTimer.Elapsed += (o, a) =>
            {
                MonitorActive(doData);
            };//到达时间的时候执行事件； 
            monitorTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            monitorTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            safetyTimer = new System.Timers.Timer(2);
            safetyTimer.Elapsed += new System.Timers.ElapsedEventHandler(SafetyTimer_Action);//到达时间的时候执行事件； 
            safetyTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            safetyTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            pressureTimer = new System.Timers.Timer(2);//87/4
            pressureTimer.Elapsed += new System.Timers.ElapsedEventHandler(PressureTimer_Action);//到达时间的时候执行事件； 
            pressureTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            pressureTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件； 

            coolTimer = new System.Timers.Timer(2);
            coolTimer.Elapsed += new System.Timers.ElapsedEventHandler(CoolTimer_Action);//到达时间的时候执行事件； 
            coolTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            coolTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            steadyTimer = new System.Timers.Timer(2);
            steadyTimer.Elapsed += new System.Timers.ElapsedEventHandler(SteadyTimer_Action);//到达时间的时候执行事件； 
            steadyTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            steadyTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            flowTimer = new System.Timers.Timer(2);
            flowTimer.Elapsed += new System.Timers.ElapsedEventHandler(FlowTimer_Action);//到达时间的时候执行事件； 
            flowTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            flowTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；
        }

        /// <summary>
        /// 切换定时器
        /// </summary>
        private void ChangeTimer()
        {
            if (logicType == LogicTypeEnum.safeTest)
            {
                safetyTimer.Enabled = true;
            }
            if (logicType == LogicTypeEnum.PressureTest)
            {
                pressureTimer.Enabled = true;
            }
            if (logicType == LogicTypeEnum.CoolTest)
            {
                coolTimer.Enabled = true;
            }
            if (logicType == LogicTypeEnum.TemTest)
            {
                steadyTimer.Enabled = true;
            }
            if (logicType == LogicTypeEnum.FlowTest)
            {
                flowTimer.Enabled = true;
            }
            graphFlag = true;
            HideOrShowCurve();
        }

        #endregion

        #region 窗体控件事件
        private void FormMain_Load(object sender, EventArgs e)
        {
            asc.Initialize(this);
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            asc.ReSize(this);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            monitorTimer.Enabled = false;
            monitorTimer.Dispose();
        }



        private void RadioBtn_CheckedChange(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
            {
                return;
            }
            if (runFlag)
            {
                MessageBox.Show("当前已有测试流程正在执行，请等待！");
                return;
            }
            //TODO:
            if (MessageBox.Show("确认切换子操作界面？注意保存数据", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                monitorTimer.Enabled = false;
                safetyTimer.Enabled = false;
                pressureTimer.Enabled = false;
                coolTimer.Enabled = false;
                steadyTimer.Enabled = false;
                flowTimer.Enabled = false;
                switch (((RadioButton)sender).Text.ToString())
                {
                    case "安全性测试":
                        logicType = LogicTypeEnum.safeTest;
                        break;
                    case "压力变化测试":
                        logicType = LogicTypeEnum.PressureTest;
                        break;
                    case "降温测试":
                        logicType = LogicTypeEnum.CoolTest;
                        break;
                    case "温度稳定性测试":
                        logicType = LogicTypeEnum.TemTest;
                        break;
                    case "流量减少测试":
                        logicType = LogicTypeEnum.FlowTest;
                        break;
                }
                InitData();
                ChangeTimer();
            }
        }

        private void StandardCbx_TextChanged(object sender, EventArgs e)
        {
            switch (((ComboBox)sender).Text.ToString())
            {
                case "EN1111-2017":
                    testStandard = TestStandardEnum.default1711;
                    break;
                case "自定义":
                    testStandard = TestStandardEnum.blank;
                    break;
            }
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            collectDataFlag = true;
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            collectDataFlag = false;
        }

        private void HeatingBtn_Click(object sender, EventArgs e)
        {
            if (heatFlag)
            {
                hslButton10.Text = "加热";
                doData[0].set_bit(1, false);
                doData[0].set_bit(2, false);
                doData[0].set_bit(3, false);
                heatFlag = false;
            }
            else
            {
                heatFlag = true;
                doData[0].set_bit(1, true);
                doData[0].set_bit(2, true);
                doData[0].set_bit(3, true);

                hslButton10.Text = "停止加热";
            }
        }

        private void CoolingBtn_Click(object sender, EventArgs e)
        {
            if (coolFlag)
            {
                hslButton9.Text = "制冷";
                doData[0].set_bit(0, false);
                doData[0].set_bit(4, false);
                coolFlag = false;
            }
            else
            {
                coolFlag = true;
                doData[0].set_bit(0, true);
                doData[0].set_bit(4, true);
                hslButton9.Text = "停止制冷";
            }
        }

        private void SetParamBtn_Click(object sender, EventArgs e)
        {
            Hide();
            System.Threading.Thread.Sleep(10);
            using (FormValueRangeSet form = new FormValueRangeSet())
            {
                form.ShowDialog();
            }
            System.Threading.Thread.Sleep(10);
            Show();
        }

        private void ExportReportBtn_Click(object sender, EventArgs e)
        {
            //TODO：导出测试报告
            DataReportExportApp exportApp = new DataReportExportApp(analyseReportDic);
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "文档|*.txt";
            fileDialog.InitialDirectory = Application.StartupPath;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (exportApp.Export(logicType))
                {
                    MessageBox.Show("导出成功");
                }
                else
                {
                    MessageBox.Show("导出失败，当前测试流程未有相应数据");
                }
            }
            fileDialog.Dispose();
        }

        private void SaveDataBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "文档|*.csv";
            fileDialog.InitialDirectory = Application.StartupPath;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTableUtils.DataTableToCsvT(dt, fileDialog.FileName);
                MessageBox.Show("保存成功!");
                index = 0;
            }
            fileDialog.Dispose();
        }

        private void ClearDataBtn_Click(object sender, EventArgs e)
        {
            systemInfoTb.Text = "";
            dt.Clear();
            index = 0;
        }

        private void SafetyTimer_Action(object source, System.Timers.ElapsedEventArgs e)
        {
            //安全性测试 流程  TODO：测试报告
            runFlag = true;
            analyseDataDic = new Dictionary<string, DataTable>();
            #region 启动a、c、11、011、12、021、vc、vh、vm 保持t1时间 然后关闭vc vm 打开v5
            doData[1].set_bit(7, true);//a
            doData[2].set_bit(1, true);//c
            doData[0].set_bit(5, true);//11
            doData[2].set_bit(7, true);//011
            doData[0].set_bit(6, true);//12
            doData[3].set_bit(1, true);//021
            doData[2].set_bit(3, true);//vc
            doData[2].set_bit(4, true);//vh
            doData[2].set_bit(5, true);//vm
            control.InstantDo_Write(doData);
            SystemInfoPrint("[初始化系统]\n");

            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));
            double orgPm = Pm;//在界面上显示初始压力，一次判断过后压力恢复到初始压力以后对温度进行判断
            //Log.Info("初始压力:" + Math.Round(orgPm, 2).ToString());

            doData[2].set_bit(3, false);//vc
            doData[2].set_bit(5, false);//vm
            doData[2].set_bit(6, true);//v5
            control.InstantDo_Write(doData);
            SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束，关闭Vc、Vm打开V5，开始冷水失效测试]\n");

            #endregion

            #region 冷水失效  持续t3后，打开VC Vm，同时关闭V5
            //测试标准：T5s内出水流量降至 1.9L/min 以下记录 Qm5
            //测试标准：T5s内出水温度应 ≤ 49℃

            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t2));//可调节时间 t2
            SystemInfoPrint("[t2 = " + Properties.Settings.Default.t2.ToString() + "s 延时结束，开始记录冷水失效数据]\n");

            dt.Rows.Add("开始采集冷水失效数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;   //开始收集数据
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t3));
            collectDataFlag = false;  //停止收集数据
            dt.Rows.Add("冷水失效数据采集完毕",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            endIndex = index;
            analyseDataDic.Add("冷水失效数据",DataTableUtils.SubDataTable(dt,startIndex,endIndex));
            SystemInfoPrint("[t3 = " + Properties.Settings.Default.t3.ToString() + " s 冷水测试阶段结束，停止记录数据。关闭V5，打开Vc、Vm，压力开始恢复]\n");

            doData[2].set_bit(3, true);//vc
            doData[2].set_bit(5, true);//vm
            doData[2].set_bit(6, false);//v5
            control.InstantDo_Write(doData);
            #endregion

            #region 压力回复初始压力后，开始收集数据 T5  
            //测试标准：混合水出水温度与所设定的温度偏差应 ≤ ±2 ℃  ？？
            for (; true;)
            {
                if (Math.Abs(Pc - (double)Properties.Settings.Default.CoolPump011) <= (double)Properties.Settings.Default.pressureThreshold)
                {
                    break;
                }
            }
            SystemInfoPrint("[压力恢复到初始压力，开始记录 5s 的数据]\n");
            dt.Rows.Add("开始采集冷水恢复数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;
            System.Threading.Thread.Sleep((int)(1000 * 5));//延时5s
            collectDataFlag = false;
            dt.Rows.Add("冷水恢复数据采集完毕",
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                index);
            index++;
            endIndex = index;
            analyseDataDic.Add("冷水恢复数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));
            SystemInfoPrint("[ 5s 的数据记录完毕]\n");
            #endregion

            #region t1后，关闭vh vm，同时打开v5
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));
            doData[2].set_bit(4, false);//vh
            doData[2].set_bit(5, false);//vm
            doData[2].set_bit(6, true);//v5
            control.InstantDo_Write(doData);
            SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束，关闭Vh、Vm打开V5，开始热水失效测试]\n");
            #endregion

            #region 热水失效  持续t3后，打开vh vm，同时关闭v5
            //测试标准：T5s内出水流量降至 1.9L/min 以下记录 Qm5
            //测试标准：T5s内出水温度应 ≤ 49℃

            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t2));//可调节时间 t2
            SystemInfoPrint("[t2 = " + Properties.Settings.Default.t2.ToString() + "s 延时结束，开始记录热水失效数据]\n");

            dt.Rows.Add("开始采集热水失效数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;   //开始收集数据
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t3));
            collectDataFlag = false;  //停止收集数据
            dt.Rows.Add("热水失效数据采集完毕",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            endIndex = index;
            analyseDataDic.Add("热水失效数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));
            SystemInfoPrint("[t3 = " + Properties.Settings.Default.t3.ToString() + " s 热水测试阶段结束，停止记录数据。关闭V5，打开Vh、Vm，压力开始恢复]\n");

            doData[2].set_bit(3, true);//vc
            doData[2].set_bit(5, true);//vm
            doData[2].set_bit(6, false);//v5
            control.InstantDo_Write(doData);
            #endregion

            #region 压力回复初始压力后，开始收集数据 T5
            //测试标准：混合水出水温度与所设定的温度偏差应 ≤ ±2 ℃  ？？
            for (; true;)
            {
                if (Math.Abs(Ph - (double)Properties.Settings.Default.HotPump021)<= (double)Properties.Settings.Default.pressureThreshold)
                {
                    break;
                }
            }
            SystemInfoPrint("[压力恢复到初始压力，开始记录 5s 的数据]\n");
            dt.Rows.Add("开始采集热水恢复数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;
            System.Threading.Thread.Sleep((int)(1000 * 5));//延时5s
            collectDataFlag = false;
            dt.Rows.Add("热水恢复数据采集完毕",
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                index);
            index++;
            endIndex = index;
            analyseDataDic.Add("热水恢复数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));
            SystemInfoPrint("[ 5s 的数据记录完毕]\n");
            #endregion

            MessageBox.Show("安全性测试结束，请注意保存数据！");
            dataReportAnalyseApp = new DataReportAnalyseApp(logicType,analyseDataDic);
            if (analyseReportDic.ContainsKey(logicType))
            {
                analyseReportDic[logicType] = dataReportAnalyseApp.AnalyseResult();
            }
            else
            {
                analyseReportDic.Add(logicType, dataReportAnalyseApp.AnalyseResult());
            }
            SystemInfoPrint(analyseReportDic[logicType]+"\n");
            runFlag = false;
            graphFlag = false;
            HideOrShowCurve();
        }

        private void PressureTimer_Action(object source,System.Timers.ElapsedEventArgs e)
        {
            runFlag = true;
            analyseDataDic = new Dictionary<string, DataTable>();
            //压力变化测试 流程 TODO：测试报告

            #region 启动a、c、11、011、12、012、022、021、vc、vh、vm 保持t1时间 然后关闭a 打开b
            doData[1].set_bit(7, true);//a
            doData[2].set_bit(0, false);//b  
            doData[2].set_bit(1, true);//c
            doData[2].set_bit(2, false);//d  
            doData[0].set_bit(5, true);//11
            doData[2].set_bit(7, true);//011
            doData[0].set_bit(6, true);//12
            doData[3].set_bit(0, true);//012
            doData[3].set_bit(2, true);//022
            doData[3].set_bit(1, true);//021
            doData[2].set_bit(3, true);//vc
            doData[2].set_bit(4, true);//vh
            doData[2].set_bit(5, true);//vm
            control.InstantDo_Write(doData);
            SystemInfoPrint("[初始化系统...]\n");

            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));
            //022压力切换为低压 用485切换    ???
            doData[1].set_bit(7, false);//a
            doData[2].set_bit(0, true);//b            
            control.InstantDo_Write(doData);
            SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束，关闭a打开b，开始压力变化测试-热水降压测试]\n");

            #endregion

            #region 热水降压 持续t3后打开a同时关闭b  022压力切换为高压 用485切换
            //测试标准：【TODO】
            //1、T5 秒内超过 3℃的时间不大于 T1.5 秒
            //2、T5 秒内低于 5℃的时间不大于 T1 秒
            //3、T5 秒后出水温度偏差 ≤ 2℃
            SystemInfoPrint("[等待压力到达设定值...]\n");
            for (; true;)
            {
                if (Math.Abs(Ph - (double)Properties.Settings.Default.PumpHotLow022) <=(double)Properties.Settings.Default.pressureThreshold)
                    break;
                System.Threading.Thread.Sleep((int)(100));
            }
            SystemInfoPrint("[压力达到设定值，开始记录热水降压数据]\n");
            dt.Rows.Add("开始采集热水降压测试数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t3));
            SystemInfoPrint("[t3 = " + Properties.Settings.Default.t3.ToString() + " s 热水降压测试阶段结束，停止记录数据。关闭b，打开a，压力开始恢复，022压力由低压切换为高压]\n");
            collectDataFlag = false;
            dt.Rows.Add("热水降压测试数据采集完毕",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            endIndex=index;
            analyseDataDic.Add("热水降压测试数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            doData[1].set_bit(7, true);//a
            doData[2].set_bit(0, false);//b 
            control.InstantDo_Write(doData);
            //022压力由低压切换为高压   ???
            #endregion

            #region 热水压力恢复到初始压力，开始记录 5s 的数据
            //测试标准：
            //1、T5 秒内混合水出水温度与所设定的温度偏差应 ≤ ±2 ℃
            for (; true;)
            {
                if (Math.Abs(Ph - (double)Properties.Settings.Default.HotPump021) <=
                   (double)Properties.Settings.Default.pressureThreshold) break;
                System.Threading.Thread.Sleep((int)(100));
            }
            SystemInfoPrint("[热水压力恢复到初始压力，开始记录 5s 的数据]\n");
            dt.Rows.Add("开始采集热水降压测试压力恢复数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;

            System.Threading.Thread.Sleep((int)(1000 * 5));//延时5s
            collectDataFlag = false;
            dt.Rows.Add("热水降压测试压力恢复数据采集完毕",
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           index);
            index++;
            endIndex = index;
            analyseDataDic.Add("热水降压恢复数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            SystemInfoPrint("[ 5s 的数据记录完毕]\n");
            #endregion

            #region t1 同时 关闭a 打开b
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));

            //022压力切换为高压 用485切换
            doData[1].set_bit(7, false);//a
            doData[2].set_bit(0, true);//b  
            control.InstantDo_Write(doData);

            //022高压切换           ???
            SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束，关闭a打开b，开始压力变化测试-热水升压测试]\n");

            #endregion

            #region 热水升压 持续t3后打开a同时关闭b  022压力切换为高压 用485切换
            //测试标准：【TODO】
            //1、T5 秒内超过 3℃的时间不大于 T1.5 秒
            //2、T5 秒内低于 5℃的时间不大于 T1 秒
            //3、T5 秒后出水温度偏差 ≤ 2℃
            SystemInfoPrint("[等待压力到达设定值...]\n");
            for (; true;)
            {
                if (Math.Abs(Ph - (double)Properties.Settings.Default.PumpHotLow022) <= (double)Properties.Settings.Default.pressureThreshold)
                    break;
                System.Threading.Thread.Sleep((int)(100));
            }
            SystemInfoPrint("[压力达到设定值，开始记录热水升压数据]\n");
            //开始收集数据
            dt.Rows.Add("开始采集热水升压测试数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t3));
            SystemInfoPrint("[t3 = " + Properties.Settings.Default.t3.ToString() + " s 热水升压测试阶段结束，停止记录数据。关闭b，打开a，压力开始恢复，022压力由低压切换为高压]\n");
            collectDataFlag = false;
            dt.Rows.Add("热水升压测试数据采集完毕",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            endIndex = index;
            analyseDataDic.Add("热水升压测试数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            doData[1].set_bit(7, true);//a
            doData[2].set_bit(0, false);//b 
            control.InstantDo_Write(doData);   //022压力切换为高压 用485切换  ???
            #endregion

            #region 热水压力恢复到初始压力，开始记录 5s 的数据
            //测试标准：
            //1、T5 秒内混合水出水温度与所设定的温度偏差应 ≤ ±2 ℃
            for (; true;)
            {
                if (Math.Abs(Ph - (double)Properties.Settings.Default.HotPump021) <=
                   (double)Properties.Settings.Default.pressureThreshold) break;
                System.Threading.Thread.Sleep((int)(100));
            }
            SystemInfoPrint("[热水压力恢复到初始压力，开始记录 5s 的数据]\n");
            dt.Rows.Add("开始采集热水升压测试压力恢复数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;

            System.Threading.Thread.Sleep((int)(1000 * 5));//延时5s
            collectDataFlag = false;
            dt.Rows.Add("热水升压测试压力恢复数据采集完毕",
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           index);
            index++;
            endIndex = index;
            analyseDataDic.Add("热水升压恢复数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            SystemInfoPrint("[ 5s 的数据记录完毕]\n");
            #endregion

            #region t1 同时 关闭 c 打开d
            doData[2].set_bit(1, true);//c
            doData[2].set_bit(2, false);//d 
            control.InstantDo_Write(doData);
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));

            //012压力切换为低压 用485切换
            doData[2].set_bit(1, false);//c
            doData[2].set_bit(2, true);//d 
            control.InstantDo_Write(doData);

            //012输出低压 485输出  ???
            SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束，关闭c打开d，开始压力变化测试-冷水降压测试]\n");

            #endregion

            #region 冷水降压 持续t3后打开c同时关闭d  012压力切换为高压 用485切换
            //测试标准：【TODO】
            //1、T5 秒内超过 3℃的时间不大于 T1.5 秒
            //2、T5 秒内低于 5℃的时间不大于 T1 秒
            //3、T5 秒后出水温度偏差 ≤ 2℃
            SystemInfoPrint("[等待压力到达设定值...]\n");

            for (; true;)
            {
                if (Math.Abs(Pc - (double)Properties.Settings.Default.PumpCoolLow012) <=
                    (double)Properties.Settings.Default.pressureThreshold)
                    break;
                System.Threading.Thread.Sleep((int)(100));
            }
            SystemInfoPrint("[压力达到设定值，开始记录冷水降压数据]\n");
            dt.Rows.Add("开始采集冷水降压测试数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;

            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t3));
            SystemInfoPrint("[t3 = " + Properties.Settings.Default.t3.ToString() + " s 冷水降压测试阶段结束，停止记录数据。关闭b，打开a，压力开始恢复，022压力由低压切换为高压]\n");
            collectDataFlag = false;
            dt.Rows.Add("冷水降压测试数据采集完毕",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            endIndex = index;
            analyseDataDic.Add("冷水降压测试数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            doData[2].set_bit(1, true);//c
            doData[2].set_bit(2, false);//d 
            //012输出高压 485输出     ???

            control.InstantDo_Write(doData);
            #endregion

            #region 冷水压力恢复到初始压力，开始记录 5s 的数据
            //测试标准：
            //1、T5 秒内混合水出水温度与所设定的温度偏差应 ≤ ±2 ℃
            for (; true;)
            {
                if (Math.Abs(Pc - (double)Properties.Settings.Default.CoolPump011) <=
                   (double)Properties.Settings.Default.pressureThreshold) break;
                System.Threading.Thread.Sleep((int)(100));
            }
            SystemInfoPrint("[冷水压力恢复到初始压力，开始记录 5s 的数据]\n");
            dt.Rows.Add("开始采集冷水降压测试压力恢复数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;
            System.Threading.Thread.Sleep((int)(1000 * 5));//延时5s
            collectDataFlag = false;
            dt.Rows.Add("冷水降压测试压力恢复数据采集完毕",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            endIndex = index;

            SystemInfoPrint("[ 5s 的数据记录完毕]\n");
            analyseDataDic.Add("冷水降压恢复数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            #endregion

            #region t1 同时 关闭c 打开d
            //冷水升压测试
            doData[2].set_bit(1, true);//c
            doData[2].set_bit(2, false);//d 
            //012输出高压 485输出

            control.InstantDo_Write(doData);
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));

            //012压力切换为高压 用485切换
            doData[2].set_bit(1, false);//c
            doData[2].set_bit(2, true);//d 

            control.InstantDo_Write(doData);
            SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束，关闭c打开d，开始压力变化测试-冷水升压测试]\n");

            #endregion

            #region 冷水升压 持续t3后打开a同时关闭b  022压力切换为高压 用485切换
            //测试标准：【TODO】
            //1、T5 秒内超过 3℃的时间不大于 T1.5 秒
            //2、T5 秒内低于 5℃的时间不大于 T1 秒
            //3、T5 秒后出水温度偏差 ≤ 2℃
            SystemInfoPrint("[等待压力到达设定值...]\n");
            for (; true;)
            {
                if (Math.Abs(Pc - (double)Properties.Settings.Default.PumpCoolHigh012) <=
                    (double)Properties.Settings.Default.pressureThreshold)
                    break;
                System.Threading.Thread.Sleep((int)(100));
            }
            SystemInfoPrint("[压力达到设定值，开始记录冷水升压数据]\n");
            //开始收集数据
            dt.Rows.Add("开始采集冷水升压测试数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t3));
            SystemInfoPrint("[t3 = " + Properties.Settings.Default.t3.ToString() + " s 冷水升压测试阶段结束，停止记录数据。关闭d，打开c，压力开始恢复，022压力由低压切换为高压]\n");
            //停止收集数据,持续t3后
            collectDataFlag = false;
            dt.Rows.Add("冷水升压测试数据采集完毕",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            endIndex = index;
            analyseDataDic.Add("冷水升压测试数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            doData[2].set_bit(1, true);//c
            doData[2].set_bit(2, false);//d            
            control.InstantDo_Write(doData);
            #endregion

            #region 冷水压力恢复到初始压力，开始记录 5s 的数据
            //测试标准：
            //1、T5 秒内混合水出水温度与所设定的温度偏差应 ≤ ±2 ℃
            for (; true;)
            {
                if (Math.Abs(Pc - (double)Properties.Settings.Default.CoolPump011) <=
                   (double)Properties.Settings.Default.pressureThreshold) break;
                System.Threading.Thread.Sleep((int)(100));
            }
            SystemInfoPrint("[冷水压力恢复到初始压力，开始记录 5s 的数据]\n");
            dt.Rows.Add("开始采集冷水升压测试压力恢复数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;
            System.Threading.Thread.Sleep((int)(1000 * 5));//延时5s
            collectDataFlag = false;
            dt.Rows.Add("冷水降压测试压力恢复数据采集完毕",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            endIndex = index;
            analyseDataDic.Add("冷水升压恢复数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            SystemInfoPrint("[ 5s 的数据记录完毕]\n");

            #endregion

            MessageBox.Show("压力变化测试结束，请注意保存数据！");

            dataReportAnalyseApp = new DataReportAnalyseApp(logicType, analyseDataDic);
            if (analyseReportDic.ContainsKey(logicType))
            {
                analyseReportDic[logicType] = dataReportAnalyseApp.AnalyseResult();
            }
            else
            {
                analyseReportDic.Add(logicType, dataReportAnalyseApp.AnalyseResult());
            }
            SystemInfoPrint(analyseReportDic[logicType] + "\n");

            runFlag = false;
            graphFlag = false;
            HideOrShowCurve();
        }

        private void CoolTimer_Action(object source,System.Timers.ElapsedEventArgs e)
        {
            runFlag = true;
            analyseDataDic = new Dictionary<string, DataTable>();
            //TODO：如何判断温度达到设定值？

            #region 启动a、c、11、011、12、021、vc、vh、vm 保持t1时间 然后关闭12 打开14
            doData[1].set_bit(7, true);//a
            doData[2].set_bit(1, true);//c
            doData[0].set_bit(5, true);//11
            doData[2].set_bit(7, true);//011
            doData[0].set_bit(6, true);//12
            doData[3].set_bit(1, true);//021
            doData[2].set_bit(3, true);//vc
            doData[2].set_bit(4, true);//vh
            doData[2].set_bit(5, true);//vm
            control.InstantDo_Write(doData);
            SystemInfoPrint("[初始化系统...]\n");

            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));

            doData[0].set_bit(6, false);//12
            doData[1].set_bit(6, true);//14            
            control.InstantDo_Write(doData);
            SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束，关闭12 打开14，开始降温测试]\n");
            #endregion

            #region 温度达到设定稳定温度后  持续t3后 关闭14 打开12
            //测试标准：【TODO】
            //1、T5 秒内超过 3℃的时间不大于 T1 秒
            //2、T5 秒出水温度波动 ≤ 1℃
            //3、T5 秒后出水温度偏差 ≤ 2℃
            SystemInfoPrint("[等待温度到达设定值...]\n");
            for (; true;)
            {
                if (true)       //???
                    break;
                System.Threading.Thread.Sleep((int)(100));
            }

            SystemInfoPrint("[温度达到设定值，开始记录降温测试数据]\n");
            dt.Rows.Add("开始采集降温测试数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            startIndex = index;
            index++;
            collectDataFlag = true;
            System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t3));
            SystemInfoPrint("[t3 = " + Properties.Settings.Default.t3.ToString() + " s 降温测试阶段结束，停止记录数据。关闭14，打开12]\n");
            collectDataFlag = false;
            dt.Rows.Add("降温测试数据采集完毕",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            endIndex = index;
            analyseDataDic.Add("降温测试数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            doData[0].set_bit(6, true);//12
            doData[1].set_bit(6, false);//14  
            control.InstantDo_Write(doData);
            #endregion

            #region 温度达到设定稳定温度后,开始记录40s 的数据
            //测试标准：
            //1、T40 秒后混合水出水温度与所设定的温度偏差应 ≤ ±2 ℃
            for (; true;)
            {
                if (true) break;                //???
                System.Threading.Thread.Sleep((int)(100));
            }
            SystemInfoPrint("[温度达到设定值，开始记录 45s 的数据]\n");
            dt.Rows.Add("开始采集降温测试恢复数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            collectDataFlag = true;

            System.Threading.Thread.Sleep((int)(1000 * 45));//延时40s
            collectDataFlag = false;
            dt.Rows.Add("降温测试恢复数据采集完毕",
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           index);
            index++;
            analyseDataDic.Add("降温测试恢复数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            SystemInfoPrint("[ 45s 的数据记录完毕]\n");
            #endregion

            dataReportAnalyseApp = new DataReportAnalyseApp(logicType, analyseDataDic);
            if (analyseReportDic.ContainsKey(logicType))
            {
                analyseReportDic[logicType] = dataReportAnalyseApp.AnalyseResult();
            }
            else
            {
                analyseReportDic.Add(logicType, dataReportAnalyseApp.AnalyseResult());
            }
            SystemInfoPrint(analyseReportDic[logicType] + "\n");

            runFlag = false;
            graphFlag = false;
            HideOrShowCurve();
        }

        private void SteadyTimer_Action(object source,System.Timers.ElapsedEventArgs e)
        {
            runFlag = true;
            analyseDataDic = new Dictionary<string, DataTable>();

            #region TODO 业务流程


            #region 记录 T35秒 的数据
            //1、T30秒后温度稳定后与所设定的温度偏差 ≤ 2℃
            //2、T30秒后温度稳定后的波动 ≤1℃
            SystemInfoPrint("[开始记录 35s 的数据]\n");
            dt.Rows.Add("开始采集流量减少测试恢复数据",
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            index);
            index++;
            collectDataFlag = true;

            System.Threading.Thread.Sleep((int)(1000 * 35));//延时40s
            collectDataFlag = false;
            dt.Rows.Add("流量减少测试恢复数据采集完毕",
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           0,
                           index);
            index++;
            analyseDataDic.Add("流量减少测试恢复数据", DataTableUtils.SubDataTable(dt, startIndex, endIndex));

            SystemInfoPrint("[ 35s 的数据记录完毕]\n");
            #endregion

            #endregion
            dataReportAnalyseApp = new DataReportAnalyseApp(logicType, analyseDataDic);
            if (analyseReportDic.ContainsKey(logicType))
            {
                analyseReportDic[logicType] = dataReportAnalyseApp.AnalyseResult();
            }
            else
            {
                analyseReportDic.Add(logicType, dataReportAnalyseApp.AnalyseResult());
            }
            SystemInfoPrint(analyseReportDic[logicType] + "\n");

            runFlag = false;
            graphFlag = false;
            HideOrShowCurve();
        }

        private void FlowTimer_Action(object source, System.Timers.ElapsedEventArgs e)
        {
            runFlag = true;
            analyseDataDic = new Dictionary<string, DataTable>();

            #region TODO 业务流程

            #endregion
            dataReportAnalyseApp = new DataReportAnalyseApp(logicType, analyseDataDic);
            if (analyseReportDic.ContainsKey(logicType))
            {
                analyseReportDic[logicType] = dataReportAnalyseApp.AnalyseResult();
            }
            else
            {
                analyseReportDic.Add(logicType, dataReportAnalyseApp.AnalyseResult());
            }
            SystemInfoPrint(analyseReportDic[logicType] + "\n");

            runFlag = false;
            graphFlag = false;
            HideOrShowCurve();
        }

        #endregion

        #region WaveformAiCtrl
        //private Automation.BDaq.WaveformAiCtrl waveformAiCtrl1;

        public void WaveformAi()
        {
            waveformAiCtrl1 = new Automation.BDaq.WaveformAiCtrl();
            waveformAiCtrl1.SelectedDevice = new DeviceInformation(collectConfig.deviceDescription);

            Conversion conversion = waveformAiCtrl1.Conversion;

            conversion.ChannelStart = collectConfig.startChannel;
            conversion.ChannelCount = collectConfig.channelCount;
            conversion.ClockRate = collectConfig.convertClkRate;
            Record record = waveformAiCtrl1.Record;
            record.SectionCount = collectConfig.sectionCount;//The 0 means setting 'streaming' mode.
            record.SectionLength = collectConfig.sectionLength;

            this.waveformAiCtrl1.Overrun += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.WaveformAiCtrl1_Overrun);
            this.waveformAiCtrl1.CacheOverflow += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.WaveformAiCtrl1_CacheOverflow);
            this.waveformAiCtrl1.DataReady += new System.EventHandler<Automation.BDaq.BfdAiEventArgs>(this.WaveformAiCtrl1_DataReady);

        }

        public void WaveformAiCtrl1_Start()
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
            Log.Info(m_dataScaled.Length.ToString());
        }

        public void WaveformAiCtrl1_Stop()
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

        private void WaveformAiCtrl1_DataReady(object sender,BfdAiEventArgs args)
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
                //Console.WriteLine("length:" + m_dataScaled.Length);

                DateTime t = DateTime.Now;
                t = t.AddSeconds(-1.0);//采集到的是一秒钟之前的数据，因此需要对当前的时间减去1s
                t.ToString("yyyy-MM-dd hh:mm:ss:fff");
                //Log.Info(t.ToString("yyyy-MM-dd hh:mm:ss:fff"));
                for (int i = 0; i < m_dataScaled.Length; i += 16)
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
                    if (graphFlag)
                    {
                        DataGraph();
                    }
                    if (collectDataFlag)
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
                            Qm5,
                            index);
                        index++;
                    }

                    t = t.AddMilliseconds(10.0);
                }
                DataReadyToUpdateStatus();
                if (err != ErrorCode.Success && err != ErrorCode.WarningRecordEnd)
                {
                    HandleError(err);
                    return;
                }
            }
            catch(Exception ex)
            {
                HandleError(err);
            }
        }

        

        private void WaveformAiCtrl1_Overrun(object sender, BfdAiEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void WaveformAiCtrl1_CacheOverflow(object sender, BfdAiEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("Sorry ! some errors happened, the error code is: " + err.ToString(), "AI_InstantAI");
            }
        }


        #endregion
    }
}
