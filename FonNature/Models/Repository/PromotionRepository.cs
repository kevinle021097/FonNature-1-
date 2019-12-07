using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class PromotionRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public PromotionRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<Promotion> GetList()
        {

            var res = _db.Database.SqlQuery<Promotion>("SP_Promotion_GetList").ToList();
            return res;
        }

        public int Add(Promotion promotion)
        {
            if (promotion == null) return 0;

            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Title", promotion.Title),
                    new SqlParameter("@Coupon", promotion.Coupon),
                    new SqlParameter("@Quantity", promotion.Quantity),
                    new SqlParameter("@Price", promotion.PromotionPrice),

                };
                int res = _db.Database.ExecuteSqlCommand("SP_Promotion_Add @Title,@Coupon,@Quantity,@Price", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Promotion promotion)
        {
            if (promotion == null) return false;

            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Title", promotion.Title),
                    new SqlParameter("@Coupon", promotion.Coupon),
                    new SqlParameter("@Quantity", promotion.Quantity),
                    new SqlParameter("@Price", promotion.PromotionPrice),

                };
                _db.Database.ExecuteSqlCommand("SP_Promotion_Update @Title,@Coupon,@Quantity,@Price", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Promotion_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
