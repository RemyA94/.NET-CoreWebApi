﻿using FrontApiCore.Models;
using FrontApiCore.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontApiCore.Controllers
{
    public class HomeController : Controller
    {
        private IServicio_API _servicioApi;

        public HomeController(IServicio_API servicioApi)
        {
            _servicioApi = servicioApi;
        }

        public async Task<IActionResult> Index()
        {
            List<Producto> lista = await _servicioApi.Lista();
            return View(lista);
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credenciales credenciales)
        {

            bool respuesta = await _servicioApi.Autenticar(credenciales);
            if (respuesta)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Unauthorized user", "Login");

            return View();
        }

        public async Task<IActionResult> Producto(int idProducto)
        {

            Producto modelo_producto = new Producto();

            ViewBag.Accion = "New product";

            if (idProducto != 0)
            {

                ViewBag.Accion = "Edit product";
                modelo_producto = await _servicioApi.Obtener(idProducto);
            }

            return View(modelo_producto);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCambios(Producto ob_producto)
        {

            bool respuesta;

            if (ob_producto.IdProducto == 0)
            {
                respuesta = await _servicioApi.Guardar(ob_producto);
            }
            else
            {
                respuesta = await _servicioApi.Editar(ob_producto);
            }


            if (respuesta)
                return RedirectToAction("Index");
            else
                return NoContent();

        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int idProducto)
        {

            var respuesta = await _servicioApi.Eliminar(idProducto);

            if (respuesta)
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