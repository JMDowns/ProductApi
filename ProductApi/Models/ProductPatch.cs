using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class ProductPatch
    {
        [Display(Name = "productNumber")]
        public string ProductNumber { get; set; }
        
        [Display(Name = "name")]
        public string Name { get; set; }
        
        [Range(10, 90)]
        [Display(Name = "price")]
        public double? Price { get; set; }
        
        [Display(Name = "department")]
        public string Department { get; set; }

        [Display(Name = "relatedProduct")]
        public virtual ICollection<RelatedProduct> RelatedProducts { get; set; }
    }
}
