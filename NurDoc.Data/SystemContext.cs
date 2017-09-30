// ***********************************************************
// ������Ӳ���ϵͳ,ϵͳ���������ݻ�����.
// ���ڻ��浱ǰ��½�û��������Ϣ,�Լ�ȫ�ֵ�һЩ������Ϣ
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
        /// ��ȡϵͳ����������ʵ��
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
        /// ��ȡ�����õ�¼ϵͳ���û���Ϣ
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
        /// ��ȡ�����õ�ǰ��ʾ�Ŀ�����Ϣ
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
        /// ��ȡ��������������ʵ��
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
        /// ��ȡ���÷���������ʵ��
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
        /// ��ȡEMR���ݿ������ʵ��
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
        /// ��ȡ�û�Ȩ�޷�����ʵ��
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
        /// ��ȡģ�������ʵ��
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
        /// ��ȡģ�������ʵ��
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
        /// ��ȡwordģ�������ʵ��
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
        /// ��ȡģ�������ʵ��
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
        /// ��ȡ�������������ʵ��
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
        /// ��ȡ�ʼ���Ϣ������ʵ��
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
        /// ��ȡ����ƻ�������ʵ��
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
        /// ��ȡ�ĵ�������ʵ��
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
        /// ��ȡ�����밲ȫ�����¼������ʵ��
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
        /// ��ȡ�����¼���ݷ�����ʵ��
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
        /// ��ȡ��ʿ�������ݷ�����ʵ��
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
        /// ��ȡ��ʿ�������ݷ�����ʵ��
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
        /// ��ȡ������¼���ݷ�����ʵ��
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
        /// ��ȡ�����������ݷ�����ʵ��
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
        /// ��ȡ�����������ͷ�����ʵ��
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
        /// ��ȡ�ĵ�������ʵ��
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
        /// ��ȡʳ��ɷַ�����ʵ��
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
        /// ��ȡ�����밲ȫ�����¼���ͷ�����ʵ��
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
        /// ��ȡ�����������õ�ϵͳѡ��
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
        /// ��ȡ�����������õĴ�����������
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
        /// ��ǰϵͳ����
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
        /// ��ϵͳ�û���Ϣ�л��ɹ��󴥷�
        /// </summary>
        [Description("��ϵͳ�û���Ϣ�л��ɹ��󴥷�")]
        public event UserChangedEventHandler UserChanged;

        /// <summary>
        /// ����ǰ��ʾ�Ŀ����л��ɹ��󴥷�
        /// </summary>
        [Description("����ǰ��ʾ�Ŀ����л��ɹ��󴥷�")]
        public event EventHandler DisplayDeptChanged;

        /// <summary>
        /// ����ǰ��ʾ�����ݱ仯�󴥷�
        /// </summary>
        [Description("����ǰ��ʾ�����ݱ仯�󴥷�")]
        public event EventHandler DataChanged;

        /// <summary>
        /// ����ϵͳģ������ˢ�³ɹ��¼�
        /// </summary>
        /// <param name="sender">��������</param>
        /// <param name="e">�¼�����</param>
        public void OnDataChanged(object sender, EventArgs e)
        {
            if (this.DataChanged != null)
                this.DataChanged(sender, e);
        }

        /// <summary>
        /// ����ϵͳ�û���Ϣ�л��ɹ��¼�
        /// </summary>
        /// <param name="sender">��������</param>
        /// <param name="e">�¼�����</param>
        public void OnUserChanged(object sender, UserChangedEventArgs e)
        {
            if (this.UserChanged != null)
                this.UserChanged(sender, e);
        }

        /// <summary>
        /// ����ϵͳ�û������л��ɹ��¼�
        /// </summary>
        /// <param name="sender">��������</param>
        /// <param name="e">�¼�����</param>
        public void OnDisplayDeptChanged(object sender, EventArgs e)
        {
            if (this.DisplayDeptChanged != null)
                this.DisplayDeptChanged(sender, e);
        }

        public bool GetSystemContext(string name, ref object value)
        {
            #region"ϵͳ����������"
            if (name == "�û�ID��" || name == "�û����")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.ID;
                return true;
            }
            if (name == "�û�����")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.Name;
                return true;
            }
            if (name == "�û�����")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.Password;
                return true;
            }
            if (name == "�û����Ҵ���")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.DeptCode;
                return true;
            }
            if (name == "�û���������")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.DeptName;
                return true;
            }
            if (name == "�û���������")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.WardCode;
                return true;
            }
            if (name == "�û���������")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = SystemContext.Instance.LoginUser.WardName;
                return true;
            }
            if (name == "����ID��" || name == "���˱��")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.PatientId;
                return true;
            }
            if (name == "��������")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.PatientName;
                return true;
            }
            if (name == "�����Ա�")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.PatientSex;
                return true;
            }
            if (name == "���˻���")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.MaritalStatus;
                return true;
            }
            if (name == "��������")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.BirthTime;
                return true;
            }
            if (name == "��������")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = GlobalMethods.SysTime.GetAgeText(
                    PatientTable.Instance.ActivePatient.BirthTime,
                    PatientTable.Instance.ActivePatient.VisitTime);
                return true;
            }
            //if (name == "��������")
            //{
            //    if (PatientTable.Instance.ActivePatient == null)
            //        return false;
            //    value = PatientTable.Instance.ActivePatient.Nation;
            //    return true;
            //}
            if (name == "���˲���")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.PatientCondition;
                return true;
            }
            if (name == "����ȼ�")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.NursingClass;
                return true;
            }
            if (name == "��Ժ��" || name == "�����")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.VisitId;
                return true;
            }
            if (name == "��Ժʱ��")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.VisitTime;
                return true;
            }
            if (name == "������Ҵ���")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.DeptCode;
                return true;
            }
            if (name == "�����������")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.DeptName;
                return true;
            }
            if (name == "���ﲡ������")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.WardCode;
                return true;
            }
            if (name == "���ﲡ������")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.WardName;
                return true;
            }
            if (name == "��������")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.VisitType;
                return true;
            }
            if (name == "����")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.BedCode;
                return true;
            }
            if (name == "סԺ��")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.InpNo;
                return true;
            }
            if (name == "���")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.Diagnosis;
                return true;
            }
            if (name == "����ҩ��")
            {
                if (PatientTable.Instance.ActivePatient == null)
                    return false;
                value = PatientTable.Instance.ActivePatient.AllergyDrugs;
                return true;
            }
            if (name == "��ǰʱ��")
            {
                value = SysTimeService.Instance.Now;
                return true;
            }
            if (name == "��ǰ���������б�"
                || name == "��ǰ���Ҳ����б�")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = PatientTable.Instance.DeptPatientTable;
                return true;
            }
            if (name == "��ǰ��ʾ�����б�")
            {
                if (SystemContext.Instance.LoginUser == null)
                    return false;
                value = PatientTable.Instance.DisplayPatientTable;
                return true;
            }
            if (name == "��ǰ���Ҵ���")
            {
                if (SystemContext.Instance.DisplayDept == null)
                    return false;
                value = SystemContext.Instance.DisplayDept.DeptCode;
                return true;
            }
            if (name == "�û�������")
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
            if (name == "��ǰ��������")
            {
                if (SystemContext.Instance.DisplayDept == null)
                    return false;
                value = SystemContext.Instance.DisplayDept.DeptName;
                return true;
            }
            if (name == "������ͼ")
            {
                DataTable dtNursingAssessInfo = new DataTable();
                DataServices.WardPatientsService.Instance.GetWardPatientsNursingAssess(PatientTable.Instance.DisplayPatientTable, ref dtNursingAssessInfo);
                value = dtNursingAssessInfo;
                return true;
            }
            if (name == "��������������")
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
            if (!string.IsNullOrEmpty(name) && name.StartsWith("�û�Ȩ��_"))
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
                    string szRightName = "�û�Ȩ��_" + rightInfo.Name;
                    if (string.Compare(szRightName, name, false) == 0)
                    {
                        value = (rightInfo != null && rightInfo.Value);
                        return true;
                    }
                }
            }
            if (!string.IsNullOrEmpty(name) && name.StartsWith("��������_"))
            {
                int index = name.IndexOf("_");
                if (index < name.Length - 1)
                {
                    value = this.GetConfig(name.Substring(index + 1), string.Empty);
                    return true;
                }
            }
            if (!string.IsNullOrEmpty(name) && name.StartsWith("ƴ������ĸ_"))
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

        #region"�ڰװ������õ���"
        private Hashtable m_timePointTable = null;

        /// <summary>
        /// ��ȡ����������ʱ����б�
        /// </summary>
        public Hashtable TimePointTable
        {
            get
            {
                if (this.m_timePointTable == null)
                {
                    this.m_timePointTable = new Hashtable();
                    this.m_timePointTable.Add("ͨ��ɸѡ����", new int[] { 2, 6, 10, 14, 18, 22 });
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
        /// ��ȡ�����ñ�׼��Ѫѹʱ���
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
        /// ��ȡ������ȫ�����������б�
        /// </summary>
        public string[] TempTypeList
        {
            get
            {
                if (this.m_tempTypeList == null)
                    return new string[] { "Ҹ��", "����", "����", "����" };
                return this.m_tempTypeList;
            }

            set
            {
                this.m_tempTypeList = value;
            }
        }

        private Dictionary<string, Color> m_patientCardColorList = null;

        /// <summary>
        /// ��ȡ�����ô�����ɫ��ɫ�б�
        /// </summary>
        public Dictionary<string, Color> PatientCardColorList
        {
            get
            {
                if (this.m_patientCardColorList == null || this.m_patientCardColorList.Count <= 0)
                {
                    m_patientCardColorList = new Dictionary<string, Color>();
                    m_patientCardColorList.Add("���������ɫ��", Color.WhiteSmoke);
                    m_patientCardColorList.Add("���������ɫ��", Color.LightSteelBlue);
                    m_patientCardColorList.Add("�����ͣ��ɫ��", Color.White);
                    m_patientCardColorList.Add("�����ͣ��ɫ��", Color.MediumOrchid);
                    m_patientCardColorList.Add("���ѡ����ɫ��", Color.MediumOrchid);
                    m_patientCardColorList.Add("���ѡ����ɫ��", Color.White);
                }

                return this.m_patientCardColorList;
            }

            set
            {
                this.m_patientCardColorList = value;
            }
        }

        /// <summary>
        /// �ж�ָ���������������Ƿ�����������
        /// </summary>
        /// <param name="szTypeName">������������</param>
        /// <returns>�Ƿ�����������</returns>
        public bool IsBodyTemperatureType(string szTypeName)
        {
            if (szTypeName == "����")
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
        /// ��ȡ������ȫ��Һ��ҽ�������б�
        /// </summary>
        public string[] LiquidOrderList
        {
            get
            {
                if (this.m_liquidOrderList == null)
                    return new string[] { "����" };
                return this.m_liquidOrderList;
            }

            set
            {
                this.m_liquidOrderList = value;
            }
        }

        private string m_VitalSignNoTimePoint = null;

        /// <summary>
        /// �������ݴ��벻��ʱ���
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
        /// ¼������ʱ���һ��
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
        /// �ж�ָ��ҽ�������Ƿ���Һ��
        /// </summary>
        /// <param name="szOrderAdm">ҽ��;��</param>
        /// <returns>�Ƿ���Һ��ҽ��</returns>
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
        /// ��ȡ�����û����¼С�������б�
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
        /// ��ȡ�����ý��Ӱ���Ŀ����˳��
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
        /// ��ȡ�����ðװ�ű������õ��������Ʊ�
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
        /// ��ȡ�����ðװ�ű������õ�����ֵӳ���
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
        /// ��ȡ�����ø��Ի�����ƻ���ť��������
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
        /// ��ȡ�����ðװ�ű������õ�ҽ��;��ӳ���
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
        /// ��ȡ�����ðװ�ű������õ�ҽ�����ӳ���
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

        /// �Ӱװ�ű������õ��������Ʊ��л�ȡָ������������
        /// </summary>
        /// <param name="originName">��ϵͳ�������������</param>
        /// <returns>ӳ������������</returns>
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
        /// �Ӱװ�ű������õ�ҽ�����ӳ����л�ȡָ������������
        /// </summary>
        /// <param name="originName">��ϵͳ�������������</param>
        /// <returns>ӳ������������</returns>
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
        /// �Ӱװ�ű������õ�ҽ��;�����л�ȡָ���ļ���
        /// </summary>
        /// <param name="originName">ҽ��;������</param>
        /// <returns>ӳ����ҽ��;������</returns>
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
        /// �Ӱװ�ű������õ�����ֵӳ����л�ȡ��ϵͳ������ֵӳ��
        /// </summary>
        /// <param name="originName">��������</param>
        /// <param name="originValue">ԭʼֵ</param>
        /// <returns>ӳ����ֵ</returns>
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

        #region"��д���������ļ�"
        /// <summary>
        /// ��ȡ���������ļ�ָ�������������ֵ
        /// </summary>
        /// <param name="key">������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>��������ֵ</returns>
        public int GetConfig(string key, int defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// ��ȡ���������ļ�ָ��������ĸ���ֵ
        /// </summary>
        /// <param name="key">������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>���ø���ֵ</returns>
        public float GetConfig(string key, float defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// ��ȡ���������ļ�ָ��������Ĳ���ֵ
        /// </summary>
        /// <param name="key">������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>���ò���ֵ</returns>
        public bool GetConfig(string key, bool defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// ��ȡ���������ļ�ָ�������������ֵ
        /// </summary>
        /// <param name="key">������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>��������ֵ</returns>
        public DateTime GetConfig(string key, DateTime defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// ��ȡ���������ļ�ָ����������ַ���ֵ
        /// </summary>
        /// <param name="key">������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>�����ַ���ֵ</returns>
        public string GetConfig(string key, string defaultValue)
        {
            if (SystemContext.Instance.LoginUser == null)
                return defaultValue;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Get(key, defaultValue);
        }

        /// <summary>
        /// ��ָ�������������ֵ���浽���������ļ�
        /// </summary>
        /// <param name="key">������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>�Ƿ�д��ɹ�</returns>
        public bool WriteConfig(string key, string value)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            key = string.Format("{0}.{1}", key, SystemContext.Instance.LoginUser.ID);
            return SystemConfig.Instance.Write(key, value);
        }

        /// <summary>
        /// ��ȡ������ϵͳ�����ļ�ȫ·��
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
        /// ��ȡָ���û���Ȩ���ʵ����п��ҵ��б�
        /// </summary>
        /// <param name="szUserID">�û�ID��</param>
        /// <returns>��Ȩ�û����ʵĿ����б�</returns>
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
        /// ��ȡ��ǰ�����Ľ������б�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
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
                MessageBoxEx.Show("��ѯ�������б�ʧ��!");
                return new ShiftRankInfo[0];
            }
            if (lstShiftRankInfos == null)
                lstShiftRankInfos = new List<ShiftRankInfo>();
            int index = 0;
            bool bHasWardRank = false;
            bool bHasGroupRank = false;
            //�ж��Ƿ����û��齻������
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
            //�ж��Ƿ��в�����������
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
            //��������ɸѡ
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
        /// ��ȡ��ǰ�����Ľ�����Ŀ�б�
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
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
                MessageBoxEx.Show("��ѯ������Ŀ�б�ʧ��!");
                return new CommonDictItem[0];
            }
            if (lstShiftItemInfos == null)
                lstShiftItemInfos = new List<CommonDictItem>();
            this.m_wardShiftItemList = lstShiftItemInfos.ToArray();
            return this.m_wardShiftItemList;
        }

        /// <summary>
        /// �ж�ʱ����Ƿ��ڱ�׼��Ѫѹʱ�����
        /// </summary>
        /// <param name="intTimePoint">ʱ���</param>
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
        /// ��ȡ����ƻ��Զ���״̬���Ͷ�Ӧ��״̬
        /// </summary>
        /// <param name="szStatusDesc">״̬����</param>
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
        /// ����ƻ��и���״̬��ȡ״̬ע��
        /// </summary>
        /// <param name="szStatus">״̬</param>
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
        /// ��ȡ�����õ�ǰ��Ҫ��ʾ���Ű�һ����Ŀ�����Ϣ
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
        /// ˢ�µ�ǰ��Ҫ��ʾ���Ű�һ����Ŀ�����Ϣ
        /// </summary>
        /// <param name="deptList">�����б�����</param>
        public void RefreshDeptTableInfo(DataTable deptList)
        {
            this.m_DeptTable = deptList;
        }
    }
}
