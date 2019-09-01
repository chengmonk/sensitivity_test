using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 恒温测试机.Model.Enum;
using 恒温测试机.Utils;

namespace 恒温测试机
{
    public partial class FormPressureCurve : Form
    {
        public FormPressureCurve()
        {
            InitializeComponent();
        }

        public FormPressureCurve(DataTable dt, LogicTypeEnum logicType)
        {
            InitializeComponent();
            this.graphDt = dt;
            this.logicType = logicType;
        }

        private DataTable graphDt;
        private LogicTypeEnum logicType;

        private void FormPressureCurve_Load(object sender, EventArgs e)
        {
            if (logicType == LogicTypeEnum.SensitivityTest||logicType==LogicTypeEnum.FidelityTest)
            {
                load_Data_AngleTm();
            }
            else
            {
                load_Data_QmTm();
            }
        }
        #region 灵敏度曲线
        private void load_Data_AngleTm()
        {
            new Thread(new ThreadStart(ThreadRead_AngleTm)) { IsBackground = true }.Start();
        }

        private void ThreadRead_AngleTm()
        {
            float[] Tm = new float[graphDt.Rows.Count];
            float[] Angle = new float[graphDt.Rows.Count];
            DateTime[] dateTime = new DateTime[graphDt.Rows.Count];
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd hh:mm:ss:fff";
            //加载数据
            for (int i = 0; i < graphDt.Rows.Count; i++)
            {
                try
                {
                    Angle[i] = (float)Convert.ToDouble(graphDt.Rows[i][1]);
                    Tm[i] = (float)Convert.ToDouble(graphDt.Rows[i][2]);
                    dateTime[i] = DateTime.ParseExact((string)graphDt.Rows[i][0], "yyyy-MM-dd hh:mm:ss:fff", dtFormat);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    return;
                }
            }
            DateTime start = dateTime[0];
            DateTime PointA = dateTime[0].AddSeconds(2);
            TimeSpan ts = new TimeSpan(1);

            // 显示出数据信息来
            Invoke(new Action(() =>
            {
                //总流量达到6L加载数据
                if (false)
                {
                    hslCurveHistory1.Text = "累计流量未达到6L...";
                    hslCurveHistory1.RemoveAllCurve();
                }
                else
                {
                    hslCurveHistory1.Text = "正在加载数据...";
                    hslCurveHistory1.RemoveAllCurve();
                    hslCurveHistory1.SetLeftCurve("出水温度Tm", Tm, Color.Green, true, "{0:F2} ℃");//布尔变量：是否开启曲线平滑
                    hslCurveHistory1.SetLeftCurve("旋转角度Angle", Angle, Color.Red, true, "{0:F2} °");

                    hslCurveHistory1.SetDateTimes(dateTime);

                    // 增加一个三角形的线段标记示例 Points的每个点的X是数据索引，Y是数据值（需要选对参考坐标轴，默认为左坐标轴）                             



                    // 添加一个活动的标记
                    HslControls.HslMarkForeSection active = new HslControls.HslMarkForeSection()
                    {
                        StartIndex = 1000,
                        EndIndex = 1500,
                        Height = 0.9f,
                    };
                    //active.CursorTexts.Add("条码", "A123123124ashdiahsd是的iahsidasd");
                    //active.CursorTexts.Add("工号", "asd2sd123dasf");
                    //hslCurveHistory1.AddMarkActiveSection(active);

                    //hslCurveHistory1.SetCurveVisible("步序", false);   // 步序不是曲线信息，不用显示出来
                    //hslCurveHistory1.ValueMaxLeft = 10;
                    //hslCurveHistory1.ValueMinLeft = 0;

                    //SetTempLight(28);
                    //SetTempLight(29);
                    //SetTempLight(30);
                    //SetRemark(29f,28f,Tm);
                    hslCurveHistory1.SetScaleByXAxis(xAxis);
                    hslCurveHistory1.RenderCurveUI();

                }



            }
            ));
        }
        #endregion

        #region 温度稳定性曲线
        private void load_Data_QmTm()
        {
            new Thread(new ThreadStart(ThreadRead_QmTm)) { IsBackground = true }.Start();
        }

