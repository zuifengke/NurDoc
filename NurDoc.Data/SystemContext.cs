// ***********************************************************
// 护理电子病历系统,系统上下文数据缓存类.
// 用于缓存当前登陆用户的相关信息,以及全局的一些配置信息
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;
using Heren.NurDoc.DAL.DbAccess;

namespace Heren.NurDoc.Data
{
    public class SystemContext
    {
        private static SystemContext m_instance = null;

        /// <summary>
        /// 获取系统运行上下文实例
        /// </summary>
        public static SystemContext Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new SystemContext();
                return m_instance;
            }
        }

        private SystemContext()
        {
        }

        private UserInfo m_loginUser = null;

        /// <summary>
        /// 获取或设置登录系统的用户信息
        /// </summary>
        public UserInfo LoginUser
        {
            get
            {
                return this.m_loginUser;
            }

            set
            {
                this.m_loginUser = value;
                if (this.m_loginUser == null)
                {
                    this.m_displayDept = null;
                    return;
                }
                this.m_wardShiftRankList = null;
                this.m_wardShiftItemList = null;
                this.m_displayDept = new DeptInfo();
                this.m_displayDept.DeptCode = value.DeptCode;
                this.m_displayDept.DeptName = value.DeptName;
            }
        }

        private bool m_SinglePatMod = false;

        public bool SinglePatMod
        {
            get
            {
                return this.m_SinglePatMod;
            }
            set
            {
                this.m_SinglePatMod = value;
            }
        }

        private DeptInfo m_displayDept = null;

        /// <summary>
        /// 获取或设置当前显示的科室信息
        /// </summary>
        public DeptInfo DisplayDept
        {
            get
            {
                return this.m_displayDept;
            }

            set
            {
                this.m_displayDept = value;
                this.m_wardShiftRankList = null;
                this.m_wardShiftItemList = null;
            }
        }

        private CommonAccess m_CommonAccess = null;

        /// <summary>
        /// 获取公共方法访问类实例
        /// </summary>
        public CommonAccess CommonAccess
        {
            get
            {
                if (this.m_CommonAccess == null)
                    this.m_CommonAccess = new CommonAccess();
                return this.m_CommonAccess;
            }
        }

        private ConfigAccess m_ConfigAccess = null;

        /// <summary>
        /// 获取配置方法访问类实例
        /// </summary>
        public ConfigAccess ConfigAccess
        {
            get
            {
                if (this.m_ConfigAccess == null)
                    this.m_ConfigAccess = new ConfigAccess();
                return this.m_ConfigAccess;
            }
        }

        private PatVisitAccess m_PatVisitAccess = null;

        /// <summary>
        /// 获取EMR数据库访问类实例
        /// </summary>
        public PatVisitAccess PatVisitAccess
        {
            get
            {
                if (this.m_PatVisitAccess == null)
                    this.m_PatVisitAccess = new PatVisitAccess();
                return this.m_PatVisitAccess;
            }
        }

        private AccountAccess m_AccountAccess = null;

        /// <summary>
        /// 获取用户权限访问类实例
        /// </summary>
        public AccountAccess AccountAccess
        {
            get
            {
                if (this.m_AccountAccess == null)
                    this.m_AccountAccess = new AccountAccess();
                return this.m_AccountAccess;
            }
        }

        private DocTypeAccess m_DocTypeAccess = null;

        /// <summary>
        /// 获取模板访问类实例
        /// </summary>
        public DocTypeAccess DocTypeAccess
        {
            get
            {
                if (this.m_DocTypeAccess == null)
                    this.m_DocTypeAccess = new DocTypeAccess();
                return this.m_DocTypeAccess;
            }
        }

        private TempletAccess m_TempletAccess = null;

        /// <summary>
        /// 获取模板访问类实例
        /// </summary>
        public TempletAccess TempletAccess
        {
            get
            {
                if (this.m_TempletAccess == null)
                    this.m_TempletAccess = new TempletAccess();
                return this.m_TempletAccess;
            }
        }

        private WordTempAccess m_WordTempAccess = null;

        /// <summary>
        /// 获取word模板访问类实例
        /// </summary>
        public WordTempAccess WordTempAccess
        {
            get
            {
                if (this.m_WordTempAccess == null)
                    this.m_WordTempAccess = new WordTempAccess();
                return this.m_WordTempAccess;
            }
        }

        private InfoLibAccess m_InfoLibAccess = null;

        /// <summary>
        /// 获取模板访问类实例
        /// </summary>
        public InfoLibAccess InfoLibAccess
        {
            get
            {
                if (this.m_InfoLibAccess == null)
                    this.m_InfoLibAccess = new InfoLibAccess();
                return this.m_InfoLibAccess;
            }
        }

        private NurApplyAccess m_NurApplyAccess = null;

        /// <summary>
        /// 获取护理申请访问类实例
        /// </summary>
        public NurApplyAccess NurApplyAccess
        {
            get
            {
                if (this.m_NurApplyAccess == null)
                    this.m_NurApplyAccess = new NurApplyAccess();
                return this.m_NurApplyAccess;
            }
        }

        private QcQuestionAccess m_QcQuestionAccess = null;

        /// <summary>
        /// 获取质检信息访问类实例
        /// </summary>
        public QcQuestionAccess QcQuestionAccess
        {
            get
            {
                if (this.m_QcQuestionAccess == null)
                    this.m_QcQuestionAccess = new QcQuestionAccess();
                return this.m_QcQuestionAccess;
            }
        }

        private CarePlanAccess m_CarePlanAccess = null;

        /// <summary>
        /// 获取护理计划访问类实例
        /// </summary>
        public CarePlanAccess CarePlanAccess
        {
            get
            {
                if (this.m_CarePlanAccess == null)
                    this.m_CarePlanAccess = new CarePlanAccess();
                return this.m_CarePlanAccess;
            }
        }

        private DocumentAccess m_DocumentAccess = null;

        /// <summary>
        /// 获取文档访问类实例
        /// </summary>
        public DocumentAccess DocumentAccess
        {
            get
            {
                if (this.m_DocumentAccess == null)
                    this.m_DocumentAccess = new DocumentAccess();
                return this.m_DocumentAccess;
            }
        }

        private QCAccess m_QCAccess = null;

        /// <summary>
        /// 获取质量与安全管理记录访问类实例
        /// </summary>
        public QCAccess QCAccess
        {
            get
            {
                if (this.m_QCAccess == null)
                    this.m_QCAccess = new QCAccess();
                return this.m_QCAccess;
            }
        }

        private NurRecAccess m_NurRecAccess = null;

        /// <summary>
        /// 获取护理记录数据访问类实例
        /// </summary>
        public NurRecAccess NurRecAccess
        {
            get
            {
                if (this.m_NurRecAccess == null)
                    this.m_NurRecAccess = new NurRecAccess();
                return this.m_NurRecAccess;
            }
        }

        private NurShiftAccess m_NurShiftAccess = null;

        /// <summary>
        /// 获取护士交班数据访问类实例
        /// </summary>
        public NurShiftAccess NurShiftAccess
        {
            get
            {
                if (this.m_NurShiftAccess == null)
                    this.m_NurShiftAccess = new NurShiftAccess();
                return this.m_NurShiftAccess;
            }
        }

        private QCExamineAccess m_QCExamineAccess = null;

        /// <summary>
        /// 获取护士交班数据访问类实例
        /// </summary>
        public QCExamineAccess QCExamineAccess
        {
            get
            {
                if (this.m_QCExamineAccess == null)
                    this.m_QCExamineAccess = new QCExamineAccess();
                return this.m_QCExamineAccess;
            }
        }

        private VitalSignsAccess m_VitalSignsAccess = null;

        /// <summary>
        /// 获取体征记录数据访问类实例
        /// </summary>
        public VitalSignsAccess VitalSignsAccess
        {
            get
            {
                if (this.m_VitalSignsAccess == null)
                    this.m_VitalSignsAccess = new VitalSignsAccess();
                return this.m_VitalSignsAccess;
            }
        }

        private UpgradeAccess m_UpgradeAccess = null;

        /// <summary>
        /// 获取程序升级数据访问类实例
        /// </summary>
        public UpgradeAccess UpgradeAccess
        {
            get
            {
                if (this.m_UpgradeAccess == null)
                    this.m_UpgradeAccess = new UpgradeAccess();
                return this.m_UpgradeAccess;
            }
        }

        private EvaTypeAccess m_EvaTypeAccess = null;

        /// <summary>
        /// 获取护理评价类型访问类实例
        /// </summary>
        public EvaTypeAccess EvaTypeAccess
        {
            get
            {
                if (this.m_EvaTypeAccess == null)
                    this.m_EvaTypeAccess = new EvaTypeAccess();
                return this.m_EvaTypeAccess;
            }
        }

        private EvaDocInfoAccess m_EvaDocInfoAccess = null;

        /// <summary>
        /// 获取文档访问类实例
        /// </summary>
        public EvaDocInfoAccess EvaDocInfoAccess
        {
            get
            {
                if (this.m_EvaDocInfoAccess == null)
                    this.m_EvaDocInfoAccess = new EvaDocInfoAccess();
                return this.m_EvaDocInfoAccess;
            }
        }

        private FoodEleAccess m_FoodEleAccess = null;

        /// <summary>
        /// 获取食物成分访问类实例
        /// </summary>
        public FoodEleAccess FoodEleAccess
        {
            get
            {
                if (this.m_FoodEleAccess == null)
                    this.m_FoodEleAccess = new FoodEleAccess();
                return this.m_FoodEleAccess;
            }
        }

        private QCTypeAccess m_QCTypeAccess = null;

        /// <summary>
        /// 获取质量与安全管理记录类型访问类实例
        /// </summary>
        public QCTypeAccess QCTypeAccess
        {
            get
            {
                if (this.m_QCTypeAccess == null)
                    this.m_QCTypeAccess = new QCTypeAccess();
                return this.m_QCTypeAccess;
            }
        }

        private SystemOption m_systemOption = null;

        /// <summary>
        /// 获取服务器端配置的系统选项
        /// </summary>
        public SystemOption SystemOption
        {
            get
            {
                //if (this.m_loginUser == null)
                //    return new SystemOption();
                if (this.m_systemOption != null)
                    return this.m_systemOption;

                ConfigService.Instance.GetSystemOptions(ref this.m_systemOption);
                if (this.m_systemOption == null)
                    return new SystemOption();
                return this.m_systemOption;
            }
        }

        private WindowName m_windowName = null;

        /// <summary>
        /// 获取服务器端配置的窗口名称配置
        /// </summary>
        public WindowName WindowName
        {
            get
            {
                if (this.m_loginUser == null)
                    return new WindowName();
                if (this.m_windowName != null)
                    return this.m_windowName;

                ConfigService.Instance.GetWindowNames(ref this.m_windowName);
                if (this.m_windowName == null)
                    return new WindowName();
                return this.m_windowName;
            }
        }

        private string szSystemName = null;

        /// <summary>
        /// 当前系统名称
        /// </summary>
        public string SystemName
        {
            get
            {
                if (szSystemName == null)
                {
                    szSystemName = SystemConfig.Instance.Get(SystemConst.ConfigKey.SYSTEM_NAME, string.Empty);
                }
                return szSystemName;
            }
        }

        /// <summary>
        /// 当系统用户信息切换成功后触发
        /// </summary>
        [Description("当系统用户信息切换成功后触发")]
        public event UserChangedEventHandler UserChanged;

        /// <summary>
        /// 当当前显示的科室切换成功后触发
        /// </summary>
        [Description("当当前显示的科室切换成功后触发")]
        public event EventHandler DisplayDeptChanged;

        /// <summary>
        /// 当当前显示的数据变化后触发
        /// </summary>
        [Description("当当前显示的数据变化后触发")]
        public event EventHandler DataChanged;

        /// <summary>
        /// 触发系统模板数据刷新成功事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">事件参数</param>
        public void OnDataChanged(object sender, EventArgs e)
        {
            if (this.DataChanged != null)
                this.DataChanged(sender, e);
        }

        /// <summary>
        /// 触发系统用户信息切换成功事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">事件参数</param>
        public void OnUserChanged(object sender, UserChangedEventArgs e)
        {
            if (this.UserChanged != null)
                this.UserChanged(sender, e);
        }

        /// <summary>
        /// 触发系统用户病区切换成功事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">事件参数</param>
        public void OnDisplayDeptChanged(object sender, EventArgs e)
        {
            if (this.DisplayDeptChanged != null)
                this.DisplayDeptChanged(sender, e);
        }

        public bool GetSystemContext(string name, ref object value)
        {
            #region"系统上下文数据"
            if (name == "用户ID号" || name == "用户编号")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.ID;
                return true;
            }
            if (name == "用户姓名")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.Name;
                return true;
            }
            if (name == "用户密码")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.Password;
                return true;
            }
            if (name == "用户科室代码")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.DeptCode;
                return true;
            }
            if (name == "用户科室名称")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.DeptName;
                return true;
            }
            if (name == "用户病区代码")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.WardCode;
                return true;
            }
            if (name == "用户病区名称")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.WardName;
                return true;
            }
            if (name == "病人ID号" || name == "病人编号")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.PatientId;
                return true;
            }
            if (name == "病人姓名")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.PatientName;
                return true;
            }
            if (name == "病人性别")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.PatientSex;
                return true;
            }
            if (name == "病人婚姻")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.MaritalStatus;
                return true;
            }
            if (name == "病人生日")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.BirthTime;
                return true;
            }
            if (name == "病人年龄")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = GlobalMethods.SysTime.GetAgeText(
                    PatientTable.Instance.ActivePatient.BirthTime,
                    PatientTable.Instance.ActivePatient.VisitTime);
                return true;
            }
            //if (name == "病人民族")
            //{
            //    if (PatientTable.Instance.ActivePatient == null)
            //        return false;
            //    value = PatientTable.Instance.ActivePatient.Nation;
            //    return true;
            //}
            if (name == "病人病情")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.PatientCondition;
                return true;
            }
            if (name == "护理等级")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.NursingClass;
                return true;
            }
            if (name == "入院次" || name == "就诊号")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.VisitId;
                return true;
            }
            if (name == "入院时间")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.VisitTime;
                return true;
            }
            if (name == "就诊科室代码")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.DeptCode;
                return true;
            }
            if (name == "就诊科室名称")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.DeptName;
                return true;
            }
            if (name == "就诊病区代码")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.WardCode;
                return true;
            }
            if (name == "就诊病区名称")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.WardName;
                return true;
            }
            if (name == "就诊类型")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.VisitType;
                return true;
            }
            if (name == "床号")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.BedCode;
                return true;
            }
            if (name == "住院号")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.InpNo;
                return true;
            }
            if (name == "诊断")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.Diagnosis;
                return true;
            }
            if (name == "过敏药物")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.AllergyDrugs;
                return true;
            }
            if (name == "当前时间")
            {
                value = SysTimeService.Instance.Now;
                return true;
            }
            if (name == "当前病区病人列表"
                || name == "当前科室病人列表")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = PatientTable.Instance.DeptPatientTable;
                return true;
            }
            if (name == "当前显示病人列表")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = PatientTable.Instance.DisplayPatientTable;
                return true;
            }
            if (name == "当前科室代码")
            {
                if (SystemContext.Instance.DisplayDept == null)
                    return false;
                value = SystemContext.Instance.DisplayDept.DeptCode;
                return true;
            }
            if (name == "用户所在组")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                List<DeptInfo> lstDeptInfo = this.GetUserDeptList(SystemContext.Instance.LoginUser.ID);
                string szDeptInfos = string.Empty;
                foreach (DeptInfo deptInfo in lstDeptInfo)
                {
                    szDeptInfos += deptInfo.DeptName + ",";
                }
                value = szDeptInfos;
                return true;
            }
            if (name == "当前科室名称")
            {
                if (SystemContext.Instance.DisplayDept == null)
                    return false;
                value = SystemContext.Instance.DisplayDept.DeptName;
                return true;
            }
            if (name == "评估视图")
            {
                DataTable dtNursingAssessInfo = new DataTable();
                DataServices.WardPatientsService.Instance.GetWardPatientsNursingAssess(PatientTable.Instance.DisplayPatientTable, ref dtNursingAssessInfo);
                value = dtNursingAssessInfo;
                return true;
            }
            if (name == "单病人评估任务")
            {
                DataTable dtPatientInfo = null;
                if (value is DataTable)
                {
                    dtPatientInfo = value as DataTable;
                }
                else
                {
                    DateTime dtCurrentTime = SysTimeService.Instance.Now;
                    dtPatientInfo = new DataTable();
                    dtPatientInfo.Columns.Add("PATIENT_ID");
                    dtPatientInfo.Columns.Add("VISIT_ID");
                    dtPatientInfo.Columns.Add("BeginTime");
                    dtPatientInfo.Columns.Add("EndTime");
                    dtPatientInfo.Columns.Add("PreBeginTime");
                    dtPatientInfo.Columns.Add("PreEndTime");
                    ShiftRankInfo shiftRankInfo = new ShiftRankInfo();
                    List<ShiftRankInfo> lstNursingShiftInfo = new List<ShiftRankInfo>();
                    string szWardCode = SystemContext.Instance.LoginUser.WardCode;
                    short shRet = NurShiftService.Instance.GetShiftRankInfos(szWardCode, null, ref lstNursingShiftInfo);
                    foreach (ShiftRankInfo item in lstNursingShiftInfo)
                    {
                        item.UpdateShiftRankTime(dtCurrentTime);
                        if (dtCurrentTime >= item.StartTime && dtCurrentTime <= item.EndTime)
                        {
                            shiftRankInfo = item;
                            break;
                        }
                    }
                    DataRow row = dtPatientInfo.Rows.Add();
                    row["PATIENT_ID"] = PatientTable.Instance.ActivePatient.PatientId;
                    row["VISIT_ID"] = PatientTable.Instance.ActivePatient.VisitId;
                    row["BeginTime"] = shiftRankInfo.StartTime;
                    row["EndTime"] = shiftRankInfo.EndTime;
                }
                DataTable dtNursingAssessInfo = new DataTable();
                PatientTaskService.Instance.GetPatientAssessTaskList(dtPatientInfo, ref dtNursingAssessInfo);
                value = dtNursingAssessInfo;
                return true;
            }
            if (!string.IsNullOrEmpty(name) && name.StartsWith("用户权限_"))
            {
                value = false;
                NurUserRight userRight = RightController.Instance.UserRight;
                PropertyInfo[] properties = typeof(NurUserRight).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    object propertyValue = property.GetValue(userRight, null);
                    RightInfo rightInfo = propertyValue as RightInfo;
                    if (rightInfo == null)
                        continue;
                    string szRightName = "用户权限_" + rightInfo.Name;
                    if (string.Compare(szRightName, name, false) == 0)
                    {
                        value = (rightInfo != null && rightInfo.Value);
                        return true;
                    }
                }
            }
            if (!string.IsNullOrEmpty(name) && name.StartsWith("本地配置_"))
            {
                int index = name.IndexOf("_");
                if (index < name.Length - 1)
                {
                    value = this.GetConfig(name.Substring(index + 1), string.Empty);
                    return true;
                }
            }
            if (!string.IsNullOrEmpty(name) && name.StartsWith("拼音首字母_"))
            {
                int index = name.IndexOf("_");
                if (index < name.Length - 1)
                {
                    value = name.Substring(index + 1);
                    value = GlobalMethods.Convert.GetInputCode(value.ToString(), true, 10);
                    return true;
                }
            }
            return false;
            #endregion
        }

        #region"在白板中配置的项"
        private Hashtable m_timePointTable = null;

        /// <summary>
        /// 获取或设置体征时间点列表
        /// </summary>
        public Hashtable TimePointTable
        {
            get
            {
                if (this.m_timePointTable == null)
                {
                    this.m_timePointTable = new Hashtable();
                    this.m_timePointTable.Add("通用筛选体征", new int[] { 2, 6, 10, 14, 18, 22 });
                    return this.m_timePointTable;
                }
                return this.m_timePointTable;
            }

            set
            {
                this.m_timePointTable = value;
            }
        }

        private int[] m_standardBPTimePoint = null;

        /// <summary>
        /// 获取或设置标准版血压时间点
        /// </summary>
        public int[] StandardBPTimePoint
        {
            get
            {
                if (this.m_standardBPTimePoint == null || this.m_standardBPTimePoint.Length != 4)
                {
                    this.m_standardBPTimePoint = new int[] { 4, 8, 12, 16 };
                    return this.m_standardBPTimePoint;
                }
                return this.m_standardBPTimePoint;
            }

            set
            {
                this.m_standardBPTimePoint = value;
            }
        }

        private string[] m_tempTypeList = null;

        /// <summary>
        /// 获取或设置全局体温类型列表
        /// </summary>
        public string[] TempTypeList
        {
            get
            {
                if (this.m_tempTypeList == null)
                    return new string[] { "腋温", "耳温", "口温", "肛温" };
                return this.m_tempTypeList;
            }

            set
            {
                this.m_tempTypeList = value;
            }
        }

        private Dictionary<string, Color> m_patientCardColorList = null;

        /// <summary>
        /// 获取或设置床卡底色颜色列表
        /// </summary>
        public Dictionary<string, Color> PatientCardColorList
        {
            get
            {
                if (this.m_patientCardColorList == null || this.m_patientCardColorList.Count <= 0)
                {
                    m_patientCardColorList = new Dictionary<string, Color>();
                    m_patientCardColorList.Add("鼠标正常颜色上", Color.WhiteSmoke);
                    m_patientCardColorList.Add("鼠标正常颜色下", Color.LightSteelBlue);
                    m_patientCardColorList.Add("鼠标悬停颜色上", Color.White);
                    m_patientCardColorList.Add("鼠标悬停颜色下", Color.MediumOrchid);
                    m_patientCardColorList.Add("鼠标选中颜色上", Color.MediumOrchid);
                    m_patientCardColorList.Add("鼠标选中颜色下", Color.White);
                }

                return this.m_patientCardColorList;
            }

            set
            {
                this.m_patientCardColorList = value;
            }
        }

        /// <summary>
        /// 判断指定体征类型名称是否是体温类型
        /// </summary>
        /// <param name="szTypeName">体征类型名称</param>
        /// <returns>是否是体温类型</returns>
        public bool IsBodyTemperatureType(string szTypeName)
        {
            if (szTypeName == "体温")
                return true;
            szTypeName = this.GetVitalSignsName(szTypeName);
            foreach (string currTypeName in this.TempTypeList)
            {
                if (string.Compare(currTypeName, szTypeName, true) == 0)
                    return true;
            }
            return false;
        }

        private string[] m_liquidOrderList = null;

        /// <summary>
        /// 获取或设置全局液体医嘱类型列表
        /// </summary>
        public string[] LiquidOrderList
        {
            get
            {
                if (this.m_liquidOrderList == null)
                    return new string[] { "静滴" };
                return this.m_liquidOrderList;
            }

            set
            {
                this.m_liquidOrderList = value;
            }
        }

        private string m_VitalSignNoTimePoint = null;

        /// <summary>
        /// 体征数据存入不带时间点
        /// </summary>
        public string VitalSignNoTimePoint
        {
            get
            {
                if (this.m_VitalSignNoTimePoint == null)
                    return string.Empty;
                return this.m_VitalSignNoTimePoint;
            }

            set
            {
                this.m_VitalSignNoTimePoint = value;
            }
        }

        private string m_VitalSignTimeBeforeOneDay = null;

        /// <summary>
        /// 录入体征时间减一天
        /// </summary>
        public string VitalSignTimeBeforeOneDay
        {
            get
            {
                if (this.m_VitalSignTimeBeforeOneDay == null)
                    return string.Empty;
                return this.m_VitalSignTimeBeforeOneDay;
            }

            set
            {
                this.m_VitalSignTimeBeforeOneDay = value;
            }
        }

        /// <summary>
        /// 判断指定医嘱类型是否是液体
        /// </summary>
        /// <param name="szOrderAdm">医嘱途径</param>
        /// <returns>是否是液体医嘱</returns>
        public bool IsLiquidOrder(string szOrderAdm)
        {
            if (this.LiquidOrderList == null)
                return false;
            foreach (string currOrderAdm in this.LiquidOrderList)
            {
                if (string.Compare(currOrderAdm, szOrderAdm, true) == 0)
                    return true;
            }
            return false;
        }

        private string[] m_summaryNameList = null;

        /// <summary>
        /// 获取或设置护理记录小结名称列表
        /// </summary>
        public string[] SummaryNameList
        {
            get
            {
                return this.m_summaryNameList;
            }
            set
            {
                this.m_summaryNameList = value;
            }
        }

        private string m_ShiftItemComparer = null;

        /// <summary>
        /// 获取或设置交接班项目排序顺序
        /// </summary>
        public string ShiftItemComparer
        {
            get
            {
                return this.m_ShiftItemComparer;
            }
            set
            {
                this.m_ShiftItemComparer = value;
            }
        }

        private Hashtable m_vitalSignsNameTable = null;

        /// <summary>
        /// 获取或设置白板脚本中配置的体征名称表
        /// </summary>
        public Hashtable VitalSignsNameTable
        {
            get
            {
                if (this.m_vitalSignsNameTable == null)
                    return new Hashtable();
                return this.m_vitalSignsNameTable;
            }

            set
            {
                this.m_vitalSignsNameTable = value;
            }
        }

        private Hashtable m_vitalSignsValueTable = null;

        /// <summary>
        /// 获取或设置白板脚本中配置的体征值映射表
        /// </summary>
        public Hashtable VitalSignsValueTable
        {
            get
            {
                if (this.m_vitalSignsValueTable == null)
                    return new Hashtable();
                return this.m_vitalSignsValueTable;
            }

            set
            {
                this.m_vitalSignsValueTable = value;
            }
        }

        private List<NurCarePlanConfig> m_NCPButtonConfigTable = null;

        /// <summary>
        /// 获取或设置个性化护理计划按钮（市三）
        /// </summary>
        public List<NurCarePlanConfig> NCPButtonConfigTable
        {
            get
            {
                if (m_NCPButtonConfigTable == null)
                    m_NCPButtonConfigTable = new List<NurCarePlanConfig>();
                return this.m_NCPButtonConfigTable;
            }

            set
            {
                this.m_NCPButtonConfigTable = value;
            }
        }

        private Hashtable m_orderWayValueTable = null;

        /// <summary>
        /// 获取或设置白板脚本中配置的医嘱途径映射表
        /// </summary>
        public Hashtable OrderWayValueTable
        {
            get
            {
                if (this.m_orderWayValueTable == null)
                    return new Hashtable();
                return this.m_orderWayValueTable;
            }

            set
            {
                this.m_orderWayValueTable = value;
            }
        }

        private Hashtable m_orderCategoryTable = null;

        /// <summary>
        /// 获取或设置白板脚本中配置的医嘱类别映射表
        /// </summary>
        public Hashtable OrderCategoryTable
        {
            get
            {
                if (this.m_orderCategoryTable == null)
                    return new Hashtable();
                return this.m_orderCategoryTable;
            }

            set
            {
                this.m_orderCategoryTable = value;
            }
        }

        /// 从白板脚本中配置的体征名称表中获取指定的体征名称
        /// </summary>
        /// <param name="originName">本系统定义的体征名称</param>
        /// <returns>映射后的体征名称</returns>
        public string GetVitalSignsName(string originName)
        {
            if (GlobalMethods.Misc.IsEmptyString(originName)
                || this.m_vitalSignsNameTable == null
                || !this.m_vitalSignsNameTable.Contains(originName))
                return originName;
            object value = this.m_vitalSignsNameTable[originName];
            return GlobalMethods.Misc.IsEmptyString(value) ? originName : value.ToString();
        }

        /// <summary>
        /// 从白板脚本中配置的医嘱类别映射表中获取指定的体征名称
        /// </summary>
        /// <param name="originName">本系统定义的体征名称</param>
        /// <returns>映射后的体征名称</returns>
        public string GetOrderCatagrateTable(string originName)
        {
            if (GlobalMethods.Misc.IsEmptyString(originName)
                || this.m_orderCategoryTable == null
                || !this.m_orderCategoryTable.Contains(originName))
                return originName;
            object value = this.m_orderCategoryTable[originName];
            return GlobalMethods.Misc.IsEmptyString(value) ? originName : value.ToString();
        }

        /// <summary>
        /// 从白板脚本中配置的医嘱途径表中获取指定的简码
        /// </summary>
        /// <param name="originName">医嘱途径名称</param>
        /// <returns>映射后的医嘱途径简码</returns>
        public string GetOrderWayCode(string originName)
        {
            if (GlobalMethods.Misc.IsEmptyString(originName)
                || this.m_orderWayValueTable == null
                || !this.m_orderWayValueTable.Contains(originName))
                return originName;
            object value = this.m_orderWayValueTable[originName];
            return GlobalMethods.Misc.IsEmptyString(value) ? originName : value.ToString();
        }

        /// <summary>
        /// 从白板脚本中配置的体征值映射表中获取本系统的体征值映射
        /// </summary>
        /// <param name="originName">体征名称</param>
        /// <param name="originValue">原始值</param>
        /// <returns>映射后的值</returns>
        public string GetVitalSignsValue(string originName, string originValue)
        {
            string key = string.Format("{0}_{1}", originName, originValue);
            if (GlobalMethods.Misc.IsEmptyString(key)
                || this.m_vitalSignsValueTable == null
                || !this.m_vitalSignsValueTable.Contains(key))
                return originValue;
            object value = this.m_vitalSignsValueTable[key];
            return GlobalMethods.Misc.IsEmptyString(value) ? originValue : value.ToString();
        }
        #endregion

        #region"读写本地配置文件"
        /// <summary>
        /// 获取本地配置文件指定配置项的整型值
        /// </summary>
        /// <param name="key">配置项</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置整型值</returns>
        public int GetConfig(string key, int defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// 获取本地配置文件指定配置项的浮点值
        /// </summary>
        /// <param name="key">配置项</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置浮点值</returns>
        public float GetConfig(string key, float defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// 获取本地配置文件指定配置项的布尔值
        /// </summary>
        /// <param name="key">配置项</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置布尔值</returns>
        public bool GetConfig(string key, bool defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// 获取本地配置文件指定配置项的日期值
        /// </summary>
        /// <param name="key">配置项</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置日期值</returns>
        public DateTime GetConfig(string key, DateTime defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// 获取本地配置文件指定配置项的字符串值
        /// </summary>
        /// <param name="key">配置项</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置字符串值</returns>
        public string GetConfig(string key, string defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// 将指定的配置项及配置值保存到本地配置文件
        /// </summary>
        /// <param name="key">配置项</param>
        /// <param name="value">配置值</param>
        /// <returns>是否写入成功</returns>
        public bool WriteConfig(string key, string value)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Write(key, value);
        }

        /// <summary>
        /// 获取护理病历系统配置文件全路径
        /// </summary>
        public string ConfigFile
        {
            get
            {
                return string.Format(@"{0}\UserData\NurDocSys.xml", GlobalMethods.Misc.GetWorkingPath());
            }
        }
        #endregion

        private Dictionary<string, List<DeptInfo>> m_userDeptTable = null;

        /// <summary>
        /// 获取指定用户授权访问的所有科室的列表
        /// </summary>
        /// <param name="szUserID">用户ID号</param>
        /// <returns>授权用户访问的科室列表</returns>
        public List<DeptInfo> GetUserDeptList(string szUserID)
        {
            if (this.m_userDeptTable == null)
                this.m_userDeptTable = new Dictionary<string, List<DeptInfo>>();
            if (this.m_userDeptTable.ContainsKey(szUserID))
                return this.m_userDeptTable[szUserID];

            List<DeptInfo> lstDeptInfos = null;
            short shRet = AccountService.Instance.GetUserDept(szUserID, ref lstDeptInfos);
            if (shRet != SystemConst.ReturnValue.OK)
                return new List<DeptInfo>();

            if (lstDeptInfos == null)
                lstDeptInfos = new List<DeptInfo>();
            //this.m_userDeptTable.Add(szUserID, lstDeptInfos);
            return lstDeptInfos;
        }

        private ShiftRankInfo[] m_wardShiftRankList = null;

        /// <summary>
        /// 获取当前病区的交班班次列表
        /// </summary>
        /// <returns>是否成功</returns>
        public ShiftRankInfo[] GetWardShiftRankList()
        {
            if (SystemContext.Instance.LoginUser == null)
                return new ShiftRankInfo[0];

            if (this.m_wardShiftRankList != null)
                return this.m_wardShiftRankList;

            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            string szUserID = SystemContext.Instance.LoginUser.ID;
            List<DeptInfo> lstDeptInfos = SystemContext.Instance.GetUserDeptList(szUserID);
            List<ShiftRankInfo> lstShiftRankInfos = null;
            short shRet = NurShiftService.Instance.GetShiftRankInfos(szWardCode, lstDeptInfos, ref lstShiftRankInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("查询交班班次列表失败!");
                return new ShiftRankInfo[0];
            }
            if (lstShiftRankInfos == null)
                lstShiftRankInfos = new List<ShiftRankInfo>();
            int index = 0;
            bool bHasWardRank = false;
            bool bHasGroupRank = false;
            //判断是否有用户组交班配置
            while (index < lstShiftRankInfos.Count)
            {
                ShiftRankInfo shiftRankInfo = lstShiftRankInfos[index++];
                for (int index1 = 0; index1 <= lstDeptInfos.Count - 1; index1++)
                {
                    DeptInfo DeptInfo = lstDeptInfos[index1];
                    if (DeptInfo.DeptCode == shiftRankInfo.WardCode)
                    {
                        bHasGroupRank = true;
                        break;
                    }
                }
            }
            index = 0;
            //判断是否有病区交班配置
            while (index < lstShiftRankInfos.Count && !bHasGroupRank)
            {
                ShiftRankInfo shiftRankInfo = lstShiftRankInfos[index++];

                if (shiftRankInfo.WardCode == szWardCode && !bHasGroupRank)
                {
                    bHasWardRank = true;
                    break;
                }
            }
            index = 0;
            //交班配置筛选
            while (index < lstShiftRankInfos.Count)
            {
                if (!bHasWardRank && !bHasGroupRank)
                    break;
                ShiftRankInfo shiftRankInfo = lstShiftRankInfos[index];
                if (bHasGroupRank)
                {
                    bool bDelFlag = true;
                    for (int index1 = 0; index1 <= lstDeptInfos.Count - 1; index1++)
                    {
                        DeptInfo DeptInfo = lstDeptInfos[index1];
                        if (DeptInfo.DeptCode == shiftRankInfo.WardCode)
                            bDelFlag = false;
                    }
                    if (bDelFlag)
                        lstShiftRankInfos.Remove(shiftRankInfo);
                    else
                        index++;
                }
                if (bHasWardRank)
                {
                    if (shiftRankInfo.WardCode == szWardCode)
                        index++;
                    else
                        lstShiftRankInfos.Remove(shiftRankInfo);
                }
            }
            this.m_wardShiftRankList = lstShiftRankInfos.ToArray();
            return this.m_wardShiftRankList;
        }

        private CommonDictItem[] m_wardShiftItemList = null;

        /// <summary>
        /// 获取当前病区的交班项目列表
        /// </summary>
        /// <returns>是否成功</returns>
        public CommonDictItem[] GetWardShiftItemList()
        {
            if (SystemContext.Instance.LoginUser == null)
                return new CommonDictItem[0];

            if (this.m_wardShiftItemList != null)
                return this.m_wardShiftItemList;

            string szDictType = ServerData.CommonDictType.SHIFT_ITEM;
            string szWardCode = SystemContext.Instance.LoginUser.WardCode;
            List<CommonDictItem> lstShiftItemInfos = null;
            short shRet = CommonService.Instance.GetCommonDict(szDictType, szWardCode, ref lstShiftItemInfos);
            if (shRet != SystemConst.ReturnValue.OK)
            {
                MessageBoxEx.Show("查询交班项目列表失败!");
                return new CommonDictItem[0];
            }
            if (lstShiftItemInfos == null)
                lstShiftItemInfos = new List<CommonDictItem>();
            this.m_wardShiftItemList = lstShiftItemInfos.ToArray();
            return this.m_wardShiftItemList;
        }

        /// <summary>
        /// 判断时间点是否在标准版血压时间点下
        /// </summary>
        /// <param name="intTimePoint">时间点</param>
        /// <returns>boolean</returns>
        public bool IsTimeInStandardList(int intTimePoint)
        {
            foreach (int value in StandardBPTimePoint)
            {
                if (value == intTimePoint)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取护理计划自定义状态解释对应的状态
        /// </summary>
        /// <param name="szStatusDesc">状态解释</param>
        /// <returns>NurCarePlanConfig</returns>
        public NurCarePlanConfig GetConfigFromListByDesc(string szStatusDesc)
        {
            for (int index = 0; index < this.m_NCPButtonConfigTable.Count; index++)
            {
                if (m_NCPButtonConfigTable[index].StatusDesc == szStatusDesc)
                    return m_NCPButtonConfigTable[index];
            }
            return null;
        }

        /// <summary>
        /// 护理计划中根据状态获取状态注释
        /// </summary>
        /// <param name="szStatus">状态</param>
        /// <returns>NurCarePlanConfig</returns>
        public NurCarePlanConfig GetConfigFromListByStatus(string szStatus)
        {
            for (int index = 0; index < this.m_NCPButtonConfigTable.Count; index++)
            {
                if (m_NCPButtonConfigTable[index].Status == szStatus)
                    return m_NCPButtonConfigTable[index];
            }
            return null;
        }

        public List<NurCarePlanConfig> GetNursCarPlanConfigs(DataTable dtConfig)
        {
            if (dtConfig == null)
                return null;
            List<NurCarePlanConfig> lstNursCarePlanConfig = new List<NurCarePlanConfig>();
            foreach (DataRow dr in dtConfig.Rows)
            {
                NurCarePlanConfig nurCarePlanConfig = new NurCarePlanConfig();
                nurCarePlanConfig.ButtonName = dr["ButtonName"].ToString();
                nurCarePlanConfig.Status = dr["Status"].ToString();
                nurCarePlanConfig.StatusDesc = dr["StatusDesc"].ToString();
                nurCarePlanConfig.IsNeedGetIn = dr["IsNeedGetIn"].ToString() == "True" ? true : false;
                nurCarePlanConfig.BtnHandler = dr["BtnHandler"].ToString().ToCharArray();
                lstNursCarePlanConfig.Add(nurCarePlanConfig);
            }
            return lstNursCarePlanConfig;
        }

        private DataTable m_DeptTable = null;

        /// <summary>
        /// 获取或设置当前需要显示在排班一览表的科室信息
        /// </summary>
        public DataTable DeptTable
        {
            get
            {
                return this.m_DeptTable;
            }

            set
            {
                this.m_DeptTable = value;
            }
        }

        /// <summary>
        /// 刷新当前需要显示在排班一览表的科室信息
        /// </summary>
        /// <param name="deptList">科室列表数据</param>
        public void RefreshDeptTableInfo(DataTable deptList)
        {
            this.m_DeptTable = deptList;
        }
    }
}
