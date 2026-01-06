using OpenQA.Selenium;
using SeleniumReqnrollFramework.Helpers;
using SeleniumReqnrollFramework.Utilities;
using System.Globalization;

namespace SeleniumReqnrollFramework.Pages;

/// <summary>
/// MyInfoPage - Page Object for the My Info / Personal Details page
/// URL: /web/index.php/pim/viewMyDetails
/// </summary>
public class MyInfoPage : BasePage
{
    // Navigation Locators
    private readonly By _myInfoLink = By.XPath("//a[@href='/web/index.php/pim/viewMyDetails']");
    private readonly By _personalDetailsHeader = By.XPath("//h6[text()='Personal Details']");

    // Form Field Locators
    private readonly By _firstNameInput = By.Name("firstName");
    private readonly By _middleNameInput = By.Name("middleName");
    private readonly By _lastNameInput = By.Name("lastName");
    private readonly By _employeeIdInput = By.XPath("//label[text()='Employee Id']/ancestor::div[contains(@class,'oxd-input-group')]//input");
    private readonly By _licenseExpiryDateInput = By.XPath("//label[text()='License Expiry Date']/ancestor::div[contains(@class,'oxd-input-group')]//input");
    private readonly By _dateOfBirthInput = By.XPath("//label[text()='Date of Birth']/ancestor::div[contains(@class,'oxd-input-group')]//input");

    // Button Locators
    private readonly By _saveButton = By.XPath("(//button[@type='submit'])[1]");
    private readonly By _addAttachmentButton = By.XPath("//button[@class='oxd-button oxd-button--medium oxd-button--text']");
    private readonly By _saveAttachmentButton = By.XPath(
        "//div[contains(@class,'orangehrm-attachment')]//button[@type='submit'] | " +
        "//form[.//input[@type='file']]//button[@type='submit'] | " +
        "//div[contains(@class,'oxd-file-input')]//ancestor::form//button[@type='submit']");

    // File Upload Locators
    private readonly By _fileInput = By.CssSelector("input[type='file']");
    private readonly By _fileInputDiv = By.XPath("//div[contains(@class,'oxd-file-input-div')]");

    // Toast Locators
    private readonly By _successToast = By.CssSelector("div.oxd-toast--success");
    private readonly By _toastMessage = By.CssSelector("div.oxd-toast-content p.oxd-text");

    // Loader
    private readonly By _loadingSpinner = By.CssSelector("div.oxd-loading-spinner");

    // Attachment Table
    private readonly By _attachmentTableContainer = By.CssSelector("div.orangehrm-attachment");
    private readonly By _attachmentTableRows = By.CssSelector("div.orangehrm-attachment div.oxd-table-body div.oxd-table-row");

    // Stored Data
    private Dictionary<string, string>? _employeeData;

    private const string TestDataPath = "TestData/";

    #region Navigation Methods

    public void NavigateToMyInfoPage()
    {
        ElementHelper.WaitForElementToBeVisible(Driver, _myInfoLink, 10);
        ElementHelper.SafeClickElement(Driver, _myInfoLink);
        WaitForPageToLoad();
        Console.WriteLine("[INFO] Navigated to My Info page");
    }

    public void NavigateToMyInfoPageDirectly()
    {
        string baseUrl = ConfigReader.GetBaseUrl();
        NavigateTo($"{baseUrl}/web/index.php/pim/viewMyDetails");
        WaitForPageToLoad();
        Console.WriteLine("[INFO] Navigated to My Info page (direct URL)");
    }

    public bool IsMyInfoPageLoaded()
    {
        bool loaded = ElementHelper.IsElementDisplayed(Driver, _personalDetailsHeader, 10);
        Console.WriteLine(loaded ? "[INFO] My Info page loaded" : "[ERROR] My Info page not loaded");
        return loaded;
    }

    public void WaitForPageToLoad()
    {
        ElementHelper.WaitForLoaderToDisappear(Driver, _loadingSpinner, 15);
        ElementHelper.Sleep(1000);
    }

    #endregion

    #region Excel Data Methods

    public void LoadEmployeeDataFromExcel()
    {
        _employeeData = ExcelReader.GetEmployeeData();
        Console.WriteLine("[INFO] Loaded employee data from Excel");
    }

    public void LoadEmployeeDataFromExcel(string fileName)
    {
        _employeeData = ExcelReader.GetEmployeeData(fileName, null);
        Console.WriteLine($"[INFO] Loaded employee data from: {fileName}");
    }

