using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class ImportInvoiceInformationRepository
    {
        private FonNatureDbContext _db = null;

        public FonNatureDbContext Db { get => _db; set => _db = value; }

        public ImportInvoiceInformationRepository()
        {
            _db = new FonNatureDbContext();
        }



        public List<ImportInvoiceInformation> GetList()
        {

            var res = _db.Database.SqlQuery<ImportInvoiceInformation>("SP_Import_Invoice_Information_GetList").ToList();
            return res;
        }

        public int Add(ImportInvoiceInformation importInvoiceInformation)
        {
            if (importInvoiceInformation == null) return 0;
            if (importInvoiceInformation.IdImportInvoice == 0) return 0;
            if (importInvoiceInformation.IdProduct == null) return 0;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdImportInvoice", importInvoiceInformation.IdImportInvoice),
                    new SqlParameter("@IdProduct", importInvoiceInformation.IdProduct),
                    new SqlParameter("@Quantity", importInvoiceInformation.Quantity),
                    new SqlParameter("@Price", importInvoiceInformation.Price),

                };
                int res = _db.Database.ExecuteSqlCommand("SP_Import_Invoice_Information_Add @IdImportInvoice,@IdProduct,@Quantity,@Price", sqlparamater);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Edit(ImportInvoiceInformation importInvoiceInformation)
        {
            if (importInvoiceInformation == null) return false;
            if (importInvoiceInformation.IdImportInvoice == 0) return false;
            if (importInvoiceInformation.IdProduct == null) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdImportInvoice", importInvoiceInformation.IdImportInvoice),
                    new SqlParameter("@IdProduct", importInvoiceInformation.IdProduct),
                    new SqlParameter("@Quantity", importInvoiceInformation.Quantity),
                    new SqlParameter("@Price", importInvoiceInformation.Price),

                };
                _db.Database.ExecuteSqlCommand("SP_Import_Invoice_Information_Update @IdImportInvoice,@IdProduct,@Quantity,@Price", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Delete(string IdImportInvoice, string IdProduct)
        {
            if (IdImportInvoice == null) return false;
            if (IdProduct == null) return false;
            try
            {
                object[] sqlparamater =
                {
                    new SqlParameter("@IdImportInvoice", IdImportInvoice),
                    new SqlParameter("@IdProduct", IdProduct),
                };
                _db.Database.ExecuteSqlCommand("SP_Import_Invoice_Information_Del @IdImportInvoice,@IdProduct", sqlparamater);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
