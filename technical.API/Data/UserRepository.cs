using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using technical.API.Helpers;
using technical.API.Models;

namespace technical.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            this._context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            this._context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            this._context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await this._context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await this._context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await this._context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = this._context.Users.Include(p => p.Photos).AsQueryable();

            // if collection, then show all sub-accounts
            if (userParams.Gender != "collection")
                users = users.Where(u => u.Id == userParams.UserId);
            else
                users = users.OrderBy(u => u.Gender);

            if (userParams.OlderThanDays != 0)
            {
                var minActive = DateTime.Today.AddDays(-userParams.OlderThanDays - 1);

                users = users.Where(u => u.LastActive <= minActive);
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);

        }

        public async Task<bool> SaveAll()
        {
            return await this._context.SaveChangesAsync() > 0;
        }
    }
}