using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using 恒温测试机.Model.Enum;
using 恒温测试机.Utils;
using FFT;

namespace 恒温测试机.UI
{
    public partial class FormCurve : Form
    {

        public FormCurve(DataTable graphData, Model.Enum.LogicTypeEnum logicType,int AngleTmMidIndex)
        {
            InitializeComponent();
            InitializeChart();
            this.myChart.GetToolTipText += new EventHandler<ToolTipEventArgs>(myChart_GetToolTipText);
            this.graphData = graphData;
            this.logicType = logicType;
            this.AngleTmMidIndex = AngleTmMidIndex;
        }

        /// <summary>
        /// 灵敏度曲线
        /// </summary>
        public FormCurve(DataTable graphData, Model.Enum.LogicTypeEnum logicType,FormMain formMain, int AngleTmMidIndex=0, bool TestFlag=false)
        {
            InitializeComponent();
            InitializeChart();
            this.myChart.GetToolTipText += new EventHandler<ToolTipEventArgs>(myChart_GetToolTipText);
            this.graphData = graphData;
            this.logicType = logicType;
            this.AngleTmMidIndex = AngleTmMidIndex;
            this.formMain = formMain;
            this.TsetFlag = TestFlag;
        }


        private FormMain formMain;
        private DataTable graphData;
        private LogicTypeEnum logicType;
        private int AngleTmMidIndex;
        private bool TsetFlag = false;


        public void InitializeChart()
        {
            #region 设置图表的属性
            //图表的背景色
            myChart.BackColor = Color.FromArgb(211, 223, 240);
            //图表背景色的渐变方式
            myChart.BackGradientStyle = GradientStyle.TopBottom;
            //图表的边框颜色、
            myChart.BorderlineColor = Color.FromArgb(26, 59, 105);
            //图表的边框线条样式
            myChart.BorderlineDashStyle = ChartDashStyle.Dash;
            //图表边框线条的宽度
            myChart.BorderlineWidth = 2;
            //图表边框的皮肤
            myChart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            #endregion

            #region 设置图表的Title
            Title title = new Title();
            //标题内容
            title.Text = "曲线图演示";
            //标题的字体
            title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            //标题字体颜色
            title.ForeColor = Color.FromArgb(26, 59, 105);
            //标题阴影颜色
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            //标题阴影偏移量
            title.ShadowOffset = 3;

            myChart.Titles.Add(title);
            #endregion

            #region 设置图表区属性
            //图表区的名字
            ChartArea chartArea = new ChartArea("Default");
            //背景色
            chartArea.BackColor = Color.FromArgb(64, 165, 191, 228);
            //背景渐变方式
            chartArea.BackGradientStyle = GradientStyle.TopBottom;
            //渐变和阴影的辅助背景色
            chartArea.BackSecondaryColor = Color.White;
            //边框颜色
            chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
            //阴影颜色
            chartArea.ShadowColor = Color.Transparent;

            //设置X轴和Y轴线条的颜色和宽度
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.LineWidth = 1;

            //设置X轴和Y轴的标题
            chartArea.AxisX.Title = "横坐标标题";
            chartArea.AxisY.Title = "纵坐标标题";

            //设置图表区网格横纵线条的颜色和宽度
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineWidth = 1;

            //myChart.ChartAreas.Add(chartArea);
            //ChartArea chartArea2 = new ChartArea("Default2");
            //myChart.ChartAreas.Add(chartArea2);
            myChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            myChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            #endregion
            //myChart.ChartAreas[0].Position= new ElementPosition(0, 0, 90, 90);

            #region 图例及图例的位置
            Legend legend = new Legend();
            legend.Alignment = StringAlignment.Center;
            legend.Docking = Docking.Bottom;

            this.myChart.Legends.Add(legend);
            #endregion
        }

        private Series SetSeriesStyle(int i)
        {
            Series series = new Series(string.Format("第{0}条数据", i + 1));

            //Series的类型
            series.ChartType = SeriesChartType.Line;
            //Series的边框颜色
            series.BorderColor = Color.FromArgb(180, 26, 59, 105);
            //线条宽度
            series.BorderWidth = 1;
            //线条阴影颜色
            series.ShadowColor = Color.Black;
            //阴影宽度
            //series.ShadowOffset = 1;
            //是否显示数据说明
            series.IsVisibleInLegend = true;
            //线条上数据点上是否有数据显示
            series.IsValueShownAsLabel = false;
            //线条上的数据点标志类型
            series.MarkerStyle = MarkerStyle.Circle;
            //线条数据点的大小
            series.MarkerSize = 1;
            //线条颜色
            switch (i)
            {
                case 0:
                    series.Color = Color.FromArgb(220, 65, 140, 240);
                    series.Name = "0";
                    break;
                case 1:
                    series.Color = Color.FromArgb(220, 224, 64, 10);
                    break;
                case 2:
                    series.Color = Color.FromArgb(220, 120, 150, 20);
                    break;
                case 3:
                    series.Color = Color.FromArgb(120, 120, 150, 20);
                    break;
                case 4:
                    series.Color = Color.FromArgb(220, 220, 150, 20);
                    series.Name = "A";
                    series.MarkerSize = 8;
                    break;
            }
            return series;
        }


