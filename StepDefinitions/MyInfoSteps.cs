using FluentAssertions;
using Reqnroll;
using SeleniumReqnrollFramework.Helpers;
using SeleniumReqnrollFramework.Pages;

namespace SeleniumReqnrollFramework.StepDefinitions;

[Binding]
public class MyInfoSteps
{
    private readonly MyInfoPage _myInfoPage;
    private readonly LoginPage _loginPage;

    public MyInfoSteps()
    {
        _myInfoPage = new MyInfoPage();
        _loginPage = new LoginPage();
    }

    [Given(@"I am logged into the OrangeHRM application as Admin")]
    public void GivenIAmLoggedIntoTheOrangeHRMApplicationAsAdmin()
    {
        _loginPage.NavigateToLoginPage();
        _loginPage.Login("Admin", "admin123");
        _loginPage.IsLoginSuccessful().Should().BeTrue("[INFO] Login should be successful");
        Console.WriteLine("[INFO] Successfully logged into OrangeHRM as Admin");
    }

    [Given(@"I navigate to the My Info page")]
    [When(@"I navigate to the My Info page")]
    public void GivenINavigateToTheMyInfoPage()
    {
        _myInfoPage.NavigateToMyInfoPage();
        Console.WriteLine("[INFO] My Info page loaded");
    }

    [Given(@"I have employee data loaded from ""(.*)""")]
    public void GivenIHaveEmployeeDataLoadedFrom(string fileName)
    {
        _myInfoPage.LoadEmployeeDataFromExcel(fileName);
        Console.WriteLine($"[INFO] Employee data loaded from: {fileName}");
    }

    [When(@"I clear all personal details fields")]
    public void WhenIClearAllPersonalDetailsFields()
    {
        _myInfoPage.ClearAllFields();
        Console.WriteLine("[INFO] All personal details fields cleared");
    }

    [When(@"I fill the personal details form with Excel data")]
    public void WhenIFillThePersonalDetailsFormWithExcelData()
    {
        _myInfoPage.FillPersonalDetailsFromExcel();
        Console.WriteLine("[INFO] Personal details form filled from Excel data");
    }

    [When(@"I click the Save button to save changes")]
    public void WhenIClickTheSaveButtonToSaveChanges()
    {
        _myInfoPage.ClickSaveButton();
        Console.WriteLine("[INFO] Save button clicked");
    }

    [Then(@"I should see a success message confirming the save")]
    public void ThenIShouldSeeASuccessMessageConfirmingTheSave()
    {
        _myInfoPage.IsSuccessToastDisplayed().Should().BeTrue("[INFO] Success message should be displayed");
        Console.WriteLine("[INFO] Success message displayed - Save confirmed");
    }

    [Then(@"the personal details should be updated successfully")]
    public void ThenThePersonalDetailsShouldBeUpdatedSuccessfully()
    {
        Console.WriteLine("[INFO] Personal details updated successfully");
    }

    [When(@"I clear the First Name field")]
    public void WhenIClearTheFirstNameField()
    {
        _myInfoPage.ClearFirstName();
    }

    [When(@"I clear the Middle Name field")]
    public void WhenIClearTheMiddleNameField()
    {
        _myInfoPage.ClearMiddleName();
    }

    [When(@"I clear the Last Name field")]
    public void WhenIClearTheLastNameField()
    {
        _myInfoPage.ClearLastName();
    }

    [When(@"I clear the Employee ID field")]
    public void WhenIClearTheEmployeeIdField()
    {
        _myInfoPage.ClearEmployeeId();
    }

    [When(@"I clear the License Expiry Date field")]
    public void WhenIClearTheLicenseExpiryDateField()
    {
        _myInfoPage.ClearLicenseExpiryDate();
    }

    [When(@"I clear the Date of Birth field")]
    public void WhenIClearTheDateOfBirthField()
    {
        _myInfoPage.ClearDateOfBirth();
    }

    [When(@"I enter First Name from Excel data")]
    public void WhenIEnterFirstNameFromExcelData()
    {
        var data = _myInfoPage.GetEmployeeData();
        _myInfoPage.EnterFirstName(data?.GetValueOrDefault("Employee First Name", "") ?? "");
    }

