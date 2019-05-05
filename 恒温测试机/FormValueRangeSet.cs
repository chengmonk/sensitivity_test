using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 恒温测试机
{
    public partial class FormValueRangeSet : Form
    {
        public FormValueRangeSet()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void HslButton1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CoolPump.Value = Properties.Settings.Default.CoolPump011;
            HotPump.Value = Properties.Settings.Default.HotPump021;

            PumpCoolHighPressureSet.Value = Properties.Settings.Default.PumpCoolHigh012;
            PumpCoolLowPressureSet.Value = Properties.Settings.Default.PumpCoolLow012;
            PumpHotHighPressureSet.Value = Properties.Settings.Default.PumpHotHigh022;
            PumpHotLowPressureSet022.Value = Properties.Settings.Default.PumpHotLow022;
            pressureThreshold.Value = Properties.Settings.Default.pressureThreshold;

            t1Set.Value = Properties.Settings.Default.t1;
            t2Set.Value = Properties.Settings.Default.t2;
            t3Set.Value = Properties.Settings.Default.t3;

            QmMax.Value = Properties.Settings.Default.QmMax;
            QcMax.Value = Properties.Settings.Default.QcMax;
            QhMax.Value = Properties.Settings.Default.QhMax;

            TmMax.Value = Properties.Settings.Default.TmMax;
            TcMax.Value = Properties.Settings.Default.TcMax;
            ThMax.Value = Properties.Settings.Default.ThMax;

            PmMax.Value = Properties.Settings.Default.PmMax;
            PcMax.Value = Properties.Settings.Default.PcMax;
            PhMax.Value = Properties.Settings.Default.PhMax;

            QmMin.Value = Properties.Settings.Default.QmMin;
            QcMin.Value = Properties.Settings.Default.QcMin;
            QhMin.Value = Properties.Settings.Default.QhMin;

            TmMin.Value = Properties.Settings.Default.TmMin;
            TcMin.Value = Properties.Settings.Default.TcMin;
            ThMin.Value = Properties.Settings.Default.ThMin;

            PmMin.Value = Properties.Settings.Default.PmMin;
            PcMin.Value = Properties.Settings.Default.PcMin;
            PhMin.Value = Properties.Settings.Default.PhMin;

            Temp1Set.Value = Properties.Settings.Default.Temp1Set;
            Temp2Set.Value = Properties.Settings.Default.Temp2Set;
            Temp3Set.Value = Properties.Settings.Default.Temp3Set;
            Temp4Set.Value = Properties.Settings.Default.Temp4Set;
            Temp5Set.Value = Properties.Settings.Default.Temp5Set;

            Temp1Range.Value = Properties.Settings.Default.Temp1Range;
            Temp2Range.Value = Properties.Settings.Default.Temp2Range;
            Temp3Range.Value = Properties.Settings.Default.Temp3Range;
            Temp4Range.Value = Properties.Settings.Default.Temp4Range;
            Temp5Range.Value = Properties.Settings.Default.Temp5Range;

        }

        private void QmMax_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.QmMax = QmMax.Value;
            Properties.Settings.Default.Save();
        }

        private void QmMin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.QmMin = QmMin.Value;
            Properties.Settings.Default.Save();
        }

        private void QcMax_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.QcMax = QcMax.Value;
            Properties.Settings.Default.Save();
        }

        private void QhMax_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.QhMax = QhMax.Value;
            Properties.Settings.Default.Save();
        }

        private void QcMin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.QcMin = QcMin.Value;
            Properties.Settings.Default.Save();
        }

        private void QhMin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.QcMin = QcMin.Value;
            Properties.Settings.Default.Save();
        }

        private void PmMax_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PmMax = PmMax.Value;
            Properties.Settings.Default.Save();
        }

        private void PcMax_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PcMax = PcMax.Value;
            Properties.Settings.Default.Save();
        }

        private void PhMax_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PhMax = PhMax.Value;
            Properties.Settings.Default.Save();
        }

        private void TmMax_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.TmMax = TmMax.Value;
            Properties.Settings.Default.Save();
        }

        private void TcMax_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.TcMax = TcMax.Value;
            Properties.Settings.Default.Save();
        }

        private void ThMax_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ThMax = ThMax.Value;
            Properties.Settings.Default.Save();
        }

        private void PmMin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PmMin = PmMin.Value;
            Properties.Settings.Default.Save();
        }

        private void PcMin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PcMin = PcMin.Value;
            Properties.Settings.Default.Save();
        }

        private void PhMin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PhMin = PhMin.Value;
            Properties.Settings.Default.Save();
        }

        private void TmMin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.TmMin = TmMin.Value;
            Properties.Settings.Default.Save();
        }

        private void TcMin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.TcMin = TcMin.Value;
            Properties.Settings.Default.Save();
        }

        private void ThMin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ThMin = ThMin.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp1Set_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp1Set = Temp1Set.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp2Set_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp2Set = Temp2Set.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp3Set_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp2Set = Temp3Set.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp4Set_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp4Set = Temp4Set.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp5Set_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp5Set = Temp5Set.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp1Range_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp1Range = Temp1Range.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp2Range_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp2Range = Temp2Range.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp3Range_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp3Range = Temp3Range.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp4Range_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp4Range = Temp4Range.Value;
            Properties.Settings.Default.Save();
        }

        private void Temp5Range_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Temp5Range = Temp5Range.Value;
            Properties.Settings.Default.Save();
        }

        private void T1Set_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.t1 = t1Set.Value;
            Properties.Settings.Default.Save();
        }

        private void T2Set_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.t2 = t2Set.Value;
            Properties.Settings.Default.Save();
        }

        private void T3Set_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.t3 = t3Set.Value;
            Properties.Settings.Default.Save();
        }

        private void PumpHotLowPressureSet_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PumpHotLow022 = PumpHotLowPressureSet022.Value;
            Properties.Settings.Default.Save();
        }

        private void PumpHotHighPressureSet_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PumpHotHigh022 = PumpHotHighPressureSet.Value;
            Properties.Settings.Default.Save();
        }

        private void PumpCoolLowPressureSet_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PumpCoolLow012 = PumpCoolLowPressureSet.Value;
            Properties.Settings.Default.Save();
        }

        private void PumpCoolHighPressureSet_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PumpCoolHigh012 = PumpCoolHighPressureSet.Value;
            Properties.Settings.Default.Save();
        }

        private void CoolPump_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CoolPump011 = CoolPump.Value;
            Properties.Settings.Default.Save();
        }

        private void HotPump_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.HotPump021 = HotPump.Value;
            Properties.Settings.Default.Save();
        }

        private void PressureThreshold_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.pressureThreshold = pressureThreshold.Value;
            Properties.Settings.Default.Save();
        }
    }
}
