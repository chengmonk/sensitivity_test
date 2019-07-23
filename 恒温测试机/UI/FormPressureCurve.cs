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
using 恒温测试机.Utils;

namespace 恒温测试机
{
    public partial class FormPressureCurve : Form
    {
        public FormPressureCurve()
        {
            InitializeComponent();
        }

        public FormPressureCurve(DataTable dt)
        {
            InitializeComponent();
            this.graphDt = dt;
        }

        private DataTable graphDt;

        private void FormPressureCurve_Load(object sender, EventArgs e)
        {
            load_Data();
        }
        private void load_Data()
        {

            new Thread(new ThreadStart(ThreadReadExample1)) { IsBackground = true }.Start();

        }

        private void ThreadReadExample1()
        {
            float[] Qc = new float[graphDt.Rows.Count];
            float[] Qh = new float[graphDt.Rows.Count];
            float[] Qm = new float[graphDt.Rows.Count];
            float[] Tc = new float[graphDt.Rows.Count];
            float[] Th = new float[graphDt.Rows.Count];
            float[] Tm = new float[graphDt.Rows.Count];
            float[] Pc = new float[graphDt.Rows.Count];
            float[] Ph = new float[graphDt.Rows.Count];
            float[] Pm = new float[graphDt.Rows.Count];
            float[] Qm5 = new float[graphDt.Rows.Count];
            float[] Wh = new float[graphDt.Rows.Count];
            DateTime[] dateTime = new DateTime[graphDt.Rows.Count];
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd hh:mm:ss:fff";
            //加载流量数据
            for (int i = 0; i < graphDt.Rows.Count; i++)
            {
                try
                {
                    Qc[i] = (float)Convert.ToDouble(graphDt.Rows[i][1]);
                    Qh[i] = (float)Convert.ToDouble(graphDt.Rows[i][2]);
                    Qm[i] = (float)Convert.ToDouble(graphDt.Rows[i][3]);
                    Tc[i] = (float)Convert.ToDouble(graphDt.Rows[i][4]);
                    Th[i] = (float)Convert.ToDouble(graphDt.Rows[i][5]);
                    Tm[i] = (float)Convert.ToDouble(graphDt.Rows[i][6]);
                    Pc[i] = (float)Convert.ToDouble(graphDt.Rows[i][7]);
                    Ph[i] = (float)Convert.ToDouble(graphDt.Rows[i][8]);
                    Pm[i] = (float)Convert.ToDouble(graphDt.Rows[i][9]);
                    Qm5[i] = (float)Convert.ToDouble(graphDt.Rows[i][10]);
                    Wh[i] = (float)Convert.ToDouble(graphDt.Rows[i][11]);
                    // dateTime[i] = Convert.ToDateTime(dt.Rows[i][0],dtFormat);

                    dateTime[i] = DateTime.ParseExact((string)graphDt.Rows[i][0], "yyyy-MM-dd hh:mm:ss:fff", dtFormat);
                }
                catch(Exception ex){
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
                    hslCurveHistory1.SetLeftCurve("出水流量Qm", Qm, Color.DodgerBlue, false, "{0:F2} L/min");//布尔变量：是否开启曲线平滑
                    hslCurveHistory1.SetLeftCurve("冷水流量Qc", Qc, Color.Tomato, true, "{0:F2} L/min");
                    hslCurveHistory1.SetLeftCurve("热水流量Qh", Qh, Color.GreenYellow, true, "{0:F2} L/min");
                    hslCurveHistory1.SetLeftCurve("Qm5", Qm5, Color.Purple, true, "{0:F2} L/min");
                    hslCurveHistory1.SetLeftCurve("出水压力Pm", Pm, Color.Red, true, "{0:F2} Bar");
                    hslCurveHistory1.SetLeftCurve("热水压力Ph", Ph, Color.Orange, true, "{0:F2} Bar");
                    hslCurveHistory1.SetLeftCurve("冷水压力Pc", Pc, Color.Yellow, true, "{0:F2} Bar");
                    hslCurveHistory1.SetRightCurve("出水温度Tm", Tm, Color.Green, true, "{0:F2} ℃");
                    hslCurveHistory1.SetRightCurve("热水温度Th", Th, Color.Honeydew, true, "{0:F2} ℃");
                    hslCurveHistory1.SetRightCurve("冷水温度Tc", Tc, Color.Pink, true, "{0:F2} ℃");
                    hslCurveHistory1.SetRightCurve("液面高度Wh", Tc, Color.Blue, true, "{0:F2} mm");

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
                    hslCurveHistory1.ValueMaxLeft = 10;
                    hslCurveHistory1.ValueMinLeft = 0;
                    hslCurveHistory1.SetScaleByXAxis(xAxis);
                    hslCurveHistory1.RenderCurveUI();

                }



            }
            ));
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
