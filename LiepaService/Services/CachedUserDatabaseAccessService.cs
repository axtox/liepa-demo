using System;
using System.Threading.Tasks;
using LiepaService.Models;
using LiepaService.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace LiepaService.Services {
    public interface IUserDatabaseAccessService {
        public Task<User> Get(int id);
        public Task<User> Put(User newUser);
        public Task<User> Delete(int id);
        public Task<User> Update(User updatedUser);
        public Task<Status> GetStatus(string statusName);
    }
    public class CachedUserDatabaseAccessService : IUserDatabaseAccessService
    {
        private IMemoryCache _cache;
        private MemoryCacheEntryOptions _cacheOptions;
        private readonly LiepaDemoDatabaseContext _databaseContext;

        public CachedUserDatabaseAccessService(IMemoryCache cache, LiepaDemoDatabaseContext databaseContext) : 
                                        this(cache, databaseContext, TimeSpan.FromMinutes(1)) {}

        public CachedUserDatabaseAccessService(IMemoryCache cache, LiepaDemoDatabaseContext databaseContext, TimeSpan cacheExpirationTime) {
            _cache = cache;
            _databaseContext = databaseContext;

            _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(relative: cacheExpirationTime);
        }

        public async Task<User> Get(int id)
        {
            return await _cache.GetOrCreateAsync(id, CacheOutEntity);
        }
        
        private async Task<User> CacheOutEntity(ICacheEntry cachedEntity) {
            cachedEntity.SetOptions(_cacheOptions);

            var foundUser = await _databaseContext.Users.FindAsync(cachedEntity.Key);

            if(foundUser == null)
                throw new NoSuchEntityFoundException((int)cachedEntity.Key, $"Unable to find entity with id:{(int)cachedEntity.Key}.");

            return foundUser;
        }

        public async Task<User> Delete(int id)
        {
            var removedUser = await Get(id);

            _databaseContext.Users.Remove(removedUser);

            await _databaseContext.SaveChangesAsync();

            _cache.Remove(id);

            return removedUser;
        }

        public async Task<User> Put(User newUser)
        {
            var existedUser = await _databaseContext.Users.FindAsync(newUser.UserId);
            if(existedUser != null)
                throw new EntityAlreadyExistedException(newUser.UserId, $"The user with id:{newUser.UserId} already exists.");

            var databaseUser = await _databaseContext.Users.AddAsync(newUser);

            await _databaseContext.SaveChangesAsync();

            var cachedEntity = _cache.Set(databaseUser.Entity.UserId, databaseUser.Entity, _cacheOptions);

            return databaseUser.Entity;
        }

        public async Task<User> Update(User updatedUser)
        {
            var removedUser = await Get(updatedUser.UserId);
            if(removedUser == null)
                throw new NoSuchEntityFoundException(updatedUser.UserId, $"Unable to update entity with id:{updatedUser.UserId}. There's no such user could be found");

            _cache.Remove(updatedUser.UserId);

            var user = _databaseContext.Users.Update(updatedUser);

            _cache.Set(updatedUser.UserId, updatedUser, _cacheOptions);

            return user.Entity;
        }

        public async Task<Status> GetStatus(string statusName)
        {
            var status = _databaseContext.UserStatuses.SingleOrDefault(status => string.Equals(status.Value, statusName));
            if(status == null)
                throw new ArgumentException("Provided User Status is incorrect.");

            return status;
        }
    }
}