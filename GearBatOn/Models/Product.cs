namespace GearBatOn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Product")]
    public partial class Product
    {
        public List<Category> ListCategory = new List<Category>();
        public List<Country> ListCountry = new List<Country>();
        public List<Brand> ListBrand = new List<Brand>();
        public List<Image> ListImage = new List<Image>();
        public string FeatureImage;
        public int TotalSeller;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Images = new HashSet<Image>();
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được quá 255 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mã danh mục không được để trống")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Mã nước sản xuất không được để trống")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Mã hãng sản xuất không được để trống")]
        public int BrandId { get; set; }

        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:n}")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Thời gian bảo hành không được để trống")]
        public int WarrantyPeriod { get; set; }

        [Required(ErrorMessage = "Tồn kho không được để trống")]
        public int Inventory { get; set; }

        public bool Status { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Category Category { get; set; }

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
