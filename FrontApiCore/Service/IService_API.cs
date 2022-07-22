using FrontApiCore.Models;

namespace FrontApiCore.Servicios
{
    public interface IService_API
    {
        Task<List<Product>> List();
        Task<Product> Get(int idProduct);

        Task<bool> Save(Product objet);

        Task<bool> Edit(Product objet);

        Task<bool> Delete(int idProduct);
        Task<bool> Authenticate(Credential credential);
    }
}

