using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class MemberRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public MemberRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<Member> GetList()
        {

            var res = _db.Database.SqlQuery<Member>("SP_Member_GetList").ToList();
            return res;
        }

        public int Add(Member member)
        {
            if (member == null) return 0;
            
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", member.Name),
                    
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Member_Add @Name", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Member member)
        {
            if (member == null) return false;
           
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Name", member.Name),
                   

                };
                _db.Database.ExecuteSqlCommand("SP_Member_Update @Name", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Member_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
