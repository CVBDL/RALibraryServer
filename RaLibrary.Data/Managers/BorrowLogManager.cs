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
    public class BorrowLogManager : IBorrowLogManager
    {
        #region Fields

        private RaLibraryContext _db = new RaLibraryContext();

        #endregion Fields

        /// <summary>
        /// List all logs from data store.
        /// </summary>
        /// <returns></returns>
        public IQueryable<BorrowLogDto> List()
        {
            List<BorrowLogDto> result = new List<BorrowLogDto>();

            foreach (BorrowLog borrowLog in _db.BorrowLogs)
            {
                result.Add(ToDto(borrowLog));
            }

            return result.AsQueryable();
        }

        /// <summary>
        /// Get a single log from data store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BorrowLogDto> GetAsync(int id)
        {
            BorrowLog borrowLog = await FindAsync(id);

            return ToDto(borrowLog);
        }

        /// <summary>
        /// Get a log that the associated book is not returned.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Fill "ReturnTime" into an existing log, then the log will be "closed".
        /// </summary>
        /// <param name="logDto"></param>
        /// <returns></returns>
        public async Task CloseAsync(BorrowLogDto logDto)
        {
            BorrowLog borrowLog = await FindAsync(logDto.Id);

            borrowLog.ReturnTime = DateTime.UtcNow;

            ModifyDbEntityEntry(borrowLog, logDto.RowVersion);

            await SaveChangesAsync();
        }

        /// <summary>
        /// Create a borrow log.
        /// </summary>
        /// <param name="logDto"></param>
        /// <returns></returns>
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

        private void ModifyDbEntityEntry(BorrowLog borrowLog, byte[] rowVersion)
        {
            DbEntityEntry<BorrowLog> dbEntry = _db.Entry(borrowLog);

            // Enable database update concurrency checking.
            dbEntry.Property(b => b.RowVersion).OriginalValue = rowVersion;
            dbEntry.Property(b => b.RowVersion).IsModified = true;

            dbEntry.State = EntityState.Modified;
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
