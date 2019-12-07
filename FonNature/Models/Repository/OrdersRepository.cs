using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class OrderRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public OrderRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<Order> GetList()
        {

            var res = _db.Database.SqlQuery<Order>("SP_Order_GetList").ToList();
            return res;
        }

        public int Add(Order order)
        {
            if (order == null) return 0;
            if (order.IdStatus == 0) return 0;
            if (order.IdCustomer == 0) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdCustomer", order.IdCustomer),
                    new SqlParameter("@IdStatus", order.IdStatus),
                    new SqlParameter("@Total", order.TotalPrice),
                   
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Order_Add @IdCustomer,@IdStatus,@Total", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Order order)
        {
            if (order == null) return false;
            if (order.IdStatus == 0) return false;
            if (order.IdCustomer == 0) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdCustomer", order.IdCustomer),
                    new SqlParameter("@IdStatus", order.IdStatus),
                    new SqlParameter("@Total", order.TotalPrice),

                };
                _db.Database.ExecuteSqlCommand("SP_Order_Update @IdCustomer,@IdStatus,@Total", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Order_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
