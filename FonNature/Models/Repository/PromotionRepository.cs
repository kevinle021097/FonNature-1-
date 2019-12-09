using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
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
                    new SqlParameter("@Id", promotion.Id),
                    new SqlParameter("@Title", promotion.Title),
                    new SqlParameter("@Coupon", promotion.Coupon),
                    new SqlParameter("@Quantity", promotion.Quantity),
                    new SqlParameter("@Price", promotion.PromotionPrice),

                };
                _db.Database.ExecuteSqlCommand("SP_Promotion_Update @Id, @Title,@Coupon,@Quantity,@Price", sqlparamater);
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

        public List<Promotion> SearchByName(string searchString)
        {
            if (searchString == null) return new List<Promotion>();
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", searchString),
                };
                var promotions = _db.Database.SqlQuery<Promotion>("SP_Promotion_SearchByName @Name", sqlparamater);
                return promotions.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Promotion> SearchByCoupon(string coupon)
        {
            if (coupon == null) return new List<Promotion>();
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", coupon),
                };
                var promotions = _db.Database.SqlQuery<Promotion>("SP_Promotion_SearchByCoupon @Name", sqlparamater);
                return promotions.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CheckExits(string coupon)
        {
            if (coupon == null) return false;
            var userNameParamater = new SqlParameter("coupon", coupon);
            var result = new SqlParameter("exits", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            _db.Database.ExecuteSqlCommand("SP_Promotion_CheckExits @exits out , @coupon", result, userNameParamater);
            return bool.Parse(result.Value.ToString());
        }


    }
}
