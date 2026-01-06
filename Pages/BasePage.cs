using OpenQA.Selenium;
using SeleniumReqnrollFramework.Drivers;

namespace SeleniumReqnrollFramework.Pages;

/// <summary>
/// BasePage - Base class for all page objects
/// Contains common elements and methods shared across pages
/// </summary>
public class BasePage
{
    protected IWebDriver Driver;

    public BasePage()
    {
        Driver = DriverFactory.GetDriver();
    }

    /// <summary>
    /// Navigate to a URL
    /// </summary>
    public void NavigateTo(string url)
    {
        Driver.Navigate().GoToUrl(url);
        Console.WriteLine($"[INFO] Navigated to: {url}");
    }

    /// <summary>
    /// Get current page title
    /// </summary>
    public string GetPageTitle()
    {
        return Driver.Title;
    }

    /// <summary>
    /// Get current URL
    /// </summary>
    public string GetCurrentUrl()
    {
        return Driver.Url;
    }

    /// <summary>
    /// Refresh the page
    /// </summary>
    public void RefreshPage()
    {
        Driver.Navigate().Refresh();
        Console.WriteLine("[INFO] Page refreshed");
    }

    /// <summary>
    /// Navigate back
    /// </summary>
    public void NavigateBack()
    {
        Driver.Navigate().Back();
        Console.WriteLine("[INFO] Navigated back");
    }
}


