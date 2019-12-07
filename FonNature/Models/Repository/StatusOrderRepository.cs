using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class StatusOrderRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public StatusOrderRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<StatusOrder> GetList()
        {

            var res = _db.Database.SqlQuery<StatusOrder>("SP_Status_Order_GetList").ToList();
            return res;
        }

        public int Add(StatusOrder statusOrder)
        {
            if (statusOrder == null) return 0;
            
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", statusOrder.Name),
                    
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Status_Order_Add @Name", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(StatusOrder statusOrder)
        {
            if (statusOrder == null) return false;
            
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", statusOrder.Name),
                   

                };
                _db.Database.ExecuteSqlCommand("SP_Status_Order_Update @Name", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Status_Order_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
