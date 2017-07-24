using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL;

namespace Heren.NurDoc.Data
{
    public class FoodEleService
    {
        private static FoodEleService m_instance = null;

        /// <summary>
        /// ��ȡʳ��ɷַ���ʵ��
        /// </summary>
        public static FoodEleService Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new FoodEleService();
                return m_instance;
            }
        }

        private FoodEleService()
        {
        }

        #region"ʳ��ɷֶ�д�ӿ�"

        /// <summary>
        /// ��ȡ����ʳ��ɷ��б�
        /// </summary>
        /// <param name="lstFoodEleInfos">ʳ��ɷ��б�</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetAllFoodElesFolder(ref List<FoodEleInfo> lstFoodEleInfos)
        {
            short shRet = SystemContext.Instance.FoodEleAccess.GetAllFoodElesFolder(ref lstFoodEleInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }
            
        /// <summary>
        /// ��ȡָ����ʳ��ɷ��ĵ���Ϣ
        /// </summary>
        /// <param name="szFoodCode">inputcode</param>
        /// <param name="lstFoodEleInfo">��Ϣ����</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetFoodEleInfo(string szInputCode, ref List<FoodEleInfo> lstFoodEleInfos)
        {
            short shRet = SystemContext.Instance.FoodEleAccess.GetChildFoodEleInfos(szInputCode, ref lstFoodEleInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// ����ʳ��ɷֵ����ݿ�
        /// </summary>
        /// <param name="foodEleInfo">ʳ��ɷ�����</param>
        /// <param name="szFilePath">�ĵ�·��</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short UpdateFoodEleToOrcale(List<FoodEleInfo> lstFoodEleInfos)
        {
            short shRet = SystemContext.Instance.FoodEleAccess.UpdateInfoFoodEle(lstFoodEleInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// ��ȡexcel�е�ʳ��ɷ�����
        /// </summary>
        /// <param name="szFilePath">�ĵ�·��</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetExcelValues(string szFilePath, ref List<FoodEleInfo> lstFoodEles)
        {
            short shRet = SystemContext.Instance.FoodEleAccess.GetExcelValueInfos(szFilePath, ref lstFoodEles);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }
        #endregion
    }
}
