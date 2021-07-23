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

        [Required(ErrorMessage = "Tên khuyến mãi không được để trống")]
        [StringLength(255, ErrorMessage = "Tên khuyến mãi không được quá 255  ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mã khuyến mãi không được để trống")]
        [StringLength(255, ErrorMessage = "Mã khuyến mãi không được quá 255  ký tự")]
        public string PromoCode { get; set; }

        [Required(ErrorMessage = "Tỷ lệ không được để trống")]
        public double Ratio { get; set; }

        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}