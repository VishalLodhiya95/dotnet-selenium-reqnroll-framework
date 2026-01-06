using Microsoft.Extensions.Configuration;

namespace SeleniumReqnrollFramework.Utilities;

/// <summary>
/// ConfigReader - Reads configuration from appsettings.json
/// </summary>
public static class ConfigReader
{
    private static IConfiguration? _configuration;
    private static TestSettings? _testSettings;
    private static EmailSettings? _emailSettings;
    private static ExecutedByInfo? _executedByInfo;

    static ConfigReader()
    {
        LoadConfiguration();
    }

    private static void LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        _configuration = builder.Build();
        
        _testSettings = new TestSettings();
        _configuration.GetSection("TestSettings").Bind(_testSettings);

        _emailSettings = new EmailSettings();
        _configuration.GetSection("EmailSettings").Bind(_emailSettings);

        _executedByInfo = new ExecutedByInfo();
        _configuration.GetSection("ExecutedBy").Bind(_executedByInfo);

        Console.WriteLine("[INFO] Configuration loaded from: appsettings.json");
    }

    /// <summary>
    /// Get test settings
    /// </summary>
    public static TestSettings GetTestSettings() => _testSettings ?? new TestSettings();

    /// <summary>
    /// Get email settings
    /// </summary>
    public static EmailSettings GetEmailSettings() => _emailSettings ?? new EmailSettings();

    /// <summary>
    /// Get executed by info
    /// </summary>
    public static ExecutedByInfo GetExecutedByInfo() => _executedByInfo ?? new ExecutedByInfo();

    /// <summary>
    /// Get base URL
    /// </summary>
    public static string GetBaseUrl() => _testSettings?.BaseUrl ?? "https://opensource-demo.orangehrmlive.com";

    /// <summary>
    /// Get default timeout
    /// </summary>
    public static int GetDefaultTimeout() => _testSettings?.DefaultTimeout ?? 10;

    /// <summary>
    /// Get browser name
    /// </summary>
    public static string GetBrowser() => _testSettings?.Browser ?? "chrome";
}

/// <summary>
/// Test settings model
/// </summary>
public class TestSettings
{
    public string Browser { get; set; } = "chrome";
    public string Environment { get; set; } = "Test";
    public string BaseUrl { get; set; } = "https://opensource-demo.orangehrmlive.com";
    public int DefaultTimeout { get; set; } = 10;
    public int PageLoadTimeout { get; set; } = 30;
    public int ImplicitWait { get; set; } = 5;
    public bool HeadlessMode { get; set; } = false;
    public bool ScreenshotOnFailure { get; set; } = true;
    public string ScreenshotPath { get; set; } = "TestResults/Screenshots";
    public string ReportPath { get; set; } = "TestResults/Reports";
    public string TestDataPath { get; set; } = "TestData";
}

/// <summary>
/// Email settings model
/// </summary>
public class EmailSettings
{
    public bool Enabled { get; set; } = false;
    public string Method { get; set; } = "api";
    public string MailtrapApiToken { get; set; } = "";
    public string MailtrapInboxId { get; set; } = "";
    public string SenderEmail { get; set; } = "test@automation.com";
    public string SenderName { get; set; } = "Test Automation";
    public string Recipients { get; set; } = "";
}

/// <summary>
/// Executed by info model
/// </summary>
public class ExecutedByInfo
{
    public string Name { get; set; } = "Vishal Lodhiya";
    public string Title { get; set; } = "QA ISTQB Certified";
}


