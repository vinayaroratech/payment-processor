using System.Threading.Tasks;

namespace Payments.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);
    }
}
