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
    public class AccountRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public AccountRepository()
        {
            _db = new FonNatureDbContext();
        }

        public Account GetDetail(string userName)
        {
            object[] sqlparamater =
            {
                new SqlParameter("@userName", userName),
            };
            var res = _db.Database.SqlQuery<Account>("SP_Account_GetDetail @userName", sqlparamater).SingleOrDefault();
            return res;
        }

        public List<Account> GetList()
        {

            var res = _db.Database.SqlQuery<Account>("SP_Account_GetList").ToList();
            return res;
        }

        public int Add(Account account)
        {
            if (account == null) return 0;
            if (account.IdStaff == 0) return 0;
            account.Password = Encryption.MD5Hash(Encryption.Base64Encode(account.Password));
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@userName", account.UserName),
                    new SqlParameter("@passWord", account.Password),
                    new SqlParameter("@displayName", account.DisplayName),
                    new SqlParameter("@idStaff", account.IdStaff),
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Account_Add @userName,@passWord,@displayName,@idStaff", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(Account account)
        {
            if (account == null) return false;
            if (account.IdStaff == 0) return false;
            account.Password = Encryption.MD5Hash(Encryption.Base64Encode(account.Password));
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@userName", account.UserName),
                    new SqlParameter("@passWord", account.Password),
                    new SqlParameter("@displayName", account.DisplayName),
                    new SqlParameter("@idStaff", account.IdStaff),

                };
                _db.Database.ExecuteSqlCommand("SP_Account_Edit @userName,@passWord,@displayName,@idStaff", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Delete(string userName)
        {
            if (userName == null) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@userName", userName),
                };
                _db.Database.ExecuteSqlCommand("SP_Account_Delete @userName", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Account> SearchByName(string userName)
        {
            if (userName == null) return null;

            object[] sqlparamater =
            {
                    new SqlParameter("@userName", userName),
                };
            var res = _db.Database.SqlQuery<Account>("SP_Account_SearchByName @userName", sqlparamater).ToList();
            return res;
        }

        public bool CheckLogin(string userName, string passWord)
        {
            if (userName == null) return false;
            if (passWord == null) return false;
            passWord = Encryption.MD5Hash(Encryption.Base64Encode(passWord));
            object[] sqlparamater =
            {
                    new SqlParameter("@userName", userName),
                    new SqlParameter("@passWord", passWord)
                };
            var res = _db.Database.SqlQuery<Account>("SP_Account_Login @userName,@passWord", sqlparamater).ToList();
            if (res.Count == 0) return false;
            return true;
        }

        public bool CheckUserName(string position, string userName)
        {
            if (position == null) return false;
            if (userName == null) return false;
            object[] sqlparamater =
            {
                    new SqlParameter("@position", position)
            };
            var res = _db.Database.SqlQuery<Account>("SP_Account_ByPosition	@position", sqlparamater).ToList();
            if (!res.Select(x => x.UserName).Contains(userName))
                return false;
            return true;
        }

        public bool CheckExits(string userName)
        {
            if (userName == null) return false;
            var userNameParamater = new SqlParameter("userName", userName);
            var result = new SqlParameter("exits", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            _db.Database.ExecuteSqlCommand("SP_Account_CheckExits @exits output, @userName", result, userNameParamater);
            return bool.Parse(result.Value.ToString());
        }
    }
}
