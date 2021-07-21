namespace GearBatOn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class Image
    {
        [NotMapped]
        [Required(ErrorMessage = "Bắt buộc chọn tệp hình ảnh")]
        public HttpPostedFileBase ImageFile { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ImagePath { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
