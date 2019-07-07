using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 恒温测试机.Model.Enum;

namespace 恒温测试机.App
{
    /// <summary>
    /// 测试数据分析
    /// </summary>
    public class DataReportAnalyseApp
    {
        private AnalyseData analyseData = new AnalyseData();
        private LogicTypeEnum logicType;
        private Dictionary<string, DataTable> analyseDataDic;
        private double DefaultTemp;

        public DataReportAnalyseApp(LogicTypeEnum logicType,Dictionary<string,DataTable> analyseDataDic)
        {
            this.logicType = logicType;
            this.analyseDataDic = analyseDataDic;
        }

        public string AnalyseResult()
        {
            switch (logicType)
            {
                case LogicTypeEnum.safeTest:
                    return AnalyseSafeTest();
                case LogicTypeEnum.PressureTest:
                    return AnalysePressureTest();
                case LogicTypeEnum.CoolTest:
                    return AnalyseCoolTest();
                case LogicTypeEnum.TemTest:
                    return AnalyseSteadyTest();
                case LogicTypeEnum.FlowTest:
                    return AnalyseFlowTest();
            }
            return "";
        }

        public string AnalyseSafeTest()
        {
            var result = "安全性测试报告：\n";
            result += "1、冷水失效\n";
            var qm5 =analyseData.QmIsLower(5,1.9, analyseDataDic["冷水失效数据"]);
            if (qm5 > 0)
            {
                result += "T5秒内出水流量降至1.9L/min以下，Qm5="+qm5+"\n";
            }
            var isLower = analyseData.TempIsLower(5,49, analyseDataDic["冷水失效数据"]);
            result+= isLower? "T5秒内出水温度应≤49℃：合格\n" : "T5秒内出水温度应≤49℃：不合格\n";

            var isBetween = analyseData.TmIsBetween(DefaultTemp, 2, analyseDataDic["冷水恢复数据"]);
            result += isBetween?"2、压力回复到初始压力后，开始收集数据，T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：合格\n":
                "2、压力回复到初始压力后，开始收集数据，T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：不合格\n";

            result += "3、热水失效\n";
            qm5 = analyseData.QmIsLower(5,1.9, analyseDataDic["热水失效数据"]);
            if (qm5 > 0)
            {
                result += "T5秒内出水流量降至1.9L/min以下，Qm5=" + qm5 + "\n";
            }
            isLower = analyseData.TempIsLower(5,49, analyseDataDic["热水失效数据"]);
            result += isLower ? "T5秒内出水温度应≤49℃：合格\n" : "T5秒内出水温度应≤49℃：不合格\n";

            isBetween = analyseData.TmIsBetween(DefaultTemp, 2, analyseDataDic["热水恢复数据"]);
            result += isBetween ? "4、压力回复到初始压力后，开始收集数据，T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：合格\n" :
                "4、压力回复到初始压力后，开始收集数据，T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：不合格\n";
            return result;
        }

