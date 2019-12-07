using FonNature.DAO;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FonNature
{
    public partial class fLogin : Form
    {
        // Property
        public static string Position;
        // Event
        public fLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var listPosition = new PositionRepository().GetList();
            if (listPosition != null)
            {
                foreach (var position in listPosition)
                {
                    cbbPosition.Items.Add(position.Name);
                }
            }
            cbbPosition.SelectedIndex = 2;
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            // Không được sử dụng kí tự đặc biệt trong username
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            if (!regexItem.IsMatch(txbusername.Text)) MessageBox.Show("Không được sử dụng kí tự đặc biệt trong username", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            var userNameExist = new AccountRepository().CheckUserName(cbbPosition.SelectedItem.ToString(), txbusername.Text);

            if(userNameExist)
            {
                var loginSuccess = new AccountRepository().CheckLogin(txbusername.Text, txbpass.Text);
                if (!loginSuccess) MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (loginSuccess)
                {
                    Position = cbbPosition.SelectedItem.ToString();
                    if (Position == "CEO" || Position == "Manager")
                    {
                        FAdmin fAdmin = new FAdmin();
                        this.Hide();
                        fAdmin.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        FMain fMain = new FMain();
                        this.Hide();
                        fMain.ShowDialog();
                        this.Show();
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc vị trí !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnquit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                e.Cancel = true; // Không cho event này thực thi
            }
        }

        // ethod
    }
}
