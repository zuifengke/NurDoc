// ***********************************************************
// 数据库访问层常量数据集合类
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Heren.NurDoc.DAL
{
    public struct ServerData
    {
        /// <summary>
        /// 返回值常量
        /// </summary>
        public struct ExecuteResult
        {
            /// <summary>
            /// 正常(0)
            /// </summary>
            public const short OK = 0;

            /// <summary>
            /// 参数错误(1)
            /// </summary>
            public const short PARAM_ERROR = 1;

            /// <summary>
            /// 数据库访问错误(2)
            /// </summary>
            public const short ACCESS_ERROR = 2;

            /// <summary>
            /// 接口内部异常(3)
            /// </summary>
            public const short EXCEPTION = 3;

            /// <summary>
            /// 资源未发现(4)
            /// </summary>
            public const short RES_NO_FOUND = 4;

            /// <summary>
            /// 资源已经存在(5)
            /// </summary>
            public const short RES_IS_EXIST = 5;

            /// <summary>
            /// 其他错误(9)
            /// </summary>
            public const short OTHER_ERROR = 9;
        }

        /// <summary>s
        /// 数据库类型常量
        /// </summary>
        internal struct DatabaseType
        {
            /// <summary>
            /// ORACLE数据库类型
            /// </summary>
            public const string ORACLE = "ORACLE";

            /// <summary>
            /// SQLSERVER数据库类型
            /// </summary>
            public const string SQLSERVER = "SQLSERVER";
        }

        /// <summary>
        /// 数据库驱动类型常量
        /// </summary>
        internal struct DataProvider
        {
            /// <summary>
            /// .NET提供的SqlClient驱动类型
            /// </summary>
            public const string SQLCLIENT = "System.Data.SqlClient";

            /// <summary>
            /// .NET提供的Oracle驱动类型
            /// </summary>
            public const string ORACLE = "System.Data.OracleClient";

            /// <summary>
            /// .NET提供的OleDb驱动类型
            /// </summary>
            public const string OLEDB = "System.Data.OleDb";

            /// <summary>
            /// .NET提供的ODBC驱动类型
            /// </summary>
            public const string ODBC = "System.Data.Odbc";

            /// <summary>
            /// Oracle提供的ODPNET驱动类型
            /// </summary>
            public const string ODPNET = "Oracle.DataAccess.Client";
        }

        /// <summary>
        /// 文件扩展名常量
        /// </summary>
        public struct FileExt
        {
            /// <summary>
            /// 护理文档扩展名"hndf"
            /// </summary>
            public const string NUR_DOCUMENT = "hndf";

            /// <summary>
            /// 护理模板文件扩展名"hndt"
            /// </summary>
            public const string NUR_TEMPLET = "hndt";

            /// <summary>
            /// 报告文件扩展名"hrdf"
            /// </summary>
            public const string REPORT_DOCUEMNT = "hrdf";

            /// <summary>
            /// 报表模板文件扩展名"hrdt"
            /// </summary>
            public const string REPORT_TEMPLET = "hrdt";
        }

        /// <summary>
        /// 文件类型
        /// </summary>
        public struct FileType
        {
            /// <summary>
            /// 护理文书模版
            /// </summary>
            public const string TEMPLET = "DocTemplet";

            /// <summary>
            /// 护理文书打印报表模版
            /// </summary>
            public const string REPORT = "Report";
        }

        /// <summary>
        /// 共享水平常量
        /// </summary>
        public struct ShareLevel
        {
            /// <summary>
            /// 全院共享"H"
            /// </summary>
            public const string HOSPITAL = "H";

            /// <summary>
            /// 科室共享"D"
            /// </summary>
            public const string DEPART = "D";

            /// <summary>
            /// 个人私有"P"
            /// </summary>
            public const string PERSONAL = "P";

            /// <summary>
            /// 全院共享
            /// </summary>
            public const string HOSPITAL_CN = "全院共享";

            /// <summary>
            /// 科室共享
            /// </summary>
            public const string DEPART_CN = "科室共享";

            /// <summary>
            /// 个人私有
            /// </summary>
            public const string PERSONAL_CN = "个人使用";

            /// <summary>
            /// 转换英文缩写与中文名称对应的共享级别
            /// </summary>
            /// <param name="szShareLevel">共享级别</param>
            /// <returns>共享级别</returns>
            public static string TransShareLevel(string szShareLevel)
            {
                szShareLevel = szShareLevel.Trim().ToUpper();
                if (szShareLevel == HOSPITAL)
                    return HOSPITAL_CN;
                if (szShareLevel == DEPART)
                    return DEPART_CN;
                if (szShareLevel == PERSONAL)
                    return PERSONAL_CN;
                if (szShareLevel == HOSPITAL_CN)
                    return HOSPITAL;
                if (szShareLevel == DEPART_CN)
                    return DEPART;
                if (szShareLevel == PERSONAL_CN)
                    return PERSONAL;
                return szShareLevel;
            }
        }

        /// <summary>
        /// 列方案标记
        /// </summary>
        public struct SchemaFlag
        {
            /// <summary>
            /// 批量录入之护理记录录入方案
            /// </summary>
            public const string NUR_REC = "NUR_REC";

            /// <summary>
            /// 批量录入之脚本配置具体时间录入
            /// </summary>
            public const string NUR_SCRIPT_TIME = "NUR_SCRIPT_TIME";

            public static string[] GetFlagNames()
            {
                return new string[] { "护理记录批量录入", "脚本配置具体时间录入" };
            }

            public static string GetFlagName(string typeCode)
            {
                if (typeCode == NUR_REC)
                    return "护理记录批量录入";
                if (typeCode == NUR_SCRIPT_TIME)
                    return "脚本配置具体时间录入";
                return string.Empty;
            }

            public static string GetFlagCode(string typeName)
            {
                if (typeName == "护理记录批量录入")
                    return NUR_REC;
                else if (typeName == "脚本配置具体时间录入")
                    return NUR_SCRIPT_TIME;
                return string.Empty;
            }
        }

        /// <summary>
        /// 病案质量监控信息表相关字段定义
        /// </summary>
        public struct QCMessageView
        {
            /// <summary>
            /// 质检信息ID
            /// </summary>
            public const string MSG_ID = "MSG_ID";
            /// <summary>
            /// 病人标识号
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";
            /// <summary>
            /// 病人本次住院标识
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";
            /// <summary>
            /// 病人姓名
            /// </summary>
            public const string NAME = "PATIENT_NAME";
            /// <summary>
            /// 病人所在科室
            /// </summary>
            public const string DEPT_STAYED = "DEPT_STAYED";
            /// <summary>
            /// 科室名称
            /// </summary>
            public const string DEPT_NAME = "DEPT_NAME";
            /// <summary>
            /// 病人经治医师
            /// </summary>
            public const string INCHARGE_DOCTOR = "DOCTOR_IN_CHARGE";
            /// <summary>
            /// 上级医生
            /// </summary>
            public const string PARENT_DOCTOR = "PARENT_DOCTOR";
            /// <summary>
            /// 主任医生
            /// </summary>
            public const string SUPER_DOCTOR = "SUPER_DOCTOR";
            /// <summary>
            /// 病案质量问题分类
            /// </summary>
            public const string QA_EVENT_TYPE = "QA_EVENT_TYPE";
            /// <summary>
            /// 反馈信息代码
            /// </summary>
            public const string QC_MSG_CODE = "QC_MSG_CODE";
            /// <summary>
            /// 反馈信息描述
            /// </summary>
            public const string MESSAGE = "MESSAGE";
            /// <summary>
            /// 发出者
            /// </summary>
            public const string ISSUED_BY = "ISSUED_BY";
            /// <summary>
            /// 发出者ID
            /// </summary>
            public const string ISSUED_ID = "ISSUED_ID";
            /// <summary>
            /// 发出时间
            /// </summary>
            public const string ISSUED_DATE_TIME = "ISSUED_DATE_TIME";
            /// <summary>
            /// 信息状态0 未确认/未接收  1 已确认/已接受 2 已修改 3合格
            /// </summary>
            public const string MSG_STATUS = "MSG_STATUS";
            /// <summary>
            /// 信息确认时间
            /// </summary>
            public const string ASK_DATE_TIME = "ASK_DATE_TIME";
            /// <summary>
            /// 病案质控类型
            /// </summary>
            public const string QC_MODULE = "QC_MODULE";
            /// <summary>
            /// 病历主题代码
            /// </summary>
            public const string TOPIC_ID = "TOPIC_ID";
            /// <summary>
            /// 病历主题
            /// </summary>
            public const string TOPIC = "TOPIC";
            /// <summary>
            /// 医生反馈信息
            /// </summary>
            public const string DOCTOR_COMMENT = "DOCTOR_COMMENT";
            /// <summary>
            /// 扣分值
            /// </summary>
            public const string POINT = "POINT";
            /// <summary>
            /// 扣分类型  0-自动扣分 1-手动输入扣分
            /// </summary>
            public const string POINT_TYPE = "POINT_TYPE";
            /// <summary>
            /// 强制锁定状态，0:不锁定 1:锁定，必须修改问题才能创建病历
            /// </summary>
            public const string LOCK_STATUS = "LOCK_STATUS";
        }

        /// <summary>
        /// 报告应用环境常量
        /// </summary>
        public struct ReportTypeApplyEnv
        {
            /// <summary>
            /// 体温单 TEMPERATURE
            /// </summary>
            public const string TEMPERATURE = "TEMPERATURE";

            /// <summary>
            /// 医嘱单 ORDERS
            /// </summary>
            public const string ORDERS = "ORDERS";

            /// <summary>
            /// 护理记录单 NUR_RECORD
            /// </summary>
            public const string NUR_RECORD = "NUR_RECORD";

            /// <summary>
            /// 监护记录单 MONITOR_RECORD
            /// </summary>
            public const string MONITOR_RECORD = "MONITOR_RECORD";

            /// <summary>
            /// 护理交接班 NUR_SHIFT
            /// </summary>
            public const string NUR_SHIFT = "NUR_SHIFT";

            /// <summary>
            /// 护理文档表单 NUR_DOC_FORM
            /// </summary>
            public const string NUR_DOC_FORM = "NUR_DOC_FORM";

            /// <summary>
            /// 护理文档列表 NUR_DOC_LIST
            /// </summary>
            public const string NUR_DOC_LIST = "NUR_DOC_LIST";

            /// <summary>
            /// 体征批量录入 BATCH_RECORD
            /// </summary>
            public const string BATCH_RECORD = "BATCH_RECORD";

            public static string[] GetTypeNames()
            {
                return new string[] { 
                    "体温单", 
                    "医嘱单", 
                    "护理记录单",
                    "监护记录单", 
                    "护理交接班",
                    "护理文档表单",
                    "护理文档列表",
                    "体征批量录入"};
            }

            public static string GetTypeName(string typeCode)
            {
                if (typeCode == TEMPERATURE)
                    return "体温单";
                else if (typeCode == ORDERS)
                    return "医嘱单";
                else if (typeCode == NUR_RECORD)
                    return "护理记录单";
                else if (typeCode == MONITOR_RECORD)
                    return "监护记录单";
                else if (typeCode == NUR_SHIFT)
                    return "护理交接班";
                else if (typeCode == NUR_DOC_FORM)
                    return "护理文档表单";
                else if (typeCode == NUR_DOC_LIST)
                    return "护理文档列表";
                else if (typeCode == BATCH_RECORD)
                    return "体征批量录入";
                return string.Empty;
            }

            public static string GetTypeCode(string typeName)
            {
                if (typeName == "体温单")
                    return TEMPERATURE;
                else if (typeName == "医嘱单")
                    return ORDERS;
                else if (typeName == "护理记录单")
                    return NUR_RECORD;
                else if (typeName == "监护记录单")
                    return MONITOR_RECORD;
                else if (typeName == "护理交接班")
                    return NUR_SHIFT;
                else if (typeName == "护理文档表单")
                    return NUR_DOC_FORM;
                else if (typeName == "护理文档列表")
                    return NUR_DOC_LIST;
                else if (typeName == "体征批量录入")
                    return BATCH_RECORD;
                return string.Empty;
            }
        }

        /// <summary>
        /// 表单类型常量
        /// </summary>
        public struct DocTypeApplyEnv
        {
            /// <summary>
            /// 全局功能模块插件 GLOBAL_MODULE
            /// </summary>
            public const string GLOBAL_MODULE = "GLOBAL_MODULE";

            /// <summary>
            /// 病人功能模块插件 PATIENT_MODULE
            /// </summary>
            public const string PATIENT_MODULE = "PATIENT_MODULE";

            /// <summary>
            /// 护理记录 NURSING_RECORD
            /// </summary>
            public const string NURSING_RECORD = "NURSING_RECORD";

            /// <summary>
            /// 护理文档 NURSING_DOCUMENT
            /// </summary>
            public const string NURSING_DOCUMENT = "NURSING_DOCUMENT";

            /// <summary>
            /// 专科护理 NURSING_SPECIAL
            /// </summary>
            public const string NURSING_SPECIAL = "NURSING_SPECIAL";

            /// <summary>
            /// 护理评估 NURSING_ASSESSMENT
            /// </summary>
            public const string NURSING_ASSESSMENT = "NURSING_ASSESSMENT";

            /// <summary>
            /// 文书列表1 NURDOC_LIST1
            /// </summary>
            public const string NURDOC_LIST1 = "NURDOC_LIST1";

            /// <summary>
            /// 文书列表2 NURDOC_LIST2
            /// </summary>
            public const string NURDOC_LIST2 = "NURDOC_LIST2";

            /// <summary>
            /// 文书列表3 NURDOC_LIST3
            /// </summary>
            public const string NURDOC_LIST3 = "NURDOC_LIST3";

            /// <summary>
            /// 文书列表4 NURDOC_LIST4
            /// </summary>
            public const string NURDOC_LIST4 = "NURDOC_LIST4";

            /// <summary>
            /// 床卡视图 BED_VIEW
            /// </summary>
            public const string BED_VIEW = "BED_VIEW";

            /// <summary>
            /// 白板视图 WHITEBOARD_VIEW
            /// </summary>
            public const string WHITEBOARD_VIEW = "WHITEBOARD_VIEW";

            /// <summary>
            /// 体征快捷录入 SIGNS_DATA_RECORD
            /// </summary>
            public const string SIGNS_DATA_RECORD = "SIGNS_DATA_RECORD";

            /// <summary>
            /// 批量录入筛选 BATCH_RECORD_FILTER
            /// </summary>
            public const string BATCH_RECORD_FILTER = "BATCH_RECORD_FILTER";

            /// <summary>
            /// 护理记录内容显示区 NURSING_REC_CONTENT
            /// </summary>
            public const string NURSING_REC_CONTENT = "NURSING_REC_CONTENT";

            /// <summary>
            /// 护理交班志
            /// </summary>
            public const string NURSING_WORKSHIFT = "NURSING_WORKSHIFT";

            /// <summary>
            /// 专科一览表
            /// </summary>
            public const string SPECIAL_PATIENT = "SPECIAL_PATIENT";

            /// <summary>
            /// 待办任务 NURSING_TASK
            /// </summary>
            public const string NURSING_TASK = "NURSING_TASK";

            /// <summary>
            /// 护理会诊 NURSING_CONSULT
            /// </summary>
            public const string NURSING_CONSULT = "NURSING_CONSULT";

            /// <summary>
            /// 查询统计 NURSING_STATISTIC
            /// </summary>
            public const string NURSING_STATISTIC = "NURSING_STATISTIC";

            /// <summary>
            /// 护理计划 NURSING_CARE_PLAN
            /// </summary>
            public const string NURSING_CARE_PLAN = "NURSING_CARE_PLAN";

            /// <summary>
            /// 护理首页 NURSING_HOME_PAGE
            /// </summary>
            public const string NURSING_HOME_PAGE = "NURSING_HOME_PAGE";

            /// <summary>
            /// 综合查询 INTEGRATE_QUERY
            /// </summary>
            public const string INTEGRATE_QUERY = "INTEGRATE_QUERY";

            /// <summary>
            /// 护理质控 NURSING_QC
            /// </summary>
            public const string NURSING_QC = "NURSING_QC";

            /// <summary>
            /// 监护记录 NURSING_MONITOR
            /// </summary>
            public const string NURSING_MONITOR = "NURSING_MONITOR";

            /// <summary>
            /// 护理记录小结 NURSING_SUMMARY
            /// </summary>
            public const string NURSING_SUMMARY = "NURSING_SUMMARY";

            public static string[] GetApplyEnvNames()
            {
                return new string[] { 
                    "护理记录", 
                    "护理文书", 
                    "护理评估", 
                    "文书列表1",
                    "文书列表2",
                    "文书列表3",
                    "文书列表4",
                    "专科护理",
                    "待办任务",
                    "查询统计",
                    "综合查询",
                    "护理会诊",
                    "护理计划",
                    "床卡视图",
                    "护理质控",
                    "监护记录",
                    "系统起始页",
                    "专科一览表",
                    "护理交接班",
                    "护理记录小结",
                    "护士站白板视图",
                    "体征快捷录入窗口",
                    "批量录入筛选窗口",
                    "护理记录内容窗口",
                    "病人功能模块插件",
                    "全局功能模块插件"};
            }

            public static string GetApplyEnvName(string applyEnvCode)
            {
                if (applyEnvCode == NURSING_RECORD)
                    return "护理记录";
                else if (applyEnvCode == NURSING_DOCUMENT)
                    return "护理文书";
                else if (applyEnvCode == NURSING_ASSESSMENT)
                    return "护理评估";
                else if (applyEnvCode == NURDOC_LIST1)
                    return "文书列表1";
                else if (applyEnvCode == NURDOC_LIST2)
                    return "文书列表2";
                else if (applyEnvCode == NURDOC_LIST3)
                    return "文书列表3";
                else if (applyEnvCode == NURDOC_LIST4)
                    return "文书列表4";
                else if (applyEnvCode == NURSING_SPECIAL)
                    return "专科护理";
                else if (applyEnvCode == BED_VIEW)
                    return "床卡视图";
                else if (applyEnvCode == NURSING_TASK)
                    return "待办任务";
                else if (applyEnvCode == NURSING_STATISTIC)
                    return "查询统计";
                else if (applyEnvCode == INTEGRATE_QUERY)
                    return "综合查询";
                else if (applyEnvCode == NURSING_CONSULT)
                    return "护理会诊";
                else if (applyEnvCode == NURSING_CARE_PLAN)
                    return "护理计划";
                else if (applyEnvCode == NURSING_QC)
                    return "护理质控";
                else if (applyEnvCode == NURSING_MONITOR)
                    return "监护记录";
                else if (applyEnvCode == NURSING_HOME_PAGE)
                    return "系统起始页";
                else if (applyEnvCode == NURSING_WORKSHIFT)
                    return "护理交接班";
                else if (applyEnvCode == SPECIAL_PATIENT)
                    return "专科一览表";
                else if (applyEnvCode == NURSING_SUMMARY)
                    return "护理记录小结";
                else if (applyEnvCode == WHITEBOARD_VIEW)
                    return "护士站白板视图";
                else if (applyEnvCode == SIGNS_DATA_RECORD)
                    return "体征快捷录入窗口";
                else if (applyEnvCode == BATCH_RECORD_FILTER)
                    return "批量录入筛选窗口";
                else if (applyEnvCode == NURSING_REC_CONTENT)
                    return "护理记录内容窗口";
                else if (applyEnvCode == PATIENT_MODULE)
                    return "病人功能模块插件";
                else if (applyEnvCode == GLOBAL_MODULE)
                    return "全局功能模块插件";
                return string.Empty;
            }

            public static string GetApplyEnvCode(string applyEnvName)
            {
                if (applyEnvName == "护理记录")
                    return NURSING_RECORD;
                else if (applyEnvName == "护理文书")
                    return NURSING_DOCUMENT;
                else if (applyEnvName == "护理评估")
                    return NURSING_ASSESSMENT;
                else if (applyEnvName == "文书列表1")
                    return NURDOC_LIST1;
                else if (applyEnvName == "文书列表2")
                    return NURDOC_LIST2;
                else if (applyEnvName == "文书列表3")
                    return NURDOC_LIST3;
                else if (applyEnvName == "文书列表4")
                    return NURDOC_LIST4;
                else if (applyEnvName == "专科护理")
                    return NURSING_SPECIAL;
                else if (applyEnvName == "床卡视图")
                    return BED_VIEW;
                else if (applyEnvName == "待办任务")
                    return NURSING_TASK;
                else if (applyEnvName == "查询统计")
                    return NURSING_STATISTIC;
                else if (applyEnvName == "综合查询")
                    return INTEGRATE_QUERY;
                else if (applyEnvName == "护理会诊")
                    return NURSING_CONSULT;
                else if (applyEnvName == "护理计划")
                    return NURSING_CARE_PLAN;
                else if (applyEnvName == "护理质控")
                    return NURSING_QC;
                else if (applyEnvName == "监护记录")
                    return NURSING_MONITOR;
                else if (applyEnvName == "护理记录小结")
                    return NURSING_SUMMARY;
                else if (applyEnvName == "系统起始页")
                    return NURSING_HOME_PAGE;
                else if (applyEnvName == "护理交接班")
                    return NURSING_WORKSHIFT;
                else if (applyEnvName == "专科一览表")
                    return SPECIAL_PATIENT;
                else if (applyEnvName == "护理记录小结")
                    return NURSING_SUMMARY;
                else if (applyEnvName == "护士站白板视图")
                    return WHITEBOARD_VIEW;
                else if (applyEnvName == "体征快捷录入窗口")
                    return SIGNS_DATA_RECORD;
                else if (applyEnvName == "批量录入筛选窗口")
                    return BATCH_RECORD_FILTER;
                else if (applyEnvName == "护理记录内容窗口")
                    return NURSING_REC_CONTENT;
                else if (applyEnvName == "病人功能模块插件")
                    return PATIENT_MODULE;
                else if (applyEnvName == "全局功能模块插件")
                    return GLOBAL_MODULE;
                return string.Empty;
            }
        }

        /// <summary>
        /// 表格视图显示方案
        /// </summary>
        public struct GridViewSchema
        {
            /// <summary>
            /// 批量录入表格 BATCH_RECORD
            /// </summary>
            public const string BATCH_RECORD = "BATCH_RECORD";

            /// <summary>
            /// 护理记录表格 NURSING_DOCUMENT
            /// </summary>
            public const string NURSING_RECORD = "NURSING_RECORD";

            /// <summary>
            /// 文档索引表格 DOCUMENT_SUMMARY
            /// </summary>
            public const string DOCUMENT_SUMMARY = "DOCUMENT_SUMMARY";

            /// <summary>
            /// 护理申请表格 NURSING_APPLY
            /// </summary>
            public const string NURSING_APPLY = "NURSING_APPLY";

            /// <summary>
            /// 护理计划表格 NURSING_CARE_PLAN
            /// </summary>
            public const string NURSING_CARE_PLAN = "NURSING_CARE_PLAN";
        }

        /// <summary>
        /// 信息库文件类型
        /// </summary>
        public struct InfoLibFileType
        {
            /// <summary>
            /// MP4格式
            /// </summary>
            public const string Mp4 = ".mp4";

            /// <summary>
            /// WMA格式
            /// </summary>
            public const string WMA = ".wmv";

            /// <summary>
            /// doc文档文件
            /// </summary>
            public const string Doc = ".doc";

            /// <summary>
            /// docx文档文件
            /// </summary>
            public const string Docx = ".docx";
        }

        /// <summary>
        /// 数据类型常量
        /// </summary>
        public struct DataType
        {
            /// <summary>
            /// 字符型
            /// </summary>
            public const string CHARACTER = "字符";

            /// <summary>
            /// 数值型
            /// </summary>
            public const string NUMERIC = "数值";

            /// <summary>
            /// 日期型
            /// </summary>
            public const string DATETIME = "日期";

            /// <summary>
            /// 布尔型
            /// </summary>
            public const string BOOLEAN = "布尔";

            /// <summary>
            /// 布尔型
            /// </summary>
            public const string CHECKBOX = "多选";

            public static string[] GetDataTypeNames()
            {
                return new string[] { "字符", "数值", "布尔", "日期", "多选" };
            }
        }

        /// <summary>
        /// 病质控审查状态常量
        /// </summary>
        public struct MedQCStatus
        {
            /// <summary>
            /// 未审查
            /// </summary>
            public const string NO_CHECK = "0";
            /// <summary>
            /// 审查通过
            /// </summary>
            public const string PASS = "1";
            /// <summary>
            /// 审查后存在问题
            /// </summary>
            public const string EXIST_BUG = "2";
            /// <summary>
            /// 审查后存在严重问题
            /// </summary>
            public const string SERIOUS_BUG = "3";
        }

        /// <summary>
        /// 护理计划状态
        /// </summary>
        public struct NurCarePlanStatus
        {
            /// <summary>
            /// 进行中
            /// </summary>
            public const string PROGRESS = "1";

            /// <summary>
            /// 进行中状态描述
            /// </summary>
            public const string PROGRESS_DESC = "进行中";

            /// <summary>
            /// 完成状态
            /// </summary>
            public const string COMPLETE = "2";

            /// <summary>
            /// 完成状态描述
            /// </summary>
            public const string COMPLETE_DESC = "完成";

            /// <summary>
            /// 停止状态
            /// </summary>
            public const string STOP = "3";

            /// <summary>
            /// 停止状态描述
            /// </summary>
            public const string STOP_DESC = "停止";

            /// <summary>
            /// 作废状态
            /// </summary>
            public const string CANCELED = "4";

            /// <summary>
            /// 作废状态描述
            /// </summary>
            public const string CANCELED_DESC = "作废";

            public static string GetStatusDesc(string StatusCode)
            {
                string szStatusDesc = string.Empty;
                switch (StatusCode)
                {
                    case PROGRESS:
                        szStatusDesc = PROGRESS_DESC;
                        break;
                    case COMPLETE:
                        szStatusDesc = COMPLETE_DESC;
                        break;
                    case STOP:
                        szStatusDesc = STOP_DESC;
                        break;
                    case CANCELED:
                        szStatusDesc = CANCELED_DESC;
                        break;
                    default:
                        szStatusDesc = string.Empty;
                        break;
                }
                return szStatusDesc;
            }
        }

        /// <summary>
        /// 文档状态数据
        /// </summary>
        public struct DocStatus
        {
            /// <summary>
            /// 正常可编辑状态0
            /// </summary>
            public const string NORMAL = "0";

            /// <summary>
            /// 已锁定.正在被别人编辑1
            /// </summary>
            public const string LOCKED = "1";

            /// <summary>
            /// 已作废2
            /// </summary>
            public const string CANCELED = "2";

            /// <summary>
            /// 已归档3
            /// </summary>
            public const string ARCHIVED = "3";

            /// <summary>
            /// "警告：\r\n"
            /// "您可能正在其他机器上修改该病历,修改开始于{0},未正常关闭!\r\n"
            /// "在不同机器上同时修改同一份病历,可能会造成文档覆盖,所以还请您保存前先确认!"
            /// </summary>
            public const string LOCKED_STATUS_DESC1 = "警告：\r\n"
                + "您可能正在其他机器上修改该病历,修改开始于{0},未正常关闭!\r\n"
                + "在不同机器上同时修改同一份病历,可能会造成文档覆盖,所以还请您保存前先确认!";

            /// <summary>
            /// 文档已锁定状态描述
            /// “当前病历正在被{0}修订,修订开始于{1}”
            /// </summary>
            public const string LOCKED_STATUS_DESC2 = "当前病历正在被{0}修订,修订开始于{1}!";

            /// <summary>
            /// 文档已作废状态描述
            /// “当前病历已经被{0}于{1}更新或删除”
            /// </summary>
            public const string CANCELED_STATUS_DESC = "当前病历已经被{0}于{1}更新或删除!";

            /// <summary>
            /// 文档已归档状态描述
            /// “当前病历已经被{0}于{1}归档”
            /// </summary>
            public const string ARCHIVED_STATUS_DESC = "当前病历已经被{0}于{1}归档!";
        }

        /// <summary>
        /// 体征数据来源
        /// </summary>
        public struct VitalSignsSourceType
        {
            /// <summary>
            /// 来自护理记录
            /// </summary>
            public const string NUR_REC = "NUR_REC";

            /// <summary>
            /// 来自护理文书
            /// </summary>
            public const string NUR_DOC = "NUR_DOC";
        }

        /// <summary>
        /// 配置文件配置项常量
        /// </summary>
        public struct ConfigFile
        {
            /// <summary>
            /// 配置数据加密密钥
            /// </summary>
            public const string CONFIG_ENCRYPT_KEY = "SUPCON.MEDDOC.ENCRYPT.KEY";

            /// <summary>
            /// 病历数据库类型
            /// </summary>
            public const string NDS_DB_TYPE = "NdsDbType";

            /// <summary>
            /// 护理文书连接模式
            /// </summary>
            public const string NUR_CONN_MODE = "NurConnMode";

            /// <summary>
            /// 病历数据库驱动类型
            /// </summary>
            public const string NDS_PROVIDER_TYPE = "NdsDbProvider";

            /// <summary>
            /// 病历数据库连接串
            /// </summary>
            public const string NDS_CONN_STRING = "NdsDbConnString";

            /// <summary>
            /// 病历数据库RestFul连接串
            /// </summary>
            public const string NRS_CONN_STRING = "NResConnString";
        }

        /// <summary>
        /// 质控审核信息类型
        /// </summary>
        public struct ExamineType
        {
            /// <summary>
            /// 体温单
            /// </summary>
            public const string TEMPERATURE = "体温单";

            /// <summary>
            /// 医嘱单
            /// </summary>
            public const string ORDERS = "医嘱单";

            /// <summary>
            /// 护理记录单
            /// </summary>
            public const string RECORDS = "护理记录单";
        }

        /// <summary>
        /// 质控审核信息状态
        /// </summary>
        public struct ExamineStatus
        {
            /// <summary>
            /// 已审核
            /// </summary>
            public const string QC_OK = "已审核";

            /// <summary>
            /// 标记中
            /// </summary>
            public const string QC_MARK = "标记中";

            /// <summary>
            /// 未审核
            /// </summary>
            public const string QC_NONE = "未审核";
        }

        /// <summary>
        /// 护理信息库目录项
        /// </summary>
        public struct InfoLibList
        {
            /// <summary>
            /// 规章制度
            /// </summary>
            public const string RULE = "Rule";

            /// <summary>
            /// 专科操作
            /// </summary>
            public const string SPECIAL_OPERATION = "Special_Operation";

            /// <summary>
            /// 专科护理
            /// </summary>
            public const string SPECIAL_NURSING = "Special_NURSING";

            /// <summary>
            /// 药物知识
            /// </summary>
            public const string MED_KNOWLEDGE = "Med_KnowLedge";

            /// <summary>
            /// 健康教育
            /// </summary>
            public const string HEALTH_TECH = "HEALTH_TECH";

            /// <summary>
            /// 其他
            /// </summary>
            public const string OTHER = "OTHER";

            public static string GetInfoLibListName(string InfoLibCode)
            {
                if (InfoLibCode == RULE)
                    return "规章制度";
                if (InfoLibCode == SPECIAL_OPERATION)
                    return "专科操作";
                if (InfoLibCode == SPECIAL_NURSING)
                    return "专科护理";
                if (InfoLibCode == MED_KNOWLEDGE)
                    return "药物知识";
                if (InfoLibCode == HEALTH_TECH)
                    return "健康教育";
                if (InfoLibCode == OTHER)
                    return "其他";
                return string.Empty;
            }
        }

        /// <summary>
        /// 配置字典表配置项常量
        /// </summary>
        public struct ConfigKey
        {
            /// <summary>
            /// 配置字典表中文档存储模式配置
            /// </summary>
            public const string STORAGE_MODE = "STORAGE_MODE";

            /// <summary>
            /// 配置字典表中文档存储模式FTP
            /// </summary>
            public const string STORAGE_MODE_FTP = "FTP";

            /// <summary>
            /// 配置字典表中文档存储模式DB
            /// </summary>
            public const string STORAGE_MODE_DB = "DB";

            /// <summary>
            /// 配置字典表中程序升级版本信息
            /// </summary>
            public const string UPGRADE_VERSION = "UPGRADE_VERSION";

            /// <summary>
            /// 配置字典表中FTP程序升级访问参数配置组名称
            /// </summary>
            public const string UPGRADE_FTP = "UPGRADE_FTP";

            /// <summary>
            /// 配置字典表中FTP护理信息库资料访问参数配置组名称
            /// </summary>
            public const string INFOLIB_FTP = "INFOLIB_FTP";

            /// <summary>
            /// 配置字典表中FTP文档库访问参数配置组名称
            /// </summary>
            public const string DOC_FTP = "DOCFTP";

            /// <summary>
            /// 配置字典表中健康教育访问IIS地址
            /// </summary>
            public const string IIS_ADDRESS = "IIS_ADDRESS";

            /// <summary>
            /// 配置字典表中健康教育访问参数配置组名称
            /// </summary>
            public const string IIS = "HEALTHTECH_IIS";

            /// <summary>
            /// 配置字典表中FTP文档库IP
            /// </summary>
            public const string FTP_IP = "IP";

            /// <summary>
            /// 配置字典表中FTP文档库端口
            /// </summary>
            public const string FTP_PORT = "PORT";

            /// <summary>
            /// 配置字典表中FTP文档库用户名
            /// </summary>
            public const string FTP_USER = "USER";

            /// <summary>
            /// 配置字典表中FTP文档库密码
            /// </summary>
            public const string FTP_PWD = "PWD";

            /// <summary>
            /// 配置字典表中FTP协议模式
            /// </summary>
            public const string FTP_MODE = "FTP_MODE";

            /// <summary>
            /// 配置字典表中病历窗口名称配置组
            /// </summary>
            public const string WINDOW_NAME = "WINDOW_NAME";

            /// <summary>
            /// 批量录入窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_BATCH_RECORD = "WINDOW_NAME_BATCH_RECORD";

            /// <summary>
            /// 病人一览表床位窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_BED_VIEW = "WINDOW_NAME_BED_VIEW";

            /// <summary>
            /// 护士交接班窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_WORKSHIFT_RECORD = "WINDOW_NAME_WORKSHIFT_RECORD";

            /// <summary>
            /// 文书列表1窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NURDOC_LIST1 = "WINDOW_NAME_NURDOC_LIST1";

            /// <summary>
            /// 文书列表2窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NURDOC_LIST2 = "WINDOW_NAME_NURDOC_LIST2";

            /// <summary>
            /// 文书列表3窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NURDOC_LIST3 = "WINDOW_NAME_NURDOC_LIST3";

            /// <summary>
            /// 文书列表4窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NURDOC_LIST4 = "WINDOW_NAME_NURDOC_LIST4";

            /// <summary>
            /// 专科护理窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_SPECIAL_NURSING = "WINDOW_NAME_SPECIAL_NURSING";

            /// <summary>
            /// 护理记录窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NURSING_RECORD = "WINDOW_NAME_NURSING_RECORD";

            /// <summary>
            /// 医嘱单窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_ORDERS_RECORD = "WINDOW_NAME_ORDERS_RECORD";

            /// <summary>
            /// 体温单窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_VITAL_SIGNS_GRAPH = "WINDOW_NAME_VITAL_SIGNS_GRAPH";

            /// <summary>
            /// 专科病人一览表窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_SPECIAL_PATIENT = "WINDOW_NAME_SPECIAL_PATIENT";

            /// <summary>
            /// 护理会诊窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NURSING_CONSULT = "WINDOW_NAME_NURSING_CONSULT";

            /// <summary>
            /// 护理计划窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NUR_CARE_PLAN = "WINDOW_NAME_NUR_CARE_PLAN";

            /// <summary>
            /// 全局模块之待办任务窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NURSING_TASK = "WINDOW_NAME_NURSING_TASK";

            /// <summary>
            /// 全局模块之查询统计窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NURSING_STAT = "WINDOW_NAME_NURSING_STAT";

            /// <summary>
            /// 病人模块之综合查询窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_INTEGRATE_QUERY = "WINDOW_NAME_INTEGRATE_QUERY";

            /// <summary>
            /// 病人模块之监护记录单窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_NURSING_MONITOR = "WINDOW_NAME_NURSING_MONITOR";

            /// <summary>
            /// 病人模块之WORD模板窗口名称配置
            /// </summary>
            public const string WINDOW_NAME_WORD_DOCUMENT = "WINDOW_NAME_WORD_DOCUMENT";

            /// <summary>
            /// 配置字典表中用户权限配置组
            /// </summary>
            public const string USER_RIGHT = "USER_RIGHT";

            /// <summary>
            /// 是否开启数据修改审批流程
            /// </summary>
            public const string USER_RIGHT_APPROVEL_DATA_MODIFY = "USER_RIGHT_APPROVEL_DATA_MODIFY";

            /// <summary>
            /// 实习护士可以编辑的数据时效配置
            /// </summary>
            public const string USER_RIGHT_STUDENT_NURSE_EDIT_TIME = "USER_RIGHT_STUDENT_NURSE_EDIT_TIME";

            /// <summary>
            /// 一般护士可以编辑的数据时效配置
            /// </summary>
            public const string USER_RIGHT_GENERAL_NURSE_EDIT_TIME = "USER_RIGHT_GENERAL_NURSE_EDIT_TIME";

            /// <summary>
            /// 质控护士可以编辑的数据时效配置
            /// </summary>
            public const string USER_RIGHT_QUALITY_NURSE_EDIT_TIME = "USER_RIGHT_QUALITY_NURSE_EDIT_TIME";

            /// <summary>
            /// 主管护师可以编辑的数据时效配置
            /// </summary>
            public const string USER_RIGHT_HIGHER_NURSE_EDIT_TIME = "USER_RIGHT_HIGHER_NURSE_EDIT_TIME";

            /// <summary>
            /// 主任护师可以编辑的数据时效配置
            /// </summary>
            public const string USER_RIGHT_HEAD_NURSE_EDIT_TIME = "USER_RIGHT_HEAD_NURSE_EDIT_TIME";

            /// <summary>
            /// 护理部可以编辑的数据时效配置
            /// </summary>
            public const string USER_RIGHT_LEADER_NURSE_EDIT_TIME = "USER_RIGHT_LEADER_NURSE_EDIT_TIME";

            /// <summary>
            /// 实习护士可以审核多长时间内其他护士提交的审核请求配置
            /// </summary>
            public const string USER_RIGHT_STUDENT_NURSE_GRANT_TIME = "USER_RIGHT_STUDENT_NURSE_GRANT_TIME";

            /// <summary>
            /// 一般护士可以审核多长时间内其他护士提交的审核请求配置
            /// </summary>
            public const string USER_RIGHT_GENERAL_NURSE_GRANT_TIME = "USER_RIGHT_GENERAL_NURSE_GRANT_TIME";

            /// <summary>
            /// 质控护士可以审核多长时间内其他护士提交的审核请求配置
            /// </summary>
            public const string USER_RIGHT_QUALITY_NURSE_GRANT_TIME = "USER_RIGHT_QUALITY_NURSE_GRANT_TIME";

            /// <summary>
            /// 主管护师可以审核多长时间内其他护士提交的审核请求配置
            /// </summary>
            public const string USER_RIGHT_HIGHER_NURSE_GRANT_TIME = "USER_RIGHT_HIGHER_NURSE_GRANT_TIME";

            /// <summary>
            /// 主任护师可以审核多长时间内其他护士提交的审核请求配置
            /// </summary>
            public const string USER_RIGHT_HEAD_NURSE_GRANT_TIME = "USER_RIGHT_HEAD_NURSE_GRANT_TIME";

            /// <summary>
            /// 护理部可以审核多长时间内其他护士提交的审核请求配置
            /// </summary>
            public const string USER_RIGHT_LEADER_NURSE_GRANT_TIME = "USER_RIGHT_LEADER_NURSE_GRANT_TIME";

            /// <summary>
            /// 护理文书文档列表默认加载时间（h）
            /// </summary>
            public const string DOC_LIST_LOAD_TIME = "DOC_LIST_LOAD_TIME";

            /// <summary>
            /// 配置字典表中病历可选功能配置组
            /// </summary>
            public const string SYSTEM_OPTION = "SYSTEM_OPTION";

            /// <summary>
            /// 护理病历系统产品授权代码
            /// </summary>
            public const string SYSTEM_OPTION_CERT_CODE = "CERT_CODE";

            /// <summary>
            /// 护理病历系统产品授权医院名称
            /// </summary>
            public const string SYSTEM_OPTION_HOSPITAL_NAME = "HOSPITAL_NAME";

            /// <summary>
            /// 0表示多病人窗口模式，1表示单病人窗口模式
            /// </summary>
            public const string SYSTEM_OPTION_SINGLE_PATIENT = "SINGLE_PATIENT_MODE";

            /// <summary>
            /// 总后版体征数据 
            /// </summary>
            public const string SYSTEM_OPTION_EMRS_VITAL = "EMRS_VITAL";

            /// <summary>
            /// 护理记录是否需要双签名
            /// </summary>
            public const string SYSTEM_OPTION_NURREC_DOUBLESIGN = "NURREC_DOUBLESIGN";

            /// <summary>
            /// 0-批量录入窗口不显示打印按钮
            /// 1-批量录入窗口显示打印按钮
            /// </summary>
            public const string SYSTEM_OPTION_OPTION_BATCH_RECORD_PRINT = "OPTION_BATCH_RECORD_PRINT";

            /// <summary>
            /// 0-护理记录单单条记录录入方式
            /// 1-护理记录单表格方式录入方式
            /// </summary>
            public const string SYSTEM_OPTION_REC_TABLE_INPUT = "OPTION_REC_TABLE_INPUT";

            /// <summary>
            /// 0-护理记录集中显示
            /// 1-护理记录显示按列显示方案归类显示
            /// </summary>
            public const string SYSTEM_OPTION_REC_SHOW_BY_TYPE = "OPTION_REC_SHOW_BY_TYPE";

            /// <summary>
            /// 护理记录显示按列显示方案归类显示开始时间
            /// </summary>
            public const string SYSTEM_OPTION_REC_SHOW_BY_TYPE_START_TIME = "OPTION_REC_SHOW_BY_TYPE_START_TIME";

            /// <summary>
            /// 任务提醒时间间隔
            /// </summary>
            public const string SYSTEM_OPTION_TASK_MESSAGE_TIME = "TASK_MESSAGE_TIME";

            /// <summary>
            /// 0-医嘱本窗口不显示打印按钮
            /// 1-医嘱本窗口显示打印按钮
            /// </summary>
            public const string SYSTEM_OPTION_OPTION_ORDERS_PRINT = "OPTION_ORDERS_PRINT";

            /// <summary>
            /// 0- 默认计算24小时出入量小结时，当天早上7点为昨天的24小结量，昨天的早上7点不算。
            /// 1- 计算24小时出入量小结时，当天早上7点不算为昨天24小结量，而昨天的早上7点算24小结量。
            /// 2-  计算24小结入量的时候，当天早上7点不算为昨天24小结量，而昨天的早上7点算24小结量；计算24小结出量时，
            ///     当天早上7点为昨天的24小结量，昨天的早上7点不算。
            /// </summary>
            public const string SYSTEM_OPTION_OPTION_CALCULATE_SUMMARY = "OPTION_CALCULATE_SUMMARY";

            /// <summary>
            /// 后台配置体征表中存储测试血压次数
            /// </summary>
            public const string SYSTEM_OPTION_OPTION_VITAL_BP_COUNT = "OPTION_VITAL_BP_COUNT";

            /// <summary>
            /// 后台配置文档是否归档校验
            /// 0- 不校验归档状态
            /// 1- 校验归档状态
            /// </summary>
            public const string DOC_PIGEONHOLE = "DOC_PIGEONHOLE";

            /// <summary>
            /// 护理小结模板名称
            /// </summary>
            public const string REC_SUMMARYNAME = "REC_SUMMARYNAME";

            /// <summary>
            /// 是否开启医嘱拆分执行计划
            /// </summary>
            public const string PLAN_ORDERS_REC = "PLAN_ORDERS_REC";

            /// <summary>
            /// 是否开启转科护理自动新增
            /// </summary>
            public const string SYSTEM_SPECIAL_NURSING_INCREASE = "SPECIAL_NURSING_INCREASE";

            /// <summary>
            /// 是否开启护理记录的单位非ml的判断
            /// </summary>
            public const string SYSTEM_NURRECORD_UNIT = "NURRECORD_UNIT";

            /// <summary>
            /// 是否开启护理计划新版配置（杭三修改）需搭配系统起始页（护理计划映射表）使用
            /// </summary>
            public const string NPC_NEW_MODEL = "NPC_NEW_MODEL";

            /// <summary>
            /// 是否开启表格式护理记录单 体征数据同步
            /// </summary>
            public const string VITAL_SIGNS_SYNC = "VITAL_SIGNS_SYNC";

            /// <summary>
            /// 义乌护理计划排序  可用于其他状态为1,2,3的排序  及进行中  完成  停止  这样排
            /// </summary>
            public const string NCP_LIST_SORT_YW = "NCP_LIST_SORT_YW";

            /// <summary>
            /// 护理记录列配置 是否将病区和全院一起显示 1是 0否 默认否
            /// </summary>
            public const string NUR_REC_SCHEMA_LIST_SHOWALL = "NUR_REC_SCHEMA_LIST_SHOWALL";

            /// <summary>
            /// 专科列表删除时 是否打开文档 1不打开  0打开
            /// </summary>
            public const string NUR_SPECIAL_DELETE_DIRECTLY = "NUR_SPECIAL_DELETE_DIRECTLY";

            /// <summary>
            /// 护理交接班项目排序开关 1打开 0关闭
            /// </summary>
            public const string NUR_SHIFT_ITEM_SORT = "NUR_SHIFT_ITEM_SORT";

            /// <summary>
            /// 护理记录单打印最大预览页数 防止内存不够问题
            /// </summary>
            public const string NUR_REC_MAX_PREVIEWPAGES = "NUR_REC_MAX_PREVIEWPAGES";
        }

        /// <summary>
        /// 公共字典类型常量
        /// </summary>
        public struct CommonDictType
        {
            /// <summary>
            /// 护理小组字典
            /// </summary>
            public const string USER_GROUP = "USER_GROUP";

            /// <summary>
            /// 护理计划护理诊断字典
            /// </summary>
            public const string NUR_DIAGNOSIS = "NUR_DIAGNOSIS";

            /// <summary>
            /// 护理计划相关因素字典
            /// </summary>
            public const string NCP_FACTOR = "NCP_FACTOR";

            /// <summary>
            /// 护理计划护理措施字典
            /// </summary>
            public const string NCP_INTERVENTION = "NCP_INTERVENTION";

            /// <summary>
            /// 护理计划预期目标字典
            /// </summary>
            public const string NCP_TARGET = "NCP_TARGET";

            /// <summary>
            /// 评估项目
            /// </summary>
            public const string NCP_ASSESSITEM = "NCP_ASSESSITEM";

            /// <summary>
            /// 特殊项
            /// </summary>
            public const string SPECIAL_ITEM = "SPECIAL_ITEM";

            /// <summary>
            /// 诊疗事件
            /// </summary>
            public const string DIAGNOSIS_EVENT = "DIAGNOSIS_EVENT";

            /// <summary>
            /// 护理交班交班项目字典
            /// </summary>
            public const string SHIFT_ITEM = "SHIFT_ITEM";

            public static string[] GetNCPItemType()
            {
                return new string[]{
                    "相关因素",
                    "预期目标",
                    "护理措施",
                    "评估项目"};
            }

            public static string GetItemTypeByName(string szItemName)
            {
                if (szItemName == "护理诊断")
                    return "NUR_DIAGNOSIS";
                else if (szItemName == "相关因素")
                    return "NCP_FACTOR";
                else if (szItemName == "预期目标")
                    return "NCP_TARGET";
                else if (szItemName == "护理措施")
                    return "NCP_INTERVENTION";
                else if (szItemName == "评估项目")
                    return "NCP_ASSESSITEM";
                else if (szItemName == "护理小组")
                    return "USER_GROUP";
                else if (szItemName == "交班项目")
                    return "SHIFT_ITEM";
                else if (szItemName == "特殊项")
                    return "SPECIAL_ITEM";
                else if (szItemName == "诊疗事件")
                    return "DIAGNOSIS_EVENT";
                return szItemName;
            }

            public static string GetItemTypeByCode(string szItemCode)
            {
                if (szItemCode == "NUR_DIAGNOSIS")
                    return "护理诊断";
                else if (szItemCode == "NCP_FACTOR")
                    return "相关因素";
                else if (szItemCode == "NCP_ASSESSITEM")
                    return "评估项目";
                else if (szItemCode == "NCP_TARGET")
                    return "预期目标";
                else if (szItemCode == "NCP_INTERVENTION")
                    return "护理措施";
                else if (szItemCode == "USER_GROUP")
                    return "护理小组";
                else if (szItemCode == "SHIFT_ITEM")
                    return "交班项目";
                else if (szItemCode == "SPECIAL_ITEM")
                    return "特殊项";
                else if (szItemCode == "DIAGNOSIS_EVENT")
                    return "诊疗事件";
                return szItemCode;
            }
        }

        /// <summary>
        /// 护理申请单类型常量
        /// </summary>
        public struct NurApplyType
        {
            /// <summary>
            /// 护理会诊申请
            /// </summary>
            public const string CONSULTATION_APPLY = "会诊申请";

            /// <summary>
            /// 压疮申报
            /// </summary>
            public const string BEDSORE_DECLARE = "压疮申报";
        }

        /// <summary>
        /// 病人性别
        /// </summary>
        public struct PatientSex
        {
            /// <summary>
            /// 男性
            /// </summary>
            public const string Male = "男";

            /// <summary>
            /// 女性
            /// </summary>
            public const string Female = "女";

            /// <summary>
            /// 未知
            /// </summary>
            public const string Unknow = "未知";
        }

        #region "视图各字段定义"
        /// <summary>
        /// RDB视图
        /// </summary>
        internal struct DataView
        {
            /// <summary>
            /// 用户视图
            /// </summary>
            public const string USER = "USER_V";

            /// <summary>
            /// 科室视图
            /// </summary>
            public const string DEPT = "DEPT_V";

            /// <summary>
            /// 用户科室视图
            /// </summary>
            public const string USER_DEPT = "USER_DEPT_V";

            /// <summary>
            /// 床位记录视图
            /// </summary>
            public const string BED_RECORD = "BED_RECORD_V";

            /// <summary>
            /// 就诊信息视图
            /// </summary>
            public const string PAT_VISIT = "PAT_VISIT_V";

            /// <summary>
            /// 在院病人视图
            /// </summary>
            public const string INP_VISIT = "INP_VISIT_V";

            /// <summary>
            /// 体征信息视图
            /// </summary>
            public const string VITAL_SIGNS = "VITAL_SIGNS_V";

            /// <summary>
            /// 医生医嘱视图
            /// </summary>
            public const string DOCTOR_ORDERS = "DOCTOR_ORDERS_V";

            /// <summary>
            /// 抄对医嘱视图
            /// </summary>
            public const string PERFORM_ORDERS = "PERFORM_ORDERS_V";

            /// <summary>
            /// 病人在科记录视图
            /// </summary>
            public const string TRANSFER = "TRANSFER_V";

            /// <summary>
            /// 检查主索引数据视图
            /// </summary>
            public const string EXAM_MASTER = "EXAM_MASTER_V";

            /// <summary>
            /// 检查报告数据视图
            /// </summary>
            public const string EXAM_RESULT = "EXAM_RESULT_V";

            /// <summary>
            /// 检验主记录数据视图
            /// </summary>
            public const string LAB_MASTER = "LAB_MASTER_V";

            /// <summary>
            /// 检验结果数据视图
            /// </summary>
            public const string LAB_RESULT = "LAB_RESULT_V";

            /// <summary>
            /// 医嘱拆分数据视图
            /// </summary>
            public const string PLAN_ORDERS = "PLAN_ORDERS_V";
            /// <summary>
            /// 反馈信息模板视图
            /// </summary>
            public const string MSG_TEMPLET_V = "MSG_TEMPLET_V";
            /// <summary>
            /// 反馈信息类别视图
            /// </summary>
            public const string MSG_TYPE_V = "MSG_TYPE_V";
            /// <summary>
            /// 病人反馈信息视图
            /// </summary>
            public const string QC_MSG_V = "QC_MSG_V";
        }

        /// <summary>
        /// 用户视图字段定义
        /// </summary>
        internal struct UserView
        {
            /// <summary>
            /// 用户ID
            /// </summary>
            public const string USER_ID = "USER_ID";

            /// <summary>
            /// 用户名字
            /// </summary>
            public const string USER_NAME = "USER_NAME";

            /// <summary>
            /// 科室代码
            /// </summary>
            public const string DEPT_CODE = "DEPT_CODE";

            /// <summary>
            /// 科室名字
            /// </summary>
            public const string DEPT_NAME = "DEPT_NAME";
        }

        /// <summary>
        /// 科室视图字段定义
        /// </summary>
        internal struct DeptView
        {
            /// <summary>
            /// 科室代码
            /// </summary>
            public const string DEPT_CODE = "DEPT_CODE";

            /// <summary>
            /// 科室名称
            /// </summary>
            public const string DEPT_NAME = "DEPT_NAME";

            /// <summary>
            /// 是临床科室
            /// </summary>
            public const string IS_CLINIC = "IS_CLINIC";

            /// <summary>
            /// 是门诊科室
            /// </summary>
            public const string IS_OUTP = "IS_OUTP";

            /// <summary>
            /// 是病区
            /// </summary>
            public const string IS_WARD = "IS_WARD";

            /// <summary>
            /// 是护理单元
            /// </summary>
            public const string IS_NURSE = "IS_NURSE";

            /// <summary>
            /// 是用户组
            /// </summary>
            public const string IS_GROUP = "IS_GROUP";

            /// <summary>
            /// 护理单元代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 输入码
            /// </summary>
            public const string INPUT_CODE = "INPUT_CODE";
        }

        /// <summary>
        /// 用户科室视图字段定义
        /// </summary>
        internal struct UserDeptView
        {
            /// <summary>
            /// 用户ID
            /// </summary>
            public const string USER_ID = "USER_ID";

            /// <summary>
            /// 科室代码
            /// </summary>
            public const string DEPT_CODE = "DEPT_CODE";
        }

        /// <summary>
        /// 床位记录视图字段定义
        /// </summary>
        internal struct BedRecordView
        {
            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 科室代码
            /// </summary>
            public const string DEPT_CODE = "DEPT_CODE";

            /// <summary>
            /// 科室名称
            /// </summary>
            public const string DEPT_NAME = "DEPT_NAME";

            /// <summary>
            /// 床位号
            /// </summary>
            public const string BED_NO = "BED_NO";

            /// <summary>
            /// 床位名称
            /// </summary>
            public const string BED_LABEL = "BED_LABEL";

            /// <summary>
            /// 编制类型
            /// </summary>
            public const string BED_APPROVED_TYPE = "BED_APPROVED_TYPE";

            /// <summary>
            /// 床类型
            /// </summary>
            public const string BED_SEX_TYPE = "BED_SEX_TYPE";

            /// <summary>
            /// 床位等级
            /// </summary>
            public const string BED_CLASS = "BED_CLASS";

            /// <summary>
            /// 床位状态
            /// </summary>
            public const string BED_STATUS = "BED_STATUS";
        }

        /// <summary>
        /// 就诊信息数据视图字段定义
        /// </summary>
        internal struct PatVisitView
        {
            /// <summary>
            /// 病人号
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 病人姓名
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 病人性别
            /// </summary>
            public const string PATIENT_SEX = "PATIENT_SEX";

            /// <summary>
            /// 出生时间
            /// </summary>
            public const string BIRTH_TIME = "BIRTH_TIME";

            /// <summary>
            /// 出生地
            /// </summary>
            public const string BIRTH_PLACE = "BIRTH_PLACE";

            /// <summary>
            /// 民族
            /// </summary>
            public const string NATION = "NATION";

            /// <summary>
            /// 家庭住址
            /// </summary>
            public const string ADDRESS = "ADDRESS";

            /// <summary>
            /// 工作单位
            /// </summary>
            public const string SERVICE_AGENCY = "SERVICE_AGENCY";

            /// <summary>
            /// 婚姻状况
            /// </summary>
            public const string MARITAL_STATUS = "MARITAL_STATUS";

            /// <summary>
            /// 费别
            /// </summary>
            public const string CHARGE_TYPE = "CHARGE_TYPE";

            /// <summary>
            /// 机密等级
            /// </summary>
            public const string SECRET_LEVEL = "SECRET_LEVEL";

            /// <summary>
            /// 住院号
            /// </summary>
            public const string INP_NO = "INP_NO";

            /// <summary>
            /// 就诊号
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 就诊时间
            /// </summary>
            public const string VISIT_TIME = "VISIT_TIME";

            /// <summary>
            /// 就诊类型
            /// </summary>
            public const string VISIT_TYPE = "VISIT_TYPE";

            /// <summary>
            /// 科室代码
            /// </summary>
            public const string DEPT_CODE = "DEPT_CODE";

            /// <summary>
            /// 科室名称
            /// </summary>
            public const string DEPT_NAME = "DEPT_NAME";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 床位代码
            /// </summary>
            public const string BED_CODE = "BED_CODE";

            /// <summary>
            /// 诊断名字
            /// </summary>
            public const string DIAGNOSIS = "DIAGNOSIS";

            /// <summary>
            /// 过敏药物
            /// </summary>
            public const string ALLERGY_DRUGS = "ALLERGY_DRUGS";

            /// <summary>
            /// 病人病情状态
            /// </summary>
            public const string PATIENT_CONDITION = "PATIENT_CONDITION";

            /// <summary>
            /// 护理等级
            /// </summary>
            public const string NURSING_CLASS = "NURSING_CLASS";

            /// <summary>
            /// 入科时间
            /// </summary>
            public const string ADM_TIME = "ADM_TIME";

            /// <summary>
            /// 病人经治医生
            /// </summary>
            public const string INCHARGE_DOCTOR = "INCHARGE_DOCTOR";

            /// <summary>
            /// 出院时间
            /// </summary>
            public const string DISCHARGE_TIME = "DISCHARGE_TIME";

            /// <summary>
            /// 病人出院方式
            /// </summary>
            public const string DISCHARGE_MODE = "DISCHARGE_MODE";
        }

        /// <summary>
        /// 在院病人信息视图字段定义
        /// </summary>
        internal struct InPVisitView
        {
            /// <summary>
            /// 病人号
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 病人姓名
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 病人性别
            /// </summary>
            public const string PATIENT_SEX = "PATIENT_SEX";

            /// <summary>
            /// 病人生日
            /// </summary>
            public const string BIRTH_TIME = "BIRTH_TIME";

            /// <summary>
            /// 病人费别
            /// </summary>
            public const string CHARGE_TYPE = "CHARGE_TYPE";

            /// <summary>
            /// 就诊号
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 就诊时间
            /// </summary>
            public const string VISIT_TIME = "VISIT_TIME";

            /// <summary>
            /// 床位代码
            /// </summary>
            public const string BED_CODE = "BED_CODE";

            /// <summary>
            /// 床标号
            /// </summary>
            public const string BED_LABEL = "BED_LABEL";

            /// <summary>
            /// 科室代码
            /// </summary>
            public const string DEPT_CODE = "DEPT_CODE";

            /// <summary>
            /// 科室名称
            /// </summary>
            public const string DEPT_NAME = "DEPT_NAME";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 医生姓名
            /// </summary>
            public const string INCHARGE_DOCTOR = "INCHARGE_DOCTOR";

            /// <summary>
            /// 诊断名字
            /// </summary>
            public const string DIAGNOSIS = "DIAGNOSIS";

            /// <summary>
            /// 过敏药物
            /// </summary>
            public const string ALLERGY_DRUGS = "ALLERGY_DRUGS";

            /// <summary>
            /// 就诊预交金
            /// </summary>
            public const string PREPAYMENTS = "PREPAYMENTS";

            /// <summary>
            /// 治疗费用总计
            /// </summary>
            public const string TOTAL_COSTS = "TOTAL_COSTS";

            /// <summary>
            /// 已交治疗费用
            /// </summary>
            public const string TOTAL_CHARGES = "TOTAL_CHARGES";

            /// <summary>
            /// 病人病情状况(中文)
            /// </summary>
            public const string PATIENT_CONDITION = "PATIENT_CONDITION";

            /// <summary>
            /// 病人护理等级(中文)
            /// </summary>
            public const string NURSING_CLASS = "NURSING_CLASS";
        }

        /// <summary>
        /// 病人在科记录视图
        /// </summary>
        internal struct TransferView
        {
            /// <summary>
            /// 病人ID号
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 病人就诊号
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 病人呆过的科室
            /// </summary>
            public const string DEPT_STAYED = "DEPT_STAYED";

            /// <summary>
            /// 入院时间
            /// </summary>
            public const string ADMISSION_DATE_TIME = "ADMISSION_DATE_TIME";

            /// <summary>
            /// 出院时间
            /// </summary>
            public const string DISCHARGE_DATE_TIME = "DISCHARGE_DATE_TIME";

            /// <summary>
            /// 转入科室
            /// </summary>
            public const string DEPT_TRANSFERED_TO = "DEPT_TRANSFERED_TO";

            /// <summary>
            /// 医生
            /// </summary>
            public const string DOCTOR_IN_CHARGE = "DOCTOR_IN_CHARGE";

            /// <summary>
            /// 医生科室
            /// </summary>
            public const string DOCTOR_DEPT = "DOCTOR_DEPT";

            /// <summary>
            /// 床位标号
            /// </summary>
            public const string BED_LABEL = "BED_LABEL";
        }

        /// <summary>
        /// 体征数据信息视图字段定义
        /// </summary>
        internal struct VitalSignsView
        {
            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 就诊ID
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 病人子ID
            /// </summary>
            public const string SUB_ID = "SUB_ID";

            /// <summary>
            /// 录入日期
            /// </summary>
            public const string RECORD_DATE = "RECORD_DATE";

            /// <summary>
            /// 录入时间
            /// </summary>
            public const string RECORD_TIME = "RECORD_TIME";

            /// <summary>
            /// 体征名称
            /// </summary>
            public const string RECORD_NAME = "RECORD_NAME";

            /// <summary>
            /// 体征数据
            /// </summary>
            public const string RECORD_DATA = "RECORD_DATA";

            /// <summary>
            /// 体征数据类型
            /// </summary>
            public const string DATA_TYPE = "DATA_TYPE";

            /// <summary>
            /// 体征数据单位
            /// </summary>
            public const string DATA_UNIT = "DATA_UNIT";

            /// <summary>
            /// 0为非体征数据不保存；1为体征数据保存；2为体征数据但是只显示在集成视图中。
            /// </summary>
            public const string CATEGORY = "CATEGORY";

            /// <summary>
            /// 体征数据备注
            /// </summary>
            public const string REMARKS = "REMARKS";

            /// <summary>
            /// 体征数据来源标记
            /// </summary>
            public const string SOURCE_TAG = "SOURCE_TAG";

            /// <summary>
            /// 体征数据来源类型
            /// </summary>
            public const string SOURCE_TYPE = "SOURCE_TYPE";
        }

        /// <summary>
        /// 医生医嘱表字段定义
        /// </summary>
        internal struct OrdersView
        {
            /// <summary>
            /// 病人标识号
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 就诊号
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 医嘱序号
            /// </summary>
            public const string ORDER_NO = "ORDER_NO";

            /// <summary>
            /// 医嘱子序号
            /// </summary>
            public const string ORDER_SUB_NO = "ORDER_SUB_NO";

            /// <summary>
            /// 长期医嘱标志
            /// </summary>
            public const string REPEAT_INDICATOR = "REPEAT_INDICATOR";

            /// <summary>
            /// 医嘱类别
            /// </summary>
            public const string ORDER_CLASS = "ORDER_CLASS";

            /// <summary>
            /// 医嘱下达时间
            /// </summary>
            public const string ENTER_DATE_TIME = "ENTER_DATE_TIME";

            /// <summary>
            /// 医嘱内容
            /// </summary>
            public const string ORDER_TEXT = "ORDER_TEXT";

            /// <summary>
            /// 是否自带药
            /// </summary>
            public const string DRUG_BILLING_ATTR = "DRUG_BILLING_ATTR";

            /// <summary>
            /// 剂量
            /// </summary>
            public const string DOSAGE = "DOSAGE";

            /// <summary>
            /// 计量单位
            /// </summary>
            public const string DOSAGE_UNITS = "DOSAGE_UNITS";

            /// <summary>
            /// 途径
            /// </summary>
            public const string ADMINISTRATION = "ADMINISTRATION";

            /// <summary>
            /// 频次
            /// </summary>
            public const string FREQUENCY = "FREQUENCY";

            /// <summary>
            /// 医生说明
            /// </summary>
            public const string FREQ_DETAIL = "FREQ_DETAIL";

            /// <summary>
            /// 带药量
            /// </summary>
            public const string PACK_COUNT = "PACK_COUNT";

            /// <summary>
            /// 医嘱停止时间
            /// </summary>
            public const string END_DATE_TIME = "END_DATE_TIME";

            /// <summary>
            /// 医生
            /// </summary>
            public const string DOCTOR = "DOCTOR";

            /// <summary>
            /// 护士
            /// </summary>
            public const string NURSE = "NURSE";

            /// <summary>
            /// 新开停止医嘱标志
            /// </summary>
            public const string START_STOP_INDICATOR = "START_STOP_INDICATOR";

            /// <summary>
            /// 医嘱状态
            /// </summary>
            public const string ORDER_STATUS = "ORDER_STATUS";

            /// <summary>
            /// 医嘱标识
            /// </summary>
            public const string ORDER_FLAG = "ORDER_FLAG";

            /// <summary>
            /// 医嘱执行时间
            /// </summary>
            public const string PERFORM_SCHEDULE = "PERFORM_SCHEDULE";

            //市三新增字段  2013-1-9

            /// <summary>
            /// 停止医生签名
            /// </summary>
            public const string STOP_DOCTOR = "STOP_DOCTOR";

            /// <summary>
            /// 长期停止执行时间
            /// </summary>
            public const string PROCESSING_STOP_DATE_TIME = "PROCESSING_STOP_DATE_TIME";

            /// <summary>
            /// 长期停止护士签名
            /// </summary>
            public const string STOP_NURSE = "STOP_NURSE";

            /// <summary>
            /// 临时医嘱停止时间
            /// </summary>
            public const string SHORT_PROCESSING_STOP_DATE = "SHORT_PROCESSING_STOP_DATE";

            /// <summary>
            /// 临时医嘱停止护士签名
            /// </summary>
            public const string SHORT_STOP_NURSE = "SHORT_STOP_NURSE";

            /// <summary>
            /// 皮试结果
            /// </summary>
            public const string PERFORM_RESULT = "PERFORM_RESULT";
        }

        /// <summary>
        /// 检验主记录表字段定义
        /// </summary>
        internal struct LabMasterView
        {
            /// <summary>
            /// 病人标识
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 本次住院标识
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 申请序号
            /// </summary>
            public const string TEST_ID = "TEST_ID";

            /// <summary>
            /// 检验主题
            /// </summary>
            public const string SUBJECT = "SUBJECT";

            /// <summary>
            /// 检验标本
            /// </summary>
            public const string SPECIMEN = "SPECIMEN";

            /// <summary>
            /// 申请时间
            /// </summary>
            public const string REQUEST_TIME = "REQUEST_TIME";

            /// <summary>
            /// 申请医生
            /// </summary>
            public const string REQUEST_DOCTOR = "REQUEST_DOCTOR";

            /// <summary>
            /// 报告状态
            /// </summary>
            public const string RESULT_STATUS = "RESULT_STATUS";

            /// <summary>
            /// 报告时间
            /// </summary>
            public const string REPORT_TIME = "REPORT_TIME";

            /// <summary>
            /// 报告医生
            /// </summary>
            public const string REPORT_DOCTOR = "REPORT_DOCTOR";
        }

        /// <summary>
        /// 检验结果表字段定义
        /// </summary>
        internal struct LabResultView
        {
            /// <summary>
            /// 申请序号
            /// </summary>
            public const string TEST_ID = "TEST_ID";

            /// <summary>
            /// 检验报告项目名称
            /// </summary>
            public const string ITEM_NO = "ITEM_NO";

            /// <summary>
            /// 检验报告项目名称
            /// </summary>
            public const string ITEM_NAME = "ITEM_NAME";

            /// <summary>
            /// 检验结果值
            /// </summary>
            public const string ITEM_RESULT = "ITEM_RESULT";

            /// <summary>
            /// 检验结果单位
            /// </summary>
            public const string ITEM_UNITS = "ITEM_UNITS";

            /// <summary>
            /// 检验结果参考值
            /// </summary>
            public const string ITEM_REFER = "ITEM_REFER";

            /// <summary>
            /// 结果正常标志
            /// </summary>
            public const string ABNORMAL_INDICATOR = "ABNORMAL_INDICATOR";
        }

        /// <summary>
        /// 检查主表字段定义
        /// </summary>
        internal struct ExamMasterView
        {
            /// <summary>
            /// 病人标识
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 本次住院标识
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 申请序号
            /// </summary>
            public const string EXAM_ID = "EXAM_ID";

            /// <summary>
            /// 检查类别
            /// </summary>
            public const string SUBJECT = "SUBJECT";

            /// <summary>
            /// 申请日期
            /// </summary>
            public const string REQUEST_TIME = "REQUEST_TIME";

            /// <summary>
            /// 申请医生
            /// </summary>
            public const string REQUEST_DOCTOR = "REQUEST_DOCTOR";

            /// <summary>
            /// 报告状态
            /// </summary>
            public const string RESULT_STATUS = "RESULT_STATUS";

            /// <summary>
            /// 报告日期
            /// </summary>
            public const string REPORT_TIME = "REPORT_TIME";

            /// <summary>
            /// 报告人
            /// </summary>
            public const string REPORT_DOCTOR = "REPORT_DOCTOR";
        }

        /// <summary>
        /// 检查报告表字段定义
        /// </summary>
        internal struct ExamResultView
        {
            /// <summary>
            /// 申请序号
            /// </summary>
            public const string EXAM_ID = "EXAM_ID";

            /// <summary>
            /// 检查参数
            /// </summary>
            public const string PARAMETERS = "PARAMETERS";

            /// <summary>
            /// 检查所见
            /// </summary>
            public const string DESCRIPTION = "DESCRIPTION";

            /// <summary>
            /// 检查印象
            /// </summary>
            public const string IMPRESSION = "IMPRESSION";

            /// <summary>
            /// 检查建议
            /// </summary>
            public const string RECOMMENDATION = "RECOMMENDATION";

            /// <summary>
            /// 是否阳性
            /// </summary>
            public const string IS_ABNORMAL = "IS_ABNORMAL";
        }

        /// <summary>
        /// 医嘱拆分字段定义
        /// </summary>
        internal struct PlanOrdersView
        {
            /// <summary>
            /// 病人标识
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 本次住院标识
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 医嘱号
            /// </summary>
            public const string ORDER_NO = "ORDER_NO";

            /// <summary>
            /// 拆分序号
            /// </summary>
            public const string ORDERPLAN_SUB_NO = "ORDERPLAN_SUB_NO";

            /// <summary>
            /// 执行类别
            /// </summary>
            public const string EXEC_CLASS = "EXEC_CLASS";

            /// <summary>
            /// 编辑状态
            /// </summary>
            public const string PLAN_STATE = "PLAN_STATE";

            /// <summary>
            /// 计划执行时间
            /// </summary>
            public const string SCHEDULE_TIME = "SCHEDULE_TIME";

            /// <summary>
            /// 执行状态
            /// </summary>
            public const string PERFORMED_FLAG = "PERFORMED_FLAG";

            /// <summary>
            /// 执行人
            /// </summary>
            public const string OPERATOR = "OPERATOR";

            /// <summary>
            /// 执行时间
            /// </summary>
            public const string OPERATING_TIME = "OPERATING_TIME";

            /// <summary>
            /// 实入量
            /// </summary>
            public const string REAL_DOSAGE = "REAL_DOSAGE";

            /// <summary>
            /// 计划执行类别
            /// </summary>
            public const string SCHEDULE_TYPE = "SCHEDULE_TYPE";

            /// <summary>
            /// 执行备忘
            /// </summary>
            public const string OPERATOR_MEMO = "OPERATOR_MEMO";

            /// <summary>
            /// 执行结果时间
            /// </summary>
            public const string OPERATING_END_TIME = "OPERATING_END_TIME";

            /// <summary>
            /// 执行结果
            /// </summary>
            public const string PERFORM_RESULT = "PERFORM_RESULT";

            /// <summary>
            /// 贴瓶条码
            /// </summary>
            public const string BAR_CODE = "BAR_CODE";

            /// <summary>
            /// 签名人
            /// </summary>
            public const string SIGNER = "SIGNER";

            /// <summary>
            /// 是否已经打印
            /// </summary>
            public const string IS_PRINTED = "IS_PRINTED";

            /// <summary>
            /// 每天执行顺序
            /// </summary>
            public const string DAILY_EXC_NO = "DAILY_EXC_NO";
        }
        #endregion

        #region"数据表字段定义"
        /// <summary>
        /// RDB数据表
        /// </summary>
        internal struct DataTable
        {
            /// <summary>
            /// 表格视图列定义表
            /// </summary>
            public const string GRID_VIEW_COLUMN = "GRID_VIEW_COLUMN";

            /// <summary>
            /// 表格视图列方案表
            /// </summary>
            public const string GRID_VIEW_SCHEMA = "GRID_VIEW_SCHEMA";

            /// <summary>
            /// 文档摘要数据
            /// </summary>
            public const string DOC_SUMMARY_DATA = "DOC_SUMMARY_DATA";

            /// <summary>
            /// 文档类型表
            /// </summary>
            public const string DOC_TYPE = "NUR_DOC_TYPE";

            /// <summary>
            /// 科室文档类型配置表
            /// </summary>
            public const string WARD_DOC_TYPE = "WARD_DOC_TYPE";

            /// <summary>
            /// 报表类型表
            /// </summary>
            public const string REPORT_TYPE = "NUR_REPORT_TYPE";

            /// <summary>
            /// 科室报表类型配置表
            /// </summary>
            public const string WARD_REPORT_TYPE = "WARD_REPORT_TYPE";

            /// <summary>
            /// 护理记录表格模板列值信息数据表
            /// </summary>
            public const string NUR_REC = "NUR_REC_INDEX";

            /// <summary>
            /// 文档索引信息表
            /// </summary>
            public const string NUR_DOC = "NUR_DOC_INDEX";

            /// <summary>
            /// 文档状态表
            /// </summary>
            public const string DOC_STATUS = "NUR_DOC_STATUS";

            /// <summary>
            /// 父子文档关系表
            /// </summary>
            public const string CHILD_DOC = "NUR_CHILD_DOC";

            /// <summary>
            /// 系统配置字典表
            /// </summary>
            public const string SYSTEM_CONFIG = "SYSTEM_CONFIG";

            /// <summary>
            /// 用户权限控制表
            /// </summary>
            public const string USER_RIGHT = "NUR_USER_RIGHT";

            /// <summary>
            /// 用户组信息表
            /// </summary>
            public const string USER_GROUP = "NUR_USER_GROUP";

            /// <summary>
            /// 护理文本模板表
            /// </summary>
            public const string TEXT_TEMPLET = "NUR_TEXT_TEMPLET";

            /// <summary>
            /// 护理word模板表
            /// </summary>
            public const string WORD_TEMP = "NUR_WORD_TEMP";

            /// <summary>
            /// 护理信息库表
            /// </summary>
            public const string INFO_LIB = "NUR_INFO_LIB";

            /// <summary>
            /// 操作审批记录表
            /// </summary>
            public const string OPERATION_APPLY_REC = "OPERATION_APPLY_REC";

            /// <summary>
            /// 体温单显示方案
            /// </summary>
            public const string VITAL_VIEW_SCHEMA = "VITAL_VIEW_SCHEMA";

            /// <summary>
            /// 护理申请记录表
            /// </summary>
            public const string NUR_APPLY_INDEX = "NUR_APPLY_INDEX";

            /// <summary>
            /// 护理申请状态表
            /// </summary>
            public const string NUR_APPLY_STATUS = "NUR_APPLY_STATUS";

            /// <summary>
            /// 护理公共字典表
            /// </summary>
            public const string NUR_COMMON_DICT = "NUR_COMMON_DICT";

            /// <summary>
            /// 护理计划字典表
            /// </summary>
            public const String NUR_CARE_PLAN_DICT = "NUR_CARE_PLAN_DICT";

            /// <summary>
            /// 护理计划记录表
            /// </summary>
            public const string NUR_CARE_PLAN_INDEX = "NUR_CARE_PLAN_INDEX";

            /// <summary>
            /// 护理计划状态表
            /// </summary>
            public const string NUR_CARE_PLAN_STATUS = "NUR_CARE_PLAN_STATUS";

            /// <summary>
            /// 护理交班班次字典表
            /// </summary>
            public const string NUR_SHIFT_RANK_DICT = "NUR_SHIFT_RANK_DICT";

            /// <summary>
            /// 护理交班主记录表
            /// </summary>
            public const string NUR_SHIFT_INDEX = "NUR_SHIFT_INDEX";

            /// <summary>
            /// 护理交班本次交班病人表
            /// </summary>
            public const string NUR_SHIFT_PATIENT = "NUR_SHIFT_PATIENT";

            /// <summary>
            /// 护理交班本次交班病区状况表
            /// </summary>
            public const string NUR_SHIFT_WARD_STATUS = "NUR_SHIFT_WARD_STATUS";

            /// <summary>
            /// 护理交班项目配置别名表
            /// </summary>
            public const string NUR_SHIFT_ITEMALIAS = "NUR_SHIFT_ITEMALIAS";

            /// <summary>
            /// 护理交班特殊病人病情交班状况表
            /// </summary>
            public const string NUR_SHIFT_SPECIALPATIENT = "NUR_SHIFT_SPECIALPATIENT";

            /// <summary>
            /// 护理交班动态配置表
            /// </summary>
            public const string NUR_SHIFT_CONFIG_DICT = "NUR_SHIFT_CONFIG_DICT";

            /// <summary>
            /// 质控审核表
            /// </summary>
            public const string NUR_QC_EXAMINE = "NUR_QC_EXAMINE";

            /// <summary>
            /// 护理记录单打印记录表
            /// </summary>
            public const string NUR_REC_PRINT_LOG = "NUR_REC_PRINT_LOG";

            /// <summary>
            /// 护理评价类型表
            /// </summary>
            public const string NUR_EVALUATION_TYPE = "NUR_EVALUATION_TYPE";

            /// <summary>
            /// 护理评价类型选项表
            /// </summary>
            public const string NUR_EVALUATION_ITEM = "NUR_EVALUATION_ITEM";

            /// <summary>
            /// 科室护理评价类型配置表
            /// </summary>
            public const string WARD_EVA_TYPE = "WARD_EVA_TYPE";

            /// <summary>
            /// 护理评价索引信息表
            /// </summary>
            public const string NUR_EVALUATION = "NUR_EVALUATION_INDEX";

            /// <summary>
            /// 护理评价索引信息内容表
            /// </summary>
            public const string NUR_EVALUATION_DATA = "NUR_EVALUATION_DATA";

            /// <summary>
            /// 食物成分信息内容表
            /// </summary>
            public const string NUR_FOOD_ELEMENT = "NUR_FOOD_ELEMENT";

            /// <summary>
            /// 质量与安全管理记录文档摘要数据
            /// </summary>
            public const string QC_SUMMARY_DATA = "QC_SUMMARY_DATA";

            /// <summary>
            /// 质量与安全管理记录文档索引信息表
            /// </summary>
            public const string QC_DOC = "QC_DOC_INDEX";

            /// <summary>
            /// 质量与安全管理记录文档状态表
            /// </summary>
            public const string QC_DOC_STATUS = "QC_DOC_STATUS";
            /// <summary>
            /// 评估任务表
            /// </summary>
            public const string ASSESSMENT_CYCLE = "ASSESSMENT_CYCLE";
        }

        /// <summary>
        /// 文档类型数据表字段定义
        /// </summary>
        internal struct DocTypeTable
        {
            /// <summary>
            /// 文档类型代码
            /// </summary>
            public const string DOCTYPE_ID = "DOCTYPE_ID";

            /// <summary>
            /// 排序值
            /// </summary>
            public const string DOCTYPE_NO = "DOCTYPE_NO";

            /// <summary>
            /// 文档类型名
            /// </summary>
            public const string DOCTYPE_NAME = "DOCTYPE_NAME";

            /// <summary>
            /// 应用环境
            /// </summary>
            public const string APPLY_ENV = "APPLY_ENV";

            /// <summary>
            /// 文档是否可重复
            /// </summary>
            public const string IS_REPEATED = "IS_REPEATED";

            /// <summary>
            /// 标识当前是否有效
            /// </summary>
            public const string IS_VALID = "IS_VALID";

            /// <summary>
            /// 标识当前是否可见
            /// </summary>
            public const string IS_VISIBLE = "IS_VISIBLE";

            /// <summary>
            /// 标识当前是否是目录
            /// </summary>
            public const string IS_FOLDER = "IS_FOLDER";

            /// <summary>
            /// 标识表单打印模式
            /// (0-不打印;1-打印表单;2-打印列表;3-都打印)
            /// </summary>
            public const string PRINT_MODE = "PRINT_MODE";

            /// <summary>
            /// 文档类型所在目录ID
            /// </summary>
            public const string PARENT_ID = "PARENT_ID";

            /// <summary>
            /// 文档类型的修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 文档模板数据
            /// </summary>
            public const string TEMPLET_DATA = "TEMPLET_DATA";

            /// <summary>
            /// 文档默认排序列名
            /// </summary>
            public const string SORT_COLUMN = "SORTCOLUMN";

            /// <summary>
            /// 文档默认排序方式，0不排序；1升序；2降序
            /// </summary>
            public const string SORT_MODE = "SORTMODE";

            /// <summary>
            /// 文档所属列配置方案ID
            /// </summary>
            public const string SCHEMA = "SCHEMA";

            /// <summary>
            /// 文档是否质控模式下可见
            /// </summary>
            public const string IS_QC_VISIBLE = "IS_QCVISIBLE";
        }

        /// <summary>
        /// 护理质控评估模板表
        /// </summary>
        internal struct EvaluationTypeTable
        {
            public const string EVATYPE_ID = "EVATYPE_ID";
            public const string EVATYPE_NAME = "EVATYPE_NAME";
            public const string CREATE_TIME = "CREATE_TIME";
            public const string EVATYPE_NO = "EVATYPE_NO";
            public const string IS_VALID = "IS_VALID";
            public const string IS_VISIBLE = "IS_VISIBLE";
            public const string IS_FOLDER = "IS_FOLDER";
            public const string PARENT_ID = "PARENT_ID";
            public const string HAVE_REMARK = "HAVE_REMARK";
            public const string STANDARD = "STANDARD";
        }

        /// <summary>
        /// 护理质控评估模板表
        /// </summary>
        internal struct EvaluationDocTable
        {
            public const string EVA_ID = "EVA_ID";
            public const string EVA_TYPE = "EVA_TYPE";
            public const string EVA_NAME = "EVA_NAME";
            public const string EVA_TIME = "EVA_TIME";
            public const string EVA_VERSION = "EVA_VERSION";
            public const string EVA_STATUS = "EVA_STATUS";
            public const string CREATOR_ID = "CREATOR_ID";
            public const string CREATOR_NAME = "CREATOR_NAME";
            public const string MODIFIER_ID = "MODIFIER_ID";
            public const string MODIFIER_NAME = "MODIFIER_NAME";
            public const string MODIFY_TIME = "MODIFY_TIME";
            public const string WARD_CODE = "WARD_CODE";
            public const string WARD_NAME = "WARD_NAME";
            public const string EVA_SCORE = "EVA_SCORE";
            public const string EVA_REMARK = "EVA_REMARK";
        }

        /// <summary>
        /// 护理质控评估内容表
        /// </summary>
        internal struct EvaluationDataTable
        {
            public const string EVA_ID = "EVA_ID";
            public const string ITEM_NO = "ITEM_NO";
            public const string ITEM_VALUE = "ITEM_VALUE";
            public const string ITEM_REMARK = "ITEM_REMARK";
        }

        /// <summary>
        /// 病区护理评价类型数据表字段定义
        /// </summary>
        internal struct WardEvaTypeTable
        {
            /// <summary>
            /// 文档类型代码
            /// </summary>
            public const string EVATYPE_ID = "EVATYPE_ID";

            /// <summary>
            /// 文档类型名称
            /// </summary>
            public const string EVATYPE_NAME = "EVATYPE_NAME";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";
        }

        /// <summary>
        /// 病区质量与安全管理记录类型数据表字段定义
        /// </summary>
        internal struct WardQSMTypeTable
        {
            /// <summary>
            /// 文档类型代码
            /// </summary>
            public const string QSMTYPE_ID = "QSMTYPE_ID";

            /// <summary>
            /// 文档类型名称
            /// </summary>
            public const string QSMTYPE_NAME = "QSMTYPE_NAME";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";
        }

        /// <summary>
        /// 护理质控选择项表
        /// </summary>
        internal struct EvaluationItemTable
        {
            public const string EVATYPE_ID = "EVATYPE_ID";
            public const string ITEM_NO = "ITEM_NO";
            public const string ITEM_TEXT = "ITEM_TEXT";
            public const string ITEM_TEXT_BOLD = "ITEM_TEXT_BOLD";
            public const string ITEM_TYPE = "ITEM_TYPE";
            public const string ITEM_SCORE = "ITEM_SCORE";
            public const string ITEM_DEFAULTVALUE = "ITEM_DEFAULTVALUE";
            public const string ITEM_IN_COUNT = "ITEM_IN_COUNT";
            public const string ITEM_SORT = "ITEM_SORT";
            public const string ITEM_READONLY = "ITEM_READONLY";
            public const string ITEM_ENABLE = "ITEM_ENABLE";
        }

        /// <summary>
        /// 病区病历类型数据表字段定义
        /// </summary>
        internal struct WardDocTypeTable
        {
            /// <summary>
            /// 文档类型代码
            /// </summary>
            public const string DOCTYPE_ID = "DOCTYPE_ID";

            /// <summary>
            /// 文档类型名称
            /// </summary>
            public const string DOCTYPE_NAME = "DOCTYPE_NAME";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";
        }

        /// <summary>
        /// 报表类型数据表字段定义
        /// </summary>
        internal struct ReportTypeTable
        {
            /// <summary>
            /// 报表类型代码
            /// </summary>
            public const string REPORT_TYPE_ID = "REPORT_TYPE_ID";

            /// <summary>
            /// 排序值
            /// </summary>
            public const string REPORT_TYPE_NO = "REPORT_TYPE_NO";

            /// <summary>
            /// 报表类型名
            /// </summary>
            public const string REPORT_TYPE_NAME = "REPORT_TYPE_NAME";

            /// <summary>
            /// 应用环境
            /// </summary>
            public const string APPLY_ENV = "APPLY_ENV";

            /// <summary>
            /// 标识当前是否有效
            /// </summary>
            public const string IS_VALID = "IS_VALID";

            /// <summary>
            /// 标识当前是否是目录
            /// </summary>
            public const string IS_FOLDER = "IS_FOLDER";

            /// <summary>
            /// 报表类型所在目录ID
            /// </summary>
            public const string PARENT_ID = "PARENT_ID";

            /// <summary>
            /// 报表类型的修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 报表模板数据
            /// </summary>
            public const string REPORT_DATA = "REPORT_DATA";
        }

        /// <summary>
        /// 病区报表类型数据表字段定义
        /// </summary>
        internal struct WardReportTypeTable
        {
            /// <summary>
            /// 报表类型代码
            /// </summary>
            public const string REPORT_TYPE_ID = "REPORT_TYPE_ID";

            /// <summary>
            /// 报表类型名称
            /// </summary>
            public const string REPORT_TYPE_NAME = "REPORT_TYPE_NAME";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";
        }

        /// <summary>
        /// 各项评估摘要数据表
        /// </summary>
        internal struct EvaSummaryDataTable
        {
            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 诊次ID
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 所属文档类型ID
            /// </summary>
            public const string DOCTYPE_ID = "DOCTYPE_ID";

            /// <summary>
            /// 数据生成时间
            /// </summary>
            public const string DATA_TIME = "DATA_TIME";

            /// <summary>
            /// 数据名称
            /// </summary>
            public const string DATA_NAME = "DATA_NAME";

            /// <summary>
            /// 数据值
            /// </summary>
            public const string DATA_VALUE = "DATA_VALUE";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";
        }
        
        /// <summary>
        /// 病历摘要数据表
        /// </summary>
        internal struct SummaryDataTable
        {
            /// <summary>
            /// 病历ID
            /// </summary>
            public const string DOC_ID = "DOC_ID";

            /// <summary>
            /// 数据标识
            /// </summary>
            public const string DATA_NAME = "DATA_NAME";

            /// <summary>
            /// 数据代码
            /// </summary>
            public const string DATA_CODE = "DATA_CODE";

            /// <summary>
            /// 数据值
            /// </summary>
            public const string DATA_VALUE = "DATA_VALUE";

            /// <summary>
            /// 数据类型
            /// </summary>
            public const string DATA_TYPE = "DATA_TYPE";

            /// <summary>
            /// 数据单位
            /// </summary>
            public const string DATA_UNIT = "DATA_UNIT";

            /// <summary>
            /// 数据产生时间
            /// </summary>
            public const string DATA_TIME = "DATA_TIME";

            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 就诊ID
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 病人子ID
            /// </summary>
            public const string SUB_ID = "SUB_ID";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 记录ID
            /// </summary>
            public const string RECORD_ID = "RECORD_ID";

            /// <summary>
            /// 所属文档类型ID
            /// </summary>
            public const string DOCTYPE_ID = "DOCTYPE_ID";
        }

        /// <summary>
        /// 质量与安全管理记录摘要数据表
        /// </summary>
        internal struct QCSummaryDataTable
        {
            /// <summary>
            /// 质量与安全管理记录ID
            /// </summary>
            public const string DOC_ID = "DOC_ID";

            /// <summary>
            /// 数据标识
            /// </summary>
            public const string DATA_NAME = "DATA_NAME";

            /// <summary>
            /// 数据代码
            /// </summary>
            public const string DATA_CODE = "DATA_CODE";

            /// <summary>
            /// 数据值
            /// </summary>
            public const string DATA_VALUE = "DATA_VALUE";

            /// <summary>
            /// 数据类型
            /// </summary>
            public const string DATA_TYPE = "DATA_TYPE";

            /// <summary>
            /// 数据单位
            /// </summary>
            public const string DATA_UNIT = "DATA_UNIT";

            /// <summary>
            /// 数据产生时间
            /// </summary>
            public const string DATA_TIME = "DATA_TIME";
            
            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 记录ID
            /// </summary>
            public const string RECORD_ID = "RECORD_ID";

            /// <summary>
            /// 所属文档类型ID
            /// </summary>
            public const string DOCTYPE_ID = "DOCTYPE_ID";
        }

        /// <summary>
        /// 文档索引信息数据表字段定义
        /// </summary>
        internal struct NurDocTable
        {
            /// <summary>
            /// 文档的唯一标识
            /// </summary>
            public const string DOC_ID = "DOC_ID";

            /// <summary>
            /// 文档的类型
            /// </summary>
            public const string DOC_TYPE_ID = "DOC_TYPE";

            /// <summary>
            /// 文档的显示标题
            /// </summary>
            public const string DOC_TITLE = "DOC_TITLE";

            /// <summary>
            /// 文档生成的时间
            /// </summary>
            public const string DOC_TIME = "DOC_TIME";

            /// <summary>
            /// 文档集编号
            /// </summary>
            public const string DOC_SETID = "DOC_SETID";

            /// <summary>
            /// 文档版本
            /// </summary>
            public const string DOC_VERSION = "DOC_VERSION";

            /// <summary>
            /// 文档法定作者的标号
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 文档法定作者姓名
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 文档修改者的标号
            /// </summary>
            public const string MODIFIER_ID = "MODIFIER_ID";

            /// <summary>
            /// 文档修改者的标号
            /// </summary>
            public const string MODIFIER_NAME = "MODIFIER_NAME";

            /// <summary>
            /// 文档修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 文档所属的病人号
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 文档所属病人姓名
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 就诊号
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 就诊时间
            /// </summary>
            public const string VISIT_TIME = "VISIT_TIME";

            /// <summary>
            /// 就诊类型
            /// </summary>
            public const string VISIT_TYPE = "VISIT_TYPE";

            /// <summary>
            /// 病人子ID
            /// </summary>
            public const string SUB_ID = "SUB_ID";

            /// <summary>
            /// 病人病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病人病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 签名代码
            /// </summary>
            public const string SIGN_CODE = "SIGN_CODE";

            /// <summary>
            /// 隐私等级代码
            /// </summary>
            public const string CONFID_CODE = "CONFID_CODE";

            /// <summary>
            /// 文档次序编号
            /// </summary>
            public const string ORDER_VALUE = "ORDER_VALUE";

            /// <summary>
            /// 病历护士记录的时间
            /// </summary>
            public const string RECORD_TIME = "RECORD_TIME";

            /// <summary>
            /// 病历关联的护理记录ID
            /// </summary>
            public const string RECORD_ID = "RECORD_ID";

            /// <summary>
            /// 文档数据
            /// </summary>
            public const string DOC_DATA = "DOC_DATA";
        }

        /// <summary>
        /// 护理记录单打印日志表字段定义
        /// </summary>
        internal struct RecPrintLog
        {
            /// <summary>
            /// 打印操作用户ID
            /// </summary>
            public const string USER_ID = "USER_ID";

            /// <summary>
            /// 打印操作用户姓名？？
            /// </summary>
            public const string USER_NAME = "USER_NAME";

            /// <summary>
            /// 用户所在病区
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 就诊号
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 护理记录列配置号
            /// </summary>
            public const string SCHEMA_ID = "SCHEMA_ID";

            /// <summary>
            /// 打印页数
            /// </summary>
            public const string PRINT_PAGES = "PRINT_PAGES";

            /// <summary>
            /// 打印日期
            /// </summary>
            public const string PRINT_DATE = "PRINT_DATE";
        }

        /// <summary>
        /// 文档状态数据表字段定义
        /// </summary>
        internal struct DocStatusTable
        {
            /// <summary>
            /// 文档唯一标识
            /// </summary>
            public const string DOC_ID = "DOC_ID";

            /// <summary>
            /// 文档的状态(编辑中，已作废，可编辑，已归档)
            /// </summary>
            public const string DOC_STATUS = "DOC_STATUS";

            /// <summary>
            /// 操作者ID
            /// </summary>
            public const string OPERATOR_ID = "OPERATOR_ID";

            /// <summary>
            /// 操作者姓名
            /// </summary>
            public const string OPERATOR_NAME = "OPERATOR_NAME";

            /// <summary>
            /// 操作时间
            /// </summary>
            public const string OPERATE_TIME = "OPERATE_TIME";

            /// <summary>
            /// 状态描述
            /// </summary>
            public const string STATUS_DESC = "STATUS_DESC";
        }

        /// <summary>
        /// 评估任务表
        /// </summary>
        internal struct AssessmentCycleTable
        {
            /// <summary>
            /// 患者ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";
            /// <summary>
            /// 患者就诊次
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";
            /// <summary>
            /// 评估文档ID
            /// </summary>
            public const string DOC_ID = "DOC_ID";
            /// <summary>
            /// 模板ID
            /// </summary>
            public const string DOCTYPE_ID = "DOCTYPE_ID";
            /// <summary>
            /// 评估时间
            /// </summary>
            public const string RECORD_TIME = "RECORD_TIME";
            /// <summary>
            /// 评估频次
            /// </summary>
            public const string CYCLE = "CYCLE";
            /// <summary>
            /// 单位
            /// </summary>
            public const string UNIT = "UNIT";
            /// <summary>
            /// 分值
            /// </summary>
            public const string DATA = "DATA";
            /// <summary>
            /// 评估科室名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";
            /// <summary>
            /// 评估科室代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";
            /// <summary>
            /// 评估人
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";
            /// <summary>
            /// 评估人ID
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";
            /// <summary>
            /// 模板名称
            /// </summary>
            public const string DOCTYPE_NAME = "DOCTYPE_NAME";
            /// <summary>
            /// 主键
            /// </summary>
            public const string ID = "ID";
        }

        /// <summary>
        /// 子文档信息表
        /// </summary>
        internal struct ChildDocTable
        {
            /// <summary>
            /// 文档唯一标识
            /// </summary>
            public const string DOC_ID = "DOC_ID";

            /// <summary>
            /// 调用者标识
            /// </summary>
            public const string CALLER = "CALLER";

            /// <summary>
            /// 子文档唯一标识
            /// </summary>
            public const string CHILD_ID = "CHILD_ID";
        }

        /// <summary>
        /// 系统配置字典表各字段定义
        /// </summary>
        internal struct ConfigDictTable
        {
            /// <summary>
            /// 配置组名称
            /// </summary>
            public const string GROUP_NAME = "GROUP_NAME";

            /// <summary>
            /// 配置项名称
            /// </summary>
            public const string CONFIG_NAME = "CONFIG_NAME";

            /// <summary>
            /// 配置项值
            /// </summary>
            public const string CONFIG_VALUE = "CONFIG_VALUE";

            /// <summary>
            /// 配置项描述
            /// </summary>
            public const string CONFIG_DESC = "CONFIG_DESC";
        }

        /// <summary>
        /// 用户权限控制表
        /// </summary>
        internal struct UserRightTable
        {
            /// <summary>
            /// 用户代码
            /// </summary>
            public const string USER_ID = "USER_ID";

            /// <summary>
            /// 用户密码
            /// </summary>
            public const string USER_PWD = "USER_PWD";

            /// <summary>
            /// 权限二进制位编码字符串
            /// </summary>
            public const string RIGHT_CODE = "RIGHT_CODE";

            /// <summary>
            /// 权限类型
            /// </summary>
            public const string RIGHT_TYPE = "RIGHT_TYPE";

            /// <summary>
            /// 权限描述
            /// </summary>
            public const string RIGHT_DESC = "RIGHT_DESC";
        }

        /// <summary>
        /// 用户组信息表
        /// </summary>
        internal struct UserGroupTable
        {
            /// <summary>
            /// 用户代码
            /// </summary>
            public const string USER_ID = "USER_ID";

            /// <summary>
            /// 组代码
            /// </summary>
            public const string GROUP_CODE = "GROUP_CODE";

            /// <summary>
            /// 用户组优先级
            /// </summary>
            public const string PRIORITY = "PRIORITY";
        }

        /// <summary>
        /// 护理公共字典表
        /// </summary>
        internal struct CommonDictTable
        {
            /// <summary>
            /// 类型
            /// </summary>
            public const string ITEM_TYPE = "ITEM_TYPE";

            /// <summary>
            /// 序号
            /// </summary>
            public const string ITEM_NO = "ITEM_NO";

            /// <summary>
            /// 字典项目代码
            /// </summary>
            public const string ITEM_CODE = "ITEM_CODE";

            /// <summary>
            /// 字典项目名称
            /// </summary>
            public const string ITEM_NAME = "ITEM_NAME";

            /// <summary>
            /// 病区代码字段
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称字段
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 输入码
            /// </summary>
            public const string INPUT_CODE = "INPUT_CODE";
        }

        /// <summary>
        /// 病案质量问题分类字典表相关字段定义
        /// </summary>
        internal struct QCMessageTypeView
        {
            /// <summary>
            /// 序号
            /// </summary>
            public const string SERIAL_NO = "SERIAL_NO";
            /// <summary>
            /// 问题分类
            /// </summary>
            public const string QA_EVENT_TYPE = "QA_EVENT_TYPE";
            /// <summary>
            /// 输入码
            /// </summary>
            public const string INPUT_CODE = "INPUT_CODE";
            /// <summary>
            /// 上级输入码
            /// </summary>
            public const string PARENT_CODE = "PARENT_CODE";
            /// <summary>
            /// 最大扣分数
            /// </summary>
            public const string MAX_SCORE = "MAX_SCORE";
            /// <summary>
            /// 应用环境
            /// </summary>
            public const string APPLY_ENV = "APPLY_ENV";
        }

        /// <summary>
        /// 质控反馈信息字典表相关字段定义
        /// </summary>
        internal struct QCMessageTempletView
        {
            /// <summary>
            /// 序号
            /// </summary>
            public const string SERIAL_NO = "SERIAL_NO";
            /// <summary>
            /// 反馈信息代码
            /// </summary>
            public const string QC_MSG_CODE = "QC_MSG_CODE";
            /// <summary>
            /// 问题分类
            /// </summary>
            public const string QA_EVENT_TYPE = "QA_EVENT_TYPE";
            /// <summary>
            /// 信息描述
            /// </summary>
            public const string MESSAGE = "MESSAGE";
            /// <summary>
            /// 信息标题
            /// </summary>
            public const string MESSAGE_TITLE = "MESSAGE_TITLE";
            /// <summary>
            /// 扣分
            /// </summary>
            public const string SCORE = "SCORE";
            /// <summary>
            /// 输入码
            /// </summary>
            public const string INPUT_CODE = "INPUT_CODE";
            /// <summary>
            /// 应用环境
            /// </summary>
            public const string APPLY_ENV = "APPLY_ENV";
        }

        /// <summary>
        /// 表格模板定义信息表
        /// </summary>
        internal struct GridViewSchemaTable
        {
            /// <summary>
            /// 表格ID字段
            /// </summary>
            public const string SCHEMA_ID = "SCHEMA_ID";

            /// <summary>
            /// 表格方案类型
            /// </summary>
            public const string SCHEMA_TYPE = "SCHEMA_TYPE";

            /// <summary>
            /// 表格名称字段
            /// </summary>
            public const string SCHEMA_NAME = "SCHEMA_NAME";

            /// <summary>
            /// 病区代码字段
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称字段
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 是否是缺省方案字段
            /// </summary>
            public const string IS_DEFAULT = "IS_DEFAULT";

            /// <summary>
            /// 创建者ID号字段
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 创建者姓名字段
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 创建时间字段
            /// </summary>
            public const string CREATE_TIME = "CREATE_TIME";

            /// <summary>
            /// 修修改者ID号字段
            /// </summary>
            public const string MODIFIER_ID = "MODIFIER_ID";

            /// <summary>
            /// 修改者姓名字段
            /// </summary>
            public const string MODIFIER_NAME = "MODIFIER_NAME";

            /// <summary>
            /// 修改时间字段
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 当前方案自定义标记
            /// </summary>
            public const string SCHEMA_FLAG = "SCHEMA_FLAG";

            /// <summary>
            /// 批量入录关联护理记录列配置
            /// </summary>
            public const string RELATIVE_SCHEMA_ID = "RELATIVE_SCHEMA_ID";
        }

        /// <summary>
        /// 表格模板列定义信息表
        /// </summary>
        internal struct GridViewColumnTable
        {
            /// <summary>
            /// 表格列所属方案ID字段
            /// </summary>
            public const string SCHEMA_ID = "SCHEMA_ID";

            /// <summary>
            /// 表格列ID字段
            /// </summary>
            public const string COLUMN_ID = "COLUMN_ID";

            /// <summary>
            /// 表格列名称字段
            /// </summary>
            public const string COLUMN_NAME = "COLUMN_NAME";

            /// <summary>
            /// 表格列名称多表头字段
            /// </summary>
            public const string COLUMN_GROUP = "COLUMN_GROUP";

            /// <summary>
            /// 列数据绑定名称标识字段
            /// </summary>
            public const string COLUMN_TAG = "COLUMN_TAG";

            /// <summary>
            /// 列显示索引
            /// </summary>
            public const string COLUMN_INDEX = "COLUMN_INDEX";

            /// <summary>
            /// 列类型字段
            /// </summary>
            public const string COLUMN_TYPE = "COLUMN_TYPE";

            /// <summary>
            /// 列宽字段
            /// </summary>
            public const string COLUMN_WIDTH = "COLUMN_WIDTH";

            /// <summary>
            /// 列显示数据的单位字段
            /// </summary>
            public const string COLUMN_UNIT = "COLUMN_UNIT";

            /// <summary>
            /// 列是否显示字段
            /// </summary>
            public const string IS_VISIBLE = "IS_VISIBLE";

            /// <summary>
            /// 列是否打印字段
            /// </summary>
            public const string IS_PRINT = "IS_PRINT";

            /// <summary>
            /// 是否居中显示和打印字段
            /// </summary>
            public const string IS_MIDDLE = "IS_MIDDLE";

            /// <summary>
            /// 列的可选项列表字段
            /// </summary>
            public const string COLUMN_ITEMS = "COLUMN_ITEMS";

            /// <summary>
            /// 列的文档类型字段
            /// </summary>
            public const string DOCTYPE_ID = "DOCTYPE_ID";
        }

        /// <summary>
        /// 体温单显示方案
        /// </summary>
        internal struct VitalViewSchemaTable
        {
            /// <summary>
            /// 方案id号
            /// </summary>
            public const string SCHEMA_ID = "VITAL_SCHEMA_ID";

            /// <summary>
            /// 文档id号
            /// </summary>
            public const string DOCTYPEID = "DOC_TYPE_ID";

            /// <summary>
            /// 文档名称
            /// </summary>
            public const string DOCTYPENAME = "DOC_TYPE_NAME";

            /// <summary>
            /// 病区代码字段
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称字段
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 创建者ID号字段
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 创建者姓名字段
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 创建时间字段
            /// </summary>
            public const string CREATE_TIME = "CREATE_TIME";

            /// <summary>
            /// 修修改者ID号字段
            /// </summary>
            public const string MODIFIER_ID = "MODIFIER_ID";

            /// <summary>
            /// 修改者姓名字段
            /// </summary>
            public const string MODIFIER_NAME = "MODIFIER_NAME";

            /// <summary>
            /// 修改时间字段
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 当前方案自定义标记
            /// </summary>
            public const string VITAL_SQL = "VITAL_SQL";
        }

        /// <summary>
        /// 护理记录表格数据信息表
        /// </summary>
        internal struct NursingRecInfoTable
        {
            /// <summary>
            /// 病人ID字段
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 入院标识
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 病人子ID
            /// </summary>
            public const string SUB_ID = "SUB_ID";

            /// <summary>
            /// 记录的ID号
            /// </summary>
            public const string RECORD_ID = "RECORD_ID";

            /// <summary>
            /// 录入日期字段
            /// </summary>
            public const string RECORD_DATE = "RECORD_DATE";

            /// <summary>
            /// 录入时间字段
            /// </summary>
            public const string RECORD_TIME = "RECORD_TIME";

            /// <summary>
            /// 病人姓名
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 病人所在病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病人所在病区名
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 记录创建者ID
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 记录创建者姓名
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 记录创建时间
            /// </summary>
            public const string CREATE_TIME = "CREATE_TIME";

            /// <summary>
            /// 记录修改者ID
            /// </summary>
            public const string MODIFIER_ID = "MODIFIER_ID";

            /// <summary>
            /// 记录修改者姓名
            /// </summary>
            public const string MODIFIER_NAME = "MODIFIER_NAME";

            /// <summary>
            /// 记录修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 实际记录者1ID号
            /// </summary>
            public const string RECORDER1_ID = "RECORDER1_ID";

            /// <summary>
            /// 实际记录者1姓名
            /// </summary>
            public const string RECORDER1_NAME = "RECORDER1_NAME";

            /// <summary>
            /// 实际记录者2ID号
            /// </summary>
            public const string RECORDER2_ID = "RECORDER2_ID";

            /// <summary>
            /// 实际记录者2姓名
            /// </summary>
            public const string RECORDER2_NAME = "RECORDER2_NAME";

            /// <summary>
            /// 小结标记
            /// </summary>
            public const string SUMMARY_FLAG = "SUMMARY_FLAG";

            /// <summary>
            /// 实际记录者2姓名
            /// </summary>
            public const string SUMMARY_NAME = "SUMMARY_NAME";

            /// <summary>
            /// 实际记录者2姓名
            /// </summary>
            public const string SUMMARY_START_TIME = "SUMMARY_START_TIME";

            /// <summary>
            /// 护理记录自动文本
            /// </summary>
            public const string RECORD_CONTENT = "RECORD_CONTENT";

            /// <summary>
            /// 护理记录自定义文本
            /// </summary>
            public const string RECORD_REMARKS = "RECORD_REMARKS";

            /// <summary>
            /// 护理记录数据打印状态
            /// </summary>
            public const string RECORD_PRINTED = "RECORD_PRINTED";

            /// <summary>
            /// 护理记录数据状态
            /// </summary>
            public const string RECORD_STATUS = "RECORD_STATUS";

            /// <summary>
            /// 护理记录列显示配置ID号
            /// </summary>
            public const string SCHEMA_ID = "SCHEMA_ID";
        }

        /// <summary>
        /// 护理文本模板数据表字段定义
        /// </summary>
        internal struct TextTempletTable
        {
            /// <summary>
            /// 模板ID
            /// </summary>
            public const string TEMPLET_ID = "TEMPLET_ID";

            /// <summary>
            /// 模板的描述名，用户自定义录入内容。
            /// </summary>
            public const string TEMPLET_NAME = "TEMPLET_NAME";

            /// <summary>
            /// 共享水平：个人P，科室D,全院H
            /// </summary>
            public const string SHARE_LEVEL = "SHARE_LEVEL";

            /// <summary>
            /// 用户病区代码字段
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 用户病区名称字段
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 创建者ID
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 创建者名称
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 创建时间
            /// </summary>
            public const string CREATE_TIME = "CREATE_TIME";

            /// <summary>
            /// 修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 标识当前模板或者文件夹的父节点ID
            /// </summary>
            public const string PARENT_ID = "PARENT_ID";

            /// <summary>
            /// 标识当前记录是否为文件夹:1文件夹
            /// </summary>
            public const string IS_FOLDER = "IS_FOLDER";

            /// <summary>
            /// 模板数据
            /// </summary>
            public const string TEMPLET_DATA = "TEMPLET_DATA";
        }

        /// <summary>
        /// 护理WORD模板数据表字段定义
        /// </summary>
        internal struct WordTempTable
        {
            /// <summary>
            /// 模板ID
            /// </summary>
            public const string WORDTEMP_ID = "WORDTEMP_ID";

            /// <summary>
            /// 模板的描述名，用户自定义录入内容。
            /// </summary>
            public const string WORDTEMP_NAME = "WORDTEMP_NAME";

            /// <summary>
            /// 共享水平：个人P，科室D,全院H
            /// </summary>
            public const string SHARE_LEVEL = "SHARE_LEVEL";

            /// <summary>
            /// 用户病区代码字段
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 用户病区名称字段
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 创建者ID
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 创建者名称
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 创建时间
            /// </summary>
            public const string CREATE_TIME = "CREATE_TIME";

            /// <summary>
            /// 修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 标识当前模板或者文件夹的父节点ID
            /// </summary>
            public const string PARENT_ID = "PARENT_ID";

            /// <summary>
            /// 标识当前记录是否为文件夹:1文件夹
            /// </summary>
            public const string IS_FOLDER = "IS_FOLDER";

            /// <summary>
            /// 模板数据
            /// </summary>
            public const string WORDTEMP_DATA = "WORDTEMP_DATA";
        }

        /// <summary>
        /// 护理信息库表字段定义
        /// </summary>
        internal struct InfoLibTable
        {
            /// <summary>
            /// 信息ID
            /// </summary>
            public const string INFO_ID = "INFO_ID";

            /// <summary>
            /// 信息的描述名，用户自定义录入内容。
            /// </summary>
            public const string INFO_NAME = "INFO_NAME";

            /// <summary>
            /// 共享水平：个人P，科室D,全院H
            /// </summary>
            public const string SHARE_LEVEL = "SHARE_LEVEL";

            /// <summary>
            /// 用户病区代码字段
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 用户病区名称字段
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 创建者ID
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 创建者名称
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 创建时间
            /// </summary>
            public const string CREATE_TIME = "CREATE_TIME";

            /// <summary>
            /// 修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 标识当前模板或者文件夹的父节点ID
            /// </summary>
            public const string PARENT_ID = "PARENT_ID";

            /// <summary>
            /// 标识当前记录是否为文件夹:1文件夹
            /// </summary>
            public const string IS_FOLDER = "IS_FOLDER";

            /// <summary>
            /// 模板数据
            /// </summary>
            public const string INFO_DATA = "INFO_DATA";

            /// <summary>
            /// 状态
            /// </summary>
            public const string STATUS = "STATUS";

            /// <summary>
            /// 护理信息库文件类型
            /// </summary>
            public const string INFO_TYPE = "INFO_TYPE";

            /// <summary>
            /// 护理信息库文件大小
            /// </summary>
            public const string INFO_SIZE = "INFO_SIZE";
        }

        /// <summary>
        /// 操作审批记录数据表字段定义
        /// </summary>
        internal struct OperationApplyTable
        {
            /// <summary>
            /// 申请人ID
            /// </summary>
            public const string REQUESTER_ID = "REQUESTER_ID";

            /// <summary>
            /// 申请人姓名
            /// </summary>
            public const string REQUESTER_NAME = "REQUESTER_NAME";

            /// <summary>
            /// 申请时间
            /// </summary>
            public const string REQUEST_TIME = "REQUEST_TIME";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 病人姓名
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 入院次
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 模块代码
            /// </summary>
            public const string MODULE_CODE = "MODULE_CODE";

            /// <summary>
            /// 模块名称
            /// </summary>
            public const string MODULE_NAME = "MODULE_NAME";

            /// <summary>
            /// 关联数据
            /// </summary>
            public const string ASSIGN_DATA = "ASSIGN_DATA";

            /// <summary>
            /// 操作类型
            /// </summary>
            public const string OPERATION_TYPE = "OPERATION_TYPE";

            /// <summary>
            /// 申请描述
            /// </summary>
            public const string REQUEST_DESC = "REQUEST_DESC";

            /// <summary>
            /// 可审批级别
            /// </summary>
            public const string APPROVER_LEVEL = "APPROVER_LEVEL";

            /// <summary>
            /// 审批人ID
            /// </summary>
            public const string APPROVER_ID = "APPROVER_ID";

            /// <summary>
            /// 审批人姓名
            /// </summary>
            public const string APPROVER_NAME = "APPROVER_NAME";

            /// <summary>
            /// 审批时间
            /// </summary>
            public const string APPROVE_TIME = "APPROVE_TIME";

            /// <summary>
            /// 审批状态
            /// </summary>
            public const string APPROVE_STATE = "APPROVE_STATE";

            /// <summary>
            /// 审批说明
            /// </summary>
            public const string APPROVE_DESC = "APPROVE_DESC";
        }

        /// <summary>
        /// 护理申请数据表字段定义
        /// </summary>
        internal struct NurApplyTable
        {
            /// <summary>
            /// 申请单ID
            /// </summary>
            public const string APPLY_ID = "APPLY_ID";

            /// <summary>
            /// 文档ID
            /// </summary>
            public const string DOC_ID = "DOC_ID";

            /// <summary>
            /// 文档类型ID
            /// </summary>
            public const string DOCTYPE_ID = "DOCTYPE_ID";

            /// <summary>
            /// 申请单类型
            /// </summary>
            public const string APPLY_TYPE = "APPLY_TYPE";

            /// <summary>
            /// 申请单名称
            /// </summary>
            public const string APPLY_NAME = "APPLY_NAME";

            /// <summary>
            /// 紧急程度
            /// </summary>
            public const string URGENCY = "URGENCY";

            /// <summary>
            /// 状态值
            /// </summary>
            public const string STATUS = "STATUS";

            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 病人名称
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 病人住院标记
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 病人子ID
            /// </summary>
            public const string SUB_ID = "SUB_ID";

            /// <summary>
            /// 病人所在护理单元编号
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病人所在护理单元名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 申请者ID
            /// </summary>
            public const string APPLICANT_ID = "APPLICANT_ID";

            /// <summary>
            /// 申请者姓名
            /// </summary>
            public const string APPLICANT_NAME = "APPLICANT_NAME";

            /// <summary>
            /// 申请时间
            /// </summary>
            public const string APPLY_TIME = "APPLY_TIME";

            /// <summary>
            /// 修改者ID
            /// </summary>
            public const string MODIFIER_ID = "MODIFIER_ID";

            /// <summary>
            /// 修改者姓名
            /// </summary>
            public const string MODIFIER_NAME = "MODIFIER_NAME";

            /// <summary>
            /// 修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";
        }

        /// <summary>
        /// 护理申请状态数据表字段定义
        /// </summary>
        internal struct NurApplyStatusTable
        {
            /// <summary>
            /// 申请单ID
            /// </summary>
            public const string APPLY_ID = "APPLY_ID";

            /// <summary>
            /// 状态值
            /// </summary>
            public const string STATUS = "STATUS";

            /// <summary>
            /// 操作者ID
            /// </summary>
            public const string OPERATOR_ID = "OPERATOR_ID";

            /// <summary>
            /// 操作者姓名
            /// </summary>
            public const string OPERATOR_NAME = "OPERATOR_NAME";

            /// <summary>
            /// 操作时间
            /// </summary>
            public const string OPERATE_TIME = "OPERATE_TIME";

            /// <summary>
            /// 状态描述
            /// </summary>
            public const string STATUS_DESC = "STATUS_DESC";

            /// <summary>
            /// 下一个操作者ID
            /// </summary>
            public const string NEXT_OPERATOR_ID = "NEXT_OPERATOR_ID";

            /// <summary>
            /// 下一个操作者姓名
            /// </summary>
            public const string NEXT_OPERATOR_NAME = "NEXT_OPERATOR_NAME";

            /// <summary>
            /// 下一个操作护理单元编码
            /// </summary>
            public const string NEXT_OPERATOR_WARD_CODE = "NEXT_OPERATOR_WARD_CODE";

            /// <summary>
            /// 下一个操作护理单元名称
            /// </summary>
            public const string NEXT_OPERATOR_WARD_NAME = "NEXT_OPERATOR_WARD_NAME";

            /// <summary>
            /// 状态消息
            /// </summary>
            public const string STATUS_MESSAGE = "STATUS_MESSAGE";
        }

        /// <summary>
        /// 护理计划诊断表
        /// </summary>
        internal struct NurCarePlanDictTable
        {
            /// <summary>
            /// 护理诊断代码
            /// </summary>
            public const string DIAGNOSIS_CODE = "DIAGNOSIS_CODE";

            /// <summary>
            /// 关联项目代码
            /// </summary>
            public const string ITEM_CODE = "ITEM_CODE";

            /// <summary>
            /// 关联项目名称
            /// </summary>
            public const string ITEM_TYPE = "ITEM_TYPE";
        }

        /// <summary>
        /// 护理计划数据表字段定义
        /// </summary>
        internal struct NurCarePlanTable
        {
            /// <summary>
            /// 护理计划ID
            /// </summary>
            public const string NCP_ID = "NCP_ID";

            /// <summary>
            /// 文档ID
            /// </summary>
            public const string DOC_ID = "DOC_ID";

            /// <summary>
            /// 文档类型ID
            /// </summary>
            public const string DOCTYPE_ID = "DOCTYPE_ID";

            /// <summary>
            /// 诊断编码
            /// </summary>
            public const string DIAGNOSIS_CODE = "DIAGNOSIS_CODE";

            /// <summary>
            /// 诊断名称
            /// </summary>
            public const string DIAGNOSIS_NAME = "DIAGNOSIS_NAME";

            /// <summary>
            /// 状态值
            /// </summary>
            public const string STATUS = "STATUS";

            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 病人名称
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 病人住院标记
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 病人子ID
            /// </summary>
            public const string SUB_ID = "SUB_ID";

            /// <summary>
            /// 病人所在护理单元编号
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病人所在护理单元名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 创建者ID
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 创建者姓名
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 创建时间
            /// </summary>
            public const string CREATE_TIME = "CREATE_TIME";

            /// <summary>
            /// 修改者ID
            /// </summary>
            public const string MODIFIER_ID = "MODIFIER_ID";

            /// <summary>
            /// 修改者姓名
            /// </summary>
            public const string MODIFIER_NAME = "MODIFIER_NAME";

            /// <summary>
            /// 修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 护理计划开始时间
            /// </summary>
            public const string START_TIME = "START_TIME";

            /// <summary>
            /// 护理计划结束时间
            /// </summary>
            public const string END_TIME = "END_TIME";
        }

        /// <summary>
        /// 护理计划状态数据表字段定义
        /// </summary>
        internal struct NurCarePlanStatusTable
        {
            /// <summary>
            /// 护理计划ID
            /// </summary>
            public const string NCP_ID = "NCP_ID";

            /// <summary>
            /// 状态值
            /// </summary>
            public const string STATUS = "STATUS";

            /// <summary>
            /// 操作者ID
            /// </summary>
            public const string OPERATOR_ID = "OPERATOR_ID";

            /// <summary>
            /// 操作者姓名
            /// </summary>
            public const string OPERATOR_NAME = "OPERATOR_NAME";

            /// <summary>
            /// 操作时间
            /// </summary>
            public const string OPERATE_TIME = "OPERATE_TIME";

            /// <summary>
            /// 状态描述
            /// </summary>
            public const string STATUS_DESC = "STATUS_DESC";
        }


        /// <summary>
        /// 护理交接班动态配置
        /// </summary>
        internal struct ShiftConfigDictTable
        {
            /// <summary>
            /// 交班动态代码
            /// </summary>
            public const string ITEM_CODE = "ITEM_CODE";

            /// <summary>
            /// 交班动态序号
            /// </summary>
            public const string ITEM_NO = "ITEM_NO";

            /// <summary>
            /// 交班动态名称
            /// </summary>
            public const string ITEM_NAME = "ITEM_NAME";

            /// <summary>
            /// 交班动态显示宽度
            /// </summary>
            public const string ITEM_WIDTH = "ITEM_WIDTH";

            /// <summary>
            /// 交班动态是否显示
            /// </summary>
            public const string IS_VISIBLE = "IS_VISIBLE";

            /// <summary>
            /// 交班动态是否居中显示
            /// </summary>
            public const string IS_MIDDLE = "IS_MIDDLE";

            /// <summary>
            /// 交班动态是否重要项目
            /// </summary>
            public const string IS_IMPORTANT = "IS_IMPORTANT";

            /// <summary>
            /// 交班动态病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 交班动态病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";
        }

        /// <summary>
        /// 护理交班班次字典表
        /// </summary>
        internal struct ShiftRankDictTable
        {
            /// <summary>
            /// 交班记录ID
            /// </summary>
            public const string RANK_CODE = "RANK_CODE";

            /// <summary>
            /// 交班班次序号
            /// </summary>
            public const string RANK_NO = "RANK_NO";

            /// <summary>
            /// 交班班次名称
            /// </summary>
            public const string RANK_NAME = "RANK_NAME";

            /// <summary>
            /// 交班班次开始时间点
            /// </summary>
            public const string START_POINT = "START_POINT";

            /// <summary>
            /// 交班班次截止时间点
            /// </summary>
            public const string END_POINT = "END_POINT";

            /// <summary>
            /// 交班班次所属病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 交班班次所属病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";
        }

        /// <summary>
        /// 护理交班主记录表
        /// </summary>
        internal struct ShiftRecordTable
        {
            /// <summary>
            /// 交班记录主表ID
            /// </summary>
            public const string SHIFT_RECORD_ID = "SHIFT_RECORD_ID";

            /// <summary>
            /// 交班日期
            /// </summary>
            public const string SHIFT_RECORD_DATE = "SHIFT_RECORD_DATE";

            /// <summary>
            /// 交班时间
            /// </summary>
            public const string SHIFT_RECORD_TIME = "SHIFT_RECORD_TIME";

            /// <summary>
            /// 交班班次代码
            /// </summary>
            public const string SHIFT_RANK_CODE = "SHIFT_RANK_CODE";

            /// <summary>
            /// 病人病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病人病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 交班记录创建人ID
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 交班记录创建人姓名
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 交班记录创建时间
            /// </summary>
            public const string CREATE_TIME = "CREATE_TIME";

            /// <summary>
            /// 交班记录修改人ID
            /// </summary>
            public const string MODIFIER_ID = "MODIFIER_ID";

            /// <summary>
            /// 交班记录修改人姓名
            /// </summary>
            public const string MODIFIER_NAME = "MODIFIER_NAME";

            /// <summary>
            /// 交班记录修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 第一签名人签名人ID
            /// </summary>
            public const string FIRST_SIGN_ID = "FIRST_SIGN_ID";

            /// <summary>
            /// 第一签名人签名人姓名
            /// </summary>
            public const string FIRST_SIGN_NAME = "FIRST_SIGN_NAME";

            /// <summary>
            /// 第一签名人签名时间
            /// </summary>
            public const string FIRST_SIGN_TIME = "FIRST_SIGN_TIME";

            /// <summary>
            /// 第二签名人ID
            /// </summary>
            public const string SECOND_SIGN_ID = "SECOND_SIGN_ID";

            /// <summary>
            /// 第二签名人姓名
            /// </summary>
            public const string SECOND_SIGN_NAME = "SECOND_SIGN_NAME";

            /// <summary>
            /// 第二签名人签名时间
            /// </summary>
            public const string SECOND_SIGN_TIME = "SECOND_SIGN_TIME";
        }

        /// <summary>
        /// 护理交班本次交班病人表
        /// </summary>
        internal struct ShiftPatientTable
        {
            /// <summary>
            /// 交班记录ID
            /// </summary>
            public const string SHIFT_RECORD_ID = "SHIFT_RECORD_ID";

            /// <summary>
            /// 交班项目名称
            /// </summary>
            public const string SHIFT_ITEM_NAME = "SHIFT_ITEM_NAME";

            /// <summary>
            /// 交班项目别名
            /// </summary>
            public const string SHIFT_ITEM_ALIAS = "SHIFT_ITEM_ALIAS";

            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 病人序号
            /// </summary>
            public const string PATIENT_NO = "PATIENT_NO";

            /// <summary>
            /// 就诊ID
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 病人子ID
            /// </summary>
            public const string SUB_ID = "SUB_ID";

            /// <summary>
            /// 病人姓名
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 病人年龄
            /// </summary>
            public const string PATIENT_AGE = "PATIENT_AGE";

            /// <summary>
            /// 交班时病人所在病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 交班时病人所在病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 交班时病人床号
            /// </summary>
            public const string BED_CODE = "BED_CODE";

            /// <summary>
            /// 交班时病人诊断
            /// </summary>
            public const string DIAGNOSIS = "DIAGNOSIS";

            /// <summary>
            /// 是否显示体征信息
            /// </summary>
            public const string SHOW_VALUE = "SHOW_VALUE";

            /// <summary>
            /// 交班时病人体温值
            /// </summary>
            public const string TEMPERATURE_VALUE = "TEMPERATURE_VALUE";

            /// <summary>
            /// 体温类型
            /// </summary>
            public const string TEMPERATURE_TYPE = "TEMPERATURE_TYPE";

            /// <summary>
            /// 交班时病人脉搏值
            /// </summary>
            public const string PULSE_VALUE = "PULSE_VALUE";

            /// <summary>
            /// 交班时病人呼吸值
            /// </summary>
            public const string BREATH_VALUE = "BREATH_VALUE";

            /// <summary>
            /// 体征采集时间
            /// </summary>
            public const string VITAL_TIME = "VITAL_TIME";

            /// <summary>
            /// 交班时病人特殊项1
            /// </summary>
            public const string SPECIAL_ITEM1 = "SPECIAL_ITEM1";

            /// <summary>
            /// 交班时病人特殊项2
            /// </summary>
            public const string SPECIAL_ITEM2 = "SPECIAL_ITEM2";

            /// <summary>
            /// 交班时病人特殊项3
            /// </summary>
            public const string SPECIAL_ITEM3 = "SPECIAL_ITEM3";

            /// <summary>
            /// 交班时病人特殊项4
            /// </summary>
            public const string SPECIAL_ITEM4 = "SPECIAL_ITEM4";

            /// <summary>
            /// 交班时病人特殊项5
            /// </summary>
            public const string SPECIAL_ITEM5 = "SPECIAL_ITEM5";

            /// <summary>
            /// 护理交班内容
            /// </summary>
            public const string SHIFT_CONTENT = "SHIFT_CONTENT";

            /// <summary>
            /// 交班时病人血压
            /// </summary>
            public const string BLOODPRESSURE_VALUE = "BLOODPRESSURE_VALUE";

            /// <summary>
            /// 交班时病人饮食
            /// </summary>
            public const string DIET = "DIET";

            /// <summary>
            /// 交班时病人不良反应
            /// </summary>
            public const string ADVERSEREACTION = "ADVERSEREACTION";

            /// <summary>
            /// 交班时病人经管医生
            /// </summary>
            public const string REQUESTDOCTOR_NAME = "REQUESTDOCTOR_NAME";

            /// <summary>
            /// 交班时病人上级医生
            /// </summary>
            public const string PARENTDOCTOR_NAME = "PARENTDOCTOR_NAME";
        }

        /// <summary>
        /// 护理交班本次交班病区状况表
        /// </summary>
        internal struct ShiftWardStatusTable
        {
            /// <summary>
            /// 交班记录ID
            /// </summary>
            public const string SHIFT_RECORD_ID = "SHIFT_RECORD_ID";

            /// <summary>
            /// 交班项目名称
            /// </summary>
            public const string SHIFT_ITEM_NAME = "SHIFT_ITEM_NAME";

            /// <summary>
            /// 交班项目代码
            /// </summary>
            public const string SHIFT_ITEM_CODE = "SHIFT_ITEM_CODE";

            /// <summary>
            /// 交班项目描述
            /// </summary>
            public const string SHIFT_ITEM_DESC = "SHIFT_ITEM_DESC";
        }
       
        /// <summary>
        /// 护理交班项目配置别名
        /// </summary>
        internal struct ShiftItemAliasTable
        {
            /// <summary>
            /// 交班项目别名
            /// </summary>
            public const string ITEM_ALIAS = "ITEM_ALIAS";

            /// <summary>
            /// 交班项目别名代码
            /// </summary>
            public const string ITEM_ALIASCODE = "ITEM_ALIASCODE";

            /// <summary>
            /// 交班项目名称
            /// </summary>
            public const string ITEM_NAME = "ITEM_NAME";

            /// <summary>
            /// 交班项目代码
            /// </summary>
            public const string ITEM_CODE = "ITEM_CODE";

            /// <summary>
            /// 病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";
        }

        /// <summary>
        /// 护理交班特殊病人交班表
        /// </summary>
        internal struct ShiftSpecialPatientTable
        {
            /// <summary>
            /// 交班记录ID
            /// </summary>
            public const string SHIFT_RECORD_ID = "SHIFT_RECORD_ID";

            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 病人序号
            /// </summary>
            public const string PATIENT_NO = "PATIENT_NO";

            /// <summary>
            /// 就诊ID
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 病人子ID
            /// </summary>
            public const string SUB_ID = "SUB_ID";

            /// <summary>
            /// 病人姓名
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 病人年龄
            /// </summary>
            public const string PATIENT_AGE = "PATIENT_AGE";

            /// <summary>
            /// 病人入科时间
            /// </summary>
            public const string LOG_DATE_TIME = "LOG_DATE_TIME";

            /// <summary>
            /// 病人性别
            /// </summary>
            public const string PATIENT_SEX = "PATIENT_SEX";

            /// <summary>
            /// 交班时病人所在病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 交班时病人所在病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 交班时病人床号
            /// </summary>
            public const string BED_CODE = "BED_CODE";

            /// <summary>
            /// 交班时病人诊断
            /// </summary>
            public const string DIAGNOSIS = "DIAGNOSIS";

            /// <summary>
            /// 交班时病人其他信息
            /// </summary>
            public const string OTHERSINFO = "OTHERSINFO";

            /// <summary>
            /// 病人过敏药物
            /// </summary>
            public const string ALLERGICDRUG = "ALLERGICDRUG";

            /// <summary>
            /// 病人不良反应药物
            /// </summary>
            public const string ADVERSEREACTIONDRUG = "ADVERSEREACTIONDRUG";

            /// <summary>
            /// 交班时病人护理
            /// </summary>
            public const string NURSING_CLASS = "NURSING_CLASS";

            /// <summary>
            /// 交班时病人饮食
            /// </summary>
            public const string DIET = "DIET";

            /// <summary>
            /// 交班时病人经管医生
            /// </summary>
            public const string REQUESTDOCTOR_NAME = "REQUESTDOCTOR_NAME";

            /// <summary>
            /// 交班时病人备注
            /// </summary>
            public const string REMARK = "REMARK";

            /// <summary>
            /// 交班日期
            /// </summary>
            public const string SHIFT_DATE = "SHIFT_DATE";
        }

        /// <summary>
        /// 病历指控审核表
        /// </summary>
        internal struct QCExamineTable
        {
            /// <summary>
            /// 审核内容标识
            /// </summary>
            public const string QC_CONTENT_KEY = "QC_CONTENT_KEY";

            /// <summary>
            /// 审核内容类型
            /// </summary>
            public const string QC_CONTENT_TYPE = "QC_CONTENT_TYPE";

            /// <summary>
            /// 病人ID
            /// </summary>
            public const string PATIENT_ID = "PATIENT_ID";

            /// <summary>
            /// 病人姓名
            /// </summary>
            public const string PATIENT_NAME = "PATIENT_NAME";

            /// <summary>
            /// 病人VISIT_ID
            /// </summary>
            public const string VISIT_ID = "VISIT_ID";

            /// <summary>
            /// 病区代码
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 病区名
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 审核状态
            /// </summary>
            public const string QC_EXAMINE_STATUS = "QC_EXAMINE_STATUS";

            /// <summary>
            /// 审核内容
            /// </summary>
            public const string QC_EXAMINE_CONTENT = "QC_EXAMINE_CONTENT";

            /// <summary>
            /// 审核人ID
            /// </summary>
            public const string QC_EXAMINE_ID = "QC_EXAMINE_ID";

            /// <summary>
            /// 审核人姓名
            /// </summary>
            public const string QC_EXAMINE_NAME = "QC_EXAMINE_NAME";

            /// <summary>
            /// 审核时间
            /// </summary>
            public const string QC_EXAMINE_TIME = "QC_EXAMINE_TIME";
        }

        /// <summary>
        /// 食物成分库表字段定义
        /// </summary>
        internal struct FoodEleTable
        {
            /// <summary>
            /// 食物编码
            /// </summary>
            public const string Food_Code = "Food_Code";

            /// <summary>
            /// 食物名称
            /// </summary>
            public const string Food_Name = "Food_Name";

            /// <summary>
            /// 食物名称拼音首字母
            /// </summary>
            public const string Input_Code = "Input_Code";

            public const string Edible = "Edible";
            public const string Water = "Water";
            public const string Kcal = "Kcal";
            public const string Kj = "Kj";
            public const string Protein = "Protein";
            public const string Fat = "Fat";
            public const string Cho = "Cho";
            public const string Fiber = "Fiber";
            public const string Cholesterol = "Cholesterol";
            public const string Ash = "Ash";
            public const string Va = "Va";
            public const string Carotene = "Carotene";
            public const string Retinol = "Retinol";
            public const string Thiamin = "Thiamin";
            public const string Ribo = "Ribo";
            public const string Niacin = "Niacin";
            public const string Vc = "Vc";
            public const string Vetotal = "Vetotal";
            public const string VeAe = "Ve_Ae";
            public const string VeBge = "Ve_Bge";
            public const string VeTe = "Ve_Te";
            public const string Ca = "Ca";
            public const string P = "P";
            public const string K = "K";
            public const string Na = "Na";
            public const string Mg = "Mg";
            public const string Fe = "Fe";
            public const string Zn = "Zn";
            public const string Se = "Se";
            public const string Cu = "Cu";
            public const string Mn = "Mn";
            public const string Remark = "Remark";
            public const string FolicAcid = "Folic_Acid";
            public const string Iodine = "Iodine";
            public const string IsofTotal = "Isof_Total";
            public const string IsofDaidzein = "Isof_Daidzein";
            public const string IsofGenistein = "Isof_Genistein";
            public const string IsofGlyCitein = "Isof_GlyCitein";
            public const string Gi = "Gi";
        }

        /// <summary>
        /// 文档索引信息数据表字段定义
        /// </summary>
        internal struct QCDocTable
        {
            /// <summary>
            /// 文档的唯一标识
            /// </summary>
            public const string DOC_ID = "DOC_ID";

            /// <summary>
            /// 文档的类型
            /// </summary>
            public const string DOC_TYPE_ID = "DOC_TYPE";

            /// <summary>
            /// 文档的显示标题
            /// </summary>
            public const string DOC_TITLE = "DOC_TITLE";

            /// <summary>
            /// 文档生成的时间
            /// </summary>
            public const string DOC_TIME = "DOC_TIME";

            /// <summary>
            /// 文档集编号
            /// </summary>
            public const string DOC_SETID = "DOC_SETID";

            /// <summary>
            /// 文档版本
            /// </summary>
            public const string DOC_VERSION = "DOC_VERSION";

            /// <summary>
            /// 文档法定作者的标号
            /// </summary>
            public const string CREATOR_ID = "CREATOR_ID";

            /// <summary>
            /// 文档法定作者姓名
            /// </summary>
            public const string CREATOR_NAME = "CREATOR_NAME";

            /// <summary>
            /// 文档法定作者所属病区名称
            /// </summary>
            public const string WARD_NAME = "WARD_NAME";

            /// <summary>
            /// 文档法定作者所属病区编号
            /// </summary>
            public const string WARD_CODE = "WARD_CODE";

            /// <summary>
            /// 文档修改者的标号
            /// </summary>
            public const string MODIFIER_ID = "MODIFIER_ID";

            /// <summary>
            /// 文档修改者的标号
            /// </summary>
            public const string MODIFIER_NAME = "MODIFIER_NAME";

            /// <summary>
            /// 文档修改时间
            /// </summary>
            public const string MODIFY_TIME = "MODIFY_TIME";

            /// <summary>
            /// 签名代码
            /// </summary>
            public const string SIGN_CODE = "SIGN_CODE";

            /// <summary>
            /// 隐私等级代码
            /// </summary>
            public const string CONFID_CODE = "CONFID_CODE";

            /// <summary>
            /// 文档次序编号
            /// </summary>
            public const string ORDER_VALUE = "ORDER_VALUE";

            /// <summary>
            /// 病历护士记录的时间
            /// </summary>
            public const string RECORD_TIME = "RECORD_TIME";

            /// <summary>
            /// 病历关联的护理记录ID
            /// </summary>
            public const string RECORD_ID = "RECORD_ID";

            /// <summary>
            /// 文档数据
            /// </summary>
            public const string DOC_DATA = "DOC_DATA";
        }

        #endregion

        /// <summary>
        /// SQL命令常量
        /// </summary>
        internal struct SQL
        {
            /// <summary>
            /// "INSERT INTO {0}({1}) VALUES({2})"
            /// </summary>
            public const string INSERT = "INSERT INTO {0}({1}) VALUES({2})";

            /// <summary>
            /// "UPDATE {0} SET {1} WHERE {2}"
            /// </summary>
            public const string UPDATE = "UPDATE {0} SET {1} WHERE {2}";

            /// <summary>
            /// "DELETE FROM {0} WHERE {1}"
            /// </summary>
            public const string DELETE = "DELETE FROM {0} WHERE {1}";

            /// <summary>
            /// "DELETE FROM {0}"
            /// </summary>
            public const string DELETE_ALL = "DELETE FROM {0}";

            /// <summary>
            /// "{0} UNION {1}"
            /// </summary>
            public const string UNION = "{0} UNION {1}";

            /// <summary>
            /// "SELECT {0}"
            /// </summary>
            public const string SELECT = "SELECT {0}";

            /// <summary>
            /// "SELECT {0} FROM {1}"
            /// </summary>
            public const string SELECT_FROM = "SELECT {0} FROM {1}";

            /// <summary>
            /// "SELECT {0} FROM {1} WHERE {2}"
            /// </summary>
            public const string SELECT_WHERE = "SELECT {0} FROM {1} WHERE {2}";

            /// <summary>
            /// "SELECT {0} FROM {1} WHERE {2} IN({3})"
            /// </summary>
            public const string SELECT_WHERE_IN = "SELECT {0} FROM {1} WHERE {2} IN({3})";

            /// <summary>
            /// "SELECT {0} FROM {1} ORDER BY {2} ASC"
            /// </summary>
            public const string SELECT_ORDER_ASC = "SELECT {0} FROM {1} ORDER BY {2} ASC";

            /// <summary>
            /// "SELECT {0} FROM {1} ORDER BY {2} DESC"
            /// </summary>
            public const string SELECT_ORDER_DESC = "SELECT {0} FROM {1} ORDER BY {2} DESC";

            /// <summary>
            /// "SELECT {0} FROM {1} WHERE {2} ORDER BY {3} ASC"
            /// </summary>
            public const string SELECT_WHERE_ORDER_ASC = "SELECT {0} FROM {1} WHERE {2} ORDER BY {3} ASC";

            /// <summary>
            /// "SELECT {0} FROM {1} WHERE {2} ORDER BY {3} DESC"
            /// </summary>
            public const string SELECT_WHERE_ORDER_DESC = "SELECT {0} FROM {1} WHERE {2} ORDER BY {3} DESC";

            /// <summary>
            /// "SELECT DISTINCT {0} FROM {1} WHERE {2} ORDER BY {3} ASC"
            /// </summary>
            public const string SELECT_DISTINCT_WHERE_ORDER_ASC = "SELECT DISTINCT {0} FROM {1} WHERE {2} ORDER BY {3} ASC";

            /// <summary>
            /// "SELECT DISTINCT {0} FROM {1} WHERE {2} ORDER BY {3} DESC"
            /// </summary>
            public const string SELECT_DISTINCT_WHERE_ORDER_DESC = "SELECT DISTINCT {0} FROM {1} WHERE {2} ORDER BY {3} DESC";

            /// <summary>
            /// "SELECT {0} FROM {1} GROUP BY {2}"
            /// </summary>
            public const string SELECT_FROM_GROUP = "SELECT {0} FROM {1} GROUP BY {2}";

            /// <summary>
            /// "SELECT {0} FROM {1} GROUP BY {2} ORDER BY {3} ASC"
            /// </summary>
            public const string SELECT_FROM_GROUP_ORDER_ASC = "SELECT {0} FROM {1} GROUP BY {2} ORDER BY {3} ASC";

            /// <summary>
            /// "SELECT {0} FROM {1} GROUP BY {2} ORDER BY {3} DESC"
            /// </summary>
            public const string SELECT_FROM_GROUP_ORDER_DESC = "SELECT {0} FROM {1} GROUP BY {2} ORDER BY {3} DESC";

            /// <summary>
            /// "SELECT {0} FROM {1} WHERE {2} GROUP BY {3}"
            /// </summary>
            public const string SELECT_FROM_WHERE_GROUP = "SELECT {0} FROM {1} WHERE {2} GROUP BY {3}";

            /// <summary>
            /// "SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ORDER BY {4} ASC"
            /// </summary>
            public const string SELECT_FROM_WHERE_GROUP_ORDER_ASC = "SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ORDER BY {4} ASC";

            /// <summary>
            /// "SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ORDER BY {4} DESC"
            /// </summary>
            public const string SELECT_FROM_WHERE_GROUP_ORDER_DESC = "SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ORDER BY {4} DESC";

            /// <summary>
            /// "Create {0} ({1}) tablespace {2}"
            /// </summary>
            public const string Create_Table = "Create Table {0} ({1}) tablespace {2} pctfree 10 initrans 1 maxtrans 255 storage (initial 64K minextents 1 maxextents unlimited)";

            /// <summary>
            /// 查询excel文件
            /// </summary>
            public const string excelCmdText = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";//HDR=YES表示默认excel表中的第一行是标题

            /// <summary>
            /// 根据两张表中主键是否匹配来决定使用的是insert还是update
            /// </summary>
            public const string MERGE_INTO = "MERGE INTO {0} USING (SELECT {1} FROM {2}){3} ON ({4}) WHEN NOT MATCHED THEN {5} WHEN MATCHED THEN {6}";
        }
    }
}