        public string AnalysePressureTest()
        {
            var result = "压力变化测试报告：\n";
            result += "1、压力达到设定压力后，开始收集数据【热水降压】\n";
            var isLower = analyseData.TmOverFlagRegion(5, 3, 1.5, analyseDataDic["热水降压测试数据"]);
            result += isLower ? "T5秒内超过3℃的时间不大于T1.5秒：合格\n" : "T5秒内超过3℃的时间不大于T1.5秒：不合格\n";

            isLower = analyseData.TmBelowFlagRegion(5, 5, 1, analyseDataDic["热水降压测试数据"]);
            result += isLower ? "T5秒内低于5℃的时间不大于T1秒：合格\n" : "T5秒内低于5℃的时间不大于T1：不合格\n";

            isLower = analyseData.TmDeviationIsBetweenFlagRegion(5, DefaultTemp, 2, analyseDataDic["热水降压测试数据"]);
            result += isLower ? "T5秒后出水温度与所设定的温度偏差≤2℃：合格\n" : "T5秒后出水温度与所设定的温度偏差≤2℃：不合格\n";

            result += "2、压力回复到初始压力后，开始收集数据【热水降压恢复】\n";
            var isBetween = analyseData.TmIsBetween(DefaultTemp, 2, analyseDataDic["热水降压恢复数据"]);
            result += isBetween ? "T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：合格\n" :
                "T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：不合格\n";

            result += "3、压力达到设定压力后，开始收集数据【热水升压】\n";
            isLower = analyseData.TmOverFlagRegion(5, 3, 1.5, analyseDataDic["热水升压测试数据"]);
            result += isLower ? "T5秒内超过3℃的时间不大于T1.5秒：合格\n" : "T5秒内超过3℃的时间不大于T1.5秒：不合格\n";

            isLower = analyseData.TmBelowFlagRegion(5, 5, 1, analyseDataDic["热水升压测试数据"]);
            result += isLower ? "T5秒内低于5℃的时间不大于T1秒：合格\n" : "T5秒内低于5℃的时间不大于T1：不合格\n";

            isLower = analyseData.TmDeviationIsBetweenFlagRegion(5, DefaultTemp, 2, analyseDataDic["热水升压测试数据"]);
            result += isLower ? "T5秒后出水温度与所设定的温度偏差≤2℃：合格\n" : "T5秒后出水温度与所设定的温度偏差≤2℃：不合格\n";

            result += "4、压力回复到初始压力后，开始收集数据【热水升压恢复】\n";
            isBetween = analyseData.TmIsBetween(DefaultTemp, 2, analyseDataDic["热水升压恢复数据"]);
            result += isBetween ? "T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：合格\n" :
                "T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：不合格\n";

            result += "5、压力达到设定压力后，开始收集数据【冷水降压】\n";
            isLower = analyseData.TmOverFlagRegion(5, 3, 1.5, analyseDataDic["冷水降压测试数据"]);
            result += isLower ? "T5秒内超过3℃的时间不大于T1.5秒：合格\n" : "T5秒内超过3℃的时间不大于T1.5秒：不合格\n";

            isLower = analyseData.TmBelowFlagRegion(5, 5, 1, analyseDataDic["冷水降压测试数据"]);
            result += isLower ? "T5秒内低于5℃的时间不大于T1秒：合格\n" : "T5秒内低于5℃的时间不大于T1：不合格\n";

            isLower = analyseData.TmDeviationIsBetweenFlagRegion(5, DefaultTemp, 2, analyseDataDic["冷水降压测试数据"]);
            result += isLower ? "T5秒后出水温度与所设定的温度偏差≤2℃：合格\n" : "T5秒后出水温度与所设定的温度偏差≤2℃：不合格\n";

            result += "6、压力回复到初始压力后，开始收集数据【冷水降压恢复】\n";
            isBetween = analyseData.TmIsBetween(DefaultTemp, 2, analyseDataDic["冷水降压恢复数据"]);
            result += isBetween ? "T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：合格\n" :
                "T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：不合格\n";

            result += "7、压力达到设定压力后，开始收集数据【冷水升压】\n";
            isLower = analyseData.TmOverFlagRegion(5, 3, 1.5, analyseDataDic["冷水升压测试数据"]);
            result += isLower ? "T5秒内超过3℃的时间不大于T1.5秒：合格\n" : "T5秒内超过3℃的时间不大于T1.5秒：不合格\n";

            isLower = analyseData.TmBelowFlagRegion(5, 5, 1, analyseDataDic["冷水升压测试数据"]);
            result += isLower ? "T5秒内低于5℃的时间不大于T1秒：合格\n" : "T5秒内低于5℃的时间不大于T1：不合格\n";

            isLower = analyseData.TmDeviationIsBetweenFlagRegion(5, DefaultTemp, 2, analyseDataDic["冷水升压测试数据"]);
            result += isLower ? "T5秒后出水温度与所设定的温度偏差≤2℃：合格\n" : "T5秒后出水温度与所设定的温度偏差≤2℃：不合格\n";

            result += "8、压力回复到初始压力后，开始收集数据【冷水升压恢复】\n";
            isBetween = analyseData.TmIsBetween(DefaultTemp, 2, analyseDataDic["冷水升压恢复数据"]);
            result += isBetween ? "T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：合格\n" :
                "T5秒内混合水出水温度与所设定的温度偏差应≤±2℃：不合格\n";

            return result;
        }

        public string AnalyseCoolTest()
        {
            var result = "降温测试报告：\n";
            result += "1、温度达到设定温度后，开始收集数据\n";
            var isOver=analyseData.TmOverFlagRegion(5, 3, 1, analyseDataDic["降温测试数据"]);
            result += isOver ? "T5秒内超过3℃的时间不大于T1秒：合格\n" : "T5秒内超过3℃的时间不大于T1秒：不合格\n";

            var isBetween = analyseData.TmDeviationIsBetween(5, 1, analyseDataDic["降温测试数据"]);
            result += isBetween ? "T5秒后出水温度波动≤1℃：合格\n" : "T5秒后出水温度波动≤1℃：不合格\n";

            var isLower = analyseData.TmDeviationIsBetweenFlagRegion(5, DefaultTemp, 2, analyseDataDic["降温测试数据"]);
            result += isLower ? "T5秒后出水温度与所设定的温度偏差≤2℃：合格\n" : "T5秒后出水温度与所设定的温度偏差≤2℃：不合格\n";

            result += "2、温度回复到初始温度后，开始收集数据\n";
            isLower = analyseData.TmDeviationIsBetweenFlagRegion(40, DefaultTemp, 2, analyseDataDic["降温测试恢复数据"]);
            result += isLower ? "T40秒后出水温度与所设定的温度偏差≤2℃：合格\n" : "T40秒后出水温度与所设定的温度偏差≤2℃：不合格\n";

            return result;
        }
        public string AnalyseSteadyTest()
        {
            var result = "温度稳定性测试报告：\n";
            result += "";
            return result;
        }
        public string AnalyseFlowTest()
        {
            var result = "流量减少测试报告：\n";
            result += "";
            return result;
        }
    }

    /// <summary>
    /// 测试数据报告导出
    /// </summary>
    public class DataReportExportApp
    {
        private Dictionary<LogicTypeEnum, string> analyseReportDic;
        public DataReportExportApp(Dictionary<LogicTypeEnum, string> analyseReportDic)
        {
            this.analyseReportDic = analyseReportDic;
        }

        public bool Export(LogicTypeEnum logicType)
        {
            if (analyseReportDic.ContainsKey(logicType))
            {
                var result = analyseReportDic[logicType];
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
