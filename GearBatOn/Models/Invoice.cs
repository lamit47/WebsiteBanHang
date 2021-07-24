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
        public List<Province> provinces = new List<Province>();
        public List<Country> countries = new List<Country>();
        public List<Promotion> promotions = new List<Promotion>();
        public string NameCustomer;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Mã khách hàng không được để trống")]
        [StringLength(128, ErrorMessage = "Mã khách hàng không được quá 128 ký tự")]
        public string CustomerId { get; set; }

        public DateTime Date { get; set; }

        public int? PromotionId { get; set; }

        [Required(ErrorMessage = "Đất nước không được để trống")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Tỉnh thành không được để trống")]
        public int ProvinceId { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được quá 255 ký tự")]
        public string Address { get; set; }

        public bool PaymentStatus { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Country Country { get; set; }

        public virtual Promotion Promotion { get; set; }

        public virtual Province Province { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
