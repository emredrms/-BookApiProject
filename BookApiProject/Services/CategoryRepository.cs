using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Models;

namespace BookApiProject.Services
{
    public class CategoryRepoitory : ICategoryRepository
    {
        private BookDbContext _categoryContext;

        public CategoryRepoitory(BookDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        public bool CategoryExist(int categoryId)
        {
            return _categoryContext.Categories.Any(c => c.Id == categoryId);
        }

        public ICollection<Book> GetAllBooksForCategory(int categoryId)
        {
            return _categoryContext.BookCategories.Where(c => c.Category.Id == categoryId).Select(b => b.Book).ToList();
        }

        public ICollection<Category> GetCategories()
        {
            return _categoryContext.Categories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetAllCategoriesForABook(int bookId)
        {
            return _categoryContext.BookCategories.Where(b => b.Book.Id == bookId).Select(c => c.Category).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public bool IsDuplicateCategoryName(int categoryId, string categoryName)
        {
            var category = _categoryContext.Categories.Where(c => c.Name.Trim().ToUpper() == categoryName.Trim().ToUpper() &&
                           c.Id != categoryId).FirstOrDefault();

            return category == null ? false : true;
        }
    }
}
