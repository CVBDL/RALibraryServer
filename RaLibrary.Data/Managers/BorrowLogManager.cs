using RaLibrary.Data.Context;
using RaLibrary.Data.Entities;
using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Models;
using System;
using System.Collections.Generic;
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
        #region Fields

        private RaLibraryContext _db = new RaLibraryContext();

        #endregion Fields

        public IQueryable<BorrowLogDto> List()
        {
            List<BorrowLogDto> result = new List<BorrowLogDto>();

            foreach (BorrowLog borrowLog in _db.BorrowLogs)
            {
                result.Add(ToDto(borrowLog));
            }

            return result.AsQueryable();
        }

        public async Task<BorrowLogDto> GetAsync(int id)
        {
            BorrowLog borrowLog = await FindAsync(id);

            return ToDto(borrowLog);
        }

        public BorrowLogDto GetActive(int bookId)
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
                return ToDto(logs.First());
            }
        }

        public async Task UpdateAsync(BorrowLogDto logDto)
        {
            BorrowLog borrowLog = await FindAsync(logDto.Id);

            borrowLog.ReturnTime = DateTime.UtcNow;

            DbEntityEntry<BorrowLog> dbEntry = _db.Entry(borrowLog);

            // Enable database update concurrency checking.
            dbEntry.Property(b => b.RowVersion).OriginalValue = logDto.RowVersion;
            dbEntry.Property(b => b.RowVersion).IsModified = true;

            _db.Entry(borrowLog).State = EntityState.Modified;

            await SaveChangesAsync();
        }

        public async Task<BorrowLogDto> CreateAsync(BorrowLogDto logDto)
        {
            int borrowedDays = int.Parse(ConfigurationManager.AppSettings.Get("MaxBookBorrowDays"));

            DateTime borrowTime = DateTime.UtcNow;
            DateTime expectedReturnTime = borrowTime.AddDays(borrowedDays);

            BorrowLog borrowLog = new BorrowLog()
            {
                F_BookID = logDto.F_BookID,
                Borrower = logDto.Borrower,
                BorrowTime = borrowTime,
                ExpectedReturnTime = expectedReturnTime,
                ReturnTime = null
            };

            _db.BorrowLogs.Add(borrowLog);

            await SaveChangesAsync();

            return await GetAsync(borrowLog.Id);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        private async Task<BorrowLog> FindAsync(int id)
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

        private BorrowLogDto ToDto(BorrowLog borrowLog)
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
    }
}
