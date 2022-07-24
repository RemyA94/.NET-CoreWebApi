using FrontApiCore.Models;
using FrontApiCore.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FrontApiCore.Controllers
{

   
    public class HomeController : Controller
    {
        private IService_API _serviceApi;

        private static string _user;
        private static string _pass;


        public HomeController(IService_API service_API)
        {
            _serviceApi = service_API;


            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            _user = builder.GetSection("ApiSettings:user").Value;
            _pass = builder.GetSection("ApiSettings:pass").Value;
 
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            List<Product> list = await _serviceApi.List();
            return View(list);

        }

        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credential credential)
        {

            if (credential.Mail == _user || credential.Key == _pass)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, _user),
                    new Claim("Key", _pass),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                bool response = await _serviceApi.Authenticate(credential);
                if (response)
                {
                    return RedirectToAction("Index");

                }
            }
            else 
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Product(int idProduct)
        {
            Product producModel = new Product();
            ViewBag.Accion = "New product";
            
            if (idProduct != 0)
            {
                ViewBag.Accion = "Edit product";
                producModel = await _serviceApi.Get(idProduct);
            }
            return View(producModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveChanges(Product product)
        {

            bool response;
            if (product.IdProduct == 0)
            {
                response = await _serviceApi.Save(product);
            }
            else
            {
                response = await _serviceApi.Edit(product);
            }

            if (response)
                return RedirectToAction("Index");
            else
                return NoContent();

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int idProduct)
        {

            var response = await _serviceApi.Delete(idProduct);

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