using OpenQA.Selenium;
using Reqnroll;
using SeleniumReqnrollFramework.Drivers;
using SeleniumReqnrollFramework.Reports;

namespace SeleniumReqnrollFramework.Hooks;

/// <summary>
/// ApplicationHooks - Reqnroll hooks for test lifecycle management
/// </summary>
[Binding]
public class ApplicationHooks
{
    private readonly ScenarioContext _scenarioContext;
    private DateTime _scenarioStartTime;

    public ApplicationHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("[INFO] TEST RUN STARTED");
        Console.WriteLine(new string('=', 60));
        TestResultCollector.SetStartTime();
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _scenarioStartTime = DateTime.Now;
        var tags = string.Join(", ", _scenarioContext.ScenarioInfo.Tags);
        
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine($"[INFO] Starting Scenario: {_scenarioContext.ScenarioInfo.Title}");
        Console.WriteLine($"   Tags: [{tags}]");
        Console.WriteLine(new string('=', 60));

        // Initialize the driver
        DriverFactory.GetDriver();
    }

    [AfterScenario]
    public void AfterScenario()
    {
        DateTime scenarioEndTime = DateTime.Now;
        TimeSpan duration = scenarioEndTime - _scenarioStartTime;
        string status = _scenarioContext.TestError == null ? "PASSED" : "FAILED";
        string? screenshotBase64 = null;

        try
        {
            if (_scenarioContext.TestError != null)
            {
                Console.WriteLine($"[ERROR] Scenario FAILED: {_scenarioContext.ScenarioInfo.Title}");
                screenshotBase64 = CaptureScreenshotWithRetry();
            }
            else
            {
                Console.WriteLine($"[INFO] Scenario PASSED: {_scenarioContext.ScenarioInfo.Title}");
            }
        }
        finally
        {
            // Get feature name from scenario info
            string featureName = _scenarioContext.ScenarioInfo.Title;
            
            // Collect test result
            TestResultCollector.AddResult(
                _scenarioContext.ScenarioInfo.Title,
                status,
                duration,
                _scenarioStartTime,
                scenarioEndTime,
                _scenarioContext.TestError?.Message ?? "",
                featureName,
                screenshotBase64
            );

            // Always quit the driver
            DriverFactory.QuitDriver();
        }

        Console.WriteLine(new string('=', 60) + "\n");
    }

    private string? CaptureScreenshotWithRetry()
    {
        string? screenshotBase64 = null;
        int maxRetries = 3;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                Thread.Sleep(500);

                var driver = DriverFactory.GetDriver();
                if (driver == null)
                {
                    Console.WriteLine("[WARN] Driver is null, cannot capture screenshot");
                    break;
                }

                byte[] screenshot = ((ITakesScreenshot)driver).GetScreenshot().AsByteArray;

                if (screenshot != null && screenshot.Length > 0)
                {
                    screenshotBase64 = Convert.ToBase64String(screenshot);
                    Console.WriteLine($"[INFO] Screenshot captured successfully (attempt {attempt}, size: {screenshot.Length} bytes)");
                    break;
                }
                else
                {
                    Console.WriteLine($"[WARN] Screenshot was empty (attempt {attempt})");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[WARN] Screenshot capture attempt {attempt} failed: {e.Message}");
                if (attempt < maxRetries)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        if (screenshotBase64 == null)
        {
            Console.WriteLine($"[WARN] Failed to capture screenshot after {maxRetries} attempts");
        }

        return screenshotBase64;
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        TestResultCollector.SetEndTime();

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("[INFO] TEST RUN COMPLETED");
        Console.WriteLine(new string('=', 60));

        // Print summary
        Console.WriteLine($"[INFO] Total: {TestResultCollector.GetTotalCount()}");
        Console.WriteLine($"[INFO] Passed: {TestResultCollector.GetPassedCount()}");
        Console.WriteLine($"[INFO] Failed: {TestResultCollector.GetFailedCount()}");

        // Generate PDF report
        string? pdfPath = PdfReportGenerator.GenerateReport(
            TestResultCollector.GetAllResults(),
            DriverFactory.GetBrowserName()
        );

        if (pdfPath != null)
        {
            Console.WriteLine($"[INFO] PDF Report: {pdfPath}");
        }

        // Generate HTML report
        string? htmlPath = HtmlReportGenerator.GenerateReport(
            TestResultCollector.GetAllResults(),
            DriverFactory.GetBrowserName()
        );

        if (htmlPath != null)
        {
            Console.WriteLine($"[INFO] HTML Report: {htmlPath}");
        }

        // Send email report
        Utilities.EmailHelper.SendTestReport(pdfPath, htmlPath, DriverFactory.GetBrowserName());

        Console.WriteLine(new string('=', 60) + "\n");
    }
}

