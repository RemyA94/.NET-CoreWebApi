using BackApi_JWT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace BackApi_JWT.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly CoreWebApiContext _dbcontext;

        public ProductController(CoreWebApiContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("List")]
        public IActionResult Lista()
        {
            List<Product> list = new List<Product>();

            try
            {
                list = _dbcontext.Products.ToList();

                return StatusCode(StatusCodes.Status200OK, new { messaje = "Ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { messaje = ex.Message, response = list });

            }
        }

        [HttpGet]
        [Route("Get/{idProduct:int}")]
        public IActionResult Obtener(int idProduct)
        {
            Product producto = _dbcontext.Products.Find(idProduct);

            if (producto == null)
            {
                return BadRequest("Producto no encontrado");
            }
            else
            {
                try
                {
                    producto = _dbcontext.Products
                        .Where(p => p.IdProduct == idProduct)
                        .FirstOrDefault();

                    return StatusCode(StatusCodes.Status200OK, new { messaje = "OK", response = producto });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status200OK, new { messaje = ex.Message, response = producto });

                }
            }
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Guardar([FromBody] Product product)
        {
            try
            {
                _dbcontext.Products.Add(product);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, new { messaje = "Save!" });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status201Created, new { messaje = ex.Message });

            }

        }

        [HttpPut]
        [Route("Edit")]
        public IActionResult Edit([FromBody] Product product)
        {
            Product oProduct = _dbcontext.Products.Find(product.IdProduct);

            if (oProduct == null)
            {
                return BadRequest("Product not found");
            }
            else
            {
                try
                {
                    oProduct.Barcode = product.Barcode is null ? oProduct.Barcode : product.Barcode;
                    oProduct.Name = product.Name is null ? oProduct.Name : product.Name;
                    oProduct.Brand = product.Brand is null ? oProduct.Brand : product.Brand;
                    oProduct.Category = product.Category is null ? oProduct.Category : product.Category;
                    oProduct.Price = product.Price is null ? oProduct.Price : product.Price;

                    _dbcontext.Products.Update(oProduct);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { messaje = "Update", response = oProduct });

                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status200OK, new { messaje = ex.Message, response = oProduct });
                }
            }
        }

        [HttpDelete]
        [Route("Delete/{idProduct:int}")]
        public IActionResult Delete(int idProduct)
        {
            Product product = _dbcontext.Products.Find(idProduct);

            if (product == null)
            {
                return BadRequest("Product not found");
            }
            else
            {
                try
                {
                    _dbcontext.Products.Remove(product);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { messaje = "Delete!" });
                }
                catch (Exception ex)
                {

                    return StatusCode(StatusCodes.Status200OK, new { messaje = ex.Message });
                }
            }

        }

    }
}
