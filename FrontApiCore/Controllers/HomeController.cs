using FrontApiCore.Models;
using FrontApiCore.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontApiCore.Controllers
{
    public class HomeController : Controller
    {
        private IService_API _servicioApi;

        public HomeController(IService_API servicioApi)
        {
            _servicioApi = servicioApi;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> list = await _servicioApi.List();
            return View(list);
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credential credenciales)
        {

            bool response = await _servicioApi.Authenticate(credenciales);
            if (response)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Unauthorized user", "Login");

            return View();
        }

        public async Task<IActionResult> Producto(int idProduct)
        {

            Product producModel = new Product();

            ViewBag.Accion = "New product";

            if (idProduct != 0)
            {

                ViewBag.Accion = "Edit product";
                producModel = await _servicioApi.Get(idProduct);
            }

            return View(producModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveChanges(Product product)
        {

            bool response;

            if (product.IdProduct == 0)
            {
                response = await _servicioApi.Save(product);
            }
            else
            {
                response = await _servicioApi.Edit(product);
            }


            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int idProduct)
        {

            var response = await _servicioApi.Delete(idProduct);

            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}