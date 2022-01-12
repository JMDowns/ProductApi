using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductApi.Daos;
using ProductApi.Extensions;

namespace ProductApi.Models
{
    public static class ProductSeed
    {
        public static void InitData(ProductContext context)
        {
            var rnd = new Random();

            var adjectives = new[] { "Small", "Ergonomic", "Rustic",
                "Smart", "Sleek" };
            var materials = new[] { "Steel", "Wooden", "Concrete", "Plastic",
                "Granite", "Rubber" };
            var names = new[] { "Chair", "Car", "Computer", "Pants", "Shoes" };
            var departments = new[] { "Books", "Movies", "Music",
                "Games", "Electronics" };

            context.Products.AddRange(500.Times(x =>
            {
                var adjective = adjectives[rnd.Next(0, 5)];
                var material = materials[rnd.Next(0, 5)];
                var name = names[rnd.Next(0, 5)];
                var department = departments[rnd.Next(0, 5)];
                var productId = $"{x,-3:000}";

                Product? relatedProduct = null;

                var hasRelatedProduct = Convert.ToBoolean(rnd.Next(0, 2));
                if (hasRelatedProduct)
                {
                    var radjective = adjectives[rnd.Next(0, 5)];
                    var rmaterial = materials[rnd.Next(0, 5)];
                    var rname = names[rnd.Next(0, 5)];
                    var rdepartment = departments[rnd.Next(0, 5)];
                    var rproductId = $"{x,-3:000}";

                    relatedProduct = new Product
                    {
                        ProductNumber =
                            $"{rdepartment.First()}{name.First()}{rproductId}",
                        Name = $"{radjective} {rmaterial} {rname}",
                        Price = (double)rnd.Next(1000, 9000) / 100,
                        Department = rdepartment,
                        RelatedProduct = null
                    };
                }

                return new Product
                {
                    ProductNumber =
                        $"{department.First()}{name.First()}{productId}",
                    Name = $"{adjective} {material} {name}",
                    Price = (double)rnd.Next(1000, 9000) / 100,
                    Department = department,
                    RelatedProduct = relatedProduct
                };
            }));

            context.SaveChanges();
        }
    }
}
