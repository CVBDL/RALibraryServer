using RaLibrary.Data.Context;
using RaLibrary.Data.Entities;
using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public class BorrowManager : IBorrowManager
    {
        #region Fields

        private RaLibraryContext _db = new RaLibraryContext();

        #endregion Fields

        /// <summary>
        /// List all borrows.
        /// </summary>
        /// <returns></returns>
        public IQueryable<BorrowDto> List()
        {
            List<Borrow> borrows = _db.Borrows.ToList();

            var result = new List<BorrowDto>();
            foreach (Borrow borrow in borrows)
            {
                result.Add(ToDto(borrow));
            }

            return result.AsQueryable();
        }

        public IQueryable<BorrowDto> List(string borrower)
        {
            if (string.IsNullOrWhiteSpace(borrower))
            {
                return List();
            }

            IQueryable<Borrow> borrows = _db.Borrows
                .Where(b => b.Borrower == borrower);

            List<BorrowDto> result = new List<BorrowDto>();
            foreach (Borrow borrow in borrows)
            {
                result.Add(ToDto(borrow));
            }

            return result.AsQueryable();
        }

        /// <summary>
        /// Get a single borrow record.
        /// </summary>
        /// <param name="id">The borrow id.</param>
        /// <returns></returns>
        public async Task<Borrow> GetAsync(int id)
        {
            Borrow borrow = await FindAsync(id);

            return borrow;
        }

        /// <summary>
        /// Get a single borrow log by book id.
        /// </summary>
        /// <param name="id">The book id.</param>
        /// <returns></returns>
        public Borrow GetByBookId(int id)
        {
            Borrow borrow = _db.Borrows
                .Where(b => b.FK_Book_Id == id)
                .FirstOrDefault();

            if (borrow == null)
            {
                throw new DbRecordNotFoundException();
            }

            return borrow;
        }

        public async Task<Borrow> CreateAsync(Borrow borrow)
        {
            int borrowedDays = int.Parse(ConfigurationManager.AppSettings.Get("MaxBookBorrowDays"));

            DateTime borrowTime = DateTime.UtcNow;
            DateTime expectedReturnTime = borrowTime.AddDays(borrowedDays);

            Borrow b = new Borrow()
            {
                FK_Book_Id = borrow.FK_Book_Id,
                Borrower = borrow.Borrower,
                BorrowTime = borrowTime,
                ExpectedReturnTime = expectedReturnTime
            };

            _db.Borrows.Add(b);

            await SaveChangesAsync();

            return await GetAsync(b.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The borrow id.</param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            Borrow borrow = await FindAsync(id);
            if (borrow == null)
            {
                throw new DbRecordNotFoundException();
            }

            _db.Borrows.Remove(borrow);

            await SaveChangesAsync();
        }

        public async Task DeleteByBookIdAsync(int bookId)
        {
            Borrow borrow = GetByBookId(bookId);

            _db.Borrows.Remove(borrow);

            await SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        /// <summary>
        /// Get a single borrow record.
        /// </summary>
        /// <param name="id">The borrow id.</param>
        /// <returns></returns>
        private async Task<Borrow> FindAsync(int id)
        {
            Borrow borrow = await _db.Borrows.FindAsync(id);
            if (borrow == null)
            {
                throw new DbRecordNotFoundException();
            }
            else
            {
                return borrow;
            }
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbOperationException("Concurrency updating conflicts detected.");
            }
            catch (DbEntityValidationException)
            {
                throw new DbOperationException("Validation of database property values failed.");
            }
            catch
            {
                throw new DbOperationException();
            }
        }

        private BorrowDto ToDto(Borrow borrow)
        {
            return new BorrowDto()
            {
                Borrower = borrow.Borrower,
                BorrowTime = borrow.BorrowTime,
                ExpectedReturnTime = borrow.ExpectedReturnTime,
                Book = borrow.Book
            };
        }
    }
}
