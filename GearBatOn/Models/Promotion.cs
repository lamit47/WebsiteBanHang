namespace GearBatOn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Promotion")]
    public partial class Promotion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Promotion()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Tên Khuyến Mãi")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Mã Khuyễn Mãi")]
        public string PromoCode { get; set; }
        [Display(Name = "Phầm trăm")]
        public double Ratio { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
