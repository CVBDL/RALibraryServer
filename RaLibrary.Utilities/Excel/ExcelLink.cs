namespace RaLibrary.Utilities.Excel
{
    /// <summary>
    /// Provide a object to store the MsExcel Link data.
    /// </summary>
    /// <example>
    ///  Text: IO_Tags
    ///  Sheet:IO_Tags
    ///  Column: A
    ///  Row: 71
    /// </example>
    public class ExcelLink
    {
        public string Text { get; set; }

        public string Sheet { get; set; }

        public string Column { get; set; }

        public string Row { get; set; }
    }
}
