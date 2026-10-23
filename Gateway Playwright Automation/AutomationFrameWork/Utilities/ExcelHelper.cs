using ClosedXML.Excel;

namespace AutomationFrameWork.Utilities

{

    public class ExcelHelper

    {

        public static Dictionary<string, string> ReadExcel(string filePath, string sheetName, string tagName)

        {

            var data = new Dictionary<string, string>();

            using (var workbook = new XLWorkbook(filePath))

            {

                var worksheet = workbook.Worksheet(sheetName);

                var headerRow = worksheet.Row(1);

                int rowCount = worksheet.LastRowUsed().RowNumber();

                int colCount = headerRow.LastCellUsed().Address.ColumnNumber;

                for (int row = 2; row <= rowCount; row++)

                {

                    if (worksheet.Row(row).Cell(1).GetString().Trim().Equals(tagName, StringComparison.OrdinalIgnoreCase))

                    {

                        for (int col = 1; col <= colCount; col++)

                        {

                            string key = headerRow.Cell(col).GetString().Trim();

                            string value = worksheet.Row(row).Cell(col).GetString().Trim();

                            data[key] = value;

                        }

                        break;

                    }

                }

            }

            return data;

        }

    }

}