    public Dictionary<string, string>? GetEmployeeData() => _employeeData;

    #endregion

    #region Clear Field Methods

    public void ClearFirstName()
    {
        ElementHelper.WaitForElementToBeVisible(Driver, _firstNameInput, 10);
        ElementHelper.ClearFieldWithKeyboard(Driver, _firstNameInput, 10);
        Console.WriteLine("[INFO] Cleared First Name field");
    }

    public void ClearMiddleName()
    {
        ElementHelper.ClearFieldWithKeyboard(Driver, _middleNameInput, 10);
        Console.WriteLine("[INFO] Cleared Middle Name field");
    }

    public void ClearLastName()
    {
        ElementHelper.ClearFieldWithKeyboard(Driver, _lastNameInput, 10);
        Console.WriteLine("[INFO] Cleared Last Name field");
    }

    public void ClearEmployeeId()
    {
        ElementHelper.ClearFieldWithKeyboard(Driver, _employeeIdInput, 10);
        Console.WriteLine("[INFO] Cleared Employee ID field");
    }

    public void ClearLicenseExpiryDate()
    {
        ElementHelper.ClearFieldWithKeyboard(Driver, _licenseExpiryDateInput, 10);
        Console.WriteLine("[INFO] Cleared License Expiry Date field");
    }

    public void ClearDateOfBirth()
    {
        ElementHelper.ClearFieldWithKeyboard(Driver, _dateOfBirthInput, 10);
        Console.WriteLine("[INFO] Cleared Date of Birth field");
    }

    public void ClearAllFields()
    {
        Console.WriteLine("[INFO] Clearing all personal details fields...");
        ClearFirstName();
        ClearMiddleName();
        ClearLastName();
        ClearEmployeeId();
        ClearLicenseExpiryDate();
        ClearDateOfBirth();
        Console.WriteLine("[INFO] All fields cleared");
    }

    #endregion

    #region Enter Data Methods

    public void EnterFirstName(string firstName)
    {
        if (!string.IsNullOrEmpty(firstName))
        {
            ElementHelper.AppendText(Driver, _firstNameInput, firstName, 10);
            Console.WriteLine($"[INFO] Entered First Name: {firstName}");
        }
    }

    public void EnterMiddleName(string middleName)
    {
        if (!string.IsNullOrEmpty(middleName))
        {
            ElementHelper.AppendText(Driver, _middleNameInput, middleName, 10);
            Console.WriteLine($"[INFO] Entered Middle Name: {middleName}");
        }
    }

    public void EnterLastName(string lastName)
    {
        if (!string.IsNullOrEmpty(lastName))
        {
            ElementHelper.AppendText(Driver, _lastNameInput, lastName, 10);
            Console.WriteLine($"[INFO] Entered Last Name: {lastName}");
        }
    }

    public void EnterEmployeeId(string employeeId)
    {
        if (!string.IsNullOrEmpty(employeeId))
        {
            ElementHelper.AppendText(Driver, _employeeIdInput, employeeId, 10);
            Console.WriteLine($"[INFO] Entered Employee ID: {employeeId}");
        }
    }

    public void EnterLicenseExpiryDate(string licenseExpiryDate)
    {
        if (!string.IsNullOrEmpty(licenseExpiryDate))
        {
            string formattedDate = ConvertToOrangeHrmDateFormat(licenseExpiryDate);
            ElementHelper.AppendText(Driver, _licenseExpiryDateInput, formattedDate, 10);
            Console.WriteLine($"[INFO] Entered License Expiry Date: {formattedDate}");
        }
    }

    public void EnterDateOfBirth(string dateOfBirth)
    {
        if (!string.IsNullOrEmpty(dateOfBirth))
        {
            string formattedDate = ConvertToOrangeHrmDateFormat(dateOfBirth);
            ElementHelper.AppendText(Driver, _dateOfBirthInput, formattedDate, 10);
            Console.WriteLine($"[INFO] Entered Date of Birth: {formattedDate}");
        }
    }

