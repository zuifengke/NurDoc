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
        /// 获取食物成分服务实例
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

        #region"食物成分读写接口"

        /// <summary>
        /// 获取所有食物成分列表
        /// </summary>
        /// <param name="lstFoodEleInfos">食物成分列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetAllFoodElesFolder(ref List<FoodEleInfo> lstFoodEleInfos)
        {
            short shRet = SystemContext.Instance.FoodEleAccess.GetAllFoodElesFolder(ref lstFoodEleInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }
            
        /// <summary>
        /// 获取指定的食物成分文档信息
        /// </summary>
        /// <param name="szFoodCode">inputcode</param>
        /// <param name="lstFoodEleInfo">信息数据</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short GetFoodEleInfo(string szInputCode, ref List<FoodEleInfo> lstFoodEleInfos)
        {
            short shRet = SystemContext.Instance.FoodEleAccess.GetChildFoodEleInfos(szInputCode, ref lstFoodEleInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 更新食物成分到数据库
        /// </summary>
        /// <param name="foodEleInfo">食物成分内容</param>
        /// <param name="szFilePath">文档路径</param>
        /// <returns>SystemData.ReturnValue</returns>
        public short UpdateFoodEleToOrcale(List<FoodEleInfo> lstFoodEleInfos)
        {
            short shRet = SystemContext.Instance.FoodEleAccess.UpdateInfoFoodEle(lstFoodEleInfos);
            if (shRet == ServerData.ExecuteResult.RES_NO_FOUND)
                return SystemConst.ReturnValue.OK;
            return shRet;
        }

        /// <summary>
        /// 获取excel中的食物成分数据
        /// </summary>
        /// <param name="szFilePath">文档路径</param>
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
