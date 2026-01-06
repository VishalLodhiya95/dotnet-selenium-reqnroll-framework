using OpenQA.Selenium;
using SeleniumReqnrollFramework.Helpers;
using SeleniumReqnrollFramework.Utilities;

namespace SeleniumReqnrollFramework.Pages;

/// <summary>
/// LoginPage - Page Object for the OrangeHRM login page
/// Uses https://opensource-demo.orangehrmlive.com as the test site
/// </summary>
public class LoginPage : BasePage
{
    // Locators for OrangeHRM
    private readonly By _usernameField = By.Name("username");
    private readonly By _passwordField = By.Name("password");
    private readonly By _loginButton = By.CssSelector("button[type='submit']");
    private readonly By _dashboardHeader = By.CssSelector("h6.oxd-topbar-header-breadcrumb-module");
    private readonly By _errorMessage = By.CssSelector("p.oxd-alert-content-text");
    private readonly By _userDropdown = By.CssSelector("span.oxd-userdropdown-tab");
    private readonly By _logoutLink = By.XPath("//a[text()='Logout']");

    /// <summary>
    /// Navigate to login page
    /// </summary>
    public void NavigateToLoginPage()
    {
        string baseUrl = ConfigReader.GetBaseUrl();
        NavigateTo($"{baseUrl}/web/index.php/auth/login");
        ElementHelper.WaitForElementToBeVisible(Driver, _usernameField, 15);
        Console.WriteLine("[INFO] Navigated to Login Page");
    }

    /// <summary>
    /// Enter username
    /// </summary>
    public void EnterUsername(string username)
    {
        ElementHelper.SafeSendKeys(Driver, _usernameField, username);
        Console.WriteLine($"[INFO] Entered username: {username}");
    }

    /// <summary>
    /// Enter password
    /// </summary>
    public void EnterPassword(string password)
    {
        ElementHelper.SafeSendKeys(Driver, _passwordField, password);
        Console.WriteLine("[INFO] Entered password: ****");
    }

    /// <summary>
    /// Click login button
    /// </summary>
    public void ClickLoginButton()
    {
        ElementHelper.SafeClickElement(Driver, _loginButton);
        Console.WriteLine("[INFO] Clicked Login button");
        ElementHelper.Sleep(2000);
    }

    /// <summary>
    /// Perform complete login
    /// </summary>
    public void Login(string username, string password)
    {
        EnterUsername(username);
        EnterPassword(password);
        ClickLoginButton();
    }

    /// <summary>
    /// Check if login was successful (Dashboard is visible)
    /// </summary>
    public bool IsLoginSuccessful()
    {
        return ElementHelper.IsElementDisplayed(Driver, _dashboardHeader, 10);
    }

    /// <summary>
    /// Check if error message is displayed
    /// </summary>
    public bool IsErrorMessageDisplayed()
    {
        return ElementHelper.IsElementDisplayed(Driver, _errorMessage, 5);
    }

    /// <summary>
    /// Get error message text
    /// </summary>
    public string GetErrorMessageText()
    {
        return ElementHelper.GetTextFromElement(Driver, _errorMessage, 5);
    }

    /// <summary>
    /// Get success message text (Dashboard header text)
    /// </summary>
    public string GetSuccessMessageText()
    {
        return ElementHelper.GetTextFromElement(Driver, _dashboardHeader, 5);
    }

    /// <summary>
    /// Click logout
    /// </summary>
    public void ClickLogout()
    {
        ElementHelper.SafeClickElement(Driver, _userDropdown);
        ElementHelper.Sleep(500);
        ElementHelper.SafeClickElement(Driver, _logoutLink);
        Console.WriteLine("[INFO] Clicked Logout");
    }

    /// <summary>
    /// Verify user is logged in (user dropdown visible)
    /// </summary>
    public bool IsLoggedIn()
    {
        return ElementHelper.IsElementDisplayed(Driver, _userDropdown, 5);
    }
}


