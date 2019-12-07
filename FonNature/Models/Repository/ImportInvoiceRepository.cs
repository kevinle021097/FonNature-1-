using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class ImportInvoiceRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public ImportInvoiceRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<ImportInvoice> GetList()
        {

            var res = _db.Database.SqlQuery<ImportInvoice>("SP_Import_Invoice_GetList").ToList();
            return res;
        }

        public int Add(ImportInvoice importInvoice)
        {
            if (importInvoice == null) return 0;
            if (importInvoice.IdSupplier == 0) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Date", importInvoice.Date),
                    new SqlParameter("@IdSupplier", importInvoice.IdSupplier),
                   
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Import_Invoice_Add @Date,@IdSupplier", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(ImportInvoice importInvoice)
        {
            if (importInvoice == null) return false;
            if (importInvoice.IdSupplier == 0) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@Date", importInvoice.Date),
                    new SqlParameter("@IdSupplier", importInvoice.IdSupplier),

                };
                _db.Database.ExecuteSqlCommand("SP_Import_Invoice_Update @Date,@IdSupplier", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Import_Invoice_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
