using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class PositionRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public PositionRepository()
        {
            _db = new FonNatureDbContext();
        }


        public List<Position> GetList()
        {

            var res = _db.Database.SqlQuery<Position>("SP_Position_GetList").ToList();
            return res;
        }

        public bool CheckExits(string name)
        {
            var positions = _db.Database.SqlQuery<Position>("SP_Position_GetList").ToList();
            if (positions.Select(x => x.Name.ToUpper()).Contains(name.ToUpper()))
            {
                return false;
            }
            return true;
        }

        public int Add(Position position)
        {
            if (position == null) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", position.Name),           
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Position_Add @Name", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Position position)
        {
            if (position == null) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Id", position.Id),
                    new SqlParameter("@Name", position.Name)
                    
                };
                _db.Database.ExecuteSqlCommand("SP_Position_Update @Id, @Name", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Position_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Position> SearchByName(string searchString)
        {
            if (searchString == null) return new List<Position>();
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", searchString),
                };
                var positions =  _db.Database.SqlQuery<Position>("SP_Position_SearchByName @Name", sqlparamater);
                return positions.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
