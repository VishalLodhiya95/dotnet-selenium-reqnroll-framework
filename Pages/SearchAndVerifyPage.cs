using OpenQA.Selenium;
using SeleniumReqnrollFramework.Helpers;
using SeleniumReqnrollFramework.Utilities;

namespace SeleniumReqnrollFramework.Pages;

/// <summary>
/// SearchAndVerifyPage - Page Object for Admin User Management Search functionality
/// URL: /web/index.php/admin/viewSystemUsers
/// </summary>
public class SearchAndVerifyPage : BasePage
{
    // Captured Data
    private string? _capturedUsername;
    private string? _capturedEmployeeName;
    private int _capturedRecordsCount;

    // Search Form Locators
    private readonly By _usernameInput = By.XPath("//label[normalize-space()='Username']/ancestor::div[contains(@class,'oxd-input-group')]//input");
    private readonly By _userRoleDropdown = By.XPath("//label[normalize-space()='User Role']/ancestor::div[contains(@class,'oxd-input-group')]//div[contains(@class,'oxd-select-text-input')]");
    private readonly By _employeeNameInput = By.XPath("//label[normalize-space()='Employee Name']/ancestor::div[contains(@class,'oxd-input-group')]//input");
    private readonly By _statusDropdown = By.XPath("//label[text()='Status']/ancestor::div[contains(@class,'oxd-input-group')]//div[contains(@class,'oxd-select-text-input')]");
    private readonly By _searchButton = By.XPath("//button[@type='submit']");
    private readonly By _resetButton = By.XPath("//button[normalize-space()='Reset']");

    // Dropdown Options
    private readonly By _dropdownOptions = By.CssSelector("div.oxd-select-option");
    private readonly By _autocompleteSuggestions = By.CssSelector("div.oxd-autocomplete-option");

    // Results Table
    private readonly By _resultsTable = By.CssSelector("div.oxd-table");
    private readonly By _tableRows = By.CssSelector("div.oxd-table-body div.oxd-table-row");
    private readonly By _recordsFoundText = By.CssSelector("span.oxd-text--span");
    private readonly By _noRecordsMessage = By.XPath("//span[contains(text(),'No Records Found')]");

    // Column Cells
    private readonly By _usernameCells = By.XPath("//div[@class='oxd-table-body']//div[@role='row']//div[@role='cell'][2]//div");
    private readonly By _userRoleCells = By.XPath("//div[@class='oxd-table-body']//div[@role='row']//div[@role='cell'][3]//div");
    private readonly By _employeeNameCells = By.XPath("//div[@class='oxd-table-body']//div[@role='row']//div[@role='cell'][4]//div");
    private readonly By _statusCells = By.XPath("//div[@class='oxd-table-body']//div[@role='row']//div[@role='cell'][5]//div");

    // Loader
    private readonly By _loadingSpinner = By.CssSelector("div.oxd-loading-spinner");

    // Toast Notifications
    private readonly By _toastContainer = By.CssSelector("div.oxd-toast-container");
    private readonly By _toastMessage = By.CssSelector("div.oxd-toast-container div.oxd-toast-content p.oxd-text");
    private readonly By _successToast = By.CssSelector("div.oxd-toast--success");

    #region Navigation Methods

    public void NavigateToAdminUserManagement()
    {
        string baseUrl = ConfigReader.GetBaseUrl();
        NavigateTo($"{baseUrl}/web/index.php/admin/viewSystemUsers");
        WaitForPageToLoad();
        Console.WriteLine("[INFO] Navigated to Admin User Management page");
    }

    public void WaitForPageToLoad()
    {
        ElementHelper.WaitForLoaderToDisappear(Driver, _loadingSpinner, 15);
        ElementHelper.Sleep(500);
    }

    #endregion

    #region Search Form Methods

    public void EnterUsername(string username)
    {
        ElementHelper.WaitForElementToBeVisible(Driver, _usernameInput, 10);
        ElementHelper.SafeSendKeys(Driver, _usernameInput, username);
        Console.WriteLine($"[INFO] Entered username: {username}");
    }

    public void SelectUserRole(string role)
    {
        SelectFromCustomDropdown(_userRoleDropdown, role);
        Console.WriteLine($"[INFO] Selected User Role: {role}");
    }

    public void EnterEmployeeName(string employeeName)
    {
        ElementHelper.WaitForElementToBeVisible(Driver, _employeeNameInput, 10);
        ElementHelper.SafeSendKeys(Driver, _employeeNameInput, employeeName);
        Console.WriteLine($"[INFO] Entered employee name: {employeeName}");
        ElementHelper.Sleep(1500);
    }

