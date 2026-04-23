using Catalog.Service.API.Entities;
using MongoDB.Driver;

namespace Catalog.Service.API.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
