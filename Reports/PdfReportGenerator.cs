using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SeleniumReqnrollFramework.Utilities;

namespace SeleniumReqnrollFramework.Reports;

/// <summary>
/// PdfReportGenerator - Generates professional PDF test reports using PdfSharpCore
/// </summary>
public static class PdfReportGenerator
{
    private static readonly string ReportPath = "TestResults/Reports/";

    public static string? GenerateReport(List<TestResult> results, string browserName)
    {
        try
        {
            // Create reports directory if it doesn't exist
            if (!Directory.Exists(ReportPath))
            {
                Directory.CreateDirectory(ReportPath);
            }

            // Generate filename with timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"TestReport_{timestamp}.pdf";
            string filePath = Path.Combine(ReportPath, fileName);

            // Calculate stats
            int total = results.Count;
            int passed = results.Count(r => r.Status.Equals("PASSED", StringComparison.OrdinalIgnoreCase));
            int failed = results.Count(r => r.Status.Equals("FAILED", StringComparison.OrdinalIgnoreCase));
            double passRate = total > 0 ? (double)passed / total * 100 : 0;

            var executedBy = ConfigReader.GetExecutedByInfo();
            string baseUrl = ConfigReader.GetBaseUrl();
            string environment = DetectEnvironment(baseUrl);

            // Create PDF document
            var document = new PdfDocument();
            document.Info.Title = "Test Automation Report";
            document.Info.Author = executedBy.Name;

            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Fonts
            var titleFont = new XFont("Arial", 24, XFontStyle.Bold);
            var subtitleFont = new XFont("Arial", 12, XFontStyle.Regular);
            var headerFont = new XFont("Arial", 11, XFontStyle.Bold);
            var regularFont = new XFont("Arial", 10, XFontStyle.Regular);
            var smallFont = new XFont("Arial", 9, XFontStyle.Regular);

            // Colors
            var primaryColor = XColor.FromArgb(44, 62, 80);
            var successColor = XColor.FromArgb(39, 174, 96);
            var failColor = XColor.FromArgb(231, 76, 60);
            var blueColor = XColor.FromArgb(52, 152, 219);
            var purpleColor = XColor.FromArgb(155, 89, 182);
            var orangeColor = XColor.FromArgb(243, 156, 18);

            double yPos = 40;
            double pageWidth = page.Width;
            double margin = 40;
            double contentWidth = pageWidth - (margin * 2);

            // Title
            gfx.DrawString("Test Automation Report", titleFont, new XSolidBrush(primaryColor),
                new XRect(0, yPos, pageWidth, 30), XStringFormats.TopCenter);
            yPos += 35;

            // Subtitle
            gfx.DrawString("Selenium Reqnroll Framework - C# .NET", subtitleFont, XBrushes.Gray,
                new XRect(0, yPos, pageWidth, 20), XStringFormats.TopCenter);
            yPos += 25;

            // Executed By
            gfx.DrawString($"Executed By: {executedBy.Name} ({executedBy.Title})", headerFont,
                new XSolidBrush(orangeColor), new XRect(0, yPos, pageWidth, 20), XStringFormats.TopCenter);
            yPos += 40;

            // Summary Boxes
            double boxWidth = 120;
            double boxHeight = 60;
            double boxSpacing = 15;
            double totalBoxWidth = (boxWidth * 4) + (boxSpacing * 3);
            double startX = (pageWidth - totalBoxWidth) / 2;

            // Total Box
            DrawSummaryBox(gfx, startX, yPos, boxWidth, boxHeight, total.ToString(), "TOTAL", blueColor, headerFont, regularFont);
            // Passed Box
            DrawSummaryBox(gfx, startX + boxWidth + boxSpacing, yPos, boxWidth, boxHeight, passed.ToString(), "PASSED", successColor, headerFont, regularFont);
            // Failed Box
            DrawSummaryBox(gfx, startX + (boxWidth + boxSpacing) * 2, yPos, boxWidth, boxHeight, failed.ToString(), "FAILED", failColor, headerFont, regularFont);
            // Pass Rate Box
            DrawSummaryBox(gfx, startX + (boxWidth + boxSpacing) * 3, yPos, boxWidth, boxHeight, $"{passRate:F1}%", "PASS RATE", purpleColor, headerFont, regularFont);

            yPos += boxHeight + 30;

            // Info Section
            string[] infoLabels = { "Browser:", "Environment:", "Base URL:", "Execution Date:", "Duration:" };
            string[] infoValues = { browserName, environment, baseUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), GetTotalExecutionDuration() };

            for (int i = 0; i < infoLabels.Length; i++)
            {
                gfx.DrawString(infoLabels[i], headerFont, XBrushes.DarkGray, margin, yPos);
                gfx.DrawString(infoValues[i], regularFont, XBrushes.Black, margin + 120, yPos);
                yPos += 18;
            }

            yPos += 20;

            // Results Header
            gfx.DrawRectangle(new XSolidBrush(primaryColor), margin, yPos, contentWidth, 25);
            gfx.DrawString("Test Results Details", headerFont, XBrushes.White,
                new XRect(margin, yPos, contentWidth, 25), XStringFormats.Center);
            yPos += 35;

            // Table Header
            double[] colWidths = { 30, 300, 80, 80 };
            string[] headers = { "#", "Scenario", "Status", "Duration" };
            double xPos = margin;

            gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(248, 249, 250)), margin, yPos - 5, contentWidth, 20);
            for (int i = 0; i < headers.Length; i++)
            {
                gfx.DrawString(headers[i], headerFont, XBrushes.DarkGray, xPos + 5, yPos + 10);
                xPos += colWidths[i];
            }
            yPos += 25;

            // Table Rows
            int index = 1;
            foreach (var result in results)
            {
                if (yPos > page.Height - 60)
                {
                    // Add new page
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPos = 40;
                }

                bool isFailed = result.Status.Equals("FAILED", StringComparison.OrdinalIgnoreCase);
                var statusBrush = isFailed ? new XSolidBrush(failColor) : new XSolidBrush(successColor);

                xPos = margin;
                gfx.DrawString(index.ToString(), regularFont, XBrushes.Black, xPos + 5, yPos);
                xPos += colWidths[0];

                // Truncate long scenario names
                string scenarioName = result.ScenarioName;
                if (scenarioName.Length > 50)
                    scenarioName = scenarioName.Substring(0, 47) + "...";
                gfx.DrawString(scenarioName, regularFont, XBrushes.Black, xPos + 5, yPos);
                xPos += colWidths[1];

                gfx.DrawString(result.Status, headerFont, statusBrush, xPos + 5, yPos);
                xPos += colWidths[2];

                gfx.DrawString(result.DurationFormatted, regularFont, XBrushes.Black, xPos + 5, yPos);

                // Draw line
                yPos += 5;
                gfx.DrawLine(XPens.LightGray, margin, yPos + 10, margin + contentWidth, yPos + 10);
                yPos += 20;
                index++;
            }

            // Footer
            yPos = page.Height - 40;
            gfx.DrawString($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss} | Selenium Reqnroll Framework",
                smallFont, XBrushes.Gray, new XRect(0, yPos, pageWidth, 20), XStringFormats.TopCenter);

            // Save the document
            document.Save(filePath);

            Console.WriteLine($"[INFO] PDF Report generated: {filePath}");
            return filePath;
        }
        catch (Exception e)
        {
            Console.WriteLine($"[ERROR] Failed to generate PDF report: {e.Message}");
            return null;
        }
    }

    private static void DrawSummaryBox(XGraphics gfx, double x, double y, double width, double height,
        string value, string label, XColor color, XFont valueFont, XFont labelFont)
    {
        // Draw border
        gfx.DrawRectangle(new XPen(color, 2), x, y, width, height);

        // Draw value
        var bigFont = new XFont("Arial", 22, XFontStyle.Bold);
        gfx.DrawString(value, bigFont, new XSolidBrush(color),
            new XRect(x, y + 10, width, 30), XStringFormats.TopCenter);

        // Draw label
        gfx.DrawString(label, labelFont, XBrushes.Gray,
            new XRect(x, y + 40, width, 15), XStringFormats.TopCenter);
    }

    private static string DetectEnvironment(string url)
    {
        if (string.IsNullOrEmpty(url)) return "Unknown";
        if (url.ToLower().Contains("qa.")) return "QA";
        if (url.ToLower().Contains("uat.")) return "UAT";
        return "Production";
    }

    private static string GetTotalExecutionDuration()
    {
        var startTime = TestResultCollector.GetTestRunStartTime();
        var endTime = TestResultCollector.GetTestRunEndTime();

        if (startTime == default || endTime == default) return "N/A";

        var duration = endTime - startTime;
        if (duration.TotalHours >= 1)
            return $"{duration.Hours} hr {duration.Minutes} min {duration.Seconds} sec";
        if (duration.TotalMinutes >= 1)
            return $"{duration.Minutes} min {duration.Seconds} sec";
        return $"{duration.Seconds} sec";
    }
}