    public void SelectStatus(string status)
    {
        SelectFromCustomDropdown(_statusDropdown, status);
        Console.WriteLine($"[INFO] Selected Status: {status}");
    }

    public void ClickSearchButton()
    {
        ElementHelper.SafeClickElement(Driver, _searchButton);
        Console.WriteLine("[INFO] Clicked Search button");
        WaitForPageToLoad();
    }

    public void ClickResetButton()
    {
        ElementHelper.SafeClickElement(Driver, _resetButton);
        Console.WriteLine("[INFO] Clicked Reset button");
        WaitForPageToLoad();
    }

    private void SelectFromCustomDropdown(By dropdownLocator, string optionText)
    {
        ElementHelper.SafeClickElement(Driver, dropdownLocator);
        ElementHelper.Sleep(500);

        var optionLocator = By.XPath(
            $"//div[contains(@class,'oxd-select-option')]//span[text()='{optionText}']" +
            $" | //div[contains(@class,'oxd-select-option') and contains(text(),'{optionText}')]");

        ElementHelper.WaitForElementToBeVisible(Driver, _dropdownOptions, 10);
        ElementHelper.SafeClickElement(Driver, optionLocator);
    }

    #endregion

    #region Results Verification Methods

    public bool AreResultsDisplayed()
    {
        bool displayed = ElementHelper.IsElementDisplayed(Driver, _resultsTable, 10);
        if (displayed)
        {
            int rowCount = Driver.FindElements(_tableRows).Count;
            Console.WriteLine($"[INFO] Results displayed: {rowCount} row(s) found");
            return rowCount > 0;
        }
        return false;
    }

    public bool IsNoRecordsMessageDisplayed()
    {
        return ElementHelper.IsElementDisplayed(Driver, _noRecordsMessage, 5);
    }

    public int GetRecordsCount()
    {
        try
        {
            string text = ElementHelper.GetTextFromElement(Driver, _recordsFoundText, 10);
            string number = new string(text.Where(char.IsDigit).ToArray());
            return int.Parse(number);
        }
        catch
        {
            return 0;
        }
    }

    public bool VerifyUsernameInResults(string expectedUsername)
    {
        try
        {
            var usernameLocator = By.XPath($"//div[@class='oxd-table-body']//div[@role='cell']//div[text()='{expectedUsername}']");
            bool found = ElementHelper.IsElementDisplayed(Driver, usernameLocator, 10);
            Console.WriteLine(found ? 
                $"[INFO] Username '{expectedUsername}' found in results" :
                $"[ERROR] Username '{expectedUsername}' NOT found in results");
            return found;
        }
        catch (Exception e)
        {
            Console.WriteLine($"[ERROR] Error verifying username: {e.Message}");
            return false;
        }
    }

    public List<string> GetAllUsernames() => ElementHelper.GetTextFromElements(Driver, _usernameCells, 10);
    public List<string> GetAllUserRoles() => ElementHelper.GetTextFromElements(Driver, _userRoleCells, 10);
    public List<string> GetAllStatuses() => ElementHelper.GetTextFromElements(Driver, _statusCells, 10);

    public bool VerifyAllResultsHaveUserRole(string expectedRole)
    {
        var roles = GetAllUserRoles();
        bool allMatch = roles.All(role => role == expectedRole);
        Console.WriteLine(allMatch ?
            $"[INFO] All {roles.Count} results have User Role: {expectedRole}" :
            $"[ERROR] Not all results have User Role: {expectedRole}");
        return allMatch;
    }

    public bool VerifyAllResultsHaveStatus(string expectedStatus)
    {
        var statuses = GetAllStatuses();
        bool allMatch = statuses.All(status => status == expectedStatus);
        Console.WriteLine(allMatch ?
            $"[INFO] All {statuses.Count} results have Status: {expectedStatus}" :
            $"[ERROR] Not all results have Status: {expectedStatus}");
        return allMatch;
    }

    #endregion

    #region Field State Verification

    public bool IsUsernameFieldEmpty()
    {
        string value = ElementHelper.GetInputValue(Driver, _usernameInput, 10);
        return string.IsNullOrEmpty(value);
    }

    public string GetUserRoleDropdownText() => ElementHelper.GetTextFromElement(Driver, _userRoleDropdown, 10);
    public string GetStatusDropdownText() => ElementHelper.GetTextFromElement(Driver, _statusDropdown, 10);

