namespace Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ImportInvoice")]
    public partial class ImportInvoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ImportInvoice()
        {
            ImportInvoiceInformations = new HashSet<ImportInvoiceInformation>();
        }

        public long Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int IdSupplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportInvoiceInformation> ImportInvoiceInformations { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
