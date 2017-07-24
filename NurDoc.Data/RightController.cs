// ***********************************************************
// 用户权限控制器.用于判断当前登录用户是否有权限使用指定功能.
// Creator:YangMingkun  Date:2012-10-31
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class RightController
    {
        private static RightController m_instance = null;

        /// <summary>
        /// 获取权限控制器对象实例
        /// </summary>
        public static RightController Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new RightController();
                return m_instance;
            }
        }

        private RightController()
        {
        }

        private RightConfig m_rightConfig = null;

        /// <summary>
        /// 获取服务器端配置的用户权限配置
        /// </summary>
        public RightConfig RightConfig
        {
            get
            {
                if (SystemContext.Instance.LoginUser == null)
                    return new RightConfig();
                if (this.m_rightConfig != null)
                    return this.m_rightConfig;

                ConfigService.Instance.GetRightConfig(ref this.m_rightConfig);
                if (this.m_rightConfig == null)
                    return new RightConfig();
                return this.m_rightConfig;
            }
        }

        private NurUserRight m_userRight = null;

        /// <summary>
        /// 获取当前护理病历系统用户的权限信息(该对象不会为空)
        /// </summary>
        public NurUserRight UserRight
        {
            set
            {
                this.m_userRight = value;
            }

            get
            {
                if (SystemContext.Instance.LoginUser == null)
                    return new NurUserRight();

                string szUserID = SystemContext.Instance.LoginUser.ID;
                if (this.m_userRight != null
                    && this.m_userRight.UserID == szUserID)
                {
                    return this.m_userRight;
                }

                UserRightType rightType = UserRightType.NurDoc;
                UserRightBase userRight = null;
                AccountService.Instance.GetUserRight(szUserID, rightType, ref userRight);
                this.m_userRight = userRight as NurUserRight;
                return (this.m_userRight == null) ? new NurUserRight() : this.m_userRight;
            }
        }

        /// <summary>
        /// 检查当前用户是否有指定的病区的访问权限
        /// </summary>
        /// <param name="szWardCode">病区代码</param>
        /// <returns>是否有访问权限</returns>
        private bool IsAccessibleWard(string szWardCode)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            if (szWardCode == SystemContext.Instance.LoginUser.WardCode)
                return true;

            string szUserID = SystemContext.Instance.LoginUser.ID;
            List<DeptInfo> lstDeptInfos = SystemContext.Instance.GetUserDeptList(szUserID);
            foreach (DeptInfo deptInfo in lstDeptInfos)
            {
                if (szWardCode == deptInfo.DeptCode)
                    return true;
            }
            return false;
        }

        #region"体征记录权限控制"

        /// <summary>
        /// 判断当前登录用户是否能够编辑当前病人的体征记录
        /// </summary>
        /// <returns>true-能创建;false-不能创建</returns>
        public bool CanEditVitalSigns()
        {
            return this.CanEditVitalSigns(PatientTable.Instance.ActivePatient);
        }

        /// <summary>
        /// 判断当前登录用户是否能够编辑指定病人的体征记录
        /// </summary>
        /// <param name="patVisit">病人信息</param>
        /// <returns>true-能创建;false-不能创建</returns>
        public bool CanEditVitalSigns(PatVisitInfo patVisit)
        {
            if (SystemContext.Instance.LoginUser == null || patVisit == null)
                return false;

            if (!this.UserRight.EditVitalSigns.Value)
            {
                MessageBoxEx.ShowWarning("您没有权限编辑体征记录!");
                return false;
            }
            if (patVisit.WardCode != SystemContext.Instance.LoginUser.WardCode)
            {
                if (!this.UserRight.EditAllVitalSigns.Value)
                {
                    MessageBoxEx.ShowWarning("您没有权限编辑其他病区的体征记录!");
                    return false;
                }
            }

            //出院病人,需要检查出院时间
            if (patVisit.DischargeTime != patVisit.DefaultTime)
            {
                return this.IsRightEnabled("VITAL_SIGNS", "体征记录", string.Empty, patVisit.DischargeTime, "编辑");
            }
            return true;
        }
        #endregion

        #region"护理记录权限控制"
        /// <summary>
        /// 判断当前登录用户是否能够创建当前病人的护理记录
        /// </summary>
        /// <returns>true-能创建;false-不能创建</returns>
        public bool CanCreateNurRec()
        {
            return this.CanCreateNurRec(PatientTable.Instance.ActivePatient);
        }

        /// <summary>
        /// 判断当前登录用户是否能够创建指定病人的护理记录
        /// </summary>
        /// <param name="patVisit">病人信息</param>
        /// <returns>true-能创建;false-不能创建</returns>
        public bool CanCreateNurRec(PatVisitInfo patVisit)
        {
            if (SystemContext.Instance.LoginUser == null || patVisit == null)
                return false;

            if (!this.UserRight.CreateNuringRec.Value)
            {
                MessageBoxEx.ShowWarning("您没有权限创建护理记录!");
                return false;
            }
            if (patVisit.WardCode != SystemContext.Instance.LoginUser.WardCode)
            {
                if (this.UserRight.LeaderNurse.Value)
                {
                    return true;
                }
                else if (this.UserRight.EditWardNuringRec.Value)
                {
                    if (!this.IsAccessibleWard(patVisit.WardCode))
                    {
                        MessageBoxEx.ShowWarning("您没有权限编辑其他病区的护理记录!");
                        return false;
                    }
                }
                else if (!this.UserRight.EditAllNuringRec.Value)
                {
                    MessageBoxEx.ShowWarning("您没有权限编辑其他病区的护理记录!");
                    return false;
                }
            }

            //出院病人,需要检查出院时间
            if (patVisit.DischargeTime != patVisit.DefaultTime)
            {
                return this.IsRightEnabled("NUR_REC", "护理记录", string.Empty, patVisit.DischargeTime, "新建");
            }
            return true;
        }

        /// <summary>
        /// 判断当前登录用户是否能够修改指定的护理记录
        /// </summary>
        /// <param name="recInfo">护理记录信息</param>
        /// <returns>true-能修改;false-不能修改</returns>
        public bool CanEditNurRec(NursingRecInfo recInfo)
        {
            if (SystemContext.Instance.LoginUser == null || recInfo == null)
                return false;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == recInfo.CreatorID)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditAllNuringRec.Value)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditWardNuringRec.Value
                && PatientTable.Instance.ActivePatient.WardCode == SystemContext.Instance.LoginUser.WardCode)
            {
                bEditEnabled = this.IsAccessibleWard(recInfo.WardCode);
            }
            if (!this.UserRight.EditNuringRec.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("您没有权限编辑当前护理记录!");
                return false;
            }
            return this.IsRightEnabled("NUR_REC", "护理记录", recInfo.RecordID, recInfo.CreateTime, "编辑");
        }

        /// <summary>
        /// 判断当前登录用户是否能够删除指定的护理记录
        /// </summary>
        /// <param name="recInfo">护理记录信息</param>
        /// <returns>true-能删除;false-不能删除</returns>
        public bool CanDeleteNurRec(NursingRecInfo recInfo)
        {
            if (SystemContext.Instance.LoginUser == null || recInfo == null)
                return false;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == recInfo.CreatorID)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditAllNuringRec.Value)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditWardNuringRec.Value
                && PatientTable.Instance.ActivePatient.WardCode == SystemContext.Instance.LoginUser.WardCode)
            {
                bEditEnabled = this.IsAccessibleWard(recInfo.WardCode);
            }
            if (!this.UserRight.DeleteNuringRec.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("您没有权限删除当前护理记录!");
                return false;
            }
            return this.IsRightEnabled("NUR_REC", "护理记录", recInfo.RecordID, recInfo.CreateTime, "删除");
        }
        #endregion

        #region"护理文书权限控制"
        /// <summary>
        /// 判断用户是否有打开该类型文档的权限
        /// </summary>
        /// <param name="htWardDocTable">文档类型所属病区时使用的Hash表</param>
        /// <param name="szDocTypeID">文档类型ID号</param>
        /// <returns>bool</returns>
        public bool CanOpenNurDoc(Dictionary<string, WardDocType> htWardDocTable, string szDocTypeID)
        {
            string szUserID = SystemContext.Instance.LoginUser.ID;
            List<DeptInfo> lstDeptInfos = SystemContext.Instance.GetUserDeptList(szUserID);
            foreach (DeptInfo deptInfo in lstDeptInfos)
            {
                string szWardDocType = string.Concat(szDocTypeID, "_", deptInfo.DeptCode);
                if (htWardDocTable.ContainsKey(szWardDocType))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断当前登录用户是否能够创建当前病人的护理文书
        /// </summary>
        /// <returns>true-能创建;false-不能创建</returns>
        public bool CanCreateNurDoc()
        {
            return this.CanCreateNurDoc(PatientTable.Instance.ActivePatient);
        }

        /// <summary>
        /// 判断当前登录用户是否能够创建指定病人的护理文书
        /// </summary>
        /// <param name="patVisit">病人信息</param>
        /// <returns>true-能创建;false-不能创建</returns>
        public bool CanCreateNurDoc(PatVisitInfo patVisit)
        {
            if (SystemContext.Instance.LoginUser == null || patVisit == null)
                return false;

            if (!this.UserRight.CreateNuringDoc.Value)
            {
                MessageBoxEx.ShowWarning("您没有权限创建护理文书!");
                return false;
            }
            if (patVisit.WardCode != SystemContext.Instance.LoginUser.WardCode)
            {
                if (this.UserRight.LeaderNurse.Value)
                {
                    return true;
                }
                else if (this.UserRight.EditWardNuringRec.Value)
                {
                    if (!this.IsAccessibleWard(patVisit.WardCode))
                        return false;
                }
                else if (!this.UserRight.EditAllNuringDoc.Value)
                {
                    MessageBoxEx.ShowWarning("您没有权限编辑其他病区的护理文书!");
                    return false;
                }
            }

            //出院病人,需要检查出院时间
            if (patVisit.DischargeTime != patVisit.DefaultTime)
            {
                return this.IsRightEnabled("NUR_DOC", "护理文书", string.Empty, patVisit.DischargeTime, "新建");
            }
            return true;
        }

        /// <summary>
        /// 判断当前登录用户是否能够修改指定的护理文书
        /// </summary>
        /// <param name="docInfo">护理文书信息</param>
        /// <returns>true-能修改;false-不能修改</returns>
        public bool CanEditNurDoc(NurDocInfo docInfo)
        {
            if (SystemContext.Instance.LoginUser == null || docInfo == null)
                return false;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == docInfo.CreatorID)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditAllNuringDoc.Value)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditWardNuringDoc.Value
                && PatientTable.Instance.ActivePatient.WardCode == SystemContext.Instance.LoginUser.WardCode)
            {
                bEditEnabled = this.IsAccessibleWard(docInfo.WardCode);
            }
            if (!this.UserRight.EditNuringDoc.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("您没有权限编辑当前护理文书!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断当前登录用户是否能够删除指定的护理文书
        /// </summary>
        /// <param name="docInfo">护理文书信息</param>
        /// <returns>true-能删除;false-不能删除</returns>
        public bool CanDeleteNurDoc(NurDocInfo docInfo)
        {
            if (SystemContext.Instance.LoginUser == null || docInfo == null)
                return false;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == docInfo.CreatorID)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditAllNuringDoc.Value)
            {
                bEditEnabled = true;
            }
            else if (this.UserRight.EditWardNuringDoc.Value
                && PatientTable.Instance.ActivePatient.WardCode == SystemContext.Instance.LoginUser.WardCode)
            {
                bEditEnabled = this.IsAccessibleWard(docInfo.WardCode);
            }
            if (!this.UserRight.DeleteNuringDoc.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("您没有权限删除当前护理文书!");
                return false;
            }
            return this.IsRightEnabled("NUR_DOC", "护理文书", docInfo.DocID, docInfo.DocTime, "删除");
        }

        #endregion

        #region"护理交班权限控制"
        /// <summary>
        /// 判断当前登录用户是否能够创建新的交班记录
        /// </summary>
        /// <returns>true-能创建;false-不能创建</returns>
        public bool CanCreateShiftRec()
        {
            if (!this.CanEditShiftRec(null))
            {
                MessageBoxEx.ShowWarning("您没有权限创建交班记录!");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 判断当前登录用户是否能够设置交班项目配置别名
        /// </summary>
        /// <returns>true-能设置;false-不能设置</returns>
        public bool CanSetItemAlias()
        {
            if (!this.CanEditItemAlias(null))
            {
                MessageBoxEx.ShowWarning("您没有权限设置项目别名!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断当前登录用户是否能够修改指定的交班记录
        /// </summary>
        /// <param name="shiftInfo">护理记录信息</param>
        /// <returns>true-能修改;false-不能修改</returns>
        public bool CanEditShiftRec(NursingShiftInfo shiftInfo)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;

            if (shiftInfo == null)
                return this.UserRight.EditShiftRec.Value;

            bool bEditEnabled = false;
            if (SystemContext.Instance.LoginUser.ID == shiftInfo.CreatorID
                || this.IsAccessibleWard(shiftInfo.WardCode))
            {
                bEditEnabled = true;
            }
            if (!this.UserRight.EditShiftRec.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("您没有权限编辑当前交班记录!");
                return false;
            }
            return this.IsRightEnabled("SHIFT_REC", "交班记录", shiftInfo.ShiftRecordID, shiftInfo.CreateTime, "编辑");
        }

        /// <summary>
        /// 判断当前登录用户是否能够修改指定的交班项目配置别名
        /// </summary>
        /// <param name="shiftInfo">护理记录信息</param>
        /// <returns>true-能修改;false-不能修改</returns>
        public bool CanEditItemAlias(string szWardCode)
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;

            if (szWardCode == null)
                return this.UserRight.HigherNurse.Value;

            bool bEditEnabled = false;
            if (this.IsAccessibleWard(szWardCode))
            {
                bEditEnabled = true;
            }
            if (!this.UserRight.HigherNurse.Value)
            {
                bEditEnabled = false;
            }
            if (!bEditEnabled)
            {
                MessageBoxEx.ShowWarning("您没有权限编辑当前交班项目配置别名!");
                return false;
            }
            return this.IsRightEnabled("SHIFT_ITEMALIAS", "交班项目配置别名", null, DateTime.Now, "编辑");
        }
        #endregion

        /// <summary>
        /// 弹出权限点不可用的时候的消息框
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="hours">可修改的小时数</param>
        /// <param name="moduleName">模块名称</param>
        private void ShowRightInvalidMessage(string operationType, int hours, string moduleName)
        {
            int timeSpan = hours;
            string spanName = "小时";
            if (hours > 72)
            {
                timeSpan = hours / 24;
                spanName = "天";
            }
            MessageBoxEx.ShowWarningFormat("您没有权限{0}{1}{2}以前的{3}!", null, operationType
                , timeSpan, spanName, moduleName);
        }

        /// <summary>
        /// 判断指定的数据操作权限点是否可用
        /// </summary>
        /// <param name="moduleCode">模块代码</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="assignData">关联数据</param>
        /// <param name="dataTime">数据时间</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>数据操作权限点是否可用</returns>
        private bool IsRightEnabled(string moduleCode, string moduleName
            , string assignData, DateTime dataTime, string operationType)
        {
            DateTime dtNow = SysTimeService.Instance.Now;
            long span = GlobalMethods.SysTime.DateDiff(DateInterval.Hour, dataTime, dtNow);

            if (this.UserRight.LeaderNurse.Value)
            {
                if (this.RightConfig.LeaderNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.LeaderNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.LeaderNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.HeadNurse.Value)
            {
                if (this.RightConfig.HeadNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.HeadNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.HeadNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.HigherNurse.Value)
            {
                if (this.RightConfig.HigherNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.HigherNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.HigherNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.QualityNurse.Value)
            {
                if (this.RightConfig.QualityNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.QualityNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.QualityNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.GeneralNurse.Value)
            {
                if (this.RightConfig.GeneralNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.GeneralNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.GeneralNurseEditTime, moduleName);
                    return false;
                }
            }
            else if (this.UserRight.StudentNurse.Value)
            {
                if (this.RightConfig.StudentNurseEditTime <= 0)
                    return true;
                if (span > this.RightConfig.StudentNurseEditTime)
                {
                    this.ShowRightInvalidMessage(operationType, this.RightConfig.StudentNurseEditTime, moduleName);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断是否允许显示另存为按钮
        /// </summary>
        /// <returns></returns>
        public bool CanShowSaveAsButton()
        {
            if (SystemContext.Instance.LoginUser == null)
                return false;
            return this.UserRight.ShowSaveAsButton.Value;
        }
    }
}
