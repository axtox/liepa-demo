using System;
using System.Threading.Tasks;
using LiepaService.Models;
using LiepaService.Models.Views;
using Microsoft.Extensions.Caching.Memory;

namespace LiepaService.Services {
    public interface IDatabaseAccessService {
        public Task<UserView> Get(int id);
        public Task<bool> Put(UserView newUser);
        public Task<bool> Delete(int id);
    }
    public class CachedDatabaseAccessService : IDatabaseAccessService
    {
        private IMemoryCache _cache;
        private MemoryCacheEntryOptions _cacheOptions;
        private readonly LiepaDemoDatabaseContext _databaseContext;

        public CachedDatabaseAccessService(IMemoryCache cache, LiepaDemoDatabaseContext databaseContext) : 
                                        this(cache, databaseContext, TimeSpan.FromMinutes(10)) {}

        public CachedDatabaseAccessService(IMemoryCache cache, LiepaDemoDatabaseContext databaseContext, TimeSpan cacheExpirationTime) {
            _cache = cache;
            _databaseContext = databaseContext;

            _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(relative: cacheExpirationTime);
        }

        public async Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserView> Get(int id)
        {
            var cachedUser = await _cache.GetOrCreateAsync(id, CacheOutEntity);

            return new UserView(cachedUser);
        }
        
        private async Task<User> CacheOutEntity(ICacheEntry cachedEntity) {
            cachedEntity.SetOptions(_cacheOptions);

            return await _databaseContext.Users.FindAsync(cachedEntity.Key);
        }

        public async Task<bool> Put(UserView newUser)
        {
            throw new System.NotImplementedException();
        }
    }
}