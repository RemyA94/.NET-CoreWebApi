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
    public class ProductoController : ControllerBase
    {
        private readonly CoreWebApiContext _dbcontext;

        public ProductoController(CoreWebApiContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                lista = _dbcontext.Productos.ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });

            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            Producto producto = _dbcontext.Productos.Find(idProducto);

            if (producto == null)
            {
                return BadRequest("Producto no encontrado");
            }
            else
            {
                try
                {
                    producto = _dbcontext.Productos
                        .Where(p => p.IdProducto == idProducto)
                        .FirstOrDefault();

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = producto });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = producto });

                }
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto producto)
        {
            try
            {
                _dbcontext.Productos.Add(producto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, new { mensaje = "Guardado" });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status201Created, new { mensaje = ex.Message });

            }

        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto producto)
        {
            Producto oProducto = _dbcontext.Productos.Find(producto.IdProducto);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }
            else
            {
                try
                {
                    oProducto.CodigoBarra = producto.CodigoBarra is null ? oProducto.CodigoBarra : producto.CodigoBarra;
                    oProducto.Descripcion = producto.Descripcion is null ? oProducto.Descripcion : producto.Descripcion;
                    oProducto.Marca = producto.Marca is null ? oProducto.Marca : producto.Marca;
                    oProducto.Categoria = producto.Categoria is null ? oProducto.Categoria : producto.Categoria;
                    oProducto.Precio = producto.Precio is null ? oProducto.Precio : producto.Precio;

                    _dbcontext.Productos.Update(oProducto);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Update", response = oProducto });

                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oProducto });
                }
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            Producto producto = _dbcontext.Productos.Find(idProducto);

            if (producto == null)
            {
                return BadRequest("Producto no encontrado");
            }
            else
            {
                try
                {
                    _dbcontext.Productos.Remove(producto);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Delete" });
                }
                catch (Exception ex)
                {

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
                }
            }

        }

    }
}
