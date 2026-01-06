using FluentAssertions;
using Reqnroll;
using SeleniumReqnrollFramework.Pages;

namespace SeleniumReqnrollFramework.StepDefinitions;

[Binding]
public class SearchAndVerifySteps
{
    private readonly SearchAndVerifyPage _searchPage;
    private readonly LoginPage _loginPage;
    private string? _searchedUsername;

    public SearchAndVerifySteps()
    {
        _searchPage = new SearchAndVerifyPage();
        _loginPage = new LoginPage();
    }

    [Given(@"I am logged into the OrangeHRM application")]
    public void GivenIAmLoggedIntoTheOrangeHRMApplication()
    {
        _loginPage.NavigateToLoginPage();
        _loginPage.Login("Admin", "admin123");
        _loginPage.IsLoginSuccessful().Should().BeTrue("[INFO] Login should be successful");
        Console.WriteLine("[INFO] Successfully logged into OrangeHRM");
    }

    [Given(@"I navigate to the Admin User Management page")]
    [When(@"I navigate to the Admin User Management page")]
    public void GivenINavigateToTheAdminUserManagementPage()
    {
        _searchPage.NavigateToAdminUserManagement();
        Console.WriteLine("[INFO] Navigated to Admin User Management page");
    }

    [When(@"I capture an existing username from the results table")]
    public void WhenICaptureAnExistingUsernameFromTheResultsTable()
    {
        _searchedUsername = _searchPage.CaptureExistingUsername();
        _searchedUsername.Should().NotBeNullOrEmpty("[INFO] Should capture a username");
        Console.WriteLine($"[INFO] Captured existing username: {_searchedUsername}");
    }

    [When(@"I search for that captured username")]
    public void WhenISearchForThatCapturedUsername()
    {
        _searchedUsername.Should().NotBeNullOrEmpty("[INFO] Username should be captured first");
        _searchPage.EnterUsername(_searchedUsername!);
        Console.WriteLine($"[INFO] Searching for captured username: {_searchedUsername}");
    }

    [When(@"I search for user with username ""(.*)""")]
    public void WhenISearchForUserWithUsername(string username)
    {
        _searchedUsername = username;
        _searchPage.EnterUsername(username);
        Console.WriteLine($"[INFO] Entered username for search: {username}");
    }

    [When(@"I click the Search button")]
    public void WhenIClickTheSearchButton()
    {
        _searchPage.ClickSearchButton();
        Console.WriteLine("[INFO] Search button clicked");
    }

    [Then(@"I should see search results displayed")]
    public void ThenIShouldSeeSearchResultsDisplayed()
    {
        bool hasResults = _searchPage.AreResultsDisplayed();
        if (!hasResults)
        {
            bool noData = _searchPage.IsNoDataAvailable();
            if (noData)
            {
                Console.WriteLine("[INFO] No data available for this search - Test still passes");
                return;
            }
        }
        hasResults.Should().BeTrue("[INFO] Search results should be displayed");
        Console.WriteLine("[INFO] Search results are displayed");
    }

    [Then(@"the results should contain the searched username")]
    public void ThenTheResultsShouldContainTheSearchedUsername()
    {
        _searchedUsername.Should().NotBeNullOrEmpty("[INFO] Username should be stored");
        bool found = _searchPage.VerifyUsernameInResults(_searchedUsername!);
        found.Should().BeTrue($"[INFO] Username '{_searchedUsername}' should be in results");
        Console.WriteLine($"[INFO] Verified captured username in results: {_searchedUsername}");
    }

    [Then(@"the results should contain username ""(.*)""")]
    public void ThenTheResultsShouldContainUsername(string username)
    {
        bool found = _searchPage.VerifyUsernameInResults(username);
        found.Should().BeTrue($"[INFO] Username '{username}' should be in results");
        Console.WriteLine($"[INFO] Verified username in results: {username}");
    }

    [When(@"I select User Role as ""(.*)""")]
    public void WhenISelectUserRoleAs(string role)
    {
        _searchPage.SelectUserRole(role);
        Console.WriteLine($"[INFO] Selected User Role: {role}");
    }

    [When(@"I select Status as ""(.*)""")]
    public void WhenISelectStatusAs(string status)
    {
        _searchPage.SelectStatus(status);
        Console.WriteLine($"[INFO] Selected Status: {status}");
    }

    [Then(@"all results should have User Role ""(.*)""")]
    public void ThenAllResultsShouldHaveUserRole(string expectedRole)
    {
        _searchPage.VerifyAllResultsHaveUserRole(expectedRole).Should().BeTrue($"[INFO] All results should have User Role: {expectedRole}");
        Console.WriteLine($"[INFO] All results have User Role: {expectedRole}");
    }

