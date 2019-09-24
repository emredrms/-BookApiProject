using BookApiProject.Models;
using System.Collections.Generic;

namespace BookApiProject.Services
{
    interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Category> GetAllCategoriesForABook(int bookId);
        ICollection<Book> GetAllBooksForCategory(int categoryId);
        bool CategoryExist(int categoryId);
    }
}
