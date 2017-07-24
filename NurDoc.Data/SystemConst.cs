// ***********************************************************
// 护理电子病历系统,全局常量数据定义类.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL.DbAccess;

namespace Heren.NurDoc.Data
{
    public struct SystemConst
    {
        /// <summary>
        /// 返回值常量
        /// </summary>
        public struct ReturnValue
        {
            /// <summary>
            /// 返回值=0
            /// </summary>
            public const short OK = 0;

            /// <summary>
            /// 返回值=1
            /// </summary>
            public const short FAILED = 1;

            /// <summary>
            /// 返回值=2
            /// </summary>
            public const short CANCEL = 2;

            /// <summary>
            /// 返回值=3
            /// </summary>
            public const short EXCEPTION = 3;

            /// <summary>
            /// 资源未发现=4
            /// </summary>
            public const short NO_FOUND = 4;
        }

        /// <summary>
        /// 程序单实例运行映射名称
        /// </summary>
        public struct MappingName
        {
            /// <summary>
            /// 护理电子病历系统
            /// </summary>
            public const string NURDOC_SYS = "NurDoc";

            /// <summary>
            /// 护理电子病历配置管理系统
            /// </summary>
            public const string CONFIG_SYS = "NdsConfig";

            /// <summary>
            /// 护理电子病历自动升级系统
            /// </summary>
            public const string UPGRADE_SYS = "NdsUpgrade";

            /// <summary>
            /// 护理电子病历护理信息库
            /// </summary>
            public const string INFOLIB_SYS = "InfoLib";

            /// <summary>
            /// 护理电子病历护理信息库
            /// </summary>
            public const string HEALTH_TECH_SYS = "HealthTech";
        }

        /// <summary>
        /// 程序启动参数有关常量
        /// </summary>
        public struct StartupArgs
        {
            /// <summary>
            /// 启动参数中的组间分隔符
            /// </summary>
            public const string GROUP_SPLIT = ";";

            /// <summary>
            /// 启动参数中的组内字段分隔符
            /// </summary>
            public const string FIELD_SPLIT = ",";

            /// <summary>
            /// 启动参数字符串内的被转义字符
            /// </summary>
            public const string ESCAPED_CHAR = " ";

            /// <summary>
            /// 启动参数字符串内的转义符
            /// </summary>
            public const string ESCAPE_CHAR = "$";
        }

        /// <summary>
        /// 某些模块固定使用的表单
        /// </summary>
        public struct FormName
        {
            /// <summary>
            /// 交班记录模块之病区动态表单名称
            /// </summary>
            public const string SHIFT_WARD_STATUS = "病区动态";

            /// <summary>
            /// 交班记录模块之交班信息表单名称
            /// </summary>
            public const string SHIFT_PATIENT = "交班信息";

            /// <summary>
            /// 交班记录模块之交班病人筛选表单名称
            /// </summary>
            public const string SHIFT_PATIENT_FILTER = "病人筛选";

            /// <summary>
            /// 交班记录模块之交班信息表单名称
            /// </summary>
            public const string SHIFT_SPECIALPATIENT = "特殊病人交班信息";

            /// <summary>
            /// 床卡视图模块之床卡视图简卡表单名称
            /// </summary>
            public const string BEDCARD_SIMPLE_CARD = "床卡视图简卡";

            /// <summary>
            /// 床卡视图模块之病人信息表单名称
            /// </summary>
            public const string PATIENT_DETAIL_INFO = "病人信息";

            /// <summary>
            /// 床卡视图模块之床卡视图细卡表单名称
            /// </summary>
            public const string BEDCARD_DETAIL_CARD = "床卡视图细卡";

            /// <summary>
            /// 床卡视图模块之床卡视图列表表单名称
            /// </summary>
            public const string BEDCARD_PATIENT_LIST = "床卡视图列表";

            /// <summary>
            /// 质量与安全管理记录模块之质量与安全管理记录信息表单名称
            /// </summary>
            public const string Quality_Safty_INFO = "质量与安全管理记录";

            /// <summary>
            /// 床卡视图模块之护士排班视图细卡表单名称
            /// </summary>
            public const string ROSTERING_CARD = "护士排班视图细卡";
        }

        /// <summary>
        /// 配置文件配置项名称常量
        /// </summary>
        public struct ConfigKey
        {
            /// <summary>
            /// 默认登录的用户ID号
            /// </summary>
            public const string DEFAULT_LOGIN_USERID = "Heren.NurDoc.DefaultLoginUser";

            /// <summary>
            /// 用户查看Readme的时间
            /// </summary>
            public const string USER_README_TIME = "Heren.NurDoc.Readme";

