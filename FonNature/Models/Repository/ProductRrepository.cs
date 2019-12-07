using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class ProductRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public ProductRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<Product> GetList()
        {

            var res = _db.Database.SqlQuery<Product>("SP_Product_GetList").ToList();
            return res;
        }

        public int Add(Product product)
        {
            if (product == null) return 0;
            if (product.IdCategory == 0) return 0;
            if (product.IdSupplier == 0) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", product.Name),
                    new SqlParameter("@Quantity", product.Quantity),
                    new SqlParameter("@Price", product.Price),
                    new SqlParameter("@IdCategory", product.IdCategory),
                    new SqlParameter("@IdPromotion", product.IdPromotion),
                    new SqlParameter("@IdSupplier", product.IdSupplier),
                    new SqlParameter("@IdColor", product.IdColor),
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Product_Add @Name,@Quantity,@Price,@IdCategory,@IdPromotion,@IdSupplier,@IdColor", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Product product)
        {
            if (product == null) return false;
            if (product.IdCategory == 0) return false;
            if (product.IdSupplier == 0) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", product.Name),
                    new SqlParameter("@Quantity", product.Quantity),
                    new SqlParameter("@Price", product.Price),
                    new SqlParameter("@IdCategory", product.IdCategory),
                    new SqlParameter("@IdPromotion", product.IdPromotion),
                    new SqlParameter("@IdSupplier", product.IdSupplier),
                    new SqlParameter("@IdColor", product.IdColor),

                };
                _db.Database.ExecuteSqlCommand("SP_Product_Update @Name,@Quantity,@Price,@IdCategory,@IdPromotion,@IdSupplier,@IdColor", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Product_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //SP_Product_ListColor

        public List<Color> ListColor(string name)
        {
            if (name == null) return null;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", name),
                };
                var res = _db.Database.SqlQuery<Color>("SP_Product_ListColor @Name", sqlparamater).ToList();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
