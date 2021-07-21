namespace GearBatOn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public List<Category> ListCategory = new List<Category>();
        public List<Country> ListCountry = new List<Country>();
        public List<Brand> ListBrand = new List<Brand>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Images = new HashSet<Image>();
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public int CountryId { get; set; }

        public int BrandId { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int WarrantyPeriod { get; set; }

        public int InventoryAmount { get; set; }

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
