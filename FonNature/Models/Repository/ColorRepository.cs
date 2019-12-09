using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class ColorRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public ColorRepository()
        {
            _db = new FonNatureDbContext();
        }

        public List<Color> GetList()
        {

            var res = _db.Database.SqlQuery<Color>("SP_Color_GetList").ToList();
            return res;
        }

        public int Add(Color Color)
        {
            if (Color == null) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", Color.Name),
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Color_Add @Name", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Color Color)
        {
            if (Color == null) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Id", Color.Id),
                    new SqlParameter("@Name", Color.Name)

                };
                _db.Database.ExecuteSqlCommand("SP_Color_Update @Id,@Name", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Color_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
