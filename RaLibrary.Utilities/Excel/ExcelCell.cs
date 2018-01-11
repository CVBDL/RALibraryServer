using System;
using System.Drawing;
using MsExcel = Microsoft.Office.Interop.Excel;

namespace RaLibrary.Utilities.Excel
{
    public class ExcelCell
    {
        private WeakReference workbook_;
        private MsExcel.Range cell_;

        public ExcelCell(MsExcel.Workbook workbook, MsExcel.Range cell)
        {
            workbook_ = new WeakReference(workbook);
            cell_ = cell;
        }

        public void SetFontColor(Color color)
        {
            if (workbook_.IsAlive)
            {
                cell_.Font.Color = ColorTranslator.ToOle(color);
            }
        }

        public void SetFontSize(int size)
        {
            if (workbook_.IsAlive)
            {
                cell_.Font.Size = size;
            }
        }

        public void SetBackgroundColor(Color color)
        {
            if (workbook_.IsAlive)
            {
                cell_.Interior.Color = ColorTranslator.ToOle(color);
            }
        }

        public void SetStyle(Color? fontColor, int? fontSize, Color? bgColor)
        {
            if (workbook_.IsAlive)
            {
                var book = (MsExcel.Workbook)workbook_.Target;
                var fColor = fontColor.HasValue ? fontColor.Value.ToString() : string.Empty;
                var fSize = fontSize.HasValue ? fontSize.Value.ToString() : string.Empty;
                var bColor = bgColor.HasValue ? bgColor.Value.ToString() : string.Empty;

                string StyleName = "Style_" + fColor + fSize + bColor;
                MsExcel.Style style;
                try
                {
                    style = book.Styles[StyleName];
                }
                catch
                {
                    style = book.Styles.Add(StyleName, Type.Missing);
                }
                if (fontColor.HasValue)
                {
                    style.Font.Color = fontColor.Value;
                }
                if (fontSize.HasValue)
                {
                    style.Font.Size = fontSize.Value;
                }
                if (bgColor.HasValue)
                {
                    style.Interior.Color = ColorTranslator.ToOle(bgColor.Value);
                }
                cell_.Style = StyleName;
            }
        }
    }
}
