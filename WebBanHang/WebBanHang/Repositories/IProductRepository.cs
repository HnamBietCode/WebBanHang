using WebBanHang.Models;
namespace WebBanHang.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAync(Product Product);
        Task DeleteAsync(int id);
    }
}
