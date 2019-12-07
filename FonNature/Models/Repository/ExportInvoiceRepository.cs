using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class ExportInvoiceRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public ExportInvoiceRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<ExportInvoice> GetList()
        {

            var res = _db.Database.SqlQuery<ExportInvoice>("SP_Export_Invoice_GetList").ToList();
            return res;
        }

        public int Add(ExportInvoice exportInvoice)
        {
            if (exportInvoice == null) return 0;
            if (exportInvoice.IdOrder == 0) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdOrder", exportInvoice.IdOrder),
                    new SqlParameter("@Date", exportInvoice.Date),
                    
                };
                int res = _db.Database.ExecuteSqlCommand("SP_Export_Invoice_Add @IdOrder,@Date", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(ExportInvoice exportInvoice)
        {
            if (exportInvoice == null) return false;
            if (exportInvoice.IdOrder == 0) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdOrder", exportInvoice.IdOrder),
                    new SqlParameter("@Date", exportInvoice.Date),

                };
                _db.Database.ExecuteSqlCommand("SP_Export_Invoice_Update @IdOrder,@Date", sqlparamater);
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
                _db.Database.ExecuteSqlCommand("SP_Export_Invoice_Del @Id", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