    private string ConvertToOrangeHrmDateFormat(string dateStr)
    {
        if (string.IsNullOrEmpty(dateStr))
            return dateStr;

        string[] formats = {
            "dd-MM-yyyy", "dd/MM/yyyy", "d-M-yyyy", "d/M/yyyy",
            "MM-dd-yyyy", "MM/dd/yyyy", "M-d-yyyy", "M/d/yyyy",
            "yyyy-MM-dd", "yyyy/MM/dd", "yyyy-dd-MM", "yyyy/dd/MM"
        };

        foreach (var format in formats)
        {
            if (DateTime.TryParseExact(dateStr.Trim(), format, CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate.ToString("yyyy-dd-MM");
            }
        }

        Console.WriteLine($"[WARN] Could not parse date: {dateStr}, using original value");
        return dateStr;
    }

    #endregion

    #region Fill Form from Excel

    public void FillPersonalDetailsFromExcel()
    {
        if (_employeeData == null || _employeeData.Count == 0)
            throw new Exception("[ERROR] No employee data loaded. Call LoadEmployeeDataFromExcel() first.");

        Console.WriteLine("[INFO] Filling personal details from Excel data...");

        EnterFirstName(_employeeData.GetValueOrDefault("Employee First Name", ""));
        EnterMiddleName(_employeeData.GetValueOrDefault("Employee Middle Name", ""));
        EnterLastName(_employeeData.GetValueOrDefault("Employee Last Name", ""));
        EnterEmployeeId(_employeeData.GetValueOrDefault("Employee ID", ""));
        EnterLicenseExpiryDate(_employeeData.GetValueOrDefault("License Expiry Date", ""));
        EnterDateOfBirth(_employeeData.GetValueOrDefault("Date of Birth", ""));

        Console.WriteLine("[INFO] Personal details filled from Excel");
    }

    #endregion

    #region Save and Verification Methods

    public void ClickSaveButton()
    {
        ElementHelper.ScrollIntoView(Driver, _saveButton);
        ElementHelper.SafeClickElement(Driver, _saveButton);
        Console.WriteLine("[INFO] Clicked Save button");
        ElementHelper.Sleep(2000);
    }

    public bool IsSuccessToastDisplayed()
    {
        bool displayed = ElementHelper.IsElementDisplayed(Driver, _successToast, 5);
        if (displayed)
        {
            string message = GetToastMessageText();
            Console.WriteLine($"[INFO] Success toast displayed: {message}");
        }
        return displayed;
    }

    public string GetToastMessageText()
    {
        try
        {
            return ElementHelper.GetTextFromElement(Driver, _toastMessage, 5);
        }
        catch
        {
            return "";
        }
    }

    public string GetFirstNameValue() => ElementHelper.GetInputValue(Driver, _firstNameInput, 10);
    public string GetMiddleNameValue() => ElementHelper.GetInputValue(Driver, _middleNameInput, 10);
    public string GetLastNameValue() => ElementHelper.GetInputValue(Driver, _lastNameInput, 10);

    public bool VerifyFormDataMatchesExcel()
    {
        if (_employeeData == null) return false;

        string expectedFirstName = _employeeData.GetValueOrDefault("Employee First Name", "");
        string expectedMiddleName = _employeeData.GetValueOrDefault("Employee Middle Name", "");
        string expectedLastName = _employeeData.GetValueOrDefault("Employee Last Name", "");

        string actualFirstName = GetFirstNameValue();
        string actualMiddleName = GetMiddleNameValue();
        string actualLastName = GetLastNameValue();

        bool firstNameMatch = actualFirstName == expectedFirstName;
        bool middleNameMatch = actualMiddleName == expectedMiddleName;
        bool lastNameMatch = actualLastName == expectedLastName;

        Console.WriteLine("[INFO] Verifying form data:");
        Console.WriteLine($"   First Name: {(firstNameMatch ? "PASS" : "FAIL")} {actualFirstName} (expected: {expectedFirstName})");
        Console.WriteLine($"   Middle Name: {(middleNameMatch ? "PASS" : "FAIL")} {actualMiddleName} (expected: {expectedMiddleName})");
        Console.WriteLine($"   Last Name: {(lastNameMatch ? "PASS" : "FAIL")} {actualLastName} (expected: {expectedLastName})");

        return firstNameMatch && middleNameMatch && lastNameMatch;
    }

    #endregion

    #region File Upload Methods

    public void ClickAddAttachmentButton()
    {
        ElementHelper.ScrollIntoView(Driver, _addAttachmentButton);
        ElementHelper.SafeClickElement(Driver, _addAttachmentButton);
        Console.WriteLine("[INFO] Clicked Add Attachment button");
        ElementHelper.Sleep(1000);
    }

    public void UploadFile(string fileName)
    {
        string filePath = Path.Combine(AppContext.BaseDirectory, TestDataPath, fileName);
        Console.WriteLine($"[INFO] Uploading file: {filePath}");

        if (!File.Exists(filePath))
            throw new Exception($"[ERROR] File not found: {filePath}");

        try
        {
            var fileInputElement = Driver.FindElement(_fileInput);
            fileInputElement.SendKeys(filePath);
            Console.WriteLine($"[INFO] File uploaded: {fileName}");
            ElementHelper.Sleep(1000);
        }
        catch (Exception)
        {
            Console.WriteLine("[WARN] Standard upload failed, trying alternative method...");
            UploadFileAlternative(filePath);
        }
    }

    private void UploadFileAlternative(string filePath)
    {
        var fileInputElement = Driver.FindElement(_fileInput);
        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].style.display='block'; arguments[0].style.visibility='visible';",
            fileInputElement);
        ElementHelper.Sleep(500);
        fileInputElement.SendKeys(filePath);
        Console.WriteLine("[INFO] File uploaded using alternative method");
    }

    public void UploadPdfDocument(string pdfFileName)
    {
        Console.WriteLine($"[INFO] Starting PDF upload: {pdfFileName}");
        UploadFile(pdfFileName);
    }

    public void ClickSaveAttachmentButton()
    {
        ElementHelper.Sleep(500);

        By[] buttonLocators = {
            By.XPath("//div[contains(@class,'orangehrm-attachment')]//button[@type='submit']"),
            By.XPath("//div[contains(@class,'oxd-file-input')]//ancestor::div[contains(@class,'oxd-form')]//button[@type='submit']"),
            By.XPath("(//button[@type='submit'])[last()]"),
            By.XPath("//button[@type='submit' and not(@data-v-6653c066)]"),
            By.XPath("(//button[@type='submit'])[2]")
        };

        foreach (var locator in buttonLocators)
        {
            try
            {
                if (ElementHelper.IsElementDisplayed(Driver, locator, 2))
                {
                    ElementHelper.ScrollIntoView(Driver, locator);
                    ElementHelper.SafeClickElement(Driver, locator);
                    Console.WriteLine("[INFO] Clicked Save Attachment button");
                    ElementHelper.Sleep(2000);
                    return;
                }
            }
            catch { }
        }

        throw new Exception("[ERROR] Could not find Save Attachment button");
    }

    public bool IsAttachmentSavedSuccessfully()
    {
        bool success = IsSuccessToastDisplayed();
        Console.WriteLine(success ? "[INFO] Attachment saved successfully!" : "[ERROR] Attachment save confirmation not received");
        return success;
    }

    public bool IsFileInAttachmentsList(string fileName)
    {
        try
        {
            ElementHelper.Sleep(2000);
            ScrollToAttachmentsTable();
            ElementHelper.Sleep(1000);

            By[] locators = {
                By.XPath($"//div[contains(@class,'orangehrm-attachment')]//div[contains(@class,'oxd-table-cell')]//div[contains(text(),'{fileName}')]"),
                By.XPath($"//div[contains(@class,'orangehrm-attachment')]//div[@role='row']//div[@role='cell'][2]//div[contains(text(),'{fileName}')]"),
                By.XPath($"//div[contains(@class,'oxd-table-body')]//div[contains(text(),'{fileName}')]"),
                By.XPath($"//div[contains(@class,'orangehrm-attachment')]//div[contains(@class,'oxd-table-row')]//div[contains(text(),'{fileName}')]")
            };

            foreach (var locator in locators)
            {
                try
                {
                    if (ElementHelper.IsElementDisplayed(Driver, locator, 3))
                    {
                        Console.WriteLine($"[INFO] File '{fileName}' found in attachments");
                        return true;
                    }
                }
                catch { }
            }

            Console.WriteLine($"[ERROR] File '{fileName}' not found in attachments");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine($"[ERROR] Exception while checking attachments: {e.Message}");
            return false;
        }
    }

    private void ScrollToAttachmentsTable()
    {
        try
        {
            if (ElementHelper.IsElementDisplayed(Driver, _attachmentTableContainer, 5))
            {
                ElementHelper.ScrollIntoView(Driver, _attachmentTableContainer);
                Console.WriteLine("[INFO] Scrolled to attachments table");
            }
        }
        catch
        {
            Console.WriteLine("[WARN] Could not scroll to attachments table");
        }
    }

    #endregion
}


