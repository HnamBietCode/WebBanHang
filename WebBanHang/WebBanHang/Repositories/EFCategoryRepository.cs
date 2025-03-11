using WebBanHang.Repositories;
using WebBanHang.Models;
using WebBanHang.Data;
using Microsoft.EntityFrameworkCore;

namespace WebBanHang.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private  AppDbContext _context;
        public EFCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }
        }


        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
        }


        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            return category;
        }


        public async Task UpdateAsync(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with id {category.Id} not found.");
            }

            existingCategory.Name = category.Name; // Cập nhật các thuộc tính cần thiết ở đây

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
        }

    }
}
