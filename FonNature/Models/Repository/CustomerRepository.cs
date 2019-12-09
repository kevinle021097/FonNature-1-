using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class CustomerRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public CustomerRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<Customer> GetList()
        {

            var res = _db.Database.SqlQuery<Customer>("SP_Customer_GetList").ToList();
            return res;
        }

        public int Add(Customer customer)
        {
            if (customer == null) return 0;
            if (customer.IdMember == 0) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", customer.Name),
                    new SqlParameter("@Email", customer.Email),
                    new SqlParameter("@Address", customer.Address),
                    new SqlParameter("@Phone", customer.Phone),
                    new SqlParameter("@idMember", customer.IdMember),
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Customer_Add @Name,@Email,@Address,@Phone,@idMember", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Customer customer)
        {
            if (customer == null) return false;
            if (customer.IdMember == 0) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Id", customer.Id),
                    new SqlParameter("@Name", customer.Name),
                    new SqlParameter("@Email", customer.Email),
                    new SqlParameter("@Address", customer.Address),
                    new SqlParameter("@Phone", customer.Phone),
                    new SqlParameter("@idMember", customer.IdMember),

                };
                _db.Database.ExecuteSqlCommand("SP_Customer_Update @Id, @Name,@Email,@Address,@Phone,@idMember", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Customer_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Customer> SearchByName(string searchString)
        {
            if (searchString == null) return new List<Customer>();
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", searchString),
                };
                var customers = _db.Database.SqlQuery<Customer>("SP_Customer_SearchByName @Name", sqlparamater);
                return customers.ToList();
            }
            catch (Exception e) 
            {
                throw e;
            }
        }


    }
}
