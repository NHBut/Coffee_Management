﻿using System;
using System.Linq;
using DTO;
using BUS;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System.Windows.Forms;

namespace GUI
{
    public partial class fAccountInformation : DevExpress.XtraEditors.XtraForm
    {
        public fAccountInformation()
        {
            InitializeComponent();
        }
        public fAccountInformation(Account acc)
        {
            InitializeComponent();
            txtUserName.Text = acc.Username;
            txtDisplayName.Text = acc.DisplayName;
        }
        private void UpdateAccount()
        {
            string displayName = txtDisplayName.Text;
            string password = txtPassword.Text;
            string newPass = txtNewPassword.Text;
            string retypePass = txtReTypePass.Text;
            string userName = txtUserName.Text;

            if (!newPass.Equals(retypePass))
                MessageBox.Show("Mật khẩu nhập lại không đúng");
            else
            {
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                if (Account_BUS.Request.UpdateInformation(userName, displayName, password, newPass))
                {
                    SplashScreenManager.CloseForm();
                    XtraMessageBox.Show("Cập nhật thành công");
                    Log.WriteLog("change account's information");
                }
                else
                {
                    SplashScreenManager.CloseForm();
                    XtraMessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fAccountInformation_Load(object sender, EventArgs e)
        {

        }
        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.UpdateAccount();
        }
    }
}