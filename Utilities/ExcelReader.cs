using OfficeOpenXml;

namespace SeleniumReqnrollFramework.Utilities;

/// <summary>
/// ExcelReader - Utility class for reading test data from Excel files
/// Uses EPPlus library for .xlsx format support
/// </summary>
public static class ExcelReader
{
    private static readonly string TestDataPath = Path.Combine(AppContext.BaseDirectory, "TestData/");

    static ExcelReader()
    {
        // Set EPPlus license context (required for EPPlus 5+)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    /// <summary>
    /// Read employee data from Excel file
    /// Reads the first data row (row 2) after headers
    /// </summary>
    public static Dictionary<string, string> GetEmployeeData(string fileName, string? sheetName)
    {
        return GetRowData(fileName, sheetName, 2); // Row 2 is first data row (1 is header in EPPlus)
    }

    /// <summary>
    /// Read employee data from default file and sheet
    /// Uses "Test Data.xlsx" and first sheet
    /// </summary>
    public static Dictionary<string, string> GetEmployeeData()
    {
        return GetEmployeeData("Test Data.xlsx", null);
    }

    /// <summary>
    /// Read a specific row from Excel file
    /// </summary>
    public static Dictionary<string, string> GetRowData(string fileName, string? sheetName, int rowIndex)
    {
        var data = new Dictionary<string, string>();
        string filePath = Path.Combine(TestDataPath, fileName);

        Console.WriteLine($"[INFO] Reading Excel file: {filePath}");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"[ERROR] Excel file not found: {filePath}");
        }

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            // Get sheet - either by name or first sheet
            ExcelWorksheet sheet;
            if (!string.IsNullOrEmpty(sheetName))
            {
                sheet = package.Workbook.Worksheets[sheetName] 
                    ?? throw new Exception($"[ERROR] Sheet not found: {sheetName}");
            }
            else
            {
                sheet = package.Workbook.Worksheets[0];
            }

            Console.WriteLine($"[INFO] Reading from sheet: {sheet.Name}");

            // Get header row (first row)
            int colCount = sheet.Dimension?.Columns ?? 0;
            if (colCount == 0)
            {
                throw new Exception("[ERROR] Sheet is empty");
            }

            // Read each cell and map header -> value
            for (int col = 1; col <= colCount; col++)
            {
                string? header = sheet.Cells[1, col].Value?.ToString()?.Trim();
                string? value = sheet.Cells[rowIndex, col].Value?.ToString()?.Trim() ?? "";

                if (!string.IsNullOrEmpty(header))
                {
                    // Handle date cells
                    var cell = sheet.Cells[rowIndex, col];
                    if (cell.Value is DateTime dateValue)
                    {
                        value = dateValue.ToString("dd-MM-yyyy");
                    }
                    // Handle numeric cells (like Employee ID)
                    else if (cell.Value is double numValue)
                    {
                        if (numValue == Math.Floor(numValue))
                        {
                            value = ((long)numValue).ToString();
                        }
                        else
                        {
                            value = numValue.ToString();
                        }
                    }

                    data[header] = value ?? "";
                    Console.WriteLine($"   {header}: {value}");
                }
            }

            Console.WriteLine($"[INFO] Successfully read {data.Count} fields from Excel");
        }

        return data;
    }

    /// <summary>
    /// Get specific field value from Excel
    /// </summary>
    public static string GetFieldValue(string fileName, string fieldName)
    {
        var data = GetEmployeeData(fileName, null);
        return data.GetValueOrDefault(fieldName, "");
    }

    /// <summary>
    /// Get specific field value from default Excel file
    /// </summary>
    public static string GetFieldValue(string fieldName)
    {
        return GetFieldValue("Test Data.xlsx", fieldName);
    }

    /// <summary>
    /// Get row count from Excel sheet (excluding header)
    /// </summary>
    public static int GetRowCount(string fileName, string? sheetName)
    {
        string filePath = Path.Combine(TestDataPath, fileName);

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var sheet = sheetName != null 
                ? package.Workbook.Worksheets[sheetName] 
                : package.Workbook.Worksheets[0];

            int rowCount = (sheet?.Dimension?.Rows ?? 1) - 1; // Exclude header
            Console.WriteLine($"[INFO] Row count in {fileName}: {rowCount}");
            return rowCount;
        }
    }
}


