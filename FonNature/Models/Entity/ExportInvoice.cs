namespace Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExportInvoice")]
    public partial class ExportInvoice
    {
        public long Id { get; set; }

        public long? IdOrder { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public virtual Order Order { get; set; }
    }
}
