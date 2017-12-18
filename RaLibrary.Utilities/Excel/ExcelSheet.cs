using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MsExcel = Microsoft.Office.Interop.Excel;

namespace RaLibrary.Utilities.Excel
{
    public class ExcelSheet : IDisposable
    {
        #region ctor
        private WeakReference workbook_;
        private MsExcel.Worksheet sheet_;
        private MsExcel.Range rng_;

        public ExcelSheet(MsExcel.Workbook workbook, MsExcel.Worksheet sheet)
        {
            workbook_ = new WeakReference(workbook);
            sheet_ = sheet;
            rng_ = sheet_.Cells[1];
        }
        #endregion

        #region Public Methods
        public List<string> ReadColumn(int row, string scol)
        {
            var list = new List<string>();
            var col = CovertCharToColumn(scol);
            while (true)
            {
                var data = ReadCell(row, col);
                if (string.IsNullOrWhiteSpace(data))
                    break;

                list.Add(data);
            }
            return list;
        }

        /// <summary>
        /// Read the sheet cell by row and column
        /// </summary>
        /// <param name="row">Row start from index 0.</param>
        /// <param name="col">Column start from index 0.</param>
        /// <returns>The content of the cell.</returns>
        public string ReadCell(int row, int col)
        {
            var data = rng_.get_Offset(row, col).Value2;
            if (data != null)
                return data.ToString();
            return string.Empty;
        }

        /// <summary>
        /// Write the excel cell.
        /// </summary>
        /// <param name="row">Row start from index 0.</param>
        /// <param name="col">Column start from index 0.</param>
        public void WriteCell(int row, int col, object value)
        {
            rng_.get_Offset(row, col).Value2 = value;
        }

        public ExcelCell GetCell(int row, int col)
        {
            if (workbook_.IsAlive)
            {
                var book = (MsExcel.Workbook)workbook_.Target;
                var cell = rng_.get_Offset(row, col);
                return new ExcelCell(book, cell);
            }
            return null;
        }

        /// <summary>
        /// Read the cell by row number and char.
        /// </summary>
        /// <param name="s">"A" ~ "z" </param>
        /// <returns></returns>
        public string ReadCell(int row, string s)
        {
            var col = CovertCharToColumn(s);
            return ReadCell(row, col);
        }

        /// <summary>
        /// Read the cell by row number and char.
        /// </summary>
        /// <param name="c">'A' ~ 'z' </param>
        /// <returns></returns>
        public string ReadCell(int row, char c)
        {
            return ReadCell(row, c.ToString());
        }

        /// <remarks>
        /// The Hyperlinks index start from 1.
        /// </remarks>
        public ExcelLink ParseTheHyperlink(int row, int col)
        {
            var links = rng_.get_Offset(row, col).Hyperlinks;
            if (links == null || links.Count == 0)
                return null;

            var data = ReadCell(row, col);
            var array = data.Split('!');
            var index = array[1].IndexOf(array[1].First(i => char.IsNumber(i)));
            var elink = new ExcelLink
            {
                Text = data,
                Sheet = array[0],
                Column = array[1].Substring(0, index),
                Row = array[1].Substring(index)
            };

            return elink;
        }

        public ExcelLink ParseTheHyperlink(int row, string s)
        {
            var col = CovertCharToColumn(s);
            return ParseTheHyperlink(row, col);
        }

        public ExcelLink ParseTheHyperlink(int row, char c)
        {
            return ParseTheHyperlink(row, c.ToString());
        }

        private int CovertCharToColumn(string s)
        {
            if (s.Length == 1)
            {
                var lowChar = s.ToUpper().ToCharArray()[0];
                return lowChar - 65;
            }
            else
            {
                var arrays = s.ToUpper().ToCharArray();
                var column = 0;
                for (var i = 0; i < arrays.Length; i++)
                {
                    var value = arrays[i] - 65;
                    var delta = 1;
                    if (i >= 1)
                    {
                        for (var j = i; j > 0; j--)
                        {
                            delta *= 26;
                        }
                        value = (value + 1) * delta;
                    }
                    column += value;
                }

                return column;
            }
        }
        #endregion

        #region Dispose
        private bool disposed_;
        public void Dispose()
        {
            if (!disposed_)
            {
                if (rng_ != null)
                    Marshal.FinalReleaseComObject(rng_);

                if (sheet_ != null)
                    Marshal.FinalReleaseComObject(sheet_);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            rng_ = null;
            sheet_ = null;

            disposed_ = true;
        }

        public void Close()
        {
            Dispose();
        }
        #endregion
    }
}
