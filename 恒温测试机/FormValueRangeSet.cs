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
    }
}
