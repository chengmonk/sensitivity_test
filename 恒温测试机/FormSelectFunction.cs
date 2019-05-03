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
    public partial class FormSelectFunction : Form
    {
        public FormSelectFunction()
        {
            InitializeComponent();
        }

        private void HslButton1_Click(object sender, EventArgs e)
        {
            Hide();
            {
                using (FormSafeTest f = new FormSafeTest())
                {
                    f.ShowDialog();
                }
            }
            System.Threading.Thread.Sleep(20);
            Show();
        }
    }
}
