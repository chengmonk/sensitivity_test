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
using UnscentedKalmanFilter;
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
        LogicTypeEnum logicType = LogicTypeEnum.SensitivityTest;
        TestStandardEnum testStandard = TestStandardEnum.sensitivityProcess;

        System.Timers.Timer senstivityTimer;        //灵敏度测试 定时器
        System.Timers.Timer fidelityTimer;        //保真度测试 定时器
        System.Timers.Timer tmSteadyTimer;        //出水温度稳定性测试 定时器

        System.Timers.Timer monitorDiTimer;          //监控数字量定时器

        COMconfig bpq_conf;
        public M_485Rtu bpq;
        public DAQ_profile collectData;
        public DAQ_profile control;
        private config collectConfig;
        private config controlConfig;
        public const int CHANNEL_COUNT_MAX = 16;
        private double[] m_dataScaled = new double[CHANNEL_COUNT_MAX];

        public byte[] doData2 = new byte[2];    //数字量输出数据
        public byte[] diData2 = new byte[2];    //数字量输入数据
        double[] aoData = new double[2];           //模拟量输出数据

        bool collectDataFlag = false;           //是否采集数据
        bool runFlag = false;                   //是否执行流程

        bool graphFlag = true;          //先记录为true
        bool autoRunFlag = false;               //是否自动运行
        bool stopFlag = false;                  //是否手动停止

        public DataTable dt;
        public DataTable GraphDt;
        public DataTable ElectDt;
        public double Temp1;
        public double Temp2;
        public double Temp3;
        public double Temp4;
        public double Temp5;
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
        public double Wh;
        public double WhHeat;
        public double WhCool;

        int index;
        int startIndex;
        int endIndex;

        public Dictionary<string, DataTable> analyseDataDic;
        public Dictionary<LogicTypeEnum, string> analyseReportDic = new Dictionary<LogicTypeEnum, string>();
        public DataReportAnalyseApp dataReportAnalyseApp;

        #endregion

        #region 委托

        //diData【0】
        //伺服1报警：0-0
        //伺服2报警：1-1
        //冷水泵报警：2-2
        //热水泵报警：3-3

        public bool isAlarmS1 = false;//伺服1报警
        public bool isAlarmS2 = false;//伺服2报警
        public bool isAlarmCool = false;//冷水泵
        public bool isAlarmHeat = false;//热水泵
        private delegate void MonitorDiActiveDelegate();//数字量输入，进行对应行为逻辑
        private void MonitorDiActive()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MonitorDiActiveDelegate monitorDiActiveDelegate = MonitorDiActive;
                    this.Invoke(monitorDiActiveDelegate);
                }
                else
                {
                    diData2[0] = collectData.InstantDi_Read();//读取数字量函数
                    //监控数字量
                    if (diData2[0].get_bit(0) == 0)
                    {
                        //TODO 伺服1报警 
                        isAlarmS1 = true;
                    }
                    else
                    {
                        isAlarmS1 = false;
                    }

                    if (diData2[0].get_bit(1) == 0)
                    {
                        //TODO 伺服2报警
                        isAlarmS2 = true;
                    }
                    else
                    {
                        isAlarmS2 = false;
                    }

                    if (diData2[0].get_bit(2) == 0)
                    {
                        //TODO 冷水泵报警
                        isAlarmCool = true;
                    }
                    else
                    {
                        isAlarmCool = false;
                    }

                    if (diData2[0].get_bit(3) == 0)
                    {
                        //TODO 热水泵报警
                        isAlarmHeat = true;
                    }
                    else
                    {
                        isAlarmHeat = false;
                    }

                    if (isAlarmS1)
                        Console.WriteLine("伺服1报警");
                    if (isAlarmS2)
                        Console.WriteLine("伺服2报警");
                    if (isAlarmCool)
                        Console.WriteLine("冷水泵报警");
                    if (isAlarmHeat)
                        Console.WriteLine("热水泵报警");
                }
            }
            catch (Exception ex)
            {
                Log.Error("数字量输入报警异常：" + ex.ToString());
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

        private delegate void ChangeRadioButtonDelegate();          //自动切换按钮
        private void ChangeRadioButton()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ChangeRadioButtonDelegate changeRadioButtonDelegate = ChangeRadioButton;
                    this.Invoke(changeRadioButtonDelegate);
                }
                else
                {
                    //暂时屏蔽自动运行，由于需要手动确认全冷位置和全开状态
                    //if (sensitivityRbt.Checked)
                    //{
                    //    sensitivityRbt.Checked = false;
                    //    freRbt.Checked = true;
                    //}
                    //if (freRbt.Checked)
                    //{
                    //    freRbt.Checked = false;
                    //    TmSteadyRbt.Checked = true;
                    //}
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
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

                    Temp1Status.Text = Temp1 + "℃\n";
                    Temp2Status.Text = Temp2 + "℃\n";
                    Temp3Status.Text = Temp3 + "℃\n";
                    Temp4Status.Text = Temp4 + "℃\n";
                    Temp5Status.Text = Temp5 + "℃\n";

                    #region LED显示
                    hslLedQm.DisplayText = Qm.ToString();
                    hslLedQc.DisplayText = Qc.ToString();
                    hslLedQh.DisplayText = Qh.ToString();
                    hslLedTm.DisplayText = Tm.ToString();
                    hslLedTc.DisplayText = Tc.ToString();
                    hslLedTh.DisplayText = Th.ToString();
                    hslLedPm.DisplayText = Pm.ToString();
                    hslLedPc.DisplayText = Pc.ToString();
                    hslLedPh.DisplayText = Ph.ToString();
                    hslLedQm5.DisplayText = Qm5.ToString();
                    #endregion

                    for (int i = 3; i < 103; i++)
                    {
                        var qcTemp = CoolFlow[i - 3];
                        var qhTemp = HotFlow[i - 3];
                        var qmTemp = (sourceDataQm[i] - 1) < 0 ? 0 : (sourceDataQm[i] - 1) * 5;
                        hslCurve1.AddCurveData(
                            new string[] {
                                    //"冷水箱温度","热水箱温度","高温水箱温度","中温水箱温度","常温水箱温度"
                                    "冷水流量Qc", "热水流量Qh", "出水流量Qm",
                                    "冷水温度Tc", "热水温度Th", "出水温度Tm",
                                    "冷水压力Pc", "热水压力Ph", "出水压力Pm",
                                    //"出水重量Qm5"
                            },
                            new float[]
                            {
                                //(float)sourceDataTemp1[i],(float)sourceDataTemp2[i],(float)sourceDataTemp3[i]
                                //,(float)sourceDataTemp4[i],(float)sourceDataTemp5[i]
                                    //(float)Qc,(float)Qh,(float)Qm,
                                    //(float)Tc,(float)Th,(float)Tm,
                                    //(float)Pc,(float)Ph,(float)Pm,
                                    //(float)Qm5
                                    //(float)((sourceDataQc[i]-1)*12.5),(float)((sourceDataQh[i]-1)*12.5),(float)((sourceDataQm[i]-1)*12.5),         //流量 1-5V 对应 0-50L/min
                                     (float)qcTemp,
                                    (float)qhTemp,
                                    (float)qmTemp,
                                    (float)sourceDataTc[i]*10,(float)sourceDataTh[i]*10,(float)sourceDataTm[i]*10,
                                    (float)sourceDataPc[i],(float)sourceDataPh[i],(float)sourceDataPm[i],
                                    //(float)sourceDataQm5[i]
                            }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("数据采集异常:" + ex.ToString());
                return;
            }

        }
        public bool IsOpenDC = false;
        


        private delegate void StopDelegate();   //手动停止委托
        private void StopPro()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    StopDelegate stopDelegate = StopPro;
                    this.Invoke(stopDelegate);
                }
                else
                {
                    //将数字量输出置为0
                    doData2[0] = 0;
                    collectData.InstantDo_Write(doData2);
                    //当前采集的数据清空
                    analyseReportDic.Remove(logicType);
                    dt.Clear();
                    index = 0;
                    //输出面板清空
                    systemInfoTb.Text = "";

                    //弹窗提示
                    MessageBox.Show("当前测试流程已停止");
                    stopFlag = false;
                    return;
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
            sensitivityRbt.Visible = true;
            freRbt.Visible = true;
            TmSteadyRbt.Visible = true;
            //sensitivityRbt.Checked = true;
            //hslCurve1.SetLeftCurve("冷水箱温度", null, Color.OrangeRed);
            //hslCurve1.SetLeftCurve("热水箱温度", null, Color.Orchid);
            //hslCurve1.SetLeftCurve("高温水箱温度", null, Color.White);
            //hslCurve1.SetLeftCurve("中温水箱温度", null, Color.GreenYellow);
            //hslCurve1.SetLeftCurve("常温水箱温度", null, Color.BlueViolet);
            hslCurve1.SetLeftCurve("冷水流量Qc", null, Color.Red);
            hslCurve1.SetLeftCurve("热水流量Qh", null, Color.Orange);
            hslCurve1.SetLeftCurve("出水流量Qm", null, Color.Yellow);
            hslCurve1.SetLeftCurve("冷水温度Tc", null, Color.Green);
            hslCurve1.SetLeftCurve("热水温度Th", null, Color.Blue);
            hslCurve1.SetLeftCurve("出水温度Tm", null, Color.Purple);
            hslCurve1.SetLeftCurve("冷水压力Pc", null, Color.White);
            hslCurve1.SetLeftCurve("热水压力Ph", null, Color.DarkOrange);
            hslCurve1.SetLeftCurve("出水压力Pm", null, Color.DodgerBlue);
            //hslCurve1.SetLeftCurve("出水重量Qm5", null, Color.YellowGreen);
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
            dt.Columns.Add("分析时间", typeof(DateTime));
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
            dt.Columns.Add("液面高度Wh", typeof(double));   //10
            dt.Columns.Add("索引", typeof(int));

            GraphDt = new DataTable();
            GraphDt.Columns.Add("时间", typeof(string));
            GraphDt.Columns.Add("冷水流量Qc", typeof(double));   //新建第一列 通道0
            GraphDt.Columns.Add("热水流量Qh", typeof(double));   //1
            GraphDt.Columns.Add("出水流量Qm", typeof(double));   //2
            GraphDt.Columns.Add("冷水温度Tc", typeof(double));   //3
            GraphDt.Columns.Add("热水温度Th", typeof(double));   //4
            GraphDt.Columns.Add("出水温度Tm", typeof(double));   //5
            //GraphDt.Columns.Add("出水温度Tm2", typeof(double));   //5
            GraphDt.Columns.Add("冷水压力Pc", typeof(double));   //6
            GraphDt.Columns.Add("热水压力Ph", typeof(double));   //7
            GraphDt.Columns.Add("出水压力Pm", typeof(double));   //8
            GraphDt.Columns.Add("出水重量Qm5", typeof(double));   //9
            GraphDt.Columns.Add("液面高度Wh", typeof(double));   //10

            ElectDt = new DataTable();
            ElectDt.Columns.Add("时间", typeof(string));
            ElectDt.Columns.Add("冷水流量Qc", typeof(double));   //新建第一列 通道0
            ElectDt.Columns.Add("热水流量Qh", typeof(double));   //1
            ElectDt.Columns.Add("出水流量Qm", typeof(double));   //2
            ElectDt.Columns.Add("冷水温度Tc", typeof(double));   //3
            ElectDt.Columns.Add("热水温度Th", typeof(double));   //4
            ElectDt.Columns.Add("出水温度Tm", typeof(double));   //5
            ElectDt.Columns.Add("冷水压力Pc", typeof(double));   //6
            ElectDt.Columns.Add("热水压力Ph", typeof(double));   //7
            ElectDt.Columns.Add("出水压力Pm", typeof(double));   //8
            ElectDt.Columns.Add("出水重量Qm5", typeof(double));   //9
            ElectDt.Columns.Add("液面高度Wh", typeof(double));   //10

            bpq_conf.botelv = "19200";
            bpq_conf.zhanhao = "1";//站号
            bpq_conf.shujuwei = "8";
            bpq_conf.tingzhiwei = "1";
            bpq_conf.dataFromZero = true;
            bpq_conf.stringReverse = false;
            bpq_conf.COM_Name = "COM11";
            bpq_conf.checkInfo = 2;
            bpq_conf.dataFrame = 2;

            bpq = new M_485Rtu(bpq_conf);
            bpq.connect();

            collectConfig = new config();
            collectConfig.channelCount = 16;
            collectConfig.convertClkRate = 100;
            collectConfig.deviceDescription = "PCI-1710HG,BID#0";//"PCI-1710HG,BID#0";//选择设备以这个为准，不用考虑设备序号            
            collectConfig.sectionCount = 0;//The 0 means setting 'streaming' mode.
            collectConfig.sectionLength = 100;
            collectConfig.startChannel = 0;

            collectData = new DAQ_profile(0, collectConfig);
            collectData.InstantAo();
            collectData.InstantAo_Write(aoData);//输出模拟量函数
            collectData.InstantDo();
            collectData.InstantDi();
            //Example:
            //collectData.InstantDo_Write(doData2);//数字量输出            
            //diData2[0]= collectData.InstantDi_Read();//数字量输入   

            //灵敏度的机器只有1710板卡,数字量的输入输出只有16个通道，也就是说byte 类型的doData长度由4变为2  diData同理，此处需要进行修改下
            //controlConfig = new config();
            //controlConfig.deviceDescription = "PCI-1756,BID#0";
            //controlConfig.sectionCount = 0;//The 0 means setting 'streaming' mode.



            //control = new DAQ_profile(0, controlConfig);

            //collectData.InstantDo();
            //collectData.InstantDi();

            for (int i = 0; i < 2; i++)     //初始化数字量输出
            {
                doData2[i] = 0x00;
            }
            //collectData.InstantDo_Write(doData);//输出数字量函数
            //collectData.InstantDi();
            diData2[0] = collectData.InstantDi_Read();//读取数字量函数
            Console.WriteLine("D0" + diData2[0].get_bit(0));
            Console.WriteLine("D1" + diData2[0].get_bit(1));
            Console.WriteLine("D2" + diData2[0].get_bit(2));
            Console.WriteLine("D3" + diData2[0].get_bit(3));
            //Log.Info("diData:" + diData[0]);
            WaveformAi();//
            WaveformAiCtrl1_Start();//开始高速读取模拟量数据

        }

        /// <summary>
        /// 初始化计时器
        /// </summary>
        private void InitTimer()
        {
            monitorDiTimer = new System.Timers.Timer(5000);
            monitorDiTimer.Elapsed += (o, a) =>
            {
                MonitorDiActive();
            };//到达时间的时候执行事件； 
            monitorDiTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            monitorDiTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            senstivityTimer = new System.Timers.Timer(2);
            senstivityTimer.Elapsed += new System.Timers.ElapsedEventHandler(SenstivityTimer_Action);//到达时间的时候执行事件； 
            senstivityTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            senstivityTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            fidelityTimer = new System.Timers.Timer(2);
            fidelityTimer.Elapsed += new System.Timers.ElapsedEventHandler(FidelityTimer_Action);//到达时间的时候执行事件； 
            fidelityTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            fidelityTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            tmSteadyTimer = new System.Timers.Timer(2);
            tmSteadyTimer.Elapsed += new System.Timers.ElapsedEventHandler(TmSteadyTimer_Action);//到达时间的时候执行事件； 
            tmSteadyTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            tmSteadyTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；
        }

        /// <summary>
        /// 切换定时器
        /// </summary>
        private void ChangeTimer()
        {
            //TODO 注意屏蔽
            //if (logicType == LogicTypeEnum.SensitivityTest)
            //    senstivityTimer.Enabled = true;
            //if (logicType == LogicTypeEnum.FidelityTest)
            //    fidelityTimer.Enabled = true;
            //if (logicType == LogicTypeEnum.TmSteadyTest)
            //    tmSteadyTimer.Enabled = true;
            graphFlag = true;
            //HideOrShowCurve();
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
            doData2[0] = 0;
            collectData.InstantDo_Write(doData2);
            if (monitorDiTimer != null)
            {
                monitorDiTimer.Enabled = false;
                monitorDiTimer.Dispose();
            }

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
            if (autoRunFlag || (MessageBox.Show("确认切换子操作界面？注意保存数据", "", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                senstivityTimer.Enabled = false;
                fidelityTimer.Enabled = false;
                tmSteadyTimer.Enabled = false;
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
                    case "灵敏度测试":
                        logicType = LogicTypeEnum.SensitivityTest;
                        break;
                    case "保真度测试":
                        logicType = LogicTypeEnum.FidelityTest;
                        break;
                    case "出水温度稳定性测试":
                        logicType = LogicTypeEnum.TmSteadyTest;
                        break;
                }
                Console.WriteLine(logicType.ToDescription());
                //InitData();
                ChangeTimer();
            }
        }

        private void StandardCbx_TextChanged(object sender, EventArgs e)
        {
            switch (((ComboBox)sender).Text.ToString())
            {
                case "EN1111-2017":
                    testStandard = TestStandardEnum.default1711;
                    sensitivityRbt.Visible = false;
                    freRbt.Visible = false;
                    TmSteadyRbt.Visible = false;
                    break;
                case "灵敏度流程":
                    testStandard = TestStandardEnum.sensitivityProcess;
                    sensitivityRbt.Visible = true;
                    freRbt.Visible = true;
                    TmSteadyRbt.Visible = true;
                    break;
                case "自定义":
                    testStandard = TestStandardEnum.blank;
                    break;
            }
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            //collectDataFlag = true;
            if (runFlag)    //避免重复开启
            {
                return;
            }
            switch (logicType)
            {
                case LogicTypeEnum.SensitivityTest:
                    senstivityTimer.Enabled = true;
                    break;
                case LogicTypeEnum.FidelityTest:
                    fidelityTimer.Enabled = true;
                    break;
                case LogicTypeEnum.TmSteadyTest:
                    tmSteadyTimer.Enabled = true;
                    break;
            }
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            collectDataFlag = false;
            stopFlag = true;
            runFlag = false;
            autoRunFlag = false;
            doData2[0] = 0;
            collectData.InstantDo_Write(doData2);
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
                DataTableUtils.DataTableToCsvT(GraphDt, fileDialog.FileName);
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

        private void SenstivityTimer_Action(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                runFlag = true;
                graphFlag = true;
                MessageBox.Show("请确认电机已设置好相关参数！");

                #region 启动 011(冷水泵) 021(热水泵) a(热水阀) b(冷水阀)

                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                runFlag = false;
                graphFlag = false;
                if (autoRunFlag)
                {
                    ChangeRadioButton();
                }
            }
        }

        private void FidelityTimer_Action(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                runFlag = true;
                graphFlag = true;
                MessageBox.Show("请确认电机已设置好相关参数！");

                #region 启动 011(冷水泵) 021(热水泵) a(热水阀) b(冷水阀)

                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                runFlag = false;
                graphFlag = false;
                if (autoRunFlag)
                {
                    ChangeRadioButton();
                }
            }
        }

        private void TmSteadyTimer_Action(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                runFlag = true;
                graphFlag = true;
                MessageBox.Show("请确认电机已设置好相关参数！");

                #region 启动 011(冷水泵) 021(热水泵) a(热水阀) b(冷水阀)

                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                runFlag = false;
                graphFlag = false;
            }
        }


        public short Read(string address, byte station)
        {
            var val = bpq.read_short(address, station);
            return val;
        }


        public void Write_uint(string address, uint val, byte station)
        {

            bpq.write_uint(address, val, station);
        }
        public void Write_short(string address, short val, byte station)
        {
            bpq.write_short(address, val, station);

        }
        public void Write(string address, short val, byte station)
        {
            bpq.write_short(address, val, station);
        }

        public FormConnectValueSetting settingForm;
        private void ElectControlBtn_Click(object sender, EventArgs e)
        {
            settingForm = new FormConnectValueSetting(this);
            settingForm.Show();
        }

        private void GraphDataBtn_Click(object sender, EventArgs e)
        {
            if (GraphDt.Rows != null && GraphDt.Rows.Count > 1)
            {
                Log.Info("启动图像界面");
                FormPressureCurve form = new FormPressureCurve(GraphDt);
                form.Show();
            }
            else
            {
                MessageBox.Show("未采集到数据");
            }
        }

        private void AutoRunBtn_Click(object sender, EventArgs e)
        {
            //if (autoRunFlag)    //停止自动运行
            //{
            //    autoRunBtn.OriginalColor = Color.Lavender;
            //    autoRunFlag = false;
            //}
            //else        //启动自动运行
            //{
            //    autoRunFlag = true;
            //    if (runFlag)
            //    {
            //        MessageBox.Show("当前已有测试流程正在执行，请等待！");
            //        return;
            //    }
            //    else
            //    {
            //        autoRunBtn.OriginalColor = Color.Green;
            //        switch (testStandard)
            //        {
            //            case TestStandardEnum.default1711:
            //                safetyTimer.Enabled = true;
            //                break;
            //            case TestStandardEnum.sensitivityProcess:
            //                senstivityTimer.Enabled = true;
            //                break;
            //        }
            //    }
            //}
        }

        public bool electDataFlag = false;
        private void ShutDownBtn_Click(object sender, EventArgs e)
        {
            if (settingForm != null)
            {
                electDataFlag = false;

                settingForm.powerState_spin = false;
                bpq.write_coil(settingForm.shutdownAddress_spin, true, 5);
                System.Threading.Thread.Sleep((int)(200));
                bpq.write_coil(settingForm.shutdownAddress_spin, false, 5);

                settingForm.powerState_upDown = false;
                bpq.write_coil(settingForm.shutdownAddress_upDown, true, 5);
                System.Threading.Thread.Sleep((int)(200));
                bpq.write_coil(settingForm.shutdownAddress_upDown, false, 5);
            }
        }

        private void DOControlBtn_Click(object sender, EventArgs e)
        {
            //IsOpenDC = true;
            //FormDOControl doControlForm = new FormDOControl(this);
            //doControlForm.Show();
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
            //Log.Info(m_dataScaled.Length.ToString());
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
        private double[] lastTempData = new double[90];

        private double[] sourceDataQc = new double[106];


        private double[] sourceDataQh = new double[106];

        private double[] sourceDataQm = new double[106];

        private double[] sourceDataTh = new double[106];

        private double[] sourceDataTm = new double[106];
        //private double[] sourceDataTm2 = new double[106];

        private double[] sourceDataTc = new double[106];

        private double[] sourceDataPc = new double[106];

        private double[] sourceDataPh = new double[106];

        private double[] sourceDataPm = new double[106];

        private double[] sourceDataQm5 = new double[106];

        private double[] sourceDataTemp1 = new double[106];

        private double[] sourceDataTemp2 = new double[106];

        private double[] sourceDataTemp3 = new double[106];

        private double[] sourceDataTemp4 = new double[106];

        private double[] sourceDataTemp5 = new double[106];

        private double[] sourceDataWh = new double[64];
        private float[] resultDataWh = new float[64];

        double[] tempCoolFlow = new double[11];//读取plc的原始数据
        double[] tempHotFlow = new double[11];
        double[] CoolFlow = new double[100];//扩充后的数据
        double[] HotFlow = new double[100];

        private void WaveformAiCtrl1_DataReady(object sender, BfdAiEventArgs args)
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

                DateTime t = DateTime.Now;
                t = t.AddSeconds(-1.03);//采集到的是一秒钟之前的数据，因此需要对当前的时间减去1s，再减去30ms是因为该30ms的数据，并在当前缓冲区做平滑
                t.ToString("yyyy-MM-dd hh:mm:ss:fff");
                //Log.Info(t.ToString("yyyy-MM-dd hh:mm:ss:fff"));

                int dataIndex = 0;
                for (int i = 0; i < m_dataScaled.Length; i += 16)
                {
                    Qc = Math.Round(m_dataScaled[i + 0], 2, MidpointRounding.AwayFromZero);// * 5;
                    Qh = Math.Round(m_dataScaled[i + 1], 2, MidpointRounding.AwayFromZero);// * 5;
                    Qm = Math.Round(m_dataScaled[i + 2], 2, MidpointRounding.AwayFromZero);// * 5;
                    Tc = Math.Round(m_dataScaled[i + 3], 2, MidpointRounding.AwayFromZero);// * 10;
                    Th = Math.Round(m_dataScaled[i + 4], 2, MidpointRounding.AwayFromZero);// * 10;
                    Tm = Math.Round(m_dataScaled[i + 5], 2, MidpointRounding.AwayFromZero);// * 10;
                    Pc = Math.Round(m_dataScaled[i + 6], 2, MidpointRounding.AwayFromZero);
                    Ph = Math.Round(m_dataScaled[i + 7], 2, MidpointRounding.AwayFromZero);
                    Pm = Math.Round(m_dataScaled[i + 8], 2, MidpointRounding.AwayFromZero);
                    Qm5 = Math.Round(m_dataScaled[i + 9], 2, MidpointRounding.AwayFromZero);
                    Temp1 = Math.Round(m_dataScaled[i + 10], 2, MidpointRounding.AwayFromZero);// * 10;
                    Temp2 = Math.Round(m_dataScaled[i + 11], 2, MidpointRounding.AwayFromZero);// * 10;
                    Temp3 = Math.Round(m_dataScaled[i + 12], 2, MidpointRounding.AwayFromZero);// * 10;
                    Temp4 = Math.Round(m_dataScaled[i + 13], 2, MidpointRounding.AwayFromZero);// * 10;
                    Temp5 = Math.Round(m_dataScaled[i + 14], 2, MidpointRounding.AwayFromZero);// * 10;
                    Wh = Math.Round(m_dataScaled[i + 15], 2, MidpointRounding.AwayFromZero) * 200;

                    if (dataIndex < 6)
                    {
                        int typeIndex = 0;
                        sourceDataQc[dataIndex] = isFirstAver ? Qc : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataQh[dataIndex] = isFirstAver ? Qh : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataQm[dataIndex] = isFirstAver ? Qm : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataTc[dataIndex] = isFirstAver ? Tc : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataTh[dataIndex] = isFirstAver ? Th : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataTm[dataIndex] = isFirstAver ? Tm : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataPc[dataIndex] = isFirstAver ? Pc : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataPh[dataIndex] = isFirstAver ? Ph : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataPm[dataIndex] = isFirstAver ? Pm : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataQm5[dataIndex] = isFirstAver ? Qm5 : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataTemp1[dataIndex] = isFirstAver ? Temp1 : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataTemp2[dataIndex] = isFirstAver ? Temp2 : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataTemp3[dataIndex] = isFirstAver ? Temp3 : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataTemp4[dataIndex] = isFirstAver ? Temp4 : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                        sourceDataTemp5[dataIndex] = isFirstAver ? Temp5 : lastTempData[typeIndex * 6 + dataIndex]; typeIndex++;
                    }
                    if (dataIndex >= 18 && dataIndex <= 81)
                    {
                        sourceDataWh[dataIndex - 18] = Wh;
                    }
                    sourceDataQc[dataIndex + 5] = Qc;
                    sourceDataQh[dataIndex + 5] = Qh;
                    sourceDataQm[dataIndex + 5] = Qm;
                    sourceDataTc[dataIndex + 5] = Tc;
                    sourceDataTh[dataIndex + 5] = Th;
                    sourceDataTm[dataIndex + 5] = Tm;
                    //sourceDataTm2[index + 5] = Tm;

                    sourceDataPc[dataIndex + 5] = Pc;
                    sourceDataPh[dataIndex + 5] = Ph;
                    sourceDataPm[dataIndex + 5] = Pm;
                    sourceDataQm5[dataIndex + 5] = Qm5;
                    sourceDataTemp1[dataIndex + 5] = Temp1;
                    sourceDataTemp2[dataIndex + 5] = Temp2;
                    sourceDataTemp3[dataIndex + 5] = Temp3;
                    sourceDataTemp4[dataIndex + 5] = Temp4;
                    sourceDataTemp5[dataIndex + 5] = Temp5;
                    dataIndex++;
                    if (dataIndex >= 101)
                        dataIndex = 0;
                }
                //Console.WriteLine("液面高度：" + Wh);

                short[] temp1 = bpq.read_short_batch("4110", 10, 5);
                short[] temp2 = bpq.read_short_batch("4120", 10, 5);
                for (int i = 0; i < 10; i++)
                {//读取
                    //数值转换 short：0~32767 对应0~50  故需要除以655.34
                    tempCoolFlow[i] = temp1[i] / 655.34;
                    tempHotFlow[i] = temp2[i] / 655.34;
                }
                tempCoolFlow[10] = tempCoolFlow[9];
                tempHotFlow[10] = tempHotFlow[9];
                for (int i = 0; i < 10; i++)
                {

                    double[] CoolFill = dataFill(tempCoolFlow[i], tempCoolFlow[i + 1], 11);//扩充数据
                    double[] HotFill = dataFill(tempHotFlow[i], tempHotFlow[i + 1], 11); ;//
                    for (int j = 0; j < 10; j++)
                    {
                        CoolFlow[i * 10 + j] = CoolFill[j];
                        HotFlow[i * 10 + j] = HotFill[j];
                    }
                }

                sourceDataQc = averge(ref sourceDataQc, 0);
                //sourceDataQc = filterKalMan(sourceDataQc);
                //sourceDataQc = averge(ref sourceDataQc, 0);

                sourceDataQh = averge(ref sourceDataQh, 1);
                //sourceDataQh = filter(ref sourceDataQh, 10);

                sourceDataQm = averge(ref sourceDataQm, 2);
                //sourceDataQm = filter(ref sourceDataQm, 10);

                sourceDataTc = averge(ref sourceDataTc, 3);
                //sourceDataTc = filter(ref sourceDataTc, 10);

                sourceDataTh = averge(ref sourceDataTh, 4);
                // sourceDataTh = filter(ref sourceDataTh, 10);

                //unsen卡尔曼滤波算法，比纯卡尔曼滤波效果要好
         
                sourceDataTm = averge(ref sourceDataTm, 5);
                //sourceDataTm = midFilter(ref sourceDataTm, 5);
                //sourceDataTm = UFK_filter(sourceDataTm);
               
                //sourceDataTm = filter(ref sourceDataTm, 10);             

                sourceDataPc = averge(ref sourceDataPc, 6);
                //sourceDataPc = filter(ref sourceDataPc, 10);

                sourceDataPh = averge(ref sourceDataPh, 7);
                // sourceDataPh = filter(ref sourceDataPh, 10);

                sourceDataPm = averge(ref sourceDataPm, 8);
                // sourceDataPm = filter(ref sourceDataPm, 10);

                sourceDataQm5 = averge(ref sourceDataQm5, 9);
                //sourceDataQm5 = filter(ref sourceDataQm5, 10);

                sourceDataTemp1 = averge(ref sourceDataTemp1, 10);
                //sourceDataTemp1 = filter(ref sourceDataTemp1, 10);

                sourceDataTemp2 = averge(ref sourceDataTemp2, 11);
                //sourceDataTemp2 = filter(ref sourceDataTemp2, 10);

                sourceDataTemp3 = averge(ref sourceDataTemp3, 12);
                //sourceDataTemp3 = filter(ref sourceDataTemp3, 10);

                sourceDataTemp4 = averge(ref sourceDataTemp4, 13);
                //sourceDataTemp4 = filter(ref sourceDataTemp4, 10);

                sourceDataTemp5 = averge(ref sourceDataTemp5, 14);
                //sourceDataTemp5 = filter(ref sourceDataTemp5, 10);

                if (isFirstAver == false)
                {
                    for (int i = 3; i < sourceDataQc.Length - 3; i++)
                    {
                        //Log.Info(t.ToString("yyyy-MM-dd hh:mm:ss:fff"));
                        //Log.Info("index:" + i + " Value:" + (float)sourceDataTemp1[i]);
                        //Console.WriteLine("index:"+i);
                        if (collectDataFlag)
                        {
                            dt.Rows.Add(t.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                                t,
                                //(sourceDataQc[i] - 1) * 12.5,
                                //(sourceDataQh[i] - 1) * 12.5,
                                CoolFlow[i - 3],
                                HotFlow[i - 3] ,
                                (sourceDataQm[i] - 1)<0?0: (sourceDataQm[i] - 1) * 5,
                                sourceDataTc[i] * 10,
                                sourceDataTh[i] * 10,
                                sourceDataTm[i] * 10,
                                sourceDataPc[i],
                                sourceDataPh[i],
                                sourceDataPm[i],
                                sourceDataQm5[i],
                                Wh,
                                index);
                            index++;
                        }
                        if (graphFlag)          //记录流程测试中的，曲线变化
                        {
                            GraphDt.Rows.Add(t.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                                //(sourceDataQc[i] - 1) * 12.5,
                                //(sourceDataQh[i] - 1) * 12.5,
                                CoolFlow[i - 3],
                                HotFlow[i - 3],
                                (sourceDataQm[i] - 1) < 0 ? 0 : (sourceDataQm[i] - 1) * 5,
                                sourceDataTc[i] * 10,
                                sourceDataTh[i] * 10,
                                sourceDataTm[i] * 10,
                                sourceDataPc[i],
                                sourceDataPh[i],
                                sourceDataPm[i],
                                sourceDataQm5[i],
                                Wh);
                        }
                        if (electDataFlag)
                        {
                            ElectDt.Rows.Add(t.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                                //(sourceDataQc[i] - 1) * 12.5,
                                //(sourceDataQh[i] - 1) * 12.5,
                                CoolFlow[i - 3],
                                HotFlow[i - 3],
                                (sourceDataQm[i] - 1) < 0 ? 0 : (sourceDataQm[i] - 1) * 5,
                                sourceDataTc[i] * 10,
                                sourceDataTh[i] * 10,
                                sourceDataTm[i] * 10,
                                sourceDataPc[i],
                                sourceDataPh[i],
                                sourceDataPm[i],
                                sourceDataQm5[i],
                                Wh);
                        }
                        t = t.AddMilliseconds(10.0);
                    }
                    //Qc = (sourceDataQc[3] + sourceDataQc[102] - 2) < 0 ? 0 : Math.Round((sourceDataQc[3] + sourceDataQc[102] - 2) * 12.5 * 0.5, 2, MidpointRounding.AwayFromZero) + (double)Properties.Settings.Default.QcAdjust;
                    //Qh = (sourceDataQh[3] + sourceDataQh[102] - 2) < 0 ? 0 : Math.Round((sourceDataQh[3] + sourceDataQh[102] - 2) * 12.5 * 0.5, 2, MidpointRounding.AwayFromZero) + (double)Properties.Settings.Default.QhAdjust;
                    Qc = CoolFlow[CoolFlow.Length - 1];
                    Qh = HotFlow[CoolFlow.Length - 1];
                    Qm = (sourceDataQm[3] + sourceDataQm[102] - 2) < 0 ? 0 : Math.Round((sourceDataQm[3] + sourceDataQm[102] - 2) * 5 * 0.5, 2, MidpointRounding.AwayFromZero);
                    Tc = Math.Round((sourceDataTc[3] + sourceDataTc[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Th = Math.Round((sourceDataTh[3] + sourceDataTh[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Tm = Math.Round((sourceDataTm[3] + sourceDataTm[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Pc = Math.Round((sourceDataPc[3] + sourceDataPc[102]) * 0.5, 2, MidpointRounding.AwayFromZero);
                    Ph = Math.Round((sourceDataPh[3] + sourceDataPh[102]) * 0.5, 2, MidpointRounding.AwayFromZero);
                    Pm = Math.Round((sourceDataPm[3] + sourceDataPm[102]) * 0.5, 2, MidpointRounding.AwayFromZero);
                    Qm5 = Math.Round((sourceDataQm5[3] + sourceDataQm5[102]) * 0.5, 2, MidpointRounding.AwayFromZero);
                    Temp1 = Math.Round((sourceDataTemp1[3] + sourceDataTemp1[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Temp2 = Math.Round((sourceDataTemp2[3] + sourceDataTemp2[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Temp3 = Math.Round((sourceDataTemp3[3] + sourceDataTemp3[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Temp4 = Math.Round((sourceDataTemp4[3] + sourceDataTemp4[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Temp5 = Math.Round((sourceDataTemp5[3] + sourceDataTemp5[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Wh = Math.Round(resultDataWh.ToList().Average(), 0, MidpointRounding.AwayFromZero);
                    DataReadyToUpdateStatus();
                }
                isFirstAver = false;
                if (err != ErrorCode.Success && err != ErrorCode.WarningRecordEnd)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error("数据采集报错：" + ex.ToString());
                HandleError(err);
            }
        }

        //输入：任意长度的数组
        //输出：滤波之后相同长度的数组
        double Average;
        public double[] filterKalMan(double[] Observe)
        {
            double[] CanShu = { 1, 1, 1, 1, 1, 0, 0, 0 };
            //导入参数
            double KamanX = CanShu[0];
            double KamanP = CanShu[1];
            double KamanQ = CanShu[2];
            double KamanR = CanShu[3];
            double KamanY = CanShu[4];
            double KamanKg = CanShu[5];
            double KamanSum = CanShu[6];

            //加载观察值
            double[] True = new double[Observe.Length];
            for (int i = 0; i <= Observe.Length - 1; i++)
            {
                //对每个观察值迭代
                KamanY = KamanX;
                KamanP = KamanP + KamanQ;
                KamanKg = KamanP / (KamanP + KamanR);
                KamanX = (KamanY + KamanKg * (Observe[i] - KamanY));
                KamanSum += KamanX;
                True[i] = KamanX;
                KamanP = (1 - KamanKg) * KamanP;
            }
            Average = KamanSum / Observe.Length;
            return True;

        }

        private void WaveformAiCtrl1_Overrun(object sender, BfdAiEventArgs e)
        {
            return;
        }

        private void WaveformAiCtrl1_CacheOverflow(object sender, BfdAiEventArgs e)
        {
            return;
        }

        private void HandleError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("Sorry ! some errors happened, the error code is: " + err.ToString(), "AI_InstantAI");
            }
        }


        #endregion

        //#region 电机控制
        ///// <summary>
        ///// 电机角度与对应温度
        ///// </summary>
        //public Dictionary<double, double> tempAngleDict = new Dictionary<double, double>();
        //public Dictionary<double, DateTime> tmTimeDict = new Dictionary<double, DateTime>();
        //public Dictionary<DateTime, double> timeAngleDict = new Dictionary<DateTime, double>();
        //public DataTable timeAngleDt;
        //public bool electDataFlag = false;

        //private ElectricalMachineryType type;
        //private string powerAddress = "";   //M8-2056   M18-2066
        //private bool powerState = false;

        //private string forwardWriteAddress = "";
        //private string forwardReadAddress = "";
        //private bool forwardState = false;

        //private string noForwardWriteAddress = "";
        //private string noForwardReadAddress = "";
        //private bool noForwadState = false;

        //private string orignWriteAddress = "";
        //private string orignReadAddress = "";
        //private bool orignState = false;

        //private string autoRunAddress = "";
        //private string backOrignAddress = "";
        //private string shutdownAddress = "";

        //private string radioAddress = "";
        //private uint radioValue = 0;
        //private string angleAddress = "";
        //private int angleValue = 0;

        ///// <summary>
        ///// 初始化电机通信
        ///// </summary>
        //private void InitE()
        //{
        //    type = ElectricalMachineryType.tempType;
        //    powerAddress = "2056";
        //    forwardWriteAddress = "2048";
        //    forwardReadAddress = "2053";
        //    noForwardWriteAddress = "2049";
        //    noForwardReadAddress = "2054";
        //    orignWriteAddress = "2050";
        //    orignReadAddress = "2055";
        //    autoRunAddress = "2051";
        //    backOrignAddress = "2052";
        //    shutdownAddress = "2057";
        //    radioAddress = "4296";
        //    angleAddress = "5432";

        //    timeAngleDt = new DataTable();
        //    timeAngleDt.Columns.Add("时间", typeof(string));
        //    timeAngleDt.Columns.Add("角度", typeof(double));

        //    monitorDTimer = new System.Timers.Timer(200);
        //    monitorDTimer.Elapsed += (o, a) =>
        //    {
        //        MonitorDActive();
        //    };//到达时间的时候执行事件；
        //    monitorDTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
        //    monitorDTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        //    electDataFlag = true;       //开始记录温度
        //}

        ///// <summary>
        ///// 分析温度与角度对应关系
        ///// </summary>
        //private void AnalyseElect()
        //{
        //    foreach (DataRow tmRow in ElectDt.Rows)
        //    {
        //        if (tmTimeDict.Keys.Count == 3)
        //        {
        //            break;
        //        }
        //        var tm = tmRow["出水温度Tm"].AsDouble();
        //        var tmTime = tmRow["时间"].AsDateTime();
        //        if (tm == 36)
        //        {
        //            if (tmTimeDict.ContainsKey(36))
        //                continue;
        //            else
        //                tmTimeDict.Add(36, tmTime);
        //        }
        //        if (tm == 38)
        //        {
        //            if (tmTimeDict.ContainsKey(38))
        //                continue;
        //            else
        //                tmTimeDict.Add(38, tmTime);
        //        }
        //        if (tm == 40)
        //        {
        //            if (tempAngleDict.ContainsKey(40))
        //                continue;
        //            else
        //                tmTimeDict.Add(40, tmTime);
        //        }
        //    }
        //    foreach (DataRow eleRow in timeAngleDt.Rows)
        //    {
        //        if (timeAngleDict.Keys.Count == 3)
        //        {
        //            break;
        //        }
        //        var angle = eleRow["角度"].AsDouble();
        //        var angleTime = eleRow["时间"].AsDateTime();

        //        if (timeAngleDict.ContainsKey(tmTimeDict[36]) == false && (angleTime - tmTimeDict[36]).TotalMilliseconds < 0.5)
        //        {
        //            timeAngleDict.Add(tmTimeDict[36], angle);
        //        }

        //        if (timeAngleDict.ContainsKey(tmTimeDict[38]) == false && (angleTime - tmTimeDict[38]).TotalMilliseconds < 0.5)
        //        {
        //            timeAngleDict.Add(tmTimeDict[38], angle);
        //        }

        //        if (timeAngleDict.ContainsKey(tmTimeDict[40]) == false && (angleTime - tmTimeDict[40]).TotalMilliseconds < 0.5)
        //        {
        //            timeAngleDict.Add(tmTimeDict[40], angle);
        //        }
        //    }

        //    tempAngleDict.Add(36, timeAngleDict[tmTimeDict[36]]);
        //    tempAngleDict.Add(36, timeAngleDict[tmTimeDict[38]]);
        //    tempAngleDict.Add(36, timeAngleDict[tmTimeDict[40]]);
        //}

        //System.Timers.Timer monitorDTimer;            //监控D寄存器定时器
        //private delegate void MonitorDActiveDelegate();
        //private void MonitorDActive()
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            MonitorDActiveDelegate monitorActiveDelegate = MonitorDActive;
        //            this.Invoke(monitorActiveDelegate);
        //        }
        //        else
        //        {
        //            radioValue = bpq.read_uint(radioAddress, 5);
        //            angleValue = bpq.read_int(angleAddress, 5);

        //            var temp1 = (radioValue * 0.0001);
        //            var temp2 = (angleValue * 0.0001);
        //            if (electDataFlag)
        //            {
        //                timeAngleDt.Rows.Add(
        //                    DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:fff"),
        //                    temp2);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }

        //}

        //#endregion

        #region 预留模拟量输出

        public void AO_Func(int index, double value)
        {
            aoData[index] = value;
            collectData.InstantAo_Write(aoData);//输出模拟量函数
        }

        /// <summary>
        /// 设置某一位的值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index">要设置的位， 值从低到高为 0-7</param>
        /// <param name="flag">要设置的值 true / false</param>
        /// 
        /// <returns></returns>
        void set_bit(ref byte data, int index, bool flag)
        {
            index++;
            if (index > 8 || index < 1)
                throw new ArgumentOutOfRangeException();
            int v = index < 2 ? index : (2 << (index - 2));
            data = flag ? (byte)(data | v) : (byte)(data & ~v);

        }


        //此算法用于历史曲线的处理，不适用于实时曲线的处理。一次性将所有的数据扔进来处理，如果是缓存区进来处理，会有边界效应。
        double[] UFK_filter(double[] res)
        {
            var filter = new UKF();
            double[] result = new double[res.Length];
            for (int i = 0; i < res.ToArray().Length; i++)
            {
                filter.Update(new[] { res[i] });
                result[i] = filter.getState()[0];
            }
            res = result;
            for (int i = 0; i < res.ToArray().Length; i++)
            {
                filter.Update(new[] { res[i] });
                result[i] = filter.getState()[0];
            }
            return result;
        }


        /// <summary>
        /// N为7的平均数据处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool isFirstMid = true;
        double[] midFilter(ref double[] data, int type)
        {
            if (isFirstMid)        //i=6開始
            {
                for (int i = 6; i < 100; i++)
                {
                    List<double> temp = new List<double>();
                    for (int j = 0; j < 7; j++)
                    {
                        temp.Add(data[i + j]);
                    }
                    temp.Sort();
                    data[i + 3] = temp[3];
                }
            }
            else
            {
                for (int i = 0; i < 100; i++)               //3——102
                {
                    List<double> temp = new List<double>();
                    for (int j = 0; j < 7; j++)
                    {
                        temp.Add(data[i + j]);
                    }
                    temp.Sort();
                    data[i + 3] = temp[3];
                }
            }
            //该六位数据，供下一组数据使用
            lastTempData[type * 6 + 0] = data[100];
            lastTempData[type * 6 + 1] = data[101];
            lastTempData[type * 6 + 2] = data[102];

            lastTempData[type * 6 + 3] = data[103];
            lastTempData[type * 6 + 4] = data[104];
            lastTempData[type * 6 + 5] = data[105];
            return data;
        }

        /// <summary>
        /// N为7的平均数据处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool isFirstAver = true;
        double[] averge(ref double[] data, int type)
        {
            if (isFirstAver)        //i=6開始
            {
                for (int i = 6; i < 100; i++)
                {
                    List<double> temp = new List<double>();
                    for (int j = 0; j < 7; j++)
                    {
                        temp.Add(data[i + j]);
                    }
                    double sum = temp.Sum() - temp.Max() - temp.Min();
                    data[i + 3] = Math.Round(sum / 5, 2, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                for (int i = 0; i < 100; i++)               //3——102
                {
                    List<double> temp = new List<double>();
                    for (int j = 0; j < 7; j++)
                    {
                        temp.Add(data[i + j]);
                    }
                    double sum = temp.Sum() - temp.Max() - temp.Min();
                    data[i + 3] = Math.Round(sum / 5, 2, MidpointRounding.AwayFromZero);
                }
            }
            //该六位数据，供下一组数据使用
            lastTempData[type * 6 + 0] = data[100];
            lastTempData[type * 6 + 1] = data[101];
            lastTempData[type * 6 + 2] = data[102];

            lastTempData[type * 6 + 3] = data[103];
            lastTempData[type * 6 + 4] = data[104];
            lastTempData[type * 6 + 5] = data[105];
            return data;
        }

        /// <summary>
        /// 传入的数组为：前6位上次数据+后100位缓冲区数据
        /// 消除后100位滤波抖动
        /// </summary>
        /// <param name="sourceData"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        double[] filter(ref double[] sourceData, int N)
        {
            double[] data = sourceData.ToList().Skip(6).Take(100).ToArray();
            double[] temp = new double[100];
            double value = -99;//前一个可信的value
            double new_value;
            int pos = 0;//记录上一个value的下标
            int count = 0;//缓存区计数器
            int addLength = 0;//每次遇到新的可用的value，要根据前一个value 复制的长度
            for (int i = 0; i < 100; i++)
            {
                count++;
                if (-99 == value)
                {
                    value = data[i];
                    //temp[i] = data[i];
                    pos = i;
                }
                else
                {
                    if (data[i] == value)
                    {
                        // addLength += count;
                        count = 0;
                    }

                    else
                    {

                        if (count >= N)
                        {
                            new_value = data[i];
                            Console.WriteLine(data[i]);
                            addLength = i - pos + 1;
                            count = 0;
                            double addvalue = (new_value - value) / addLength;
                            for (int j = 0; j < addLength; j++)
                            {
                                temp[pos + j] = value + addvalue * j;
                            }
                            value = data[i];
                            pos = i;
                            count = 0;
                            //addLength = 0;
                        }

                    }
                }
            }


            {
                new_value = data[99];
                Console.WriteLine("++:" + data[99]);
                addLength = 99 - pos + 1;
                count = 0;
                double addvalue = (new_value - value) / addLength;
                for (int j = 0; j < addLength; j++)
                {
                    temp[pos + j] = value + addvalue * j;
                }

            }
            for (int i = 6; i < sourceData.Length; i++)
            {
                sourceData[i] = temp[i - 6];
            }
            return sourceData;
        }

        #endregion

        private void HslSwitch1_OnSwitchChanged(object arg1, bool arg2)
        {
            if (arg2)
            {
                bpq.write_coil("2078", true, 5);
                hslSwitch1.Text = "夹紧产品";
            }
            else
            {
                bpq.write_coil("2078", false, 5);
                hslSwitch1.Text = "松开产品";
            }
        }

        public double[] dataFill(double begin, double end, int len)
        {
            double[] data = new double[len];
            data[0] = begin;
            data[len - 1] = end;
            double d = (end - begin) / (len - 1);
            for (int i = 1; i < len - 1; i++)
            {
                data[i] = begin + i * d;
            }
            return data;
        }
    }
}