        private void FormCurve_Load(object sender, EventArgs e)
        {
            if (logicType == LogicTypeEnum.SensitivityTest || logicType == LogicTypeEnum.FidelityTest)
            {
                load_Data_AngleTm();
            }
            else
            {
                load_Data_QmTm();
            }


            //myChart.ChartAreas[0].Axes[0].MajorGrid;

            //double[] xdata = new double[10] { 1f, 1.01f, 1.02f, 1.03f, 1.04f, 1.05f, 1.06f, 1.07f, 1.08f, 1.09f };
            //double[] ydata = new double[10] { 1f, 2.99f, 4.01f, 3.01f, 6.01f, 5.01f, 3.01f, 8.01f, 3.01f, 2.01f };
            //Series series = this.SetSeriesStyle(0);

            //for (int i = 0; i < xdata.Length; i++)
            //{
            //    series.Points.AddXY(Math.Round(xdata[i], 2), Math.Round(ydata[i], 2));
            //}
            //for (int i = xdata.Length - 1; i > -1; i--)
            //{
            //    series.Points.AddXY(Math.Round(xdata[i], 2), Math.Round(ydata[i], 2));
            //}
            //this.myChart.Series.Add(series);

            #region 指定一个坐标、或者point的位置，在其附近添加文字描述信息
            //TextAnnotation textAnnotation = new TextAnnotation();
            //textAnnotation.AxisX = myChart.ChartAreas[0].AxisX;
            //textAnnotation.AxisY = myChart.ChartAreas[0].AxisY;
            ////textAnnotation.AnchorX = 1;
            ////textAnnotation.AnchorY = 1;
            //textAnnotation.SetAnchor(series.Points[4]);
            //textAnnotation.Name = "123";
            //textAnnotation.Text = "这是一个文字标记";
            //textAnnotation.Visible = true;
            //myChart.Annotations.Add(textAnnotation);
            #endregion

            #region 添加指定起点、长度、方向的辅助线，没有终点，适用于跟坐标轴平行的辅助线
            //LineAnnotation annotation = new LineAnnotation();
            //annotation.IsSizeAlwaysRelative = true;
            //annotation.AxisX = myChart.ChartAreas[0].AxisX;
            //annotation.AxisY = myChart.ChartAreas[0].AxisY;
            //annotation.Name = "辅助线2";
            //annotation.ToolTip = "这是一个辅助线";//鼠标悬停在辅助线上面显示的信息
            //annotation.AllowTextEditing = true;
            //annotation.AnchorX = 10;
            //annotation.AnchorY = 20;//绝对坐标定位开始坐标
            ////annotation.AnchorDataPoint = series.Points[2];//使用series上的点作为起点
            ////annotation.SetAnchor(series.Points[4]);//使用series上的点作为起点
            ////annotation.X = 1;
            ////annotation.Y = 1;
            //annotation.LineDashStyle = ChartDashStyle.Dash;           
            ////由于没有终点，因此通过下面的两个属性来进行长度和方向的控制
            ////垂直线示例
            ////annotation.Height = -15;
            ////annotation.Width = 0;
            ////平行线示例
            //annotation.Height = 15;
            //annotation.Width = 15;
            //annotation.LineWidth = 2;
            //annotation.StartCap = LineAnchorCapStyle.None;
            //annotation.EndCap = LineAnchorCapStyle.None;
            //myChart.Annotations.Add(annotation);
            #endregion

            #region 添加两点之间的线段
            ////添加两点之间的线段
            //myChart.Series.Add(addLine_between2Point(1, 1, 1.05, 5.5));
            #endregion

            #region 添加特殊点

            //myChart.Series.Add(addMarkedPoint(1, 1, "ABC"));
            #endregion

            //区域高亮
            //addLateline(myChart,"0.4");  
        }

