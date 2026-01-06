using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using SeleniumReqnrollFramework.Utilities;

namespace SeleniumReqnrollFramework.Drivers;

/// <summary>
/// DriverFactory - Manages WebDriver instances using ThreadLocal for parallel execution
/// </summary>
public static class DriverFactory
{
    private static readonly ThreadLocal<IWebDriver?> _driver = new();
    private static string _browserName = "Unknown";

    /// <summary>
    /// Initialize and return WebDriver based on configuration
    /// </summary>
    public static IWebDriver GetDriver()
    {
        if (_driver.Value == null)
        {
            InitializeDriver();
        }
        return _driver.Value!;
    }

    /// <summary>
    /// Initialize the WebDriver based on browser configuration
    /// </summary>
    private static void InitializeDriver()
    {
        var config = ConfigReader.GetTestSettings();
        string browser = config.Browser.ToLower();
        bool headless = config.HeadlessMode;

        Console.WriteLine($"[INFO] Initializing browser: {browser.ToUpper()}");

        switch (browser)
        {
            case "chrome":
            case "headless":
                new DriverManager().SetUpDriver(new ChromeConfig());
                var chromeOptions = new ChromeOptions();
                if (headless || browser == "headless")
                {
                    chromeOptions.AddArguments("--headless=new");
                    chromeOptions.AddArguments("--window-size=1920,1080");
                }
                chromeOptions.AddArguments("--disable-gpu");
                chromeOptions.AddArguments("--no-sandbox");
                chromeOptions.AddArguments("--disable-dev-shm-usage");
                _driver.Value = new ChromeDriver(chromeOptions);
                _browserName = "Chrome";
                break;

            case "firefox":
                new DriverManager().SetUpDriver(new FirefoxConfig());
                var firefoxOptions = new FirefoxOptions();
                if (headless)
                {
                    firefoxOptions.AddArguments("--headless");
                }
                _driver.Value = new FirefoxDriver(firefoxOptions);
                _browserName = "Firefox";
                break;

            case "edge":
                new DriverManager().SetUpDriver(new EdgeConfig());
                var edgeOptions = new EdgeOptions();
                if (headless)
                {
                    edgeOptions.AddArguments("--headless=new");
                }
                _driver.Value = new EdgeDriver(edgeOptions);
                _browserName = "Edge";
                break;

            default:
                throw new Exception($"Unsupported browser: {browser}");
        }

        // Configure timeouts
        _driver.Value!.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(config.PageLoadTimeout);
        _driver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(config.ImplicitWait);
        _driver.Value.Manage().Window.Maximize();

        Console.WriteLine($"[INFO] Browser launched: {_browserName} | Thread ID: {Environment.CurrentManagedThreadId}");
    }

    /// <summary>
    /// Get the browser name
    /// </summary>
    public static string GetBrowserName() => _browserName;

    /// <summary>
    /// Quit the WebDriver and clean up
    /// </summary>
    public static void QuitDriver()
    {
        if (_driver.Value != null)
        {
            try
            {
                _driver.Value.Quit();
                Console.WriteLine("[INFO] Browser closed successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Error closing browser: {e.Message}");
            }
            finally
            {
                _driver.Value = null;
            }
        }
    }
}