            /// <summary>
            /// 护理文书是否弹出编辑
            /// </summary>
            public const string NUR_DOC_SHOW_AS_MODEL = "Heren.NurDoc.ShowAsModel";

            /// <summary>
            /// 质量与安全管理记录是否弹出编辑
            /// </summary>
            public const string QC_SHOW_AS_MODEL = "Heren.NurDoc.ShowAsModel";

            /// <summary>
            /// 监护记录单是否保存后显示预览
            /// </summary>
            public const string NUR_DOC_SAVE_PREVIEW = "Heren.NurDoc.SavePreview";

            /// <summary>
            /// 护理文书是否保存后直接返回列表
            /// </summary>
            public const string NUR_DOC_SAVE_RETURN = "Heren.NurDoc.SaveReturn";

            /// <summary>
            /// 质量与安全管理记录是否保存后直接返回列表
            /// </summary>
            public const string QC_SAVE_RETURN = "Heren.NurDoc.SaveReturn";


            /// <summary>
            /// 护理计划是否保存后直接返回列表
            /// </summary>
            public const string NCP_SAVE_RETURN = "Heren.NCP.SaveReturn";
            
            /// <summary>
            /// 护理记录是否弹出编辑
            /// </summary>
            public const string NUR_REC_SHOW_AS_MODEL = "Heren.NurDoc.NurRec.ShowAsModel";

            /// <summary>
            /// 护理记录保存后返回列表
            /// </summary>
            public const string NUR_REC_SAVE_RETURN = "Heren.NurDoc.NurRec.SaveReturn";

            /// <summary>
            /// 床位卡视图显示模式配置
            /// </summary>
            public const string PATIENT_VIEW = "Heren.NurDoc.BedView.PatientView";

            /// <summary>
            /// 床位卡视图显示位置配置
            /// </summary>
            public const string WHITEBOARD_LOCATION = "Heren.NurDoc.Whiteboard.Location";

            /// <summary>
            /// 护理电子病历系统当前版本信息
            /// </summary>
            public const string CURRENT_VERSION = "Heren.NurDoc.Current.Verion";

            /// <summary>
            /// 系统名称配置
            /// </summary>
            public const string SYSTEM_NAME = "Heren.NurDoc.SystemName";

            /// <summary>
            /// 待办任务节点列表默认是否展开所有
            /// </summary>
            public const string TASK_NODE_EXPAND = "Heren.NurDoc.TaskNode.ExpandAll";

            /// <summary>
            /// 查询统计列表默认是否展开所有
            /// </summary>
            public const string STATISTIC_EXPAND = "Heren.NurDoc.Statistic.ExpandAll";

            /// <summary>
            /// 质量与安全管理记录列表默认是否展开所有
            /// </summary>
            public const string QC_EXPAND = "Heren.NurDoc.QC.ExpandAll";

            /// <summary>
            /// 护理记录评估单列表默认是否展开所有
            /// </summary>
            public const string NUR_REC_EXPAND = "Heren.NurDoc.NurRec.ExpandAll";

            /// <summary>
            /// 文档列表1树列表默认是否展开所有
            /// </summary>
            public const string NURDOC_LIST1_EXPAND = "Heren.NurDoc.DocumentList1.ExpandAll";

            /// <summary>
            /// 文档列表2树列表默认是否展开所有
            /// </summary>
            public const string NURDOC_LIST2_EXPAND = "Heren.NurDoc.DocumentList2.ExpandAll";

            /// <summary>
            /// 文档列表3树列表默认是否展开所有
            /// </summary>
            public const string NURDOC_LIST3_EXPAND = "Heren.NurDoc.DocumentList3.ExpandAll";

            /// <summary>
            /// 文档列表4树列表默认是否展开所有
            /// </summary>
            public const string NURDOC_LIST4_EXPAND = "Heren.NurDoc.DocumentList4.ExpandAll";

            /// <summary>
            /// 专科护理评估单列表默认是否展开所有
            /// </summary>
            public const string SPECIAL_NURSING_EXPAND = "Heren.NurDoc.SpecialNursing.ExpandAll";

            /// <summary>
            /// 监护记录评估单列表默认是否展开所有
            /// </summary>
            public const string GRAPH_NURSING_EXPAND = "Heren.NurDoc.GraphNursing.ExpandAll";

            /// <summary>
            /// 护理评估单列表默认是否展开所有
            /// </summary>
            public const string NURSING_ASSESSMENT_EXPAND = "Heren.NurDoc.NursingAssessment.ExpandAll";

            /// <summary>
            /// 护理评价节点列表默认是否展开所有
            /// </summary>
            public const string EVALUATION_EXPAND = "Heren.NurDoc.Evaluation.ExpandAll";
        }
    }
}
