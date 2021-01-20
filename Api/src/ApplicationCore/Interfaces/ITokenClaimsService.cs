using System.Threading.Tasks;

namespace MistyDaycare.ApplicationCore.Interfaces
{
    public interface ITokenClaimsService
    {
        Task<string> GetTokenAsync(string username);
    }
}