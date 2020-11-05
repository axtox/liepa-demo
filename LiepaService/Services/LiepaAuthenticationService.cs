using System.Net;
using System.Threading.Tasks;

namespace LiepaService.Services {
    public interface ILiepaAuthenticationService {
        public Task<bool> Authenticate(NetworkCredential credentials);
    }

    public class LiepaAuthenticationService : ILiepaAuthenticationService
    {
        // Hardcoded credential, for demo use only
        private readonly NetworkCredential _masterCredential = new NetworkCredential("master", "42");

        public async Task<bool> Authenticate(NetworkCredential credentials)
        {
            // imitating credential retritement
            var serverCredential = await Task.Factory.StartNew<NetworkCredential>(() => _masterCredential );

            return serverCredential.UserName == credentials.UserName &&
            serverCredential.Password == credentials.Password;
        }
    }
}