        /// <summary>
        /// 添加文字信息描述
        /// </summary>
        private TextAnnotation addDescription(double x,double y,string name,string text)
        {
            TextAnnotation textAnnotation = new TextAnnotation();
            textAnnotation.AxisX = myChart.ChartAreas[0].AxisX;
            textAnnotation.AxisY = myChart.ChartAreas[0].AxisY;
            textAnnotation.AnchorX = x;
            textAnnotation.AnchorY = y;
            //textAnnotation.SetAnchor(series.Points[4]);
            textAnnotation.Name = name;
            textAnnotation.Text = text;
            textAnnotation.Visible = true;
            return textAnnotation;
        }

        /// <summary>
        /// 添加指定起点、长度、方向的辅助线，没有终点，适用于跟坐标轴平行的辅助线
        /// </summary>
        private LineAnnotation addLine_Axis(string name,string toolTip,double x,double y,double height,double width)
        {
            LineAnnotation annotation = new LineAnnotation();
            annotation.IsSizeAlwaysRelative = true;
            annotation.AxisX = myChart.ChartAreas[0].AxisX;
            annotation.AxisY = myChart.ChartAreas[0].AxisY;
            annotation.Name = name;
            annotation.ToolTip = toolTip;//鼠标悬停在辅助线上面显示的信息
            annotation.AllowTextEditing = true;
            annotation.AnchorX = x;
            annotation.AnchorY = y;//绝对坐标定位开始坐标
            //annotation.AnchorDataPoint = series.Points[2];//使用series上的点作为起点
            //annotation.SetAnchor(series.Points[4]);//使用series上的点作为起点
            //annotation.X = 1;
            //annotation.Y = 1;
            //线条样式的选择
            annotation.LineDashStyle = ChartDashStyle.Dash;
            //由于没有终点，因此通过下面的两个属性来进行长度和方向的控制
            //垂直线示例
            //annotation.Height = -15;
            //annotation.Width = 0;
            //平行线示例
            annotation.Height = height;
            annotation.Width = width;
            annotation.LineWidth = 1;
            annotation.StartCap = LineAnchorCapStyle.None;
            annotation.EndCap = LineAnchorCapStyle.None;
            return annotation;
        }

        /// <summary>
        /// 添加两点之间的线段
        /// </summary>
        private Series addLine_between2Point(double x1, double y1, double x2, double y2, MarkerStyle markerStyle= MarkerStyle.Circle,string toopTip="")
        {
            Series s = new Series();
            //这里面的两个数是可以为任意数，由计算得出的
            s.Points.AddXY(x1,y1);
            s.Points.AddXY(x2,y2);
            //Series的类型   画线的类型
            s.ChartType = SeriesChartType.Line;          
            //Series的边框颜色
            s.BorderColor = Color.FromArgb(180, 26, 59, 105);
            //线条宽度
            s.BorderWidth = 2;
            //线条阴影颜色
            s.ShadowColor = Color.Black;
            //阴影宽度
            //series.ShadowOffset = 1;
            //是否显示数据说明
            s.IsVisibleInLegend = true;
            //线条上数据点上是否有数据显示
            s.IsValueShownAsLabel = false;
            //线条上的数据点标志类型
            s.MarkerStyle = markerStyle;
            //线条数据点的大小            
            s.MarkerSize = 8;
            s.ToolTip = toopTip;
            s.IsVisibleInLegend = false;
            return s;
        }

        /// <summary>
        /// 添加特殊点
        /// </summary>
        private Series addMarkedPoint(double x1, double y1,string label)
        {
            Series s = new Series();
            s.Label = label;            
            //这里面的两个数是可以为任意数，由计算得出的
            s.Points.AddXY(x1,y1);         
            //Series的类型
            s.ChartType = SeriesChartType.Line;
            //Series的边框颜色
            s.BorderColor = Color.FromArgb(180, 26, 59, 105);
            //线条宽度
            s.BorderWidth = 2;
            //线条阴影颜色
            s.ShadowColor = Color.Black;
            //阴影宽度
            //series.ShadowOffset = 1;
            //是否显示数据说明
            s.IsVisibleInLegend = true;
            //线条上数据点上是否有数据显示
            s.IsValueShownAsLabel = false;
            //线条上的数据点标志类型
            s.MarkerStyle = MarkerStyle.Circle;
            //线条数据点的大小            
            s.MarkerSize = 8;
            s.IsVisibleInLegend = false;
            s.SmartLabelStyle.Enabled=true;
            s.SmartLabelStyle.IsMarkerOverlappingAllowed = false;
            
            return s;
        }