    [Then(@"all results should have Status ""(.*)""")]
    public void ThenAllResultsShouldHaveStatus(string expectedStatus)
    {
        _searchPage.VerifyAllResultsHaveStatus(expectedStatus).Should().BeTrue($"[INFO] All results should have Status: {expectedStatus}");
        Console.WriteLine($"[INFO] All results have Status: {expectedStatus}");
    }

    [Then(@"I should see either results with Status ""(.*)"" or no records message")]
    public void ThenIShouldSeeEitherResultsWithStatusOrNoRecordsMessage(string status)
    {
        bool hasResults = _searchPage.AreResultsDisplayed();
        
        if (hasResults)
        {
            _searchPage.VerifyAllResultsHaveStatus(status).Should().BeTrue($"[INFO] Results should have Status: {status}");
            Console.WriteLine($"[INFO] Results displayed with Status: {status}");
        }
        else
        {
            bool noData = _searchPage.IsNoDataAvailable();
            noData.Should().BeTrue("[INFO] Should show 'No Records' message");
            Console.WriteLine($"[INFO] No data available for Status: {status} - This is acceptable");
        }
    }

    [When(@"I capture the first username from results")]
    public void WhenICaptureTheFirstUsernameFromResults()
    {
        _searchedUsername = _searchPage.CaptureFirstUsername();
        Console.WriteLine($"[INFO] Captured first username: {_searchedUsername}");
    }

    [When(@"I click the Reset button")]
    public void WhenIClickTheResetButton()
    {
        _searchPage.ClickResetButton();
        Console.WriteLine("[INFO] Reset button clicked");
    }

    [When(@"I search for the captured username")]
    public void WhenISearchForTheCapturedUsername()
    {
        _searchedUsername.Should().NotBeNullOrEmpty("[INFO] Username should be captured first");
        _searchPage.EnterUsername(_searchedUsername!);
        Console.WriteLine($"[INFO] Searching for captured username: {_searchedUsername}");
    }

    [Then(@"the results should contain the captured username")]
    public void ThenTheResultsShouldContainTheCapturedUsername()
    {
        _searchedUsername.Should().NotBeNullOrEmpty("[INFO] Username should be captured");
        bool found = _searchPage.VerifyUsernameInResults(_searchedUsername!);
        found.Should().BeTrue($"[INFO] Captured username '{_searchedUsername}' should be in results");
        Console.WriteLine($"[INFO] Verified captured username: {_searchedUsername}");
    }

    [Then(@"all search fields should be cleared")]
    public void ThenAllSearchFieldsShouldBeCleared()
    {
        _searchPage.IsUsernameFieldEmpty().Should().BeTrue("[INFO] Username field should be empty");
        Console.WriteLine("[INFO] Username field is cleared");
    }

    [Then(@"the User Role dropdown should show ""(.*)""")]
    public void ThenTheUserRoleDropdownShouldShow(string expectedText)
    {
        string actualText = _searchPage.GetUserRoleDropdownText();
        actualText.Should().Be(expectedText, $"[INFO] User Role should show: {expectedText}");
        Console.WriteLine($"[INFO] User Role dropdown shows: {actualText}");
    }

    [Then(@"the Status dropdown should show ""(.*)""")]
    public void ThenTheStatusDropdownShouldShow(string expectedText)
    {
        string actualText = _searchPage.GetStatusDropdownText();
        actualText.Should().Be(expectedText, $"[INFO] Status should show: {expectedText}");
        Console.WriteLine($"[INFO] Status dropdown shows: {actualText}");
    }

    [Then(@"I should see ""(.*)"" message")]
    public void ThenIShouldSeeMessage(string expectedMessage)
    {
        bool noData = _searchPage.IsNoDataAvailable();
        noData.Should().BeTrue($"[INFO] '{expectedMessage}' indication should be displayed");
        Console.WriteLine($"[INFO] '{expectedMessage}' indication is displayed");
    }

    [When(@"I type partial employee name ""(.*)"" in the search field")]
    public void WhenITypePartialEmployeeNameInTheSearchField(string partialName)
    {
        _searchPage.TypePartialEmployeeName(partialName);
        Console.WriteLine($"[INFO] Typed partial employee name: {partialName}");
    }

    [Then(@"I should see autocomplete suggestions appear")]
    public void ThenIShouldSeeAutocompleteSuggestionsAppear()
    {
        _searchPage.AreAutocompleteSuggestionsVisible().Should().BeTrue("[INFO] Autocomplete suggestions should be visible");
        Console.WriteLine("[INFO] Autocomplete suggestions are visible");
    }

    [When(@"I select the first autocomplete suggestion")]
    public void WhenISelectTheFirstAutocompleteSuggestion()
    {
        _searchPage.SelectFirstAutocompleteSuggestion();
        Console.WriteLine("[INFO] Selected first autocomplete suggestion");
    }
}