        private void ThreadRead_QmTm()
        {
            float[] Tm = new float[graphDt.Rows.Count];
            float[] Qm = new float[graphDt.Rows.Count];
            DateTime[] dateTime = new DateTime[graphDt.Rows.Count];
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd hh:mm:ss:fff";
            //加载数据
            for (int i = 0; i < graphDt.Rows.Count; i++)
            {
                try
                {
                    Qm[i] = (float)Convert.ToDouble(graphDt.Rows[i][1]);
                    Tm[i] = (float)Convert.ToDouble(graphDt.Rows[i][2]);
                    dateTime[i] = DateTime.ParseExact((string)graphDt.Rows[i][0], "yyyy-MM-dd hh:mm:ss:fff", dtFormat);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    return;
                }
            }
            DateTime start = dateTime[0];
            DateTime PointA = dateTime[0].AddSeconds(2);
            TimeSpan ts = new TimeSpan(1);

            // 显示出数据信息来
            Invoke(new Action(() =>
            {
                //总流量达到6L加载数据
                if (false)
                {
                    hslCurveHistory1.Text = "累计流量未达到6L...";
                    hslCurveHistory1.RemoveAllCurve();
                }
                else
                {
                    hslCurveHistory1.Text = "正在加载数据...";
                    hslCurveHistory1.RemoveAllCurve();
                    hslCurveHistory1.SetLeftCurve("出水温度Tm", Tm, Color.Green, true, "{0:F2} ℃");//布尔变量：是否开启曲线平滑
                    hslCurveHistory1.SetLeftCurve("出水流量Qm", Qm, Color.Red, true, "{0:F2} L/Min");

                    hslCurveHistory1.SetDateTimes(dateTime);

                    // 增加一个三角形的线段标记示例 Points的每个点的X是数据索引，Y是数据值（需要选对参考坐标轴，默认为左坐标轴）                             



                    // 添加一个活动的标记
                    HslControls.HslMarkForeSection active = new HslControls.HslMarkForeSection()
                    {
                        StartIndex = 1000,
                        EndIndex = 1500,
                        Height = 0.9f,
                    };
                    //active.CursorTexts.Add("条码", "A123123124ashdiahsd是的iahsidasd");
                    //active.CursorTexts.Add("工号", "asd2sd123dasf");
                    //hslCurveHistory1.AddMarkActiveSection(active);

                    //hslCurveHistory1.SetCurveVisible("步序", false);   // 步序不是曲线信息，不用显示出来
                    //hslCurveHistory1.ValueMaxLeft = 10;
                    //hslCurveHistory1.ValueMinLeft = 0;

                    //SetTempLight(28);
                    //SetTempLight(29);
                    //SetTempLight(30);
                    //SetRemark(29f,28f,Tm);
                    hslCurveHistory1.SetScaleByXAxis(xAxis);
                    hslCurveHistory1.RenderCurveUI();

                }



            }
            ));
        }
        #endregion

        /// <summary>
        /// 区间标注（超过或低于设定的温度）
        /// </summary>
        /// <param name="tempMax">设定的温度Max</param>
        /// <param name="tempMin">设定的温度Min</param>
        /// <param name="Tm">出水温度数组</param>
        private void SetRemark(float tempMax,float tempMin,float[] Tm)
        {
            bool isHigh = false;
            int highStartIndex = 0;
            int highEndIndex = 0;

            bool islow = false;
            int belowStartIndex = 0;
            int belowEndIndex = 0;

            for(int i = 0; i < Tm.Length; i++)
            {
                if ((isHigh == false) && (Tm[i] > tempMax))
                {
                    isHigh = true;
                    highStartIndex = i;
                }
                if(isHigh&&(Tm[i] <= tempMax))
                {
                    isHigh = false;
                    highEndIndex = i;
                    int totalTimes = (highEndIndex - highStartIndex) * 10;
                    HslControls.HslMarkForeSection foreSection = new HslControls.HslMarkForeSection()       //区间表示操作
                    {
                        StartIndex = highStartIndex,
                        EndIndex = highEndIndex,
                        StartHeight = 0.3f,         // 如果值是(0-1)的话，表示的是位置百分比，0.9就是曲线高度为90%，从上往下看的视角，如果填了600，那就是绝对坐标
                        Height = 0.4f,              // 和上面同理
                        LinePen = Pens.Chocolate,   // 指定颜色
                        IsRenderTimeText = false,   // 是否显示额外的起始时间和结束时间，此处就不要了
                        MarkText = "温度超标了:"+ totalTimes+"ms",
                    };
                    hslCurveHistory1.AddMarkForeSection(foreSection);
                }

                if ((islow == false) && (Tm[i] < tempMin))
                {
                    islow = true;
                    belowStartIndex = i;
                }
                if (islow && (Tm[i] >= tempMin))
                {
                    islow = false;
                    belowEndIndex = i;
                    int totalTimes = (belowEndIndex - belowStartIndex) * 10;
                    HslControls.HslMarkForeSection foreSection = new HslControls.HslMarkForeSection()       //区间表示操作
                    {
                        StartIndex = belowStartIndex,
                        EndIndex = belowEndIndex,
                        StartHeight = 0.3f,         // 如果值是(0-1)的话，表示的是位置百分比，0.9就是曲线高度为90%，从上往下看的视角，如果填了600，那就是绝对坐标
                        Height = 0.9f,              // 和上面同理
                        LinePen = Pens.Chocolate,   // 指定颜色
                        IsRenderTimeText = false,   // 是否显示额外的起始时间和结束时间，此处就不要了
                        MarkText = "温度过低了:" + totalTimes + "ms",
                    };
                    hslCurveHistory1.AddMarkForeSection(foreSection);
                }
            }
            
        }

        /// <summary>
        /// 画虚线
        /// </summary>
        private void SetTempLight(float temp)
        {
            hslCurveHistory1.AddLeftAuxiliary(temp);
        }

        private void HslButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "图片|*.png";
            fileDialog.InitialDirectory = Application.StartupPath;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                hslCurveHistory1.SaveToBitmap().Save(fileDialog.FileName);
                MessageBox.Show("保存成功!");
            }
            fileDialog.Dispose();
        }
        float xAxis = 7;
        private void HslButton4_Click(object sender, EventArgs e)
        {
            hslCurveHistory1.SetScaleByXAxis(--xAxis > 0 ? xAxis : (xAxis = 1));
            hslCurveHistory1.RenderCurveUI();
        }

        private void HslButton3_Click(object sender, EventArgs e)
        {
            hslCurveHistory1.SetScaleByXAxis(++xAxis > 0 ? xAxis : (xAxis = 1));
            hslCurveHistory1.RenderCurveUI();
        }
    }
}
