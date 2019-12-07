using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class StaffRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public StaffRepository()
        {
            _db = new FonNatureDbContext();
        }

        

        public List<Staff> GetList()
        {

            var res = _db.Database.SqlQuery<Staff>("SP_Staff_GetList").ToList();
            return res;
        }

        public int Add(Staff staff)
        {
            if (staff == null) return 0;
            if (staff.IdPosition == 0) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", staff.Name),
                    new SqlParameter("@Email", staff.Email),
                    new SqlParameter("@Address", staff.Address),
                    new SqlParameter("@Phone", staff.Phone),
                    new SqlParameter("@idPosition", staff.IdPosition),
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Staff_Add @Name,@Email,@Address,@Phone,@idPosition", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Staff staff)
        {
            if (staff == null) return false;
            if (staff.IdPosition == 0) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", staff.Name),
                    new SqlParameter("@Email", staff.Email),
                    new SqlParameter("@Address", staff.Address),
                    new SqlParameter("@Phone", staff.Phone),
                    new SqlParameter("@idPosition", staff.IdPosition),

                };
                _db.Database.ExecuteSqlCommand("SP_Account_Update @Name,@Email,@Address,@Phone,@idPosition", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Delete(string Id)
        {
            if (Id == null) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Id", Id),
                };
                _db.Database.ExecuteSqlCommand("SP_Staff_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

       
    }
}
