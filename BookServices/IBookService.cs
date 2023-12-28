using BlazorBookManagementSystem.Models;

namespace BlazorBookManagementSystem.BookServices
{
    public interface IBookService
    {
        Task<(bool Success, string Message)> ManageBookAsync(Book book);
        Task<(bool Success, string Message)> DeleteBookAsync(int id);
        Task<List<Book>> GetBookAsync();
    }
}
