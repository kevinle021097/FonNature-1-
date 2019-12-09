using FastMember;
using FonNature.DAO;
using FonNature.DTO;
using Models.Entity;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = Models.Entity.Color;
using Member = Models.Entity.Member;

namespace FonNature
{
    public partial class FAdmin : Form
    {
        public FAdmin()
        {
            InitializeComponent();
            LoadHoaDon();
        }


        #region method
       


        private void LoadHoaDon()
        {

            if (dpendday.Value.Month == 1)
            {
                int monthstartday = 12;
                int daystartdayy = dpendday.Value.Day;
                int yearstartday = dpendday.Value.Year - 1;
                DateTime starday = new DateTime(yearstartday, monthstartday, daystartdayy);
                dpStartday.Value = starday;
            }
            else
            {
                int monthstartday = dpendday.Value.Month - 1;
                int daystartdayy = dpendday.Value.Day;
                int yearstartday = dpendday.Value.Year;
                DateTime starday = new DateTime(yearstartday, monthstartday, daystartdayy);
                dpStartday.Value = starday;
            }


            button1_Click(this, new EventArgs());



        }




        #endregion

        #region hoadon
        private void btngiaohang_Click(object sender, EventArgs e)
        {
            int rowindex = dgvHoaDon.CurrentRow.Index;
            if (dgvHoaDon.CurrentCell.ColumnIndex == 6)
            {
                if (dgvHoaDon.CurrentCell.Value.ToString() == "Chưa Giao Hàng")
                {
                    dgvHoaDon.CurrentCell.Value = "Đang Giao Hàng";
                    int idhd = int.Parse(dgvHoaDon.Rows[rowindex].Cells[0].Value.ToString());
                    DataTable dt = ThongTinHDDAO.Instance.GetTableforKho(idhd);
                    List<ThongTinHoaDon> listtthd = new List<ThongTinHoaDon>();


                    foreach (DataRow item in dt.Rows)
                    {
                        ThongTinHoaDon tthd = new ThongTinHoaDon(item);
                        listtthd.Add(tthd);
                    }

                    // Lấy ra các id sp và update
                    for (int i = 0; i < listtthd.Count; i++)
                    {
                        SanPhamDAO.Instance.Updatekhohang(listtthd[i].Idsp, listtthd[i].Soluong);
                    }

                    //Cập nhật lại dgvsanpham


                }
                else
                {
                    dgvHoaDon.CurrentCell.Value = "Đã Giao Hàng";

                }

            }
            int id = int.Parse(dgvHoaDon.Rows[rowindex].Cells[0].Value.ToString());
            HoaDonDAO.Instance.xacnhangiaohang(dgvHoaDon.CurrentCell.Value.ToString(), id);
        }

        private void dgvHoaDon_Click(object sender, EventArgs e)
        {
            int id = 0;

            int indexrows = dgvHoaDon.CurrentRow.Index;
            id = int.Parse(dgvHoaDon.Rows[indexrows].Cells[0].Value.ToString());


            if (id != 0)
            {
                DataTable dt = ThongTinHDDAO.Instance.GetTTHDbyID(id);
                dgvchitiethd.DataSource = dt;
            }
        }


        private void txbsearchhd_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbsearchhd.Text != "")
            {
                DataTable dt = HoaDonDAO.Instance.SearchHdBySDT("%" + txbsearchhd.Text + "%");
                dgvHoaDon.DataSource = dt;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DateTime starday = dpStartday.Value;
            DateTime endday = dpendday.Value;
            DataTable dt = HoaDonDAO.Instance.Gethdbydateandpage(starday, endday, int.Parse(txbpage.Text));

            dgvHoaDon.DataSource = dt;
            dgvHoaDon.Columns[4].DefaultCellStyle.Format = "N0";
        }

        private void btnDoanhthu_Click(object sender, EventArgs e)
        {
            double doanhthu = 0;
            for (int i = 0; i < dgvHoaDon.Rows.Count; i++)
            {
                if (dgvHoaDon.Rows[i].Cells[6].Value.ToString() == "Đã Giao Hàng" || dgvHoaDon.Rows[i].Cells[6].Value.ToString() == "Đang Giao Hàng")
                {
                    double tien = double.Parse(dgvHoaDon.Rows[i].Cells[4].Value.ToString());
                    doanhthu += tien;
                }
            }
            MessageBox.Show("Doanh Thu = " + doanhthu.ToString("N0"));
        }

