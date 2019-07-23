using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 恒温测试机.Utils
{
    /// <summary>
    /// 界面自适应
    /// </summary>
    class AutoSizeFormClass
    {
        public struct controlRect
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }

        private bool _Flag;
        public bool Flag
        {
            get { return _Flag; }
            set { _Flag = value; }
        }

        private Dictionary<int, controlRect> oldCtrl;

        public void Initialize(Form mForm)
        {
            oldCtrl = new Dictionary<int, controlRect>();
            controlRect cR;

            cR.Left = mForm.Left;
            cR.Top = mForm.Top;
            cR.Width = mForm.Width;
            cR.Height = mForm.Height;
            oldCtrl.Add(0, cR);
            AddControl(mForm);
            Flag = true;
        }

        private void AddControl(Control ctl)
        {

            foreach (Control c in ctl.Controls)
            {
                controlRect objCtrl;
                objCtrl.Left = c.Left;
                objCtrl.Top = c.Top;
                objCtrl.Width = c.Width;
                objCtrl.Height = c.Height;
                oldCtrl.Add(c.GetHashCode(), objCtrl);
                if (c.Controls.Count > 0)
                {
                    AddControl(c);
                }
            }

        }

        public void ReSize(Form mForm)
        {
            if (!Flag) return;

            float wScale = (float)mForm.Width / (float)oldCtrl[0].Width;
            float hScale = (float)mForm.Height / (float)oldCtrl[0].Height;

            try
            {
                ResizeControl(mForm, wScale, hScale);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return;
            }
        }

        private void ResizeControl(Control ctl, float wScale, float hScale)
        {
            int ctrLeft0, ctrTop0, ctrWidth0, ctrHeight0;
            foreach (Control c in ctl.Controls)
            {
                ctrLeft0 = oldCtrl[c.GetHashCode()].Left;
                ctrTop0 = oldCtrl[c.GetHashCode()].Top;
                ctrWidth0 = oldCtrl[c.GetHashCode()].Width;
                ctrHeight0 = oldCtrl[c.GetHashCode()].Height;

                c.Left = (int)(ctrLeft0 * wScale);
                c.Top = (int)(ctrTop0 * hScale);
                c.Width = (int)(ctrWidth0 * wScale);
                c.Height = (int)(ctrHeight0 * hScale);
                if (c.Controls.Count > 0)
                {
                    ResizeControl(c, wScale, hScale);
                }
            }
        }
    }
}
