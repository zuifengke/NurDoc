// 数据库访问层与食物成分内容有关的数据的访问类.
// Creator:YangMingkun  Date:2013-11-07
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using Heren.Common.Libraries;
using Heren.Common.Libraries.DbAccess;
using Heren.Common.Libraries.Ftp;

namespace Heren.NurDoc.DAL.DbAccess
{
    public class FoodEleAccess : DBAccessBase
    {
        #region"食物成分读写接口"
        /// <summary>
        /// 更新数据库中的食物成分
        /// </summary>
        /// <param name="foodEleInfo">食物成分信息</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short UpdateInfoFoodEle(List<FoodEleInfo> lstFoodEleInfos)
        {
            if (lstFoodEleInfos == null)
            {
                LogManager.Instance.WriteLog("FoodEleAccess.InsertInfoFoodEle FoodCode, FoodName 参数不能为空!");
                return ServerData.ExecuteResult.PARAM_ERROR;
            }
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            WorkProcess.Instance.Initialize(null, lstFoodEleInfos.Count, "正在更新数据库数据，请稍候...");

            string szTranField = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41}"
                , ServerData.FoodEleTable.Food_Code, ServerData.FoodEleTable.Food_Name
                , ServerData.FoodEleTable.Input_Code, ServerData.FoodEleTable.Edible
                , ServerData.FoodEleTable.Water, ServerData.FoodEleTable.Kcal
                , ServerData.FoodEleTable.Kj, ServerData.FoodEleTable.Protein
                , ServerData.FoodEleTable.Fat, ServerData.FoodEleTable.Cho
                , ServerData.FoodEleTable.Fiber, ServerData.FoodEleTable.Cholesterol
                , ServerData.FoodEleTable.Ash, ServerData.FoodEleTable.Va
                , ServerData.FoodEleTable.Carotene, ServerData.FoodEleTable.Retinol
                , ServerData.FoodEleTable.Thiamin, ServerData.FoodEleTable.Ribo
                , ServerData.FoodEleTable.Niacin, ServerData.FoodEleTable.Vc
                , ServerData.FoodEleTable.Vetotal, ServerData.FoodEleTable.VeAe
                , ServerData.FoodEleTable.VeBge, ServerData.FoodEleTable.VeTe
                , ServerData.FoodEleTable.Ca, ServerData.FoodEleTable.P
                , ServerData.FoodEleTable.K, ServerData.FoodEleTable.Na
                , ServerData.FoodEleTable.Mg, ServerData.FoodEleTable.Fe
                , ServerData.FoodEleTable.Zn, ServerData.FoodEleTable.Se
                , ServerData.FoodEleTable.Cu, ServerData.FoodEleTable.Mn
                , ServerData.FoodEleTable.Remark, ServerData.FoodEleTable.FolicAcid
                , ServerData.FoodEleTable.Iodine, ServerData.FoodEleTable.IsofTotal
                , ServerData.FoodEleTable.IsofDaidzein, ServerData.FoodEleTable.IsofGenistein
                , ServerData.FoodEleTable.IsofGlyCitein, ServerData.FoodEleTable.Gi);

            string strTranSql = string.Empty;
            string strTran = string.Empty;
            List<string> strSQL = new List<string>();
            for (int i = 0; i < lstFoodEleInfos.Count; i++)
            {
                strTranSql= string.Format("'{0}','{1}','{2}',{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},'{34}',{35},{36},{37},{38},{39},{40},{41}"
                      , lstFoodEleInfos[i].FoodCode, lstFoodEleInfos[i].FoodName, lstFoodEleInfos[i].InputCode
                      , lstFoodEleInfos[i].Edible, lstFoodEleInfos[i].Water, lstFoodEleInfos[i].Kcal
                      , lstFoodEleInfos[i].Kj, lstFoodEleInfos[i].Protein, lstFoodEleInfos[i].Fat, lstFoodEleInfos[i].Cho
                      , lstFoodEleInfos[i].Fiber, lstFoodEleInfos[i].Cholesterol, lstFoodEleInfos[i].Ash
                      , lstFoodEleInfos[i].Va, lstFoodEleInfos[i].Carotene, lstFoodEleInfos[i].Retinol
                      , lstFoodEleInfos[i].Thiamin, lstFoodEleInfos[i].Ribo, lstFoodEleInfos[i].Niacin
                      , lstFoodEleInfos[i].Vc, lstFoodEleInfos[i].VeTotal, lstFoodEleInfos[i].VeAe
                      , lstFoodEleInfos[i].VeBge, lstFoodEleInfos[i].VeTe, lstFoodEleInfos[i].Ca, lstFoodEleInfos[i].P
                      , lstFoodEleInfos[i].K, lstFoodEleInfos[i].Na, lstFoodEleInfos[i].Mg, lstFoodEleInfos[i].Fe
                      , lstFoodEleInfos[i].Zn, lstFoodEleInfos[i].Se, lstFoodEleInfos[i].Cu, lstFoodEleInfos[i].Mn
                      , lstFoodEleInfos[i].Remark, lstFoodEleInfos[i].FolicAcid, lstFoodEleInfos[i].Iodine
                      , lstFoodEleInfos[i].IsofTotal, lstFoodEleInfos[i].IsofDaidzein, lstFoodEleInfos[i].IsofGenistein
                      , lstFoodEleInfos[i].IsofGlycitein, lstFoodEleInfos[i].Gi);
                strTran = string.Format(ServerData.SQL.INSERT, ServerData.DataTable.NUR_FOOD_ELEMENT, szTranField, strTranSql);
                strSQL.Add(strTran);
            }            

