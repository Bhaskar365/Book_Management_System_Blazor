using BlazorBookManagementSystem.Data;
using BlazorBookManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorBookManagementSystem.BookServices
{
    public class BookService : IBookService
    {
        private readonly AppDbContext appDbContext;
        public BookService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<(bool Success, string Message)> DeleteBookAsync(int id)
        {
            if (id <= 0) return ErrorMessage();

            var bookToDelete = await appDbContext.BooksTable.FirstOrDefaultAsync(_ => _.Id == id);
            if (bookToDelete is null) return ErrorMessage();
            appDbContext.BooksTable.Remove(bookToDelete);
            await appDbContext.SaveChangesAsync();
            return SuccessMessage();
        }

        public async Task<List<Book>> GetBookAsync() => await appDbContext.BooksTable.ToListAsync();

        public async Task<(bool Success, string Message)> ManageBookAsync(Book book)
        {
            if (book == null) return ErrorMessage();

            if(book.Id == 0) 
            {
                if (!await IsBookAlreadyAdded(book.Title!)) 
                {
                    appDbContext.BooksTable.Add(book);
                    await appDbContext.SaveChangesAsync();
                    return SuccessMessage();
                }
            }

            var bookToUpdate = await appDbContext.BooksTable.FirstOrDefaultAsync(x=>x.Id == book.Id);
            if(bookToUpdate is null) return ErrorMessage();

            bookToUpdate.Title = book.Title;
            bookToUpdate.Description = book.Description;
            bookToUpdate.Image = book.Image;
            await appDbContext.SaveChangesAsync();
            return SuccessMessage();
        }

        private static (bool, string) SuccessMessage() => (true, "Process successfully completed");
        private static (bool, string) ErrorMessage() => (false, "Error occured while processing");
        private async Task<bool> IsBookAlreadyAdded(string bookName) 
        {
            var book = await appDbContext.BooksTable.Where(x => x.Title.ToLower().Equals(bookName)).FirstOrDefaultAsync();
            return book is not null;
        }
    }
}
