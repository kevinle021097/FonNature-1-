namespace Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            ImportInvoiceInformations = new HashSet<ImportInvoiceInformation>();
            OrderInformations = new HashSet<OrderInformation>();
        }

        [StringLength(100)]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int IdCategory { get; set; }

        public int? IdPromotion { get; set; }

        public int IdSupplier { get; set; }

        public int IdColor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportInvoiceInformation> ImportInvoiceInformations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderInformation> OrderInformations { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public virtual Promotion Promotion { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
