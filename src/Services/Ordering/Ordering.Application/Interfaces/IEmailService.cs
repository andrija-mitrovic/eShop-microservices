using Ordering.Application.Models;
using System.Threading.Tasks;

namespace Ordering.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
