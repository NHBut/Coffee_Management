﻿using BUS;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using DevExpress.XtraEditors.Repository;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;

namespace GUI
{
    public partial class fAccount : DevExpress.XtraEditors.XtraForm
    {
        private string loginUserName;

        public string LoginUserName
        {
            get { return loginUserName; }
            set { loginUserName = value; }
        }
        public fAccount()
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));
            InitializeComponent();
            LoadAcount();
            btnRemove.Enabled = false;
            btnResetPassword.Enabled = false;
            btnSearch.Enabled = false;
            SplashScreenManager.CloseForm();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                gcAccount.DataSource = Account_BUS.Request.SearchAccountByUserName(txtSearchAccount.Text);
                SplashScreenManager.CloseForm();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm();
                XtraMessageBox.Show("Error: " + ex);
            }
        }
        private void LoadAcount()
        {
            try
            {
                gcAccount.DataSource = Account_BUS.Request.GetAllAcount();
                gvAccount.Columns[0].Caption = "Tên đăng nhập";
                gvAccount.Columns[1].Caption = "Tên hiển thị";
                gvAccount.Columns[2].Caption = "Loại tài khoản";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex);
            }

            RepositoryItemLookUpEdit myLookup = new RepositoryItemLookUpEdit();
            try
            {
                myLookup.DataSource = AccountType_BUS.Request.GetAllAccountType();
                myLookup.DisplayMember = "TypeName";
                myLookup.ValueMember = "ID";
                myLookup.NullText = "-- Chọn loại tài khoản --";
                gvAccount.Columns[2].ColumnEdit = myLookup;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex);
            }
        }

        private void btnResetPassword_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string userName = gvAccount.GetRowCellValue(gvAccount.FocusedRowHandle, gvAccount.Columns[0]).ToString();
            if (XtraMessageBox.Show("Đặt lại mật khẩu cho tài khoản " + userName + "?", "Xác nhận", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                if (Account_BUS.Request.ResetPassword(userName))
                {
                    SplashScreenManager.ShowForm(typeof(WaitForm1));
                    LoadAcount();
                    SplashScreenManager.CloseForm();
                    XtraMessageBox.Show("Đặt lại mật khẩu thành công\n Mật khẩu mặc định là '0'");
                    Log.WriteLog("set password for Account: " + userName);
                }
                else
                    XtraMessageBox.Show("Đặt lại mật khẩu thất bại", "Lỗi");
            }
            btnResetPassword.Enabled = false;
        }

        private void btnRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string userName = gvAccount.GetRowCellValue(gvAccount.FocusedRowHandle, gvAccount.Columns[0]).ToString();
            if (loginUserName.Equals(userName))
            {
                XtraMessageBox.Show("Không thể xóa tài khoản hiện hành!!!");
                return;
            }

            if (XtraMessageBox.Show("Xóa " + userName + "?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (Account_BUS.Request.Delete(userName))
                {
                    SplashScreenManager.ShowForm(typeof(WaitForm1));
                    LoadAcount();
                    SplashScreenManager.CloseForm();
                    XtraMessageBox.Show("Xóa tài khoản thành công");
                    Log.WriteLog("delete Account: " + userName);
                }
                else
                    XtraMessageBox.Show("Xóa tài khoản thất bại", "Lỗi");
            }
            btnRemove.Enabled = false;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));
            LoadAcount();
            SplashScreenManager.CloseForm();
        }

        private void gcAccount_DoubleClick(object sender, EventArgs e)
        {

        }
        private void InsertAccount(GridView view, int rowHandle)
        {
            string userName = view.GetRowCellValue(rowHandle, view.Columns[0]).ToString();
            if (userName == "" || CheckCharacter(userName) == false)
            {
                XtraMessageBox.Show("Tên tài khoản không hợp lệ");
                return;
            }

            for (int i = 0; i < view.RowCount - 1; i++)
            {
                if (userName.Equals(view.GetRowCellValue(i, view.Columns[0]).ToString()))
                {
                    view.SetRowCellValue(view.FocusedRowHandle, view.Columns[0], "");
                    XtraMessageBox.Show("Tên tài khoản này đã được sử dụng!");
                    return;
                }
            }

            string displayName = view.GetRowCellValue(rowHandle, view.Columns[1]).ToString();
            if (displayName == "")
            {
                XtraMessageBox.Show("Tên hiển thị không hợp lệ");
                return;
            }

            int accountType;
            if (int.TryParse(view.GetRowCellValue(rowHandle, view.Columns[2]).ToString(), out accountType) == false)
            {
                XtraMessageBox.Show("Hãy chọn loại tài khoản");
                return;
            }

            if (Account_BUS.Request.Insert(userName, displayName, accountType))
            {
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                LoadAcount();
                SplashScreenManager.CloseForm();
                XtraMessageBox.Show("Thêm tài khoản mới thành công\n Mật khẩu mặc định là '0'\n Hãy đổi mật khẩu để bảo mật tài khoản", "Thông báo");
                Log.WriteLog("add new Account: " + userName);
            }
            else
                XtraMessageBox.Show("Thêm tài khoản mới thất bại", "Lỗi");
        }
        private bool CheckCharacter(string str)
        {
            string correctString = "1234567890 QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            foreach (char character1 in str)
            {
                bool isCorrect = false;
                foreach (char character2 in correctString)
                {
                    if (character1 == character2)
                        isCorrect = true;
                }
                if (isCorrect == false)
                    return false;
            }
            return true;
        }

        private void gvAccount_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;

            if (view.IsNewItemRow(e.RowHandle))
                InsertAccount(view, e.RowHandle);
            else
            {
                string curUserName = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
                for (int i = 0; i < view.RowCount - 1; i++)
                {
                    if (curUserName.Equals(view.GetRowCellValue(i, view.Columns[0]).ToString()))
                    {
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns[0], "");

                        return;
                    }
                }
            }
        }

        private void txtSearchAccount_TextChanged(object sender, EventArgs e)
        {
          
                if (txtSearchAccount.Text != "")
                    btnSearch.Enabled = true;
                else
                    btnSearch.Enabled = false;
      
        }

        private void gvAccount_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

        }
    }
}