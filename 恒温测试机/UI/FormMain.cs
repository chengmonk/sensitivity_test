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
using 恒温测试机.Model;
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
        public static Model_Export model = new Model_Export();         //灵敏度导出报告模板


        System.Timers.Timer senstivityTimer;        //灵敏度测试 定时器
        System.Timers.Timer fidelityTimer;        //保真度测试 定时器
        System.Timers.Timer tmSteadyTimer65;        //出水温度稳定性测试 定时器 65
        System.Timers.Timer tmSteadyTimer50;        //出水温度稳定性测试 定时器 50

        System.Timers.Timer monitorDiTimer;          //监控数字量定时器
        System.Timers.Timer monitorTimer;            //监控管道泵 阀  状态
        COMconfig bpq_conf;
        public M_485Rtu bpq;
        public DAQ_profile collectData;
        public DAQ_profile control;
        private config collectConfig;
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

        double QmTemp;
        double TmTemp;
        int index;

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

        private delegate void MonitorActiveDelegate();//doData的状态监控
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
                    //冷水泵
                    var val = doData2[0].get_bit(0);
                    if (val == 1)
                        hslPumpOne8.MoveSpeed = 1;
                    else
                        hslPumpOne8.MoveSpeed = 0;
                    
                    //热水泵
                    val = doData2[0].get_bit(1);
                    if (val == 1)
                        reshui.MoveSpeed = 1;
                    else
                        reshui.MoveSpeed = 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error("管道监控异常：" + ex.ToString());
                return;
            }
        }

        public delegate void SystemInfoPrintDelegate(string s);//记录数据  
        public void SystemInfoPrint(string msg)
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
                    systemInfoTb.AppendText("\r\n");
                    systemInfoTb.AppendText("[时间:" + DateTime.Now.ToString() + "] " + msg);
                    systemInfoTb.AppendText("\r\n");
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

                    #region LED显示
                    hslLedQm.DisplayText = Qm.ToString();
                    hslLedTm.DisplayText = Tm.ToString();
                    hslLedTc.DisplayText = Tc.ToString();
                    hslLedTh.DisplayText = Th.ToString();
                    hslLedPc.DisplayText = Pc.ToString();
                    hslLedPh.DisplayText = Ph.ToString();
                    #endregion

                    for (int i = 3; i < 103; i++)
                    {
                        var qcTemp = CoolFlow[i - 3];
                        var qhTemp = HotFlow[i - 3];
                        //var qmTemp = (sourceDataQm[i] - 1) < 0 ? 0 : (sourceDataQm[i] - 1) * 15;//25;
                        var qmTemp = (sourceDataQm[i]) < 0 ? 0 : (sourceDataQm[i]) * 6;//25;
                        hslCurve1.AddCurveData(
                            new string[] {
                                    "冷水流量Qc", "热水流量Qh", "出水流量Qm",
                                    "冷水温度Tc", "热水温度Th", "出水温度Tm",
                                    "冷水压力Pc", "热水压力Ph", "出水压力Pm",
                            },
                            new float[]
                            {
                                     (float)qcTemp,
                                    (float)qhTemp,
                                    (float)qmTemp,
                                    (float)sourceDataTc[i]*10,(float)sourceDataTh[i]*10,(float)sourceDataTm[i]*10,
                                    //(float)sourceDataPc[i],(float)sourceDataPh[i],
                                    (float)Pc,
                                    (float)Ph,
                                    (float)sourceDataPm[i],
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
                    //电机停止
                    bpq.write_coil(powerAddress_spin, false, 5);
                    bpq.write_coil(forwardWriteAddress_spin, false, 5);
                    bpq.write_coil(noForwardWriteAddress_spin, false, 5);
                    bpq.write_coil(powerAddress_upDown, false, 5);
                    bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                    bpq.write_coil(noForwardWriteAddress_upDown, false, 5);

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
            TmSteadyRbt65.Visible = true;
            TmSteadyRbt50.Visible = true;
            //sensitivityRbt.Checked = true;
            //hslCurve1.SetLeftCurve("冷水箱温度", null, Color.OrangeRed);
            //hslCurve1.SetLeftCurve("热水箱温度", null, Color.Orchid);
            //hslCurve1.SetLeftCurve("高温水箱温度", null, Color.White);
            //hslCurve1.SetLeftCurve("中温水箱温度", null, Color.GreenYellow);
            //hslCurve1.SetLeftCurve("常温水箱温度", null, Color.BlueViolet);
            //hslCurve1.SetLeftCurve("冷水流量Qc", null, Color.Red);
            //hslCurve1.SetLeftCurve("热水流量Qh", null, Color.Orange);
            hslCurve1.SetLeftCurve("出水流量Qm", null, Color.Yellow);
            hslCurve1.SetLeftCurve("冷水温度Tc", null, Color.Aqua);
            hslCurve1.SetLeftCurve("热水温度Th", null, Color.Red);
            hslCurve1.SetLeftCurve("出水温度Tm", null, Color.Yellow);
            hslCurve1.SetLeftCurve("冷水压力Pc", null, Color.White);
            hslCurve1.SetLeftCurve("热水压力Ph", null, Color.DarkOrange);
            //hslCurve1.SetLeftCurve("出水压力Pm", null, Color.DodgerBlue);
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

            QmTmTable65 = new DataTable();
            QmTmTable65.Columns.Add("时间", typeof(string));
            QmTmTable65.Columns.Add("出水流量Qm", typeof(double));   //新建第一列 通道0
            QmTmTable65.Columns.Add("出水温度Tm", typeof(double));   //1

            QmTmTable50 = new DataTable();
            QmTmTable50.Columns.Add("时间", typeof(string));
            QmTmTable50.Columns.Add("出水流量Qm", typeof(double));   //新建第一列 通道0
            QmTmTable50.Columns.Add("出水温度Tm", typeof(double));   //1

            AngleTmTable = new DataTable();
            AngleTmTable.Columns.Add("时间", typeof(string));
            AngleTmTable.Columns.Add("角度", typeof(double));   //新建第一列 通道0
            AngleTmTable.Columns.Add("出水温度Tm", typeof(double));   //1

            timeAngleDt = new DataTable();
            timeAngleDt.Columns.Add("时间", typeof(string));
            timeAngleDt.Columns.Add("角度", typeof(double));

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

            monitorTimer = new System.Timers.Timer(1000);
            monitorTimer.Elapsed += (o, a) =>
            {
                MonitorActive();
            };//到达时间的时候执行事件； 
            monitorTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            monitorTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；


            senstivityTimer = new System.Timers.Timer(2);
            senstivityTimer.Elapsed += new System.Timers.ElapsedEventHandler(SenstivityTimer_Action);//到达时间的时候执行事件； 
            senstivityTimer.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            senstivityTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；


            tmSteadyTimer65 = new System.Timers.Timer(2);
            tmSteadyTimer65.Elapsed += new System.Timers.ElapsedEventHandler(TmSteadyTimer65_Action);//到达时间的时候执行事件； 
            tmSteadyTimer65.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            tmSteadyTimer65.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            tmSteadyTimer50 = new System.Timers.Timer(2);
            tmSteadyTimer50.Elapsed += new System.Timers.ElapsedEventHandler(TmSteadyTimer50_Action);//到达时间的时候执行事件； 
            tmSteadyTimer50.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            tmSteadyTimer50.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            monitorMTimer = new System.Timers.Timer(1000);
            monitorMTimer.Elapsed += (o, a) =>
            {
                MonitorMActive();
            };//到达时间的时候执行事件；
            monitorMTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            monitorMTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            monitorDTimer = new System.Timers.Timer(1000);
            monitorDTimer.Elapsed += (o, a) =>
            {
                MonitorDActive();
            };//到达时间的时候执行事件；
            monitorDTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            monitorDTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
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
            //asc.Initialize(this);
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            //asc.ReSize(this);
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
            if (monitorTimer != null)
            {
                monitorTimer.Enabled = false;
                monitorTimer.Dispose();
            }
            if (monitorMTimer != null)
            {
                monitorMTimer.Enabled = false;
                monitorMTimer.Dispose();
            }
            if (monitorDTimer != null)
            {
                monitorDTimer.Enabled = false;
                monitorDTimer.Dispose();
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
                //fidelityTimer.Enabled = false;
                tmSteadyTimer65.Enabled = false;
                tmSteadyTimer50.Enabled = false;
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
                    case "灵敏度测试+保真度测试":
                        logicType = LogicTypeEnum.SensitivityTest;
                        break;
                    case "保真度测试":
                        logicType = LogicTypeEnum.FidelityTest;
                        break;
                    case "出水温度稳定性测试（65℃）":
                        logicType = LogicTypeEnum.TmSteadyTest65;
                        break;
                    case "出水温度稳定性测试（50℃）":
                        logicType = LogicTypeEnum.TmSteadyTest50;
                        break;
                }
                Console.WriteLine(logicType.ToDescription());
                //InitData();
                ChangeTimer();
            }
        }

        

        private void StartBtn_Click(object sender, EventArgs e)
        {
            //collectDataFlag = true;
            if (runFlag)    //避免重复开启
            {
                Console.WriteLine("正在运行");
                return;
            }
            switch (logicType)
            {
                case LogicTypeEnum.SensitivityTest:
                    senstivityTimer.Enabled = true;
                    break;
                //case LogicTypeEnum.FidelityTest:
                //    fidelityTimer.Enabled = true;
                //    break;
                case LogicTypeEnum.TmSteadyTest65:
                    tmSteadyTimer65.Enabled = true;
                    break;
                case LogicTypeEnum.TmSteadyTest50:
                    tmSteadyTimer50.Enabled = true;
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
            FormSaveTemplate formSaveTemplate = new FormSaveTemplate(model);
            formSaveTemplate.Show();

            //TODO：导出测试报告
            //DataReportExportApp exportApp = new DataReportExportApp(analyseReportDic);
            //SaveFileDialog fileDialog = new SaveFileDialog();
            //fileDialog.Filter = "文档|*.txt";
            //fileDialog.InitialDirectory = Application.StartupPath;
            //if (fileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    if (exportApp.Export(logicType))
            //    {
            //        MessageBox.Show("导出成功");
            //    }
            //    else
            //    {
            //        MessageBox.Show("导出失败，当前测试流程未有相应数据");
            //    }
            //}
            //fileDialog.Dispose();
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

        public bool isNormalCategory = true;
        private int AngleTmIndex = 0;
        private bool isForwardFlag = true;
        private int AngleTmMidIndex = 0;
        public DataTable AngleTmTable;
        public bool AngleTmFlag = false;
        public double sensitityOrgTm = 0;
        private void SenstivityTimer_Action(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                runFlag = true;
                graphFlag = true;
                isForwardFlag = true;
                MessageBox.Show("请确认电机已设置好相关参数！");

                #region 启动 011(冷水泵) 021(热水泵) a(热水阀) b(冷水阀)
                set_bit(ref doData2[0], 0, true);
                set_bit(ref doData2[0], 1, true);
                set_bit(ref doData2[0], 2, true);
                set_bit(ref doData2[0], 3, true);
                collectData.InstantDo_Write(doData2);
                //control.InstantDo_Write(doData2);
                SystemInfoPrint("[初始化系统...]\n");
                if (stopFlag)   //手动停止
                {
                    StopPro();
                    return;
                }

                System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));
                SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束,开始灵敏度测试+保真度测试]\n");

                double orgTm = 0;// Math.Round((Th + Tc) * 0.5, 2); //记录当前出水温度
                double range = isNormalCategory ? 4 : 2; 
                //SystemInfoPrint("当前Tm——>" + orgTm);

                #endregion

                #region 电机旋转
                double G11 = 0; bool G11Flag = true;
                double G21 = 0; bool G21Flag = true;
                double G12 = 0; bool G12Flag = true;
                double G22 = 0; bool G22Flag = true;
                double GA = 0;  bool GAFlag = true;
                double GB = 0;  bool GBFlag = true;
                double currentAngle = 0;
                bool startCollectTmFlag = false;
                AngleTmFlag = true;

                //if (settingForm != null)
                //{
                //正传到预设角度
                SystemInfoPrint("预设角度值——>" + autoFindAngle_spin);
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始左转...");
                bpq.write_coil(orignWriteAddress_spin, false, 5);
                bpq.write_coil(noForwardWriteAddress_spin, true, 5);
                while (currentAngle <= autoFindAngle_spin)
                {
                    if (stopFlag)   //手动停止
                    {
                        StopPro();
                        return;
                    }
                    currentAngle = Math.Abs(Math.Round(angleValue_spin / 3200.0, 1));
                    if (currentAngle >= 10 && startCollectTmFlag == false)
                    {
                        orgTm = Math.Round((Th + Tc) * 0.5, 2); //记录当前平均温度
                        sensitityOrgTm = orgTm;
                        SystemInfoPrint("当前Tm——>" + orgTm);
                        startCollectTmFlag = true;
                    }
                    if (startCollectTmFlag)
                    {
                        if (Math.Abs(Tm - 38) <= 0.2 && GBFlag)
                        {
                            GB = currentAngle;    //角度  读取
                            SystemInfoPrint("38度的角度B为——>" + GB);
                            GBFlag = false;
                        }
                        if (Math.Abs(Tm - (orgTm - range)) <= 0.2 && G21Flag)
                        {
                            G21 = currentAngle;    //角度  读取
                            SystemInfoPrint("G2:" + (orgTm - range) + "的角度为——>" + G21);
                            G21Flag = false;
                        }
                        if (Math.Abs(Tm - (orgTm + range)) <= 0.2 && G22Flag)
                        {
                            G22 = currentAngle;    //角度  读取
                            SystemInfoPrint("G2:" + (orgTm + range) + "的角度为——>" + G22);
                            G22Flag = false;
                        }
                    }
                }
                bpq.write_coil(noForwardWriteAddress_spin, false, 5);
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "结束左转...");

                //反转回到原点
                bpq.write_coil(forwardWriteAddress_spin, true, 5);
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始右转...");
                AngleTmMidIndex = AngleTmIndex;
                isForwardFlag = false;
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "Index:"+ AngleTmMidIndex);
                //SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + angleValue_spin);
                while (angleValue_spin <= 0)
                {
                    if (stopFlag)   //手动停止
                    {
                        StopPro();
                        return;
                    }
                    //等待角度达到 原点
                    if (Math.Abs(Tm-38)<=0.2 && GAFlag)
                    {
                        GA = Math.Round(angleValue_spin / 3200.0, 1);    //角度  读取
                        SystemInfoPrint("38度的角度A为——>" + GA);
                        GAFlag = false;
                    }
                    if (Math.Abs(Tm-(orgTm - range))<=0.2 && G11Flag)
                    {
                        G11 = Math.Round(angleValue_spin / 3200.0, 1);    //角度  读取
                        SystemInfoPrint("G1:" + (orgTm - range) + "的角度为——>" + G11);
                        G11Flag = false;
                    }
                    if (Math.Abs(Tm -(orgTm + range))<=0.2 && G12Flag)
                    {
                        G12 = Math.Round(angleValue_spin / 3200.0, 1);    //角度  读取
                        SystemInfoPrint("G1:" + (orgTm + range) + "的角度为——>" + G12);
                        G12Flag = false;
                    }
                }
                bpq.write_coil(forwardWriteAddress_spin, false, 5);
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "结束右转...");
                //FormCurve form = new FormCurve(AngleTmTable, logicType, this,AngleTmMidIndex, model, orgTm, 0);
                //form.Show();
                //FormCurve form2 = new FormCurve(AngleTmTable, logicType, this,AngleTmMidIndex, model, orgTm, 1);
                //form2.Show();
                //FormCurve form3 = new FormCurve(AngleTmTable, logicType, this,AngleTmMidIndex, model, orgTm, 2);
                //form3.Show();
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                AngleTmFlag = false;
                runFlag = false;
                graphFlag = false;
                bpq.write_coil(powerAddress_spin, false, 5);
                bpq.write_coil(powerAddress_upDown, false, 5);
                //if (autoRunFlag)
                //{
                //    ChangeRadioButton();
                //}
            }
        }
        public bool qm6Flag = false;
        public bool qm3Flag = false;

        public DataTable QmTmTable65;
        public bool QmTmTableFlag65 = false;
        private void TmSteadyTimer65_Action(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                runFlag = true;
                graphFlag = true;
                qm6Flag = false;
                qm3Flag = false;
                MessageBox.Show("请确认电机已设置好相关参数！");

                #region 启动 011(冷水泵) 021(热水泵) a(热水阀) b(冷水阀)
                set_bit(ref doData2[0], 0, true);
                set_bit(ref doData2[0], 1, true);
                set_bit(ref doData2[0], 2, true);
                set_bit(ref doData2[0], 3, true);
                collectData.InstantDo_Write(doData2);
                //control.InstantDo_Write(doData2);
                SystemInfoPrint("[初始化系统...]\n");
                if (stopFlag)   //手动停止
                {
                    StopPro();
                    return;
                }
                var orgTh = Th; //记录当前热水温度
                SystemInfoPrint("当前热水温度——>" + orgTh);

                System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));
                SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束,开始出水温度稳定性测试]\n");
                #endregion

                #region 电机升降
                double Tm1 = 0;
                bool Tm1Flag = false;
                double Tm2 = 0;
                bool Tm2Flag = false;
                //if (settingForm != null)
                //{
                QmTmTableFlag65 = true;
                //下降
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始下降...");
                bpq.write_coil(forwardWriteAddress_upDown, true, 5);
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "当前流量-->" + QmTemp);

                while (QmTemp >= 2)
                {
                    if (stopFlag)   //手动停止
                    {
                        StopPro();
                        return;
                    }
                    //出水流量>=1L/min
                    if (Math.Round(Math.Abs(Qm - 5.8), 2) <= 0.2&&(qm6Flag==false))
                    {
                        Tm1 = TmTemp;
                        qm6Flag = true;
                        SystemInfoPrint("流量为6L的出水温度——>" + Tm1);
                    }
                    if (Math.Round(Math.Abs(Qm - 2.8), 2) <= 0.2&&(qm3Flag==false))
                    {
                        qm3Flag = true;
                        Tm2 = TmTemp;
                        SystemInfoPrint("流量为3L的出水温度——>" + Tm2);
                    }
                }
                runFlag = false;
                graphFlag = false;
                QmTmTableFlag65 = false;
                bpq.write_coil(forwardWriteAddress_upDown, false, 5);   //停止下降
                Thread.Sleep(100);
                bpq.write_coil(backOrignAddress_upDown, true, 5);       //回到原点

                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "结束下降...");
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "当前流量-->" + QmTemp);

                //}
                #endregion

                
                //bpq.write_coil(powerAddress_spin, false, 5);
                //bpq.write_coil(forwardWriteAddress_spin, false, 5);
                //bpq.write_coil(noForwardWriteAddress_spin, false, 5);

                //bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                //bpq.write_coil(noForwardWriteAddress_upDown, false, 5);
                //bpq.write_coil(powerAddress_upDown, false, 5);
                //FormCurve form = new FormCurve(QmTmTable65, logicType,this, 0,model);
                //form.Show();
            }
            catch (Exception ex)
            {
                Log.Error("温度稳定性65℃异常："+ex.ToString());
            }
            finally
            {
                //bpq.write_coil(powerAddress_spin, false, 5);
                //bpq.write_coil(forwardWriteAddress_spin, false, 5);
                //bpq.write_coil(noForwardWriteAddress_spin, false, 5);

                //bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                //bpq.write_coil(noForwardWriteAddress_upDown, false, 5);
                //bpq.write_coil(powerAddress_upDown, false, 5);
                
                runFlag = false;
                graphFlag = false;
                QmTmTableFlag65 = false;
            }
        }

        public DataTable QmTmTable50;
        public bool QmTmTableFlag50 = false;
        private void TmSteadyTimer50_Action(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                runFlag = true;
                graphFlag = true;
                qm6Flag = false;
                qm3Flag = false;
                MessageBox.Show("请确认电机已设置好相关参数！");

                #region 启动 011(冷水泵) 021(热水泵) a(热水阀) b(冷水阀)
                set_bit(ref doData2[0], 0, true);
                set_bit(ref doData2[0], 1, true);
                set_bit(ref doData2[0], 2, false);
                set_bit(ref doData2[0], 3, true);
                collectData.InstantDo_Write(doData2);
                //control.InstantDo_Write(doData2);
                SystemInfoPrint("[初始化系统...]\n");
                if (stopFlag)   //手动停止
                {
                    StopPro();
                    return;
                }
                var orgTh = Th; //记录当前热水温度
                SystemInfoPrint("当前热水温度——>" + orgTh);

                System.Threading.Thread.Sleep((int)(1000 * Properties.Settings.Default.t1));
                SystemInfoPrint("[t1 = " + Properties.Settings.Default.t1.ToString() + " s 计时结束,开始出水温度稳定性测试]\n");
                #endregion

                #region 电机升降
                double Tm1 = 0;
                double Tm2 = 0;
                //if (settingForm != null)
                //{
                QmTmTableFlag50 = true;
                //下降
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始下降...");
                bpq.write_coil(forwardWriteAddress_upDown, true, 5);
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "当前流量-->"+Qm);
                while (Qm >= 2)
                {
                    if (stopFlag)   //手动停止
                    {
                        StopPro();
                        return;
                    }
                    //出水流量>=1L/min
                    if (Math.Round(Math.Abs(Qm - 5.8), 2) <= 0.2 && (qm6Flag == false))
                    {
                        Tm1 = TmTemp;
                        qm6Flag = true;
                        SystemInfoPrint("流量为6L的出水温度——>" + Tm1);
                    }
                    if (Math.Round(Math.Abs(Qm - 2.8), 2) <= 0.2 && (qm3Flag == false))
                    {
                        Tm2 = TmTemp;
                        qm3Flag = true;
                        SystemInfoPrint("流量为3L的出水温度——>" + Tm2);
                    }
                }
                runFlag = false;
                graphFlag = false;
                QmTmTableFlag50 = false;
                bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                bpq.write_coil(backOrignAddress_upDown, true, 5);

                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "结束下降...");
                SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "当前流量-->" + Qm);
                //}
                #endregion

              
                //bpq.write_coil(powerAddress_spin, false, 5);
                //bpq.write_coil(forwardWriteAddress_spin, false, 5);
                //bpq.write_coil(noForwardWriteAddress_spin, false, 5);

               // bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                //bpq.write_coil(noForwardWriteAddress_upDown, false, 5);
                //FormCurve form = new FormCurve(QmTmTable50, logicType, this,0,model);
                //form.Show();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                //bpq.write_coil(powerAddress_spin, false, 5);
                //bpq.write_coil(forwardWriteAddress_spin, false, 5);
                //bpq.write_coil(noForwardWriteAddress_spin, false, 5);

                //bpq.write_coil(powerAddress_upDown, false, 5);
                //bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                //bpq.write_coil(noForwardWriteAddress_upDown, false, 5);
                runFlag = false;
                graphFlag = false;
                QmTmTableFlag50 = false;
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
            //settingForm = new FormConnectValueSetting(this);
            //settingForm.Show();
        }

        private void GraphDataBtn_Click(object sender, EventArgs e)
        {
            if (logicType == LogicTypeEnum.SensitivityTest)
            {
                if (AngleTmTable.Rows != null && AngleTmTable.Rows.Count > 1)
                {
                    Log.Info("启动图像界面");
                    //FormPressureCurve form = new FormPressureCurve(AngleTmTable, logicType);
                    FormCurve form = new FormCurve(AngleTmTable, logicType, this, AngleTmMidIndex, model, sensitityOrgTm, 0);
                    form.Show();
                    FormCurve form2 = new FormCurve(AngleTmTable, logicType, this, AngleTmMidIndex, model, sensitityOrgTm, 1);
                    form2.Show();
                    FormCurve form3 = new FormCurve(AngleTmTable, logicType, this, AngleTmMidIndex, model, sensitityOrgTm, 2);
                    form3.Show();
                }
                else
                {
                    MessageBox.Show("未采集到数据");
                }
            }
            else if(logicType==LogicTypeEnum.TmSteadyTest65)
            {
                if (QmTmTable65.Rows != null && QmTmTable65.Rows.Count > 1)
                {
                    Log.Info("启动图像界面");
                    //FormPressureCurve form = new FormPressureCurve(QmTmTable65, logicType);
                    FormCurve form = new FormCurve(QmTmTable65, logicType,this,0, model);
                    form.Show();

                }
                else
                {
                    MessageBox.Show("未采集到数据");
                }
            }
            else
            {
                if (QmTmTable50.Rows != null && QmTmTable50.Rows.Count > 1)
                {
                    Log.Info("启动图像界面");
                    FormCurve form = new FormCurve(QmTmTable50, logicType,this,0,model);
                    form.Show();
                }
                else
                {
                    MessageBox.Show("未采集到数据");
                }
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

        /// <summary>
        /// 导出灵敏度和保真度的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HslButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "文档|*.csv";
            fileDialog.InitialDirectory = Application.StartupPath;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTableUtils.DataTableToCsvT(AngleTmTable, fileDialog.FileName);
                MessageBox.Show("保存成功!");
                index = 0;
            }
            fileDialog.Dispose();
        }

        /// <summary>
        /// 导出出水稳定性的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HslButton6_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "文档|*.csv";
            fileDialog.InitialDirectory = Application.StartupPath;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (logicType == LogicTypeEnum.TmSteadyTest65)
                {
                    DataTableUtils.DataTableToCsvT(QmTmTable65, fileDialog.FileName);
                    MessageBox.Show("保存成功!");
                }
                else
                {
                    DataTableUtils.DataTableToCsvT(QmTmTable50, fileDialog.FileName);
                    MessageBox.Show("保存成功!");
                }

                index = 0;
            }
            fileDialog.Dispose();
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

        double[] CoolFlow = new double[100];//扩充后的数据
        double[] HotFlow = new double[100];
        double[] AngleExtends = new double[100];
        double lastAngle = 0;

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
                //Console.WriteLine("m_dataScaled.Length--->"+ m_dataScaled.Length);
                //for (int i = 0; i < m_dataScaled.Length; i += 16)
                for (int i = 0; i < 1600; i += 16)
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

                var currentAngle = Math.Round(angleValue_spin / 3200.0, 2);
                var diffAngle = radio * 0.01;
                Console.WriteLine("diffAngle：--->" + diffAngle);
                Console.WriteLine("currentAngle：--->" + currentAngle);
                Console.WriteLine("lastAngle：--->" + lastAngle);

                //将每秒采集到的角度扩充到100个
                //for (int i = 0; i < 10; i++)
                //{
                //    double[] angleFill = dataFill(lastAngle, currentAngle, 11);//扩充数据
                //    for (int j = 0; j < 10; j++)
                //    {
                //        AngleExtends[i * 10 + j] = angleFill[j];
                //    }
                //}
                for (int i = 0; i < 100; i++)
                {
                    if (lastAngle <= currentAngle)
                    {
                        AngleExtends[i] = Math.Round(lastAngle + diffAngle * i, 2);
                    }
                    else
                    {
                        AngleExtends[i] = Math.Round(lastAngle - diffAngle * i, 2);
                    }
                    //Console.WriteLine("AngleExtends[" + i + "]：--->" + AngleExtends[i]);
                }
                lastAngle = currentAngle;
                if (isFirstAver == false)
                {
                    for (int i = 3; i < sourceDataQc.Length - 3; i++)
                    {
                        if (collectDataFlag)
                        {
                            dt.Rows.Add(t.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                                t,
                                //(sourceDataQc[i] - 1) * 12.5,
                                //(sourceDataQh[i] - 1) * 12.5,
                                CoolFlow[i - 3],
                                HotFlow[i - 3] ,
                                (sourceDataQm[i])<0?0: (sourceDataQm[i]) * 6,
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
                                (sourceDataQm[i]) < 0 ? 0 : (sourceDataQm[i]) * 6,
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
                                (sourceDataQm[i]) < 0 ? 0 : (sourceDataQm[i]) * 6,
                                sourceDataTc[i] * 10,
                                sourceDataTh[i] * 10,
                                sourceDataTm[i] * 10,
                                sourceDataPc[i],
                                sourceDataPh[i],
                                sourceDataPm[i],
                                sourceDataQm5[i],
                                Wh);
                        }
                        if (QmTmTableFlag65)
                        {
                            QmTmTable65.Rows.Add(t.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                                (sourceDataQm[i]) < 0 ? 0 : (sourceDataQm[i]) * 6,
                                sourceDataTm[i] * 10
                                );
                        }
                        if (QmTmTableFlag50)
                        {
                            QmTmTable50.Rows.Add(t.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                                (sourceDataQm[i]) < 0 ? 0 : (sourceDataQm[i]) * 6,
                                sourceDataTm[i] * 10
                                );
                        }
                        if (AngleTmFlag)
                        {
                            AngleTmIndex++;
                            AngleTmTable.Rows.Add(t.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                                AngleExtends[i - 3],
                                sourceDataTm[i] * 10
                                );
                            
                        }
                        else
                        {
                            AngleTmIndex = 0;
                        }
                        t = t.AddMilliseconds(10.0);
                    }
                    //Qc = (sourceDataQc[3] + sourceDataQc[102] - 2) < 0 ? 0 : Math.Round((sourceDataQc[3] + sourceDataQc[102] - 2) * 12.5 * 0.5, 2, MidpointRounding.AwayFromZero) + (double)Properties.Settings.Default.QcAdjust;
                    //Qh = (sourceDataQh[3] + sourceDataQh[102] - 2) < 0 ? 0 : Math.Round((sourceDataQh[3] + sourceDataQh[102] - 2) * 12.5 * 0.5, 2, MidpointRounding.AwayFromZero) + (double)Properties.Settings.Default.QhAdjust;
                    Qc = CoolFlow[CoolFlow.Length - 1];
                    Qh = HotFlow[CoolFlow.Length - 1];
                    //Qm = (sourceDataQm[3] + sourceDataQm[102] - 2) < 0 ? 0 : Math.Round((sourceDataQm[3] + sourceDataQm[102] - 2) * 15 * 0.5, 2, MidpointRounding.AwayFromZero);
                    Qm = (sourceDataQm[3] + sourceDataQm[102]) < 0 ? 0 : Math.Round((sourceDataQm[3] + sourceDataQm[102]) * 6 * 0.5, 2, MidpointRounding.AwayFromZero);
                    QmTemp = Qm;
                    Tc = Math.Round((sourceDataTc[3] + sourceDataTc[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Th = Math.Round((sourceDataTh[3] + sourceDataTh[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    Tm = Math.Round((sourceDataTm[3] + sourceDataTm[102]) * 5, 2, MidpointRounding.AwayFromZero);
                    TmTemp = Tm;
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
                    Pc= Math.Round(Read("8716", 1) / 1000.0, 2, MidpointRounding.AwayFromZero);
                    Ph= Math.Round(Read("8716", 2) / 1000.0, 2, MidpointRounding.AwayFromZero); 
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

        #region 电机控制相关


        System.Timers.Timer monitorMTimer;            //监控M寄存器定时器
        System.Timers.Timer monitorDTimer;            //监控D寄存器定时器

        public double autoFindAngle_spin = 0;         //自动找点角度A
        public double autoFindAngle_upDown = 0;         //自动找点角度L   
        public double rSpin = 0;

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


        #region 电机相关定时器

        private delegate void MonitorMActiveDelegate();
        private void MonitorMActive()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MonitorMActiveDelegate monitorMActiveDelegate = MonitorMActive;
                    this.Invoke(monitorMActiveDelegate);
                }
                else
                {
                    #region 旋转电机状态
                    powerState_spin = bpq.read_coil(powerAddress_spin, 5);
                    forwardState_spin = bpq.read_coil(forwardReadAddress_spin, 5);
                    noForwadState_spin = bpq.read_coil(noForwardReadAddress_spin, 5);
                    orignState_spin = bpq.read_coil(orignReadAddress_spin, 5);

                    powerBtn_spin.BackColor = powerState_spin ? Color.Green : DefaultBackColor;
                    powerBtn_spin.Text = powerState_spin ? "伺服开" : "伺服关";
                    forwardBtn_spin.BackColor = forwardState_spin ? Color.Green : DefaultBackColor;
                    noForwardBtn_spin.BackColor = noForwadState_spin ? Color.Green : DefaultBackColor;
                    orignBtn_spin.BackColor = orignState_spin ? Color.Green : DefaultBackColor;
                    #endregion

                    #region 升降电机状态
                    powerState_upDown = bpq.read_coil(powerAddress_upDown, 5);
                    forwardState_upDown = bpq.read_coil(forwardReadAddress_upDown, 5);
                    noForwadState_upDown = bpq.read_coil(noForwardReadAddress_upDown, 5);
                    orignState_upDown = bpq.read_coil(orignReadAddress_upDown, 5);

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
                    radioValue_spin = bpq.read_uint(radioAddress_spin, 5);   //转速  读取 uint
                    angleValue_spin = bpq.read_int(angleAddress_spin, 5);    //角度  读取 int

                    radioLb_spin.Text = "" + Math.Round(radioValue_spin / 3200.0, 1);
                    angelLb_spin.Text = "" + Math.Round(angleValue_spin / 3200.0, 1);
                    if (isAutoFindAngle)
                    {
                        timeAngleDt.Rows.Add(
                            DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:fff"),
                            Math.Round(angleValue_spin / 10000.0, 1));
                    }
                    #endregion

                    #region 升降电机
                    radioValue_upDown = bpq.read_uint(radioAddress_upDown, 5);   //转速  读取 uint
                    angleValue_upDown = bpq.read_int(angleAddress_upDown, 5);    //角度  读取 int

                    var temp3 = Math.Round((radioValue_upDown / 4000.0), 1);
                    var temp4 = Math.Round((angleValue_upDown / 4000.0), 1);
                    radioLb_upDown.Text = "" + temp3;
                    angelLb_upDown.Text = "" + temp4;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Log.Error("电机角度转速读取异常：" + ex.ToString());
                return;
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
                bpq.write_coil(powerAddress_spin, false, 5);
                isAutoFindAngle = false;
            }
            else
                bpq.write_coil(powerAddress_spin, true, 5);
        }

        private void ForwardBtn_spin_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else
            {
                forwardBtn_spin.BackColor = Color.Green;
                bpq.write_coil(forwardWriteAddress_spin, true, 5);
            }
        }

        private void ForwardBtn_spin_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else
            {
                forwardBtn_spin.BackColor = DefaultBackColor;
                bpq.write_coil(forwardWriteAddress_spin, false, 5);
            }
        }

        private void NoForwardBtn_spin_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else
            {
                noForwardBtn_spin.BackColor = Color.Green;
                bpq.write_coil(noForwardWriteAddress_spin, true, 5);
            }
        }

        private void NoForwardBtn_spin_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else
            {
                noForwardBtn_spin.BackColor = DefaultBackColor;
                bpq.write_coil(noForwardWriteAddress_spin, false, 5);
            }
        }

        private void OrignBtn_spin_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else
            {
                orignBtn_spin.BackColor = Color.Green;
                bpq.write_coil(orignWriteAddress_spin, true, 5);
            }
        }

        private void OrignBtn_spin_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState_spin)
            {
                MessageBox.Show("请开启旋转电机");
            }
            else
            {
                orignBtn_spin.BackColor = DefaultBackColor;
                bpq.write_coil(orignWriteAddress_spin, false, 5);
            }
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
                if (forwardState_spin)
                    bpq.write_coil(forwardWriteAddress_spin, false, 5);
                else
                    bpq.write_coil(forwardWriteAddress_spin, true, 5);
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
                    bpq.write_coil(noForwardWriteAddress_spin, false, 5);
                else
                    bpq.write_coil(noForwardWriteAddress_spin, true, 5);
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
                    bpq.write_coil(orignWriteAddress_spin, false, 5);
                else
                    bpq.write_coil(orignWriteAddress_spin, true, 5);
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
                bpq.write_coil(shutdownAddress_spin, true, 5);
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
                bpq.write_coil(shutdownAddress_spin, false, 5);
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
                bpq.write_coil(backOrignAddress_spin, true, 5);
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
                bpq.write_coil(autoRunAddress_spin, true, 5);
            }
        }

        public double radio = 0;
        private void Radio_spin_Click(object sender, EventArgs e)
        {
            try
            {
                uint val2 = (uint)(double.Parse(radioTb_spin.Text) * 3200);
                radio = double.Parse(radioTb_spin.Text);
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
                    Write_uint(radioAddress_spin, val2, 5);
                    SystemInfoPrint("写入：【" + radioAddress_spin + "】【" + val2 + "】\n\n");
                }
            }
            catch(Exception ex)
            {
                return;
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

        private void rBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (isAutoFindAngle)
                {
                    return;
                }
                rSpin = rTb.Text.ToDouble();
                SystemInfoPrint("写入半径值：【" + rSpin + "】【" + DateTime.Now.ToString() + "】\n");

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
                    ElectDt.Clear();
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始找点");
                    isAutoFindAngle = true;
                    electDataFlag = true;
                    System.Threading.Thread.Sleep(100);
                    //正传
                    bpq.write_coil(forwardWriteAddress_spin, true, 5);
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始正传...");
                    while (isAutoFindAngle && (angleValue_spin <= autoFindAngle_spin))
                    {
                        //等待角度达到 预设角度
                    }
                    bpq.write_coil(forwardWriteAddress_spin, false, 5);
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "结束正传...");
                    if (isAutoFindAngle == false)
                    {
                        SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "当前已手动停止找点...");
                        timeAngleDt.Clear();
                        ElectDt.Clear();
                        return;
                    }
                    //反传
                    bpq.write_coil(noForwardWriteAddress_spin, true, 5);
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "开始反传...");
                    while (isAutoFindAngle && (angleValue_spin == 0))
                    {
                        //等待角度达到 原点
                    }
                    bpq.write_coil(noForwardWriteAddress_spin, false, 5);
                    SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "结束反传...");
                    if (isAutoFindAngle == false)
                    {
                        SystemInfoPrint("【" + DateTime.Now.ToString() + "】" + "当前已手动停止找点...");
                        timeAngleDt.Clear();
                        ElectDt.Clear();
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
                electDataFlag = false;
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
            foreach (DataRow tmRow in ElectDt.Rows)
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
                bpq.write_coil(powerAddress_upDown, false, 5);
            else
                bpq.write_coil(powerAddress_upDown, true, 5);
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
                    bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                else
                    bpq.write_coil(forwardWriteAddress_upDown, true, 5);
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
                    bpq.write_coil(noForwardWriteAddress_upDown, false, 5);
                else
                    bpq.write_coil(noForwardWriteAddress_upDown, true, 5);
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
                    bpq.write_coil(orignWriteAddress_upDown, false, 5);
                else
                    bpq.write_coil(orignWriteAddress_upDown, true, 5);
            }
        }
        private void NoForwardBtn_upDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                
                noForwardBtn_upDown.BackColor = Color.Green;
                bpq.write_coil(noForwardWriteAddress_upDown, true, 5);
            }
        }

        private void NoForwardBtn_upDown_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                noForwardBtn_upDown.BackColor = DefaultBackColor;
                bpq.write_coil(noForwardWriteAddress_upDown, false, 5);

            }
        }
        private void ForwardBtn_upDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                forwardBtn_upDown.BackColor = Color.Green;
                bpq.write_coil(forwardWriteAddress_upDown, true, 5);
            }
        }

        private void ForwardBtn_upDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                forwardBtn_upDown.BackColor = DefaultBackColor;
                //bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                bpq.write_coil(forwardWriteAddress_upDown, false, 5);
                //bpq.write_coil(backOrignAddress_upDown, true, 5);

            }
        }


        private void OrignBtn_upDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                orignBtn_upDown.BackColor = Color.Green;
                bpq.write_coil(orignWriteAddress_upDown, true, 5);
            }
        }

        private void OrignBtn_upDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (!powerState_upDown)
            {
                MessageBox.Show("请开启升降电机");
            }
            else
            {
                orignBtn_upDown.BackColor = DefaultBackColor;
                bpq.write_coil(orignWriteAddress_upDown, false, 5);
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
                bpq.write_coil(shutdownAddress_upDown, true, 5);
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
                bpq.write_coil(shutdownAddress_upDown, false, 5);
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
                bpq.write_coil(backOrignAddress_upDown, true, 5);
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
                bpq.write_coil(autoRunAddress_upDown, true, 5);
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

                Write_uint(radioAddress_upDown, val2, 5);
                SystemInfoPrint("写入：【" + radioAddress_upDown + "】【" + val2 + "】\n\n\n");
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
        #endregion

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

        #endregion

        #region 冷热水泵按钮 压力

        /// <summary>
        /// 冷水泵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HslPumpOne8_Click(object sender, EventArgs e)
        {
            var pump = sender as HslControls.HslPumpOne;
            if (pump.MoveSpeed == 0)//说明水泵当前处于关闭状态
            {
                //执行打开水泵的代码
                pump.MoveSpeed = 1;
                set_bit(ref doData2[0], 0, true);
                collectData.InstantDo_Write(doData2);
            }
            else//说明水泵当前处于打开状态
            {
                pump.MoveSpeed = 0;
                set_bit(ref doData2[0], 0, false);
                collectData.InstantDo_Write(doData2);
            }
        }

        /// <summary>
        /// 热水泵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reshui_Click(object sender, EventArgs e)
        {
            var pump = sender as HslControls.HslPumpOne;
            if (pump.MoveSpeed == 0)//说明水泵当前处于关闭状态
            {
                //执行打开水泵的代码
                pump.MoveSpeed = 1;
                set_bit(ref doData2[0], 1, true);
                collectData.InstantDo_Write(doData2);
            }
            else//说明水泵当前处于打开状态
            {
                pump.MoveSpeed = 0;
                set_bit(ref doData2[0], 1, false);
                collectData.InstantDo_Write(doData2);
            }
        }

        /// <summary>
        /// 设置冷水压力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoolPump_ValueChanged(object sender, EventArgs e)
        {
            int val2 = Convert.ToInt32(CoolPump.Value * 500) ;

            if (val2 < 0 || val2 > 5000)
            {
                MessageBox.Show("请输入0-10 范围内的值");
                CoolPump.Value = 0;
            }
            else
            {
                short val3 = Convert.ToInt16(val2);
                Write("8193", val3, 1);
                SystemInfoPrint("写入：【8193】【" + val3 + "】\n");
            }
        }

        /// <summary>
        /// 热水压力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeatPump_ValueChanged(object sender, EventArgs e)
        {
            int val2 = Convert.ToInt32(HeatPump.Value * 500) ;

            if (val2 < 0 || val2 > 5000)
            {
                MessageBox.Show("请输入0-10 范围内的值");
                HeatPump.Value = 0;
            }
            else
            {
                short val3 = Convert.ToInt16(val2);
                Write("8193", val3, 2);
                SystemInfoPrint("写入：【8193】【" + val3 + "】\n");
            }
        }







        #endregion

        private void HslButton4_Click(object sender, EventArgs e)
        {
            FormCurve form = new FormCurve(AngleTmTable, LogicTypeEnum.SensitivityTest, this, 17100, true, model);
            form.Show();
            FormCurve form2 = new FormCurve(AngleTmTable, LogicTypeEnum.SensitivityTest, this, 17100, true, model, 1);
            form2.Show();
            FormCurve form3 = new FormCurve(AngleTmTable, LogicTypeEnum.SensitivityTest, this, 17100, true, model, 2);
            form3.Show();
        }

        private void HslButton5_Click(object sender, EventArgs e)
        {
            FormCurve form = new FormCurve(AngleTmTable, LogicTypeEnum.TmSteadyTest50, this, 0, true,model);
            form.Show();
            FormCurve form2 = new FormCurve(AngleTmTable, LogicTypeEnum.TmSteadyTest65, this, 0, true, model);
            form2.Show();
        }
    }
}