    #endregion

    #region Dynamic Data Capture Methods

    public string? CaptureFirstUsername()
    {
        try
        {
            var usernames = GetAllUsernames();
            if (usernames.Count > 0)
            {
                _capturedUsername = usernames[0];
                Console.WriteLine($"[INFO] Captured username: {_capturedUsername}");
                return _capturedUsername;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"[WARN] Could not capture username: {e.Message}");
        }
        return null;
    }

    public string? CaptureExistingUsername()
    {
        WaitForPageToLoad();
        ElementHelper.Sleep(1000);
        return CaptureFirstUsername();
    }

    public string? GetCapturedUsername() => _capturedUsername;

    public int CaptureRecordsCount()
    {
        _capturedRecordsCount = GetRecordsCount();
        Console.WriteLine($"[INFO] Captured records count: {_capturedRecordsCount}");
        return _capturedRecordsCount;
    }

    #endregion

    #region Autocomplete Methods

    public void TypePartialEmployeeName(string partialName)
    {
        ElementHelper.WaitForElementToBeVisible(Driver, _employeeNameInput, 10);
        ElementHelper.SafeSendKeys(Driver, _employeeNameInput, partialName);
        Console.WriteLine($"[INFO] Typed partial employee name: {partialName}");
        ElementHelper.Sleep(1500);
    }

    public bool AreAutocompleteSuggestionsVisible()
    {
        bool visible = ElementHelper.IsElementDisplayed(Driver, _autocompleteSuggestions, 5);
        Console.WriteLine(visible ? "[INFO] Autocomplete suggestions visible" : "[INFO] Autocomplete suggestions not visible");
        return visible;
    }

    public void SelectFirstAutocompleteSuggestion()
    {
        try
        {
            var firstSuggestion = By.CssSelector("div.oxd-autocomplete-option:first-child");
            ElementHelper.WaitForElementToBeVisible(Driver, _autocompleteSuggestions, 10);
            
            string suggestionText = ElementHelper.GetTextFromElement(Driver, firstSuggestion, 5);
            _capturedEmployeeName = suggestionText;
            
            ElementHelper.SafeClickElement(Driver, firstSuggestion);
            Console.WriteLine($"[INFO] Selected first autocomplete suggestion: {suggestionText}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[WARN] Could not select first suggestion: {e.Message}");
        }
    }

    #endregion

    #region Toast Methods

    public bool IsToastDisplayed()
    {
        bool displayed = ElementHelper.IsElementDisplayed(Driver, _toastContainer, 3);
        if (displayed) Console.WriteLine("[INFO] Toast notification detected");
        return displayed;
    }

    public string GetToastMessage()
    {
        try
        {
            if (IsToastDisplayed())
            {
                string message = ElementHelper.GetTextFromElement(Driver, _toastMessage, 3);
                Console.WriteLine($"[INFO] Toast message: {message}");
                return message;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"[WARN] Could not get toast message: {e.Message}");
        }
        return "";
    }

    public bool IsNoDataAvailable()
    {
        ElementHelper.Sleep(1000);

        if (IsNoRecordsMessageDisplayed())
        {
            Console.WriteLine("[INFO] No Records Found - displayed in table");
            return true;
        }

        if (IsToastDisplayed())
        {
            string message = GetToastMessage().ToLower();
            if (message.Contains("no records") || message.Contains("no data") ||
                message.Contains("not found") || message.Contains("invalid") ||
                string.IsNullOrEmpty(message))
            {
                Console.WriteLine("[INFO] No data available - indicated by toast notification");
                return true;
            }
        }

        try
        {
            int rowCount = Driver.FindElements(_tableRows).Count;
            if (rowCount == 0)
            {
                Console.WriteLine("[INFO] No Records Found - table has 0 rows");
                return true;
            }
        }
        catch { }

        return false;
    }

    public bool VerifySearchOutcome(string filterDescription)
    {
        ElementHelper.Sleep(1000);

        if (AreResultsDisplayed())
        {
            Console.WriteLine($"[INFO] Search returned results for filter: {filterDescription}");
            return true;
        }

        if (IsNoDataAvailable())
        {
            Console.WriteLine($"[INFO] No data available for filter: {filterDescription} (This is acceptable)");
            return true;
        }

        Console.WriteLine($"[WARN] Unexpected state - no results and no 'no data' message for: {filterDescription}");
        return false;
    }

    #endregion
}


