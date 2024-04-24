using LibraryManagement.Entities.DataModels;
using LibraryManagement.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories.Repositories.Interface
{
    public interface IBookRepositories
    {
        BookModel GetBooks(BookModel bm);
        Task<bool> AddBook(ViewBook vb);
        Task<bool> EditBook(ViewBook vb);
        ViewBook GetBookById(int BookId);
        Task<bool> RemoveBook(int BookId);
        Task<List<BrrowerComboBox>> ComboBoxBorrower();

    }
}
