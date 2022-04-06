using Shopping.Aggregator.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetOrdersByUserName(string userName);
    }
}
