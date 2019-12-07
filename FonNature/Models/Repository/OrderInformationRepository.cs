using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class OrderInformationRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public OrderInformationRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<OrderInformation> GetList()
        {

            var res = _db.Database.SqlQuery<OrderInformation>("SP_Order_Information_GetList").ToList();
            return res;
        }

        public int Add(OrderInformation orderInformation)
        {
            if (orderInformation == null) return 0;
            if (orderInformation.IdOrder == 0) return 0;
            if (orderInformation.IdProduct == null) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdOrder", orderInformation.IdOrder),
                    new SqlParameter("@IdProduct", orderInformation.IdProduct),
                    new SqlParameter("@Quantity", orderInformation.Quantity),
                    new SqlParameter("@Price", orderInformation.Price),
                    
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Order_Information_Add @IdOrder,@IdProduct,@Quantity,@Price", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(OrderInformation orderInformation)
        {
            if (orderInformation == null) return false;
            if (orderInformation.IdOrder == 0) return false;
            if (orderInformation.IdProduct == null) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdOrder", orderInformation.IdOrder),
                    new SqlParameter("@IdProduct", orderInformation.IdProduct),
                    new SqlParameter("@Quantity", orderInformation.Quantity),
                    new SqlParameter("@Price", orderInformation.Price),

                };
                _db.Database.ExecuteSqlCommand("SP_Order_Information_Update @IdOrder,@IdProduct,@Quantity,@Price", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Delete(string IdOrder, string IdProduct)
        {
            if (IdOrder == null) return false;
            if (IdProduct == null) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdOrder", IdOrder),
                    new SqlParameter("@IdProduct", IdProduct),
                };
                _db.Database.ExecuteSqlCommand("SP_Order_Information_Del @IdOrder,@IdProduct", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
