using FrontApiCore.Controllers;
using FrontApiCore.Models;
using FrontApiCore.Servicios;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FrontApiCore.Servicios
{
    public class Service_API : IService_API
    {
        private static string _user;
        private static string _pass;
        private static string _baseUrl;
        private static string _token;

        public Service_API()
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            _user = builder.GetSection("ApiSettings:usuario").Value;
            _pass = builder.GetSection("ApiSettings:clave").Value;
            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task<bool> Authenticate(Credential credential)
        {
            bool resp;

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);

            if (credential.Mail == _user && credential.Key == _pass)
            {
                var content = new StringContent(JsonConvert.SerializeObject(credential), Encoding.UTF8, "application/json");
                var response = await cliente.PostAsync("api/Autenticacion/Validar", content);
                var json_response = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiToken>(json_response);
                _token = result.Token;              
            }
            else
            {
                resp = false;
            }
            return resp = true;

        }

        public async Task<List<Product>> List()
        {
            List<Product> list = new List<Product>();
            Credential credential = new Credential();

            await Authenticate(credential);


            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await client.GetAsync("api/Producto/Lista");

            if (response.IsSuccessStatusCode)
            {

                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponse>(json_response);
                list = result.response;
            }

            return list;
        }

        public async Task<Product> Get(int idProduct)
        {
            Product objet = new Product();
            Credential credential = new Credential();

            await Authenticate(credential);


            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseUrl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await cliente.GetAsync($"api/Producto/Obtener/{idProduct}");

            if (response.IsSuccessStatusCode)
            {

                var json_response = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<FrontRequest>(json_response);
                objet = result.response;
            }

            return objet;
        }

        public async Task<bool> Save(Product objet)
        {
            bool resp = false;
            Credential credential = new Credential();

            await Authenticate(credential);


            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var content = new StringContent(JsonConvert.SerializeObject(objet), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Producto/Guardar/", content);

            if (response.IsSuccessStatusCode)
            {
                resp = true;
            }

            return resp;
        }

        public async Task<bool> Edit(Product objet)
        {
            bool resp = false;
            Credential credential = new Credential();

            await Authenticate(credential);


            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var content = new StringContent(JsonConvert.SerializeObject(objet), Encoding.UTF8, "application/json");

            var response = await client.PutAsync("api/Producto/Editar/", content);

            if (response.IsSuccessStatusCode)
            {
                resp = true;
            }

            return resp;
        }

        public async Task<bool> Delete(int idProduct)
        {
            bool resp = false;
            Credential credential = new Credential();

            await Authenticate(credential);


            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);


            var response = await client.DeleteAsync($"api/Producto/Eliminar/{idProduct}");

            if (response.IsSuccessStatusCode)
            {
                resp = true;
            }

            return resp;
        }

    }
}
