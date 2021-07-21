namespace GearBatOn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Invoice")]
    public partial class Invoice
    {
        public string NameStaff;
        public string NameCustomer;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string StaffId { get; set; }

        [Required]
        [StringLength(128)]
        public string CustomerId { get; set; }

        public DateTime Date { get; set; }

        public int? PromotionId { get; set; }

        public int CountryId { get; set; }

        public int ProvinceId { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        public bool PaymentStatus { get; set; }

        public virtual Country Country { get; set; }

        public virtual Promotion Promotion { get; set; }

        public virtual Province Province { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
