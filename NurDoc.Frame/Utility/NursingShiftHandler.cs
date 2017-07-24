using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Frame
{
    internal class NursingShiftHandler
    {
        private static NursingShiftHandler m_instance = null;

        /// <summary>
        /// »ñÈ¡ÊµÀý
        /// </summary>
        public static NursingShiftHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new NursingShiftHandler();
                return m_instance;
            }
        }

        public NursingShiftInfo Create(UserInfo userInfo, DateTime shiftDate, DateTime createTime, ShiftRankInfo shiftRankInfo, int iSRSelectedIndex)
        {
            NursingShiftInfo nursingShiftInfo = new NursingShiftInfo();
            nursingShiftInfo.ShiftRecordDate = shiftDate.Date;
            nursingShiftInfo.ShiftRecordTime = new DateTime(shiftDate.Year, shiftDate.Month, shiftDate.Day, createTime.Hour, createTime.Minute, createTime.Second);
            nursingShiftInfo.ShiftRankCode = shiftRankInfo.RankCode;
            nursingShiftInfo.CreatorID = userInfo.ID;
            nursingShiftInfo.CreatorName = userInfo.Name;
            nursingShiftInfo.CreateTime = createTime;
            nursingShiftInfo.ModifierID = userInfo.ID;
            nursingShiftInfo.ModifierName = userInfo.Name;
            nursingShiftInfo.ModifyTime = createTime;
            nursingShiftInfo.WardCode = userInfo.WardCode;
            nursingShiftInfo.WardName = userInfo.WardName;
            nursingShiftInfo.FirstSignID = nursingShiftInfo.CreatorID;
            nursingShiftInfo.FirstSignName = nursingShiftInfo.CreatorName;
            nursingShiftInfo.FirstSignTime = nursingShiftInfo.CreateTime;
            nursingShiftInfo.ShiftRecordID = nursingShiftInfo.MakeRecordID(shiftDate, iSRSelectedIndex);
            return nursingShiftInfo;
        }

        public SpecialShiftInfo Create(UserInfo userInfo, DateTime shiftDate, DateTime createTime)
        {
            SpecialShiftInfo specialShiftInfo = new SpecialShiftInfo();
            specialShiftInfo.ShiftRecordDate = shiftDate.Date;
            specialShiftInfo.ShiftRecordTime = new DateTime(shiftDate.Year, shiftDate.Month, shiftDate.Day, createTime.Hour, createTime.Minute, createTime.Second);
            specialShiftInfo.CreatorID = userInfo.ID;
            specialShiftInfo.CreatorName = userInfo.Name;
            specialShiftInfo.CreateTime = createTime;
            specialShiftInfo.ModifierID = userInfo.ID;
            specialShiftInfo.ModifierName = userInfo.Name;
            specialShiftInfo.ModifyTime = createTime;
            specialShiftInfo.WardCode = userInfo.WardCode;
            specialShiftInfo.WardName = userInfo.WardName;
            specialShiftInfo.FirstSignID = specialShiftInfo.CreatorID;
            specialShiftInfo.FirstSignName = specialShiftInfo.CreatorName;
            specialShiftInfo.FirstSignTime = specialShiftInfo.CreateTime;
            specialShiftInfo.ShiftRecordID = specialShiftInfo.MakeRecordID(shiftDate);
            return specialShiftInfo;
        }
    }
}
