using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class ProductCategoryRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public ProductCategoryRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<ProductCategory> GetList()
        {

            var res = _db.Database.SqlQuery<ProductCategory>("SP_Product_Category_GetList").ToList();
            return res;
        }

        public int Add(ProductCategory productCategory)
        {
            if (productCategory == null) return 0;
           
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", productCategory.Name),
                    
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Product_Category_Add @Name", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(ProductCategory productCategory)
        {
            if (productCategory == null) return false;
            
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Id", productCategory.Id),
                    new SqlParameter("@Name", productCategory.Name)

                };
                _db.Database.ExecuteSqlCommand("SP_Product_Category_Update @Id, @Name", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Product_Category_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ProductCategory> SearchByName(string searchString)
        {
            if (searchString == null) return new List<ProductCategory>();
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", searchString),
                };
                var categories = _db.Database.SqlQuery<ProductCategory>("SP_ProductCategory_SearchByName @Name", sqlparamater);
                return categories.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
