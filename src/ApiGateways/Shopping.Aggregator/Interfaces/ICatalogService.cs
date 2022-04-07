using Shopping.Aggregator.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogDto>> GetCatalog();
        Task<IEnumerable<CatalogDto>> GetCatalogByCategory(string category);
        Task<CatalogDto> GetCatalog(string id);
    }
}
