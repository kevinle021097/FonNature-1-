using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class SupplierRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public SupplierRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<Supplier> GetList()
        {

            var res = _db.Database.SqlQuery<Supplier>("SP_Supplier_GetList").ToList();
            return res;
        }

        public int Add(Supplier supplier)
        {
            if (supplier == null) return 0;
            
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", supplier.Name),
                    new SqlParameter("@Address", supplier.Address),
                    new SqlParameter("@Phone", supplier.Phone),
                    new SqlParameter("@Email", supplier.Email),
                    new SqlParameter("@SupplierProduct", supplier.SupplierProduct),
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Supplier_Add @Name,@Address,@Phone,@Email,@SupplierProduct", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Supplier supplier)
        {
            if (supplier == null) return false;
           
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Id", supplier.Id),
                     new SqlParameter("@Name", supplier.Name),
                    new SqlParameter("@Address", supplier.Address),
                    new SqlParameter("@Phone", supplier.Phone),
                    new SqlParameter("@Email", supplier.Email),
                    new SqlParameter("@SupplierProduct", supplier.SupplierProduct),

                };
                _db.Database.ExecuteSqlCommand("SP_Supplier_Update @Id,@Name,@Address,@Phone,@Email,@SupplierProduct", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Supplier_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Supplier> SearchByName(string searchString)
        {
            if (searchString == null) return new List<Supplier>();
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", searchString),
                };
                var suppliers = _db.Database.SqlQuery<Supplier>("SP_Supplier_SearchByName @Name", sqlparamater);
                return suppliers.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
