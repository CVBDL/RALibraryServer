using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MsExcel = Microsoft.Office.Interop.Excel;

namespace RaLibrary.Utilities.Excel
{
    /// <summary>
    /// Provide easy wrapper for the Excel interop operation.
    /// </summary>
    /// <remarks>
    /// Currently the wrapper is available for the Office 2013.(.xls and .xlsx)
    /// </remarks>
    /// <see cref="https://msdn.microsoft.com/zh-cn/library/aa168292(office.11).aspx#EDAA"/>
    public class ExcelFile : IDisposable
    {
        #region ctor
        private MsExcel.Application app_ = new MsExcel.Application();
        private MsExcel.Workbook workBook_;
        private MsExcel.Sheets sheets_;

        private List<ExcelSheet> activedSheets_ = new List<ExcelSheet>();
        #endregion

        #region methods
        public void ReadFile(string fullPath)
        {
            try
            {
                if (disposed_ || app_ == null)
                    throw new ObjectDisposedException("The file has been disposed");

                workBook_ = app_.Workbooks.Open(fullPath);
                if (workBook_ == null)
                    throw new Exception();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ExcelSheet ReadSheet(string name)
        {
            try
            {
                sheets_ = workBook_.Worksheets;
                var worksheet = sheets_[name] as MsExcel.Worksheet;
                var sheet = new ExcelSheet(workBook_, worksheet);
                activedSheets_.Add(sheet);
                return sheet;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Get the sheet by sheet index.
        /// </summary>
        /// <remarks>
        /// Warning: Index start with 1 rather than 0. 
        /// </remarks>
        public ExcelSheet ReadSheet(int index)
        {
            var worksheet = workBook_.Worksheets[index] as MsExcel.Worksheet;
            var sheet = new ExcelSheet(workBook_, worksheet);
            activedSheets_.Add(sheet);
            return sheet;
        }

        public void Create()
        {
            try
            {
                if (disposed_ || app_ == null)
                    throw new ObjectDisposedException("The file has been disposed");

                workBook_ = app_.Workbooks.Add();
                if (workBook_ == null)
                    throw new Exception();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ExcelSheet AddSheet(string sheetName = null)
        {
            try
            {
                if (workBook_ != null)
                {
                    var worksheet = workBook_.Worksheets.Add() as MsExcel.Worksheet;
                    if (!string.IsNullOrWhiteSpace(sheetName))
                    {
                        worksheet.Name = sheetName;
                    }
                    var sheet = new ExcelSheet(workBook_, worksheet);
                    activedSheets_.Add(sheet);
                    return sheet;
                }
            }
            catch
            {
                Close();
            }
            return null;
        }

        public void Save()
        {
            workBook_.Save();
        }

        public void SaveAs(string fullPath)
        {
            workBook_.SaveAs(fullPath);
        }

        #endregion

        #region dispose
        public void Close()
        {
            Dispose();
        }

        private bool disposed_;
        public void Dispose()
        {
            if (!this.disposed_)
            {
                activedSheets_.ForEach(i => i.Close());

                if (sheets_ != null)
                {
                    Marshal.FinalReleaseComObject(sheets_);
                }

                if (workBook_ != null)
                {
                    workBook_.Close(false, Type.Missing, Type.Missing);
                    Marshal.FinalReleaseComObject(workBook_);
                }

                if (app_ != null)
                {
                    app_.Quit();
                    Marshal.FinalReleaseComObject(app_);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                sheets_ = null;
                workBook_ = null;
                app_ = null;

                disposed_ = true;
            }
        }
        #endregion
    }
}