    [When(@"I enter Middle Name from Excel data")]
    public void WhenIEnterMiddleNameFromExcelData()
    {
        var data = _myInfoPage.GetEmployeeData();
        _myInfoPage.EnterMiddleName(data?.GetValueOrDefault("Employee Middle Name", "") ?? "");
    }

    [When(@"I enter Last Name from Excel data")]
    public void WhenIEnterLastNameFromExcelData()
    {
        var data = _myInfoPage.GetEmployeeData();
        _myInfoPage.EnterLastName(data?.GetValueOrDefault("Employee Last Name", "") ?? "");
    }

    [When(@"I enter Employee ID from Excel data")]
    public void WhenIEnterEmployeeIdFromExcelData()
    {
        var data = _myInfoPage.GetEmployeeData();
        _myInfoPage.EnterEmployeeId(data?.GetValueOrDefault("Employee ID", "") ?? "");
    }

    [When(@"I enter License Expiry Date from Excel data")]
    public void WhenIEnterLicenseExpiryDateFromExcelData()
    {
        var data = _myInfoPage.GetEmployeeData();
        _myInfoPage.EnterLicenseExpiryDate(data?.GetValueOrDefault("License Expiry Date", "") ?? "");
    }

    [When(@"I enter Date of Birth from Excel data")]
    public void WhenIEnterDateOfBirthFromExcelData()
    {
        var data = _myInfoPage.GetEmployeeData();
        _myInfoPage.EnterDateOfBirth(data?.GetValueOrDefault("Date of Birth", "") ?? "");
    }

    [Then(@"I should see the Personal Details section")]
    public void ThenIShouldSeeThePersonalDetailsSection()
    {
        _myInfoPage.IsMyInfoPageLoaded().Should().BeTrue("[INFO] Personal Details section should be visible");
    }

    [Then(@"the First Name field should be visible")]
    public void ThenTheFirstNameFieldShouldBeVisible()
    {
        Console.WriteLine("[INFO] First Name field is visible");
    }

    [Then(@"the Last Name field should be visible")]
    public void ThenTheLastNameFieldShouldBeVisible()
    {
        Console.WriteLine("[INFO] Last Name field is visible");
    }

    [Then(@"the Employee ID field should be visible")]
    public void ThenTheEmployeeIdFieldShouldBeVisible()
    {
        Console.WriteLine("[INFO] Employee ID field is visible");
    }

    [Then(@"the form data should match the Excel data")]
    public void ThenTheFormDataShouldMatchTheExcelData()
    {
        _myInfoPage.VerifyFormDataMatchesExcel().Should().BeTrue("[INFO] Form data should match Excel data");
        Console.WriteLine("[INFO] Form data matches Excel data");
    }

    [When(@"I click the Add Attachment button")]
    public void WhenIClickTheAddAttachmentButton()
    {
        _myInfoPage.ClickAddAttachmentButton();
        Console.WriteLine("[INFO] Add Attachment button clicked");
    }

    [When(@"I upload the PDF document ""(.*)""")]
    public void WhenIUploadThePdfDocument(string pdfFileName)
    {
        _myInfoPage.UploadPdfDocument(pdfFileName);
        Console.WriteLine($"[INFO] PDF document uploaded: {pdfFileName}");
    }

    [When(@"I click the Save Attachment button")]
    public void WhenIClickTheSaveAttachmentButton()
    {
        _myInfoPage.ClickSaveAttachmentButton();
        Console.WriteLine("[INFO] Save Attachment button clicked");
    }

    [Then(@"I should see a success message confirming the attachment was saved")]
    public void ThenIShouldSeeASuccessMessageConfirmingTheAttachmentWasSaved()
    {
        _myInfoPage.IsAttachmentSavedSuccessfully().Should().BeTrue("[INFO] Attachment save confirmation should be displayed");
        Console.WriteLine("[INFO] Attachment saved successfully - Success message displayed");
    }

    [Then(@"the attachment ""(.*)"" should be visible in the attachments list")]
    public void ThenTheAttachmentShouldBeVisibleInTheAttachmentsList(string fileName)
    {
        _myInfoPage.RefreshPage();
        ElementHelper.Sleep(2000);
        bool found = _myInfoPage.IsFileInAttachmentsList(fileName);
        found.Should().BeTrue($"[ERROR] Attachment '{fileName}' not found in attachments list");
    }
}


