﻿using System;
using System.Data;
using System.Linq;

namespace DAO
{
    public class AccountType_DAO
    {
        private static AccountType_DAO request = new AccountType_DAO();
        public static AccountType_DAO Request { get => request; set => request = value; }
        public AccountType_DAO() { }
        public DataTable GetAllAccountType()
        {
            try
            {
                return DatabaseProvider.Request.ExecuteQuery("select * from AccountType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
