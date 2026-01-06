using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SeleniumReqnrollFramework.Reports;

namespace SeleniumReqnrollFramework.Utilities;

/// <summary>
/// EmailHelper - Sends test execution reports via email using Mailtrap API
/// </summary>
public static class EmailHelper
{
    private const string MailtrapApiUrl = "https://sandbox.api.mailtrap.io/api/send";

    public static async Task SendTestReportAsync(string? pdfReportPath, string? htmlReportPath, string browserName)
    {
        var emailSettings = ConfigReader.GetEmailSettings();

        if (!emailSettings.Enabled)
        {
            Console.WriteLine("[INFO] Email sending is disabled in appsettings.json");
            return;
        }

        Console.WriteLine("\n[INFO] Starting email send process (HTTP API)...");

        try
        {
            if (string.IsNullOrEmpty(emailSettings.MailtrapApiToken) || 
                string.IsNullOrEmpty(emailSettings.MailtrapInboxId))
            {
                Console.WriteLine("[WARN] Mailtrap API not configured. Please set MailtrapApiToken and MailtrapInboxId in appsettings.json");
                return;
            }

            var results = TestResultCollector.GetAllResults();
            int passed = results.Count(r => r.Status.Equals("PASSED", StringComparison.OrdinalIgnoreCase));
            int failed = results.Count(r => r.Status.Equals("FAILED", StringComparison.OrdinalIgnoreCase));
            int total = results.Count;

            string baseUrl = ConfigReader.GetBaseUrl();
            string environment = DetectEnvironment(baseUrl);
            string status = failed > 0 ? "FAILED" : "PASSED";
            string subject = $"[{environment}] Test Automation Report - {status} | {passed}/{total} Passed";

            string htmlBody = CreateEmailBody(results, browserName, environment);

            // Build attachments
            var attachments = new List<object>();

            if (!string.IsNullOrEmpty(pdfReportPath) && File.Exists(pdfReportPath))
            {
                byte[] pdfBytes = await File.ReadAllBytesAsync(pdfReportPath);
                attachments.Add(new
                {
                    content = Convert.ToBase64String(pdfBytes),
                    filename = "TestReport.pdf",
                    type = "application/pdf"
                });
                Console.WriteLine("[INFO] Attached: TestReport.pdf");
            }

            if (!string.IsNullOrEmpty(htmlReportPath) && File.Exists(htmlReportPath))
            {
                byte[] htmlBytes = await File.ReadAllBytesAsync(htmlReportPath);
                attachments.Add(new
                {
                    content = Convert.ToBase64String(htmlBytes),
                    filename = "TestReport.html",
                    type = "text/html"
                });
                Console.WriteLine("[INFO] Attached: TestReport.html");
            }

            var payload = new
            {
                from = new { email = emailSettings.SenderEmail, name = emailSettings.SenderName },
                to = new[] { new { email = emailSettings.Recipients.Split(',')[0].Trim() } },
                subject = subject,
                html = htmlBody,
                attachments = attachments
            };

            string apiUrl = $"{MailtrapApiUrl}/{emailSettings.MailtrapInboxId}";
            Console.WriteLine("[INFO] Sending to Mailtrap API...");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Api-Token", emailSettings.MailtrapApiToken);
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("[INFO] Email sent successfully via Mailtrap API!");
                Console.WriteLine("[INFO] Check your inbox at: https://mailtrap.io/inboxes");
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Failed to send email. Status: {response.StatusCode}");
                Console.WriteLine($"[ERROR] Error: {error}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"[ERROR] Failed to send email: {e.Message}");
        }
    }

    public static void SendTestReport(string? pdfReportPath, string? htmlReportPath, string browserName)
    {
        SendTestReportAsync(pdfReportPath, htmlReportPath, browserName).GetAwaiter().GetResult();
    }

    private static string CreateEmailBody(List<TestResult> results, string browserName, string environment)
    {
        var executedBy = ConfigReader.GetExecutedByInfo();
        int total = results.Count;
        int passed = results.Count(r => r.Status.Equals("PASSED", StringComparison.OrdinalIgnoreCase));
        int failed = results.Count(r => r.Status.Equals("FAILED", StringComparison.OrdinalIgnoreCase));
        double passRate = total > 0 ? (double)passed / total * 100 : 0;
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html><head><style>");
        html.AppendLine("body { font-family: Arial, sans-serif; margin: 0; padding: 20px; background: #f5f5f5; }");
        html.AppendLine(".container { max-width: 600px; margin: 0 auto; background: white; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }");
        html.AppendLine(".header { background: linear-gradient(135deg, #2c3e50, #3498db); color: white; padding: 25px; text-align: center; }");
        html.AppendLine(".header h1 { margin: 0 0 10px 0; font-size: 24px; }");
        html.AppendLine(".executed-by { background: #f39c12; color: white; padding: 10px; text-align: center; font-weight: bold; }");
        html.AppendLine(".content { padding: 25px; }");
        html.AppendLine(".stat { display: inline-block; text-align: center; padding: 15px; border-radius: 8px; min-width: 80px; margin: 5px; }");
        html.AppendLine(".stat.total { background: #3498db; color: white; }");
        html.AppendLine(".stat.passed { background: #27ae60; color: white; }");
        html.AppendLine(".stat.failed { background: #e74c3c; color: white; }");
        html.AppendLine(".stat .number { font-size: 28px; font-weight: bold; display: block; }");
        html.AppendLine(".stat .label { font-size: 12px; text-transform: uppercase; }");
        html.AppendLine(".info-table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
        html.AppendLine(".info-table td { padding: 10px; border-bottom: 1px solid #eee; }");
        html.AppendLine(".info-table td:first-child { font-weight: bold; color: #666; width: 40%; }");
        html.AppendLine(".results-table { width: 100%; border-collapse: collapse; }");
        html.AppendLine(".results-table th { background: #2c3e50; color: white; padding: 12px; text-align: left; }");
        html.AppendLine(".results-table td { padding: 10px; border-bottom: 1px solid #eee; }");
        html.AppendLine(".status-pass { color: #27ae60; font-weight: bold; }");
        html.AppendLine(".status-fail { color: #e74c3c; font-weight: bold; }");
        html.AppendLine(".footer { background: #f8f9fa; padding: 15px; text-align: center; color: #666; font-size: 12px; }");
        html.AppendLine("</style></head><body>");

        html.AppendLine("<div class='container'>");

        // Header
        html.AppendLine("<div class='header'>");
        html.AppendLine("<h1>Test Automation Report</h1>");
        html.AppendLine("<p>Selenium Reqnroll Framework - C#</p>");
        html.AppendLine("</div>");

        // Executed By
        html.AppendLine("<div class='executed-by'>");
        html.AppendLine($"Executed By: {executedBy.Name} ({executedBy.Title})");
        html.AppendLine("</div>");

        html.AppendLine("<div class='content'>");

        // Summary Stats
        html.AppendLine("<div style='margin-bottom:25px'>");
        html.AppendLine($"<div class='stat total'><span class='number'>{total}</span><span class='label'>Total</span></div>");
        html.AppendLine($"<div class='stat passed'><span class='number'>{passed}</span><span class='label'>Passed</span></div>");
        html.AppendLine($"<div class='stat failed'><span class='number'>{failed}</span><span class='label'>Failed</span></div>");
        html.AppendLine("</div>");

        // Info Table
        html.AppendLine("<table class='info-table'>");
        html.AppendLine($"<tr><td>Environment</td><td>{environment}</td></tr>");
        html.AppendLine($"<tr><td>Browser</td><td>{browserName}</td></tr>");
        html.AppendLine($"<tr><td>Execution Date</td><td>{timestamp}</td></tr>");
        html.AppendLine($"<tr><td>Pass Rate</td><td>{passRate:F1}%</td></tr>");
        html.AppendLine("</table>");

        // Results Table
        html.AppendLine("<table class='results-table'>");
        html.AppendLine("<tr><th>#</th><th>Scenario</th><th>Status</th><th>Duration</th></tr>");

        int index = 1;
        foreach (var result in results)
        {
            string statusClass = result.Status.Equals("PASSED", StringComparison.OrdinalIgnoreCase) ? "status-pass" : "status-fail";
            html.AppendLine("<tr>");
            html.AppendLine($"<td>{index++}</td>");
            html.AppendLine($"<td>{result.ScenarioName}</td>");
            html.AppendLine($"<td class='{statusClass}'>{result.Status}</td>");
            html.AppendLine($"<td>{result.DurationFormatted}</td>");
            html.AppendLine("</tr>");
        }
        html.AppendLine("</table>");

        html.AppendLine("</div>"); // content

        // Footer
        html.AppendLine("<div class='footer'>");
        html.AppendLine("<p>This is an automated email from Test Automation Framework</p>");
        html.AppendLine("<p>Detailed reports attached</p>");
        html.AppendLine("</div>");

        html.AppendLine("</div>"); // container
        html.AppendLine("</body></html>");

        return html.ToString();
    }

    private static string DetectEnvironment(string url)
    {
        if (string.IsNullOrEmpty(url)) return "Unknown";
        if (url.ToLower().Contains("qa.")) return "QA";
        if (url.ToLower().Contains("uat.")) return "UAT";
        return "Production";
    }
}


