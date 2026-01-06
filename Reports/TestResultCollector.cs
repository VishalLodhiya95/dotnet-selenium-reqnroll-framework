namespace SeleniumReqnrollFramework.Reports;

/// <summary>
/// TestResultCollector - Collects and manages test results across the test run
/// </summary>
public static class TestResultCollector
{
    private static readonly List<TestResult> _results = new();
    private static DateTime _testRunStartTime;
    private static DateTime _testRunEndTime;

    public static void SetStartTime()
    {
        _testRunStartTime = DateTime.Now;
        Console.WriteLine($"[INFO] Test run started at: {_testRunStartTime:yyyy-MM-dd HH:mm:ss}");
    }

    public static void SetEndTime()
    {
        _testRunEndTime = DateTime.Now;
        Console.WriteLine($"[INFO] Test run ended at: {_testRunEndTime:yyyy-MM-dd HH:mm:ss}");
    }

    public static DateTime GetTestRunStartTime() => _testRunStartTime;
    public static DateTime GetTestRunEndTime() => _testRunEndTime;

    public static void AddResult(
        string scenarioName,
        string status,
        TimeSpan duration,
        DateTime startTime,
        DateTime endTime,
        string errorMessage,
        string featureName,
        string? screenshotBase64)
    {
        var result = new TestResult
        {
            ScenarioName = scenarioName,
            Status = status,
            Duration = duration,
            StartTime = startTime,
            EndTime = endTime,
            ErrorMessage = errorMessage,
            FeatureName = featureName,
            ScreenshotBase64 = screenshotBase64
        };

        _results.Add(result);
        Console.WriteLine($"[INFO] Result added: {scenarioName} - {status}");
    }

    public static List<TestResult> GetAllResults() => _results.ToList();
    public static int GetTotalCount() => _results.Count;
    public static int GetPassedCount() => _results.Count(r => r.Status.Equals("PASSED", StringComparison.OrdinalIgnoreCase));
    public static int GetFailedCount() => _results.Count(r => r.Status.Equals("FAILED", StringComparison.OrdinalIgnoreCase));

    public static void Clear()
    {
        _results.Clear();
    }
}


