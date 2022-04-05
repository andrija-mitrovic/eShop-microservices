using Shopping.Aggregator.DTOs;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Interfaces
{
    public interface IBasketService
    {
        Task<BasketDto> GetBasket(string userName);
    }
}
