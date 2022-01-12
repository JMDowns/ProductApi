using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Daos;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;

            if (_context.Products.Any()) return;

            ProductSeed.InitData(context);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IQueryable<Product>> GetProducts([FromQuery] string department)
        {
            var result = _context.Products as IQueryable<Product>;

            if (!string.IsNullOrEmpty(department))
            {
                result = result.Where(p => p.Department.StartsWith(department, StringComparison.InvariantCultureIgnoreCase));
            }

            return Ok(result
                .OrderBy(p => p.ProductNumber)
                .Take(15));
        }

        [HttpGet]
        [Route("{productNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IQueryable<Product>> GetProductsById([FromRoute] string productNumber)
        {
            var result = _context.Products as IQueryable<Product>;

            if (!string.IsNullOrEmpty(productNumber))
            {
                result = result.Where(p => p.ProductNumber.Equals(productNumber));
            }

            return Ok(result
                .OrderBy(p => p.ProductNumber)
                .Take(15));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> PostProduct([FromBody] Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return new CreatedResult($"/products/{product.ProductNumber.ToLower()}", product);
            }
            catch (Exception e)
            {
                // Typically an error log is produced here
                return ValidationProblem(e.Message);
            }
        }

        [HttpPatch]
        [Route("{productNumber}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> PatchProduct([FromRoute] string productNumber, [FromBody] ProductPatch newProduct)
        {
            try
            {
                var productList = _context.Products as IQueryable<Product>;
                var product = productList.First(p => p.ProductNumber.Equals(productNumber));

                product.ProductNumber = newProduct.ProductNumber ?? product.ProductNumber;
                product.Department = newProduct.Department ?? product.Department;
                product.Name = newProduct.Name ?? product.Name;
                product.Price = newProduct.Price ?? product.Price;
                product.RelatedProduct = newProduct.RelatedProduct ?? product.RelatedProduct;

                _context.Products.Update(product);
                _context.SaveChanges();

                return new CreatedResult($"/products/{product.ProductNumber.ToLower()}", product);
            }
            catch (Exception e)
            {
                // Typically an error log is produced here
                return ValidationProblem(e.Message);
            }
        }

        [HttpPut]
        [Route("{productNumber}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> PutProduct([FromRoute] string productNumber, [FromBody] Product newProduct)
        {
            try
            {
                var productList = _context.Products as IQueryable<Product>;
                var product = productList.First(p => p.ProductNumber.Equals(productNumber));

                _context.Products.Remove(product);
                _context.Products.Add(newProduct);
                _context.SaveChanges();

                return new CreatedResult($"/products/{product.ProductNumber.ToLower()}", product);
            }
            catch (Exception e)
            {
                // Typically an error log is produced here
                return ValidationProblem(e.Message);
            }
        }

        [HttpDelete]
        [Route("{productNumber}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> DeleteProduct([FromRoute] string productNumber)
        {
            try
            {
                var productList = _context.Products as IQueryable<Product>;
                var product = productList.First(p => p.ProductNumber.Equals(productNumber));

                _context.Products.Remove(product);
                _context.SaveChanges();

                return new CreatedResult($"/products/{product.ProductNumber.ToLower()}", product);
            }
            catch (Exception e)
            {
                // Typically an error log is produced here
                return ValidationProblem(e.Message);
            }
        }
    }
}
