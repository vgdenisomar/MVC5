using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Z_Market.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        public string Description { get; set; }
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Usted debe ingresar {0}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
        public DateTime LastBuy { get; set; }
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Usted debe ingresar {0}")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public float Stock { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}