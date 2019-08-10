using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 恒温测试机.Utils;

namespace 恒温测试机.Model.Enum
{
    /// <summary>
    /// 电机类型
    /// </summary>
    public enum ElectricalMachineryType
    {
        [EnumDescription("按键恒温电机")]
        tempType = 1,
        [EnumDescription("按键流量电机")]
        flowType = 2,
        [EnumDescription("旋转伺服电机")]
        spinType = 3,
        [EnumDescription("升降伺服电机")]
        upDownType = 4
    }
}