            //开始数据库事务
            if (!base.DataAccess.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return ServerData.ExecuteResult.EXCEPTION;
            }

            short shRet = this.DeleteInfoFoodEle();
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
                return shRet;
            }
            int count = 0;
            foreach (string strInsertSQL in strSQL)
            {
                count += 1;
                WorkProcess.Instance.Show(count, false);
                shRet = this.UpdateInfoFoodEle(strInsertSQL);
                if (shRet != ServerData.ExecuteResult.OK)
                    break;
            }
            if (shRet != ServerData.ExecuteResult.OK)
            {
                base.DataAccess.AbortTransaction();
            }
            else
            {
                if (!base.DataAccess.CommitTransaction(true))
                    return ServerData.ExecuteResult.EXCEPTION;
            }
            WorkProcess.Instance.Close();
            return shRet;
        }

        /// <summary>
        /// 在更新数据库中食物成分表之前先将原有的数据删除
        /// </summary>
        /// <returns>ServerData.ExecuteResult</returns>
        public short DeleteInfoFoodEle()
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szSQL = string.Format(ServerData.SQL.DELETE_ALL, ServerData.DataTable.NUR_FOOD_ELEMENT);
            
            int nCount = 0;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(szSQL, CommandType.Text);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            return (nCount >= 0) ? ServerData.ExecuteResult.OK : ServerData.ExecuteResult.OTHER_ERROR;
        }

        /// <summary>
        /// 更新数据库中食物成分表
        /// </summary>
        /// <param name="strTranSql">需要提交、执行的Insert语句</param>
        private short UpdateInfoFoodEle(string strTranSql)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            int nCount;
            try
            {
                nCount = base.DataAccess.ExecuteNonQuery(strTranSql, CommandType.Text);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLog("FoodEleAccess.UpdateInfoFoodEle", new string[] { "strTranSql" }, new object[] { strTranSql }, "SQL语句执行失败!", ex);
                return ServerData.ExecuteResult.EXCEPTION;
            }
            if (nCount <= 0)
            {
                LogManager.Instance.WriteLog("FoodEleAccess.UpdateInfoFoodEle", new string[] { "strTranSql" }, new object[] { strTranSql }, "SQL语句执行后返回0!");
                return ServerData.ExecuteResult.EXCEPTION;
            }
            return ServerData.ExecuteResult.OK;
        }
               
        /// <summary>
        /// 获取指定文档中的食物成分内容
        /// </summary>
        /// <param name="szFilePath">文档路径</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetExcelValueInfos(string szFilePath, ref List<FoodEleInfo> lstFoodEles)
        {
            string cmdText = string.Format(ServerData.SQL.excelCmdText, szFilePath);
            string szSQL = string.Format(ServerData.SQL.SELECT_FROM, "*", "[sheet1$]");
            DataSet szDataSet = new DataSet();
            try
            {
                OleDbConnection oledbConn = new OleDbConnection(cmdText);
                oledbConn.Open();
                OleDbDataAdapter OleDbDap = new OleDbDataAdapter(szSQL, oledbConn);
                OleDbDap.Fill(szDataSet, "table");
                oledbConn.Close();
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            
            if (szDataSet.Tables[0].Columns.Count < 41)
            {
                return ServerData.ExecuteResult.OTHER_ERROR;
            }
            if (lstFoodEles == null)
                lstFoodEles = new List<FoodEleInfo>();
            List<double> strValue = new List<double>();
            for (int i = 0; i < szDataSet.Tables[0].Rows.Count; i++)
            {
                
                if (!szDataSet.Tables[0].Rows[i][0].ToString().Trim().Equals("") 
                    && !szDataSet.Tables[0].Rows[i][1].ToString().Trim().Equals("")
                    && !IsCorrectValue(szDataSet, i, 3).Equals("''"))
                {
                    FoodEleInfo foodEleInfo = new FoodEleInfo();
                    foodEleInfo.FoodCode = szDataSet.Tables[0].Rows[i][0].ToString();
                    foodEleInfo.FoodName = szDataSet.Tables[0].Rows[i][1].ToString();
                    foodEleInfo.InputCode = GlobalMethods.Convert.GetInputCode(szDataSet.Tables[0].Rows[i][1].ToString(), true, 20);
                    foodEleInfo.Edible = IsCorrectValue(szDataSet, i, 2);
                    foodEleInfo.Water = IsCorrectValue(szDataSet, i, 3);
                    foodEleInfo.Kcal = IsCorrectValue(szDataSet, i, 4);
                    foodEleInfo.Kj = IsCorrectValue(szDataSet, i, 5);
                    foodEleInfo.Protein = IsCorrectValue(szDataSet, i, 6);
                    foodEleInfo.Fat = IsCorrectValue(szDataSet, i, 7);
                    foodEleInfo.Cho = IsCorrectValue(szDataSet, i, 8);
                    foodEleInfo.Fiber = IsCorrectValue(szDataSet, i, 9);
                    foodEleInfo.Cholesterol = IsCorrectValue(szDataSet, i, 10);
                    foodEleInfo.Ash = IsCorrectValue(szDataSet, i, 11);
                    foodEleInfo.Va = IsCorrectValue(szDataSet, i, 12);
                    foodEleInfo.Carotene = IsCorrectValue(szDataSet, i, 13);
                    foodEleInfo.Retinol = IsCorrectValue(szDataSet, i, 14);
                    foodEleInfo.Thiamin = IsCorrectValue(szDataSet, i, 15);
                    foodEleInfo.Ribo = IsCorrectValue(szDataSet, i, 16);
                    foodEleInfo.Niacin = IsCorrectValue(szDataSet, i, 17);
                    foodEleInfo.Vc = IsCorrectValue(szDataSet, i, 18);
                    foodEleInfo.VeTotal = IsCorrectValue(szDataSet, i, 19);
                    foodEleInfo.VeAe = IsCorrectValue(szDataSet, i, 20);
                    foodEleInfo.VeBge = IsCorrectValue(szDataSet, i, 21);
                    foodEleInfo.VeTe = IsCorrectValue(szDataSet, i, 22);
                    foodEleInfo.Ca = IsCorrectValue(szDataSet, i, 23);
                    foodEleInfo.P = IsCorrectValue(szDataSet, i, 24);
                    foodEleInfo.K = IsCorrectValue(szDataSet, i, 25);
                    foodEleInfo.Na = IsCorrectValue(szDataSet, i, 26);
                    foodEleInfo.Mg = IsCorrectValue(szDataSet, i, 27);
                    foodEleInfo.Fe = IsCorrectValue(szDataSet, i, 28);
                    foodEleInfo.Zn = IsCorrectValue(szDataSet, i, 29);
                    foodEleInfo.Se = IsCorrectValue(szDataSet, i, 30);
                    foodEleInfo.Cu = IsCorrectValue(szDataSet, i, 31);
                    foodEleInfo.Mn = IsCorrectValue(szDataSet, i, 32);
                    foodEleInfo.Remark = szDataSet.Tables[0].Rows[i][33].ToString();
                    foodEleInfo.FolicAcid = IsCorrectValue(szDataSet, i, 34);
                    foodEleInfo.Iodine = IsCorrectValue(szDataSet, i, 35);
                    foodEleInfo.IsofTotal = IsCorrectValue(szDataSet, i, 36);
                    foodEleInfo.IsofDaidzein = IsCorrectValue(szDataSet, i, 37);
                    foodEleInfo.IsofGenistein = IsCorrectValue(szDataSet, i, 38);
                    foodEleInfo.IsofGlycitein = IsCorrectValue(szDataSet, i, 39);
                    foodEleInfo.Gi = IsCorrectValue(szDataSet, i, 40);
                    lstFoodEles.Add(foodEleInfo);
                }
            }
            return ServerData.ExecuteResult.OK;   
        }

        /// <summary>
        /// 判断、过滤excel表中的数据
        /// </summary>
        /// <param name="szDataSet">存储excel中数据的表</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        /// <returns>过滤后的值</returns>
        private string IsCorrectValue(DataSet szDataSet, int rowIndex, int columnIndex)
        {
            if (szDataSet.Tables[0].Rows[rowIndex][columnIndex].Equals("") || szDataSet.Tables[0].Rows[rowIndex][columnIndex].ToString().Equals(""))
                return "''";
            else
            {
                string strValue = szDataSet.Tables[0].Rows[rowIndex][columnIndex].ToString();
                if (strValue.Contains("*"))
                    return strValue.Substring(0, strValue.Length - 1);
                else if (strValue.Equals("Tr") || strValue.Equals("…") || strValue.Equals("－") || strValue.Equals("—"))
                    return "''";
                else
                    return strValue;
            }
        }

        /// <summary>
        /// 获取食物成分表中信息
        /// </summary>
        /// <param name="lstFoodEleInfos">食物成分列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetAllFoodElesFolder(ref List<FoodEleInfo> lstFoodEleInfos)
        {
            string szCondition = string.Empty;
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.FoodEleTable.Input_Code);
            return this.GetFoodEleInfosInternal(string.Empty, szOrderSQL, ref lstFoodEleInfos);
        }

        /// <summary>
        /// 获取指定input_code的护理食物成分列表
        /// </summary>
        /// <param name="szInputCode">input_code</param>
        /// <param name="lstFoodEles">食物成分列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        public short GetChildFoodEleInfos(string szInputCode, ref List<FoodEleInfo> lstFoodEles)
        {
            string szCondition = string.Format("{0}='{1}'", ServerData.FoodEleTable.Input_Code, szInputCode);
            string szOrderSQL = string.Format("ORDER BY {0} ASC", ServerData.FoodEleTable.Input_Code);
            return this.GetFoodEleInfosInternal(szCondition, szOrderSQL, ref lstFoodEles);
        }

        /// <summary>
        /// 根据指定的查询条件获取对应的食物编号、食物名称、食物名称首字母、食物含水量
        /// </summary>
        /// <param name="szCondition">查询条件</param>
        /// <param name="szOrderSQL">排序子SQL</param>
        /// <param name="lstFoodEles">食物成分列表</param>
        /// <returns>ServerData.ExecuteResult</returns>
        private short GetFoodEleInfosInternal(string szCondition, string szOrderSQL, ref List<FoodEleInfo> lstFoodEles)
        {
            if (base.DataAccess == null)
                return ServerData.ExecuteResult.PARAM_ERROR;

            string szField = string.Format("{0},{1},{2},{3}"
                , ServerData.FoodEleTable.Food_Code, ServerData.FoodEleTable.Food_Name
                , ServerData.FoodEleTable.Input_Code, ServerData.FoodEleTable.Water);

            string szSQL = string.Empty;
            if (GlobalMethods.Misc.IsEmptyString(szCondition))
                szSQL = string.Format(ServerData.SQL.SELECT_FROM, szField, ServerData.DataTable.NUR_FOOD_ELEMENT);
            else
                szSQL = string.Format(ServerData.SQL.SELECT_WHERE, szField, ServerData.DataTable.NUR_FOOD_ELEMENT, szCondition);

            if (!GlobalMethods.Misc.IsEmptyString(szOrderSQL))
                szSQL = string.Concat(szSQL, " ", szOrderSQL);
            IDataReader dataReader = null;
            try
            {
                dataReader = base.DataAccess.ExecuteReader(szSQL, CommandType.Text);
                if (dataReader == null || dataReader.IsClosed || !dataReader.Read())
                {
                    return ServerData.ExecuteResult.RES_NO_FOUND;
                }
                if (lstFoodEles == null)
                    lstFoodEles = new List<FoodEleInfo>();
                do
                {
                    FoodEleInfo foodEleInfo = new FoodEleInfo();
                    foodEleInfo.FoodCode = dataReader.GetString(0);
                    foodEleInfo.FoodName = dataReader.GetString(1);
                    if (!dataReader.IsDBNull(2))
                        foodEleInfo.InputCode = dataReader.GetString(2);
                    foodEleInfo.Water = Convert.ToString(dataReader.GetDouble(3));

                    lstFoodEles.Add(foodEleInfo);
                } while (dataReader.Read());
                return ServerData.ExecuteResult.OK;
            }
            catch (Exception ex)
            {
                return this.HandleException(ex, System.Reflection.MethodInfo.GetCurrentMethod(), szSQL, "SQL语句执行失败!");
            }
            finally { base.DataAccess.CloseConnnection(false); }
        }
        #endregion      
    }
}
