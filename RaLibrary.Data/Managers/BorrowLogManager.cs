using RaLibrary.Data.Context;
using RaLibrary.Data.Entities;
using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Models;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public class BorrowLogManager
    {
        private RaLibraryContext _db = new RaLibraryContext();

        public IQueryable<BorrowLog> List()
        {
            return _db.BorrowLogs;
        }

        public async Task<BorrowLog> GetAsync(int id)
        {
            BorrowLog borrowLog = await _db.BorrowLogs.FindAsync(id);
            if (borrowLog == null)
            {
                throw new DbRecordNotFoundException();
            }
            else
            {
                return borrowLog;
            }
        }

        public BorrowLog GetActive(int bookId)
        {
            IQueryable<BorrowLog> logs = _db.BorrowLogs.Where(log => log.F_BookID == bookId && log.ReturnTime == null);

            int count = logs.Count();
            if (count == 0)
            {
                throw new DbRecordNotFoundException();
            }
            else if (count > 1)
            {
                throw new DbOperationException("Multiple logs exist.");
            }
            else
            {
                return logs.First();
            }
        }

        public async Task UpdateAsync(BorrowLog log)
        {
            if (!BorrowLogExists(log.Id))
            {
                throw new DbRecordNotFoundException();
            }

            log.ReturnTime = DateTime.UtcNow;

            _db.Entry(log).State = EntityState.Modified;

            await SaveChangesAsync();
        }

        public async Task<BorrowLog> CreateAsync(int bookId, string borrower)
        {
            int borrowedDays = int.Parse(ConfigurationManager.AppSettings.Get("MaxBookBorrowDays"));

            DateTime borrowTime = DateTime.UtcNow;

            BorrowLog borrowLog = new BorrowLog()
            {
                F_BookID = bookId,
                Borrower = borrower,
                BorrowTime = borrowTime,
                ExpectedReturnTime = borrowTime.AddDays(borrowedDays),
                ReturnTime = null
            };

            _db.BorrowLogs.Add(borrowLog);

            await SaveChangesAsync();

            return await GetAsync(borrowLog.Id);
        }

        public async Task DeleteAsync(int id)
        {
            BorrowLog borrowLog = await GetAsync(id);
            if (borrowLog == null)
            {
                throw new DbRecordNotFoundException();
            }

            _db.BorrowLogs.Remove(borrowLog);

            await SaveChangesAsync();
        }

        public BorrowLogDto ToDto(BorrowLog borrowLog)
        {
            return new BorrowLogDto()
            {
                Id = borrowLog.Id,
                F_BookID = borrowLog.F_BookID,
                Borrower = borrowLog.Borrower,
                BorrowTime = borrowLog.BorrowTime,
                ExpectedReturnTime = borrowLog.ExpectedReturnTime,
                ReturnTime = borrowLog.ReturnTime,
                RowVersion = borrowLog.RowVersion
            };
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        private bool BorrowLogExists(int id)
        {
            return _db.BorrowLogs.Count(log => log.Id == id) > 0;
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
    }
}
