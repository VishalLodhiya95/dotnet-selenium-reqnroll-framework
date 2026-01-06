namespace SeleniumReqnrollFramework.Reports;

/// <summary>
/// TestResult - Model class to store individual test result information
/// </summary>
public class TestResult
{
    public string ScenarioName { get; set; } = "";
    public string Status { get; set; } = "";
    public TimeSpan Duration { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string ErrorMessage { get; set; } = "";
    public string FeatureName { get; set; } = "";
    public string? ScreenshotBase64 { get; set; }

    public string DurationFormatted
    {
        get
        {
            if (Duration.TotalHours >= 1)
                return $"{Duration.Hours}h {Duration.Minutes}m {Duration.Seconds}s";
            if (Duration.TotalMinutes >= 1)
                return $"{Duration.Minutes}m {Duration.Seconds}s";
            if (Duration.TotalSeconds >= 1)
                return $"{Duration.TotalSeconds:F1}s";
            return $"{Duration.TotalMilliseconds:F0}ms";
        }
    }

    public bool HasScreenshot => !string.IsNullOrEmpty(ScreenshotBase64);
}