        /// <summary>
        /// 区域高亮
        /// </summary>
        private void addLateline(Chart chart, String strLate)
        {
            StripLine stripline = new StripLine();
            stripline.Interval = 0;
            stripline.IntervalOffset = double.Parse(strLate) * 10;
            stripline.StripWidth = 1;
            stripline.BackColor = Color.Red;
            stripline.BorderDashStyle = ChartDashStyle.Dash;
            chart.ChartAreas[0].AxisX.StripLines.Add(stripline);
        }

        private void myChart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            //if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            //{
            //    int i = e.HitTestResult.PointIndex;
            //    DataPoint dp = e.HitTestResult.Series.Points[i];
            //    e.Text = string.Format("时间:{0};数值:{1:F1} ", DateTime.FromOADate(dp.XValue), dp.YValues[0]);
            //}
        }

        #region 灵敏度曲线


        private void load_Data_AngleTm()
        {
            new Thread(new ThreadStart(ThreadRead_AngleTm)) { IsBackground = true }.Start();
        }

        private int upCount = 0;
        private delegate void AngleTmDelegate();   
        private void ThreadRead_AngleTm()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    AngleTmDelegate angleTmDelegate = ThreadRead_AngleTm;
                    this.Invoke(angleTmDelegate);
                }
                else
                {
                    if(TsetFlag)
                        graphData = CsvToDataTable(@"D:\灵敏度0904.csv", 1);
                    Dictionary<double, bool> keyValues1 = new Dictionary<double, bool>();
                    Dictionary<double, bool> keyValues2 = new Dictionary<double, bool>();
                    bool flag = true;
                    int index = 0;
                    var AngleTmTable = new DataTable();
                    AngleTmTable.Columns.Add("角度", typeof(double));   //新建第一列 通道0
                    AngleTmTable.Columns.Add("出水温度Tm", typeof(double));   //1
                    foreach (DataRow row in graphData.Rows)
                    {
                        index++;
                        if (index == AngleTmMidIndex)
                        {
                            flag = false;
                        }
                        var qm = Convert.ToDouble(row[1]);
                        qm = Math.Abs(qm);
                        if (flag)
                        {
                            if (keyValues1.ContainsKey(qm))
                            {
                                continue;
                            }
                            else
                            {
                                upCount++;
                                keyValues1.Add(qm, true);
                            }
                        }
                        else
                        {
                            if (keyValues2.ContainsKey(qm))
                            {
                                continue;
                            }
                            else
                            {
                                keyValues2.Add(qm, true);
                            }
                        }

                        AngleTmTable.Rows.Add(
                            qm,
                            Convert.ToDouble(row[2])
                            );
                    }
                    //QmTmTable.DefaultView.Sort = "出水流量Qm DESC";//按Id倒序;

                    //var orderData = QmTmTable.DefaultView.ToTable();
                    double[] xdata = new double[AngleTmTable.Rows.Count];
                    double[] ydata = new double[AngleTmTable.Rows.Count];
                    index = 0;
                    double G11_X = 0; bool G11Flag = true;double G11_Y = 0;
                    double G21_X = 0; bool G21Flag = true;double G21_Y = 0;
                    double G12_X = 0; bool G12Flag = true;double G12_Y = 0;
                    double G22_X = 0; bool G22Flag = true; double G22_Y = 0;
                    double GA_X = 0; bool GAFlag = true; double GA_Y = 0;
                    double GB_X = 0; bool GBFlag = true;double GB_Y = 0;
                    double GC_X = 0; bool GCFlag = true; double GC_Y = 0;
                    double GD_X = 0; bool GDFlag = true; double GD_Y = 0;
                    double orgTm = 37.15;
                    foreach (DataRow row in AngleTmTable.Rows)
                    {
                        xdata[index] = Convert.ToDouble(row[0]);
                        ydata[index] = Convert.ToDouble(row[1]);
                        if (index < upCount)        //当前数据为左转数据
                        {
                            if (Math.Abs(ydata[index] - 38) <= 0.2 && GBFlag)
                            {
                                GB_X = xdata[index];    //角度  读取
                                GB_Y = ydata[index];
                                formMain.SystemInfoPrint("38度的角度B为——>" + GB_X);
                                GBFlag = false;
                            }
                            if (Math.Abs(ydata[index] - (orgTm - 4)) <= 0.2 && G21Flag)
                            {
                                G21_X = xdata[index];    //角度  读取
                                G21_Y = ydata[index];
                                formMain.SystemInfoPrint("G2:" + (orgTm - 4) + "的角度为——>" + G21_X);
                                G21Flag = false;
                            }
                            if (Math.Abs(ydata[index] - (orgTm + 4)) <= 0.2 && G22Flag)
                            {
                                G22_X = xdata[index];    //角度  读取
                                G22_Y = ydata[index];
                                formMain.SystemInfoPrint("G2:" + (orgTm + 4) + "的角度为——>" + G22_X);
                                G22Flag = false;
                            }
                        }
                        else
                        {
                            if (Math.Abs(ydata[index] - 38) <= 0.2 && GAFlag)
                            {
                                GA_X = xdata[index];    //角度  读取
                                GA_Y = ydata[index];
                                formMain.SystemInfoPrint("38度的角度A为——>" + GA_X);
                                GAFlag = false;
                            }
                            if (Math.Abs(ydata[index] - (orgTm - 4)) <= 0.2 && G11Flag)
                            {
                                G11_X = xdata[index];    //角度  读取
                                G11_Y = ydata[index];
                                formMain.SystemInfoPrint("G1:" + (orgTm - 4) + "的角度为——>" + G11_X);
                                G11Flag = false;
                            }
                            if (Math.Abs(ydata[index] - (orgTm + 4)) <= 0.2 && G12Flag)
                            {
                                G12_X = xdata[index];    //角度  读取
                                G12_Y = ydata[index];
                                formMain.SystemInfoPrint("G1:" + (orgTm + 4) + "的角度为——>" + G12_X);
                                G12Flag = false;
                            }
                        }
                        if (index == 0)
                        {
                            Console.WriteLine(xdata[index]);
                            Console.WriteLine(ydata[index]);
                        }
                        index++;
                    }
                    index = 0;
                    foreach (DataRow row in AngleTmTable.Rows)
                    {
                        xdata[index] = Convert.ToDouble(row[0]);
                        ydata[index] = Convert.ToDouble(row[1]);
                        if (index < upCount)        //当前数据为左转数据
                        {
                            if (Math.Abs(xdata[index] - (GA_X+GB_X)*0.5) <= 0.2 && GCFlag)
                            {
                                GC_X = xdata[index];    //角度  读取
                                GC_Y = ydata[index];
                                GCFlag = false;
                            }
                        }
                        else
                        {
                            if (Math.Abs(xdata[index] - (GA_X + GB_X) * 0.5) <= 0.2 && GDFlag)
                            {
                                GD_X = xdata[index];    //角度  读取
                                GD_Y = ydata[index];
                                GDFlag = false;
                            }
                        }
                        index++;
                    }

                    Series series = this.SetSeriesStyle(0);
                    //修改图例里面显示的内容
                    series.LegendText = "text";
                    for (int i = 0; i < xdata.Length; i++)
                    {
                        series.Points.AddXY(xdata[i], ydata[i]);
                    }
                    this.myChart.Series.Add(series);
                    //设置坐标轴范围
                    myChart.ChartAreas[0].AxisX.Minimum = 0;
                    myChart.ChartAreas[0].AxisX.Maximum = 75;
                    myChart.ChartAreas[0].AxisY.Maximum = 70;
                    myChart.ChartAreas[0].AxisY.Minimum = 0;
                    //myChart.Annotations.Add(addDescription(GA_X,GA_Y,"pointA","A"));
                    //myChart.Annotations.Add(addDescription(GB_X,GB_Y,"pointB","B"));
                    myChart.Annotations.Add(addLine_Axis("Tm+4", "41.15", 0, 41.15, 0, 75));
                    myChart.Annotations.Add(addLine_Axis("Tm-4", "33.15", 0, 33.15, 0, 75));
                    myChart.Series.Add(addMarkedPoint(GA_X, GA_Y, "A"));
                    myChart.Series.Add(addMarkedPoint(GB_X, GB_Y, "B"));
                    myChart.Series.Add(addMarkedPoint(GC_X, GC_Y, "C"));
                    myChart.Series.Add(addMarkedPoint(GD_X, GD_Y, "D"));
                    myChart.Series.Add(addMarkedPoint(G11_X, G11_Y, ""));
                    myChart.Series.Add(addMarkedPoint(G12_X, G12_Y, ""));
                    myChart.Series.Add(addMarkedPoint(G21_X, G21_Y, ""));
                    myChart.Series.Add(addMarkedPoint(G22_X, G22_Y, ""));

                    myChart.Series.Add(addLine_between2Point(GA_X, GA_Y, GB_X, GB_Y));
                    myChart.Series.Add(addLine_between2Point(GC_X, GC_Y, GD_X, GD_Y));

                    myChart.Series.Add(addLine_between2Point(G11_X, G11_Y, G11_X, G12_Y+10,MarkerStyle.None));
                    myChart.Series.Add(addLine_between2Point(G12_X, G12_Y, G12_X, G12_Y+10, MarkerStyle.None));
                    var G1 = G12_X - G11_X;
                    myChart.Annotations.Add(addDescription(G11_X, G12_Y + 10, "G1", "G1:" + G1));
                    myChart.Series.Add(addLine_between2Point(G11_X, G12_Y + 10, G12_X, G12_Y + 10, MarkerStyle.Diamond));

                    myChart.Series.Add(addLine_between2Point(G21_X, G21_Y, G21_X, G21_Y - 10, MarkerStyle.None));
                    myChart.Series.Add(addLine_between2Point(G22_X, G22_Y, G22_X, G21_Y - 10, MarkerStyle.None));
                    var G2 = G22_X - G21_X;
                    myChart.Annotations.Add(addDescription(G21_X, G21_Y - 10, "G2","G2:"+ G2));
                    myChart.Series.Add(addLine_between2Point(G21_X, G21_Y - 10, G22_X, G21_Y - 10, MarkerStyle.Diamond));

                    var tmDiff = GD_Y - GC_Y;
                    myChart.Annotations.Add(addDescription(GD_X, GD_Y, "保真度温度差", "保真度温度差:" + tmDiff));

                    //var G1Height = G12_Y - G11_Y + 10;
                    //myChart.Annotations.Add(addLine_Axis("G12Y", "", G11_X, G11_Y, -G1Height, 0));
                    //myChart.Annotations.Add(addLine_Axis("G11Y", "", G12_X, G12_Y, -10, 0));

                    //var G2Height = G22_Y - G21_Y;
                    //myChart.Annotations.Add(addLine_Axis("G22Y", "", G22_X, G22_Y, G2Height, 0));
                    //myChart.Annotations.Add(addLine_Axis("G21Y", "", G21_X, G21_Y, 10, 0));



                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return;
            }
        }
        #endregion

        #region 温度稳定性曲线
        private void load_Data_QmTm()
        {
            new Thread(new ThreadStart(ThreadRead_QmTm)) { IsBackground = true }.Start();
        }

        private delegate void QmTmDelegate();
        private void ThreadRead_QmTm()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    QmTmDelegate qmTmDelegate = ThreadRead_QmTm;
                    this.Invoke(qmTmDelegate);
                }
                else
                {
                    if (TsetFlag)
                        graphData = CsvToDataTable(@"D:\稳定性0904.csv", 1);
                    var QmTmTable = new DataTable();
                    QmTmTable.Columns.Add("出水流量Qm", typeof(double));   //新建第一列 通道0
                    QmTmTable.Columns.Add("出水温度Tm", typeof(double));   //1
                    Dictionary<double, bool> keyValues = new Dictionary<double, bool>();
                    foreach (DataRow row in graphData.Rows)
                    {
                        var qm = Convert.ToDouble(row[1]);
                        if (keyValues.ContainsKey(qm))
                        {
                            continue;
                        }
                        else
                        {
                            keyValues.Add(qm, true);
                        }
                        QmTmTable.Rows.Add(
                            qm,
                            Convert.ToDouble(row[2])
                            );
                    }
                    QmTmTable.DefaultView.Sort = "出水流量Qm DESC";//按Id倒序;

                    var orderData = QmTmTable.DefaultView.ToTable();
                    double[] xdata = new double[orderData.Rows.Count];
                    double[] ydata = new double[orderData.Rows.Count];
                    int index = 0;
                    bool Tm1Flag = false;
                    bool Tm2Flag = false;
                    double Tm1_x = 0;
                    double Tm1_y = 0;
                    double Tm2_x = 0;
                    double Tm2_y = 0;

                    foreach (DataRow row in orderData.Rows)
                    {
                        xdata[index] = Convert.ToDouble(row[0]);
                        ydata[index] = Convert.ToDouble(row[1]);

                        if (Math.Abs(xdata[index] - 6) <= 0.1 && Tm1Flag == false)
                        {
                            Tm1_x = xdata[index];
                            Tm1_y = ydata[index];
                            Tm1Flag = true;
                            formMain.SystemInfoPrint("流量为6L的出水温度——>" + Tm1_y);
                        }
                        if (Math.Abs(xdata[index] - 3) <= 0.1 && Tm2Flag == false)
                        {
                            Tm2_x = xdata[index];
                            Tm2_y = ydata[index];
                            Tm2Flag = true;
                            formMain.SystemInfoPrint("流量为3L的出水温度——>" + Tm2_y);
                        }
                        index++;
                    }
                    //滤波前数据
                    Series series2 = this.SetSeriesStyle(2);
                    for (int i = 0; i < xdata.Length; i++)
                    {
                        //添加数据
                        series2.Points.AddXY(xdata[i], ydata[i]);

                    }
                    this.myChart.Series.Add(series2);
                    Series series = this.SetSeriesStyle(0);
                    #region 傅里叶变化例子
                    //数据填充
                    //List<double> Y = new List<double>(ydata);
                    //Console.WriteLine("ydata:" + ydata.Length);
                    //Console.WriteLine("a:" + Y.ToArray().Length);
                    //Y =TWFFT.DataFill(Y);
                    //Console.WriteLine("b:" + Y.ToArray().Length);                   
                    //float[] y = new float[Y.ToArray().Length];
                    //y = TWFFT.FFT_filter(Y.ToArray(), 0.1);//第二个参数可调，调整范围是：(0,1)。为1 的时候没有滤波效果，为0的时候将所有频率都过滤掉。
                    //int putlen = TWFFT.putlen;//获取填充数据的长度的一半
                    //Console.WriteLine("putlen:" + putlen);
                    //Console.WriteLine("a:" + Y.ToArray().Length);
                    //for (int i = 0; i < xdata.Length; i++)
                    //{
                    //    ydata[i] = Math.Round(y[i + putlen], 2);
                    //}
                    ydata = TWFFT.filterFFT(ydata, 0.1);
                    Console.WriteLine("#:" + ydata.ToArray().Length);
                    //for (int i = 0; i < ydata.Length; i++)
                    //    ydata[i] = y[i];
                    #endregion

                    bool Tm1Flag1 = false;
                    bool Tm2Flag1 = false;
                    double Tm1_x1 = 0;
                    double Tm1_y1 = 0;
                    double Tm2_x1 = 0;
                    double Tm2_y1 = 0;
                    for (int i = 0; i < xdata.Length; i++)
                    {
                        //添加数据
                        series.Points.AddXY(xdata[i], ydata[i]);
                        if (Math.Abs(xdata[i] - 6) <= 0.1 && Tm1Flag1 == false)
                        {
                            Tm1_x1 = xdata[i];
                            Tm1_y1 = ydata[i];
                            Tm1Flag1 = true;
                            formMain.SystemInfoPrint("流量为6L的出水温度——>" + Tm1_y1);
                        }
                        if (Math.Abs(xdata[i] - 3) <= 0.1 && Tm2Flag1 == false)
                        {
                            Tm2_x1 = xdata[i];
                            Tm2_y1 = ydata[i];
                            Tm2Flag1 = true;
                            formMain.SystemInfoPrint("流量为3L的出水温度——>" + Tm2_y1);
                        }
                    }
                    this.myChart.Series.Add(series);
                    //设置坐标轴范围
                    myChart.ChartAreas[0].AxisX.Minimum = 0;
                    myChart.ChartAreas[0].AxisX.Maximum = 15;
                    myChart.ChartAreas[0].AxisY.Maximum = 45;
                    myChart.ChartAreas[0].AxisY.Minimum =30;
                    myChart.ChartAreas[0].AxisX.IsReversed = true;

                    myChart.Annotations.Add(addLine_Axis("Tm38", "38", 0, 38, 0, -75));
                    myChart.Annotations.Add(addLine_Axis("Qm6_x", "6L", 0, Tm1_y, 0, -75));
                    myChart.Annotations.Add(addLine_Axis("Qm3_x", "3L", 0, Tm2_y, 0,-75));
                    myChart.Annotations.Add(addLine_Axis("Qm6_x1", "6L", 0, Tm1_y1, 0, -75));
                    myChart.Annotations.Add(addLine_Axis("Qm3_x1", "3L", 0, Tm2_y1, 0, -75));

                    myChart.Annotations.Add(addLine_Axis("Qm6", "6L", 6, 30, -75, 0));
                    myChart.Annotations.Add(addLine_Axis("Qm3", "3L", 3, 30, -75, 0));

                    //myChart.Annotations.Add(addDescription(1, 38, "Tm38Des", "38℃"));
                    //myChart.Annotations.Add(addDescription(1, Tm1_y, "Tm1_yDes", Tm1_y+"℃"));
                    //myChart.Annotations.Add(addDescription(1, Tm2_y-2, "Tm2_yDes", Tm2_y+ "℃"));

                    myChart.Series.Add(addMarkedPoint(0, Tm1_y, Tm1_y + "℃"));
                    myChart.Series.Add(addMarkedPoint(0, Tm2_y, Tm2_y + "℃"));

                    myChart.Series.Add(addMarkedPoint(0, 38, "38℃"));

                    myChart.Series.Add(addMarkedPoint(6, Tm1_y, "Q1"));
                    myChart.Series.Add(addMarkedPoint(3, Tm2_y, "Q2"));

                    myChart.Series.Add(addMarkedPoint(6, Tm1_y1, "Q11"));
                    myChart.Series.Add(addMarkedPoint(3, Tm2_y1, "Q21"));

                    double Tm1Diff = Math.Round(Math.Abs(Tm1_y - 38), 2);
                    myChart.Series.Add(addLine_between2Point(13, Tm1_y, 13, 38, MarkerStyle.Diamond));
                    myChart.Annotations.Add(addDescription(13, Tm1_y, "Qm6Des", "Tm1与38℃的温差：" + Tm1Diff + "℃"));

                    double Tm2Diff = Math.Round(Math.Abs(Tm2_y - 38), 2);
                    myChart.Series.Add(addLine_between2Point(12.5, Tm2_y, 12.5, 38, MarkerStyle.Diamond));
                    myChart.Annotations.Add(addDescription(12.5, Tm2_y, "Qm3Des", "Tm2与38℃的温差：" + Tm2Diff + "℃"));

                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
                return;
            }
            
        }
        #endregion

        #region 相关算法


        ///<summary>
        ///用最小二乘法拟合二元多次曲线
        ///例如y=ax+b
        ///其中MultiLine将返回a，b两个参数。
        ///a对应MultiLine[1]
        ///b对应MultiLine[0]
        ///</summary>
        ///<param name="arrX">已知点的x坐标集合</param>
        ///<param name="arrY">已知点的y坐标集合</param>
        ///<param name="length">已知点的个数</param>
        ///<param name="dimension">方程的最高次数</param>
        public double[] MultiLine(double[] arrX, double[] arrY, int length, int dimension)//二元多次线性方程拟合曲线
        {
            int n = dimension + 1;                  //dimension次方程需要求 dimension+1个 系数
            double[,] Guass = new double[n, n + 1];      //高斯矩阵 例如：y=a0+a1*x+a2*x*x
            for (int i = 0; i < n; i++)
            {
                int j;
                for (j = 0; j < n; j++)
                {
                    Guass[i, j] = SumArr(arrX, j + i, length);
                }
                Guass[i, j] = SumArr(arrX, i, arrY, 1, length);
            }

            return ComputGauss(Guass, n);

        }
        private double SumArr(double[] arr, int n, int length) //求数组的元素的n次方的和
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != 0 || n != 0)
                    s = s + Math.Pow(arr[i], n);
                else
                    s = s + 1;
            }
            return s;
        }
        private double SumArr(double[] arr1, int n1, double[] arr2, int n2, int length)
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if ((arr1[i] != 0 || n1 != 0) && (arr2[i] != 0 || n2 != 0))
                    s = s + Math.Pow(arr1[i], n1) * Math.Pow(arr2[i], n2);
                else
                    s = s + 1;
            }
            return s;

        }
        private double[] ComputGauss(double[,] Guass, int n)
        {
            int i, j;
            int k, m;
            double temp;
            double max;
            double s;
            double[] x = new double[n];

            for (i = 0; i < n; i++) x[i] = 0.0;//初始化


            for (j = 0; j < n; j++)
            {
                max = 0;

                k = j;
                for (i = j; i < n; i++)
                {
                    if (Math.Abs(Guass[i, j]) > max)
                    {
                        max = Guass[i, j];
                        k = i;
                    }
                }



                if (k != j)
                {
                    for (m = j; m < n + 1; m++)
                    {
                        temp = Guass[j, m];
                        Guass[j, m] = Guass[k, m];
                        Guass[k, m] = temp;

                    }
                }

                if (0 == max)
                {
                    // "此线性方程为奇异线性方程" 

                    return x;
                }


                for (i = j + 1; i < n; i++)
                {

                    s = Guass[i, j];
                    for (m = j; m < n + 1; m++)
                    {
                        Guass[i, m] = Guass[i, m] - Guass[j, m] * s / (Guass[j, j]);

                    }
                }


            }//结束for (j=0;j<n;j++)


            for (i = n - 1; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    s = s + Guass[i, j] * x[j];
                }

                x[i] = (Guass[i, n] - s) / Guass[i, i];

            }

            return x;
        }//返回值是函数的系数


        /// <summary>
        /// 读取CSV
        /// </summary>
        /// <param name="filePath">CSV路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        /// <returns></returns>
        public System.Data.DataTable CsvToDataTable(string filePath, int n)
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
        #endregion

    }
}
