namespace Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderInformation")]
    public partial class OrderInformation
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long IdOrder { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string IdProduct { get; set; }

        public int Quantity { get; set; }

        public decimal? Price { get; set; }

        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }
    }
}
