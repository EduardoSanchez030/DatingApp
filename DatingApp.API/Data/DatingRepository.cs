using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.DatingApp;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;

        }
        void IDatingRepository.Add<T>(T entity)
        {
           _context.Add(entity);
        }

        void IDatingRepository.Delete<T>(T entity)
        {
            _context.Remove(entity);
        }

        async Task<User> IDatingRepository.GetUser(int id)
        {
            return await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        async Task<IEnumerable<User>> IDatingRepository.GetUsers()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        async Task<bool> IDatingRepository.SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}