        private void dpendday_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(this, new EventArgs());
        }

        private void dpStartday_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(this, new EventArgs());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            txbpage.Text = "1";
        }

        private void txbpage_TextChanged(object sender, EventArgs e)
        {
            button1_Click(this, new EventArgs());
        }

        private void btnLastpage_Click(object sender, EventArgs e)
        {
            int sumrecord = HoaDonDAO.Instance.GetCountHdByDate(dpStartday.Value, dpendday.Value);
            int lastpage = sumrecord / 20;
            if (sumrecord % 20 != 0)
                lastpage++;

            txbpage.Text = lastpage.ToString();

        }

        private void btnNextpage_Click(object sender, EventArgs e)
        {
            int sumrecord = HoaDonDAO.Instance.GetCountHdByDate(dpStartday.Value, dpendday.Value);
            int lastpage = sumrecord / 20;
            if (sumrecord % 20 != 0)
                lastpage++;
            int page = int.Parse(txbpage.Text);

            if (page < lastpage)
            {
                page++;
                txbpage.Text = page.ToString();
            }

        }

        private void btnPrePage_Click(object sender, EventArgs e)
        {
            int page = int.Parse(txbpage.Text);
            if (page > 1)
            {
                page--;
                txbpage.Text = page.ToString();
            }
        }

        #endregion

        #region Position
        private void LoadPosition()
        {
            var positionList = new PositionRepository().GetList();
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(positionList, "Id", "Name"))
            {
                dt.Load(reader);
            }
            dgvPositionList.DataSource = dt;
            dgvPositionList.Columns[0].HeaderText = "ID";
            dgvPositionList.Columns[1].HeaderText = "Tên chức vụ";
        }
        private void tpPosition_Enter(object sender, EventArgs e)
        {
            LoadPosition();
        }
        private void btnAddPosition_Click(object sender, EventArgs e)
        {
            if (txbPositionName.Text != "")
            {
                var checkExits = new PositionRepository().CheckExits(txbPositionName.Text);
                if (checkExits)
                {
                    var position = new Position() { Name = txbPositionName.Text };
                    var addSuccess = new PositionRepository().Add(position);
                    if (addSuccess == 0) MessageBox.Show("Thêm chức vụ không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadPosition();
                    txbPositionName.Text = "";
                }
                else
                {
                    MessageBox.Show("Tên chức vụ đã tồn tại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Tên chức vụ không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnRemovePosition_Click(object sender, EventArgs e)
        {
            int index = dgvPositionList.CurrentRow.Index;
            
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                var deleteSuccess = new PositionRepository().Delete(dgvPositionList.Rows[index].Cells[0].Value.ToString());
                LoadPosition();
                txbIdPosition.Text = "";
                txbPositionName.Text = "";
                if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditPosition_Click(object sender, EventArgs e)
        {
            if (txbPositionName.Text != "" && txbIdPosition.Text != "")
            {
                var checkExits = new PositionRepository().CheckExits(txbPositionName.Text);
                if (checkExits)
                {
                    var position = new Position() { Id = int.Parse(txbIdPosition.Text), Name = txbPositionName.Text };
                    var updateSuccess = new PositionRepository().Edit(position);
                    LoadPosition();
                    txbPositionName.Text = "";
                    txbIdPosition.Text = "";

                }
                else
                {
                    MessageBox.Show("Tên vị trí đã tồn tại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Tên vị trí và Id không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvPositionList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvPositionList.CurrentRow.Index;

            txbIdPosition.Text = dgvPositionList.Rows[index].Cells[0].Value.ToString();
            txbPositionName.Text = dgvPositionList.Rows[index].Cells[1].Value.ToString();
        }

        private void btnClearPosition_Click(object sender, EventArgs e)
        {
            txbIdPosition.Text = "";
            txbPositionName.Text = "";
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbSearchPosition.Text == "") LoadPosition();
            var positionList = new PositionRepository().SearchByName(txbSearchPosition.Text);
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(positionList, "Id", "Name"))
            {
                dt.Load(reader);
            }
            dgvPositionList.DataSource = dt;
            dgvPositionList.Columns[0].HeaderText = "ID";
            dgvPositionList.Columns[1].HeaderText = "Vị trí";
        }

        #endregion

        #region Staff

        private void LoadStaff()
        {

            var staffsList = new StaffRepository().GetList();
            var positionList = new PositionRepository().GetList();
            if(positionList.Count != 0)
            {
                DataTable dt = new DataTable();
                using (var reader = ObjectReader.Create(staffsList, "Id", "Name", "Email", "Address", "Phone", "IdPosition"))
                {
                    dt.Load(reader);
                }

                //Change datatype of column PostionID
                DataTable dtCloned = dt.Clone();
                dtCloned.Columns[5].DataType = typeof(string);
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                dgvStaffList.DataSource = dtCloned;
                dgvStaffList.Columns[0].HeaderText = "ID";
                dgvStaffList.Columns[1].HeaderText = "Tên Nhân Viên";
                dgvStaffList.Columns[2].HeaderText = "Email";
                dgvStaffList.Columns[3].HeaderText = "Địa chỉ";
                dgvStaffList.Columns[4].HeaderText = "Số điện thoại";
                dgvStaffList.Columns[5].HeaderText = "Chức vụ";
                // Change positonID to position Name
                for (int i = 0; i < dgvStaffList.RowCount; i++)
                {
                    foreach (var position in positionList)
                    {
                        if (dgvStaffList.Rows[i].Cells[5].Value.ToString() == position.Id.ToString())
                        {
                            dgvStaffList.Rows[i].Cells[5].Value = position.Name;
                        }
                    }
                }


                cbbStaffPosition.DataSource = positionList;
                cbbStaffPosition.DisplayMember = "Name";
                cbbStaffPosition.ValueMember = "Id";
            }
            else
            {
                MessageBox.Show("Vui lòng thêm chức vụ trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tpPosition.Select();
            }
            
        }

        private void tpStaff_Enter(object sender, EventArgs e)
        {
            LoadStaff();
        }
        // Add
        private void button6_Click(object sender, EventArgs e)
        {
            if (txbStaffName.Text != "")
            {
                var regexItem = new Regex(@"(09|08|03|04|05|07)+([0-9]{8})\b");
                if (regexItem.IsMatch(txbStaffPhone.Text))
                {
                    var staff = new Staff() { Name = txbStaffName.Text, Address = txbStaffAddress.Text, Email = txbStaffEmail.Text, Phone = txbStaffPhone.Text, IdPosition = int.Parse(cbbStaffPosition.SelectedValue.ToString()) };

                    var addSuccess = new StaffRepository().Add(staff);
                    if (addSuccess == 0) MessageBox.Show("Thêm nhân viên không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txbStaffName.Text = "";
                    txbStaffPhone.Text = "";
                    txbStaffAddress.Text = "";
                    txbStaffEmail.Text = "";
                    cbbStaffPosition.DataSource = new List<Position>();
                    LoadStaff();
                }
                else
                {
                    MessageBox.Show("Phải điền đúng định dạng số điện thoại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            else
            {
                MessageBox.Show("Tên nhân viên không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Remove
        private void button5_Click(object sender, EventArgs e)
        {
            int index = dgvStaffList.CurrentRow.Index;
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                var deleteSuccess = new StaffRepository().Delete(dgvStaffList.Rows[index].Cells[0].Value.ToString());
                LoadStaff();
                txbStaffID.Text = "";
                txbStaffName.Text = "";
                txbStaffPhone.Text = "";
                txbStaffEmail.Text = "";
                txbStaffAddress.Text = "";
                cbbStaffPosition.SelectedIndex = 2;
                if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // edit
        private void button3_Click(object sender, EventArgs e)
        {
            if (txbStaffID.Text != "" && txbStaffName.Text != "")
            {
                var regexItem = new Regex(@"(09|08|03|04|05|07)+([0-9]{8})\b");
                if (regexItem.IsMatch(txbStaffPhone.Text))
                {
                    var staff = new Staff() { Id = int.Parse(txbStaffID.Text.ToString()), Name = txbStaffName.Text, Address = txbStaffAddress.Text, Email = txbStaffEmail.Text, Phone = txbStaffPhone.Text, IdPosition = int.Parse(cbbStaffPosition.SelectedValue.ToString()) };
                    var updateSuccess = new StaffRepository().Edit(staff);
                    txbStaffName.Text = "";
                    txbStaffPhone.Text = "";
                    txbStaffAddress.Text = "";
                    txbStaffEmail.Text = "";
                    cbbStaffPosition.DataSource = new List<Staff>();
                    LoadStaff();
                }
                else
                {
                    MessageBox.Show("Phải điền đúng định dạng số điện thoại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Tên nhân viên và Id không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // clear
        private void button4_Click(object sender, EventArgs e)
        {
            txbStaffID.Text = "";
            txbStaffName.Text = "";
            txbStaffPhone.Text = "";
            txbStaffEmail.Text = "";
            txbStaffAddress.Text = "";
            cbbStaffPosition.SelectedIndex = 2;
        }


        private void dgvStaffList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvStaffList.CurrentRow.Index;

            txbStaffID.Text = dgvStaffList.Rows[index].Cells[0].Value.ToString();
            txbStaffName.Text = dgvStaffList.Rows[index].Cells[1].Value.ToString();
            txbStaffEmail.Text = dgvStaffList.Rows[index].Cells[2].Value.ToString();
            txbStaffAddress.Text = dgvStaffList.Rows[index].Cells[3].Value.ToString();
            txbStaffPhone.Text = dgvStaffList.Rows[index].Cells[4].Value.ToString();
            int i = 0;
            foreach (Position item in cbbStaffPosition.Items)
            {
                if (item.Name == dgvStaffList.Rows[index].Cells[5].Value.ToString())
                {
                    cbbStaffPosition.SelectedIndex = i;
                }
                i++;
            }
        }

        // search
        private void txbSearchStaff_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbSearchStaff.Text == "") LoadStaff();
            var staffsList = new StaffRepository().SearchByName(txbSearchStaff.Text);
            var positionList = new PositionRepository().GetList();
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(staffsList, "Id", "Name", "Email", "Address", "Phone", "IdPosition"))
            {
                dt.Load(reader);
            }

            //Change datatype of column PostionID
            DataTable dtCloned = dt.Clone();
            dtCloned.Columns[5].DataType = typeof(string);
            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }
            dgvStaffList.DataSource = dtCloned;
            dgvStaffList.Columns[0].HeaderText = "ID";
            dgvStaffList.Columns[1].HeaderText = "Tên Nhân Viên";
            dgvStaffList.Columns[2].HeaderText = "Email";
            dgvStaffList.Columns[3].HeaderText = "Địa chỉ";
            dgvStaffList.Columns[4].HeaderText = "Số điện thoại";
            dgvStaffList.Columns[5].HeaderText = "Chức vụ";
            // Change positonID to position Name
            for (int i = 0; i < dgvStaffList.RowCount; i++)
            {
                foreach (var position in positionList)
                {
                    if (dgvStaffList.Rows[i].Cells[5].Value.ToString() == position.Id.ToString())
                    {
                        dgvStaffList.Rows[i].Cells[5].Value = position.Name;
                    }
                }
            }
        }

        #endregion

        #region Account

        private void tpAccount_Enter(object sender, EventArgs e)
        {
            LoadAccount();
        }
        public void LoadAccount()
        {
            var accounstList = new AccountRepository().GetList();
            var staffsList = new StaffRepository().GetList();
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(accounstList, "UserName", "Password", "DisplayName", "IdStaff"))
            {
                dt.Load(reader);
            }

            //Change datatype of column PostionID
            DataTable dtCloned = dt.Clone();
            dtCloned.Columns[3].DataType = typeof(string);
            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }
            dgvAccount.DataSource = dtCloned;
            dgvAccount.Columns[0].HeaderText = "Tên đăng nhập";
            dgvAccount.Columns[1].HeaderText = "Mật khẩu";
            dgvAccount.Columns[2].HeaderText = "Tên hiển thị";
            dgvAccount.Columns[3].HeaderText = "Nhân Viên";
            // Change positonID to position Name
            for (int i = 0; i < dgvAccount.RowCount; i++)
            {
                foreach (var staff in staffsList)
                {
                    if (dgvAccount.Rows[i].Cells[3].Value.ToString() == staff.Id.ToString())
                    {
                        dgvAccount.Rows[i].Cells[3].Value = staff.Name;
                    }
                }
            }


            cbbAccountStaff.DataSource = staffsList;
            cbbAccountStaff.DisplayMember = "Name";
            cbbAccountStaff.ValueMember = "Id";
        }

        private void btnAccountAdd_Click(object sender, EventArgs e)
        {
            if (txbAccountUsername.Text != "" && txbAccountPass.Text != "" && txbAccountPassRe.Text != "" && txbAccountDisplayName.Text != "")
            {
                var regexItem = new Regex(@"^[a-zA-Z0-9]*$");
                if (regexItem.IsMatch(txbAccountUsername.Text) && regexItem.IsMatch(txbAccountPass.Text))
                {
                    var checkExits = new AccountRepository().CheckExits(txbAccountUsername.Text);
                    if (checkExits)
                    {
                        if (txbAccountPass.Text == txbAccountPassRe.Text)
                        {
                            var account = new Account() { UserName = txbAccountUsername.Text, Password = txbAccountPass.Text, DisplayName = txbAccountDisplayName.Text, IdStaff = int.Parse(cbbAccountStaff.SelectedValue.ToString()) };

                            var addSuccess = new AccountRepository().Add(account);
                            if (addSuccess == 0) MessageBox.Show("Thêm nhân viên không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                            {
                                txbAccountUsername.Text = "";
                                txbAccountPass.Text = "";
                                txbAccountPassRe.Text = "";
                                txbAccountDisplayName.Text = "";
                                cbbAccountStaff.DataSource = new List<Staff>();
                                LoadAccount();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Nhập lại mật khẩu không trùng khớp !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại trong hệ thống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                else
                {
                    MessageBox.Show("Không được sử dụng ký tự đặc biệt trong Tên đăng nhập và Mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            else
            {
                MessageBox.Show("Không được để trống thông tin nào !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAccountDelete_Click(object sender, EventArgs e)
        {
            int index = dgvAccount.CurrentRow.Index;
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (dgvAccount.Rows[index].Cells[0].Value.ToString() != "admin")
                {
                    var deleteSuccess = new AccountRepository().Delete(dgvAccount.Rows[index].Cells[0].Value.ToString());
                    LoadAccount();
                    txbAccountUsername.Text = "";
                    txbAccountPass.Text = "";
                    txbAccountPassRe.Text = "";
                    txbAccountDisplayName.Text = "";
                    cbbAccountStaff.SelectedIndex = 0; 
                    
                    if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Không được xóa tài khoản Admin !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnAccountReset_Click(object sender, EventArgs e)
        {
            txbAccountUsername.Text = "";
            txbAccountPass.Text = "";
            txbAccountPassRe.Text = "";
            txbAccountDisplayName.Text = "";
            cbbAccountStaff.SelectedIndex = 0;
        }

        #endregion

        #region Member

        private void tpMember_Enter(object sender, EventArgs e)
        {
            LoadMember();
        }
        public void LoadMember()
        {
            var membersList = new MemberRepository().GetList();

            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(membersList, "Id", "Name"))
            {
                dt.Load(reader);
            }

            dgvMember.DataSource = dt;
            dgvMember.Columns[0].HeaderText = "Id";
            dgvMember.Columns[1].HeaderText = "Loại thành viên";


        }

        private void btnMemberAdd_Click(object sender, EventArgs e)
        {
            if (txbMemberName.Text != "")
            {
                var member = new Models.Entity.Member() { Name = txbMemberName.Text };

                var addSuccess = new MemberRepository().Add(member);
                if (addSuccess == 0) MessageBox.Show("Thêm không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbMemberName.Text = "";
                LoadMember();
            }
            else
            {
                MessageBox.Show("Loại thành viên không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMemberDelete_Click(object sender, EventArgs e)
        {
            int index = dgvMember.CurrentRow.Index;
            var customerList = new CustomerRepository().GetList();
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (!customerList.Select(x => x.IdMember).Contains(int.Parse(dgvMember.Rows[index].Cells[0].Value.ToString())))
                {
                    var deleteSuccess = new MemberRepository().Delete(dgvMember.Rows[index].Cells[0].Value.ToString());
                    LoadMember();
                    txbMemberId.Text = "";
                    txbMemberName.Text = ""; 
                    if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Vui lòng xóa nhân viên có loại thành viên này !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        private void btnMemberEdit_Click(object sender, EventArgs e)
        {
            if (txbMemberId.Text != "" && txbMemberName.Text != "")
            {
                var member = new Models.Entity.Member() { Id = int.Parse(txbMemberId.Text.ToString()), Name = txbMemberName.Text };
                var updateSuccess = new MemberRepository().Edit(member);
                txbMemberId.Text = "";
                txbMemberName.Text = "";
                LoadMember();
            }
            else
            {
                MessageBox.Show("Loại thành viên và Id không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMember_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvMember.CurrentRow.Index;

            txbMemberId.Text = dgvMember.Rows[index].Cells[0].Value.ToString();
            txbMemberName.Text = dgvMember.Rows[index].Cells[1].Value.ToString();
        }

        private void btnMemberReset_Click(object sender, EventArgs e)
        {
            txbMemberId.Text = "";
            txbMemberName.Text = "";
        }
        #endregion  

        #region Customer
        private void tpCustomer_Enter(object sender, EventArgs e)
        {
            LoadCustomer();
        }
        public void LoadCustomer()
        {
            var customersList = new CustomerRepository().GetList();
            var membersList = new MemberRepository().GetList();
            if(membersList.Count != 0)
            {
                DataTable dt = new DataTable();
                using (var reader = ObjectReader.Create(customersList, "Id", "Name", "Email", "Address", "Phone", "IdMember"))
                {
                    dt.Load(reader);
                }

                //Change datatype of column PostionID
                DataTable dtCloned = dt.Clone();
                dtCloned.Columns[5].DataType = typeof(string);
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                dgvCustomer.DataSource = dtCloned;
                dgvCustomer.Columns[0].HeaderText = "ID";
                dgvCustomer.Columns[1].HeaderText = "Tên Khách Hàng";
                dgvCustomer.Columns[2].HeaderText = "Email";
                dgvCustomer.Columns[3].HeaderText = "Địa chỉ";
                dgvCustomer.Columns[4].HeaderText = "Số điện thoại";
                dgvCustomer.Columns[5].HeaderText = "Loại Thành Viên";
                // Change positonID to position Name
                for (int i = 0; i < dgvCustomer.RowCount; i++)
                {
                    foreach (var member in membersList)
                    {
                        if (dgvCustomer.Rows[i].Cells[5].Value.ToString() == member.Id.ToString())
                        {
                            dgvCustomer.Rows[i].Cells[5].Value = member.Name;
                        }
                    }
                }
                cbbCustomerMember.DataSource = membersList;
                cbbCustomerMember.DisplayMember = "Name";
                cbbCustomerMember.ValueMember = "Id";
            }
            else
            {
                MessageBox.Show("Vui lòng thêm thành viên trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tpMember.Select();
            }
        }


        private void btnCustomerRemove_Click(object sender, EventArgs e)
        {
            int index = dgvCustomer.CurrentRow.Index;
            var ordersList = new OrderRepository().GetList();
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (!ordersList.Select(x => x.IdCustomer).Contains(int.Parse(dgvCustomer.Rows[index].Cells[0].Value.ToString())))
                {
                    var deleteSuccess = new CustomerRepository().Delete(dgvCustomer.Rows[index].Cells[0].Value.ToString());
                    LoadCustomer();
                    txbCustomerId.Text = "";
                    txbCustomerName.Text = "";
                    txbCustomerPhone.Text = "";
                    txbCustomerEmail.Text = "";
                    txbCustomerAddress.Text = "";
                    txbCustomerSearch.Text = "";
                    if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Vui lòng xóa hóa đơn có loại thành viên này !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnCustomerEdit_Click(object sender, EventArgs e)
        {
            if (txbCustomerId.Text != "" && txbCustomerName.Text != "")
            {
                var regexItem = new Regex(@"(09|08|03|04|05|07)+([0-9]{8})\b");
                if (regexItem.IsMatch(txbCustomerPhone.Text))
                {
                    var customer = new Customer() { Id = int.Parse(txbCustomerId.Text.ToString()), Name = txbCustomerName.Text, Address = txbCustomerAddress.Text, Email = txbCustomerEmail.Text, Phone = txbCustomerPhone.Text, IdMember = int.Parse(cbbCustomerMember.SelectedValue.ToString()) };
                    var updateSuccess = new CustomerRepository().Edit(customer);
                    txbCustomerName.Text = "";
                    txbCustomerPhone.Text = "";
                    txbCustomerAddress.Text = "";
                    txbCustomerEmail.Text = "";
                    cbbCustomerMember.DataSource = new List<Models.Entity.Member>();
                    LoadCustomer();
                }
                else
                {
                    MessageBox.Show("Phải điền đúng định dạng số điện thoại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Tên nhân viên và Id không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvCustomer.CurrentRow.Index;

            txbCustomerId.Text = dgvCustomer.Rows[index].Cells[0].Value.ToString();
            txbCustomerName.Text = dgvCustomer.Rows[index].Cells[1].Value.ToString();
            txbCustomerEmail.Text = dgvCustomer.Rows[index].Cells[2].Value.ToString();
            txbCustomerAddress.Text = dgvCustomer.Rows[index].Cells[3].Value.ToString();
            txbCustomerPhone.Text = dgvCustomer.Rows[index].Cells[4].Value.ToString();
            int i = 0;
            foreach (Member item in cbbCustomerMember.Items)
            {
                if (item.Name == dgvCustomer.Rows[index].Cells[5].Value.ToString())
                {
                    cbbCustomerMember.SelectedIndex = i;
                }
                i++;
            }
        }

        private void btnCustomerReset_Click(object sender, EventArgs e)
        {
            txbCustomerId.Text = "";
            txbCustomerName.Text = "";
            txbCustomerPhone.Text = "";
            txbCustomerEmail.Text = "";
            txbCustomerAddress.Text = "";
            cbbCustomerMember.SelectedIndex = 0;
        }

        private void txbCustomerSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbCustomerSearch.Text == "") LoadCustomer();
            var customersList = new CustomerRepository().SearchByName(txbCustomerSearch.Text);
            var membersList = new MemberRepository().GetList();
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(customersList, "Id", "Name", "Email", "Address", "Phone", "IdMember"))
            {
                dt.Load(reader);
            }

            //Change datatype of column PostionID
            DataTable dtCloned = dt.Clone();
            dtCloned.Columns[5].DataType = typeof(string);
            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }
            dgvCustomer.DataSource = dtCloned;
            dgvCustomer.Columns[0].HeaderText = "ID";
            dgvCustomer.Columns[1].HeaderText = "Tên Nhân Viên";
            dgvCustomer.Columns[2].HeaderText = "Email";
            dgvCustomer.Columns[3].HeaderText = "Địa chỉ";
            dgvCustomer.Columns[4].HeaderText = "Số điện thoại";
            dgvCustomer.Columns[5].HeaderText = "Thành viên";
            // Change positonID to position Name
            for (int i = 0; i < dgvCustomer.RowCount; i++)
            {
                foreach (var member in membersList)
                {
                    if (dgvCustomer.Rows[i].Cells[5].Value.ToString() == member.Id.ToString())
                    {
                        dgvCustomer.Rows[i].Cells[5].Value = member.Name;
                    }
                }
            }
        }

        #endregion

        #region ProductCategory
        public void LoadProductCategory()
        {
            var productCategoryList = new ProductCategoryRepository().GetList();

            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(productCategoryList, "Id", "Name"))
            {
                dt.Load(reader);
            }   

            dgvProductCategory.DataSource = dt;
            dgvProductCategory.Columns[0].HeaderText = "Id";
            dgvProductCategory.Columns[1].HeaderText = "Tên danh mục";
        }

        private void tpProductCategory_Enter(object sender, EventArgs e)
        {
            LoadProductCategory();
        }

        private void btnProductCategoryAdd_Click(object sender, EventArgs e)
        {
            if (txbProductCategoryName.Text != "")
            {
                var productCategory = new ProductCategory() { Name = txbProductCategoryName.Text };
                var addSuccess = new ProductCategoryRepository().Add(productCategory);
                if (addSuccess == 0) MessageBox.Show("Thêm không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbProductCategoryName.Text = "";
                LoadProductCategory();
            }
            else
            {
                MessageBox.Show("Tên danh mục không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProductCategoryRemove_Click(object sender, EventArgs e)
        {
            int index = dgvProductCategory.CurrentRow.Index;
            var productsList = new ProductRepository().GetList();
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (!productsList.Select(x => x.IdCategory).Contains(int.Parse(dgvProductCategory.Rows[index].Cells[0].Value.ToString())))
                {
                    var deleteSuccess = new ProductCategoryRepository().Delete(dgvProductCategory.Rows[index].Cells[0].Value.ToString());
                    LoadProductCategory();
                    txbProductCategoryId.Text = "";
                    txbProductCategoryName.Text = "";
                    txbProductCategorySearch.Text = "";
                    if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Vui lòng xóa sản phẩm của danh mục này trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnProductCategoryEdit_Click(object sender, EventArgs e)
        {
            if (txbProductCategoryId.Text != "" && txbProductCategoryName.Text != "")
            {

                var productCategory = new ProductCategory() { Id = int.Parse(txbProductCategoryId.Text.ToString()), Name = txbProductCategoryName.Text};
                var updateSuccess = new ProductCategoryRepository().Edit(productCategory);
                txbProductCategoryId.Text = "";
                txbProductCategoryName.Text = "";
                LoadProductCategory();
            }
            else
            {
                MessageBox.Show("Tên danh mục và Id không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProductCategoryReset_Click(object sender, EventArgs e)
        {
            txbProductCategoryId.Text = "";
            txbProductCategoryName.Text = "";
            txbProductCategorySearch.Text = "";
        }

        private void dgvProductCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvProductCategory.CurrentRow.Index;

            txbProductCategoryId.Text = dgvProductCategory.Rows[index].Cells[0].Value.ToString();
            txbProductCategoryName.Text = dgvProductCategory.Rows[index].Cells[1].Value.ToString();

        }

        private void txbProductCategorySearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbProductCategorySearch.Text == "") LoadProductCategory();
            var productCategories = new ProductCategoryRepository().SearchByName(txbProductCategorySearch.Text);
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(productCategories, "Id", "Name"))
            {
                dt.Load(reader);
            }

            dgvProductCategory.DataSource = dt;
            dgvProductCategory.Columns[0].HeaderText = "Id";
            dgvProductCategory.Columns[1].HeaderText = "Tên danh mục";
        }
        #endregion

        #region Supplier
        public void LoadSupplier()
        {
            var supplierList = new SupplierRepository().GetList();
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(supplierList, "Id", "Name", "Address", "Phone", "Email", "SupplierProduct"))
            {
                dt.Load(reader);
            }
            dgvSuppList.DataSource = dt;
            dgvSuppList.Columns[0].HeaderText = "ID";
            dgvSuppList.Columns[1].HeaderText = "Tên nhà cung cấp";
            dgvSuppList.Columns[2].HeaderText = "Địa chỉ";
            dgvSuppList.Columns[3].HeaderText = "Số điện thoại";
            dgvSuppList.Columns[4].HeaderText = "Email";
            dgvSuppList.Columns[5].HeaderText = "Sản phẩm cung cấp";

        }
        private void tpSupplier_Enter(object sender, EventArgs e)
        {
            LoadSupplier();
        }

        private void btnSuppAdd_Click(object sender, EventArgs e)
        {
            if (txbSuppName.Text != "")
            {
                var regexItem = new Regex(@"(09|08|03|04|05|07)+([0-9]{8})\b");
                if (regexItem.IsMatch(txbSuppPhone.Text))
                {
                    var supp = new Supplier() { Name = txbSuppName.Text, Address = txbSuppAddress.Text, Phone = txbSuppPhone.Text, Email = txbSuppMail.Text, SupplierProduct = txbSuppProduct.Text };

                    var addSuccess = new SupplierRepository().Add(supp);
                    if (addSuccess == 0) MessageBox.Show("Thêm nhà cung cấp không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txbSuppName.Text = "";
                    txbSuppAddress.Text = "";
                    txbSuppPhone.Text = "";
                    txbSuppMail.Text = "";
                    txbSuppProduct.Text = "";
                    LoadSupplier();
                }
                else
                {
                    MessageBox.Show("Số điện thoại phải đúng định dạng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Tên nhà cung cấp không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuppDelete_Click(object sender, EventArgs e)
        {
            int index = dgvSuppList.CurrentRow.Index;
            var productsList = new ProductRepository().GetList();
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (!productsList.Select(x => x.IdSupplier).Contains(int.Parse(dgvProductCategory.Rows[index].Cells[0].Value.ToString())))
                {
                    var deleteSuccess = new SupplierRepository().Delete(dgvSuppList.Rows[index].Cells[0].Value.ToString());
                    LoadSupplier();
                    if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txbSuppID.Text = "";
                    txbSuppName.Text = "";
                    txbSuppAddress.Text = "";
                    txbSuppPhone.Text = "";
                    txbSuppMail.Text = "";
                    txbSuppProduct.Text = "";
                    txbSuppSearch.Text = "";
                }
                else
                {
                    MessageBox.Show("Vui lòng xóa sản phẩm nhập từ nhà cung cấp này trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSuppEdit_Click(object sender, EventArgs e)
        {
            if (txbSuppID.Text != "" && txbSuppName.Text != "")
            {
                var regexItem = new Regex(@"(09|08|03|04|05|07)+([0-9]{8})\b");
                if (regexItem.IsMatch(txbSuppPhone.Text))
                {
                    var supp = new Supplier() { Id = int.Parse(txbSuppID.Text.ToString()), Name = txbSuppName.Text, Address = txbSuppAddress.Text, Phone = txbSuppPhone.Text, Email = txbSuppMail.Text, SupplierProduct = txbSuppProduct.Text };
                    var updateSuccess = new SupplierRepository().Edit(supp);
                    txbSuppName.Text = "";
                    txbSuppAddress.Text = "";
                    txbSuppPhone.Text = "";
                    txbSuppMail.Text = "";
                    txbSuppProduct.Text = "";
                    LoadSupplier();
                }
                else
                {
                    MessageBox.Show("Số điện thoại phải đúng định dạng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Tên nhà cung cấp và Id không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuppReset_Click(object sender, EventArgs e)
        {
            txbSuppID.Text = "";
            txbSuppName.Text = "";
            txbSuppAddress.Text = "";
            txbSuppPhone.Text = "";
            txbSuppMail.Text = "";
            txbSuppProduct.Text = "";
        }

        private void dgvSuppList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvSuppList.CurrentRow.Index;

            txbSuppID.Text = dgvSuppList.Rows[index].Cells[0].Value.ToString();
            txbSuppName.Text = dgvSuppList.Rows[index].Cells[1].Value.ToString();
            txbSuppAddress.Text = dgvSuppList.Rows[index].Cells[2].Value.ToString();
            txbSuppPhone.Text = dgvSuppList.Rows[index].Cells[3].Value.ToString();
            txbSuppMail.Text = dgvSuppList.Rows[index].Cells[4].Value.ToString();
            txbSuppProduct.Text = dgvSuppList.Rows[index].Cells[5].Value.ToString();
        }

        private void txbSuppSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbSuppSearch.Text == "") LoadSupplier();
            var suppList = new SupplierRepository().SearchByName(txbSuppSearch.Text);
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(suppList, "Id", "Name", "Address", "Phone", "Email", "SupplierProduct"))
            {
                dt.Load(reader);
            }
            dgvSuppList.DataSource = dt;
            dgvSuppList.Columns[0].HeaderText = "ID";
            dgvSuppList.Columns[1].HeaderText = "Tên nhà cung cấp";
            dgvSuppList.Columns[2].HeaderText = "Địa chỉ";
            dgvSuppList.Columns[3].HeaderText = "Số điện thoại";
            dgvSuppList.Columns[4].HeaderText = "Email";
            dgvSuppList.Columns[5].HeaderText = "Sản phẩm cung cấp";
        }



        #endregion

        #region Promotion
        public void LoadPromotion()
        {
            var promotions = new PromotionRepository().GetList();

            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(promotions, "Id", "Title", "Coupon", "Quantity","PromotionPrice"))
            {
                dt.Load(reader);    
            }

            dgvPromotion.DataSource = dt;
            dgvPromotion.Columns[0].HeaderText = "Id";
            dgvPromotion.Columns[1].HeaderText = "Tên khuyến mãi";
            dgvPromotion.Columns[2].HeaderText = "Mã khuyến mãi";
            dgvPromotion.Columns[3].HeaderText = "Số lượng";
            dgvPromotion.Columns[4].HeaderText = "Giá khuyến mãi";
        }
        private void tpPromotion_Enter(object sender, EventArgs e)
        {
            LoadPromotion();
        }

        private void btnPromotionAdd_Click(object sender, EventArgs e)
        {
            if (txbPromotionTitle.Text != "" && txbPromotionPrice.Text != "" && txbPromotionQuantity.Text != "" && txbPromotionCoupon.Text != "")
            {
                var checkExits = new PromotionRepository().CheckExits(txbPromotionCoupon.Text);
                if(checkExits)
                {
                    var promotion = new Promotion() { Title = txbPromotionTitle.Text, Coupon = txbPromotionCoupon.Text, Quantity = (int)txbPromotionQuantity.Value, PromotionPrice = (int)txbPromotionPrice.Value };

                    var addSuccess = new PromotionRepository().Add(promotion);
                    if (addSuccess == 0) MessageBox.Show("Thêm khuyến mãi không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadPromotion();
                    txbPromotionTitle.Text = "";
                    txbPromotionCoupon.Text = "";
                    txbPromotionQuantity.Value = 0;
                    txbPromotionPrice.Value = 0;
                }
                else
                {
                    MessageBox.Show("Mã khuyến mãi đã tồn tại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Không được để trống bất cứ thông tin nào !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPromotionRemove_Click(object sender, EventArgs e)
        {
            int index = dgvPromotion.CurrentRow.Index;
            var promotionsList = new PromotionRepository().GetList();
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                var deleteSuccess = new PromotionRepository().Delete(dgvPromotion.Rows[index].Cells[0].Value.ToString());
                LoadPromotion();
                if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPromotionID.Text = "";
                txbPromotionTitle.Text = "";
                txbPromotionCoupon.Text = "";
                txbPromotionQuantity.Value = 0;
                txbPromotionPrice.Value = 0;
                txbPromotionSearch.Text = "";
            }
        }
        private void btnPromotionEdit_Click(object sender, EventArgs e)
        {
            if (txbPromotionTitle.Text != "" && txbPromotionPrice.Text != "" && txbPromotionQuantity.Text != "" && txbPromotionCoupon.Text != "")
            {
                var checkExits = new PromotionRepository().CheckExits(txbPromotionCoupon.Text);
                if (checkExits)
                {
                    var promotion = new Promotion() { Id = int.Parse(txbPromotionID.Text.ToString()), Title = txbPromotionTitle.Text, Coupon = txbPromotionCoupon.Text, Quantity = (int)txbPromotionQuantity.Value, PromotionPrice = (int)txbPromotionPrice.Value };
                    var updateSuccess = new PromotionRepository().Edit(promotion);
                    LoadPromotion();
                    txbPromotionID.Text = "";
                    txbPromotionTitle.Text = "";
                    txbPromotionCoupon.Text = "";
                    txbPromotionQuantity.Value = 0;
                    txbPromotionPrice.Value = 0;
                }
                else
                {
                    MessageBox.Show("Mã khuyến mãi đã tồn tại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Tên nhà cung cấp và Id không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPromotion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvPromotion.CurrentRow.Index;

            txbPromotionID.Text = dgvPromotion.Rows[index].Cells[0].Value.ToString();
            txbPromotionTitle.Text = dgvPromotion.Rows[index].Cells[1].Value.ToString();
            txbPromotionCoupon.Text = dgvPromotion.Rows[index].Cells[2].Value.ToString();
            txbPromotionQuantity.Text = dgvPromotion.Rows[index].Cells[3].Value.ToString();
            txbPromotionPrice.Text = dgvPromotion.Rows[index].Cells[4].Value.ToString();
        }
        private void txbPromotionSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbPromotionSearch.Text == "") LoadPromotion();
            var promotions = new PromotionRepository().SearchByCoupon(txbPromotionSearch.Text);
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(promotions, "Id", "Title", "Coupon", "Quantity", "PromotionPrice"))
            {
                dt.Load(reader);
            }

            dgvPromotion.DataSource = dt;
            dgvPromotion.Columns[0].HeaderText = "Id";
            dgvPromotion.Columns[1].HeaderText = "Tên khuyến mãi";
            dgvPromotion.Columns[2].HeaderText = "Mã khuyến mãi";
            dgvPromotion.Columns[3].HeaderText = "Số lượng";
            dgvPromotion.Columns[4].HeaderText = "Giá khuyến mãi";
        }
        private void btnPromotionReset_Click(object sender, EventArgs e)
        {
            txbPromotionID.Text = "";
            txbPromotionTitle.Text = "";
            txbPromotionCoupon.Text = "";
            txbPromotionQuantity.Value = 0;
            txbPromotionSearch.Text = "";
            txbPromotionPrice.Value = 0;
        }


        #endregion

        #region Color
        public void LoadColor()
        {
            var colorList = new ColorRepository().GetList();
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(colorList, "Id", "Name"))
            {
                dt.Load(reader);
            }

            dgvColor.DataSource = dt;
            dgvColor.Columns[0].HeaderText = "ID";
            dgvColor.Columns[1].HeaderText = "Màu sắc";


        }

        private void tpColor_Enter(object sender, EventArgs e)
        {
            LoadColor();
        }

        private void btnColorAdd_Click(object sender, EventArgs e)
        {

            if (txbColorName.Text != "")
            {
                var color = new Models.Entity.Color() { Name = txbColorName.Text };

                var addSuccess = new ColorRepository().Add(color);
                if (addSuccess == 0) MessageBox.Show("Thêm màu sắc không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbColorId.Text = "";
                txbColorName.Text = "";
                LoadColor();

            }
            else
            {
                MessageBox.Show("Tên màu sắc không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnColorRemove_Click(object sender, EventArgs e)
        {
            int index = dgvColor.CurrentRow.Index;
            var productsList = new ProductRepository().GetList();
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (!productsList.Select(x => x.IdColor).Contains(int.Parse(dgvProductCategory.Rows[index].Cells[0].Value.ToString())))
                {
                    var deleteSuccess = new ColorRepository().Delete(dgvColor.Rows[index].Cells[0].Value.ToString());
                    LoadColor();
                    if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Cần xóa sản phẩm có màu sắc này trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                    
            }
        }

        private void btnColorEdit_Click(object sender, EventArgs e)
        {
            if (txbColorId.Text != "" && txbColorName.Text != "")
            {
                var color = new Models.Entity.Color() { Id = int.Parse(txbColorId.Text.ToString()), Name = txbColorName.Text };
                var updateSuccess = new ColorRepository().Edit(color);
                txbColorId.Text = "";
                txbColorName.Text = "";
                LoadColor();
            }
            else
            {
                MessageBox.Show("Tên khuyến mãi và Id không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnColorReset_Click(object sender, EventArgs e)
        {
            txbColorId.Text = "";
            txbColorName.Text = "";
            
        }

        private void dgvColor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvColor.CurrentRow.Index;

            txbColorId.Text = dgvColor.Rows[index].Cells[0].Value.ToString();
            txbColorName.Text = dgvColor.Rows[index].Cells[1].Value.ToString();
        }
        #endregion

        #region Product
        public void LoadProduct()
        {
            var productLists = new ProductRepository().GetList();
            var categoryList = new ProductCategoryRepository().GetList();
            var promotionList = new PromotionRepository().GetList();
            var suppliersList = new SupplierRepository().GetList();
            var colorsList = new ColorRepository().GetList();
            if (categoryList.Count != 0)
            {
                if (suppliersList.Count != 0)
                {
                    if (colorsList.Count != 0)
                    {
                        DataTable dt = new DataTable();
                        using (var reader = ObjectReader.Create(productLists, "Id", "Name", "Quantity", "Price", "IdCategory", "IdPromotion", "IdSupplier", "IdColor"))
                        {
                            dt.Load(reader);
                        }

                        //Change datatype of column PostionID
                        DataTable dtCloned = dt.Clone();
                        dtCloned.Columns[4].DataType = typeof(string);
                        dtCloned.Columns[5].DataType = typeof(string);
                        dtCloned.Columns[6].DataType = typeof(string);
                        dtCloned.Columns[7].DataType = typeof(string);
                        foreach (DataRow row in dt.Rows)
                        {
                            dtCloned.ImportRow(row);
                        }
                        dgvProduct.DataSource = dtCloned;
                        dgvProduct.Columns[0].HeaderText = "ID";
                        dgvProduct.Columns[1].HeaderText = "Tên sản phẩm";
                        dgvProduct.Columns[2].HeaderText = "Số lượng";
                        dgvProduct.Columns[3].HeaderText = "Giá sản phẩm";
                        dgvProduct.Columns[4].HeaderText = "Danh mục";
                        dgvProduct.Columns[5].HeaderText = "Khuyến mãi";
                        dgvProduct.Columns[6].HeaderText = "Nhà cung cấp";
                        dgvProduct.Columns[7].HeaderText = "Màu sắc";
                        // Change positonID to position Name
                        for (int i = 0; i < dgvProduct.RowCount; i++)
                        {
                            foreach (var category in categoryList)
                            {
                                if (dgvProduct.Rows[i].Cells[4].Value.ToString() == category.Id.ToString())
                                {
                                    dgvProduct.Rows[i].Cells[4].Value = category.Name;
                                }
                            }
                            foreach (var promotion in promotionList)
                            {
                                if (dgvProduct.Rows[i].Cells[5].Value.ToString() == promotion.Id.ToString())
                                {
                                    dgvProduct.Rows[i].Cells[5].Value = promotion.Coupon;
                                }
                            }
                            foreach (var supplier in suppliersList)
                            {
                                if (dgvProduct.Rows[i].Cells[6].Value.ToString() == supplier.Id.ToString())
                                {
                                    dgvProduct.Rows[i].Cells[6].Value = supplier.Name;
                                }
                            }
                            foreach (var color in colorsList)
                            {
                                if (dgvProduct.Rows[i].Cells[7].Value.ToString() == color.Id.ToString())
                                {
                                    dgvProduct.Rows[i].Cells[7].Value = color.Name;
                                }
                            }
                        }
                        cbbProductCategory.DataSource = categoryList;
                        cbbProductCategory.DisplayMember = "Name";
                        cbbProductCategory.ValueMember = "Id";

                        if(promotionList.Count != 0)
                        {
                            Dictionary<int, string> cbbProductPromotionSource = new Dictionary<int, string>();
                            cbbProductPromotionSource.Add(0, "");
                            foreach (Promotion item in promotionList)
                            {
                                cbbProductPromotionSource.Add(item.Id, item.Coupon);
                            }

                            cbbProductPromotion.DataSource = new BindingSource(cbbProductPromotionSource,null);
                            cbbProductPromotion.DisplayMember = "Value";
                            cbbProductPromotion.ValueMember = "Key";

                        }
                        else
                        {
                            Dictionary<int, string> cbbProductPromotionSource = new Dictionary<int, string>();
                            cbbProductPromotionSource.Add(0, "");
                            cbbProductPromotion.DataSource = new BindingSource(cbbProductPromotionSource, null);
                            cbbProductPromotion.DisplayMember = "Value";
                            cbbProductPromotion.ValueMember = "Key";
                        }
                        

                        cbbProductSupplier.DataSource = suppliersList;
                        cbbProductSupplier.DisplayMember = "Name";
                        cbbProductSupplier.ValueMember = "Id";

                        cbbProductColor.DataSource = colorsList;
                        cbbProductColor.DisplayMember = "Name";
                        cbbProductColor.ValueMember = "Id";
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng thêm màu sắc sản phẩm trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tpMember.Select();
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng thêm nhà cung cấp sản phẩm trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tpMember.Select();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng thêm danh mục sản phẩm trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tpMember.Select();
            }
        }

        private void tpProduct_Enter(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            if (txbProductName.Text != "" && txbProductId.Text != "")
            {
                var checkExits = new ProductRepository().CheckExits(txbProductId.Text);
                if(checkExits)
                {

                    var product = new Product() { Id = txbProductId.Text, Name = txbProductName.Text, Price = (int)nmProductPrice.Value, IdCategory = int.Parse(cbbProductCategory.SelectedValue.ToString()),IdPromotion = null , IdColor = int.Parse(cbbProductColor.SelectedValue.ToString()), IdSupplier = int.Parse(cbbProductSupplier.SelectedValue.ToString()) };
                    if (cbbProductPromotion.SelectedIndex  != 0)
                    {
                        product = new Product() { Id = txbProductId.Text, Name = txbProductName.Text, Price = (int)nmProductPrice.Value, IdCategory = int.Parse(cbbProductCategory.SelectedValue.ToString()), IdPromotion = int.Parse(cbbProductPromotion.SelectedValue.ToString()), IdColor = int.Parse(cbbProductColor.SelectedValue.ToString()), IdSupplier = int.Parse(cbbProductSupplier.SelectedValue.ToString()) };
                    }

                    var addSuccess = new ProductRepository().Add(product);
                    if (addSuccess == 0) MessageBox.Show("Thêm sản phẩm không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txbProductId.Text = "";
                    txbProductName.Text = "";
                    nmProductPrice.Value = 0;
                    cbbProductCategory.DataSource = new List<ProductCategory>();
                    cbbProductColor.DataSource = new List<Models.Entity.Color>();
                    cbbProductSupplier.DataSource = new List<Supplier>();
                    cbbProductPromotion.DataSource = new List<Promotion>(); ;
                    LoadProduct();
                }
                else
                {
                    MessageBox.Show("ID sản phẩm đã tồn tại trong hệ thống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
            }
            else
            {
                MessageBox.Show("ID và tên sản phẩm không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProductRemove_Click(object sender, EventArgs e)
        {
            int index = dgvProduct.CurrentRow.Index;
            var orderInformationList = new OrderInformationRepository().GetList();
            var importInvoiceInfoList = new ImportInvoiceInformationRepository().GetList();
            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (!orderInformationList.Select(x => x.IdProduct).Contains(dgvProduct.Rows[index].Cells[0].Value.ToString()))
                {
                    if (!importInvoiceInfoList.Select(x => x.IdProduct).Contains(dgvProduct.Rows[index].Cells[0].Value.ToString()))
                    {
                        var deleteSuccess = new ProductRepository().Delete(dgvProduct.Rows[index].Cells[0].Value.ToString());
                        txbProductId.Text = "";
                        txbProductName.Text = "";
                        txbProductSearch.Text = "";
                        nmProductPrice.Value = 0;
                        cbbProductCategory.DataSource = new List<ProductCategory>();
                        cbbProductColor.DataSource = new List<Models.Entity.Color>();
                        cbbProductSupplier.DataSource = new List<Supplier>();
                        cbbProductPromotion.DataSource = new List<Promotion>();
                        LoadProduct();
                        if (!deleteSuccess) MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Phải xóa sản phẩm trong thông tin hóa đơn nhập trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Phải xóa sản phẩm trong thông tin đơn hàng trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnProductEdit_Click(object sender, EventArgs e)
        {
            if (txbProductId.Text != "" && txbProductName.Text != "")
            {
                var checkExits = new ProductRepository().CheckExits(txbProductId.Text);
                if (!checkExits)
                {
                    var product = new Product() { Id = txbProductId.Text, Name = txbProductName.Text, Price = (int)nmProductPrice.Value, IdCategory = int.Parse(cbbProductCategory.SelectedValue.ToString()), IdPromotion = null, IdColor = int.Parse(cbbProductColor.SelectedValue.ToString()), IdSupplier = int.Parse(cbbProductSupplier.SelectedValue.ToString()) };
                    if (cbbProductPromotion.SelectedIndex != 0)
                    {
                        product = new Product() { Id = txbProductId.Text, Name = txbProductName.Text, Price = (int)nmProductPrice.Value, IdCategory = int.Parse(cbbProductCategory.SelectedValue.ToString()), IdPromotion = int.Parse(cbbProductPromotion.SelectedValue.ToString()), IdColor = int.Parse(cbbProductColor.SelectedValue.ToString()), IdSupplier = int.Parse(cbbProductSupplier.SelectedValue.ToString()) };
                    }
                    var updateSuccess = new ProductRepository().Edit(product);
                    if(!updateSuccess) MessageBox.Show("Chỉnh sửa sản phẩm không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txbProductId.Text = "";
                    txbProductName.Text = "";
                    nmProductPrice.Value = 0;
                    cbbProductCategory.DataSource = new List<ProductCategory>();
                    cbbProductColor.DataSource = new List<Models.Entity.Color>();
                    cbbProductSupplier.DataSource = new List<Supplier>();
                    cbbProductPromotion.DataSource = new List<Promotion>(); ;
                    LoadProduct();
                }
                else
                {
                    MessageBox.Show("Mã sản phẩm không tồn tại trong hệ thống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Tên nhân viên và Id không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvProduct.CurrentRow.Index;

            txbProductId.Text = dgvProduct.Rows[index].Cells[0].Value.ToString();
            txbProductName.Text = dgvProduct.Rows[index].Cells[1].Value.ToString();
            nmProductPrice.Value = int.Parse(dgvProduct.Rows[index].Cells[3].Value.ToString());

            int indexCategory = 0;
            foreach (ProductCategory item in cbbProductCategory.Items)
            {
                if (item.Name == dgvProduct.Rows[index].Cells[4].Value.ToString())
                {
                    cbbProductCategory.SelectedIndex = indexCategory;
                }
                indexCategory++;
            }
            if (cbbProductPromotion.Items.Count != 0)
            {
                int indexPromotion = 0;
                foreach (KeyValuePair<int, string> item in cbbProductPromotion.Items)
                {
                    if (item.Value.ToString() == dgvProduct.Rows[index].Cells[5].Value.ToString())
                    {
                        cbbProductPromotion.SelectedIndex = indexPromotion;
                    }
                    indexPromotion++;
                }
            }

            int indexSupplier = 0;
            foreach (Supplier item in cbbProductSupplier.Items)
            {
                if (item.Name == dgvProduct.Rows[index].Cells[6].Value.ToString())
                {
                    cbbProductSupplier.SelectedIndex = indexSupplier;
                }
                indexSupplier++;
            }

            int indexColor = 0;
            foreach (Color item in cbbProductColor.Items)
            {
                if (item.Name == dgvProduct.Rows[index].Cells[7].Value.ToString())
                {
                    cbbProductColor.SelectedIndex = indexColor;
                }
                indexColor++;
            }

        }

        private void txbProductSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbProductSearch.Text == "") LoadProduct();
            var productsList = new ProductRepository().SearchByName(txbProductSearch.Text);
            var categoryList = new ProductCategoryRepository().GetList();
            var promotionList = new PromotionRepository().GetList();
            var suppliersList = new SupplierRepository().GetList();
            var colorsList = new ColorRepository().GetList();
            if (categoryList.Count != 0)
            {
                if (suppliersList.Count != 0)
                {
                    if (colorsList.Count != 0)
                    {
                        DataTable dt = new DataTable();
                        using (var reader = ObjectReader.Create(productsList, "Id", "Name", "Quantity", "Price", "IdCategory", "IdPromotion", "IdSupplier", "IdColor"))
                        {
                            dt.Load(reader);
                        }

                        //Change datatype of column PostionID
                        DataTable dtCloned = dt.Clone();
                        dtCloned.Columns[4].DataType = typeof(string);
                        dtCloned.Columns[5].DataType = typeof(string);
                        dtCloned.Columns[6].DataType = typeof(string);
                        dtCloned.Columns[7].DataType = typeof(string);
                        foreach (DataRow row in dt.Rows)
                        {
                            dtCloned.ImportRow(row);
                        }
                        dgvProduct.DataSource = dtCloned;
                        dgvProduct.Columns[0].HeaderText = "ID";
                        dgvProduct.Columns[1].HeaderText = "Tên sản phẩm";
                        dgvProduct.Columns[2].HeaderText = "Số lượng";
                        dgvProduct.Columns[3].HeaderText = "Giá sản phẩm";
                        dgvProduct.Columns[4].HeaderText = "Danh mục";
                        dgvProduct.Columns[5].HeaderText = "Khuyến mãi";
                        dgvProduct.Columns[6].HeaderText = "Nhà cung cấp";
                        dgvProduct.Columns[7].HeaderText = "Màu sắc";
                        // Change positonID to position Name
                        for (int i = 0; i < dgvProduct.RowCount; i++)
                        {
                            foreach (var category in categoryList)
                            {
                                if (dgvProduct.Rows[i].Cells[4].Value.ToString() == category.Id.ToString())
                                {
                                    dgvProduct.Rows[i].Cells[4].Value = category.Name;
                                }
                            }
                            foreach (var promotion in promotionList)
                            {
                                if (dgvProduct.Rows[i].Cells[5].Value.ToString() == promotion.Id.ToString())
                                {
                                    dgvProduct.Rows[i].Cells[5].Value = promotion.Coupon;
                                }
                            }
                            foreach (var supplier in suppliersList)
                            {
                                if (dgvProduct.Rows[i].Cells[6].Value.ToString() == supplier.Id.ToString())
                                {
                                    dgvProduct.Rows[i].Cells[6].Value = supplier.Name;
                                }
                            }
                            foreach (var color in colorsList)
                            {
                                if (dgvProduct.Rows[i].Cells[7].Value.ToString() == color.Id.ToString())
                                {
                                    dgvProduct.Rows[i].Cells[7].Value = color.Name;
                                }
                            }
                        }
                        cbbProductCategory.DataSource = categoryList;
                        cbbProductCategory.DisplayMember = "Name";
                        cbbProductCategory.ValueMember = "Id";

                        if (promotionList.Count != 0)
                        {
                            Dictionary<int, string> cbbProductPromotionSource = new Dictionary<int, string>();
                            cbbProductPromotionSource.Add(0, "");
                            foreach (Promotion item in promotionList)
                            {
                                cbbProductPromotionSource.Add(item.Id, item.Coupon);
                            }

                            cbbProductPromotion.DataSource = new BindingSource(cbbProductPromotionSource, null);
                            cbbProductPromotion.DisplayMember = "Value";
                            cbbProductPromotion.ValueMember = "Key";

                        }
                        else
                        {
                            Dictionary<int, string> cbbProductPromotionSource = new Dictionary<int, string>();
                            cbbProductPromotionSource.Add(0, "");
                            cbbProductPromotion.DataSource = new BindingSource(cbbProductPromotionSource, null);
                            cbbProductPromotion.DisplayMember = "Value";
                            cbbProductPromotion.ValueMember = "Key";
                        }


                        cbbProductSupplier.DataSource = suppliersList;
                        cbbProductSupplier.DisplayMember = "Name";
                        cbbProductSupplier.ValueMember = "Id";

                        cbbProductColor.DataSource = colorsList;
                        cbbProductColor.DisplayMember = "Name";
                        cbbProductColor.ValueMember = "Id";
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng thêm màu sắc sản phẩm trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tpMember.Select();
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng thêm nhà cung cấp sản phẩm trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tpMember.Select();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng thêm danh mục sản phẩm trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tpMember.Select();
            }
        }





        #endregion

        public void LoadImportInvoice()
        {
           
            var supplierList = new SupplierRepository().GetList();
            var productsList = new ProductRepository().GetList();
            if (supplierList.Count != 0 && productsList.Count != 0)
            {
                var importInvoices = new ImportInvoiceRepository().GetListByDateAndPage(dtpImportStartDay.Value, dtpImportEndDay.Value, int.Parse(txbImportPageNumber.Text));
                DataTable dt = new DataTable();
                using (var reader = ObjectReader.Create(importInvoices, "Id", "Date", "IdSupplier"))
                {
                    dt.Load(reader);
                }

                //Change datatype of column PostionID
                DataTable dtCloned = dt.Clone();
                dtCloned.Columns[2].DataType = typeof(string);
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                dgvImport.DataSource = dtCloned;
                dgvImport.Columns[0].HeaderText = "ID";
                dgvImport.Columns[1].HeaderText = "Ngày nhập";
                dgvImport.Columns[2].HeaderText = "Nhà cung cấp";
                // Change positonID to position Name
                for (int i = 0; i < dgvImport.RowCount; i++)
                {
                    foreach (var supplier in supplierList)
                    {
                        if (dgvImport.Rows[i].Cells[2].Value.ToString() == supplier.Id.ToString())
                        {
                            dgvImport.Rows[i].Cells[2].Value = supplier.Name;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng thêm sản phẩm và nhà cung cấp trước !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tpPosition.Select();
            }
        }

        private void tpImportInvoice_Enter(object sender, EventArgs e)
        {
            dtpImportStartDay.Value = DateTime.Now.AddMonths(-1);
            LoadImportInvoice();

        }

        private void dgvImport_CellClick(object sender, DataGridViewCellEventArgs e)
        {   
            dgvImportDetail.Refresh();
            int index = dgvImport.CurrentRow.Index;
            var importInfoList = new ImportInvoiceInformationRepository().GetListByIdInvoice(int.Parse(dgvImport.Rows[index].Cells[0].Value.ToString()));
            if(importInfoList.Count != 0)
            {
                var productList = new ProductRepository().GetList();
                DataTable dt = new DataTable();
                using (var reader = ObjectReader.Create(importInfoList, "IdImportInvoice", "IdProduct", "Quantity" , "Price"))
                {
                    dt.Load(reader);
                }

                //Change datatype of column PostionID
                DataTable dtCloned = dt.Clone();
                dtCloned.Columns[1].DataType = typeof(string);
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                dgvImportDetail.DataSource = dtCloned;
                dgvImportDetail.Columns[0].HeaderText = "Id hóa đơn";
                dgvImportDetail.Columns[1].HeaderText = "Tên sản phẩm";
                dgvImportDetail.Columns[2].HeaderText = "Số lượng";
                dgvImportDetail.Columns[3].HeaderText = "Giá tiền";
                // Change positonID to position Name
                for (int i = 0; i < dgvImportDetail.RowCount; i++)
                {
                    foreach (var product in productList)
                    {
                        if (dgvImportDetail.Rows[i].Cells[1].Value.ToString() == product.Id.ToString())
                        {
                            dgvImportDetail.Rows[i].Cells[1].Value = product.Name;
                        }
                    }
                }
            }
           
        }

        private void dtpImportEndDay_ValueChanged(object sender, EventArgs e)
        {
            LoadImportInvoice();
        }

        private void dtpImportStartDay_ValueChanged(object sender, EventArgs e)
        {
            LoadImportInvoice();
        }

        private void txbImportPageFirst_Click(object sender, EventArgs e)
        {
            txbImportPageNumber.Text = "1";
            LoadImportInvoice();
        }

        private void txbImportPageLast_Click(object sender, EventArgs e)
        {
            var importInvoiceList = new ImportInvoiceRepository().GetListByDate(dtpImportStartDay.Value, dtpImportEndDay.Value);

            int lastPage = (importInvoiceList.Count) / 1;
            if ((importInvoiceList.Count) % 1 != 0)
                lastPage++;
            txbImportPageNumber.Text = lastPage.ToString();
            LoadImportInvoice();
        }

        private void txbImportPageNext_Click(object sender, EventArgs e)
        {
            var importInvoiceList = new ImportInvoiceRepository().GetListByDate(dtpImportStartDay.Value, dtpImportEndDay.Value);

            int lastPage = (importInvoiceList.Count) / 10;
            if ((importInvoiceList.Count) % 10 != 0)
                lastPage++;
            var pageNumber = int.Parse(txbImportPageNumber.Text) + 1;
            if (pageNumber > lastPage) pageNumber -= 1;
            txbImportPageNumber.Text = pageNumber.ToString();
            LoadImportInvoice();
        }

        private void txbImportPagePre_Click(object sender, EventArgs e)
        {
            var pageNumber = int.Parse(txbImportPageNumber.Text) - 10;
            if (pageNumber >= 10)
            {
                txbImportPageNumber.Text = pageNumber.ToString();
            }
            LoadImportInvoice();
        }

        private void btnImportTotal_Click(object sender, EventArgs e)
        {
            var importsList = new ImportInvoiceRepository().GetListByDate(dtpImportStartDay.Value, dtpImportEndDay.Value);
            int tong = 0;
            if(importsList.Count != 0)
            {
                foreach(var item in importsList)
                {
                    var importInfoList = new ImportInvoiceInformationRepository().GetListByIdInvoice(int.Parse(item.Id.ToString()));
                    if(importInfoList.Count != 0)
                    {
                        foreach (var info in importInfoList)
                        {
                            tong += int.Parse(info.Price.ToString());
                            
                        }
                    }
                    
                }
            }
            MessageBox.Show("Tổng tiền nhập hàng từ ngày " + dtpImportStartDay.Value.ToString("dd/MM/yyyy") + " tới ngày " + dtpImportEndDay.Value.ToString("dd/MM/yyyy") + " là : " + tong.ToString("N0"), "Tổng tiền", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnImportOpenAdd_Click(object sender, EventArgs e)
        {

        }
    }
}
