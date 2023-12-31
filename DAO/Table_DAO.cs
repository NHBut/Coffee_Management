﻿using System;
using System.Data;

namespace DAO
{
    public class Table_DAO
    {
        private static Table_DAO request;
        public static Table_DAO Request
        {
            get
            {
                if (request == null)
                    request = new Table_DAO();
                return request;
            }
        }
        private Table_DAO() { }
        public DataTable GetAllTable()
        {
            try
            {
                return DatabaseProvider.Request.ExecuteQuery("USP_GetAllTable");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetTableList()
        {
            try
            {
                return DatabaseProvider.Request.ExecuteQuery("USP_GetListTable");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SwitchTable(int tableID1, int tableID2)
        {
            try
            {
                DatabaseProvider.Request.ExecuteQuery("USP_SwitchTable @TableID1 , @TableID2", new object[] { tableID1, tableID2 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void MergeTable(int tableID1, int tableID2)
        {
            try
            {
                DatabaseProvider.Request.ExecuteNonQuery("USP_MergeTable @TableID1 , @TableID2", new object[] { tableID1, tableID2 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertTable(string name)
        {
            string query = "USP_InsertTable @Name";
            int result = DatabaseProvider.Request.ExecuteNonQuery(query, new object[] { name });
            return result > 0;
        }
        public bool UpdateTable(int id, string name)
        {
            string query = "USP_UpdateTable @ID , @Name";
            int result = DatabaseProvider.Request.ExecuteNonQuery(query, new object[] { id, name });
            return result > 0;
        }
        public bool DeleteTable(int id)
        {
            string query = string.Format("USP_DeleteTableFood @ID");
            int result = DatabaseProvider.Request.ExecuteNonQuery(query, new object[] { id });
            return result > 0;
        }
    }
}