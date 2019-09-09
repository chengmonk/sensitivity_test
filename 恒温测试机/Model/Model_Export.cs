using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 恒温测试机.Model
{
    public class Model_Export
    {
        #region 稳定性65
        public double Tm_65_3 { get; set; }
        public double Tm_65_3diff { get; set; }
        public double Tm_65_6 { get; set; }
        public double Tm_65_6diff { get; set; }
        #endregion

        #region 稳定性50
        public double Tm_50_3 { get; set; }
        public double Tm_50_3diff { get; set; }
        public double Tm_50_6 { get; set; }
        public double Tm_50_6diff { get; set; }
        #endregion

        #region 保真度测试
        public double tmDiff { get; set; }
        #endregion

        #region 灵敏度测试
        public double G1 { get; set; }
        public double G2 { get; set; }
        #endregion
    }
}
