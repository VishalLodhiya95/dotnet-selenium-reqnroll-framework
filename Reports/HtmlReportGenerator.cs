using SeleniumReqnrollFramework.Utilities;
using System.Text;

namespace SeleniumReqnrollFramework.Reports;

/// <summary>
/// HtmlReportGenerator - Generates professional HTML test reports with screenshots
/// </summary>
public static class HtmlReportGenerator
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
            string fileName = $"TestReport_{timestamp}.html";
            string filePath = Path.Combine(ReportPath, fileName);

            // Calculate stats
            int total = results.Count;
            int passed = results.Count(r => r.Status.Equals("PASSED", StringComparison.OrdinalIgnoreCase));
            int failed = results.Count(r => r.Status.Equals("FAILED", StringComparison.OrdinalIgnoreCase));
            double passRate = total > 0 ? (double)passed / total * 100 : 0;

            // Build HTML content
            var html = new StringBuilder();
            html.Append(GetHtmlHeader());
            html.Append(GetHtmlBody(results, browserName, total, passed, failed, passRate));
            html.Append(GetHtmlFooter());

            // Write to file
            File.WriteAllText(filePath, html.ToString());

            Console.WriteLine($"[INFO] HTML Report generated: {filePath}");
            return filePath;
        }
        catch (Exception e)
        {
            Console.WriteLine($"[ERROR] Failed to generate HTML report: {e.Message}");
            return null;
        }
    }

    private static string GetHtmlHeader()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head>");
        sb.AppendLine("    <meta charset=\"UTF-8\">");
        sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        sb.AppendLine("    <title>Test Automation Report</title>");
        sb.AppendLine("    <style>");
        
        // CSS Styles
        sb.AppendLine("        * { margin: 0; padding: 0; box-sizing: border-box; }");
        sb.AppendLine("        body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; background: #f0f2f5; padding: 20px; }");
        sb.AppendLine("        .container { max-width: 1400px; margin: 0 auto; }");
        sb.AppendLine("        .header-card { background: linear-gradient(135deg, #2c3e50 0%, #3498db 100%); border-radius: 12px; padding: 30px; margin-bottom: 20px; color: white; }");
        sb.AppendLine("        .header-card h1 { font-size: 2em; margin-bottom: 8px; }");
        sb.AppendLine("        .executed-by-badge { display: inline-block; background: rgba(255,255,255,0.2); padding: 8px 16px; border-radius: 20px; margin-top: 15px; }");
        sb.AppendLine("        .executed-by-badge strong { color: #f1c40f; }");
        sb.AppendLine("        .cards-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); gap: 20px; margin-bottom: 20px; }");
        sb.AppendLine("        .card { background: white; border-radius: 12px; padding: 24px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }");
        sb.AppendLine("        .card-title { font-size: 0.85em; color: #7f8c8d; text-transform: uppercase; margin-bottom: 8px; }");
        sb.AppendLine("        .card-value { font-size: 1.3em; font-weight: 600; color: #2c3e50; word-break: break-all; }");
        sb.AppendLine("        .summary-grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 20px; margin-bottom: 20px; }");
        sb.AppendLine("        @media (max-width: 768px) { .summary-grid { grid-template-columns: repeat(2, 1fr); } }");
        sb.AppendLine("        .summary-card { background: white; border-radius: 12px; padding: 24px; text-align: center; border-top: 4px solid; }");
        sb.AppendLine("        .summary-card.total { border-top-color: #3498db; }");
        sb.AppendLine("        .summary-card.passed { border-top-color: #27ae60; }");
        sb.AppendLine("        .summary-card.failed { border-top-color: #e74c3c; }");
        sb.AppendLine("        .summary-card.rate { border-top-color: #9b59b6; }");
        sb.AppendLine("        .summary-number { font-size: 2.5em; font-weight: 700; display: block; margin-bottom: 5px; }");
        sb.AppendLine("        .summary-card.total .summary-number { color: #3498db; }");
        sb.AppendLine("        .summary-card.passed .summary-number { color: #27ae60; }");
        sb.AppendLine("        .summary-card.failed .summary-number { color: #e74c3c; }");
        sb.AppendLine("        .summary-card.rate .summary-number { color: #9b59b6; }");
        sb.AppendLine("        .summary-label { font-size: 0.9em; color: #7f8c8d; text-transform: uppercase; }");
        sb.AppendLine("        .results-card { background: white; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); overflow: hidden; }");
        sb.AppendLine("        .results-header { background: #2c3e50; color: white; padding: 20px 24px; }");
        sb.AppendLine("        .results-table { width: 100%; border-collapse: collapse; }");
        sb.AppendLine("        .results-table th { background: #f8f9fa; padding: 14px 20px; text-align: left; font-weight: 600; border-bottom: 2px solid #e9ecef; }");
        sb.AppendLine("        .results-table td { padding: 16px 20px; border-bottom: 1px solid #f0f0f0; vertical-align: top; }");
        sb.AppendLine("        .results-table tr:hover { background: #f8f9fa; }");
        sb.AppendLine("        .status-badge { display: inline-flex; align-items: center; padding: 6px 14px; border-radius: 6px; font-weight: 600; }");
        sb.AppendLine("        .status-badge.passed { background: #d5f5e3; color: #1e8449; }");
        sb.AppendLine("        .status-badge.failed { background: #fadbd8; color: #c0392b; }");
        sb.AppendLine("        .screenshot-toggle { background: #e74c3c; color: white; border: none; padding: 8px 16px; border-radius: 6px; cursor: pointer; }");
        sb.AppendLine("        .screenshot-toggle:hover { background: #c0392b; }");
        sb.AppendLine("        .screenshot-container { margin-top: 12px; display: none; }");
        sb.AppendLine("        .screenshot-container.show { display: block; }");
        sb.AppendLine("        .screenshot-img { max-width: 100%; border-radius: 8px; border: 2px solid #e9ecef; }");
        sb.AppendLine("        .footer { text-align: center; padding: 20px; color: #7f8c8d; margin-top: 20px; }");
        
        sb.AppendLine("    </style>");
        sb.AppendLine("</head>");
        return sb.ToString();
    }

    private static string GetHtmlBody(List<TestResult> results, string browserName, int total, int passed, int failed, double passRate)
    {
        var executedBy = ConfigReader.GetExecutedByInfo();
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string baseUrl = ConfigReader.GetBaseUrl();
        string environment = DetectEnvironment(baseUrl);
        string totalDuration = GetTotalExecutionDuration();

        var body = new StringBuilder();
        body.AppendLine("<body>");
        body.AppendLine("<div class=\"container\">");

        // Header
        body.AppendLine("<div class=\"header-card\">");
        body.AppendLine("    <h1>Test Automation Report</h1>");
        body.AppendLine("    <p>Selenium Reqnroll Framework - C# .NET</p>");
        body.AppendLine($"    <div class=\"executed-by-badge\">Executed By: <strong>{executedBy.Name}</strong> ({executedBy.Title})</div>");
        body.AppendLine("</div>");

        // Info Cards
        body.AppendLine("<div class=\"cards-grid\">");
        body.AppendLine(CreateInfoCard("Browser", browserName));
        body.AppendLine(CreateInfoCard("Execution Date", timestamp));
        body.AppendLine(CreateInfoCard("Environment", environment));
        body.AppendLine(CreateInfoCard("Base URL", baseUrl));
        body.AppendLine(CreateInfoCard("Total Duration", totalDuration));
        body.AppendLine("</div>");

        // Summary Grid
        body.AppendLine("<div class=\"summary-grid\">");
        body.AppendLine(CreateSummaryCard("total", total.ToString(), "Total Tests"));
        body.AppendLine(CreateSummaryCard("passed", passed.ToString(), "Passed"));
        body.AppendLine(CreateSummaryCard("failed", failed.ToString(), "Failed"));
        body.AppendLine(CreateSummaryCard("rate", $"{passRate:F1}%", "Pass Rate"));
        body.AppendLine("</div>");

        // Results Table
        body.AppendLine("<div class=\"results-card\">");
        body.AppendLine("<div class=\"results-header\"><h2>Test Results Details</h2></div>");
        body.AppendLine("<table class=\"results-table\">");
        body.AppendLine("<thead><tr><th>#</th><th>Scenario</th><th>Status</th><th>Duration</th><th>Feature</th></tr></thead>");
        body.AppendLine("<tbody>");

        int index = 1;
        foreach (var result in results)
        {
            bool isFailed = result.Status.Equals("FAILED", StringComparison.OrdinalIgnoreCase);
            string statusClass = isFailed ? "failed" : "passed";
            string statusIcon = isFailed ? "X" : "OK";
            string screenshotId = $"screenshot-{index}";

            body.AppendLine("<tr>");
            body.AppendLine($"    <td>{index}</td>");
            body.AppendLine("    <td>");
            body.AppendLine($"        <div>{EscapeHtml(result.ScenarioName)}</div>");

            if (isFailed && result.HasScreenshot)
            {
                body.AppendLine($"        <div style=\"margin-top:10px\">");
                body.AppendLine($"            <button class=\"screenshot-toggle\" onclick=\"toggleScreenshot('{screenshotId}')\">View Screenshot</button>");
                body.AppendLine($"            <div id=\"{screenshotId}\" class=\"screenshot-container\">");
                body.AppendLine($"                <img class=\"screenshot-img\" src=\"data:image/png;base64,{result.ScreenshotBase64}\" alt=\"Failed Screenshot\">");
                body.AppendLine($"            </div>");
                body.AppendLine($"        </div>");
            }

            body.AppendLine("    </td>");
            body.AppendLine($"    <td><span class=\"status-badge {statusClass}\">{statusIcon} {result.Status}</span></td>");
            body.AppendLine($"    <td>{result.DurationFormatted}</td>");
            body.AppendLine($"    <td>{result.FeatureName}</td>");
            body.AppendLine("</tr>");
            index++;
        }

        body.AppendLine("</tbody></table></div>");

        // Footer
        body.AppendLine("<div class=\"footer\">");
        body.AppendLine($"    <p>Generated on {timestamp} | Selenium Reqnroll Framework</p>");
        body.AppendLine($"    <p>Executed By: <strong>{executedBy.Name}</strong> ({executedBy.Title})</p>");
        body.AppendLine("</div>");

        body.AppendLine("</div>");

        // JavaScript
        body.AppendLine("<script>");
        body.AppendLine("function toggleScreenshot(id) {");
        body.AppendLine("    var container = document.getElementById(id);");
        body.AppendLine("    container.classList.toggle('show');");
        body.AppendLine("}");
        body.AppendLine("</script>");

        body.AppendLine("</body>");
        return body.ToString();
    }

    private static string CreateInfoCard(string label, string value)
    {
        return $@"<div class=""card"">
    <div class=""card-title"">{label}</div>
    <div class=""card-value"">{value ?? "-"}</div>
</div>";
    }

    private static string CreateSummaryCard(string type, string number, string label)
    {
        return $@"<div class=""summary-card {type}"">
    <span class=""summary-number"">{number}</span>
    <span class=""summary-label"">{label}</span>
</div>";
    }

    private static string EscapeHtml(string? text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text.Replace("&", "&amp;")
                   .Replace("<", "&lt;")
                   .Replace(">", "&gt;")
                   .Replace("\"", "&quot;");
    }

    private static string GetHtmlFooter() => "</html>";

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


