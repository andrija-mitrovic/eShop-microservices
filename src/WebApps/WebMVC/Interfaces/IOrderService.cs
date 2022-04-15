using WebMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebMVC.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }
}
