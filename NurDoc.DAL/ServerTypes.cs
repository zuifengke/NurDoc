// ***********************************************************
// 数据库访问层涉及到的类型定义集合.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Heren.Common.Libraries;
using Heren.Common.Libraries.Ftp;

namespace Heren.NurDoc.DAL
{
    #region"enum"
    /// <summary>
    /// 文档存储模式
    /// </summary>
    public enum StorageMode
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// 数据库存储
        /// </summary>
        DB = 0,

        /// <summary>
        /// FTP存储
        /// </summary>
        FTP = 1
    }

    /// <summary>
    /// 系统连接模式
    /// </summary>
    public enum ConnectionMode
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// 两层直连数据库模式
        /// </summary>
        DB = 0,

        /// <summary>
        /// 三层连接RestFul服务模式
        /// </summary>
        Rest = 1,
    }

    /// <summary>
    /// 用户权限类型
    /// </summary>
    public enum UserRightType
    {
        /// <summary>
        /// 护理病历用户权限
        /// </summary>
        NurDoc
    }

    /// <summary>
    /// 获取或设置表单打印模式
    /// </summary>
    public enum FormPrintMode
    {
        /// <summary>
        /// 都不打印
        /// </summary>
        None = 0,

        /// <summary>
        /// 仅打印表单
        /// </summary>
        Form = 1,

        /// <summary>
        /// 仅打印已写列表
        /// </summary>
        List = 2,

        /// <summary>
        /// 表单和列表都打印
        /// </summary>
        FormAndList = 3
    }

    public enum SortMode
    {
        /// <summary>
        /// 不排序
        /// </summary>
        None = 0,

        /// <summary>
        /// 升序排列
        /// </summary>
        Ascending = 1,

        /// <summary>
        /// 降序排列
        /// </summary>
        Descending = 2
    }
    #endregion

    #region"class"
    /// <summary>
    /// 数据库访问程序中各对象的基类
    /// </summary>
    [System.Xml.Serialization.XmlInclude(typeof(NurDocInfo))]
    public class DbTypeBase : Object, ICloneable
    {
        /// <summary>
        /// 获取缺省时间
        /// </summary>
        public DateTime DefaultTime
        {
            set { }
            get { return DateTime.Parse("1900-1-1"); }
        }

        public virtual object Clone()
        {
            object instance = Activator.CreateInstance(this.GetType());
            GlobalMethods.Reflect.CopyProperties(this, instance);
            return instance;
        }

        public DataTable ToDataTable()
        {
            return GlobalMethods.Table.GetDataTable(this);
        }

        public bool FromDataRow(DataRow row)
        {
            return GlobalMethods.Table.DataRowToObject(row, this);
        }
    }

    /// <summary>
    /// 系统配置信息类
    /// </summary>
    public class ConfigInfo : DbTypeBase, ICloneable
    {
        private string m_szGroupName = string.Empty;

        /// <summary>
        /// 获取或设置配置组的名称
        /// </summary>
        public string GroupName
        {
            get { return this.m_szGroupName; }
            set { this.m_szGroupName = value; }
        }

        private string m_szConfigName = string.Empty;

        /// <summary>
        /// 获取或设置配置项的名称
        /// </summary>
        public string ConfigName
        {
            get { return this.m_szConfigName; }
            set { this.m_szConfigName = value; }
        }

        private string m_szConfigValue = string.Empty;

        /// <summary>
        /// 获取或设置配置项的值
        /// </summary>
        public string ConfigValue
        {
            get { return this.m_szConfigValue; }
            set { this.m_szConfigValue = value; }
        }

        private string m_szConfigDesc = string.Empty;

        /// <summary>
        /// 获取或设置配置项的描述
        /// </summary>
        public string ConfigDesc
        {
            get { return this.m_szConfigDesc; }
            set { this.m_szConfigDesc = value; }
        }

        public override object Clone()
        {
            ConfigInfo configInfo = new ConfigInfo();
            configInfo.GroupName = this.m_szGroupName;
            configInfo.ConfigName = this.m_szConfigName;
            configInfo.ConfigValue = this.m_szConfigValue;
            configInfo.ConfigDesc = this.m_szConfigDesc;
            return configInfo;
        }

        public override string ToString()
        {
            return string.Format("GroupName={0},ConfigName={1}"
                , this.m_szGroupName, this.m_szConfigName);
        }
    }

    /// <summary>
    /// 系统配置信息类
    /// </summary>
    public class ChildDocInfo : DbTypeBase, ICloneable
    {
        private string m_szDocID = string.Empty;

        /// <summary>
        /// 获取或设置父文档标识
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
            set { this.m_szDocID = value; }
        }

        private string m_szCaller = string.Empty;

        /// <summary>
        /// 获取或设置调用者标识
        /// </summary>
        public string Caller
        {
            get { return this.m_szCaller; }
            set { this.m_szCaller = value; }
        }

        private string m_szChildDocID = string.Empty;

        /// <summary>
        /// 获取或设置子文档标识
        /// </summary>
        public string ChildDocID
        {
            get { return this.m_szChildDocID; }
            set { this.m_szChildDocID = value; }
        }

        public override object Clone()
        {
            ChildDocInfo childDocInfo = new ChildDocInfo();
            childDocInfo.DocID = this.m_szDocID;
            childDocInfo.Caller = this.m_szCaller;
            childDocInfo.m_szChildDocID = this.m_szChildDocID;
            return childDocInfo;
        }
    }

    /// <summary>
    /// 窗口名称配置信息类
    /// </summary>
    public class WindowName : DbTypeBase, ICloneable
    {
        private string m_szBatchRecord = "体征批量录入";

        /// <summary>
        /// 获取或设置体征批量录入窗口标题
        /// </summary>
        public string BatchRecord
        {
            get { return this.m_szBatchRecord; }
            set { this.m_szBatchRecord = value; }
        }

        private string m_szInfoLib = "护理信息库";

        /// <summary>
        /// 获取或设置护理信息库
        /// </summary>
        public string InfoLib
        {
            get { return this.m_szInfoLib; }
            set { this.m_szInfoLib = value; }
        }

        private string m_szBedView = "床卡一览表";

        /// <summary>
        /// 获取或设置病人一栏床位窗口标题
        /// </summary>
        public string BedView
        {
            get { return this.m_szBedView; }
            set { this.m_szBedView = value; }
        }

        private string m_szRosteringView = "排班一览表";

        /// <summary>
        /// 获取或设置护理排班一览窗口标题
        /// </summary>
        public string RosteringView
        {
            get { return this.m_szRosteringView; }
            set { this.m_szRosteringView = value; }
        }

        private string m_szWorkShiftRecord = "交接班记录";

        /// <summary>
        /// 获取或设置交接班窗口标题
        /// </summary>
        public string WorkShiftRecord
        {
            get { return this.m_szWorkShiftRecord; }
            set { this.m_szWorkShiftRecord = value; }
        }

        private string m_szNursingQC = "护理质控";

        /// <summary>
        /// 获取或设置护理质控窗口标题
        /// </summary>
        public string NursingQC
        {
            get { return this.m_szNursingQC; }
            set { this.m_szNursingQC = value; }
        }

        private string m_szNursingMonitor = "监护记录单";

        /// <summary>
        /// 获取或设置监护记录单标题
        /// </summary>
        public string NursingMonitor
        {
            get { return this.m_szNursingMonitor; }
            set { this.m_szNursingMonitor = value; }
        }

        private string m_szVitalSignsGraph = "体征三测单";

        /// <summary>
        /// 获取或设置体温单窗口标题
        /// </summary>
        public string VitalSignsGraph
        {
            get { return this.m_szVitalSignsGraph; }
            set { this.m_szVitalSignsGraph = value; }
        }

        private string m_szWordDocument = "文档模板";

        /// <summary>
        /// 获取或设置Word文档标题
        /// </summary>
        public string WordDocument
        {
            get { return this.m_szWordDocument; }
            set { this.m_szWordDocument = value; }
        }

        private string m_szOrdersRecord = "医嘱记录单";

        /// <summary>
        /// 获取或设置医嘱单窗口标题
        /// </summary>
        public string OrdersRecord
        {
            get { return this.m_szOrdersRecord; }
            set { this.m_szOrdersRecord = value; }
        }

        private string m_szNursingRecord = "护理记录单";

        /// <summary>
        /// 获取或设置护理记录窗口标题
        /// </summary>
        public string NursingRecord
        {
            get { return this.m_szNursingRecord; }
            set { this.m_szNursingRecord = value; }
        }

        private string m_szDocumentList1 = "文书列表1";

        /// <summary>
        /// 获取或设置文书列表1窗口标题
        /// </summary>
        public string DocumentList1
        {
            get { return this.m_szDocumentList1; }
            set { this.m_szDocumentList1 = value; }
        }

        private string m_szDocumentList2 = "文书列表2";

        /// <summary>
        /// 获取或设置文书列表2窗口标题
        /// </summary>
        public string DocumentList2
        {
            get { return this.m_szDocumentList2; }
            set { this.m_szDocumentList2 = value; }
        }

        private string m_szDocumentList3 = "文书列表3";

        /// <summary>
        /// 获取或设置文书列表3窗口标题
        /// </summary>
        public string DocumentList3
        {
            get { return this.m_szDocumentList3; }
            set { this.m_szDocumentList3 = value; }
        }

        private string m_szDocumentList4 = "文书列表4";

        /// <summary>
        /// 获取或设置文书列表4窗口标题
        /// </summary>
        public string DocumentList4
        {
            get { return this.m_szDocumentList4; }
            set { this.m_szDocumentList4 = value; }
        }

        private string m_szNursingAssessMent = "护理评价";

        /// <summary>
        /// 获取或设置护理评估窗口标题
        /// </summary>
        public string NursingAssessMent
        {
            get { return this.m_szNursingAssessMent; }
            set { this.m_szNursingAssessMent = value; }
        }

        private string m_szSpecialNursing = "专科护理";

        /// <summary>
        /// 获取或设置专科护理窗口标题
        /// </summary>
        public string SpecialNursing
        {
            get { return this.m_szSpecialNursing; }
            set { this.m_szSpecialNursing = value; }
        }

        private string m_szSpecialPatient = "专科一览表";

        /// <summary>
        /// 获取或设置专科一览表窗口标题
        /// </summary>
        public string SpecialPatient
        {
            get { return this.m_szSpecialPatient; }
            set { this.m_szSpecialPatient = value; }
        }

        private string m_szNursingConsult = "护理会诊";

        /// <summary>
        /// 获取或设置护理会诊窗口标题
        /// </summary>
        public string NursingConsult
        {
            get { return this.m_szNursingConsult; }
            set { this.m_szNursingConsult = value; }
        }

        private string m_szNurCarePlan = "护理计划";

        /// <summary>
        /// 获取或设置护理计划窗口标题
        /// </summary>
        public string NurCarePlan
        {
            get { return this.m_szNurCarePlan; }
            set { this.m_szNurCarePlan = value; }
        }

        private string m_szNursingStat = "查询统计";

        /// <summary>
        /// 获取或设置查询统计窗口标题
        /// </summary>
        public string NursingStat
        {
            get { return this.m_szNursingStat; }
            set { this.m_szNursingStat = value; }
        }

        private string m_szNursingTask = "待办任务";

        /// <summary>
        /// 获取或设置待办任务窗口标题
        /// </summary>
        public string NursingTask
        {
            get { return this.m_szNursingTask; }
            set { this.m_szNursingTask = value; }
        }

        private string m_szIntegrateQuery = "综合查询";

        /// <summary>
        /// 获取或设置综合查询窗口标题
        /// </summary>
        public string IntegrateQuery
        {
            get { return this.m_szIntegrateQuery; }
            set { this.m_szIntegrateQuery = value; }
        }

        private string m_szQualitySafty = "质量与安全管理记录";

        /// <summary>
        /// 获取或设置质量与安全管理记录窗口标题
        /// </summary>
        public string QualitySafty
        {
            get { return this.m_szQualitySafty; }
            set { this.m_szQualitySafty = value; }
        }

        public string GetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            int index = name.IndexOf(';');
            if (index <= 0)
                return name;
            return name.Substring(0, index);
        }

        public int GetIndex(string name, int defaultIndex)
        {
            if (string.IsNullOrEmpty(name))
                return defaultIndex;
            int index = name.IndexOf(';');
            if (index <= 0)
                return defaultIndex;
            string value = name.Substring(index + 1);
            return GlobalMethods.Convert.StringToValue(value, defaultIndex);
        }
    }

    /// <summary>
    /// 权限配置信息类
    /// </summary>
    public class RightConfig : DbTypeBase, ICloneable
    {
        private bool m_bApprovelDataModify = false;

        /// <summary>
        /// 获取或设置是否开启数据修改审批功能
        /// </summary>
        public bool ApprovelDataModify
        {
            get { return this.m_bApprovelDataModify; }
            set { this.m_bApprovelDataModify = value; }
        }

        private int m_nStudentNurseEditTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int StudentNurseEditTime
        {
            get { return this.m_nStudentNurseEditTime; }
            set { this.m_nStudentNurseEditTime = value; }
        }

        private int m_nGeneralNurseEditTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int GeneralNurseEditTime
        {
            get { return this.m_nGeneralNurseEditTime; }
            set { this.m_nGeneralNurseEditTime = value; }
        }

        private int m_nQualityNurseEditTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int QualityNurseEditTime
        {
            get { return this.m_nQualityNurseEditTime; }
            set { this.m_nQualityNurseEditTime = value; }
        }

        private int m_nHeadNurseEditTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int HeadNurseEditTime
        {
            get { return this.m_nHeadNurseEditTime; }
            set { this.m_nHeadNurseEditTime = value; }
        }

        private int m_nHigherNurseEditTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int HigherNurseEditTime
        {
            get { return this.m_nHigherNurseEditTime; }
            set { this.m_nHigherNurseEditTime = value; }
        }

        private int m_nLeaderNurseEditTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int LeaderNurseEditTime
        {
            get { return this.m_nLeaderNurseEditTime; }
            set { this.m_nLeaderNurseEditTime = value; }
        }

        private int m_nStudentNurseGrantTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int StudentNurseGrantTime
        {
            get { return this.m_nStudentNurseGrantTime; }
            set { this.m_nStudentNurseGrantTime = value; }
        }

        private int m_nGeneralNurseGrantTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int GeneralNurseGrantTime
        {
            get { return this.m_nGeneralNurseGrantTime; }
            set { this.m_nGeneralNurseGrantTime = value; }
        }

        private int m_nQualityNurseGrantTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int QualityNurseGrantTime
        {
            get { return this.m_nQualityNurseGrantTime; }
            set { this.m_nQualityNurseGrantTime = value; }
        }

        private int m_nHigherNurseGrantTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int HigherNurseGrantTime
        {
            get { return this.m_nHigherNurseGrantTime; }
            set { this.m_nHigherNurseGrantTime = value; }
        }

        private int m_nHeadNurseGrantTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int HeadNurseGrantTime
        {
            get { return this.m_nHeadNurseGrantTime; }
            set { this.m_nHeadNurseGrantTime = value; }
        }

        private int m_nLeaderNurseGrantTime = 0;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int LeaderNurseGrantTime
        {
            get { return this.m_nLeaderNurseGrantTime; }
            set { this.m_nLeaderNurseGrantTime = value; }
        }
    }

    /// <summary>
    /// 系统配置信息类
    /// </summary>
    public class SystemOption : DbTypeBase, ICloneable
    {
        private string m_szCertCode = string.Empty;

        /// <summary>
        /// 获取或设置产品授权代码
        /// </summary>
        public string CertCode
        {
            get { return this.m_szCertCode; }
            set { this.m_szCertCode = value; }
        }

        private string m_szHospitalName = string.Empty;

        /// <summary>
        /// 获取或设置产品授权医院的名称
        /// </summary>
        public string HospitalName
        {
            get { return this.m_szHospitalName; }
            set { this.m_szHospitalName = value; }
        }

        private string m_szUpgradeVersion = string.Empty;

        /// <summary>
        /// 获取或设置产品自动升级版本号
        /// </summary>
        public string UpgradeVersion
        {
            get { return this.m_szUpgradeVersion; }
            set { this.m_szUpgradeVersion = value; }
        }

        private bool m_bSinglePatientMode = false;

        /// <summary>
        /// 获取或设置是否启用单病人窗口编辑模式
        /// </summary>
        public bool SinglePatientMode
        {
            get { return this.m_bSinglePatientMode; }
            set { this.m_bSinglePatientMode = value; }
        }

        private bool m_bEmrsVital = false;

        /// <summary>
        /// 总后标准版体征数据
        /// </summary>
        public bool EmrsVital
        {
            get { return this.m_bEmrsVital; }
            set { this.m_bEmrsVital = value; }
        }

        private bool m_bNursingRecordDoubleSign = false;

        /// <summary>
        /// 获取或设置护理记录是否需要双签名
        /// </summary>
        public bool NursingRecordDoubleSign
        {
            get { return this.m_bNursingRecordDoubleSign; }
            set { this.m_bNursingRecordDoubleSign = value; }
        }

        private bool m_bOptionBatchRecordPrint = true;

        /// <summary>
        /// 获取或设置批量录入窗口显示打印按钮
        /// </summary>
        public bool OptionBatchRecordPrint
        {
            get { return this.m_bOptionBatchRecordPrint; }
            set { this.m_bOptionBatchRecordPrint = value; }
        }

        private bool m_bOptionRecTableInput = false;

        /// <summary>
        /// 获取或设置护理记录单录入方式
        /// </summary>
        public bool OptionRecTableInput
        {
            get { return this.m_bOptionRecTableInput; }
            set { this.m_bOptionRecTableInput = value; }
        }

        private bool m_bOptionRecShowByType = false;

        /// <summary>
        /// 获取或设置护理记录单是否按类型分类显示
        /// </summary>
        public bool OptionRecShowByType
        {
            get { return this.m_bOptionRecShowByType; }
            set { this.m_bOptionRecShowByType = value; }
        }

        private DateTime m_tOptionRecShowByTypeStartTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置护理记录单按类型显示的开始时间
        /// </summary>
        public DateTime OptionRecShowByTypeStartTime
        {
            get { return this.m_tOptionRecShowByTypeStartTime; }
            set { this.m_tOptionRecShowByTypeStartTime = value; }
        }

        private bool m_bOptionOrdersPrint = true;

        /// <summary>
        /// 获取或设置医嘱本窗口显示打印按钮
        /// </summary>
        public bool OptionOrdersPrint
        {
            get { return this.m_bOptionOrdersPrint; }
            set { this.m_bOptionOrdersPrint = value; }
        }

        private int m_nOptionCalculateSummary = 0;

        /// <summary>
        /// 获取或设置24小结出入量的算法.
        /// 目前默认为杭三算法
        /// </summary>
        public int OptionCalculateSummary
        {
            get { return m_nOptionCalculateSummary; }
            set { m_nOptionCalculateSummary = value; }
        }

        private int m_nOptionBPCount = 1;

        /// <summary>
        /// 获取或者设置当前环境下血压测试的次数
        /// </summary>
        public int OptionBPCount
        {
            get { return this.m_nOptionBPCount; }
            set { this.m_nOptionBPCount = value; }
        }

        private int m_nTaskMessageTime = 0;

        /// <summary>
        /// 任务提醒时间间隔
        /// </summary>
        public int TaskMessageTime
        {
            get { return this.m_nTaskMessageTime; }
            set { this.m_nTaskMessageTime = value; }
        }

        private bool m_bDocPigeonhole = false;

        /// <summary>
        /// 文档归档后的打开校验
        /// </summary>
        public bool DocPigeonhole
        {
            get { return this.m_bDocPigeonhole; }
            set { this.m_bDocPigeonhole = value; }
        }

        private string m_szRecSummaryName = string.Empty;

        /// <summary>
        /// 护理小结模板名称
        /// </summary>
        public string RecSummaryName
        {
            get { return this.m_szRecSummaryName; }
            set { this.m_szRecSummaryName = value; }
        }

        private bool m_szPlanOrderRec = false;

        /// <summary>
        /// 是否显示医嘱拆分
        /// </summary>
        public bool PlanOrderRec
        {
            get { return this.m_szPlanOrderRec; }
            set { this.m_szPlanOrderRec = value; }
        }

        private bool m_szSpecialNursingIncrease = false;

        /// <summary>
        /// 是否开启转科护理自动新增
        /// </summary>
        public bool SpecialNursingIncrease
        {
            get { return this.m_szSpecialNursingIncrease; }
            set { this.m_szSpecialNursingIncrease = value; }
        }

        private bool m_szNurRecordUnit = false;

        /// <summary>
        /// 是否开启护理记录的单位非ml的判断
        /// </summary>
        public bool NurRecordUnit
        {
            get { return this.m_szNurRecordUnit; }
            set { this.m_szNurRecordUnit = value; }
        }

        private bool m_bNpcNewModel = false;

        /// <summary>
        /// 是否开启护理计划新版配置（杭三修改）需搭配系统起始页（护理计划映射表）使用
        /// </summary>
        public bool NpcNewModel
        {
            get { return this.m_bNpcNewModel; }
            set { this.m_bNpcNewModel = value; }
        }

        private bool m_bVitalSignsSync = true;

        /// <summary>
        /// 是否开启表格式护理记录单 体征数据同步
        /// </summary>
        public bool VitalSignsSync
        {
            get { return this.m_bVitalSignsSync; }
            set { this.m_bVitalSignsSync = value; }
        }

        private bool m_bNcpListStatusSort = false;

        /// <summary>
        /// 护理计划 状态排序
        /// </summary>
        public bool NcpListStatusSort
        {
            get { return this.m_bNcpListStatusSort; }
            set { this.m_bNcpListStatusSort = value; }
        }

        private int m_iDocLoadTime = 0;

        /// <summary>
        /// 护理文书列表显示默认加载时间范围（h）
        /// </summary>
        public int DocLoadTime
        {
            get { return this.m_iDocLoadTime; }
            set { this.m_iDocLoadTime = value; }
        }

        private bool m_bShowAllInNurRec = false;

        /// <summary>
        /// 护理记录列配置 是否将病区和全院一起显示
        /// </summary>
        public bool ShowAllInNurRec
        {
            get { return this.m_bShowAllInNurRec; }
            set { this.m_bShowAllInNurRec = value; }
        }

        private bool m_bNursingSpecialDeleteDirectly = false;

        /// <summary>
        /// 专科列表删除时 是否打开文档 
        /// </summary>
        public bool NursingSpecialDeleteDirectly
        {
            get { return this.m_bNursingSpecialDeleteDirectly; }
            set { this.m_bNursingSpecialDeleteDirectly = value; }
        }

        private bool m_bShiftItemSort = false;

        /// <summary>
        /// 护理交接班项目排序开关
        /// </summary>
        public bool ShiftItemSort
        {
            get { return this.m_bShiftItemSort; }
            set { this.m_bShiftItemSort = value; }
        }

        private int m_iMaxPreviewPages = 100;

        /// <summary>
        /// 护理记录单打印最大预览页数 防止内存不够问题
        /// </summary>
        public int MaxPreviewPages
        {
            get { return this.m_iMaxPreviewPages; }
            set { this.m_iMaxPreviewPages = value; }
        }

        private bool m_bSaveAsButtonVisible = false;
        /// <summary>
        /// 是否允许显示另存为按钮
        /// </summary>
        public bool SaveAsButtonVisible
        {
            get { return this.m_bSaveAsButtonVisible; }
            set { this.m_bSaveAsButtonVisible = value; }
        }
    }

    /// <summary>
    /// FTP配置信息类
    /// </summary>
    public class FtpConfig : DbTypeBase, ICloneable
    {
        private string m_szFtpIP = null;

        /// <summary>
        /// 获取或设置FTP的访问IP地址
        /// </summary>
        public string FtpIP
        {
            get { return this.m_szFtpIP; }
            set { this.m_szFtpIP = value; }
        }

        private int m_nFtpPort = -1;

        /// <summary>
        /// 获取或设置FTP的访问端口号
        /// </summary>
        public int FtpPort
        {
            get { return this.m_nFtpPort; }
            set { this.m_nFtpPort = value; }
        }

        private string m_szFtpUser = null;

        /// <summary>
        /// 获取或设置FTP的访问用户名称
        /// </summary>
        public string FtpUser
        {
            get { return this.m_szFtpUser; }
            set { this.m_szFtpUser = value; }
        }

        private string m_szFtpPwd = null;

        /// <summary>
        /// 获取或设置FTP的访问用户密码
        /// </summary>
        public string FtpPwd
        {
            get { return this.m_szFtpPwd; }
            set { this.m_szFtpPwd = value; }
        }

        private FtpMode m_nFtpMode = FtpMode.PASV;

        /// <summary>
        /// 获取或设置FTP协议模式
        /// </summary>
        public FtpMode FtpMode
        {
            get { return this.m_nFtpMode; }
            set { this.m_nFtpMode = value; }
        }
    }

    /// <summary>
    /// 文档类型信息类
    /// </summary>
    public class DocTypeInfo : DbTypeBase, ICloneable
    {
        private string m_szDocTypeID = string.Empty;

        /// <summary>
        /// 获取或设置文档类型的ID，采用和CDA兼容的LONIC文档编码作为文档的ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        private int m_nDocTypeNo = 0;

        /// <summary>
        /// 获取或设置该文档类型在列表中的排序
        /// </summary>
        public int DocTypeNo
        {
            get { return this.m_nDocTypeNo; }
            set { this.m_nDocTypeNo = value; }
        }

        private string m_szDocTypeName = string.Empty;

        /// <summary>
        /// 获取或设置文件类型的显示中文名
        /// </summary>
        public string DocTypeName
        {
            get { return this.m_szDocTypeName; }
            set { this.m_szDocTypeName = value; }
        }

        private string m_szApplyEnv = string.Empty;

        /// <summary>
        /// 获取或设置应用环境
        /// </summary>
        public string ApplyEnv
        {
            get { return this.m_szApplyEnv; }
            set { this.m_szApplyEnv = value; }
        }

        private string m_szParentID = string.Empty;

        /// <summary>
        /// 获取或设置该文档类型对应的目录ID
        /// </summary>
        public string ParentID
        {
            get { return this.m_szParentID; }
            set { this.m_szParentID = value; }
        }

        private bool m_bIsRepeated = true;

        /// <summary>
        /// 获取或设置该类型的文档是否可重复
        /// </summary>
        public bool IsRepeated
        {
            get { return this.m_bIsRepeated; }
            set { this.m_bIsRepeated = value; }
        }

        private bool m_bIsValid = true;

        /// <summary>
        /// 获取或设置标识当前文档类型是否有效
        /// </summary>
        public bool IsValid
        {
            get { return this.m_bIsValid; }
            set { this.m_bIsValid = value; }
        }

        private bool m_bIsVisible = true;

        /// <summary>
        /// 获取或设置标识当前文档类型是否界面可见
        /// </summary>
        public bool IsVisible
        {
            get { return this.m_bIsVisible; }
            set { this.m_bIsVisible = value; }
        }

        private bool m_bIsQCVisible = true;

        /// <summary>
        /// 获取或设置标识当前文档是否在质控模式下可见
        /// </summary>
        public bool IsQCVisible
        {
            get { return this.m_bIsQCVisible; }
            set { this.m_bIsQCVisible = value; }
        }

        private FormPrintMode m_nPrintMode = FormPrintMode.None;

        /// <summary>
        /// 获取或设置标识当前文档模板打印模式
        /// </summary>
        public FormPrintMode PrintMode
        {
            get { return this.m_nPrintMode; }
            set { this.m_nPrintMode = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置标识当前文档类型是否是目录
        /// </summary>
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private DateTime m_dtModifyTime;

        /// <summary>
        /// 获取或设置文档类型信息的修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szSortColumn = string.Empty;

        /// <summary>
        /// 获取或设置文档列表默认排序列
        /// </summary>
        public string SortColumn
        {
            get { return this.m_szSortColumn; }
            set { this.m_szSortColumn = value; }
        }

        private SortMode m_iSortMode = SortMode.None;

        /// <summary>
        /// 获取或设置文档列表默认排序方式
        /// </summary>
        public SortMode SortMode
        {
            get { return this.m_iSortMode; }
            set { this.m_iSortMode = value; }
        }

        private string m_szSchemaID = null;

        /// <summary>
        /// 获取或设置文档所属表格列配置方案ID
        /// </summary>
        public string SchemaID
        {
            get { return this.m_szSchemaID; }
            set { this.m_szSchemaID = value; }
        }

        public DocTypeInfo()
        {
            this.m_dtModifyTime = base.DefaultTime;
        }

        public override object Clone()
        {
            DocTypeInfo docTypeInfo = new DocTypeInfo();
            GlobalMethods.Reflect.CopyProperties(this, docTypeInfo);
            return docTypeInfo;
        }

        public override string ToString()
        {
            return this.m_szDocTypeName;
        }

        public string MakeDocTypeID()
        {
            return GlobalMethods.Misc.Random(100000, 999999).ToString();
        }
    }

    /// <summary>
    /// 评估模板子项类
    /// </summary>
    public class EvaTypeItem : DbTypeBase, ICloneable
    {
        private string m_szEvaTypeID = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string EvaTypeID
        {
            get { return this.m_szEvaTypeID; }
            set { this.m_szEvaTypeID = value; }
        }

        private string m_szItemNo = "";

        /// <summary>
        /// 
        /// </summary>
        public string ItemNo
        {
            get { return this.m_szItemNo; }
            set { this.m_szItemNo = value; }
        }

        private string m_szItemText = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string ItemText
        {
            get { return this.m_szItemText; }
            set { this.m_szItemText = value; }
        }

        private bool m_szItemTextBlod = false;

        /// <summary>
        /// 
        /// </summary>
        public bool ItemTextBlod
        {
            get { return this.m_szItemTextBlod; }
            set { this.m_szItemTextBlod = value; }
        }

        private string m_ItemType = "选择框";

        /// <summary>
        /// 
        /// </summary>
        public string ItemType
        {
            get { return this.m_ItemType; }
            set { this.m_ItemType = value; }
        }

        private int m_iItemScore = 1;

        /// <summary>
        /// 
        /// </summary>
        public int ItemScore
        {
            get { return this.m_iItemScore; }
            set { this.m_iItemScore = value; }
        }

        private string m_szItemDefaultValue = "";

        /// <summary>
        /// 
        /// </summary>
        public string ItemDefaultValue
        {
            get { return this.m_szItemDefaultValue; }
            set { this.m_szItemDefaultValue = value; }
        }

        private bool m_bItemInCount = true;

        /// <summary>
        /// 
        /// </summary>
        public bool ItemInCount
        {
            get { return this.m_bItemInCount; }
            set { this.m_bItemInCount = value; }
        }

        private int m_iItemSort = 1;

        /// <summary>
        /// 
        /// </summary>
        public int ItemSort
        {
            get { return this.m_iItemSort; }
            set { this.m_iItemSort = value; }
        }

        private bool m_bItemReadOnly = false;

        /// <summary>
        /// 
        /// </summary>
        public bool ItemReadOnly
        {
            get { return this.m_bItemReadOnly; }
            set { this.m_bItemReadOnly = value; }
        }

        private bool m_bItemEnable = false;

        /// <summary>
        /// 
        /// </summary>
        public bool ItemEnable
        {
            get { return this.m_bItemEnable; }
            set { this.m_bItemEnable = value; }
        }

        public static string MakeEvaItemNo()
        {
            return string.Format("Item_{0}", GlobalMethods.Misc.Random(100, 999).ToString());
        }
    }

    /// <summary>
    /// 护理评估模板类
    /// </summary>
    public class EvaTypeInfo : DbTypeBase, ICloneable
    {
        private string m_szEvaTypeID = string.Empty;

        /// <summary>
        /// 获取或设置该评估模板的模板类型ID
        /// </summary>
        public string EvaTypeID
        {
            get { return this.m_szEvaTypeID; }
            set { this.m_szEvaTypeID = value; }
        }

        private string m_szEvaTypeName = string.Empty;

        /// <summary>
        /// 获取或设置该评估模板的模板名称
        /// </summary>
        public string EvaTypeName
        {
            get { return this.m_szEvaTypeName; }
            set { this.m_szEvaTypeName = value; }
        }

        private DateTime m_dtCreateTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置该评估模板的创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private int m_iEvaTypeNo = 0;

        /// <summary>
        /// 获取或设置该评估模板类型在列表中的排序
        /// </summary>
        public int EvaTypeNo
        {
            get { return this.m_iEvaTypeNo; }
            set { this.m_iEvaTypeNo = value; }
        }

        private bool m_isValid = true;

        /// <summary>
        /// 获取或设置该评估模板类型是否可用
        /// </summary>
        public bool IsValid
        {
            get { return this.m_isValid; }
            set { this.m_isValid = value; }
        }

        private bool m_IsVisible = true;

        /// <summary>
        /// 获取或设置该评估模板类型是否可见
        /// </summary>
        public bool IsVisible
        {
            get { return this.m_IsVisible; }
            set { this.m_IsVisible = value; }
        }

        private bool m_IsFolder = true;

        /// <summary>
        /// 获取或设置该评估模板类型是否问文件目录
        /// </summary>
        public bool IsFolder
        {
            get { return this.m_IsFolder; }
            set { this.m_IsFolder = value; }
        }

        private string m_szParentID = string.Empty;

        /// <summary>
        /// 获取或设置该评估模板类型父ID
        /// </summary>
        public string ParentID
        {
            get { return this.m_szParentID; }
            set { this.m_szParentID = value; }
        }

        private bool m_bHaveRemark = false;

        /// <summary>
        /// 获取或设置该评估模板类型是否有备注
        /// </summary>
        public bool HaveRemark
        {
            get { return this.m_bHaveRemark; }
            set { this.m_bHaveRemark = value; }
        }

        private int m_iStandard = 0;

        /// <summary>
        /// 获取或设置该评估模板类型的达标率
        /// </summary>
        public int Standard
        {
            get { return this.m_iStandard; }
            set { this.m_iStandard = value; }
        }

        public string MakeEvaTypeID(DateTime datetime)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}"
                , datetime.Year.ToString(), datetime.Month.ToString()
                , datetime.Day.ToString(), datetime.Hour.ToString()
                , datetime.Minute.ToString(), GlobalMethods.Misc.Random(100, 999).ToString());
        }
    }

    /// <summary>
    /// 护理评价内容信息类
    /// </summary>
    public class EvaDocInfo : DbTypeBase, ICloneable
    {
        private string m_szEvaID = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息的ID
        /// </summary>
        public string EvaID
        {
            get { return this.m_szEvaID; }
            set { this.m_szEvaID = value; }
        }

        private string m_szEvaTypeID = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息的模板类型ID
        /// </summary>
        public string EvaTypeID
        {
            get { return this.m_szEvaTypeID; }
            set { this.m_szEvaTypeID = value; }
        }

        private string m_szEvaTypeName = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息的模板名称
        /// </summary>
        public string EvaTypeName
        {
            get { return this.m_szEvaTypeName; }
            set { this.m_szEvaTypeName = value; }
        }

        private DateTime m_dtEvaTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置该评估信息的创建时间
        /// </summary>
        public DateTime EvaTime
        {
            get { return this.m_dtEvaTime; }
            set { this.m_dtEvaTime = value; }
        }

        private int m_iVersion = 1;

        /// <summary>
        /// 获取或设置该评估信息的版本号
        /// </summary>
        public int Version
        {
            get { return this.m_iVersion; }
            set { this.m_iVersion = value; }
        }

        private string m_szStatus = "0";

        /// <summary>
        /// 获取或设置该评估信息的状态
        /// </summary>
        public string Status
        {
            get { return this.m_szStatus; }
            set { this.m_szStatus = value; }
        }

        private string m_szCreatorId = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息的创建人ID
        /// </summary>
        public string CreatorId
        {
            get { return this.m_szCreatorId; }
            set { this.m_szCreatorId = value; }
        }

        private string m_szCreatorName = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息的创建人姓名
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        private string m_szModifierID = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息的修改人ID
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
            set { this.m_szModifierID = value; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息的修改人姓名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        private DateTime m_dtModifyTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置该评估信息的修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息所属科室ID
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息所属科室
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private decimal m_fScore = 0;

        /// <summary>
        /// 获取或设置该评估信息分值
        /// </summary>
        public decimal Score
        {
            get { return this.m_fScore; }
            set { this.m_fScore = value; }
        }

        private string m_szRemark = "";

        /// <summary>
        /// 获取或设置该评估信息备注
        /// </summary>
        public string Remark
        {
            get { return this.m_szRemark; }
            set { this.m_szRemark = value; }
        }

        public string MakeEvaID()
        {
            string strNum = GlobalMethods.Misc.Random(100, 999).ToString();
            return string.Format("{0}_{1}{2}_{3}", this.WardCode, this.EvaTime.ToString("yyyyMMddHHmmss"), strNum, this.Version);
        }

        public string MakeEvaID(string evaID)
        {
            string strValue = evaID.Substring(0, evaID.LastIndexOf('_'));
            return string.Format("{0}_{1}", strValue, this.Version);
        }

        public override object Clone()
        {
            EvaDocInfo evaDocInfo = new EvaDocInfo();
            GlobalMethods.Reflect.CopyProperties(this, evaDocInfo);
            return evaDocInfo;
        }

        public static EvaDocInfo CreateEvaDocInfo(UserInfo userInfo, DeptInfo deptInfo, DateTime createTime, EvaTypeInfo typeInfo)
        {
            EvaDocInfo docinfo = new EvaDocInfo();
            docinfo.EvaTypeID = typeInfo.EvaTypeID;
            docinfo.EvaTypeName = typeInfo.EvaTypeName;
            docinfo.EvaTime = createTime;
            docinfo.Version = 1;
            docinfo.Status = ServerData.DocStatus.NORMAL;
            docinfo.CreatorId = userInfo.ID;
            docinfo.CreatorName = userInfo.Name;
            docinfo.ModifierID = userInfo.ID;
            docinfo.ModifierName = userInfo.Name;
            docinfo.ModifyTime = createTime;
            docinfo.WardCode = deptInfo.DeptCode;
            docinfo.WardName = deptInfo.DeptName;
            docinfo.EvaID = docinfo.MakeEvaID();
            docinfo.Remark = "";
            return docinfo;
        }
    }

    /// <summary>
    /// 护理评价内容信息类
    /// </summary>
    public class EvaDocItemInfo : DbTypeBase, ICloneable
    {
        private string m_szEvaID = string.Empty;

        /// <summary>
        /// 获取或设置该评估信息的ID
        /// </summary>
        public string EvaID
        {
            get { return this.m_szEvaID; }
            set { this.m_szEvaID = value; }
        }

        private string m_szItemNo = string.Empty;

        /// <summary>
        /// 获取或设置评价单选项ID
        /// </summary>
        public string ItemNo
        {
            get { return this.m_szItemNo; }
            set { this.m_szItemNo = value; }
        }

        private string m_szItemValue = string.Empty;

        /// <summary>
        /// 获取或设置该评估选项值
        /// </summary>
        public string ItemValue
        {
            get { return this.m_szItemValue; }
            set { this.m_szItemValue = value; }
        }

        private string m_szItemRemark = string.Empty;

        /// <summary>
        /// 获取或设置该评估选项备注
        /// </summary>
        public string ItemRemark
        {
            get { return this.m_szItemRemark; }
            set { this.m_szItemRemark = value; }
        }

    }

    /// <summary>
    /// 科室文档类型信息
    /// </summary>
    public class WardEvaType : DbTypeBase, ICloneable
    {
        private string m_szEvaTypeID = string.Empty;

        /// <summary>
        /// 获取或设置评价单类型代码
        /// </summary>
        public string EvaTypeID
        {
            get { return this.m_szEvaTypeID; }
            set { this.m_szEvaTypeID = value; }
        }

        private string m_szEvaTypeName = string.Empty;

        /// <summary>
        /// 获取或设置评价单类型名称
        /// </summary>
        public string EvaTypeName
        {
            get { return this.m_szEvaTypeName; }
            set { this.m_szEvaTypeName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        public override object Clone()
        {
            WardEvaType wardEvaType = new WardEvaType();
            GlobalMethods.Reflect.CopyProperties(this, wardEvaType);
            return wardEvaType;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.m_szEvaTypeName, this.m_szWardName);
        }
    }

    /// <summary>
    /// 科室安全与质量管理记录文档类型信息
    /// </summary>
    public class WardQCType : DbTypeBase, ICloneable
    {
        private string m_szQCTypeID = string.Empty;

        /// <summary>
        /// 获取或设置管理记录类型代码
        /// </summary>
        public string QCTypeID
        {
            get { return this.m_szQCTypeID; }
            set { this.m_szQCTypeID = value; }
        }

        private string m_szQCTypeName = string.Empty;

        /// <summary>
        /// 获取或设置管理记录类型名称
        /// </summary>
        public string QCTypeName
        {
            get { return this.m_szQCTypeName; }
            set { this.m_szQCTypeName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        public override object Clone()
        {
            WardQCType wardQCType = new WardQCType();
            GlobalMethods.Reflect.CopyProperties(this, wardQCType);
            return wardQCType;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.m_szQCTypeName, this.m_szWardName);
        }
    }

    /// <summary>
    /// 文档类型信息类
    /// </summary>
    public class DocTypeData : DbTypeBase, ICloneable
    {
        private string m_szDocTypeID = string.Empty;

        /// <summary>
        /// 获取或设置文档类型的ID，采用和CDA兼容的LONIC文档编码作为文档的ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        private int m_nDocTypeNo = 0;

        /// <summary>
        /// 获取或设置该文档类型在列表中的排序
        /// </summary>
        public int DocTypeNo
        {
            get { return this.m_nDocTypeNo; }
            set { this.m_nDocTypeNo = value; }
        }

        private string m_szDocTypeName = string.Empty;

        /// <summary>
        /// 获取或设置文件类型的显示中文名
        /// </summary>
        public string DocTypeName
        {
            get { return this.m_szDocTypeName; }
            set { this.m_szDocTypeName = value; }
        }

        private string m_szApplyEnv = string.Empty;

        /// <summary>
        /// 获取或设置应用环境
        /// </summary>
        public string ApplyEnv
        {
            get { return this.m_szApplyEnv; }
            set { this.m_szApplyEnv = value; }
        }

        private string m_szParentID = string.Empty;

        /// <summary>
        /// 获取或设置该文档类型对应的目录ID
        /// </summary>
        public string ParentID
        {
            get { return this.m_szParentID; }
            set { this.m_szParentID = value; }
        }

        private bool m_bIsRepeated = true;

        /// <summary>
        /// 获取或设置该类型的文档是否可重复
        /// </summary>
        public bool IsRepeated
        {
            get { return this.m_bIsRepeated; }
            set { this.m_bIsRepeated = value; }
        }

        private bool m_bIsValid = true;

        /// <summary>
        /// 获取或设置标识当前文档类型是否有效
        /// </summary>
        public bool IsValid
        {
            get { return this.m_bIsValid; }
            set { this.m_bIsValid = value; }
        }

        private bool m_bIsVisible = true;

        /// <summary>
        /// 获取或设置标识当前文档类型是否界面可见
        /// </summary>
        public bool IsVisible
        {
            get { return this.m_bIsVisible; }
            set { this.m_bIsVisible = value; }
        }

        private FormPrintMode m_nPrintMode = FormPrintMode.None;

        /// <summary>
        /// 获取或设置标识当前文档模板打印模式
        /// </summary>
        public FormPrintMode PrintMode
        {
            get { return this.m_nPrintMode; }
            set { this.m_nPrintMode = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置标识当前文档类型是否是目录
        /// </summary>
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private DateTime m_dtModifyTime;

        /// <summary>
        /// 获取或设置文档类型信息的修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szSortColumn = string.Empty;

        /// <summary>
        /// 获取或设置文档列表默认排序列
        /// </summary>
        public string SortColumn
        {
            get { return this.m_szSortColumn; }
            set { this.m_szSortColumn = value; }
        }

        private SortMode m_iSortMode = SortMode.None;

        /// <summary>
        /// 获取或设置文档列表默认排序方式
        /// </summary>
        public SortMode SortMode
        {
            get { return this.m_iSortMode; }
            set { this.m_iSortMode = value; }
        }

        private byte[] m_byteTempletData = null;

        /// <summary>
        /// 获取或设置模板二进制数据
        /// </summary>
        public byte[] ByteTempletData
        {
            get { return this.m_byteTempletData; }
            set { this.m_byteTempletData = value; }
        }

        public DocTypeData()
        {
            this.m_dtModifyTime = base.DefaultTime;
        }

        public override object Clone()
        {
            DocTypeInfo docTypeInfo = new DocTypeInfo();
            GlobalMethods.Reflect.CopyProperties(this, docTypeInfo);
            return docTypeInfo;
        }

        public override string ToString()
        {
            return this.m_szDocTypeName;
        }

        public string MakeDocTypeID()
        {
            return GlobalMethods.Misc.Random(100000, 999999).ToString();
        }
    }

    /// <summary>
    /// 科室文档类型信息
    /// </summary>
    public class WardDocType : DbTypeBase, ICloneable
    {
        private string m_szDocTypeID = string.Empty;

        /// <summary>
        /// 获取或设置文档类型代码
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        private string m_szDocTypeName = string.Empty;

        /// <summary>
        /// 获取或设置文档类型名称
        /// </summary>
        public string DocTypeName
        {
            get { return this.m_szDocTypeName; }
            set { this.m_szDocTypeName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        public override object Clone()
        {
            WardDocType wardDocType = new WardDocType();
            GlobalMethods.Reflect.CopyProperties(this, wardDocType);
            return wardDocType;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.m_szDocTypeName, this.m_szWardName);
        }
    }

    /// <summary>
    /// 报表类型信息类
    /// </summary>
    public class ReportTypeInfo : DbTypeBase, ICloneable
    {
        private string m_szReportTypeID = string.Empty;

        /// <summary>
        /// 获取或设置报表类型的ID
        /// </summary>
        public string ReportTypeID
        {
            get { return this.m_szReportTypeID; }
            set { this.m_szReportTypeID = value; }
        }

        private int m_nReportTypeNo = 0;

        /// <summary>
        /// 获取或设置该报表类型在列表中的排序
        /// </summary>
        public int ReportTypeNo
        {
            get { return this.m_nReportTypeNo; }
            set { this.m_nReportTypeNo = value; }
        }

        private string m_szReportTypeName = string.Empty;

        /// <summary>
        /// 获取或设置文件类型的显示中文名
        /// </summary>
        public string ReportTypeName
        {
            get { return this.m_szReportTypeName; }
            set { this.m_szReportTypeName = value; }
        }

        private string m_szApplyEnv = string.Empty;

        /// <summary>
        /// 获取或设置应用环境
        /// </summary>
        public string ApplyEnv
        {
            get { return this.m_szApplyEnv; }
            set { this.m_szApplyEnv = value; }
        }

        private bool m_bIsValid = true;

        /// <summary>
        /// 获取或设置标识当前报表类型是否有效
        /// </summary>
        public bool IsValid
        {
            get { return this.m_bIsValid; }
            set { this.m_bIsValid = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置标识当前报表类型是否是目录
        /// </summary>
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private string m_szParentID = string.Empty;

        /// <summary>
        /// 获取或设置该报表型对应的目录ID
        /// </summary>
        public string ParentID
        {
            get { return this.m_szParentID; }
            set { this.m_szParentID = value; }
        }

        private DateTime m_dtModifyTime;

        /// <summary>
        /// 获取或设置报表类型信息的修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        public ReportTypeInfo()
        {
            this.m_dtModifyTime = base.DefaultTime;
        }

        public override object Clone()
        {
            ReportTypeInfo reportTypeInfo = new ReportTypeInfo();
            GlobalMethods.Reflect.CopyProperties(this, reportTypeInfo);
            return reportTypeInfo;
        }

        public override string ToString()
        {
            return this.m_szReportTypeName;
        }

        public string MakeDocTypeID()
        {
            return GlobalMethods.Misc.Random(100000, 999999).ToString();
        }
    }

    /// <summary>
    /// 报表类型信息类
    /// </summary>
    public class ReportTypeData : DbTypeBase, ICloneable
    {
        private string m_szReportTypeID = string.Empty;

        /// <summary>
        /// 获取或设置报表类型的ID
        /// </summary>
        public string ReportTypeID
        {
            get { return this.m_szReportTypeID; }
            set { this.m_szReportTypeID = value; }
        }

        private int m_nReportTypeNo = 0;

        /// <summary>
        /// 获取或设置该报表类型在列表中的排序
        /// </summary>
        public int ReportTypeNo
        {
            get { return this.m_nReportTypeNo; }
            set { this.m_nReportTypeNo = value; }
        }

        private string m_szReportTypeName = string.Empty;

        /// <summary>
        /// 获取或设置文件类型的显示中文名
        /// </summary>
        public string ReportTypeName
        {
            get { return this.m_szReportTypeName; }
            set { this.m_szReportTypeName = value; }
        }

        private string m_szApplyEnv = string.Empty;

        /// <summary>
        /// 获取或设置应用环境
        /// </summary>
        public string ApplyEnv
        {
            get { return this.m_szApplyEnv; }
            set { this.m_szApplyEnv = value; }
        }

        private bool m_bIsValid = true;

        /// <summary>
        /// 获取或设置标识当前报表类型是否有效
        /// </summary>
        public bool IsValid
        {
            get { return this.m_bIsValid; }
            set { this.m_bIsValid = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置标识当前报表类型是否是目录
        /// </summary>
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private string m_szParentID = string.Empty;

        /// <summary>
        /// 获取或设置该报表型对应的目录ID
        /// </summary>
        public string ParentID
        {
            get { return this.m_szParentID; }
            set { this.m_szParentID = value; }
        }

        private DateTime m_dtModifyTime;

        /// <summary>
        /// 获取或设置报表类型信息的修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private byte[] m_byteTempletData = null;

        /// <summary>
        /// 获取或设置模板二进制数据
        /// </summary>
        public byte[] ByteTempletData
        {
            get { return this.m_byteTempletData; }
            set { this.m_byteTempletData = value; }
        }

        public ReportTypeData()
        {
            this.m_dtModifyTime = base.DefaultTime;
        }

        public override object Clone()
        {
            ReportTypeInfo reportTypeInfo = new ReportTypeInfo();
            GlobalMethods.Reflect.CopyProperties(this, reportTypeInfo);
            return reportTypeInfo;
        }

        public override string ToString()
        {
            return this.m_szReportTypeName;
        }

        public string MakeDocTypeID()
        {
            return GlobalMethods.Misc.Random(100000, 999999).ToString();
        }
    }

    /// <summary>
    /// 科室报表类型信息
    /// </summary>
    public class WardReportType : DbTypeBase, ICloneable
    {
        private string m_szReportTypeID = string.Empty;

        /// <summary>
        /// 获取或设置报表类型代码
        /// </summary>
        public string ReportTypeID
        {
            get { return this.m_szReportTypeID; }
            set { this.m_szReportTypeID = value; }
        }

        private string m_szReportTypeName = string.Empty;

        /// <summary>
        /// 获取或设置报表类型名称
        /// </summary>
        public string ReportTypeName
        {
            get { return this.m_szReportTypeName; }
            set { this.m_szReportTypeName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        public override object Clone()
        {
            WardReportType wardReportType = new WardReportType();
            GlobalMethods.Reflect.CopyProperties(this, wardReportType);
            return wardReportType;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.m_szReportTypeID, this.m_szWardName);
        }
    }

    /// <summary>
    /// 病历类型摘要数据信息
    /// </summary>
    public class SummaryData : DbTypeBase, ICloneable
    {
        private string m_szDocID = null;

        /// <summary>
        /// 获取或设置文档ID
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
            set { this.m_szDocID = value; }
        }

        private string m_szDataName = null;

        /// <summary>
        /// 获取或设置数据名
        /// </summary>
        public string DataName
        {
            get { return this.m_szDataName; }
            set { this.m_szDataName = value; }
        }

        private string m_szDataCode = null;

        /// <summary>
        /// 获取或设置数据代码
        /// </summary>
        public string DataCode
        {
            get { return this.m_szDataCode; }
            set { this.m_szDataCode = value; }
        }

        private string m_szDataValue = null;

        /// <summary>
        /// 获取或设置数据值
        /// </summary>
        public string DataValue
        {
            get { return this.m_szDataValue; }
            set { this.m_szDataValue = value; }
        }

        private string m_dataType = ServerData.DataType.CHARACTER;

        /// <summary>
        /// 获取或设置数据值类型
        /// </summary>
        public string DataType
        {
            get { return this.m_dataType; }
            set { this.m_dataType = value; }
        }

        private string m_szDataUnit = null;

        /// <summary>
        /// 获取或设置数据值单位
        /// </summary>
        public string DataUnit
        {
            get { return this.m_szDataUnit; }
            set { this.m_szDataUnit = value; }
        }

        private DateTime m_dtDataTime;

        /// <summary>
        /// 获取或设置数据产生时间
        /// </summary>
        public DateTime DataTime
        {
            get { return this.m_dtDataTime; }
            set { this.m_dtDataTime = value; }
        }

        private string m_szPatientID = null;

        /// <summary>
        /// 获取或设置关联的病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szVisitID = null;

        /// <summary>
        /// 获取或设置关联的就诊ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szSubID = string.Empty;

        /// <summary>
        /// 获取或设置病人子ID
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
            set { this.m_szSubID = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置所属病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szRecordID = null;

        /// <summary>
        /// 获取或设置关联的护理记录ID
        /// </summary>
        public string RecordID
        {
            get { return this.m_szRecordID; }
            set { this.m_szRecordID = value; }
        }

        private int m_category = 0;

        /// <summary>
        /// 判断此数据是否为体征数据，默认为非体征数据
        /// </summary>
        public int Category
        {
            get { return this.m_category; }
            set { this.m_category = value; }
        }

        private string m_remarks = null;

        /// <summary>
        /// 获取或设置关键数据备注
        /// </summary>
        public string Remarks
        {
            get { return this.m_remarks; }
            set { this.m_remarks = value; }
        }

        private bool m_bContainsTime = true;

        /// <summary>
        /// 获取或设置体征项目保存时是否包含时间
        /// </summary>
        public bool ContainsTime
        {
            get { return this.m_bContainsTime; }
            set { this.m_bContainsTime = value; }
        }

        private string m_szDocTypeID = null;

        /// <summary>
        /// 获取或设置文档类型ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        public SummaryData()
        {
            this.m_dtDataTime = this.DefaultTime;
        }

        public override object Clone()
        {
            SummaryData summaryData = new SummaryData();
            GlobalMethods.Reflect.CopyProperties(this, summaryData);
            return summaryData;
        }

        public override string ToString()
        {
            return string.Format("Name={0},Value={1}", this.m_szDataName, this.m_szDataValue);
        }
    }

    /// <summary>
    /// 医嘱回写信息
    /// </summary>
    public class OrdersWriteBack : DbTypeBase, ICloneable
    {
        private string m_szPatientID = null;

        /// <summary>
        /// 获取或设置病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szVisitID = null;

        /// <summary>
        /// 获取或设置病人就诊ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szOrderNo = null;

        /// <summary>
        /// 获取或设置医嘱号
        /// </summary>
        public string OrderNo
        {
            get { return this.m_szOrderNo; }
            set { this.m_szOrderNo = value; }
        }

        private string m_szOrderSubNo = null;

        /// <summary>
        /// 获取或设置子医嘱号
        /// </summary>
        public string OrderSubNo
        {
            get { return this.m_szOrderSubNo; }
            set { this.m_szOrderSubNo = value; }
        }

        private DateTime m_dtRecordTime;

        /// <summary>
        /// 获取或设置医嘱执行时间
        /// </summary>
        public DateTime RecordTime
        {
            get { return this.m_dtRecordTime; }
            set { this.m_dtRecordTime = value; }
        }

        private string m_szRecordID;

        /// <summary>
        /// 获取或设置记录ID号
        /// </summary>
        public string RecordID
        {
            get { return this.m_szRecordID; }
            set { this.m_szRecordID = value; }
        }

        private string m_szDataName;

        /// <summary>
        /// 获取或设置数据名称
        /// </summary>
        public string DataName
        {
            get { return this.m_szDataName; }
            set { this.m_szDataName = value; }
        }

        private string m_szStatus;

        /// <summary>
        /// 获取或设置管道状态
        /// </summary>
        public string Status
        {
            get { return this.m_szStatus; }
            set { this.m_szStatus = value; }
        }

        private string m_szDocID;
        /// <summary>
        /// 获取或设置文档ID
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
            set { this.m_szDocID = value; }
        }

        public OrdersWriteBack()
        {
            this.m_dtRecordTime = this.DefaultTime;
        }

        public override object Clone()
        {
            OrdersWriteBack ordersWriteBack = new OrdersWriteBack();
            GlobalMethods.Reflect.CopyProperties(this, ordersWriteBack);
            return ordersWriteBack;
        }
    }

    /// <summary>
    /// 质量与安全管理记录类型摘要数据信息
    /// </summary>
    public class QCSummaryData : DbTypeBase, ICloneable
    {
        private string m_szDocID = null;

        /// <summary>
        /// 获取或设置文档ID
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
            set { this.m_szDocID = value; }
        }

        private string m_szDataName = null;

        /// <summary>
        /// 获取或设置数据名
        /// </summary>
        public string DataName
        {
            get { return this.m_szDataName; }
            set { this.m_szDataName = value; }
        }

        private string m_szDataCode = null;

        /// <summary>
        /// 获取或设置数据代码
        /// </summary>
        public string DataCode
        {
            get { return this.m_szDataCode; }
            set { this.m_szDataCode = value; }
        }

        private string m_szDataValue = null;

        /// <summary>
        /// 获取或设置数据值
        /// </summary>
        public string DataValue
        {
            get { return this.m_szDataValue; }
            set { this.m_szDataValue = value; }
        }

        private string m_dataType = ServerData.DataType.CHARACTER;

        /// <summary>
        /// 获取或设置数据值类型
        /// </summary>
        public string DataType
        {
            get { return this.m_dataType; }
            set { this.m_dataType = value; }
        }

        private string m_szDataUnit = null;

        /// <summary>
        /// 获取或设置数据值单位
        /// </summary>
        public string DataUnit
        {
            get { return this.m_szDataUnit; }
            set { this.m_szDataUnit = value; }
        }

        private DateTime m_dtDataTime;

        /// <summary>
        /// 获取或设置数据产生时间
        /// </summary>
        public DateTime DataTime
        {
            get { return this.m_dtDataTime; }
            set { this.m_dtDataTime = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置所属病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szRecordID = null;

        /// <summary>
        /// 获取或设置关联的护理记录ID
        /// </summary>
        public string RecordID
        {
            get { return this.m_szRecordID; }
            set { this.m_szRecordID = value; }
        }

        private int m_category = 0;

        /// <summary>
        /// 判断此数据是否为体征数据，默认为非体征数据
        /// </summary>
        public int Category
        {
            get { return this.m_category; }
            set { this.m_category = value; }
        }

        private string m_remarks = null;

        /// <summary>
        /// 获取或设置关键数据备注
        /// </summary>
        public string Remarks
        {
            get { return this.m_remarks; }
            set { this.m_remarks = value; }
        }

        private bool m_bContainsTime = true;

        /// <summary>
        /// 获取或设置体征项目保存时是否包含时间
        /// </summary>
        public bool ContainsTime
        {
            get { return this.m_bContainsTime; }
            set { this.m_bContainsTime = value; }
        }

        private string m_szDocTypeID = null;

        /// <summary>
        /// 获取或设置文档类型ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        public QCSummaryData()
        {
            this.m_dtDataTime = this.DefaultTime;
        }

        public override object Clone()
        {
            QCSummaryData qcSummaryData = new QCSummaryData();
            GlobalMethods.Reflect.CopyProperties(this, qcSummaryData);
            return qcSummaryData;
        }

        public override string ToString()
        {
            return string.Format("Name={0},Value={1}", this.m_szDataName, this.m_szDataValue);
        }
    }

    /// <summary>
    /// 各项评估摘要数据信息
    /// </summary>
    public class EvaSummaryData : DbTypeBase, ICloneable
    {
        private string m_szPatientID = null;

        /// <summary>
        /// 获取或设置病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szVisitID = null;

        /// <summary>
        /// 获取或设置病人就诊ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szDocTypeID = null;

        /// <summary>
        /// 获取或设置模板ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        private DateTime m_dtDataTime;

        /// <summary>
        /// 获取或设置数据产生时间
        /// </summary>
        public DateTime DataTime
        {
            get { return this.m_dtDataTime; }
            set { this.m_dtDataTime = value; }
        }

        private string m_szScore = null;

        /// <summary>
        /// 获取或设置数据值
        /// </summary>
        public string Score
        {
            get { return this.m_szScore; }
            set { this.m_szScore = value; }
        }

        private string m_szRisk = null;

        /// <summary>
        /// 获取或设置风险值
        /// </summary>
        public string Risk
        {
            get { return this.m_szRisk; }
            set { this.m_szRisk = value; }
        }

        private string m_szFreq = null;

        /// <summary>
        /// 获取或设置评估频率
        /// </summary>
        public string Freq
        {
            get { return this.m_szFreq; }
            set { this.m_szFreq = value; }
        }

        public EvaSummaryData()
        {
            this.m_dtDataTime = this.DefaultTime;
        }

        public override object Clone()
        {
            EvaSummaryData evaSummaryData = new EvaSummaryData();
            GlobalMethods.Reflect.CopyProperties(this, evaSummaryData);
            return evaSummaryData;
        }
    }

    /// <summary>
    /// 翻身、约束具、过敏史摘要数据信息
    /// </summary>
    public class FYGSummaryData : DbTypeBase, ICloneable
    {
        private string m_szPatientID = null;

        /// <summary>
        /// 获取或设置病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szVisitID = null;

        /// <summary>
        /// 获取或设置病人就诊ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szDocTypeID = null;

        /// <summary>
        /// 获取或设置模板ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        private DateTime m_dtDataTime;

        /// <summary>
        /// 获取或设置数据产生时间
        /// </summary>
        public DateTime DataTime
        {
            get { return this.m_dtDataTime; }
            set { this.m_dtDataTime = value; }
        }

        private string m_szDataName = null;

        /// <summary>
        /// 获取或设置数据名称
        /// </summary>
        public string DataName
        {
            get { return this.m_szDataName; }
            set { this.m_szDataName = value; }
        }

        private string m_szDataValue = null;

        /// <summary>
        /// 获取或设置数据值
        /// </summary>
        public string DataValue
        {
            get { return this.m_szDataValue; }
            set { this.m_szDataValue = value; }
        }

        public FYGSummaryData()
        {
            this.m_dtDataTime = this.DefaultTime;
        }

        public override object Clone()
        {
            FYGSummaryData fygSummaryData = new FYGSummaryData();
            GlobalMethods.Reflect.CopyProperties(this, fygSummaryData);
            return fygSummaryData;
        }
    }

    /// <summary>
    /// 医疗文档信息类 
    /// </summary>
    public class NurDocInfo : DbTypeBase, ICloneable
    {
        private string m_szDocID;           //文档编号
        private string m_szDocTypeID;       //文档类型代码
        private string m_szDocTitle;        //文档标题
        private DateTime m_dtDocTime;       //文档创建时间
        private string m_szDocSetID;        //文档集编号
        private int m_nDocVersion;          //文档版本号
        private string m_szCreatorID;       //文档创建人ID
        private string m_szCreatorName;     //文档创建人姓名
        private string m_szModifierID;      //文档修改人ID
        private string m_szModifierName;    //文档修改人姓名
        private DateTime m_dtModifyTime;    //文档修改时间
        private string m_szPatientID;       //病人编号
        private string m_szPatientName;     //病人姓名
        private string m_szVisitID;         //就诊号
        private DateTime m_dtVisitTime;     //就诊时间
        private string m_szVisitType;       //就诊类型
        private string m_szSubID;           //病人子ID
        private string m_szWardCode;        //病人病区代码
        private string m_szWardName;        //病人病区名称
        private string m_szSignCode;        //签名代码
        private string m_szConfidCode;      //机密等级
        private int m_nOrderValue;          //文档排序值
        private DateTime m_dtRecordTime;    //实际记录时间
        private string m_szRecordID;        //关联的护理记录ID
        private string m_szStatusDesc;      //文档状态描述
        private string m_szResult;           //护理评估结果
        private string m_szMeasures;        //是否做过护理措施
        private string m_szFinished;        //是否已经完成
        private string m_szFrequency;        //评估频次

        /// <summary>
        /// 文档信息类
        /// </summary>
        /// <remarks></remarks>
        public NurDocInfo()
        {
            this.Initialize();
        }

        /// <summary>
        /// 文档信息类成员初始化过程(设置其默认值)
        /// </summary>
        /// <remarks></remarks>
        public void Initialize()
        {
            this.m_szDocID = string.Empty;
            this.m_szDocTypeID = string.Empty;
            this.m_szDocTitle = string.Empty;
            this.m_dtDocTime = base.DefaultTime;
            this.m_szDocSetID = string.Empty;
            this.m_nDocVersion = 0;
            this.m_szCreatorID = string.Empty;
            this.m_szCreatorName = string.Empty;
            this.m_szModifierID = string.Empty;
            this.m_szModifierName = string.Empty;
            this.m_dtModifyTime = base.DefaultTime;
            this.m_szPatientID = string.Empty;
            this.m_szPatientName = string.Empty;
            this.m_szVisitID = string.Empty;
            this.m_dtVisitTime = base.DefaultTime;
            this.m_szVisitType = string.Empty;
            this.m_szWardCode = string.Empty;
            this.m_szWardName = string.Empty;
            this.m_szSignCode = string.Empty;
            this.m_szConfidCode = string.Empty;
            this.m_nOrderValue = -1;
            this.m_dtRecordTime = base.DefaultTime;
            this.m_szRecordID = string.Empty;
            this.m_szStatusDesc = string.Empty;
            this.m_szResult = string.Empty;
            this.m_szMeasures = string.Empty;
            this.m_szFinished = string.Empty;
            this.m_szFrequency = string.Empty;
        }

        /// <summary>
        /// 评分结果
        /// </summary>
        public string Result
        {
            get { return this.m_szResult; }
            set { this.m_szResult = value; }
        }

        /// <summary>
        /// 是否做过措施
        /// </summary>
        public string Measures
        {
            get { return this.m_szMeasures; }
            set { this.m_szMeasures = value; }
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        public string szFinished
        {
            get { return this.m_szFinished; }
            set { this.m_szFinished = value; }
        }

        /// <summary>
        /// 获取或设置文档编号
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
            set { this.m_szDocID = value; }
        }

        /// <summary>
        /// 获取或设置文档类型代码
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        /// <summary>
        /// 获取或设置文档的显示标题
        /// </summary>
        public string DocTitle
        {
            get { return this.m_szDocTitle; }
            set { this.m_szDocTitle = value; }
        }

        /// <summary>
        /// 获取或设置文档的创建时间
        /// </summary>
        public DateTime DocTime
        {
            get { return this.m_dtDocTime; }
            set { this.m_dtDocTime = value; }
        }

        /// <summary>
        /// 获取或设置文档所属文档集编号
        /// </summary>
        public string DocSetID
        {
            get { return this.m_szDocSetID; }
            set { this.m_szDocSetID = value; }
        }

        /// <summary>
        /// 获取或设置文档版本号
        /// </summary>
        public int DocVersion
        {
            get { return this.m_nDocVersion; }
            set { this.m_nDocVersion = value; }
        }

        /// <summary>
        /// 获取或设置作者ID
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
            set { this.m_szCreatorID = value; }
        }

        /// <summary>
        /// 获取或设置作者姓名
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        /// <summary>
        /// 获取或设置文档修改人编号
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
            set { this.m_szModifierID = value; }
        }

        /// <summary>
        /// 获取或设置文档修改人姓名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        /// <summary>
        /// 获取或设置文档的修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        /// <summary>
        /// 获取或设置病人唯一编号
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        /// <summary>
        /// 获取或设置病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
            set { this.m_szPatientName = value; }
        }

        /// <summary>
        /// 获取或设置就诊号
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        /// <summary>
        /// 获取或设置就诊时间
        /// </summary>
        public DateTime VisitTime
        {
            get { return this.m_dtVisitTime; }
            set { this.m_dtVisitTime = value; }
        }

        /// <summary>
        /// 获取或设置就诊类型(OP-门诊，IP-住院)
        /// </summary>
        public string VisitType
        {
            get { return this.m_szVisitType; }
            set { this.m_szVisitType = value; }
        }

        /// <summary>
        /// 获取或设置病人子ID
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
            set { this.m_szSubID = value; }
        }

        /// <summary>
        /// 获取或设置病人病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        /// <summary>
        /// 获取或设置病人病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        /// <summary>
        /// 获取或设置签名代码
        /// </summary>
        public string SignCode
        {
            get { return this.m_szSignCode; }
            set { this.m_szSignCode = value; }
        }

        /// <summary>
        /// 获取或设置机密等级
        /// </summary>
        public string ConfidCode
        {
            get { return this.m_szConfidCode; }
            set { this.m_szConfidCode = value; }
        }

        /// <summary>
        /// 获取或设置文档排序值
        /// </summary>
        public int OrderValue
        {
            get { return this.m_nOrderValue; }
            set { this.m_nOrderValue = value; }
        }

        /// <summary>
        /// 获取或设置实际记录时间
        /// </summary>
        public DateTime RecordTime
        {
            get { return this.m_dtRecordTime; }
            set { this.m_dtRecordTime = value; }
        }

        /// <summary>
        /// 获取或设置文档存放路径(对于新病历,为空)
        /// </summary>
        public string RecordID
        {
            get { return this.m_szRecordID; }
            set { this.m_szRecordID = value; }
        }

        /// <summary>
        /// 获取或设置文档状态描述
        /// </summary>
        public string StatusDesc
        {
            get { return this.m_szStatusDesc; }
            set { this.m_szStatusDesc = value; }
        }

        /// <summary>
        /// 评估频次
        /// </summary>
        public string Frequency
        {
            get { return this.m_szFrequency; }
            set { this.m_szFrequency = value; }
        }

        public override string ToString()
        {
            return string.Format("DocID={0},DocTitle={1}", this.m_szDocID, this.m_szDocTitle);
        }
    }

    /// <summary>
    /// 医疗文档信息列表
    /// </summary>
    public class NurDocList : List<NurDocInfo>
    {
        /// <summary>
        /// 对列表按内部方法进行排序
        /// </summary>
        new public void Sort()
        {
            base.Sort(new Comparison<NurDocInfo>(this.Compare));
        }

        /// <summary>
        /// 对列表按内部方法进行排序
        /// </summary>
        public void SortByTime(bool isDesc)
        {
            if (!isDesc)
                base.Sort(new Comparison<NurDocInfo>(this.CompareTimeAsc));
            else
                base.Sort(new Comparison<NurDocInfo>(this.CompareTimeDesc));
        }

        /// <summary>
        /// 对指定的两个文档信息对象进行排序
        /// </summary>
        /// <param name="docInfo1">文档信息对象1</param>
        /// <param name="docInfo2">文档信息对象2</param>
        /// <returns>int</returns>
        public int Compare(NurDocInfo docInfo1, NurDocInfo docInfo2)
        {
            if (docInfo1 == null && docInfo2 != null)
                return -1;
            if (docInfo1 != null && docInfo2 == null)
                return 1;
            if (docInfo1 == null && docInfo2 == null)
                return 0;
            DateTime dtRecordTime1 = docInfo1.RecordTime;
            DateTime dtRecordTime2 = docInfo2.RecordTime;
            if (dtRecordTime1 == docInfo1.DefaultTime)
                dtRecordTime1 = docInfo1.DocTime;
            if (dtRecordTime2 == docInfo2.DefaultTime)
                dtRecordTime2 = docInfo2.DocTime;
            if (docInfo1.OrderValue >= 0 && docInfo2.OrderValue >= 0)
                return docInfo1.OrderValue - docInfo2.OrderValue;
            return dtRecordTime1.CompareTo(dtRecordTime2);
        }

        /// <summary>
        /// 对指定的两个文档信息对象进行排序
        /// </summary>
        /// <param name="docInfo1">文档信息对象1</param>
        /// <param name="docInfo2">文档信息对象2</param>
        /// <returns>int</returns>
        public int CompareTimeAsc(NurDocInfo docInfo1, NurDocInfo docInfo2)
        {
            if (docInfo1 == null && docInfo2 != null)
                return -1;
            if (docInfo1 != null && docInfo2 == null)
                return 1;
            if (docInfo1 == null && docInfo2 == null)
                return 0;
            DateTime dtRecordTime1 = docInfo1.RecordTime;
            DateTime dtRecordTime2 = docInfo2.RecordTime;
            if (dtRecordTime1 == docInfo1.DefaultTime)
                dtRecordTime1 = docInfo1.DocTime;
            if (dtRecordTime2 == docInfo2.DefaultTime)
                dtRecordTime2 = docInfo2.DocTime;
            return dtRecordTime1.CompareTo(dtRecordTime2);
        }

        /// <summary>
        /// 对指定的两个文档信息对象进行排序
        /// </summary>
        /// <param name="docInfo1">文档信息对象1</param>
        /// <param name="docInfo2">文档信息对象2</param>
        /// <returns>int</returns>
        public int CompareTimeDesc(NurDocInfo docInfo1, NurDocInfo docInfo2)
        {
            return this.CompareTimeAsc(docInfo2, docInfo1);
        }
    }

    /// <summary>
    /// 质量与安全管理记录文档信息列表
    /// </summary>
    public class QCDocList : List<QCDocInfo>
    {
        /// <summary>
        /// 对列表按内部方法进行排序
        /// </summary>
        new public void Sort()
        {
            base.Sort(new Comparison<QCDocInfo>(this.Compare));
        }

        /// <summary>
        /// 对列表按内部方法进行排序
        /// </summary>
        public void SortByTime(bool isDesc)
        {
            if (!isDesc)
                base.Sort(new Comparison<QCDocInfo>(this.CompareTimeAsc));
            else
                base.Sort(new Comparison<QCDocInfo>(this.CompareTimeDesc));
        }

        /// <summary>
        /// 对指定的两个文档信息对象进行排序
        /// </summary>
        /// <param name="qcInfo1">文档信息对象1</param>
        /// <param name="qcInfo2">文档信息对象2</param>
        /// <returns>int</returns>
        public int Compare(QCDocInfo qcInfo1, QCDocInfo qcInfo2)
        {
            if (qcInfo1 == null && qcInfo2 != null)
                return -1;
            if (qcInfo1 != null && qcInfo2 == null)
                return 1;
            if (qcInfo1 == null && qcInfo2 == null)
                return 0;
            DateTime dtRecordTime1 = qcInfo1.RecordTime;
            DateTime dtRecordTime2 = qcInfo2.RecordTime;
            if (dtRecordTime1 == qcInfo1.DefaultTime)
                dtRecordTime1 = qcInfo1.DocTime;
            if (dtRecordTime2 == qcInfo2.DefaultTime)
                dtRecordTime2 = qcInfo2.DocTime;
            if (qcInfo1.OrderValue >= 0 && qcInfo2.OrderValue >= 0)
                return qcInfo1.OrderValue - qcInfo2.OrderValue;
            return dtRecordTime1.CompareTo(dtRecordTime2);
        }

        /// <summary>
        /// 对指定的两个文档信息对象进行排序
        /// </summary>
        /// <param name="qcInfo1">文档信息对象1</param>
        /// <param name="qcInfo2">文档信息对象2</param>
        /// <returns>int</returns>
        public int CompareTimeAsc(QCDocInfo qcInfo1, QCDocInfo qcInfo2)
        {
            if (qcInfo1 == null && qcInfo2 != null)
                return -1;
            if (qcInfo1 != null && qcInfo2 == null)
                return 1;
            if (qcInfo1 == null && qcInfo2 == null)
                return 0;
            DateTime dtRecordTime1 = qcInfo1.RecordTime;
            DateTime dtRecordTime2 = qcInfo2.RecordTime;
            if (dtRecordTime1 == qcInfo1.DefaultTime)
                dtRecordTime1 = qcInfo1.DocTime;
            if (dtRecordTime2 == qcInfo2.DefaultTime)
                dtRecordTime2 = qcInfo2.DocTime;
            return dtRecordTime1.CompareTo(dtRecordTime2);
        }

        /// <summary>
        /// 对指定的两个文档信息对象进行排序
        /// </summary>
        /// <param name="qcInfo1">文档信息对象1</param>
        /// <param name="qcInfo2">文档信息对象2</param>
        /// <returns>int</returns>
        public int CompareTimeDesc(QCDocInfo qcInfo1, QCDocInfo qcInfo2)
        {
            return this.CompareTimeAsc(qcInfo2, qcInfo1);
        }
    }

    /// <summary>
    /// 护理计划按钮配置类
    /// </summary>
    public class NurCarePlanConfig
    {
        private string m_ButtonName = string.Empty;

        /// <summary>
        /// 获取或设置按钮名称
        /// </summary>
        public string ButtonName
        {
            get { return this.m_ButtonName; }
            set { this.m_ButtonName = value; }
        }

        private string m_szStatus = string.Empty;

        /// <summary>
        /// 获取或设置状态
        /// </summary>
        public string Status
        {
            get { return this.m_szStatus; }
            set { this.m_szStatus = value; }
        }

        private string m_szStatusDesc = string.Empty;

        /// <summary>
        /// 获取或设置状态说明
        /// </summary>
        public string StatusDesc
        {
            get { return this.m_szStatusDesc; }
            set { this.m_szStatusDesc = value; }
        }

        private bool m_bIsNeedGetIn = false;

        /// <summary>
        /// 获取或设置是否需要进入表单修改相关数据
        /// </summary>
        public bool IsNeedGetIn
        {
            get { return this.m_bIsNeedGetIn; }
            set { this.m_bIsNeedGetIn = value; }
        }

        private char[] m_bBtnHandler = null;

        /// <summary>
        /// 获取或设置是否需要进入表单修改相关数据
        /// </summary>
        public char[] BtnHandler
        {
            get { return this.m_bBtnHandler; }
            set { this.m_bBtnHandler = value; }
        }

        public override string ToString()
        {
            return this.m_ButtonName;
        }
    }

    /// <summary>
    /// 医嘱信息类
    /// </summary>
    public class OrderInfo : DbTypeBase, ICloneable
    {
        private bool m_bIsRepeat = false;

        /// <summary>
        /// 获取或设置长期/临时医嘱
        /// </summary>
        public bool IsRepeat
        {
            get { return this.m_bIsRepeat; }
            set { this.m_bIsRepeat = value; }
        }

        private string m_szOrderNO = string.Empty;

        /// <summary>
        /// 获取或设置医嘱号
        /// </summary>
        public string OrderNO
        {
            get { return this.m_szOrderNO; }
            set { this.m_szOrderNO = value; }
        }

        private string m_szOrderSubNO = string.Empty;

        /// <summary>
        /// 获取或设置子医嘱号
        /// </summary>
        public string OrderSubNO
        {
            get { return this.m_szOrderSubNO; }
            set { this.m_szOrderSubNO = value; }
        }

        private string m_szOrderClass = string.Empty;

        /// <summary>
        /// 获取或设置医嘱类别
        /// </summary>
        public string OrderClass
        {
            get { return this.m_szOrderClass; }
            set { this.m_szOrderClass = value; }
        }

        private string m_szOrderText = string.Empty;

        /// <summary>
        /// 获取或设置医嘱内容
        /// </summary>
        public string OrderText
        {
            get { return this.m_szOrderText; }
            set { this.m_szOrderText = value; }
        }

        private DateTime m_dtEnterTime = DateTime.Now;

        /// <summary>
        /// 获取或设置医嘱下达时间
        /// </summary>
        public DateTime EnterTime
        {
            get { return this.m_dtEnterTime; }
            set { this.m_dtEnterTime = value; }
        }

        private DateTime m_dtStopTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置医嘱停止时间
        /// </summary>
        public DateTime StopTime
        {
            get { return this.m_dtStopTime; }
            set { this.m_dtStopTime = value; }
        }

        private string m_szDrugBillingAttr = string.Empty;

        /// <summary>
        /// 获取或设置是否自带
        /// </summary>
        public string DrugBillingAttr
        {
            get { return this.m_szDrugBillingAttr; }
            set { this.m_szDrugBillingAttr = value; }
        }

        private float m_fDosage = 0;

        /// <summary>
        /// 获取或设置剂量
        /// </summary>
        public float Dosage
        {
            get { return this.m_fDosage; }
            set { this.m_fDosage = value; }
        }

        private string m_szDosageUnits = string.Empty;

        /// <summary>
        /// 获取或设置单位
        /// </summary>
        public string DosageUnits
        {
            get { return this.m_szDosageUnits; }
            set { this.m_szDosageUnits = value; }
        }

        private string m_szAdministration = string.Empty;

        /// <summary>
        /// 获取或设置途径
        /// </summary>
        public string Administration
        {
            get { return this.m_szAdministration; }
            set { this.m_szAdministration = value; }
        }

        private string m_szFrequency = string.Empty;

        /// <summary>
        /// 获取或设置频次
        /// </summary>
        public string Frequency
        {
            get { return this.m_szFrequency; }
            set { this.m_szFrequency = value; }
        }

        private string m_szFreqDetail = string.Empty;

        /// <summary>
        /// 获取或设置医生说明
        /// </summary>
        public string FreqDetail
        {
            get { return this.m_szFreqDetail; }
            set { this.m_szFreqDetail = value; }
        }

        private string m_szPackCount = string.Empty;

        /// <summary>
        /// 获取或设置带药量
        /// </summary>
        public string PackCount
        {
            get { return this.m_szPackCount; }
            set { this.m_szPackCount = value; }
        }

        private string m_szDoctor = string.Empty;

        /// <summary>
        /// 获取或设置医生
        /// </summary>
        public string Doctor
        {
            get { return this.m_szDoctor; }
            set { this.m_szDoctor = value; }
        }

        private string m_szNurse = string.Empty;

        /// <summary>
        /// 获取或设置护士
        /// </summary>
        public string Nurse
        {
            get { return this.m_szNurse; }
            set { this.m_szNurse = value; }
        }

        private bool m_bIsStartStop = false;

        /// <summary>
        /// 获取或设置新开停止医嘱标志
        /// </summary>
        public bool IsStartStop
        {
            get { return this.m_bIsStartStop; }
            set { this.m_bIsStartStop = value; }
        }

        private string m_szOrderStatus = string.Empty;

        /// <summary>
        /// 获取或设置医嘱状态
        /// </summary>
        public string OrderStatus
        {
            get { return this.m_szOrderStatus; }
            set { this.m_szOrderStatus = value; }
        }

        private string m_szOrderFlag = string.Empty;

        /// <summary>
        /// 获取或设置医嘱标识
        /// </summary>
        public string OrderFlag
        {
            get { return this.m_szOrderFlag; }
            set { this.m_szOrderFlag = value; }
        }

        private string m_szPerformSchedule = string.Empty;

        /// <summary>
        /// 获取或设置医嘱执行时间
        /// </summary>
        public string PerformSchedule
        {
            get { return this.m_szPerformSchedule; }
            set { this.m_szPerformSchedule = value; }
        }

        private string m_szStopDoctor = string.Empty;

        /// <summary>
        /// 获取或设置停止医生签名
        /// </summary>
        public string StopDoctor
        {
            get { return m_szStopDoctor; }
            set { this.m_szStopDoctor = value; }
        }

        private DateTime m_dtProcessingEndDateTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置停止执行时间
        /// </summary>
        public DateTime ProcessingEndDateTime
        {
            get { return m_dtProcessingEndDateTime; }
            set { this.m_dtProcessingEndDateTime = value; }
        }

        private string m_szStopNurse = string.Empty;

        /// <summary>
        /// 获取或设置停止护士签名
        /// </summary>
        public string StopNurse
        {
            get { return m_szStopNurse; }
            set { this.m_szStopNurse = value; }
        }

        private DateTime m_dtShortProcessingEndDateTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置临时医嘱停止执行时间
        /// </summary>
        public DateTime ShortProcessingEndDateTime
        {
            get { return m_dtShortProcessingEndDateTime; }
            set { this.m_dtShortProcessingEndDateTime = value; }
        }

        private string m_szShortStopNurse = string.Empty;

        /// <summary>
        /// 获取或设置临时医嘱停止护士签名
        /// </summary>
        public string ShortStopNurse
        {
            get { return m_szShortStopNurse; }
            set { this.m_szShortStopNurse = value; }
        }

        private string m_szPerformResult = string.Empty;

        /// <summary>
        /// 皮试结果
        /// </summary>
        public string PerformResult
        {
            get { return m_szPerformResult; }
            set { this.m_szPerformResult = value; }
        }

        public OrderInfo()
        {
            this.m_dtEnterTime = this.DefaultTime;
            this.m_dtStopTime = this.DefaultTime;
            this.ProcessingEndDateTime = this.DefaultTime;
            this.m_dtShortProcessingEndDateTime = this.DefaultTime;
        }
    }

    /// <summary>
    /// 病案质量问题分类
    /// </summary>
    public class QCEventType : DbTypeBase
    {
        private string m_szSerialNo = string.Empty;     //序号
        private string m_szTypeDesc = string.Empty;     //问题分类
        private string m_szInputCode = string.Empty;    //输入码
        private string m_szParnetCode = string.Empty;   //上级代码

        /// <summary>
        /// 上级输入码
        /// </summary>
        public string ParnetCode
        {
            get
            {
                return m_szParnetCode;
            }
            set
            {
                m_szParnetCode = value;
            }
        }
        private double m_szMaxScore = 0.0;     //扣分上限

        /// <summary>
        /// 扣分上限
        /// </summary>
        public double MaxScore
        {
            get
            {
                return m_szMaxScore;
            }
            set
            {
                m_szMaxScore = value;
            }
        }
        /// <summary>
        /// 获取或设置序号
        /// </summary>
        public string SerialNo
        {
            get
            {
                return this.m_szSerialNo;
            }
            set
            {
                this.m_szSerialNo = value;
            }
        }
        /// <summary>
        /// 获取或设置问题分类
        /// </summary>
        public string TypeDesc
        {
            get
            {
                return this.m_szTypeDesc;
            }
            set
            {
                this.m_szTypeDesc = value;
            }
        }
        /// <summary>
        /// 获取或设置输入码
        /// </summary>
        public string InputCode
        {
            get
            {
                return this.m_szInputCode;
            }
            set
            {
                this.m_szInputCode = value;
            }
        }

        public override string ToString()
        {
            return this.m_szTypeDesc;
        }
    }

    /// <summary>
    /// 质控反馈信息模板类
    /// </summary>
    public class QCMessageTemplet : DbTypeBase
    {
        private string m_szSerialNo = string.Empty;      //序号
        private string m_szQCMsgCode = string.Empty;     //反馈信息代码
        private string m_szQCEventType = string.Empty;   //问题分类
        private string m_szMessage = string.Empty;       //信息描述
        private string m_szMessageTitle = string.Empty;  //信息标题
        private string m_szScore = string.Empty;         //扣分
        private string m_szHosScore = string.Empty;      //科室扣分
        private string m_szInputCode = string.Empty;     //输入码

        /// <summary>
        /// 获取或设置序号
        /// </summary>
        public string SerialNo
        {
            get
            {
                return this.m_szSerialNo;
            }
            set
            {
                this.m_szSerialNo = value;
            }
        }
        /// <summary>
        /// 获取或设置反馈信息代码
        /// </summary>
        public string QCMsgCode
        {
            get
            {
                return this.m_szQCMsgCode;
            }
            set
            {
                this.m_szQCMsgCode = value;
            }
        }
        /// <summary>
        /// 获取或设置问题分类
        /// </summary>
        public string QCEventType
        {
            get
            {
                return this.m_szQCEventType;
            }
            set
            {
                this.m_szQCEventType = value;
            }
        }
        /// <summary>
        /// 获取或设置信息描述
        /// </summary>
        public string Message
        {
            get
            {
                return this.m_szMessage;
            }
            set
            {
                this.m_szMessage = value;
            }
        }
        /// <summary>
        /// 获取或设置信息标题
        /// </summary>
        public string MessageTitle
        {
            get
            {
                return m_szMessageTitle;
            }
            set
            {
                m_szMessageTitle = value;
            }
        }
        /// <summary>
        /// 获取或设置扣分
        /// </summary>
        public string Score
        {
            get
            {
                return this.m_szScore;
            }
            set
            {
                this.m_szScore = value;
            }
        }
        /// <summary>
        /// 获取或设置全院扣分
        /// </summary>
        public string HosScore
        {
            get
            {
                return this.m_szHosScore;
            }
            set
            {
                this.m_szHosScore = value;
            }
        }
        /// <summary>
        /// 获取或设置输入码
        /// </summary>
        public string InputCode
        {
            get
            {
                return this.m_szInputCode;
            }
            set
            {
                this.m_szInputCode = value;
            }
        }

        public override string ToString()
        {
            return this.m_szMessage;
        }
    }

    /// <summary>
    /// 质控问题信息类
    /// </summary>
    public class QCQuestionInfo : DbTypeBase
    {
        private string m_szDeptCode = string.Empty;
        /// <summary>
        /// 获取或设置所在科室代码
        /// </summary>
        public string DeptCode
        {
            get
            {
                return this.m_szDeptCode;
            }
            set
            {
                this.m_szDeptCode = value;
            }
        }

        private string m_szDeptName = string.Empty;
        /// <summary>
        /// 获取或设置所在科室名称
        /// </summary>
        public string DeptName
        {
            get
            {
                return this.m_szDeptName;
            }
            set
            {
                this.m_szDeptName = value;
            }
        }

        private string m_szPatientID = string.Empty;
        /// <summary>
        /// 获取或设置患者的ID
        /// </summary>
        public string PatientID
        {
            get
            {
                return this.m_szPatientID;
            }
            set
            {
                this.m_szPatientID = value;
            }
        }

        private string m_szVisitID = string.Empty;
        /// <summary>
        /// 获取或设置患者的本次就诊ID
        /// </summary>
        public string VisitID
        {
            get
            {
                return this.m_szVisitID;
            }
            set
            {
                this.m_szVisitID = value;
            }
        }

        private string m_szPatientName = string.Empty;
        /// <summary>
        /// 获取或设置患者姓名
        /// </summary>
        public string PatientName
        {
            get
            {
                return this.m_szPatientName;
            }
            set
            {
                this.m_szPatientName = value;
            }
        }

        private string m_szQuestionType = string.Empty;
        /// <summary>
        /// 获取或设置问题类型
        /// </summary>
        public string QuestionType
        {
            get
            {
                return this.m_szQuestionType;
            }
            set
            {
                this.m_szQuestionType = value;
            }
        }

        private string m_szQuestionCode = string.Empty;
        /// <summary>
        /// 获取或设置问题代码
        /// </summary>
        public string QuestionCode
        {
            get
            {
                return this.m_szQuestionCode;
            }
            set
            {
                this.m_szQuestionCode = value;
            }
        }

        private string m_szQuestionContent = string.Empty;
        /// <summary>
        /// 获取或设置问题内容
        /// </summary>
        public string QuestionContent
        {
            get
            {
                return this.m_szQuestionContent;
            }
            set
            {
                this.m_szQuestionContent = value;
            }
        }

        private string m_szQuestionStatus = string.Empty;
        /// <summary>
        /// 获取或设置问题确认状态
        /// 0 未确认/未接收  1 已确认/已接受 2 已修改 3合格
        /// </summary>
        public string QuestionStatus
        {
            get
            {
                return this.m_szQuestionStatus;
            }
            set
            {
                this.m_szQuestionStatus = value;
            }
        }

        private string m_szDoctorInCharge = string.Empty;
        /// <summary>
        /// 获取或设置经治医生
        /// </summary>
        public string DoctorInCharge
        {
            get
            {
                return this.m_szDoctorInCharge;
            }
            set
            {
                this.m_szDoctorInCharge = value;
            }
        }

        private string m_ParentDoctor = string.Empty;
        /// <summary>
        /// 上级医生
        /// </summary>
        public string ParentDoctor
        {
            get
            {
                return m_ParentDoctor;
            }
            set
            {
                m_ParentDoctor = value;
            }
        }


        private string m_SuperDoctor = string.Empty;
        /// <summary>
        /// 主任医生
        /// </summary>
        public string SuperDoctor
        {
            get
            {
                return m_SuperDoctor;
            }
            set
            {
                m_SuperDoctor = value;
            }
        }
        private string m_szCheckerName = string.Empty;
        /// <summary>
        /// 获取或设置质控检查者
        /// </summary>
        public string CheckerName
        {
            get
            {
                return this.m_szCheckerName;
            }
            set
            {
                this.m_szCheckerName = value;
            }
        }

        private DateTime m_dtCheckTime = DateTime.Now;
        /// <summary>
        /// 获取或设置检查日期
        /// </summary>
        public DateTime CheckTime
        {
            get
            {
                return this.m_dtCheckTime;
            }
            set
            {
                this.m_dtCheckTime = value;
            }
        }

        private DateTime m_dtConfirmTime = DateTime.Now;
        /// <summary>
        /// 获取或设置确认日期
        /// </summary>
        public DateTime ConfirmTime
        {
            get
            {
                return this.m_dtConfirmTime;
            }
            set
            {
                this.m_dtConfirmTime = value;
            }
        }

        private string m_szQcModule = string.Empty;
        /// <summary>
        /// 获取或设置病案质控类型
        /// </summary>
        public string QcModule
        {
            get
            {
                return this.m_szQcModule;
            }
            set
            {
                this.m_szQcModule = value;
            }
        }

        private string m_szTopicID = string.Empty;
        /// <summary>
        /// 获取或设置病历主题代码
        /// </summary>
        public string TopicID
        {
            get
            {
                return this.m_szTopicID;
            }
            set
            {
                this.m_szTopicID = value;
            }
        }

        private string m_szTopic = string.Empty;
        /// <summary>
        /// 获取或设置病历主题
        /// </summary>
        public string Topic
        {
            get
            {
                return this.m_szTopic;
            }
            set
            {
                this.m_szTopic = value;
            }
        }

        private string m_szDoctorComment = string.Empty;
        /// <summary>
        /// 获取或设置医生反馈信息
        /// </summary>
        public string DoctorComment
        {
            get
            {
                return this.m_szDoctorComment;
            }
            set
            {
                this.m_szDoctorComment = value;
            }
        }

        private bool m_bIsSend = false;
        /// <summary>
        /// 获取或设置是否把反馈信息发送到EMRS
        /// </summary>
        public bool IsSend
        {
            get
            {
                return this.m_bIsSend;
            }
            set
            {
                this.m_bIsSend = value;
            }
        }

        private bool m_bDeductMark = true;
        /// <summary>
        ///  获取或设置反馈信息是否立刻扣分
        /// </summary>
        public bool DeductMark
        {
            get
            {
                return this.m_bDeductMark;
            }
            set
            {
                this.m_bDeductMark = value;
            }
        }

        private string m_szPointPeriod = string.Empty;
        /// <summary>
        /// 获取或设置扣分期限
        /// </summary>
        public string PointPeriod
        {
            get
            {
                return this.m_szPointPeriod;
            }
            set
            {
                this.m_szPointPeriod = value;
            }
        }

        private string m_szPoint = "0.0";
        /// <summary>
        /// 获取或设置扣分值
        /// </summary>
        public string Point
        {
            get
            {
                return this.m_szPoint;
            }
            set
            {
                this.m_szPoint = value;
            }
        }

        private string m_szPointType = "1";
        /// <summary>
        /// 获取或设置扣分类型  0-自动扣分 1-手动输入扣分
        /// </summary>
        public string PointType
        {
            get
            {
                return this.m_szPointType;
            }
            set
            {
                this.m_szPointType = value;
            }
        }

        private string m_szQaEventType = string.Empty;
        /// <summary>
        /// 获取或设置问题大类的类型
        /// </summary>
        public string QaEventType
        {
            get
            {
                return this.m_szQaEventType;
            }
            set
            {
                this.m_szQaEventType = value;
            }
        }

        private string m_szInpNo = string.Empty;

        /// <summary>
        /// 病案号
        /// </summary>
        public string InpNo
        {
            get
            {
                return m_szInpNo;
            }
            set
            {
                m_szInpNo = value;
            }
        }

        private string m_szDiagnosis = string.Empty;

        /// <summary>
        /// 诊断
        /// </summary>
        public string Diagnosis
        {
            get
            {
                return m_szDiagnosis;
            }
            set
            {
                m_szDiagnosis = value;
            }
        }
        private string m_MSG_ID = string.Empty;
        /// <summary>
        /// 质检信息ID
        /// </summary>
        public string MSG_ID
        {
            get
            {
                return m_MSG_ID;
            }
            set
            {
                m_MSG_ID = value;
            }
        }
        private bool m_bLockStatus = false;
        /// <summary>
        /// 强制锁定状态，0：不锁定；1：锁定，医生必须修改问题才能创建病历
        /// </summary>
        public bool LockStatus
        {
            get
            {
                return m_bLockStatus;
            }
            set
            {
                m_bLockStatus = value;
            }
        }
        /// <summary>
        /// 质检者ID
        /// </summary>
        public string CheckerID
        {
            get
            {
                return m_CheckerID;
            }
            set
            {
                m_CheckerID = value;
            }
        }
        /// <summary>
        /// 检查者科室名称
        /// </summary>
        public string CheckerDeptName
        {
            get
            {
                return m_CheckerDeptName;
            }
            set
            {
                m_CheckerDeptName = value;
            }
        }
        /// <summary>
        /// 检查者科室代码
        /// </summary>
        public string CheckerDeptCode
        {
            get
            {
                return m_CheckerDeptCode;
            }
            set
            {
                m_CheckerDeptCode = value;
            }
        }

        private string m_CheckerID = string.Empty;

        private string m_CheckerDeptName = string.Empty;
        private string m_CheckerDeptCode = string.Empty;
        /// <summary> 
        /// 
        /// </summary>
        /// <param name="isMakeID">是否为质检信息生成ID【仅新添加的质检信息需要】</param>
        public QCQuestionInfo(bool isMakeID)
        {
            this.m_dtCheckTime = this.DefaultTime;
            this.m_dtConfirmTime = this.DefaultTime;
            if (isMakeID)
            {
                Random rand = new Random();
                string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
                this.m_MSG_ID = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
            }
        }
        /// <summary>
        /// 为新建的质检信息添加ID[老数据会没有ID]
        /// </summary>
        private void MakeMsgID()
        {

        }
    }

    /// <summary>
    /// 医嘱拆分信息类
    /// </summary>
    public class PlanOrderInfo : DbTypeBase, ICloneable
    {
        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取或设置病人Id
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取或设置本次住院标识
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szOrderNO = string.Empty;

        /// <summary>
        /// 获取或设置医嘱号
        /// </summary>
        public string OrderNO
        {
            get { return this.m_szOrderNO; }
            set { this.m_szOrderNO = value; }
        }

        private string m_szOrderPlanSubNO = string.Empty;

        /// <summary>
        /// 获取或设置医嘱号拆分序号
        /// </summary>
        public string OrderPlanSubNO
        {
            get { return this.m_szOrderPlanSubNO; }
            set { this.m_szOrderPlanSubNO = value; }
        }

        private string m_szExecClass = string.Empty;

        /// <summary>
        /// 获取或设置执行类别
        /// </summary>
        public string ExecClass
        {
            get { return this.m_szExecClass; }
            set { this.m_szExecClass = value; }
        }

        private string m_szPlanState = string.Empty;

        /// <summary>
        /// 获取或设置编辑状态
        /// </summary>
        public string PlanState
        {
            get { return this.m_szPlanState; }
            set { this.m_szPlanState = value; }
        }

        private DateTime m_dtScheduleTime = DateTime.Now;

        /// <summary>
        /// 获取或设置计划执行时间
        /// </summary>
        public DateTime ScheduleTime
        {
            get { return this.m_dtScheduleTime; }
            set { this.m_dtScheduleTime = value; }
        }

        private string m_szPerformedFlag = string.Empty;

        /// <summary>
        /// 获取或设置执行状态
        /// </summary>
        public string PerformedFlag
        {
            get { return this.m_szPerformedFlag; }
            set { this.m_szPerformedFlag = value; }
        }

        private string m_szOperator = string.Empty;

        /// <summary>
        /// 获取或设置执行人
        /// </summary>
        public string Operator
        {
            get { return this.m_szOperator; }
            set { this.m_szOperator = value; }
        }

        private DateTime m_dtOperatingTime = DateTime.Now;

        /// <summary>
        /// 获取或设置医嘱执行时间
        /// </summary>
        public DateTime OperatingTime
        {
            get { return this.m_dtOperatingTime; }
            set { this.m_dtOperatingTime = value; }
        }

        private int m_szRealDosage = 0;

        /// <summary>
        /// 获取或设置实入量
        /// </summary>
        public int RealDosage
        {
            get { return this.m_szRealDosage; }
            set { this.m_szRealDosage = value; }
        }

        private string m_szScheduleType = string.Empty;

        /// <summary>
        /// 获取或设置计划执行类别
        /// </summary>
        public string ScheduleType
        {
            get { return this.m_szScheduleType; }
            set { this.m_szScheduleType = value; }
        }

        private string m_szOperatorMemo = string.Empty;

        /// <summary>
        /// 获取或设置执行备忘(理由)
        /// </summary>
        public string OperatorMemo
        {
            get { return this.m_szOperatorMemo; }
            set { this.m_szOperatorMemo = value; }
        }

        private DateTime m_dtOperatingEndTime = DateTime.Now;

        /// <summary>
        /// 获取或设置执行结束时间
        /// </summary>
        public DateTime OperatingEndTime
        {
            get { return this.m_dtOperatingEndTime; }
            set { this.m_dtOperatingEndTime = value; }
        }

        private string m_szPerformResult = string.Empty;

        /// <summary>
        /// 获取或设置执行结果
        /// </summary>
        public string PerformResult
        {
            get { return this.m_szPerformResult; }
            set { this.m_szPerformResult = value; }
        }

        private string m_szBarCode = string.Empty;

        /// <summary>
        /// 获取或设置瓶贴条码
        /// </summary>
        public string BarCode
        {
            get { return this.m_szBarCode; }
            set { this.m_szBarCode = value; }
        }

        private string m_szSigner = string.Empty;

        /// <summary>
        /// 获取或设置签名人
        /// </summary>
        public string Signer
        {
            get { return this.m_szSigner; }
            set { this.m_szSigner = value; }
        }

        private string m_szIsPrinted = string.Empty;

        /// <summary>
        /// 获取或设置是否已经打印
        /// </summary>
        public string IsPrinted
        {
            get { return this.m_szIsPrinted; }
            set { this.m_szIsPrinted = value; }
        }

        private string m_szDailyExcNO = string.Empty;

        /// <summary>
        /// 获取或设置每天执行顺序
        /// </summary>
        public string DailyExcNO
        {
            get { return this.m_szDailyExcNO; }
            set { this.m_szDailyExcNO = value; }
        }

        public PlanOrderInfo()
        {
            this.m_dtScheduleTime = this.DefaultTime;
            this.m_dtOperatingTime = this.DefaultTime;
            this.m_dtOperatingEndTime = this.DefaultTime;
        }
    }

    /// <summary>
    /// 系统用户信息
    /// </summary>
    public class UserInfo : DbTypeBase, ICloneable
    {
        protected string m_szID = string.Empty;

        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        public string ID
        {
            get { return this.m_szID; }
            set { this.m_szID = value; }
        }

        protected string m_szName = string.Empty;

        /// <summary>
        /// 获取或设置用户名
        /// </summary>
        public string Name
        {
            get { return this.m_szName; }
            set { this.m_szName = value; }
        }

        protected string m_szDeptCode = string.Empty;

        /// <summary>
        /// 获取或设置用户的科室代码
        /// </summary>
        public string DeptCode
        {
            get { return this.m_szDeptCode; }
            set { this.m_szDeptCode = value; }
        }

        protected string m_szDeptName = string.Empty;

        /// <summary>
        /// 获取或设置用户的科室名称
        /// </summary>
        public string DeptName
        {
            get { return this.m_szDeptName; }
            set { this.m_szDeptName = value; }
        }

        protected string m_szPwd = string.Empty;

        /// <summary>
        /// 获取或设置用户密码
        /// </summary>
        public string Password
        {
            get { return this.m_szPwd; }
            set { this.m_szPwd = value; }
        }

        protected string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置用户的病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        protected string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置用户的病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        public UserInfo()
        {
            this.Initialize();
        }

        public UserInfo(string id, string name, string deptCode, string deptName)
        {
            this.m_szID = id;
            this.m_szName = name;
            this.m_szDeptCode = deptCode;
            this.m_szDeptName = deptName;
            this.m_szWardCode = deptCode;
            this.m_szWardName = deptName;
        }

        /// <summary>
        /// 初始化为缺省值
        /// </summary>
        public void Initialize()
        {
            this.m_szID = string.Empty;
            this.m_szName = string.Empty;
            this.m_szPwd = string.Empty;

            this.m_szDeptCode = string.Empty;
            this.m_szDeptName = string.Empty;
            this.m_szWardCode = string.Empty;
            this.m_szWardName = string.Empty;
        }

        public override string ToString()
        {
            return this.m_szName;
        }

        public override object Clone()
        {
            UserInfo userInfo = new UserInfo();
            GlobalMethods.Reflect.CopyProperties(this, userInfo);
            return userInfo;
        }

        /// <summary>
        /// 比较两个UserInfo对象
        /// </summary>
        /// <param name="userInfo">比较的UserInfo对象</param>
        /// <returns>bool</returns>
        public bool Equals(UserInfo userInfo)
        {
            if (userInfo == null)
                return false;

            return ((this.m_szID == userInfo.ID)
                && (this.m_szName == userInfo.Name)
                && (this.m_szPwd == userInfo.Password)
                && (this.m_szDeptCode == userInfo.DeptCode)
                && (this.m_szDeptName == userInfo.DeptName)
                && (this.m_szWardCode == userInfo.WardCode)
                && (this.m_szWardName == userInfo.WardName));
        }
    }

    /// <summary>
    /// 病人转科信息
    /// </summary>
    public class TransferInfo : DbTypeBase, ICloneable
    {
        private string m_szPatientId = string.Empty;

        /// <summary>
        /// 获取或设置病人Id
        /// </summary>
        public string PatientId
        {
            get { return this.m_szPatientId; }
            set { this.m_szPatientId = value; }
        }

        private string m_szVisitId = string.Empty;

        /// <summary>
        /// 获取或设置访问次
        /// </summary>
        public string VisitId
        {
            get { return this.m_szVisitId; }
            set { this.m_szVisitId = value; }
        }

        private string m_szDeptCode = string.Empty;

        /// <summary>
        /// 获取或设置病人呆过的科室编码
        /// </summary>
        public string DeptCode
        {
            get { return this.m_szDeptCode; }
            set { this.m_szDeptCode = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置就诊病区码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szDeptName = string.Empty;

        /// <summary>
        /// 获取或设置病人呆过的科室名称
        /// </summary>
        public string DeptName
        {
            get { return this.m_szDeptName; }
            set { this.m_szDeptName = value; }
        }

        private DateTime m_dtAdmissionTime;

        /// <summary>
        /// 获取或设置入科时间
        /// </summary>
        public DateTime AdmissionTime
        {
            get { return this.m_dtAdmissionTime; }
            set { this.m_dtAdmissionTime = value; }
        }

        private DateTime m_dtDischargeTime;

        /// <summary>
        /// 获取或设置出科或出院时间
        /// </summary>
        public DateTime DischargeTime
        {
            get { return this.m_dtDischargeTime; }
            set { this.m_dtDischargeTime = value; }
        }

        private string m_szDeptTransfer = string.Empty;

        /// <summary>
        /// 获取或设置转入科室
        /// </summary>
        public string DeptTransfer
        {
            get { return this.m_szDeptTransfer; }
            set { this.m_szDeptTransfer = value; }
        }

        private string m_szDoctor = string.Empty;

        /// <summary>
        /// 获取或设置医生
        /// </summary>
        public string Doctor
        {
            get { return this.m_szDoctor; }
            set { this.m_szDoctor = value; }
        }

        private string m_szDoctorDept = string.Empty;

        /// <summary>
        /// 获取或设置医生科室
        /// </summary>
        public string DoctorDept
        {
            get { return this.m_szDoctorDept; }
            set { this.m_szDoctorDept = value; }
        }

        public override string ToString()
        {
            return this.m_szDeptName;
        }

        public override object Clone()
        {
            TransferInfo transferInfo = new TransferInfo();
            GlobalMethods.Reflect.CopyProperties(this, transferInfo);
            return transferInfo;
        }
    }

    /// <summary>
    /// 用户组信息
    /// </summary>
    public class UserGroup : DbTypeBase, ICloneable
    {
        protected string m_szGroupCode = string.Empty;

        /// <summary>
        /// 获取或设置组代码
        /// </summary>
        public string GroupCode
        {
            get { return this.m_szGroupCode; }
            set { this.m_szGroupCode = value; }
        }

        protected UserInfo m_userInfo = null;

        /// <summary>
        /// 获取或设置用户信息
        /// </summary>
        public UserInfo UserInfo
        {
            get
            {
                return this.m_userInfo;
            }

            set
            {
                if (value != null)
                    this.m_userInfo = value;
            }
        }

        protected int m_nPriority = 9;

        /// <summary>
        /// 获取或设置用户组优先级
        /// </summary>
        public int Priority
        {
            get { return this.m_nPriority; }
            set { this.m_nPriority = value; }
        }

        public UserGroup()
        {
            this.Initialize();
        }

        /// <summary>
        /// 初始化为缺省值
        /// </summary>
        public void Initialize()
        {
            this.m_szGroupCode = string.Empty;
            this.m_userInfo = new UserInfo();
            this.m_nPriority = 9;
        }

        public override string ToString()
        {
            return string.Format("UserID={0};GroupCode={1}", this.m_userInfo.ID, this.m_szGroupCode);
        }

        public override object Clone()
        {
            UserGroup userGroup = new UserGroup();
            userGroup.GroupCode = this.m_szGroupCode;
            userGroup.Priority = this.m_nPriority;
            userGroup.UserInfo.ID = this.m_userInfo.ID;
            userGroup.UserInfo.Name = this.m_userInfo.ID;
            userGroup.UserInfo.DeptCode = this.m_userInfo.DeptCode;
            userGroup.UserInfo.DeptName = this.m_userInfo.DeptName;
            userGroup.UserInfo.WardCode = this.m_userInfo.WardCode;
            userGroup.UserInfo.WardName = this.m_userInfo.WardName;
            userGroup.UserInfo.Password = this.m_userInfo.Password;
            return userGroup;
        }
    }

    /// <summary>
    /// 科室信息
    /// </summary>
    public class DeptInfo : DbTypeBase, ICloneable
    {
        private string m_szDeptCode = string.Empty;

        /// <summary>
        /// 获取或设置科室代码
        /// </summary>
        public string DeptCode
        {
            get { return this.m_szDeptCode; }
            set { this.m_szDeptCode = value; }
        }

        private string m_szDeptName = string.Empty;

        /// <summary>
        /// 获取或设置科室名称
        /// </summary>
        public string DeptName
        {
            get { return this.m_szDeptName; }
            set { this.m_szDeptName = value; }
        }

        private bool m_bIsClinicDept = true;

        /// <summary>
        /// 获取或设置是否是临床科室
        /// </summary>
        public bool IsClinicDept
        {
            get { return this.m_bIsClinicDept; }
            set { this.m_bIsClinicDept = value; }
        }

        private bool m_bIsWardDept = true;

        /// <summary>
        /// 获取或设置是否是住院科室
        /// </summary>
        public bool IsWardDept
        {
            get { return this.m_bIsWardDept; }
            set { this.m_bIsWardDept = value; }
        }

        private bool m_bIsOutpDept = false;

        /// <summary>
        /// 获取或设置是否是门诊科室
        /// </summary>
        public bool IsOutpDept
        {
            get { return this.m_bIsOutpDept; }
            set { this.m_bIsOutpDept = value; }
        }

        private bool m_bIsNurseDept = false;

        /// <summary>
        /// 获取或设置是否是护理单元
        /// </summary>
        public bool IsNurseDept
        {
            get { return this.m_bIsNurseDept; }
            set { this.m_bIsNurseDept = value; }
        }

        private bool m_bIsUserGroup = false;

        /// <summary>
        /// 获取或设置是否是用户组
        /// </summary>
        public bool IsUserGroup
        {
            get { return this.m_bIsUserGroup; }
            set { this.m_bIsUserGroup = value; }
        }

        private string m_szInputCode = string.Empty;

        /// <summary>
        /// 获取或设置科室名称输入码
        /// </summary>
        public string InputCode
        {
            get { return this.m_szInputCode; }
            set { this.m_szInputCode = value; }
        }

        public override string ToString()
        {
            return this.m_szDeptName;
        }
    }

    /// <summary>
    /// 检验信息类
    /// </summary>
    public class LabTestInfo : DbTypeBase, ICloneable
    {
        private string m_szTestID = string.Empty;

        /// <summary>
        /// 获取或设置申请序号
        /// </summary>
        public string TestID
        {
            get { return this.m_szTestID; }
            set { this.m_szTestID = value; }
        }

        private string m_szSubject = string.Empty;

        /// <summary>
        /// 获取或设置检验主题名称
        /// </summary>
        public string Subject
        {
            get { return this.m_szSubject; }
            set { this.m_szSubject = value; }
        }

        private string m_szSpecimen = string.Empty;

        /// <summary>
        /// 获取或设置标本
        /// </summary>
        public string Specimen
        {
            get { return this.m_szSpecimen; }
            set { this.m_szSpecimen = value; }
        }

        private string m_szRequestDoctor = string.Empty;

        /// <summary>
        /// 获取或设置申请医生
        /// </summary>
        public string RequestDoctor
        {
            get { return this.m_szRequestDoctor; }
            set { this.m_szRequestDoctor = value; }
        }

        private DateTime m_dtRequestTime = DateTime.Now;

        /// <summary>
        /// 获取或设置申请日期
        /// </summary>
        public DateTime RequestTime
        {
            get { return this.m_dtRequestTime; }
            set { this.m_dtRequestTime = value; }
        }

        private string m_szResultStatus = string.Empty;

        /// <summary>
        /// 获取或设置报告状态
        /// </summary>
        public string ResultStatus
        {
            get { return this.m_szResultStatus; }
            set { this.m_szResultStatus = value; }
        }

        private string m_szReportDoctor = string.Empty;

        /// <summary>
        /// 获取或设置报告人
        /// </summary>
        public string ReportDoctor
        {
            get { return this.m_szReportDoctor; }
            set { this.m_szReportDoctor = value; }
        }

        private DateTime m_dtReportTime = DateTime.Now;

        /// <summary>
        /// 获取或设置报告日期
        /// </summary>
        public DateTime ReportTime
        {
            get { return this.m_dtReportTime; }
            set { this.m_dtReportTime = value; }
        }

        private List<TestResultInfo> m_lstTestResultInfo;

        /// <summary>
        /// 获取或设置当次检验记录
        /// </summary>
        public List<TestResultInfo> lstTestResultInfo
        {
            get { return this.m_lstTestResultInfo; }
            set { this.m_lstTestResultInfo = value; }
        }

        public LabTestInfo()
        {
            this.m_dtReportTime = this.DefaultTime;
            this.m_dtRequestTime = this.DefaultTime;
        }
    }

    /// <summary>
    /// 检验记录类
    /// </summary>
    public class TestResultInfo : DbTypeBase, ICloneable
    {
        private string m_szTestID = string.Empty;

        /// <summary>
        /// 获取或设置申请序号
        /// </summary>
        public string TestID
        {
            get { return this.m_szTestID; }
            set { this.m_szTestID = value; }
        }

        private int m_nItemNo = 0;

        /// <summary>
        /// 获取或设置报告项目序号
        /// </summary>
        public int ItemNo
        {
            get { return this.m_nItemNo; }
            set { this.m_nItemNo = value; }
        }

        private string m_szItemName = string.Empty;

        /// <summary>
        /// 获取或设置报告项目名称
        /// </summary>
        public string ItemName
        {
            get { return this.m_szItemName; }
            set { this.m_szItemName = value; }
        }

        private string m_szItemResult = string.Empty;

        /// <summary>
        /// 获取或设置检验结果值
        /// </summary>
        public string ItemResult
        {
            get { return this.m_szItemResult; }
            set { this.m_szItemResult = value; }
        }

        private string m_szItemUnits = string.Empty;

        /// <summary>
        /// 获取或设置检验结果单位
        /// </summary>
        public string ItemUnits
        {
            get { return this.m_szItemUnits; }
            set { this.m_szItemUnits = value; }
        }

        private string m_szItemRefer = string.Empty;

        /// <summary>
        /// 获取或设置正常参考值
        /// </summary>
        public string ItemRefer
        {
            get { return this.m_szItemRefer; }
            set { this.m_szItemRefer = value; }
        }

        private string m_szAbnormalIndecator = string.Empty;

        /// <summary>
        /// 获取或设置异常标识
        /// </summary>
        public string AbnormalIndecator
        {
            get { return this.m_szAbnormalIndecator; }
            set { this.m_szAbnormalIndecator = value; }
        }
    }

    /// <summary>
    /// 检查信息
    /// </summary>
    public class ExamInfo : DbTypeBase, ICloneable
    {
        private string m_szExamID = string.Empty;

        /// <summary>
        /// 获取或设置检查序号
        /// </summary>
        public string ExamID
        {
            get { return this.m_szExamID; }
            set { this.m_szExamID = value; }
        }

        private string m_szSubject = string.Empty;

        /// <summary>
        /// 获取或设置检查主题
        /// </summary>
        public string Subject
        {
            get { return this.m_szSubject; }
            set { this.m_szSubject = value; }
        }

        private DateTime m_dtRequestTime = DateTime.Now;

        /// <summary>
        /// 获取或设置检查申请日期
        /// </summary>
        public DateTime RequestTime
        {
            get { return this.m_dtRequestTime; }
            set { this.m_dtRequestTime = value; }
        }

        private string m_szRequestDoctor = string.Empty;

        /// <summary>
        /// 获取或设置检查申请医生
        /// </summary>
        public string RequestDoctor
        {
            get { return this.m_szRequestDoctor; }
            set { this.m_szRequestDoctor = value; }
        }

        private string m_szResultStatus = string.Empty;

        /// <summary>
        /// 获取或设置报告状态
        /// </summary>
        public string ResultStatus
        {
            get { return this.m_szResultStatus; }
            set { this.m_szResultStatus = value; }
        }

        private DateTime m_dtReportTime = DateTime.Now;

        /// <summary>
        /// 获取或设置检查报告日期
        /// </summary>
        public DateTime ReportTime
        {
            get { return this.m_dtReportTime; }
            set { this.m_dtReportTime = value; }
        }

        private string m_szReportDoctor = string.Empty;

        /// <summary>
        /// 获取或设置检查报告人
        /// </summary>
        public string ReportDoctor
        {
            get { return this.m_szReportDoctor; }
            set { this.m_szReportDoctor = value; }
        }

        private ExamResultInfo m_examResultInfo = null;

        /// <summary>
        /// 获取或设置检查报告详细信息
        /// </summary>
        public ExamResultInfo ExamResultInfo
        {
            get { return this.m_examResultInfo; }
            set { this.m_examResultInfo = value; }
        }

        public ExamInfo()
        {
            this.m_dtReportTime = this.DefaultTime;
            this.m_dtRequestTime = this.DefaultTime;
        }
    }

    /// <summary>
    /// 检查报告详细信息
    /// </summary>
    public class ExamResultInfo : DbTypeBase, ICloneable
    {
        private string m_szExamID = string.Empty;

        /// <summary>
        /// 获取或设置检查号
        /// </summary>
        public string ExamID
        {
            get { return this.m_szExamID; }
            set { this.m_szExamID = value; }
        }

        private string m_szParameters = string.Empty;

        /// <summary>
        /// 获取或设置检查参数
        /// </summary>
        public string Parameters
        {
            get { return this.m_szParameters; }
            set { this.m_szParameters = value; }
        }

        private string m_szDiscription = string.Empty;

        /// <summary>
        /// 获取或设置检查所见
        /// </summary>
        public string Discription
        {
            get { return this.m_szDiscription; }
            set { this.m_szDiscription = value; }
        }

        private string m_szImpression = string.Empty;

        /// <summary>
        /// 获取或设置检查印象
        /// </summary>
        public string Impression
        {
            get { return this.m_szImpression; }
            set { this.m_szImpression = value; }
        }

        private string m_szRecommendation = string.Empty;

        /// <summary>
        /// 获取或设置检查建议
        /// </summary>
        public string Recommendation
        {
            get { return this.m_szRecommendation; }
            set { this.m_szRecommendation = value; }
        }

        private string m_szAbnormal = string.Empty;

        /// <summary>
        /// 获取或设置是否阳性
        /// </summary>
        public string Abnormal
        {
            get { return this.m_szAbnormal; }
            set { this.m_szAbnormal = value; }
        }
    }

    /// <summary>
    /// 病人就诊信息类
    /// </summary>
    public class PatVisitInfo : DbTypeBase, ICloneable
    {
        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取或设置病人ID
        /// </summary>
        public string PatientId
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szPatientName = string.Empty;

        /// <summary>
        /// 获取或设置病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
            set { this.m_szPatientName = value; }
        }

        private string m_szPatientSex = string.Empty;

        /// <summary>
        /// 获取或设置性别显示名
        /// </summary>
        public string PatientSex
        {
            get { return this.m_szPatientSex; }
            set { this.m_szPatientSex = value; }
        }

        private DateTime m_dtBirthTime = DateTime.Now;

        /// <summary>
        /// 获取或设置出生时间
        /// </summary>
        public DateTime BirthTime
        {
            get { return this.m_dtBirthTime; }
            set { this.m_dtBirthTime = value; }
        }

        private string m_szMaritalStatus = string.Empty;

        /// <summary>
        /// 获取或设置婚姻状况
        /// </summary>
        public string MaritalStatus
        {
            get { return this.m_szMaritalStatus; }
            set { this.m_szMaritalStatus = value; }
        }

        private string m_szBirthPlace = string.Empty;

        /// <summary>
        /// 获取或设置出生地
        /// </summary>
        public string BirthPlace
        {
            get { return this.m_szBirthPlace; }
            set { this.m_szBirthPlace = value; }
        }

        private string m_szNation = string.Empty;

        /// <summary>
        /// 获取或设置民族
        /// </summary>
        public string Nation
        {
            get { return this.m_szNation; }
            set { this.m_szNation = value; }
        }

        private string m_szFamilyAddress = string.Empty;

        /// <summary>
        /// 获取或设置家庭住址
        /// </summary>
        public string FamilyAddress
        {
            get { return this.m_szFamilyAddress; }
            set { this.m_szFamilyAddress = value; }
        }

        private string m_szServiceAgency = string.Empty;

        /// <summary>
        /// 获取或设置工作单位
        /// </summary>
        public string ServiceAgency
        {
            get { return this.m_szServiceAgency; }
            set { this.m_szServiceAgency = value; }
        }

        private string m_szOccupation = string.Empty;

        /// <summary>
        /// 获取或设置职业
        /// </summary>
        public string Occupation
        {
            get { return this.m_szOccupation; }
            set { this.m_szOccupation = value; }
        }

        private string m_szChargeType = string.Empty;

        /// <summary>
        /// 获取或设置费别
        /// </summary>
        public string ChargeType
        {
            get { return this.m_szChargeType; }
            set { this.m_szChargeType = value; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取或设置就诊ID
        /// </summary>
        public string VisitId
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szSubID = "0";

        /// <summary>
        /// 获取或设置就诊子ID
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
            set { this.m_szSubID = value; }
        }

        private string m_szInpNo = string.Empty;

        /// <summary>
        /// 获取或设置住院号
        /// </summary>
        public string InpNo
        {
            get { return this.m_szInpNo; }
            set { this.m_szInpNo = value; }
        }

        private DateTime m_dtVisitTime;

        /// <summary>
        /// 获取或设置就诊时间
        /// </summary>
        public DateTime VisitTime
        {
            get { return this.m_dtVisitTime; }
            set { this.m_dtVisitTime = value; }
        }

        private string m_szVisitType = "IP";

        /// <summary>
        /// 获取或设置就诊类型
        /// </summary>
        public string VisitType
        {
            get { return this.m_szVisitType; }
            set { this.m_szVisitType = value; }
        }

        private DateTime m_dtEnterTime;

        /// <summary>
        /// 获取或设置入科时间
        /// </summary>
        public DateTime EnterTime
        {
            get { return this.m_dtEnterTime; }
            set { this.m_dtEnterTime = value; }
        }

        private string m_szDeptCode = string.Empty;

        /// <summary>
        /// 获取或设置就诊科室代码
        /// </summary>
        public string DeptCode
        {
            get { return this.m_szDeptCode; }
            set { this.m_szDeptCode = value; }
        }

        private string m_szDeptName = string.Empty;

        /// <summary>
        /// 获取或设置就诊科室
        /// </summary>
        public string DeptName
        {
            get { return this.m_szDeptName; }
            set { this.m_szDeptName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置就诊病区码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置就诊病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szBedCode = string.Empty;

        /// <summary>
        /// 获取或设置床位，设置了Ward后必要
        /// </summary>
        public string BedCode
        {
            get { return this.m_szBedCode; }
            set { this.m_szBedCode = value; }
        }

        private string m_szBedLabel = string.Empty;

        /// <summary>
        /// 获取或设置床位，设置了Ward后必要
        /// </summary>
        public string BedLabel
        {
            get { return this.m_szBedLabel; }
            set { this.m_szBedLabel = value; }
        }

        private string m_szDiagnosis = string.Empty;

        /// <summary>
        /// 获取或设置诊断名称
        /// </summary>
        public string Diagnosis
        {
            get { return this.m_szDiagnosis; }
            set { this.m_szDiagnosis = value; }
        }

        private string m_allergyDrugs = string.Empty;

        /// <summary>
        /// 获取或设置过敏药物
        /// </summary>
        public string AllergyDrugs
        {
            get { return this.m_allergyDrugs; }
            set { this.m_allergyDrugs = value; }
        }

        private string m_szConfidCode = string.Empty;

        /// <summary>
        /// 获取或设置病人机密性代码
        /// </summary>
        public string ConfidCode
        {
            get { return this.m_szConfidCode; }
            set { this.m_szConfidCode = value; }
        }

        private string m_inchargeDoctor = string.Empty;

        /// <summary>
        /// 获取或设置经治医生
        /// </summary>
        public string InchargeDoctor
        {
            get { return this.m_inchargeDoctor; }
            set { this.m_inchargeDoctor = value; }
        }

        private string m_patientCondition = string.Empty;

        /// <summary>
        /// 获取或设置病情状态
        /// </summary>
        public string PatientCondition
        {
            get { return this.m_patientCondition; }
            set { this.m_patientCondition = value; }
        }

        private string m_nursingClass = string.Empty;

        /// <summary>
        /// 获取或设置护理等级
        /// </summary>
        public string NursingClass
        {
            get { return this.m_nursingClass; }
            set { this.m_nursingClass = value; }
        }

        private DateTime m_dtDischargeTime;

        /// <summary>
        /// 获取或设置出院时间
        /// </summary>
        public DateTime DischargeTime
        {
            get { return this.m_dtDischargeTime; }
            set { this.m_dtDischargeTime = value; }
        }

        private string m_szDischargeMode = null;

        /// <summary>
        /// 获取或设置出院方式
        /// </summary>
        public string DischargeMode
        {
            get { return this.m_szDischargeMode; }
            set { this.m_szDischargeMode = value; }
        }
        private string m_szQCResultStatus = string.Empty;
        /// <summary>
        /// 获取或设置病案质量审查状态
        /// </summary>
        public string QCResultStatus
        {
            get { return this.m_szQCResultStatus; }
            set { this.m_szQCResultStatus = value; }
        }
        private string m_TotalScore = null;
        /// <summary>
        /// 获取或设置病案评分总分
        /// </summary>
        public string TotalScore
        {
            get { return this.m_TotalScore; }
            set { this.m_TotalScore = value; }
        }
        public PatVisitInfo()
        {
            this.Initialize();
        }

        /// <summary>
        /// 初始化为缺省值
        /// </summary>
        public void Initialize()
        {
            this.m_dtBirthTime = base.DefaultTime;
            this.m_dtVisitTime = base.DefaultTime;
            this.m_dtEnterTime = base.DefaultTime;
            this.m_dtDischargeTime = base.DefaultTime;
        }

        /// <summary>
        /// 已重写
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return this.m_szPatientName;
        }

        /// <summary>
        /// 判定指定的病人就诊信息是否是同一次就诊
        /// </summary>
        /// <param name="patVisit">待判定病人就诊信息</param>
        /// <returns>是否是同一次就诊</returns>
        public bool IsPatVisitSame(PatVisitInfo patVisit)
        {
            if (patVisit == null)
                return false;
            if (this.PatientId != patVisit.PatientId || this.VisitId != patVisit.VisitId)
                return false;
            if (this.VisitTime != patVisit.VisitTime || this.VisitType != patVisit.VisitType)
                return false;
            return true;
        }
    }

    /// <summary>
    /// 权限点信息
    /// </summary>
    public class RightInfo : DbTypeBase, ICloneable
    {
        private string m_szName = string.Empty;

        /// <summary>
        /// 获取权限点名字
        /// </summary>
        public string Name
        {
            set { }
            get { return this.m_szName; }
        }

        private int m_nIndex = -1;

        /// <summary>
        /// 获取权限点在权限控制表中的索引
        /// </summary>
        public int Index
        {
            set { }
            get { return this.m_nIndex; }
        }

        private bool m_bValue = false;

        /// <summary>
        /// 获取或设置权限值
        /// </summary>
        public bool Value
        {
            get { return this.m_bValue; }
            set { this.m_bValue = value; }
        }

        private string m_szDescription = string.Empty;

        /// <summary>
        /// 获取或设置权限描述
        /// </summary>
        public string Description
        {
            set { }
            get { return this.m_szDescription; }
        }

        public RightInfo(string name, int index, bool value, string description)
        {
            this.m_szName = name;
            this.m_nIndex = index;
            this.m_bValue = value;
            this.m_szDescription = description;
        }
    }

    /// <summary>
    /// 护理病历系统公共字典项目信息
    /// </summary>
    public class CommonDictItem : DbTypeBase, ICloneable
    {
        private string m_itemType = string.Empty;

        /// <summary>
        /// 获取或设置字典项目类型
        /// </summary>
        public string ItemType
        {
            get { return this.m_itemType; }
            set { this.m_itemType = value; }
        }

        private int m_itemNo = 0;

        /// <summary>
        /// 获取或设置字典项目序号
        /// </summary>
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }

        private string m_itemCode = string.Empty;

        /// <summary>
        /// 获取或设置字典项目代码
        /// </summary>
        public string ItemCode
        {
            get { return this.m_itemCode; }
            set { this.m_itemCode = value; }
        }

        private string m_itemName = string.Empty;

        /// <summary>
        /// 获取或设置字典项目名称
        /// </summary>
        public string ItemName
        {
            get { return this.m_itemName; }
            set { this.m_itemName = value; }
        }

        private string m_wardCode = "ALL";

        /// <summary>
        /// 获取或设置所属科室代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_wardCode; }
            set { this.m_wardCode = value; }
        }

        private string m_wardName = "全院";

        /// <summary>
        /// 获取或设置所属科室名称
        /// </summary>
        public string WardName
        {
            get { return this.m_wardName; }
            set { this.m_wardName = value; }
        }

        private string m_inputName = string.Empty;

        /// <summary>
        /// 获取或设置字典项目输入码
        /// </summary>
        public string InputCode
        {
            get { return this.m_inputName; }
            set { this.m_inputName = value; }
        }

        public override string ToString()
        {
            return this.m_itemName;
        }

        public override object Clone()
        {
            CommonDictItem commonDictItem = new CommonDictItem();
            GlobalMethods.Reflect.CopyProperties(this, commonDictItem);
            return commonDictItem;
        }

        public string MakeItemCode()
        {
            StringBuilder sbPrefix = new StringBuilder();
            if (!string.IsNullOrEmpty(this.m_itemType))
            {
                string[] words = this.m_itemType.Split('_');
                if (words.Length >= 1 && !string.IsNullOrEmpty(words[0]))
                    sbPrefix.Append(words[0][0].ToString().ToUpper());
                if (words.Length >= 2 && !string.IsNullOrEmpty(words[1]))
                    sbPrefix.Append(words[1][0].ToString().ToUpper());
            }

            Random rand = new Random();
            string szRand = rand.Next(10000, 99999).ToString().PadLeft(5, '0');
            return string.Concat(sbPrefix.ToString(), szRand);
        }
    }

    /// <summary>
    /// 用户权限基类
    /// </summary>
    public abstract class UserRightBase : DbTypeBase, ICloneable
    {
        private string m_szUserID = string.Empty;

        /// <summary>
        /// 获取或设置用户代码
        /// </summary>
        public string UserID
        {
            get { return this.m_szUserID; }
            set { this.m_szUserID = value; }
        }

        private string m_szRightDesc = string.Empty;

        /// <summary>
        /// 获取或设置权限描述
        /// </summary>
        public string RightDesc
        {
            get { return this.m_szRightDesc; }
            set { this.m_szRightDesc = value; }
        }

        protected UserRightType m_eRightType = UserRightType.NurDoc;

        /// <summary>
        /// 获取或设置权限类型 编辑器-MEDDOC 质控-MRQC
        /// </summary>
        public UserRightType RightType
        {
            set { }
            get { return this.m_eRightType; }
        }

        /// <summary>
        /// 获取权限类型名称
        /// </summary>
        /// <param name="rightType">用户权限类型</param>
        /// <returns>string</returns>
        public static string GetRightTypeName(UserRightType rightType)
        {
            return "NURDOC";
        }

        /// <summary>
        /// 创建指定的用户权限
        /// </summary>
        /// <param name="rightType">用户权限类型</param>
        /// <returns>MDSDBLib.UserRightBase</returns>
        public static UserRightBase Create(UserRightType rightType)
        {
            return new NurUserRight();
        }

        /// <summary>
        /// 获取二进制位表示的用户权限代码
        /// </summary>
        /// <returns>权限代码</returns>
        public string GetRightCode()
        {
            PropertyInfo[] propertyInfos = this.GetType().GetProperties();
            if (propertyInfos == null || propertyInfos.Length <= 0)
                return string.Empty;

            StringBuilder sbRightCode = new StringBuilder();
            for (int index = 0; index < propertyInfos.Length; index++)
            {
                PropertyInfo propertyInfo = propertyInfos[index];
                if (propertyInfo.PropertyType != typeof(RightInfo))
                    continue;
                RightInfo rightInfo = propertyInfo.GetValue(this, null) as RightInfo;
                if (rightInfo == null)
                    continue;

                if (sbRightCode.Length <= rightInfo.Index)
                {
                    int count = rightInfo.Index + 1 - sbRightCode.Length;
                    sbRightCode.Append(string.Empty.PadRight(count, '0'));
                }
                if (rightInfo.Value)
                    sbRightCode.Replace('0', '1', rightInfo.Index, 1);
            }
            return sbRightCode.ToString();
        }

        /// <summary>
        /// 设置二进制位表示的权限代码
        /// </summary>
        /// <param name="szRightCode">权限代码</param>
        public void SetRightCode(string szRightCode)
        {
            int nLength = 0;
            if (!string.IsNullOrEmpty(szRightCode))
                nLength = szRightCode.Length;

            PropertyInfo[] propertyInfos = this.GetType().GetProperties();
            if (propertyInfos == null || propertyInfos.Length <= 0)
                return;
            for (int index = 0; index < propertyInfos.Length; index++)
            {
                PropertyInfo propertyInfo = propertyInfos[index];
                if (propertyInfo.PropertyType != typeof(RightInfo))
                    continue;
                RightInfo rightInfo = propertyInfo.GetValue(this, null) as RightInfo;
                if (rightInfo == null)
                    continue;
                rightInfo.Value = false;//默认为false
                if (nLength > rightInfo.Index)
                    rightInfo.Value = (szRightCode[rightInfo.Index] == '1');
            }
        }
    }

    /// <summary>
    /// 护理病历用户权限表信息
    /// </summary>
    public class NurUserRightInfo : DbTypeBase, ICloneable
    {
        private string m_szUserId;//用户标识

        public string UserId
        {
            get { return this.m_szUserId; }
            set { this.m_szUserId = value; }
        }

        private string m_szUserPwd;//用户密码

        public string UserPwd
        {
            get { return this.m_szUserPwd; }
            set { this.m_szUserPwd = value; }
        }

        private string m_szRightCode;//权限编码

        public string RightCode
        {
            get { return this.m_szRightCode; }
            set { this.m_szRightCode = value; }
        }

        private string m_zRightDesc;//权限描述

        public string RightDesc
        {
            get { return this.m_zRightDesc; }
            set { this.m_zRightDesc = value; }
        }

        private string m_szRightType;//权限类型

        public string RightType
        {
            get { return this.m_szRightType; }
            set { this.m_szRightType = value; }
        }
    }

    /// <summary>
    /// 护理病历用户权限信息
    /// </summary>
    public class NurUserRight : UserRightBase, ICloneable
    {
        private RightInfo m_NurDocSystem = null;

        /// <summary>
        /// 获取或设置是否允许使用护理病历系统
        /// </summary>
        public RightInfo NurDocSystem
        {
            set { }
            get { return this.m_NurDocSystem; }
        }

        private RightInfo m_StudentNurse = null;

        /// <summary>
        /// 获取或设置是否是实习护士级别
        /// </summary>
        public RightInfo StudentNurse
        {
            set { }
            get { return this.m_StudentNurse; }
        }

        private RightInfo m_GeneralNurse = null;

        /// <summary>
        /// 获取或设置是否是一般护士级别
        /// </summary>
        public RightInfo GeneralNurse
        {
            set { }
            get { return this.m_GeneralNurse; }
        }

        private RightInfo m_QualityNurse = null;

        /// <summary>
        /// 获取或设置是否是质控护士级别
        /// </summary>
        public RightInfo QualityNurse
        {
            set { }
            get { return this.m_QualityNurse; }
        }

        private RightInfo m_HigherNurse = null;

        /// <summary>
        /// 获取或设置是否是主管护师级别
        /// </summary>
        public RightInfo HigherNurse
        {
            set { }
            get { return this.m_HigherNurse; }
        }

        private RightInfo m_HeadNurse = null;

        /// <summary>
        /// 获取或设置是否是主任护师级别
        /// </summary>
        public RightInfo HeadNurse
        {
            set { }
            get { return this.m_HeadNurse; }
        }

        private RightInfo m_LeaderNurse = null;

        /// <summary>
        /// 获取或设置是否是护理部级别
        /// </summary>
        public RightInfo LeaderNurse
        {
            set { }
            get { return this.m_LeaderNurse; }
        }

        private RightInfo m_GrantModifyData = null;

        /// <summary>
        /// 获取或设置是否允许批准下级提交的数据修改请求
        /// </summary>
        public RightInfo GrantModifyData
        {
            set { }
            get { return this.m_GrantModifyData; }
        }

        private RightInfo m_ApprovalWardReport = null;

        /// <summary>
        /// 获取或设置是否允许审批病区报告
        /// </summary>
        public RightInfo ApprovalWardReport
        {
            set { }
            get { return this.m_ApprovalWardReport; }
        }

        private RightInfo m_ApprovalAllReport = null;

        /// <summary>
        /// 获取或设置是否允许审批全院报告
        /// </summary>
        public RightInfo ApprovalAllReport
        {
            set { }
            get { return this.m_ApprovalAllReport; }
        }

        private RightInfo m_ShowPatientListForm = null;

        /// <summary>
        /// 获取或设置是否显示病人列表窗口
        /// </summary>
        public RightInfo ShowPatientListForm
        {
            set { }
            get { return this.m_ShowPatientListForm; }
        }

        private RightInfo m_ShowBedViewForm = null;

        /// <summary>
        /// 获取或设置是否显示床位卡视图
        /// </summary>
        public RightInfo ShowBedViewForm
        {
            set { }
            get { return this.m_ShowBedViewForm; }
        }

        private RightInfo m_ShowNursingTaskForm = null;

        /// <summary>
        /// 获取或设置是否显示我的待办任务窗口
        /// </summary>
        public RightInfo ShowNursingTaskForm
        {
            set { }
            get { return this.m_ShowNursingTaskForm; }
        }

        private RightInfo m_ShowBatchRecordForm = null;

        /// <summary>
        /// 获取或设置是否显示批量录入窗口
        /// </summary>
        public RightInfo ShowBatchRecordForm
        {
            set { }
            get { return this.m_ShowBatchRecordForm; }
        }

        private RightInfo m_ShowShiftRecordForm = null;

        /// <summary>
        /// 获取或设置是否显示交接班窗口
        /// </summary>
        public RightInfo ShowShiftRecordForm
        {
            set { }
            get { return this.m_ShowShiftRecordForm; }
        }

        private RightInfo m_ShowVitalSignsGraphForm = null;

        /// <summary>
        /// 获取或设置是否显示体温单窗口
        /// </summary>
        public RightInfo ShowVitalSignsGraphForm
        {
            set { }
            get { return this.m_ShowVitalSignsGraphForm; }
        }

        private RightInfo m_ShowOrderRecordForm = null;

        /// <summary>
        /// 获取或设置是否显示医嘱单窗口
        /// </summary>
        public RightInfo ShowOrderRecordForm
        {
            set { }
            get { return this.m_ShowOrderRecordForm; }
        }

        private RightInfo m_ShowNursingRecordForm = null;

        /// <summary>
        /// 获取或设置是否显示护理记录单窗口
        /// </summary>
        public RightInfo ShowNursingRecordForm
        {
            set { }
            get { return this.m_ShowNursingRecordForm; }
        }

        private RightInfo m_ShowNursingAssessmentForm = null;

        /// <summary>
        /// 获取或设置是否显示护理评估单窗口
        /// </summary>
        public RightInfo ShowNursingAssessmentForm
        {
            set { }
            get { return this.m_ShowNursingAssessmentForm; }
        }

        private RightInfo m_ShowDocumentListForm1 = null;

        /// <summary>
        /// 获取或设置是否显示文书列表1窗口
        /// </summary>
        public RightInfo ShowDocumentListForm1
        {
            set { }
            get { return this.m_ShowDocumentListForm1; }
        }

        private RightInfo m_ShowDocumentListForm2 = null;

        /// <summary>
        /// 获取或设置是否显示文书列表2窗口
        /// </summary>
        public RightInfo ShowDocumentListForm2
        {
            set { }
            get { return this.m_ShowDocumentListForm2; }
        }

        private RightInfo m_ShowDocumentListForm3 = null;

        /// <summary>
        /// 获取或设置是否显示文书列表3窗口
        /// </summary>
        public RightInfo ShowDocumentListForm3
        {
            set { }
            get { return this.m_ShowDocumentListForm3; }
        }

        private RightInfo m_ShowDocumentListForm4 = null;

        /// <summary>
        /// 获取或设置是否显示文书列表4窗口
        /// </summary>
        public RightInfo ShowDocumentListForm4
        {
            set { }
            get { return this.m_ShowDocumentListForm4; }
        }

        private RightInfo m_ShowSpecialNursingForm = null;

        /// <summary>
        /// 获取或设置是否显示专科护理窗口
        /// </summary>
        public RightInfo ShowSpecialNursingForm
        {
            set { }
            get { return this.m_ShowSpecialNursingForm; }
        }

        private RightInfo m_EditVitalSigns = null;

        /// <summary>
        /// 获取或设置是否允许编辑体征记录
        /// </summary>
        public RightInfo EditVitalSigns
        {
            set { }
            get { return this.m_EditVitalSigns; }
        }

        private RightInfo m_EditAllVitalSigns = null;

        /// <summary>
        /// 获取或设置是否允许编辑全院体征记录
        /// </summary>
        public RightInfo EditAllVitalSigns
        {
            set { }
            get { return this.m_EditAllVitalSigns; }
        }

        private RightInfo m_PrintVitalSigns = null;

        /// <summary>
        /// 获取或设置是否允许打印体征记录
        /// </summary>
        public RightInfo PrintVitalSigns
        {
            set { }
            get { return this.m_PrintVitalSigns; }
        }

        private RightInfo m_CreateNuringRec = null;

        /// <summary>
        /// 获取或设置是否允许创建护理记录
        /// </summary>
        public RightInfo CreateNuringRec
        {
            set { }
            get { return this.m_CreateNuringRec; }
        }

        private RightInfo m_BabyRecVitalSign = null;

        /// <summary>
        /// 获取或设置是否允许婴儿体征录入
        /// </summary>
        public RightInfo BabyRecVitalSign
        {
            set { }
            get { return this.m_BabyRecVitalSign; }
        }

        private RightInfo m_EditNuringRec = null;

        /// <summary>
        /// 获取或设置是否允许编辑护理记录
        /// </summary>
        public RightInfo EditNuringRec
        {
            set { }
            get { return this.m_EditNuringRec; }
        }

        private RightInfo m_EditWardNuringRec = null;

        /// <summary>
        /// 获取或设置是否允许编辑病区护理记录
        /// </summary>
        public RightInfo EditWardNuringRec
        {
            set { }
            get { return this.m_EditWardNuringRec; }
        }

        private RightInfo m_EditAllNuringRec = null;

        /// <summary>
        /// 获取或设置是否允许编辑所有护理记录
        /// </summary>
        public RightInfo EditAllNuringRec
        {
            set { }
            get { return this.m_EditAllNuringRec; }
        }

        private RightInfo m_DeleteNuringRec = null;

        /// <summary>
        /// 获取或设置是否允许删除护理记录
        /// </summary>
        public RightInfo DeleteNuringRec
        {
            set { }
            get { return this.m_DeleteNuringRec; }
        }

        private RightInfo m_PrintNuringRec = null;

        /// <summary>
        /// 获取或设置是否允许打印护理记录
        /// </summary>
        public RightInfo PrintNuringRec
        {
            set { }
            get { return this.m_PrintNuringRec; }
        }

        private RightInfo m_CreateNuringDoc = null;

        /// <summary>
        /// 获取或设置是否允许创建护理文书
        /// </summary>
        public RightInfo CreateNuringDoc
        {
            set { }
            get { return this.m_CreateNuringDoc; }
        }

        private RightInfo m_EditNuringDoc = null;

        /// <summary>
        /// 获取或设置是否允许编辑护理文书
        /// </summary>
        public RightInfo EditNuringDoc
        {
            set { }
            get { return this.m_EditNuringDoc; }
        }

        private RightInfo m_EditWardNuringDoc = null;

        /// <summary>
        /// 获取或设置是否允许编辑病区护理文书
        /// </summary>
        public RightInfo EditWardNuringDoc
        {
            set { }
            get { return this.m_EditWardNuringDoc; }
        }

        private RightInfo m_EditAllNuringDoc = null;

        /// <summary>
        /// 获取或设置是否允许编辑所有护理文书
        /// </summary>
        public RightInfo EditAllNuringDoc
        {
            set { }
            get { return this.m_EditAllNuringDoc; }
        }

        private RightInfo m_DeleteNuringDoc = null;

        /// <summary>
        /// 获取或设置是否允许删除护理文书
        /// </summary>
        public RightInfo DeleteNuringDoc
        {
            set { }
            get { return this.m_DeleteNuringDoc; }
        }

        private RightInfo m_PrintNuringDoc = null;

        /// <summary>
        /// 获取或设置是否允许打印护理文书
        /// </summary>
        public RightInfo PrintNuringDoc
        {
            set { }
            get { return this.m_PrintNuringDoc; }
        }

        private RightInfo m_EditShiftRec = null;

        /// <summary>
        /// 获取或设置是否允许编辑交接班记录
        /// </summary>
        public RightInfo EditShiftRec
        {
            set { }
            get { return this.m_EditShiftRec; }
        }

        private RightInfo m_PrintShiftRec = null;

        /// <summary>
        /// 获取或设置是否允许打印交接班记录
        /// </summary>
        public RightInfo PrintShiftRec
        {
            set { }
            get { return this.m_PrintShiftRec; }
        }

        private RightInfo m_PrintOrderRec = null;

        /// <summary>
        /// 获取或设置是否允许打印医嘱记录
        /// </summary>
        public RightInfo PrintOrderRec
        {
            set { }
            get { return this.m_PrintOrderRec; }
        }

        private RightInfo m_ShowSpecialPatientForm = null;

        /// <summary>
        /// 获取是否允许显示专科一览表
        /// </summary>
        public RightInfo ShowSpecialPatientForm
        {
            set { }
            get { return this.m_ShowSpecialPatientForm; }
        }

        private RightInfo m_ShowNursingStatForm = null;

        /// <summary>
        /// 获取是否允许查看查询统计窗口
        /// </summary>
        public RightInfo ShowNursingStatForm
        {
            set { }
            get { return this.m_ShowNursingStatForm; }
        }

        private RightInfo m_ShowNursingQCForm = null;

        /// <summary>
        /// 获取是否允许查看质控窗口
        /// </summary>
        public RightInfo ShowNursingQCForm
        {
            set { }
            get { return this.m_ShowNursingQCForm; }
        }

        private RightInfo m_ShowNursingGraphForm = null;

        /// <summary>
        /// 获取是否允许查看监护记录单窗口
        /// </summary>
        public RightInfo ShowNursingGraphForm
        {
            set { }
            get { return this.m_ShowNursingGraphForm; }
        }

        private RightInfo m_ShowWordDocumentForm = null;

        /// <summary>
        /// 获取是否允许查看Word模板单窗口
        /// </summary>
        public RightInfo ShowWordDocumentForm
        {
            set { }
            get { return this.m_ShowWordDocumentForm; }
        }

        private RightInfo m_ShowNursingAssessForm = null;

        /// <summary>
        /// 获取是否允许查看护理评价窗口
        /// </summary>
        public RightInfo ShowNursingAssessForm
        {
            set { }
            get { return this.m_ShowNursingAssessForm; }
        }

        private RightInfo m_ShowNurCardPlanForm = null;

        /// <summary>
        /// 获取是否允许查看护理计划窗口
        /// </summary>
        public RightInfo ShowNurCardPlanForm
        {
            set { }
            get { return this.m_ShowNurCardPlanForm; }
        }

        private RightInfo m_ShowIntegrateQueryForm = null;

        /// <summary>
        /// 获取是否允许查看综合查询窗口
        /// </summary>
        public RightInfo ShowIntegrateQueryForm
        {
            set { }
            get { return m_ShowIntegrateQueryForm; }
        }

        private RightInfo m_ShowNursingConsultForm = null;

        /// <summary>
        /// 获取是否允许查看护理会诊窗口
        /// </summary>
        public RightInfo ShowNursingConsultForm
        {
            set { }
            get { return this.m_ShowNursingConsultForm; }
        }

        private RightInfo m_ShowInfoLibForm = null;

        /// <summary>
        /// 获取是否允许查看护理信息库窗口
        /// </summary>
        public RightInfo ShowInfoLibForm
        {
            set { }
            get { return this.m_ShowInfoLibForm; }
        }

        private RightInfo m_ShowRosteringCardForm = null;

        /// <summary>
        /// 获取或设置是否允许显示护士排班一览
        /// </summary>
        public RightInfo ShowRosteringCardForm
        {
            set { }
            get { return this.m_ShowRosteringCardForm; }
        }

        public NurUserRight()
        {
            //注意：权限索引编号禁止重复,同时禁止修改已存在权限的索引编号
            this.m_NurDocSystem = new RightInfo("护理病历系统", 0, false, "是否允许使用护理病历系统");

            this.m_StudentNurse = new RightInfo("实习护士", 2, false, "是否是实习护士级别");
            this.m_GeneralNurse = new RightInfo("一般护士", 3, false, "是否是一般护士级别");
            this.m_QualityNurse = new RightInfo("质控护士", 4, false, "是否是质控护士级别");
            this.m_HigherNurse = new RightInfo("主管护师", 5, false, "是否是主管护师级别");
            this.m_HeadNurse = new RightInfo("主任护师", 6, false, "是否是主任护师级别");
            this.m_LeaderNurse = new RightInfo("护理部", 7, false, "是否是护理部级别");

            this.m_GrantModifyData = new RightInfo("批准修改数据", 9, false, "是否允许审批下级提交的数据修改请求");
            this.m_ApprovalWardReport = new RightInfo("审批病区报告", 10, false, "是否允许审批病区提交的报告");
            this.m_ApprovalAllReport = new RightInfo("审批全院报告", 11, false, "是否允许审批全院提交的报告");
            this.m_ShowDocumentListForm1 = new RightInfo("查看文书列表1窗口", 12, false, "是否允许查看文书列表1窗口");
            this.m_ShowDocumentListForm2 = new RightInfo("查看文书列表2窗口", 13, false, "是否允许查看文书列表2窗口");
            this.m_ShowPatientListForm = new RightInfo("查看病人列表窗口", 14, false, "是否允许查看病人列表窗口");
            this.m_ShowBedViewForm = new RightInfo("查看床位卡视图", 15, false, "是否允许查看床位卡视图");
            this.m_ShowNursingTaskForm = new RightInfo("查看待办任务窗口", 16, false, "是否允许查看待办任务窗口");
            this.m_ShowBatchRecordForm = new RightInfo("查看批量录入窗口", 17, false, "是否允许查看批量录入窗口");
            this.m_BabyRecVitalSign = new RightInfo("允许录入婴儿体征信息", 18, false, "是否允许录入婴儿体征信息");
            this.m_ShowShiftRecordForm = new RightInfo("查看交接班窗口", 19, false, "是否允许查看交接班窗口");
            this.m_ShowInfoLibForm = new RightInfo("查看护理信息窗口", 20, false, "是否允许查看护理信息窗口");
            this.m_ShowNursingAssessmentForm = new RightInfo("查看护理评估窗口", 21, false, "是否允许查看护理评估窗口");
            this.m_ShowVitalSignsGraphForm = new RightInfo("查看体温单窗口", 22, false, "是否允许查看体温单窗口");
            this.m_ShowOrderRecordForm = new RightInfo("查看医嘱单窗口", 23, false, "是否允许查看医嘱单窗口");
            this.m_ShowNursingRecordForm = new RightInfo("查看护理记录窗口", 24, false, "是否允许查看护理记录单窗口");
            this.m_ShowSpecialNursingForm = new RightInfo("查看专科护理窗口", 25, false, "是否允许查看专科护理窗口");
            this.m_ShowDocumentListForm3 = new RightInfo("查看文书列表3窗口", 26, false, "是否允许查看文书列表3窗口");
            this.m_ShowDocumentListForm4 = new RightInfo("查看文书列表4窗口", 27, false, "是否允许查看文书列表4窗口");
            this.m_EditVitalSigns = new RightInfo("编辑体征记录", 28, false, "是否允许编辑体征记录");
            this.m_PrintVitalSigns = new RightInfo("打印体征记录", 29, false, "是否允许打印体征记录");
            this.m_CreateNuringRec = new RightInfo("创建护理记录", 30, false, "是否允许创建护理记录");
            this.m_EditNuringRec = new RightInfo("编辑护理记录", 31, false, "是否允许编辑护理记录");
            this.m_EditWardNuringRec = new RightInfo("编辑病区护理记录", 32, false, "是否允许编辑本病区护理记录");
            this.m_EditAllNuringRec = new RightInfo("编辑全院护理记录", 33, false, "是否允许编辑全院护理记录");
            this.m_DeleteNuringRec = new RightInfo("删除护理记录", 34, false, "是否允许删除护理记录");
            this.m_PrintNuringRec = new RightInfo("打印护理记录", 35, false, "是否允许打印护理记录");
            this.m_CreateNuringDoc = new RightInfo("创建护理文书", 36, false, "是否允许创建护理文书");
            this.m_EditNuringDoc = new RightInfo("编辑护理文书", 37, false, "是否允许编辑护理文书");
            this.m_EditWardNuringDoc = new RightInfo("编辑病区护理文书", 38, false, "是否允许编辑本病区护理文书");
            this.m_EditAllNuringDoc = new RightInfo("编辑全院护理文书", 39, false, "是否允许编辑全院护理文书");
            this.m_DeleteNuringDoc = new RightInfo("删除护理文书", 40, false, "是否允许删除护理文书");
            this.m_PrintNuringDoc = new RightInfo("打印护理文书", 41, false, "是否允许打印护理文书");
            this.m_EditShiftRec = new RightInfo("编辑交接班记录", 42, false, "是否允许编辑交接班记录");
            this.m_PrintShiftRec = new RightInfo("打印交接班记录", 43, false, "是否允许打印交接班记录");
            this.m_PrintOrderRec = new RightInfo("打印医嘱记录", 44, false, "是否允许打印医嘱记录");
            this.m_EditAllVitalSigns = new RightInfo("编辑全院体征记录", 45, false, "是否允许编辑全院体征记录");
            this.m_ShowSpecialPatientForm = new RightInfo("查看专科一览表", 46, false, "是否允许查看专科一览表");
            this.m_ShowNursingStatForm = new RightInfo("查看查询统计窗口", 47, false, "是否允许查看查询统计窗口");
            this.m_ShowNurCardPlanForm = new RightInfo("查看护理计划窗口", 48, false, "是否允许查看护理计划窗口");
            this.m_ShowIntegrateQueryForm = new RightInfo("查看综合查询窗口", 49, false, "是否允许查看综合查询窗口");
            this.m_ShowNursingConsultForm = new RightInfo("查看护理会诊窗口", 50, false, "是否允许查看护理会诊窗口");
            this.m_ShowNursingQCForm = new RightInfo("查看护理质控窗口", 51, false, "是否允许查看护理质控窗口");
            this.m_ShowNursingGraphForm = new RightInfo("查看监护记录窗口", 52, false, "是否允许查看监护记录窗口");
            this.m_ShowWordDocumentForm = new RightInfo("查看WORD文档窗口", 53, false, "是否允许查看WORD文档窗口");
            this.m_ShowNursingAssessForm = new RightInfo("查看护理评价窗口", 54, false, "是否允许查看护理评价窗口");
            this.m_ShowRosteringCardForm = new RightInfo("查看护士排班一览", 55, false, "是否允许查看护士排班一览");
        }
    }

    /// <summary>
    /// 护理记录单打印信息类
    /// </summary>
    public class RecPrintinfo
    {
        private string m_szPatientID = string.Empty;        //护理记录病人ID
        private string m_szVisitID = string.Empty;          //护理记录病人就诊号
        private string m_szWardCode = string.Empty;         //护理记录病人病区代码
        private string m_szUserID = string.Empty;           //护理记录打印操作用户ID
        private string m_szUserName = string.Empty;         //护理记录打印操作用户姓名
        private string m_szSchemaID = string.Empty;         //护理记录类型ID
        private int m_iPrintPages = int.MinValue;           //护理记录打印页数
        private DateTime m_dtPrintTime = DateTime.MinValue; //护理记录打印日期

        /// <summary>
        /// 护理记录病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        /// <summary>
        /// 护理记录病人就诊号
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        /// <summary>
        /// 护理记录病人病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        /// <summary>
        /// 护理记录打印操作用户ID
        /// </summary>
        public string UserID
        {
            get { return this.m_szUserID; }
            set { this.m_szUserID = value; }
        }

        /// <summary>
        /// 护理记录打印操作用户姓名
        /// </summary>
        public string UserName
        {
            get { return this.m_szUserName; }
            set { this.m_szUserName = value; }
        }

        /// <summary>
        /// 护理记录类型ID
        /// </summary>
        public string SchemaID
        {
            get { return this.m_szSchemaID; }
            set { this.m_szSchemaID = value; }
        }

        /// <summary>
        /// 护理记录打印页数
        /// </summary>
        public int PrintPages
        {
            get { return this.m_iPrintPages; }
            set { this.m_iPrintPages = value; }
        }

        /// <summary>
        /// 护理记录打印日期
        /// </summary>
        public DateTime PrintTime
        {
            get { return this.m_dtPrintTime; }
            set { this.m_dtPrintTime = value; }
        }
    }

    /// <summary>
    /// 文档状态信息类
    /// </summary>
    public class DocStatusInfo : DbTypeBase, ICloneable
    {
        private string m_szDocID = string.Empty;            //文档编号
        private string m_szDocStatus = string.Empty;        //文档类型代码
        private string m_szOperatorID = string.Empty;       //文档标题
        private string m_szOperatorName = string.Empty;     //文档集编号
        private DateTime m_dtOperateTime = DateTime.Now;    //文档创建时间
        private string m_szStatusDesc = string.Empty;       //状态描述
        private string m_szStatusMessage = string.Empty;    //状态描述

        /// <summary>
        /// 获取或设置文档编号
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
            set { this.m_szDocID = value; }
        }

        /// <summary>
        /// 获取或设置文档状态
        /// </summary>
        public string DocStatus
        {
            get { return this.m_szDocStatus; }
            set { this.m_szDocStatus = value; }
        }

        /// <summary>
        /// 获取或设置操作者ID
        /// </summary>
        public string OperatorID
        {
            get { return this.m_szOperatorID; }
            set { this.m_szOperatorID = value; }
        }

        /// <summary>
        /// 获取或设置操作者姓名
        /// </summary>
        public string OperatorName
        {
            get { return this.m_szOperatorName; }
            set { this.m_szOperatorName = value; }
        }

        /// <summary>
        /// 获取或设置操作时间
        /// </summary>
        public DateTime OperateTime
        {
            get { return this.m_dtOperateTime; }
            set { this.m_dtOperateTime = value; }
        }

        /// <summary>
        /// 获取或设置文档状态描述
        /// </summary>
        public string StatusDesc
        {
            get { return this.m_szStatusDesc; }
            set { this.m_szStatusDesc = value; }
        }

        /// <summary>
        /// 获取或设置用于返回的文档状态消息
        /// </summary>
        public string StatusMessage
        {
            get { return this.m_szStatusMessage; }
            set { this.m_szStatusMessage = value; }
        }

        public DocStatusInfo()
        {
            this.m_dtOperateTime = base.DefaultTime;
        }
    }

    /// <summary>
    /// 体征数据信息类
    /// </summary>
    public class VitalSignsData : DbTypeBase, ICloneable
    {
        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取或设置病人ID号
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取或设置就诊ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szSubID = string.Empty;

        /// <summary>
        /// 获取或设置子ID
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
            set { this.m_szSubID = value; }
        }

        private DateTime m_dtRecordDate = DateTime.Now.Date;

        /// <summary>
        /// 获取或设置录入日期
        /// </summary>
        public DateTime RecordDate
        {
            get { return this.m_dtRecordDate; }
            set { this.m_dtRecordDate = value; }
        }

        private DateTime m_dtRecordTime = DateTime.Now.ToLocalTime();

        /// <summary>
        /// 获取或设置录入时间
        /// </summary>
        public DateTime RecordTime
        {
            get { return this.m_dtRecordTime; }
            set { this.m_dtRecordTime = value; }
        }

        private string m_szRecordName = "0000";

        /// <summary>
        /// 获取或设置体征名称
        /// </summary>
        public string RecordName
        {
            get { return this.m_szRecordName; }
            set { this.m_szRecordName = value; }
        }

        private string m_szRecordData = string.Empty;

        /// <summary>
        /// 获取或设置体征数据
        /// </summary>
        public string RecordData
        {
            get { return this.m_szRecordData; }
            set { this.m_szRecordData = value; }
        }

        private string m_szDataType = string.Empty;

        /// <summary>
        /// 获取或设置数据类型
        /// </summary>
        public string DataType
        {
            get { return this.m_szDataType; }
            set { this.m_szDataType = value; }
        }

        private string m_szDataUnit = string.Empty;

        /// <summary>
        /// 获取或设置数据单位
        /// </summary>
        public string DataUnit
        {
            get { return this.m_szDataUnit; }
            set { this.m_szDataUnit = value; }
        }

        private int m_nCategory = 0;

        /// <summary>
        /// 获取或设置属性种类：
        /// 0为非体征数据不保存；
        /// 1为体征数据保存；
        /// 2为体征数据但是只显示在集成视图中。
        /// </summary>
        public int Category
        {
            get { return this.m_nCategory; }
            set { this.m_nCategory = value; }
        }

        private string m_remarks = null;

        /// <summary>
        /// 获取或设置关键数据备注
        /// </summary>
        public string Remarks
        {
            get { return this.m_remarks; }
            set { this.m_remarks = value; }
        }

        private bool m_bContainsTime = true;

        /// <summary>
        /// 获取或设置体征项目保存时是否包含时间
        /// </summary>
        public bool ContainsTime
        {
            get { return m_bContainsTime; }
            set { m_bContainsTime = value; }
        }

        private string m_szSourceTag = null;

        /// <summary>
        /// 获取或设置数据来源标记
        /// </summary>
        public string SourceTag
        {
            get { return this.m_szSourceTag; }
            set { this.m_szSourceTag = value; }
        }

        private string m_szSourceType = null;

        /// <summary>
        /// 获取或设置数据来源类型
        /// </summary>
        public string SourceType
        {
            get { return this.m_szSourceType; }
            set { this.m_szSourceType = value; }
        }

        public VitalSignsData()
        {
            this.m_dtRecordDate = this.DefaultTime;
            this.m_dtRecordTime = this.DefaultTime;
        }

        public override string ToString()
        {
            return this.m_szRecordName;
        }

        public override object Clone()
        {
            VitalSignsData vitalSignsData = new VitalSignsData();
            GlobalMethods.Reflect.CopyProperties(this, vitalSignsData);
            return vitalSignsData;
        }
    }

    /// <summary>
    /// 设置体温单显示
    /// </summary>
    public class VitalViewSchema : DbTypeBase, ICloneable
    {
        private string m_szSchemaID = string.Empty;

        /// <summary>
        /// 获取或设置表格方案ID
        /// </summary>
        public string SchemaID
        {
            get { return this.m_szSchemaID; }
            set { this.m_szSchemaID = value; }
        }

        private string m_szDocTypeID = string.Empty;

        /// <summary>
        /// 获取或设置表格模板ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        private string m_szDocTypeName = string.Empty;

        /// <summary>
        /// 获取或设置表格模板类型
        /// </summary>
        public string DocTypeName
        {
            get { return this.m_szDocTypeName; }
            set { this.m_szDocTypeName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置用户病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置用户病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szCreatorId = string.Empty;

        /// <summary>
        /// 获取或设置创建模板者ID
        /// </summary>
        public string CreateID
        {
            get { return this.m_szCreatorId; }
            set { this.m_szCreatorId = value; }
        }

        private string m_szCreateName = string.Empty;

        /// <summary>
        /// 获取或设置创建模板者姓名
        /// </summary>
        public string CreateName
        {
            get { return this.m_szCreateName; }
            set { this.m_szCreateName = value; }
        }

        private DateTime m_dtCreateTime = DateTime.Now;

        /// <summary>
        /// 获取或设置创建模板时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private string m_szModifierId = string.Empty;

        /// <summary>
        /// 获取或设置修改者ID
        /// </summary>
        public string ModiferID
        {
            get { return this.m_szModifierId; }
            set { this.m_szModifierId = value; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// 获取或设置修改者姓名
        /// </summary>
        public string ModiferName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        private DateTime m_dtModifyTime = DateTime.Now;

        /// <summary>
        /// 获取或设置修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szVitalSQL = string.Empty;

        /// <summary>
        /// 获取或设置修改时间
        /// </summary>
        public string VitalSQL
        {
            get { return this.m_szVitalSQL; }
            set { this.m_szVitalSQL = value; }
        }

        public VitalViewSchema()
        {
            this.m_dtCreateTime = this.DefaultTime;
            this.m_dtModifyTime = this.DefaultTime;
        }

        public override object Clone()
        {
            VitalViewSchema schemaInfo = new VitalViewSchema();
            GlobalMethods.Reflect.CopyProperties(this, schemaInfo);
            return schemaInfo;
        }

        public string MakeSchemaID()
        {
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
            //return string.Format("{0}_{1}_{2}", this.m_szCreatorId, DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
        }
    }

    /// <summary>
    /// 表格视图模板信息
    /// </summary>
    public class GridViewSchema : DbTypeBase, ICloneable
    {
        private string m_szSchemaID = string.Empty;

        /// <summary>
        /// 获取或设置表格模板ID
        /// </summary>
        public string SchemaID
        {
            get { return this.m_szSchemaID; }
            set { this.m_szSchemaID = value; }
        }

        private string m_szSchemaType = string.Empty;

        /// <summary>
        /// 获取或设置表格模板类型
        /// </summary>
        public string SchemaType
        {
            get { return this.m_szSchemaType; }
            set { this.m_szSchemaType = value; }
        }

        private string m_szSchemaName = string.Empty;

        /// <summary>
        /// 获取或设置表格模板名称
        /// </summary>
        public string SchemaName
        {
            get { return this.m_szSchemaName; }
            set { this.m_szSchemaName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置用户病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置用户病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private bool m_bIsDefault = false;

        /// <summary>
        /// 获取或设置是否是默认的
        /// </summary>
        public bool IsDefault
        {
            get { return this.m_bIsDefault; }
            set { this.m_bIsDefault = value; }
        }

        private string m_szCreatorId = string.Empty;

        /// <summary>
        /// 获取或设置创建模板者ID
        /// </summary>
        public string CreateID
        {
            get { return this.m_szCreatorId; }
            set { this.m_szCreatorId = value; }
        }

        private string m_szCreateName = string.Empty;

        /// <summary>
        /// 获取或设置创建模板者姓名
        /// </summary>
        public string CreateName
        {
            get { return this.m_szCreateName; }
            set { this.m_szCreateName = value; }
        }

        private DateTime m_dtCreateTime = DateTime.Now;

        /// <summary>
        /// 获取或设置创建模板时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private string m_szModifierId = string.Empty;

        /// <summary>
        /// 获取或设置修改者ID
        /// </summary>
        public string ModiferID
        {
            get { return this.m_szModifierId; }
            set { this.m_szModifierId = value; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// 获取或设置修改者姓名
        /// </summary>
        public string ModiferName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        private DateTime m_dtModifyTime = DateTime.Now;

        /// <summary>
        /// 获取或设置修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szSchemaFlag = string.Empty;

        /// <summary>
        /// 获取或设置列配置标记
        /// </summary>
        public string SchemaFlag
        {
            get { return this.m_szSchemaFlag; }
            set { this.m_szSchemaFlag = value; }
        }

        private string m_szRelatvieSchemaId = string.Empty;

        /// <summary>
        /// 获取或设置关联列配置 用于批量入录保存护理记录单
        /// </summary>
        public string RelatvieSchemaId
        {
            get { return this.m_szRelatvieSchemaId; }
            set { this.m_szRelatvieSchemaId = value; }
        }


        public GridViewSchema()
        {
            this.m_dtCreateTime = this.DefaultTime;
            this.m_dtModifyTime = this.DefaultTime;
        }

        public override string ToString()
        {
            return this.m_szSchemaName;
        }

        public override object Clone()
        {
            GridViewSchema schemaInfo = new GridViewSchema();
            GlobalMethods.Reflect.CopyProperties(this, schemaInfo);
            return schemaInfo;
        }

        public string MakeSchemaID()
        {
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}{2}", this.m_szCreatorId, DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
        }
    }

    /// <summary>
    /// 表格视图模板列信息
    /// </summary>
    public class GridViewColumn : DbTypeBase, ICloneable
    {
        private string m_szSchemaID = string.Empty;

        /// <summary>
        /// 获取或设置关联的表格ID
        /// </summary>
        public string SchemaID
        {
            get { return this.m_szSchemaID; }
            set { this.m_szSchemaID = value; }
        }

        private string m_szColumnID = string.Empty;

        /// <summary>
        /// 获取或设置列ID
        /// </summary>
        public string ColumnID
        {
            get { return this.m_szColumnID; }
            set { this.m_szColumnID = value; }
        }

        private string m_szColumnName = string.Empty;

        /// <summary>
        /// 获取或设置列名称
        /// </summary>
        public string ColumnName
        {
            get { return this.m_szColumnName; }
            set { this.m_szColumnName = value; }
        }

        private string m_szColumnTag = string.Empty;

        /// <summary>
        /// 获取或设置列标识
        /// </summary>
        public string ColumnTag
        {
            get { return this.m_szColumnTag; }
            set { this.m_szColumnTag = value; }
        }

        private string m_szColumnGroup = string.Empty;

        /// <summary>
        /// 获取或设置列名称的多表头
        /// </summary>
        public string ColumnGroup
        {
            get { return this.m_szColumnGroup; }
            set { this.m_szColumnGroup = value; }
        }

        private int m_nColumnIndex = -1;

        /// <summary>
        /// 获取或设置列索引
        /// </summary>
        public int ColumnIndex
        {
            get { return this.m_nColumnIndex; }
            set { this.m_nColumnIndex = value; }
        }

        private string m_szColumnType = "字符";

        /// <summary>
        /// 获取或设置列列类型
        /// </summary>
        public string ColumnType
        {
            get { return this.m_szColumnType; }
            set { this.m_szColumnType = value; }
        }

        private int m_nColumnWidth = 100;

        /// <summary>
        /// 获取或设置列列宽度
        /// </summary>
        public int ColumnWidth
        {
            get { return this.m_nColumnWidth; }
            set { this.m_nColumnWidth = value; }
        }

        private string m_szColumnUnit = string.Empty;

        /// <summary>
        /// 获取或设置列单位
        /// </summary>
        public string ColumnUnit
        {
            get { return this.m_szColumnUnit; }
            set { this.m_szColumnUnit = value; }
        }

        private bool m_bIsMiddle = true;

        /// <summary>
        /// 获取或设置列是否显示
        /// </summary>
        public bool IsVisible
        {
            get { return this.m_bIsVisible; }
            set { this.m_bIsVisible = value; }
        }

        private bool m_bIsPrint = true;

        /// <summary>
        /// 获取或设置列是否打印
        /// </summary>
        public bool IsPrint
        {
            get { return this.m_bIsPrint; }
            set { this.m_bIsPrint = value; }
        }

        private bool m_bIsVisible = true;

        /// <summary>
        /// 获取或设置列数据是否居中
        /// </summary>
        public bool IsMiddle
        {
            get { return this.m_bIsMiddle; }
            set { this.m_bIsMiddle = value; }
        }

        private string m_szColumnItems = string.Empty;

        /// <summary>
        /// 获取或设置单元格值可选项列表
        /// </summary>
        public string ColumnItems
        {
            get { return this.m_szColumnItems; }
            set { this.m_szColumnItems = value; }
        }

        private string m_szDocTypeID = string.Empty;

        /// <summary>
        /// 获取或设置关联的文档ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        /// <summary>
        /// 不要修改ToString方法
        /// </summary>
        /// <returns>返回列名</returns>
        public override string ToString()
        {
            return this.m_szColumnName;
        }

        public override object Clone()
        {
            GridViewColumn columnInfo = new GridViewColumn();
            GlobalMethods.Reflect.CopyProperties(this, columnInfo);
            return columnInfo;
        }
    }

    /// <summary>
    /// 交班班次字典信息
    /// </summary>
    public class ShiftRankInfo : DbTypeBase, ICloneable
    {
        private string m_szRankCode = string.Empty;

        /// <summary>
        /// 获取或设置交班班次代码
        /// </summary>
        public string RankCode
        {
            get { return this.m_szRankCode; }
            set { this.m_szRankCode = value; }
        }

        private int m_nRankNo = 0;

        /// <summary>
        /// 获取或设置交班班次序号
        /// </summary>
        public int RankNo
        {
            get { return this.m_nRankNo; }
            set { this.m_nRankNo = value; }
        }

        private string m_szRankName = string.Empty;

        /// <summary>
        /// 获取或设置交班班次名称
        /// </summary>
        public string RankName
        {
            get { return this.m_szRankName; }
            set { this.m_szRankName = value; }
        }

        private string m_szStartPoint = string.Empty;

        /// <summary>
        /// 获取或设置交班班次起始时间点
        /// </summary>
        public string StartPoint
        {
            get { return this.m_szStartPoint; }
            set { this.m_szStartPoint = value; }
        }

        private string m_szEndPoint = string.Empty;

        /// <summary>
        /// 获取或设置交班班次截止时间点
        /// </summary>
        public string EndPoint
        {
            get { return this.m_szEndPoint; }
            set { this.m_szEndPoint = value; }
        }

        private DateTime m_dtStartTime = DateTime.Now;

        /// <summary>
        /// 获取或设置交班班次起始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return this.m_dtStartTime; }
            set { this.m_dtStartTime = value; }
        }

        private DateTime m_dtEndTime = DateTime.Now;

        /// <summary>
        /// 获取或设置交班班次截止时间
        /// </summary>
        public DateTime EndTime
        {
            get { return this.m_dtEndTime; }
            set { this.m_dtEndTime = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置科室代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置科室名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        public ShiftRankInfo()
        {
            this.m_dtStartTime = this.DefaultTime;
            this.m_dtEndTime = this.DefaultTime;
            this.m_szWardCode = "ALL";
            this.m_szWardName = "全院";
        }

        /// <summary>
        /// 不要修改ToString方法
        /// </summary>
        /// <returns>返回列名</returns>
        public override string ToString()
        {
            return this.m_szRankName;
        }

        public override object Clone()
        {
            ShiftRankInfo shiftRankInfo = new ShiftRankInfo();
            GlobalMethods.Reflect.CopyProperties(this, shiftRankInfo);
            return shiftRankInfo;
        }

        public string MakeRankCode()
        {
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return szRand;
        }

        /// <summary>
        /// 更新交班班次下拉列表中的班次时间
        /// </summary>
        /// <param name="dtShiftDate">交班日期</param>
        public void UpdateShiftRankTime(DateTime dtShiftDate)
        {
            string szStartPoint = string.Format("{0} {1}:00"
                , dtShiftDate.ToString("yyyy-MM-dd"), this.m_szStartPoint);
            this.m_dtStartTime = GlobalMethods.Convert.StringToValue(szStartPoint, DateTime.Now);

            string szEndPoint = string.Format("{0} {1}:00"
                , dtShiftDate.ToString("yyyy-MM-dd"), this.m_szEndPoint);
            this.m_dtEndTime = GlobalMethods.Convert.StringToValue(szEndPoint, DateTime.Now);

            if (this.m_dtEndTime <= this.m_dtStartTime)
                this.m_dtEndTime = this.m_dtEndTime.AddDays(1);
        }
    }

    /// <summary>
    /// 交班时病区状况信息
    /// </summary>
    public class ShiftWardStatus : DbTypeBase, ICloneable
    {
        private string m_szShiftRecordID = string.Empty;

        /// <summary>
        /// 获取或设置交班记录ID
        /// </summary>
        public string ShiftRecordID
        {
            get { return this.m_szShiftRecordID; }
            set { this.m_szShiftRecordID = value; }
        }

        private string m_szShiftItemName = string.Empty;

        /// <summary>
        /// 获取或设置交班项目名称
        /// </summary>
        public string ShiftItemName
        {
            get { return this.m_szShiftItemName; }
            set { this.m_szShiftItemName = value; }
        }

        private string m_szShiftItemCode = string.Empty;

        /// <summary>
        /// 获取或设置交班项目代码
        /// </summary>
        public string ShiftItemCode
        {
            get { return this.m_szShiftItemCode; }
            set { this.m_szShiftItemCode = value; }
        }

        private string m_szShiftItemDesc = string.Empty;

        /// <summary>
        /// 获取或设置交班项目描述
        /// </summary>
        public string ShiftItemDesc
        {
            get { return this.m_szShiftItemDesc; }
            set { this.m_szShiftItemDesc = value; }
        }

        public override object Clone()
        {
            ShiftWardStatus shiftWardStatus = new ShiftWardStatus();
            GlobalMethods.Reflect.CopyProperties(this, shiftWardStatus);
            return shiftWardStatus;
        }

        public override string ToString()
        {
            return string.Format("ItemName={0},ItemCode={1},ItemDesc={2}"
                , this.m_szShiftItemName, this.m_szShiftItemCode, this.m_szShiftItemDesc);
        }
    }

    /// <summary>
    /// 交班病人信息及内容
    /// </summary>
    public class ShiftPatient : DbTypeBase, ICloneable
    {
        private string m_szShiftRecordID = string.Empty;

        /// <summary>
        /// 获取或设置交班记录ID
        /// </summary>
        public string ShiftRecordID
        {
            get { return this.m_szShiftRecordID; }
            set { this.m_szShiftRecordID = value; }
        }

        private string m_szShiftItemName = string.Empty;

        /// <summary>
        /// 获取或设置交班项目名称
        /// </summary>
        public string ShiftItemName
        {
            get { return this.m_szShiftItemName; }
            set { this.m_szShiftItemName = value; }
        }

        private string m_szShiftItemAlias = string.Empty;

        /// <summary>
        /// 获取或设置交班项目别名
        /// </summary>
        public string ShiftItemAlias
        {
            get { return this.m_szShiftItemAlias; }
            set { this.m_szShiftItemAlias = value; }
        }

        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取或设置当前交班病人ID号
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private int m_szPatientNo = 0;

        /// <summary>
        /// 获取或设置当前交班病人序号
        /// </summary>
        public int PatientNo
        {
            get { return this.m_szPatientNo; }
            set { this.m_szPatientNo = value; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取或设置当前交班病人就诊号
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szSubID = string.Empty;

        /// <summary>
        /// 获取或设置当前交班病人子ID号
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
            set { this.m_szSubID = value; }
        }

        private string m_szPatientName = string.Empty;

        /// <summary>
        /// 获取或设置当前交班病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
            set { this.m_szPatientName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szBedCode = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人床号
        /// </summary>
        public string BedCode
        {
            get { return this.m_szBedCode; }
            set { this.m_szBedCode = value; }
        }

        private string m_szDiagnosis = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人诊断
        /// </summary>
        public string Diagnosis
        {
            get { return this.m_szDiagnosis; }
            set { this.m_szDiagnosis = value; }
        }

        private string m_szPatientAge = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人年龄
        /// </summary>
        public string PatientAge
        {
            get { return this.m_szPatientAge; }
            set { this.m_szPatientAge = value; }
        }

        private bool m_bShowValue = true;

        /// <summary>
        /// 是否显示体征信息
        /// </summary>
        public bool ShowValue
        {
            get { return this.m_bShowValue; }
            set { this.m_bShowValue = value; }
        }

        private float m_fTemperatureValue = -1;

        /// <summary>
        /// 获取或设置交班时病人体温
        /// </summary>
        public float TemperatureValue
        {
            get { return this.m_fTemperatureValue; }
            set { this.m_fTemperatureValue = value; }
        }

        private int m_nTemperatureType = 0;

        /// <summary>
        /// 获取或设置体温类型
        /// </summary>
        public int TemperatureType
        {
            get { return this.m_nTemperatureType; }
            set { this.m_nTemperatureType = value; }
        }

        private int m_nPulseValue = -1;

        /// <summary>
        /// 获取或设置交班时病人脉搏
        /// </summary>
        public int PulseValue
        {
            get { return this.m_nPulseValue; }
            set { this.m_nPulseValue = value; }
        }

        private int m_nBreathValue = -1;

        /// <summary>
        /// 获取或设置交班时病人呼吸
        /// </summary>
        public int BreathValue
        {
            get { return this.m_nBreathValue; }
            set { this.m_nBreathValue = value; }
        }

        private DateTime m_dtVitalTime = DateTime.Now;

        /// <summary>
        /// 获取或设置病人体征检测时间
        /// </summary>
        public DateTime VitalTime
        {
            get { return this.m_dtVitalTime; }
            set { this.m_dtVitalTime = value; }
        }

        private string m_szSpecialItem1 = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人特殊项1
        /// </summary>
        public string SpecialItem1
        {
            get { return this.m_szSpecialItem1; }
            set { this.m_szSpecialItem1 = value; }
        }

        private string m_szSpecialItem2 = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人特殊项2
        /// </summary>
        public string SpecialItem2
        {
            get { return this.m_szSpecialItem2; }
            set { this.m_szSpecialItem2 = value; }
        }

        private string m_szSpecialItem3 = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人特殊项3
        /// </summary>
        public string SpecialItem3
        {
            get { return this.m_szSpecialItem3; }
            set { this.m_szSpecialItem3 = value; }
        }

        private string m_szSpecialItem4 = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人特殊项4
        /// </summary>
        public string SpecialItem4
        {
            get { return this.m_szSpecialItem4; }
            set { this.m_szSpecialItem4 = value; }
        }

        private string m_szSpecialItem5 = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人特殊项5
        /// </summary>
        public string SpecialItem5
        {
            get { return this.m_szSpecialItem5; }
            set { this.m_szSpecialItem5 = value; }
        }

        private string m_szShiftContent = string.Empty;

        /// <summary>
        /// 获取或设置当前交班记录内容
        /// </summary>
        public string ShiftContent
        {
            get { return this.m_szShiftContent; }
            set { this.m_szShiftContent = value; }
        }

        private DateTime m_VisitTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置当前交班病人的入院时间
        /// </summary>
        public DateTime VisitTime
        {
            get { return this.m_VisitTime; }
            set { this.m_VisitTime = value; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// 获取或设置当前交班记录的修改人名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        private DateTime m_szShiftDate = DateTime.MinValue;

        /// <summary>
        /// 获取或设置当前交班记录的交班时间
        /// </summary>
        public DateTime ShiftDate
        {
            get { return this.m_szShiftDate; }
            set { this.m_szShiftDate = value; }
        }

        private string m_szBloodPressure = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人血压
        /// </summary>
        public string BloodPressure
        {
            get { return this.m_szBloodPressure; }
            set { this.m_szBloodPressure = value; }
        }

        private string m_szDiet = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人饮食
        /// </summary>
        public string Diet
        {
            get { return this.m_szDiet; }
            set { this.m_szDiet = value; }
        }

        private string m_szAdverseReaction = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人不良反应
        /// </summary>
        public string AdverseReaction
        {
            get { return this.m_szAdverseReaction; }
            set { this.m_szAdverseReaction = value; }
        }

        private string m_szRequestDoctor = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人的经管医生
        /// </summary>
        public string RequestDoctor
        {
            get { return this.m_szRequestDoctor; }
            set { this.m_szRequestDoctor = value; }
        }

        private string m_szParentDoctor = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人的上级医生
        /// </summary>
        public string ParentDoctor
        {
            get { return this.m_szParentDoctor; }
            set { this.m_szParentDoctor = value; }
        }

        public ShiftPatient()
        {
            this.m_dtVitalTime = base.DefaultTime;
        }

        public override object Clone()
        {
            ShiftPatient shiftPatient = new ShiftPatient();
            GlobalMethods.Reflect.CopyProperties(this, shiftPatient);
            return shiftPatient;
        }

        public override string ToString()
        {
            return string.Format("PatientID={0},VisitID={1},PatientName={2}"
                , this.m_szPatientID, this.m_szVisitID, this.m_szPatientName);
        }
    }

    /// <summary>
    /// 特殊病人病情交班信息及内容
    /// </summary>
    public class ShiftSpecialPatient : DbTypeBase, ICloneable
    {
        private string m_szShiftRecordID = string.Empty;

        /// <summary>
        /// 获取或设置交班记录ID
        /// </summary>
        public string ShiftRecordID
        {
            get { return this.m_szShiftRecordID; }
            set { this.m_szShiftRecordID = value; }
        }

        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取或设置当前交班病人ID号
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private int m_szPatientNo = 0;

        /// <summary>
        /// 获取或设置当前交班病人序号
        /// </summary>
        public int PatientNo
        {
            get { return this.m_szPatientNo; }
            set { this.m_szPatientNo = value; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取或设置当前交班病人就诊号
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szSubID = string.Empty;

        /// <summary>
        /// 获取或设置当前交班病人子ID号
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
            set { this.m_szSubID = value; }
        }

        private string m_szPatientName = string.Empty;

        /// <summary>
        /// 获取或设置当前交班病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
            set { this.m_szPatientName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szBedCode = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人床号
        /// </summary>
        public string BedCode
        {
            get { return this.m_szBedCode; }
            set { this.m_szBedCode = value; }
        }

        private string m_szDiagnosis = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人诊断
        /// </summary>
        public string Diagnosis
        {
            get { return this.m_szDiagnosis; }
            set { this.m_szDiagnosis = value; }
        }

        private string m_szPatientAge = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人年龄
        /// </summary>
        public string PatientAge
        {
            get { return this.m_szPatientAge; }
            set { this.m_szPatientAge = value; }
        }

        private string m_szPatientSex = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人性别
        /// </summary>
        public string PatientSex
        {
            get { return this.m_szPatientSex; }
            set { this.m_szPatientSex = value; }
        }

        private DateTime m_dtLogDateTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置病人入科时间
        /// </summary>
        public DateTime LogDateTime
        {
            get { return this.m_dtLogDateTime; }
            set { this.m_dtLogDateTime = value; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// 获取或设置当前交班记录的修改人名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        private DateTime m_dtShiftDate = DateTime.MinValue;

        /// <summary>
        /// 获取或设置当前交班记录的交班时间
        /// </summary>
        public DateTime ShiftDate
        {
            get { return this.m_dtShiftDate; }
            set { this.m_dtShiftDate = value; }
        }

        private string m_szNursingClass = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人护理
        /// </summary>
        public string NursingClass
        {
            get { return this.m_szNursingClass; }
            set { this.m_szNursingClass = value; }
        }

        private string m_szDiet = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人饮食
        /// </summary>
        public string Diet
        {
            get { return this.m_szDiet; }
            set { this.m_szDiet = value; }
        }

        private string m_szAllergicDrug = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人过敏药物
        /// </summary>
        public string AllergicDrug
        {
            get { return this.m_szAllergicDrug; }
            set { this.m_szAllergicDrug = value; }
        }

        private string m_szAdverseReactionDrug = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人不良反应药物
        /// </summary>
        public string AdverseReactionDrug
        {
            get { return this.m_szAdverseReactionDrug; }
            set { this.m_szAdverseReactionDrug = value; }
        }

        private string m_szOthersInfo = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人其他信息
        /// </summary>
        public string OthersInfo
        {
            get { return this.m_szOthersInfo; }
            set { this.m_szOthersInfo = value; }
        }

        private string m_szRemark = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人备注
        /// </summary>
        public string Remark
        {
            get { return this.m_szRemark; }
            set { this.m_szRemark = value; }
        }

        private string m_szRequestDoctor = string.Empty;

        /// <summary>
        /// 获取或设置交班时病人的经管医生
        /// </summary>
        public string RequestDoctor
        {
            get { return this.m_szRequestDoctor; }
            set { this.m_szRequestDoctor = value; }
        }

        public ShiftSpecialPatient()
        {

        }

        public override object Clone()
        {
            ShiftSpecialPatient shiftSpecialPatient = new ShiftSpecialPatient();
            GlobalMethods.Reflect.CopyProperties(this, shiftSpecialPatient);
            return shiftSpecialPatient;
        }

        public override string ToString()
        {
            return string.Format("PatientID={0},VisitID={1},PatientName={2}"
                , this.m_szPatientID, this.m_szVisitID, this.m_szPatientName);
        }
    }

    /// <summary>
    /// 交班主记录对象类
    /// </summary>
    public class NursingShiftInfo : DbTypeBase, ICloneable
    {
        private string m_szShiftRecordID = string.Empty;

        /// <summary>
        /// 获取或设置交班记录ID
        /// </summary>
        public string ShiftRecordID
        {
            get { return this.m_szShiftRecordID; }
            set { this.m_szShiftRecordID = value; }
        }

        private DateTime m_dtShiftRecordDate = DateTime.Now;

        /// <summary>
        /// 获取或设置交班记录日期
        /// </summary>
        public DateTime ShiftRecordDate
        {
            get { return this.m_dtShiftRecordDate; }
            set { this.m_dtShiftRecordDate = value; }
        }

        private DateTime m_dtShiftRecordTime = DateTime.Now;

        /// <summary>
        /// 获取或设置交班记录时间
        /// </summary>
        public DateTime ShiftRecordTime
        {
            get { return this.m_dtShiftRecordTime; }
            set { this.m_dtShiftRecordTime = value; }
        }

        private string m_szShiftRankCode = string.Empty;

        /// <summary>
        /// 获取或设置交班班次代码
        /// </summary>
        public string ShiftRankCode
        {
            get { return this.m_szShiftRankCode; }
            set { this.m_szShiftRankCode = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置交班病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置交班病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szCreatorID = string.Empty;

        /// <summary>
        /// 获取或设置创建者ID
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
            set { this.m_szCreatorID = value; }
        }

        private string m_szCreatorName = string.Empty;

        /// <summary>
        /// 获取或设置创建者姓名
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        private DateTime m_dtCreateTime = DateTime.Now;

        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private string m_szModifierID = string.Empty;

        /// <summary>
        /// 获取或设置修改者ID
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
            set { this.m_szModifierID = value; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// 获取或设置修改者姓名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        private DateTime m_dtModifyTime = DateTime.Now;

        /// <summary>
        /// 获取或设置修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szFirstSignID = string.Empty;

        /// <summary>
        /// 获取或设置交班第一签名人ID
        /// </summary>
        public string FirstSignID
        {
            get { return this.m_szFirstSignID; }
            set { this.m_szFirstSignID = value; }
        }

        private string m_szFirstSignName = string.Empty;

        /// <summary>
        /// 获取或设置交班第一签名人姓名
        /// </summary>
        public string FirstSignName
        {
            get { return this.m_szFirstSignName; }
            set { this.m_szFirstSignName = value; }
        }

        private DateTime m_dtFirstSignTime = DateTime.Now;

        /// <summary>
        /// 获取或设置交班第一签名时间
        /// </summary>
        public DateTime FirstSignTime
        {
            get { return this.m_dtFirstSignTime; }
            set { this.m_dtFirstSignTime = value; }
        }

        private string m_szSecondSignID = string.Empty;

        /// <summary>
        /// 获取或设置交班第二签名人ID
        /// </summary>
        public string SecondSignID
        {
            get { return this.m_szSecondSignID; }
            set { this.m_szSecondSignID = value; }
        }

        private string m_szSecondSignName = string.Empty;

        /// <summary>
        /// 获取或设置交班第二签名人姓名
        /// </summary>
        public string SecondSignName
        {
            get { return this.m_szSecondSignName; }
            set { this.m_szSecondSignName = value; }
        }

        private DateTime m_dtSecondSignTime = DateTime.Now;

        /// <summary>
        /// 获取或设置交班第二签名时间
        /// </summary>
        public DateTime SecondSignTime
        {
            get { return this.m_dtSecondSignTime; }
            set { this.m_dtSecondSignTime = value; }
        }

        public NursingShiftInfo()
        {
            this.m_dtShiftRecordDate = base.DefaultTime;
            this.m_dtShiftRecordTime = base.DefaultTime;
            this.m_dtCreateTime = base.DefaultTime;
            this.m_dtModifyTime = base.DefaultTime;
            this.m_dtFirstSignTime = base.DefaultTime;
            this.m_dtSecondSignTime = base.DefaultTime;
        }

        public override object Clone()
        {
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            GlobalMethods.Reflect.CopyProperties(this, nursingShiftInfo);
            return nursingShiftInfo;
        }

        public override string ToString()
        {
            return string.Format("ShiftID={0},ShiftDate={1},ShiftRank={2}"
                , this.m_szShiftRecordID, this.m_dtShiftRecordDate, this.m_szShiftRankCode);
        }

        public string MakeRecordID()
        {
            return MakeRecordID(DateTime.Now, 0);
        }

        public string MakeRecordID(DateTime serverDateTime, int iSRSelectedIndex)
        {
            if (this.m_szCreatorID == null)
                this.m_szCreatorID = string.Empty;
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}{2}{3}", this.m_szCreatorID.ToUpper()
                , iSRSelectedIndex, serverDateTime.ToString("yyyyMMddHHmmss").Substring(0, 13), szRand);
        }
    }

    /// <summary>
    /// 特殊病人病情交接对象类
    /// </summary>
    public class SpecialShiftInfo : DbTypeBase, ICloneable
    {
        private string m_szShiftRecordID = string.Empty;

        /// <summary>
        /// 获取或设置交班记录ID
        /// </summary>
        public string ShiftRecordID
        {
            get { return this.m_szShiftRecordID; }
            set { this.m_szShiftRecordID = value; }
        }

        private DateTime m_dtShiftRecordDate = DateTime.Now;

        /// <summary>
        /// 获取或设置交班记录日期
        /// </summary>
        public DateTime ShiftRecordDate
        {
            get { return this.m_dtShiftRecordDate; }
            set { this.m_dtShiftRecordDate = value; }
        }

        private DateTime m_dtShiftRecordTime = DateTime.Now;

        /// <summary>
        /// 获取或设置交班记录时间
        /// </summary>
        public DateTime ShiftRecordTime
        {
            get { return this.m_dtShiftRecordTime; }
            set { this.m_dtShiftRecordTime = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置交班病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置交班病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szCreatorID = string.Empty;

        /// <summary>
        /// 获取或设置创建者ID
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
            set { this.m_szCreatorID = value; }
        }

        private string m_szCreatorName = string.Empty;

        /// <summary>
        /// 获取或设置创建者姓名
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        private DateTime m_dtCreateTime = DateTime.Now;

        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private string m_szModifierID = string.Empty;

        /// <summary>
        /// 获取或设置修改者ID
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
            set { this.m_szModifierID = value; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// 获取或设置修改者姓名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        private DateTime m_dtModifyTime = DateTime.Now;

        /// <summary>
        /// 获取或设置修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szFirstSignID = string.Empty;

        /// <summary>
        /// 获取或设置交班第一签名人ID
        /// </summary>
        public string FirstSignID
        {
            get { return this.m_szFirstSignID; }
            set { this.m_szFirstSignID = value; }
        }

        private string m_szFirstSignName = string.Empty;

        /// <summary>
        /// 获取或设置交班第一签名人姓名
        /// </summary>
        public string FirstSignName
        {
            get { return this.m_szFirstSignName; }
            set { this.m_szFirstSignName = value; }
        }

        private DateTime m_dtFirstSignTime = DateTime.Now;

        /// <summary>
        /// 获取或设置交班第一签名时间
        /// </summary>
        public DateTime FirstSignTime
        {
            get { return this.m_dtFirstSignTime; }
            set { this.m_dtFirstSignTime = value; }
        }

        private string m_szSecondSignID = string.Empty;

        /// <summary>
        /// 获取或设置交班第二签名人ID
        /// </summary>
        public string SecondSignID
        {
            get { return this.m_szSecondSignID; }
            set { this.m_szSecondSignID = value; }
        }

        private string m_szSecondSignName = string.Empty;

        /// <summary>
        /// 获取或设置交班第二签名人姓名
        /// </summary>
        public string SecondSignName
        {
            get { return this.m_szSecondSignName; }
            set { this.m_szSecondSignName = value; }
        }

        private DateTime m_dtSecondSignTime = DateTime.Now;

        /// <summary>
        /// 获取或设置交班第二签名时间
        /// </summary>
        public DateTime SecondSignTime
        {
            get { return this.m_dtSecondSignTime; }
            set { this.m_dtSecondSignTime = value; }
        }

        public SpecialShiftInfo()
        {
            this.m_dtShiftRecordDate = base.DefaultTime;
            this.m_dtShiftRecordTime = base.DefaultTime;
            this.m_dtCreateTime = base.DefaultTime;
            this.m_dtModifyTime = base.DefaultTime;
            this.m_dtFirstSignTime = base.DefaultTime;
            this.m_dtSecondSignTime = base.DefaultTime;
        }

        public override object Clone()
        {
            SpecialShiftInfo specialShiftInfo = new SpecialShiftInfo();
            GlobalMethods.Reflect.CopyProperties(this, specialShiftInfo);
            return specialShiftInfo;
        }

        public override string ToString()
        {
            return string.Format("ShiftID={0},ShiftDate={1},ShiftRank=-1"
                , this.m_szShiftRecordID, this.m_dtShiftRecordDate);
        }

        public string MakeRecordID(DateTime serverDateTime)
        {
            if (this.m_szCreatorID == null)
                this.m_szCreatorID = string.Empty;
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}{2}", this.m_szCreatorID.ToUpper()
                , serverDateTime.ToString("yyyyMMddHHmmss").Substring(0, 13), szRand);
        }
    }

    /// <summary>
    /// 交班动态配置
    /// </summary>
    public class ShiftConfigInfo : DbTypeBase, ICloneable
    {
        private string m_szItemCode = string.Empty;

        /// <summary>
        /// 获取或设置交班配置代码
        /// </summary>
        public string ItemCode
        {
            get { return this.m_szItemCode; }
            set { this.m_szItemCode = value; }
        }

        private int m_iItemNo = 0;

        /// <summary>
        /// 获取或设置交班配置序号
        /// </summary>
        public int ItemNo
        {
            get { return this.m_iItemNo; }
            set { this.m_iItemNo = value; }
        }

        private string m_szItemName = string.Empty;

        /// <summary>
        /// 获取或设置交班配置名称
        /// </summary>
        public string ItemName
        {
            get { return this.m_szItemName; }
            set { this.m_szItemName = value; }
        }

        private int m_iItemWidth = 80;

        /// <summary>
        /// 获取或设置交班配置显示宽度
        /// </summary>
        public int ItemWidth
        {
            get { return this.m_iItemWidth; }
            set { this.m_iItemWidth = value; }
        }

        private bool m_bVisible = true;

        /// <summary>
        /// 获取或设置交班配置是否显示
        /// </summary>
        public bool Visible
        {
            get { return this.m_bVisible; }
            set { this.m_bVisible = value; }
        }

        private bool m_bMiddle = true;

        /// <summary>
        /// 获取或设置交班配置是否居中显示
        /// </summary>
        public bool Middle
        {
            get { return this.m_bMiddle; }
            set { this.m_bMiddle = value; }
        }

        private bool m_bImportant = false;

        /// <summary>
        /// 获取或设置交班配置是否为重要配置
        /// </summary>
        public bool Important
        {
            get { return this.m_bImportant; }
            set { this.m_bImportant = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置交班配置病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置交班配置病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        public override object Clone()
        {
            ShiftConfigInfo shiftConfigInfo = new ShiftConfigInfo();
            GlobalMethods.Reflect.CopyProperties(this, shiftConfigInfo);
            return shiftConfigInfo;
        }

        public string MakeItemCode()
        {
            Random rand = new Random();
            string szRand = rand.Next(100000, 999999).ToString().PadLeft(6, '0');
            return string.Concat("SC", szRand);
        }
    }

    /// <summary>
    /// 交班项目配置别名
    /// </summary>
    public class ShiftItemAliasInfo : DbTypeBase, ICloneable
    {
        private string m_szItemAlias = string.Empty;

        /// <summary>
        /// 获取或设置交班项目配置别名
        /// </summary>
        public string ItemAlias
        {
            get { return this.m_szItemAlias; }
            set { this.m_szItemAlias = value; }
        }

        private string m_szItemAliasCode = string.Empty;

        /// <summary>
        /// 获取或设置交班项目配置别名代码
        /// </summary>
        public string ItemAliasCode
        {
            get { return this.m_szItemAliasCode; }
            set { this.m_szItemAliasCode = value; }
        }

        private string m_iItemName = string.Empty;

        /// <summary>
        /// 获取或设置交班项目配置名称
        /// </summary>
        public string ItemName
        {
            get { return this.m_iItemName; }
            set { this.m_iItemName = value; }
        }

        private string m_szItemCode = string.Empty;

        /// <summary>
        /// 获取或设置交班项目配置名称代码
        /// </summary>
        public string ItemCode
        {
            get { return this.m_szItemCode; }
            set { this.m_szItemCode = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置交班配置病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置交班配置病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        public string MakeAliasCode()
        {
            Random rand = new Random();
            string szRand = rand.Next(9999, 99999).ToString();
            return string.Format("AC{0}", szRand);
        }

        public override object Clone()
        {
            ShiftItemAliasInfo shiftItemAliasInfo = new ShiftItemAliasInfo();
            GlobalMethods.Reflect.CopyProperties(this, shiftItemAliasInfo);
            return shiftItemAliasInfo;
        }
    }

    /// <summary>
    /// 指控信息类
    /// </summary>
    public class QCExamineInfo : DbTypeBase, ICloneable
    {
        private string m_szQcContentKey = string.Empty;

        /// <summary>
        /// 获取或设置审核内容标识
        /// </summary>
        public string QcContentKey
        {
            get { return this.m_szQcContentKey; }
            set { this.m_szQcContentKey = value; }
        }

        private string m_szQcContentType = string.Empty;

        /// <summary>
        /// 获取或设置审核内容类型
        /// </summary>
        public string QcContentType
        {
            get { return this.m_szQcContentType; }
            set { this.m_szQcContentType = value; }
        }

        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取或设置病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szPatientName = string.Empty;

        /// <summary>
        /// 获取或设置病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
            set { this.m_szPatientName = value; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取或设置病人VID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szQcExamineStatus = string.Empty;

        /// <summary>
        /// 获取或设审核状态
        /// </summary>
        public string QcExamineStatus
        {
            get { return this.m_szQcExamineStatus; }
            set { this.m_szQcExamineStatus = value; }
        }

        private string m_szQcExamineContent = string.Empty;

        /// <summary>
        /// 获取或设置审核内容
        /// </summary>
        public string QcExamineContent
        {
            get { return this.m_szQcExamineContent; }
            set { this.m_szQcExamineContent = value; }
        }

        private string m_szQcExamineID = string.Empty;

        /// <summary>
        /// 获取或设置审核人ID
        /// </summary>
        public string QcExamineID
        {
            get { return this.m_szQcExamineID; }
            set { this.m_szQcExamineID = value; }
        }

        private string m_szQcExamineName = string.Empty;

        /// <summary>
        /// 获取或设置审核人姓名
        /// </summary>
        public string QcExamineName
        {
            get { return this.m_szQcExamineName; }
            set { this.m_szQcExamineName = value; }
        }

        private DateTime m_szQcExamineTime = new DateTime();

        /// <summary>
        /// 获取或设置审核时间
        /// </summary>
        public DateTime QcExamineTime
        {
            get { return this.m_szQcExamineTime; }
            set { this.m_szQcExamineTime = value; }
        }

        public override object Clone()
        {
            QCExamineInfo qcExamineInfo = new QCExamineInfo();
            GlobalMethods.Reflect.CopyProperties(this, qcExamineInfo);
            return qcExamineInfo;
        }
    }

    /// <summary>
    /// 操作审批信息
    /// </summary>
    public class OperationApplyInfo : DbTypeBase, ICloneable
    {
        private string m_szRequesterID = string.Empty;

        /// <summary>
        /// 获取或设置申请人ID
        /// </summary>
        public string RequesterID
        {
            get { return this.m_szRequesterID; }
            set { this.m_szRequesterID = value; }
        }

        private string m_szRequesterName = string.Empty;

        /// <summary>
        /// 获取或设置申请人姓名
        /// </summary>
        public string RequesterName
        {
            get { return this.m_szRequesterName; }
            set { this.m_szRequesterName = value; }
        }

        private DateTime m_dtRequestTime;

        /// <summary>
        /// 获取或设置申请时间
        /// </summary>
        public DateTime RequestTime
        {
            get { return this.m_dtRequestTime; }
            set { this.m_dtRequestTime = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置申请人病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置申请人病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取或设置病人ID号
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szPatientName = string.Empty;

        /// <summary>
        /// 获取或设置病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
            set { this.m_szPatientName = value; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取或设置就诊ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szModuleCode = string.Empty;

        /// <summary>
        /// 获取或设置模块代码
        /// </summary>
        public string ModuleCode
        {
            get { return this.m_szModuleCode; }
            set { this.m_szModuleCode = value; }
        }

        private string m_szModuleName = string.Empty;

        /// <summary>
        /// 获取或设置模块代码
        /// </summary>
        public string ModuleName
        {
            get { return this.m_szModuleName; }
            set { this.m_szModuleName = value; }
        }

        private string m_szAssignData = string.Empty;

        /// <summary>
        /// 获取或设置关联数据
        /// </summary>
        public string AssignData
        {
            get { return this.m_szAssignData; }
            set { this.m_szAssignData = value; }
        }

        private string m_szOperationType = string.Empty;

        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        public string OperationType
        {
            get { return this.m_szOperationType; }
            set { this.m_szOperationType = value; }
        }

        private string m_szRequestDesc = string.Empty;

        /// <summary>
        /// 获取或设置申请说明
        /// </summary>
        public string RequestDesc
        {
            get { return this.m_szRequestDesc; }
            set { this.m_szRequestDesc = value; }
        }

        private string m_szApproverLevel = string.Empty;

        /// <summary>
        /// 获取或设置需要的审批人级别
        /// </summary>
        public string ApproverLevel
        {
            get { return this.m_szApproverLevel; }
            set { this.m_szApproverLevel = value; }
        }

        private string m_szApproverID = string.Empty;

        /// <summary>
        /// 获取或设置需要的审批人ID
        /// </summary>
        public string ApproverID
        {
            get { return this.m_szApproverID; }
            set { this.m_szApproverID = value; }
        }

        private string m_szApproverName = string.Empty;

        /// <summary>
        /// 获取或设置需要的审批人姓名
        /// </summary>
        public string ApproverName
        {
            get { return this.m_szApproverName; }
            set { this.m_szApproverName = value; }
        }

        private DateTime m_dtApproveTime;

        /// <summary>
        /// 获取或设置需要的审批时间
        /// </summary>
        public DateTime ApproveTime
        {
            get { return this.m_dtApproveTime; }
            set { this.m_dtApproveTime = value; }
        }

        private string m_szApproveState = string.Empty;

        /// <summary>
        /// 获取或设置需要的审批状态
        /// </summary>
        public string ApproveState
        {
            get { return this.m_szApproveState; }
            set { this.m_szApproveState = value; }
        }

        private string m_szApproveDesc = string.Empty;

        /// <summary>
        /// 获取或设置需要的审批说明
        /// </summary>
        public string ApproveDesc
        {
            get { return this.m_szApproveDesc; }
            set { this.m_szApproveDesc = value; }
        }

        public OperationApplyInfo()
        {
            this.m_dtRequestTime = this.DefaultTime;
            this.m_dtApproveTime = this.DefaultTime;
        }

        public override string ToString()
        {
            return this.m_szRequestDesc;
        }

        public override object Clone()
        {
            OperationApplyInfo operationApplyInfo = new OperationApplyInfo();
            GlobalMethods.Reflect.CopyProperties(this, operationApplyInfo);
            return operationApplyInfo;
        }
    }

    /// <summary>
    /// 护理记录信息
    /// </summary>
    public class NursingRecInfo : DbTypeBase, ICloneable
    {
        private string m_szPatientID = string.Empty;

        /// <summary>
        /// 获取或设置病人ID号
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        private string m_szVisitID = string.Empty;

        /// <summary>
        /// 获取或设置就诊ID
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        private string m_szSubID = string.Empty;

        /// <summary>
        /// 获取或设置病人子ID
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
            set { this.m_szSubID = value; }
        }

        private string m_szRecordID = string.Empty;

        /// <summary>
        /// 获取或设置记录ID号
        /// </summary>
        public string RecordID
        {
            get { return this.m_szRecordID; }
            set { this.m_szRecordID = value; }
        }

        private DateTime m_dtRecordDate = DateTime.Now.Date;

        /// <summary>
        /// 获取或设置录入日期
        /// </summary>
        public DateTime RecordDate
        {
            get { return this.m_dtRecordDate; }
            set { this.m_dtRecordDate = value; }
        }

        private DateTime m_dtRecordTime = DateTime.Now;

        /// <summary>
        /// 获取或设置录入时间
        /// </summary>
        public DateTime RecordTime
        {
            get { return this.m_dtRecordTime; }
            set { this.m_dtRecordTime = value; }
        }

        private string m_szPatientName = string.Empty;

        /// <summary>
        /// 获取或设置病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
            set { this.m_szPatientName = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置病人病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置病人病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szCreatorID = string.Empty;

        /// <summary>
        /// 获取或设置记录创建者ID
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
            set { this.m_szCreatorID = value; }
        }

        private string m_szCreatorName = string.Empty;

        /// <summary>
        /// 获取或设置记录创建者姓名
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        private DateTime m_dtCreateTime = DateTime.Now;

        /// <summary>
        /// 获取或设置记录创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private string m_szModifierID = string.Empty;

        /// <summary>
        /// 获取或设置记录修改者ID
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
            set { this.m_szModifierID = value; }
        }

        private string m_szModifierName = string.Empty;

        /// <summary>
        /// 获取或设置记录修改者姓名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        private DateTime m_dtModifyTime = DateTime.Now;

        /// <summary>
        /// 获取或设置记录修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private int m_nSummaryFlag = 0;

        /// <summary>
        /// 获取或设置小结标记
        /// </summary>
        public int SummaryFlag
        {
            get { return this.m_nSummaryFlag; }
            set { this.m_nSummaryFlag = value; }
        }

        private string m_szSummaryName = string.Empty;

        /// <summary>
        /// 获取或设置小结名称
        /// </summary>
        public string SummaryName
        {
            get { return this.m_szSummaryName; }
            set { this.m_szSummaryName = value; }
        }

        private DateTime m_dtSummaryStartTime;

        /// <summary>
        /// 获取或设置小结起始时间
        /// </summary>
        public DateTime SummaryStartTime
        {
            get { return this.m_dtSummaryStartTime; }
            set { this.m_dtSummaryStartTime = value; }
        }

        private string m_szRecorder1ID = string.Empty;

        /// <summary>
        /// 获取或设置记录者1ID
        /// </summary>
        public string Recorder1ID
        {
            get { return this.m_szRecorder1ID; }
            set { this.m_szRecorder1ID = value; }
        }

        private string m_szRecorder1Name = string.Empty;

        /// <summary>
        /// 获取或设置记录者1姓名
        /// </summary>
        public string Recorder1Name
        {
            get { return this.m_szRecorder1Name; }
            set { this.m_szRecorder1Name = value; }
        }

        private string m_szRecorder2ID = string.Empty;

        /// <summary>
        /// 获取或设置记录者2ID
        /// </summary>
        public string Recorder2ID
        {
            get { return this.m_szRecorder2ID; }
            set { this.m_szRecorder2ID = value; }
        }

        private string m_szRecorder2Name = string.Empty;

        /// <summary>
        /// 获取或设置记录者2姓名
        /// </summary>
        public string Recorder2Name
        {
            get { return this.m_szRecorder2Name; }
            set { this.m_szRecorder2Name = value; }
        }

        private string m_szRecordContent = string.Empty;

        /// <summary>
        /// 获取或设置自动生成的记录内容
        /// </summary>
        public string RecordContent
        {
            get { return this.m_szRecordContent; }
            set { this.m_szRecordContent = value; }
        }

        private string m_szRecordRemarks = string.Empty;

        /// <summary>
        /// 获取或设置自由编辑的记录内容
        /// </summary>
        public string RecordRemarks
        {
            get { return this.m_szRecordRemarks; }
            set { this.m_szRecordRemarks = value; }
        }

        private string m_szSchemaID = string.Empty;

        /// <summary>
        /// 护理记录列显示方案ID
        /// </summary>
        public string SchemaID
        {
            get { return this.m_szSchemaID; }
            set { this.m_szSchemaID = value; }
        }

        private int m_szRecordPrinted = 0;

        /// <summary>
        /// 获取或设置记录的打印信息
        /// </summary>
        public int RecordPrinted
        {
            get { return this.m_szRecordPrinted; }
            set { this.m_szRecordPrinted = value; }
        }

        public NursingRecInfo()
        {
            this.m_dtRecordDate = this.DefaultTime;
            this.m_dtRecordTime = this.DefaultTime;
            this.m_dtCreateTime = this.DefaultTime;
            this.m_dtModifyTime = this.DefaultTime;
            this.m_dtSummaryStartTime = this.DefaultTime;
        }

        public override string ToString()
        {
            return this.m_dtRecordTime.ToString();
        }

        public override object Clone()
        {
            NursingRecInfo nursingRecInfo = new NursingRecInfo();
            GlobalMethods.Reflect.CopyProperties(this, nursingRecInfo);
            return nursingRecInfo;
        }

        public string MakeRecordID()
        {
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}_{2}{3}", this.m_szPatientID, this.m_szVisitID, DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
        }
    }

    /// <summary>
    /// 文本模板信息类
    /// </summary>
    /// <remarks></remarks>
    public class TextTempletInfo : DbTypeBase, ICloneable
    {
        private string m_szTempletID = string.Empty;

        /// <summary>
        /// 获取或设置模板编号
        /// </summary>
        public string TempletID
        {
            get { return this.m_szTempletID; }
            set { this.m_szTempletID = value; }
        }

        private string m_szTempletName = string.Empty;

        /// <summary>
        /// 获取或设置模板类型描述，用户自定义输入内容
        /// </summary>
        public string TempletName
        {
            get { return this.m_szTempletName; }
            set { this.m_szTempletName = value; }
        }

        private string m_szShareLevel = string.Empty;

        /// <summary>
        /// 获取或设置模板的共享水平，P-个人，D-科室，H-公用等
        /// </summary>
        public string ShareLevel
        {
            get { return this.m_szShareLevel; }
            set { this.m_szShareLevel = value; }
        }

        private string m_szCreatorID = string.Empty;

        /// <summary>
        /// 获取或设置模板的创建者编号
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
            set { this.m_szCreatorID = value; }
        }

        private string m_szCreatorName = string.Empty;

        /// <summary>
        /// 获取或设置模板的创建者名称
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        private DateTime m_dtCreateTime;

        /// <summary>
        /// 获取或设置模板文件的创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private DateTime m_dtModifyTime;

        /// <summary>
        /// 获取或设置模板的最后更新时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置所属病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置所属病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szParentID = string.Empty;

        /// <summary>
        /// 获取或设置当前模板或者文件夹的父节点ID
        /// </summary>
        public string ParentID
        {
            get { return this.m_szParentID; }
            set { this.m_szParentID = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置当前记录是否为文件夹
        /// </summary>
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private string m_szTempletData = string.Empty;

        /// <summary>
        /// 获取或设置模板内容
        /// </summary>
        public string TempletData
        {
            get { return this.m_szTempletData; }
            set { this.m_szTempletData = value; }
        }

        public TextTempletInfo()
        {
            this.m_dtCreateTime = this.DefaultTime;
            this.m_dtModifyTime = this.DefaultTime;
        }

        public override string ToString()
        {
            return this.m_szTempletName;
        }

        public override object Clone()
        {
            TextTempletInfo textTempletInfo = new TextTempletInfo();
            GlobalMethods.Reflect.CopyProperties(this, textTempletInfo);
            return textTempletInfo;
        }

        public string MakeTempletID()
        {
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}{2}", this.m_szCreatorID, DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
        }
    }

    /// <summary>
    /// word模板信息类
    /// </summary>
    /// <remarks></remarks>
    public class WordTempInfo : DbTypeBase, ICloneable
    {
        private string m_szTempletID = string.Empty;

        /// <summary>
        /// 获取或设置模板编号
        /// </summary>
        public string TempletID
        {
            get { return this.m_szTempletID; }
            set { this.m_szTempletID = value; }
        }

        private string m_szTempletName = string.Empty;

        /// <summary>
        /// 获取或设置模板类型描述，用户自定义输入内容
        /// </summary>
        public string TempletName
        {
            get { return this.m_szTempletName; }
            set { this.m_szTempletName = value; }
        }

        private string m_szShareLevel = string.Empty;

        /// <summary>
        /// 获取或设置模板的共享水平，P-个人，D-科室，H-公用等
        /// </summary>
        public string ShareLevel
        {
            get { return this.m_szShareLevel; }
            set { this.m_szShareLevel = value; }
        }

        private string m_szCreatorID = string.Empty;

        /// <summary>
        /// 获取或设置模板的创建者编号
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
            set { this.m_szCreatorID = value; }
        }

        private string m_szCreatorName = string.Empty;

        /// <summary>
        /// 获取或设置模板的创建者名称
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        private DateTime m_dtCreateTime;

        /// <summary>
        /// 获取或设置模板文件的创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private DateTime m_dtModifyTime;

        /// <summary>
        /// 获取或设置模板的最后更新时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置所属病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置所属病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szParentID = string.Empty;

        /// <summary>
        /// 获取或设置当前模板或者文件夹的父节点ID
        /// </summary>
        public string ParentID
        {
            get { return this.m_szParentID; }
            set { this.m_szParentID = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置当前记录是否为文件夹
        /// </summary>
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private string m_szTempletData = string.Empty;

        /// <summary>
        /// 获取或设置模板内容
        /// </summary>
        public string TempletData
        {
            get { return this.m_szTempletData; }
            set { this.m_szTempletData = value; }
        }

        public WordTempInfo()
        {
            this.m_dtCreateTime = this.DefaultTime;
            this.m_dtModifyTime = this.DefaultTime;
        }

        public override string ToString()
        {
            return this.m_szTempletName;
        }

        public override object Clone()
        {
            WordTempInfo WordTempInfo = new WordTempInfo();
            GlobalMethods.Reflect.CopyProperties(this, WordTempInfo);
            return WordTempInfo;
        }

        public string MakeTempletID()
        {
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}{2}", this.m_szCreatorID, DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
        }
    }

    /// <summary>
    /// 护理信息库类
    /// </summary>
    /// <remarks></remarks>
    public class InfoLibInfo : DbTypeBase, ICloneable
    {
        private string m_szInfoID = string.Empty;

        /// <summary>
        /// 获取或设置信息编号
        /// </summary>
        public string InfoID
        {
            get { return this.m_szInfoID; }
            set { this.m_szInfoID = value; }
        }

        private string m_szInfoName = string.Empty;

        /// <summary>
        /// 获取或设置信息描述，用户自定义输入内容
        /// </summary>
        public string InfoName
        {
            get { return this.m_szInfoName; }
            set { this.m_szInfoName = value; }
        }

        private string m_szShareLevel = string.Empty;

        /// <summary>
        /// 获取或设置模板的共享水平，P-个人，D-科室，H-公用等
        /// </summary>
        public string ShareLevel
        {
            get { return this.m_szShareLevel; }
            set { this.m_szShareLevel = value; }
        }

        private string m_szCreatorID = string.Empty;

        /// <summary>
        /// 获取或设置模板的创建者编号
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
            set { this.m_szCreatorID = value; }
        }

        private string m_szCreatorName = string.Empty;

        /// <summary>
        /// 获取或设置模板的创建者名称
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        private DateTime m_dtCreateTime;

        /// <summary>
        /// 获取或设置模板文件的创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private DateTime m_dtModifyTime;

        /// <summary>
        /// 获取或设置模板的最后更新时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szWardCode = string.Empty;

        /// <summary>
        /// 获取或设置所属病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        private string m_szWardName = string.Empty;

        /// <summary>
        /// 获取或设置所属病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        private string m_szParentID = string.Empty;

        /// <summary>
        /// 获取或设置当前模板或者文件夹的父节点ID
        /// </summary>
        public string ParentID
        {
            get { return this.m_szParentID; }
            set { this.m_szParentID = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置当前记录是否为文件夹
        /// </summary>
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private string m_szInfoData = string.Empty;

        /// <summary>
        /// 获取或设置护理信息内容
        /// </summary>
        public string InfoData
        {
            get { return this.m_szInfoData; }
            set { this.m_szInfoData = value; }
        }

        private string m_szStatus = string.Empty;

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get { return this.m_szStatus; }
            set { this.m_szStatus = value; }
        }

        private string m_szInfoType = string.Empty;

        /// <summary>
        /// 护理信息文件类型
        /// </summary>
        public string InfoType
        {
            get { return this.m_szInfoType; }
            set { this.m_szInfoType = value; }
        }

        private string m_szInfoSize = string.Empty;

        /// <summary>
        /// 护理信息文件大小
        /// </summary>
        public string InfoSize
        {
            get { return this.m_szInfoSize; }
            set { this.m_szInfoSize = value; }
        }

        public InfoLibInfo()
        {
            this.m_dtCreateTime = this.DefaultTime;
            this.m_dtModifyTime = this.DefaultTime;
        }

        public override string ToString()
        {
            return this.m_szInfoName;
        }

        public override object Clone()
        {
            InfoLibInfo infoLibInfo = new InfoLibInfo();
            GlobalMethods.Reflect.CopyProperties(this, infoLibInfo);
            return infoLibInfo;
        }

        public string MakeInfoID()
        {
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}{2}", this.m_szCreatorID, DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
        }
    }

    /// <summary>
    /// 护理申请信息类
    /// </summary>
    public class NurApplyInfo : DbTypeBase, ICloneable
    {
        private string m_szApplyID = string.Empty;            //申请者ID
        private string m_szDocID = string.Empty;            //文档编号
        private string m_szDocTypeID = string.Empty;        //文档类型编号
        private string m_szApplyType = string.Empty;        //申请单类型
        private string m_szApplyName = string.Empty;        //申请单名称
        private int m_nUrgency = 0;                         //紧急程度
        private string m_szPatientID = string.Empty;         //病人ID
        private string m_szPatientName = string.Empty;     //病人姓名
        private string m_szVisitID = string.Empty;          //病人住院标记
        private string m_szSubID = string.Empty;            //病人子ID
        private string m_szWardCode = string.Empty;         //病人所在护理单元编号
        private string m_szWardName = string.Empty;          //病人所在护理单元名称
        private string m_szApplicantID = string.Empty;      //申请者ID
        private string m_szApplicantName = string.Empty;    //申请者姓名
        private DateTime m_dtApplyTime = DateTime.Now;     //申请时间
        private string m_szStatus = string.Empty;           //申请单状态
        private string m_szModifierID = string.Empty;        //申请单修改者ID
        private string m_szModifierName = string.Empty;     //申请单修改者姓名
        private DateTime m_dtModifyTime = DateTime.Now;       //申请单修改时间

        /// <summary>
        /// 护理申请信息类
        /// </summary>
        /// <remarks></remarks>
        public NurApplyInfo()
        {
            this.Initialize();
        }

        /// <summary>
        /// 护理申请信息类成员初始化过程(设置其默认值)
        /// </summary>
        /// <remarks></remarks>
        public void Initialize()
        {
            this.m_dtApplyTime = this.DefaultTime;
            this.m_dtModifyTime = this.DefaultTime;
            this.m_nUrgency = 0;
            this.m_szApplicantID = string.Empty;
            this.m_szApplicantName = string.Empty;
            this.m_szApplyID = string.Empty;
            this.m_szApplyName = string.Empty;
            this.m_szApplyType = string.Empty;
            this.m_szDocID = string.Empty;
            this.m_szDocTypeID = string.Empty;
            this.m_szModifierID = string.Empty;
            this.m_szModifierName = string.Empty;
            this.m_szPatientID = string.Empty;
            this.m_szPatientName = string.Empty;
            this.m_szStatus = string.Empty;
            this.m_szSubID = string.Empty;
            this.m_szVisitID = string.Empty;
            this.m_szWardCode = string.Empty;
            this.m_szWardName = string.Empty;
        }

        /// <summary>
        /// 获取或设置申请单ID
        /// </summary>
        public string ApplyID
        {
            get { return this.m_szApplyID; }
            set { this.m_szApplyID = value; }
        }

        /// <summary>
        /// 获取或设置文档ID
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
            set { this.m_szDocID = value; }
        }

        /// <summary>
        /// 获取或设置文档类型ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        /// <summary>
        /// 获取或设置申请单类型
        /// </summary>
        public string ApplyType
        {
            get { return this.m_szApplyType; }
            set { this.m_szApplyType = value; }
        }

        /// <summary>
        /// 获取或设置申请单名称
        /// </summary>
        public string ApplyName
        {
            get { return this.m_szApplyName; }
            set { this.m_szApplyName = value; }
        }

        /// <summary>
        /// 获取或设置紧急程度
        /// </summary>
        public int Urgency
        {
            get { return this.m_nUrgency; }
            set { this.m_nUrgency = value; }
        }

        /// <summary>
        /// 获取或设置病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        /// <summary>
        /// 获取或设置病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
            set { this.m_szPatientName = value; }
        }

        /// <summary>
        /// 获取或设置病人住院标记
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        /// <summary>
        /// 获取或设置病人子ID
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
            set { this.m_szSubID = value; }
        }

        /// <summary>
        /// 获取或设置病人所在护理单元编码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        /// <summary>
        /// 获取或设置病人所在护理单元名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        /// <summary>
        /// 获取或设置申请者ID
        /// </summary>
        public string ApplicantID
        {
            get { return this.m_szApplicantID; }
            set { this.m_szApplicantID = value; }
        }

        /// <summary>
        /// 获取或设置申请者姓名
        /// </summary>
        public string ApplicantName
        {
            get { return this.m_szApplicantName; }
            set { this.m_szApplicantName = value; }
        }

        /// <summary>
        /// 获取或设置申请时间
        /// </summary>
        public DateTime ApplyTime
        {
            get { return this.m_dtApplyTime; }
            set { this.m_dtApplyTime = value; }
        }

        /// <summary>
        /// 获取或设置申请单状态
        /// </summary>
        public string Status
        {
            get { return this.m_szStatus; }
            set { this.m_szStatus = value; }
        }

        /// <summary>
        /// 获取或设置修改者ID
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
            set { this.m_szModifierID = value; }
        }

        /// <summary>
        /// 获取或设置修改者姓名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        /// <summary>
        /// 获取或设置修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        public override object Clone()
        {
            object instance = Activator.CreateInstance(this.GetType());
            GlobalMethods.Reflect.CopyProperties(this, instance);
            return instance;
        }

        public override string ToString()
        {
            return string.Format("ID={0},State={1}", this.m_szApplyID, this.m_szStatus);
        }

        /// <summary>
        /// 生成申请单ID
        /// </summary>
        /// <returns>string</returns>
        public string MakeApplyID()
        {
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}_{2}{3}", this.m_szPatientID, this.m_szVisitID, DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
        }
    }

    /// <summary>
    /// 护理申请状态信息类
    /// </summary>
    public class NurApplyStatusInfo : DbTypeBase, ICloneable
    {
        private string m_szApplyID = string.Empty;            //申请编号
        private string m_szStatus = string.Empty;             //申请状态
        private string m_szOperatorID = string.Empty;         //操作者ID
        private string m_szOperatorName = string.Empty;       //操作者姓名
        private DateTime m_dtOperateTime = DateTime.Now;      //操作时间
        private string m_szStatusDesc = string.Empty;         //状态描述
        private string m_szNextOperatorID = string.Empty;     //下一个操作者ID
        private string m_szNextOperatorName = string.Empty;   //下一个操作者姓名
        private string m_szNextOperatorWardCode = string.Empty; //下一个操作护理单元编码
        private string m_szNextOperatorWardName = string.Empty; //下一个操作护理单元名称
        private string m_szStatusMessage = string.Empty;    //状态关联的消息

        /// <summary>
        /// 获取或设置申请编号
        /// </summary>
        public string ApplyID
        {
            get { return this.m_szApplyID; }
            set { this.m_szApplyID = value; }
        }

        /// <summary>
        /// 获取或设置申请状态
        /// </summary>
        public string Status
        {
            get { return this.m_szStatus; }
            set { this.m_szStatus = value; }
        }

        /// <summary>
        /// 获取或设置下一个操作者ID
        /// </summary>
        public string NextOperatorID
        {
            get { return this.m_szNextOperatorID; }
            set { this.m_szNextOperatorID = value; }
        }

        /// <summary>
        /// 获取或设置下一个操作者姓名
        /// </summary>
        public string NextOperatorName
        {
            get { return this.m_szNextOperatorName; }
            set { this.m_szNextOperatorName = value; }
        }

        /// <summary>
        /// 获取或设置操作时间
        /// </summary>
        public DateTime OperateTime
        {
            get { return this.m_dtOperateTime; }
            set { this.m_dtOperateTime = value; }
        }

        /// <summary>
        /// 获取或设置申请状态描述
        /// </summary>
        public string StatusDesc
        {
            get { return this.m_szStatusDesc; }
            set { this.m_szStatusDesc = value; }
        }

        /// <summary>
        /// 获取或设置操作者ID
        /// </summary>
        public string OperatorID
        {
            get { return this.m_szOperatorID; }
            set { this.m_szOperatorID = value; }
        }

        /// <summary>
        /// 获取或设置操作者姓名
        /// </summary>
        public string OperatorName
        {
            get { return this.m_szOperatorName; }
            set { this.m_szOperatorName = value; }
        }

        /// <summary>
        /// 获取或设置下一个操作护理单元编码
        /// </summary>
        public string NextOperatorWardCode
        {
            get { return this.m_szNextOperatorWardCode; }
            set { this.m_szNextOperatorWardCode = value; }
        }

        /// <summary>
        /// 获取或设置下一个操作护理单元名称
        /// </summary>
        public string NextOperatorWardName
        {
            get { return this.m_szNextOperatorWardName; }
            set { this.m_szNextOperatorWardName = value; }
        }

        /// <summary>
        /// 获取或设置用于返回的文档状态消息
        /// </summary>
        public string StatusMessage
        {
            get { return this.m_szStatusMessage; }
            set { this.m_szStatusMessage = value; }
        }

        public NurApplyStatusInfo()
        {
            this.m_dtOperateTime = base.DefaultTime;
        }

        public override object Clone()
        {
            object instance = Activator.CreateInstance(this.GetType());
            GlobalMethods.Reflect.CopyProperties(this, instance);
            return instance;
        }

        public override string ToString()
        {
            return string.Format("ID={0},State={1}", this.m_szApplyID, this.m_szStatus);
        }
    }

    /// <summary>
    /// 护理计划信息类
    /// </summary>
    public class NurCarePlanInfo : DbTypeBase, ICloneable
    {
        private string m_szNCPID = string.Empty;            //计划ID
        private string m_szDocID = string.Empty;            //文档编号
        private string m_szDocTypeID = string.Empty;        //文档类型编号
        private string m_szDiagCode = string.Empty;         //诊断编码
        private string m_szDiagName = string.Empty;         //诊断名称
        private string m_szPatientID = string.Empty;        //病人ID
        private string m_szPatientName = string.Empty;     //病人姓名
        private string m_szVisitID = string.Empty;          //病人住院标记
        private string m_szSubID = string.Empty;            //病人子ID
        private string m_szWardCode = string.Empty;         //病人所在护理单元编号
        private string m_szWardName = string.Empty;          //病人所在护理单元名称
        private string m_szCreatorID = string.Empty;      //创建者ID
        private string m_szCreatorName = string.Empty;    //创建者姓名
        private DateTime m_dtCreateTime = DateTime.Now;     //创建时间
        private string m_szStatus = string.Empty;           //护理计划状态
        private string m_szModifierID = string.Empty;        //修改者ID
        private string m_szModifierName = string.Empty;     //修改者姓名
        private DateTime m_dtModifyTime = DateTime.Now;       //修改时间
        private DateTime m_dtStartTime = DateTime.Now;       //护理计划开始时间
        private DateTime m_dtEndTime = DateTime.Now;       //护理计划结束时间

        /// <summary>
        /// 护理计划信息类
        /// </summary>
        /// <remarks></remarks>
        public NurCarePlanInfo()
        {
            this.Initialize();
        }

        /// <summary>
        /// 护理计划信息类成员初始化过程(设置其默认值)
        /// </summary>
        /// <remarks></remarks>
        public void Initialize()
        {
            this.m_dtCreateTime = this.DefaultTime;
            this.m_dtModifyTime = this.DefaultTime;
            this.m_szCreatorID = string.Empty;
            this.m_szCreatorName = string.Empty;
            this.m_szNCPID = string.Empty;
            this.m_szDiagCode = string.Empty;
            this.m_szDiagName = string.Empty;
            this.m_szDocID = string.Empty;
            this.m_szDocTypeID = string.Empty;
            this.m_dtEndTime = this.DefaultTime;
            this.m_dtStartTime = this.DefaultTime;
            this.m_szModifierID = string.Empty;
            this.m_szModifierName = string.Empty;
            this.m_szPatientID = string.Empty;
            this.m_szPatientName = string.Empty;
            this.m_szStatus = string.Empty;
            this.m_szSubID = string.Empty;
            this.m_szVisitID = string.Empty;
            this.m_szWardCode = string.Empty;
            this.m_szWardName = string.Empty;
        }

        /// <summary>
        /// 获取或设置护理计划编号
        /// </summary>
        public string NCPID
        {
            get { return this.m_szNCPID; }
            set { this.m_szNCPID = value; }
        }

        /// <summary>
        /// 文档ID
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
            set { this.m_szDocID = value; }
        }

        /// <summary>
        /// 文档类型ID
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        /// <summary>
        /// 诊断编码
        /// </summary>
        public string DiagCode
        {
            get { return this.m_szDiagCode; }
            set { this.m_szDiagCode = value; }
        }

        /// <summary>
        /// 诊断名称
        /// </summary>
        public string DiagName
        {
            get { return this.m_szDiagName; }
            set { this.m_szDiagName = value; }
        }

        /// <summary>
        /// 病人ID
        /// </summary>
        public string PatientID
        {
            get { return this.m_szPatientID; }
            set { this.m_szPatientID = value; }
        }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public string PatientName
        {
            get { return this.m_szPatientName; }
            set { this.m_szPatientName = value; }
        }

        /// <summary>
        /// 病人住院标记
        /// </summary>
        public string VisitID
        {
            get { return this.m_szVisitID; }
            set { this.m_szVisitID = value; }
        }

        /// <summary>
        /// 病人子ID
        /// </summary>
        public string SubID
        {
            get { return this.m_szSubID; }
            set { this.m_szSubID = value; }
        }

        /// <summary>
        /// 病人所在护理单元编码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        /// <summary>
        /// 病人所在护理单元名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        /// <summary>
        /// 创建者ID
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
            set { this.m_szCreatorID = value; }
        }

        /// <summary>
        /// 创建者姓名
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        /// <summary>
        /// 护理计划状态
        /// </summary>
        public string Status
        {
            get { return this.m_szStatus; }
            set { this.m_szStatus = value; }
        }

        /// <summary>
        /// 修改者ID
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
            set { this.m_szModifierID = value; }
        }

        /// <summary>
        /// 修改者姓名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        /// <summary>
        /// 护理计划开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return this.m_dtStartTime; }
            set { this.m_dtStartTime = value; }
        }

        /// <summary>
        /// 护理计划结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return this.m_dtEndTime; }
            set { this.m_dtEndTime = value; }
        }

        public override string ToString()
        {
            return string.Format("ID={0},State={1}", this.m_szNCPID, this.m_szStatus);
        }

        public override object Clone()
        {
            object instance = Activator.CreateInstance(this.GetType());
            GlobalMethods.Reflect.CopyProperties(this, instance);
            return instance;
        }

        /// <summary>
        /// 生成护理计划ID
        /// </summary>
        /// <returns>string</returns>
        public string MakeNCPID()
        {
            Random rand = new Random();
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}_{1}_{2}{3}", this.m_szPatientID, this.m_szVisitID, DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
        }
    }

    /// <summary>
    /// 护理计划状态信息类
    /// </summary>
    public class NurCarePlanStatusInfo : DbTypeBase, ICloneable
    {
        private string m_szNCPID = string.Empty;            //护理计划ID
        private string m_szStatus = string.Empty;             //护理计划状态
        private string m_szOperatorID = string.Empty;         //操作者ID
        private string m_szOperatorName = string.Empty;       //操作者姓名
        private DateTime m_dtOperateTime = DateTime.Now;      //操作时间
        private string m_szStatusDesc = string.Empty;         //状态描述

        /// <summary>
        /// 获取或设置护理计划编号
        /// </summary>
        public string NCPID
        {
            get { return this.m_szNCPID; }
            set { this.m_szNCPID = value; }
        }

        /// <summary>
        /// 获取或设置护理计划状态
        /// </summary>
        public string Status
        {
            get { return this.m_szStatus; }
            set { this.m_szStatus = value; }
        }

        /// <summary>
        /// 获取或设置操作时间
        /// </summary>
        public DateTime OperateTime
        {
            get { return this.m_dtOperateTime; }
            set { this.m_dtOperateTime = value; }
        }

        /// <summary>
        /// 获取或设置护理计划状态描述
        /// </summary>
        public string StatusDesc
        {
            get { return this.m_szStatusDesc; }
            set { this.m_szStatusDesc = value; }
        }

        /// <summary>
        /// 获取或设置操作者ID
        /// </summary>
        public string OperatorID
        {
            get { return this.m_szOperatorID; }
            set { this.m_szOperatorID = value; }
        }

        /// <summary>
        /// 获取或设置操作者姓名
        /// </summary>
        public string OperatorName
        {
            get { return this.m_szOperatorName; }
            set { this.m_szOperatorName = value; }
        }

        public NurCarePlanStatusInfo()
        {
            this.m_dtOperateTime = base.DefaultTime;
        }

        public override object Clone()
        {
            object instance = Activator.CreateInstance(this.GetType());
            GlobalMethods.Reflect.CopyProperties(this, instance);
            return instance;
        }

        public override string ToString()
        {
            return string.Format("ID={0},State={1}", this.m_szNCPID, this.m_szStatus);
        }
    }

    /// <summary>
    /// 护理计划字典信息
    /// </summary>
    public class NurCarePlanDictInfo : DbTypeBase, ICloneable
    {
        private string m_szDiagCode = string.Empty;

        /// <summary>
        /// 获取或设置诊断代码
        /// </summary>
        public string DiagCode
        {
            get { return this.m_szDiagCode; }
            set { this.m_szDiagCode = value; }
        }

        private CommonDictItem m_item = null;

        /// <summary>
        /// 获取或设置关联项目信息
        /// </summary>
        public CommonDictItem Item
        {
            get
            {
                return this.m_item;
            }

            set
            {
                if (value != null)
                    this.m_item = value;
            }
        }

        public NurCarePlanDictInfo()
        {
            this.m_item = new CommonDictItem();
        }

        public override object Clone()
        {
            NurCarePlanDictInfo nurCarePlanDictInfo = new NurCarePlanDictInfo();
            GlobalMethods.Reflect.CopyProperties(this, nurCarePlanDictInfo);
            return nurCarePlanDictInfo;
        }

        public override string ToString()
        {
            return string.Format("DiagCode={0},ItemName={1}", this.m_szDiagCode, this.m_item.ItemName);
        }
    }

    /// <summary>
    /// 食物成分信息类
    /// </summary>
    public class FoodEleInfo : DbTypeBase, ICloneable
    {
        private string m_szFoodCode = string.Empty;            //食物编号
        private string m_szFoodName = string.Empty;            //食物名称
        private string m_szInputCode = string.Empty;           //食物名称拼音首字母

        private string m_nEdible;      //食物可食用的成分
        private string m_nWater;         //食物水分
        private string m_nKcal;         //食物卡路里
        private string m_nKj;       //食物千焦
        private string m_nProtein;     //食物蛋白
        private string m_nFat;        //食物脂肪
        private string m_nCho;           //食物总胆固醇
        private string m_nFiber;     //食物纤维
        private string m_nCholesterol;          //食物胆固醇
        private string m_nAsh;      //食物灰分
        private string m_nVa;    //食物维生素A
        private string m_nCarotene;       //食物胡萝卜素
        private string m_nRetinol;       //食物视黄醇
        private string m_nThiamin;       //食物硫胺素（维生素B1）
        private string m_nRibo;       //食物核黄素磷酸酯钠
        private string m_nNiacin;       //食物烟酸（尼克酸）
        private string m_nVc;       //食物维生素C
        private string m_nVeTotal;       //食物维生素E总量
        private string m_nVeAe;       //食物维生素EA
        private string m_nVeBge;       //食物成分
        private string m_nVeTe;       //食物成分
        private string m_nCa;       //食物钙
        private string m_nP;       //食物磷
        private string m_nK;       //食物钾
        private string m_nNa;       //食物钠
        private string m_nMg;       //食物镁
        private string m_nFe;       //食物铁
        private string m_nZn;       //食物锌
        private string m_nSe;       //食物硒
        private string m_nCu;       //食物铜
        private string m_nMn;       //食物锰
        private string m_nRemark;       //食物标记
        private string m_nFolicAcid;       //食物叶酸
        private string m_nIodine;       //食物碘
        private string m_nIsofTotal;       //食物成分
        private string m_nIsofDaidzein;       //食物大豆黄素
        private string m_nIsofGenistein;       //食物金雀异黄素
        private string m_nIsofGlycitein;       //食物黄豆黄素
        private string m_nGi;       //食物血糖生成指数

        /// <summary>
        /// 获取或设置食物编号
        /// </summary>
        public string FoodCode
        {
            get { return m_szFoodCode; }
            set { m_szFoodCode = value; }
        }
        /// <summary>
        /// 食物名称
        /// </summary>
        public string FoodName
        {
            get { return m_szFoodName; }
            set { m_szFoodName = value; }
        }

        /// <summary>
        /// 食物名称拼音首字母
        /// </summary>
        public string InputCode
        {
            get { return m_szInputCode; }
            set { m_szInputCode = value; }
        }
        public string Edible
        {
            get { return m_nEdible; }
            set { m_nEdible = value; }
        }
        public string Water
        {
            get { return m_nWater; }
            set { m_nWater = value; }
        }

        public string Kcal
        {
            get { return m_nKcal; }
            set { m_nKcal = value; }
        }

        public string Kj
        {
            get { return m_nKj; }
            set { m_nKj = value; }
        }

        public string Protein
        {
            get { return m_nProtein; }
            set { m_nProtein = value; }
        }

        public string Fat
        {
            get { return m_nFat; }
            set { m_nFat = value; }
        }

        public string Cho
        {
            get { return m_nCho; }
            set { m_nCho = value; }
        }

        public string Fiber
        {
            get { return m_nFiber; }
            set { m_nFiber = value; }
        }

        public string Cholesterol
        {
            get { return m_nCholesterol; }
            set { m_nCholesterol = value; }
        }

        public string Ash
        {
            get { return m_nAsh; }
            set { m_nAsh = value; }
        }

        public string Va
        {
            get { return m_nVa; }
            set { m_nVa = value; }
        }

        public string Carotene
        {
            get { return m_nCarotene; }
            set { m_nCarotene = value; }
        }

        public string Retinol
        {
            get { return m_nRetinol; }
            set { m_nRetinol = value; }
        }

        public string Thiamin
        {
            get { return m_nThiamin; }
            set { m_nThiamin = value; }
        }

        public string Ribo
        {
            get { return m_nRibo; }
            set { m_nRibo = value; }
        }

        public string Niacin
        {
            get { return m_nNiacin; }
            set { m_nNiacin = value; }
        }

        public string Vc
        {
            get { return m_nVc; }
            set { m_nVc = value; }
        }

        public string VeTotal
        {
            get { return m_nVeTotal; }
            set { m_nVeTotal = value; }
        }

        public string VeAe
        {
            get { return m_nVeAe; }
            set { m_nVeAe = value; }
        }

        public string VeBge
        {
            get { return m_nVeBge; }
            set { m_nVeBge = value; }
        }

        public string VeTe
        {
            get { return m_nVeTe; }
            set { m_nVeTe = value; }
        }

        public string Ca
        {
            get { return m_nCa; }
            set { m_nCa = value; }
        }

        public string P
        {
            get { return m_nP; }
            set { m_nP = value; }
        }

        public string K
        {
            get { return m_nK; }
            set { m_nK = value; }
        }

        public string Na
        {
            get { return m_nNa; }
            set { m_nNa = value; }
        }

        public string Mg
        {
            get { return m_nMg; }
            set { m_nMg = value; }
        }

        public string Fe
        {
            get { return m_nFe; }
            set { m_nFe = value; }
        }

        public string Zn
        {
            get { return m_nZn; }
            set { m_nZn = value; }
        }

        public string Se
        {
            get { return m_nSe; }
            set { m_nSe = value; }
        }

        public string Cu
        {
            get { return m_nCu; }
            set { m_nCu = value; }
        }

        public string Mn
        {
            get { return m_nMn; }
            set { m_nMn = value; }
        }

        public string Remark
        {
            get { return m_nRemark; }
            set { m_nRemark = value; }
        }

        public string FolicAcid
        {
            get { return m_nFolicAcid; }
            set { m_nFolicAcid = value; }
        }

        public string Iodine
        {
            get { return m_nIodine; }
            set { m_nIodine = value; }
        }

        public string IsofTotal
        {
            get { return m_nIsofTotal; }
            set { m_nIsofTotal = value; }
        }

        public string IsofDaidzein
        {
            get { return m_nIsofDaidzein; }
            set { m_nIsofDaidzein = value; }
        }

        public string IsofGenistein
        {
            get { return m_nIsofGenistein; }
            set { m_nIsofGenistein = value; }
        }

        public string IsofGlycitein
        {
            get { return m_nIsofGlycitein; }
            set { m_nIsofGlycitein = value; }
        }

        public string Gi
        {
            get { return m_nGi; }
            set { m_nGi = value; }
        }

        ///// <summary>
        ///// 食物成分类成员初始化过程(设置其默认值)
        ///// </summary>
        ///// <remarks></remarks>
        //public void Initialize()
        //{
        //    this.m_szFoodID = string.Empty;
        //    this.m_szFoodname = string.Empty;
        //    this.m_nAsh = -1;
        //    this.m_nCa = -1;
        //    this.m_nCarotene = -1;
        //    this.m_nCho = -1;
        //    this.m_nCholesterol = -1;
        //    this.m_nCu = -1;
        //    this.m_nEdible= -1;
        //    this.m_nFat = -1;
        //    this.m_nFe = -1;
        //    this.m_nFiber = -1;
        //    this.m_nFolicAcid = -1;
        //    this.m_nGi = -1;
        //    this.m_nIodine = -1;
        //    this.m_nIsofDaidzein = -1;
        //    this.m_nIsofGenistein = -1;
        //    this.m_nIsofGlycitein = -1;
        //    this.m_nIsofTotal = -1;
        //    this = -1;
        //    this = -1;

        //}        
        /// <summary>
        /// 不要修改ToString方法
        /// </summary>
        /// <returns>返回列名</returns>
        public override string ToString()
        {
            return this.m_szFoodName;
        }

        //public override string ToString()
        //{
        //    return string.Format("ID={0},State={1}", this.m_szNCPID, this.m_szStatus);
        //}

        public override object Clone()
        {
            object instance = Activator.CreateInstance(this.GetType());
            GlobalMethods.Reflect.CopyProperties(this, instance);
            return instance;
        }
    }

    /// <summary>
    /// 质量与安全管理记录模板类
    /// </summary>
    public class QCTypeInfo : DbTypeBase, ICloneable
    {
        private string m_szQCTypeID = string.Empty;

        /// <summary>
        /// 获取或设置该记录模板的模板类型ID
        /// </summary>
        public string QCTypeID
        {
            get { return this.m_szQCTypeID; }
            set { this.m_szQCTypeID = value; }
        }

        private string m_szQCTypeName = string.Empty;

        /// <summary>
        /// 获取或设置该记录模板的模板名称
        /// </summary>
        public string QCTypeName
        {
            get { return this.m_szQCTypeName; }
            set { this.m_szQCTypeName = value; }
        }

        private DateTime m_dtCreateTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置该记录模板的创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.m_dtCreateTime; }
            set { this.m_dtCreateTime = value; }
        }

        private DateTime m_dtModifyTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置该记录模板的修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        private string m_szCreateName = string.Empty;

        /// <summary>
        /// 获取或设置创建该记录模板的用户名
        /// </summary>
        public string CreateName
        {
            get { return this.m_szCreateName; }
            set { this.m_szCreateName = value; }
        }

        private string m_szCreateID = string.Empty;

        /// <summary>
        /// 获取或设置创建该记录模板的用户ID
        /// </summary>
        public string CreateID
        {
            get { return this.m_szCreateID; }
            set { this.m_szCreateID = value; }
        }

        private string m_szModifyName = string.Empty;

        /// <summary>
        /// 获取或设置修改该记录模板的用户名
        /// </summary>
        public string ModifyName
        {
            get { return this.m_szModifyName; }
            set { this.m_szModifyName = value; }
        }

        private string m_szModifyID = string.Empty;

        /// <summary>
        /// 获取或设置创修改记录模板的用户ID
        /// </summary>
        public string ModifyID
        {
            get { return this.m_szModifyID; }
            set { this.m_szModifyID = value; }
        }

        private string m_szApplyEnv = string.Empty;

        /// <summary>
        /// 获取或设置应用环境
        /// </summary>
        public string ApplyEnv
        {
            get { return this.m_szApplyEnv; }
            set { this.m_szApplyEnv = value; }
        }

        private int m_nQCTypeNo = 0;

        /// <summary>
        /// 获取或设置该文档类型在列表中的排序
        /// </summary>
        public int QCTypeNo
        {
            get { return this.m_nQCTypeNo; }
            set { this.m_nQCTypeNo = value; }
        }

        private string m_szParentID = string.Empty;

        /// <summary>
        /// 获取或设置该文档类型对应的目录ID
        /// </summary>
        public string ParentID
        {
            get { return this.m_szParentID; }
            set { this.m_szParentID = value; }
        }

        private bool m_bIsRepeated = true;

        /// <summary>
        /// 获取或设置该类型的文档是否可重复
        /// </summary>
        public bool IsRepeated
        {
            get { return this.m_bIsRepeated; }
            set { this.m_bIsRepeated = value; }
        }

        private bool m_bIsValid = true;

        /// <summary>
        /// 获取或设置标识当前文档类型是否有效
        /// </summary>
        public bool IsValid
        {
            get { return this.m_bIsValid; }
            set { this.m_bIsValid = value; }
        }

        private bool m_bIsVisible = true;

        /// <summary>
        /// 获取或设置标识当前文档类型是否界面可见
        /// </summary>
        public bool IsVisible
        {
            get { return this.m_bIsVisible; }
            set { this.m_bIsVisible = value; }
        }

        private FormPrintMode m_nPrintMode = FormPrintMode.None;

        /// <summary>
        /// 获取或设置标识当前文档模板打印模式
        /// </summary>
        public FormPrintMode PrintMode
        {
            get { return this.m_nPrintMode; }
            set { this.m_nPrintMode = value; }
        }

        private bool m_bIsFolder = false;

        /// <summary>
        /// 获取或设置标识当前文档类型是否是目录
        /// </summary>
        public bool IsFolder
        {
            get { return this.m_bIsFolder; }
            set { this.m_bIsFolder = value; }
        }

        private bool m_bHaveRemark = false;

        /// <summary>
        /// 获取或设置该模板类型是否有备注
        /// </summary>
        public bool HaveRemark
        {
            get { return this.m_bHaveRemark; }
            set { this.m_bHaveRemark = value; }
        }

        private string m_szSortColumn = string.Empty;

        /// <summary>
        /// 获取或设置文档列表默认排序列
        /// </summary>
        public string SortColumn
        {
            get { return this.m_szSortColumn; }
            set { this.m_szSortColumn = value; }
        }

        private SortMode m_iSortMode = SortMode.None;

        /// <summary>
        /// 获取或设置文档列表默认排序方式
        /// </summary>
        public SortMode SortMode
        {
            get { return this.m_iSortMode; }
            set { this.m_iSortMode = value; }
        }

        private string m_szSchemaID = null;

        /// <summary>
        /// 获取或设置文档所属表格列配置方案ID
        /// </summary>
        public string SchemaID
        {
            get { return this.m_szSchemaID; }
            set { this.m_szSchemaID = value; }
        }

        public QCTypeInfo()
        {
            this.m_dtModifyTime = base.DefaultTime;
        }

        public override object Clone()
        {
            QCTypeInfo qcTypeInfo = new QCTypeInfo();
            GlobalMethods.Reflect.CopyProperties(this, qcTypeInfo);
            return qcTypeInfo;
        }
    }


    /// <summary>
    /// 质量与安全管理记录文档信息类 
    /// </summary>
    public class QCDocInfo : DbTypeBase, ICloneable
    {
        private string m_szDocID;           //文档编号
        private string m_szDocTypeID;       //文档类型代码
        private string m_szDocTitle;        //文档标题
        private DateTime m_dtDocTime;       //文档创建时间
        private string m_szDocSetID;        //文档集编号
        private int m_nDocVersion;          //文档版本号
        private string m_szCreatorID;       //文档创建人ID
        private string m_szCreatorName;     //文档创建人姓名
        private string m_szModifierID;      //文档修改人ID
        private string m_szModifierName;    //文档修改人姓名
        private DateTime m_dtModifyTime;    //文档修改时间
        private string m_szWardCode;        //病人病区代码
        private string m_szWardName;        //病人病区名称
        private string m_szSignCode;        //签名代码
        private string m_szConfidCode;      //机密等级
        private int m_nOrderValue;          //文档排序值
        private DateTime m_dtRecordTime;    //实际记录时间
        private string m_szRecordID;        //关联的质量与安全管理记录ID
        private string m_szStatusDesc;      //文档状态描述
        private string m_szResult;           //结果      

        /// <summary>
        /// 文档信息类
        /// </summary>
        /// <remarks></remarks>
        public QCDocInfo()
        {
            this.Initialize();
        }

        /// <summary>
        /// 文档信息类成员初始化过程(设置其默认值)
        /// </summary>
        /// <remarks></remarks>
        public void Initialize()
        {
            this.m_szDocID = string.Empty;
            this.m_szDocTypeID = string.Empty;
            this.m_szDocTitle = string.Empty;
            this.m_dtDocTime = base.DefaultTime;
            this.m_szDocSetID = string.Empty;
            this.m_nDocVersion = 0;
            this.m_szCreatorID = string.Empty;
            this.m_szCreatorName = string.Empty;
            this.m_szModifierID = string.Empty;
            this.m_szModifierName = string.Empty;
            this.m_dtModifyTime = base.DefaultTime;
            this.m_szWardCode = string.Empty;
            this.m_szWardName = string.Empty;
            this.m_szSignCode = string.Empty;
            this.m_szConfidCode = string.Empty;
            this.m_nOrderValue = -1;
            this.m_dtRecordTime = base.DefaultTime;
            this.m_szRecordID = string.Empty;
            this.m_szStatusDesc = string.Empty;
            this.m_szResult = string.Empty;
        }

        /// <summary>
        /// 结果
        /// </summary>
        public string Result
        {
            get { return this.m_szResult; }
            set { this.m_szResult = value; }
        }

        /// <summary>
        /// 获取或设置文档编号
        /// </summary>
        public string DocID
        {
            get { return this.m_szDocID; }
            set { this.m_szDocID = value; }
        }

        /// <summary>
        /// 获取或设置文档类型代码
        /// </summary>
        public string DocTypeID
        {
            get { return this.m_szDocTypeID; }
            set { this.m_szDocTypeID = value; }
        }

        /// <summary>
        /// 获取或设置文档的显示标题
        /// </summary>
        public string DocTitle
        {
            get { return this.m_szDocTitle; }
            set { this.m_szDocTitle = value; }
        }

        /// <summary>
        /// 获取或设置文档的创建时间
        /// </summary>
        public DateTime DocTime
        {
            get { return this.m_dtDocTime; }
            set { this.m_dtDocTime = value; }
        }

        /// <summary>
        /// 获取或设置文档所属文档集编号
        /// </summary>
        public string DocSetID
        {
            get { return this.m_szDocSetID; }
            set { this.m_szDocSetID = value; }
        }

        /// <summary>
        /// 获取或设置文档版本号
        /// </summary>
        public int DocVersion
        {
            get { return this.m_nDocVersion; }
            set { this.m_nDocVersion = value; }
        }

        /// <summary>
        /// 获取或设置作者ID
        /// </summary>
        public string CreatorID
        {
            get { return this.m_szCreatorID; }
            set { this.m_szCreatorID = value; }
        }

        /// <summary>
        /// 获取或设置作者姓名
        /// </summary>
        public string CreatorName
        {
            get { return this.m_szCreatorName; }
            set { this.m_szCreatorName = value; }
        }

        /// <summary>
        /// 获取或设置文档修改人编号
        /// </summary>
        public string ModifierID
        {
            get { return this.m_szModifierID; }
            set { this.m_szModifierID = value; }
        }

        /// <summary>
        /// 获取或设置文档修改人姓名
        /// </summary>
        public string ModifierName
        {
            get { return this.m_szModifierName; }
            set { this.m_szModifierName = value; }
        }

        /// <summary>
        /// 获取或设置文档的修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this.m_dtModifyTime; }
            set { this.m_dtModifyTime = value; }
        }

        /// <summary>
        /// 获取或设置病区代码
        /// </summary>
        public string WardCode
        {
            get { return this.m_szWardCode; }
            set { this.m_szWardCode = value; }
        }

        /// <summary>
        /// 获取或设置病区名称
        /// </summary>
        public string WardName
        {
            get { return this.m_szWardName; }
            set { this.m_szWardName = value; }
        }

        /// <summary>
        /// 获取或设置签名代码
        /// </summary>
        public string SignCode
        {
            get { return this.m_szSignCode; }
            set { this.m_szSignCode = value; }
        }

        /// <summary>
        /// 获取或设置机密等级
        /// </summary>
        public string ConfidCode
        {
            get { return this.m_szConfidCode; }
            set { this.m_szConfidCode = value; }
        }

        /// <summary>
        /// 获取或设置文档排序值
        /// </summary>
        public int OrderValue
        {
            get { return this.m_nOrderValue; }
            set { this.m_nOrderValue = value; }
        }

        /// <summary>
        /// 获取或设置实际记录时间
        /// </summary>
        public DateTime RecordTime
        {
            get { return this.m_dtRecordTime; }
            set { this.m_dtRecordTime = value; }
        }

        /// <summary>
        /// 获取或设置文档存放路径(对于新病历,为空)
        /// </summary>
        public string RecordID
        {
            get { return this.m_szRecordID; }
            set { this.m_szRecordID = value; }
        }

        /// <summary>
        /// 获取或设置文档状态描述
        /// </summary>
        public string StatusDesc
        {
            get { return this.m_szStatusDesc; }
            set { this.m_szStatusDesc = value; }
        }

        public override string ToString()
        {
            return string.Format("DocID={0},DocTitle={1}", this.m_szDocID, this.m_szDocTitle);
        }
    }
    public class AssessmentCycle : DbTypeBase, ICloneable
    {
        /// <summary>
        /// 患者ID
        /// </summary>
        public string PATIENT_ID { get; set; }

        /// <summary>
        /// 患者就诊次
        /// </summary>
        public string VISIT_ID { get; set; }

        /// <summary>
        /// 评估文档ID
        /// </summary>
        public string DOC_ID { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public string DOCTYPE_ID { get; set; }

        /// <summary>
        /// 评估时间
        /// </summary>
        public DateTime RECORD_TIME { get; set; }

        /// <summary>
        /// 评估频次
        /// </summary>
        public decimal CYCLE { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UNIT { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public string DATA { get; set; }

        /// <summary>
        /// 评估科室名称
        /// </summary>
        public string WARD_NAME { get; set; }

        /// <summary>
        /// 评估科室代码
        /// </summary>
        public string WARD_CODE { get; set; }

        /// <summary>
        /// 评估人
        /// </summary>
        public string CREATOR_NAME { get; set; }

        /// <summary>
        /// 评估人ID
        /// </summary>
        public string CREATOR_ID { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string DOCTYPE_NAME { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        public string MakeID()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            string szRand = rand.Next(0, 9999).ToString().PadLeft(4, '0');
            return string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), szRand);
        }
    }
    #endregion
}
