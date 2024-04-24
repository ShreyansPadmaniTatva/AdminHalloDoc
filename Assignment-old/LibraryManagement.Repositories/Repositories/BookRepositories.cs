using LibraryManagement.Entities.DataContext;
using LibraryManagement.Entities.DataModels;
using LibraryManagement.Entities.ViewModel;
using LibraryManagement.Repositories.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagement.Repositories.Repositories
{
    public class BookRepositories : IBookRepositories
    {
        #region Constructor
        private readonly ApplicationDbContext _context;

        public BookRepositories(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetBooks
        /// <summary>
        /// Get All None deleted book with filter on borrower And BookName
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public BookModel GetBooks(BookModel bm)
        {
            BookModel dm = new BookModel();



            List<ViewBook> allData = (from book in _context.Books
                                      join borrower in _context.Borrowers
                                      on book.Borrowerid equals borrower.Borrowerid into BookGroup
                                      from borrowers in BookGroup.DefaultIfEmpty()
                                      where ((bm.SearchInput == null ||
                                            book.Bookname.ToLower().Contains(bm.SearchInput.ToLower()) || book.Author.ToLower().Contains(bm.SearchInput.ToLower()) ||
                                            book.City.ToLower().Contains(bm.SearchInput.ToLower())) && (book.Isdeleted == false)) && (bm.Borrowerid == null || borrowers.Borrowerid == bm.Borrowerid)
                                            orderby book.Createddate
                                            select new ViewBook
                                      {
                                          Bookname = book.Bookname,
                                          Isdeleted = false,
                                          Borrowerid = book.Borrowerid,
                                          Author = book.Author,
                                          Bookid = book.Bookid,
                                          BorrowerName = borrowers.Borrowername,
                                          City = book.City,
                                          Dateofissue = (DateTime)book.Dateofissue,
                                          Genere = (short)book.Genere,
                                          Createddate = book.Createddate,

                                      }).OrderBy(x => x.Createddate).ToList();


           

            dm.TotalPages = (int)Math.Ceiling((double)allData.Count() / bm.PageSize);
            dm.BookList = allData.Skip((bm.CurrentPage - 1) * bm.PageSize).Take(bm.PageSize).ToList();
            dm.PageSize = bm.PageSize;
            dm.CurrentPage = bm.CurrentPage;
            return dm;
        }
        #endregion

        #region AddBook
        /// <summary>
        /// When Book Will Be add Then Chek If Borrower Is nEw Or Old If New Then First Insert Borrow After Book
        /// </summary>
        /// <param name="vb"></param>
        /// <returns></returns>
        public async Task<bool> AddBook(ViewBook vb)
        {
            try
            {
                var borrower = await IsBorrowerExist(vb.BorrowerName.Trim() != null ? vb.BorrowerName.Trim() : null);
                if (borrower != null)
                {
                    Book book = new Book();
                    book.Author = vb.Author;
                    book.Bookname = vb.Bookname;
                    book.Dateofissue = vb.Dateofissue;
                    book.City = vb.City;
                    book.Genere = vb.Genere;
                    book.Isdeleted = false;
                    book.Borrowerid = borrower.Borrowerid;
                    book.Createddate = DateTime.Now;
                    _context.Books.Add(book);
                    _context.SaveChanges();
                }
                else
                {
                    Borrower borrower1 = new Borrower();
                    borrower1.Borrowername = vb.BorrowerName.Trim();
                    borrower1.City = vb.City;
                    borrower1.Createddate = DateTime.Now;
                    _context.Borrowers.Add(borrower1);
                    _context.SaveChanges();

                    Book book = new Book();
                    book.Author = vb.Author;
                    book.Bookname = vb.Bookname;
                    book.Dateofissue = vb.Dateofissue;
                    book.City = vb.City;
                    book.Genere = vb.Genere;
                    book.Createddate = DateTime.Now;
                    book.Isdeleted = false;
                    book.Borrowerid = borrower1.Borrowerid;
                    _context.Books.Add(book);
                    _context.SaveChanges();

                }
                return true;

            }
            catch (Exception ex)
            {

            }
            return true;
        }
        #endregion

        #region EditBook
        /// <summary>
        /// Edit Book With That Id And Chake Borrower Is new Or Old
        /// </summary>
        /// <param name="vb"></param>
        /// <returns></returns>
        public async Task<bool> EditBook(ViewBook vb)
        {
            try
            {
                var borrower = await IsBorrowerExist(vb.BorrowerName.Trim() != null ? vb.BorrowerName.Trim() : null);
                if (borrower != null)
                {
                    Book book = _context.Books.Where(r => r.Bookid == vb.Bookid).FirstOrDefault();
                    book.Author = vb.Author;
                    book.Bookname = vb.Bookname;
                    book.Dateofissue = vb.Dateofissue;
                    book.City = vb.City;
                    book.Genere = vb.Genere;
                    book.Borrowerid = borrower.Borrowerid;
                    book.Modifieddate = DateTime.Now;
                    _context.Books.Update(book);
                    _context.SaveChanges();
                }
                else
                {
                    Borrower borrower1 = new Borrower();
                    borrower1.Borrowername = vb.BorrowerName.Trim();
                    borrower1.City = vb.City;
                    borrower1.Createddate = DateTime.Now;
                    _context.Borrowers.Add(borrower1);
                    _context.SaveChanges();

                    Book book = _context.Books.Where(r => r.Bookid == vb.Bookid).FirstOrDefault();
                    book.Author = vb.Author;
                    book.Bookname = vb.Bookname;
                    book.Dateofissue = vb.Dateofissue;
                    book.City = vb.City;
                    book.Genere = vb.Genere;
                    book.Modifieddate = DateTime.Now;
                    book.Borrowerid = borrower1.Borrowerid;
                    _context.Books.Update(book);
                    _context.SaveChanges();

                }
                return true;

            }
            catch (Exception ex)
            {

            }
            return true;
        }
        #endregion

        #region IsBorrowerExist
        /// <summary>
        /// Check BorrowerExit Or Not 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Borrower> IsBorrowerExist(string name)
        {
            return await _context.Borrowers.Where(r => r.Borrowername.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }
        #endregion

        #region GetBookById
        /// <summary>
        /// Get Detalis Of Book By BookId
        /// </summary>
        /// <param name="BookId"></param>
        /// <returns></returns>
        public ViewBook GetBookById(int BookId)
        {
            var v = (from book in _context.Books
                     join borrower in _context.Borrowers
                     on book.Borrowerid equals borrower.Borrowerid into BookGroup
                     from borrowers in BookGroup.DefaultIfEmpty()
                     where (book.Bookid == BookId)
                     orderby book.Createddate
                     select new ViewBook
                     {
                         Bookname = book.Bookname,
                         Isdeleted = false,
                         Borrowerid = book.Borrowerid,
                         Author = book.Author,
                         Bookid = book.Bookid,
                         BorrowerName = borrowers.Borrowername,
                         City = book.City,
                         Dateofissue = (DateTime)book.Dateofissue,
                         Genere = (short)book.Genere,
                         Createddate = book.Createddate,

                     })
                .FirstOrDefault(); 
            return v;
        }
        #endregion

        #region RemoveBook
        /// <summary>
        /// Remove Book soft delete here
        /// </summary>
        /// <param name="BookId"></param>
        /// <returns></returns>

        public async Task<bool> RemoveBook(int BookId)
        {
            try
            {
                    Book book = _context.Books.Where(r => r.Bookid == BookId).FirstOrDefault();
                if (book != null)
                {
                    book.Isdeleted = true;
                    _context.Books.Update(book);
                    _context.SaveChanges();
                }
                return true;

            }
            catch (Exception ex)
            {

            }
            return true;
        }
        #endregion

        #region ComboBoxBorrower
        /// <summary>
        /// DropDown OF borrower
        /// </summary>
        /// <returns></returns>
        public async Task<List<BrrowerComboBox>> ComboBoxBorrower()
        {
            return await _context.Borrowers.Select(result => new BrrowerComboBox { BrrowerId = result.Borrowerid,BrrowerName = result.Borrowername }).ToListAsync();
        }
        #endregion
    }
}
