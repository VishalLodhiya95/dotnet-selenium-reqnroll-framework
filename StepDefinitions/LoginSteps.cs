using FluentAssertions;
using Reqnroll;
using SeleniumReqnrollFramework.Pages;

namespace SeleniumReqnrollFramework.StepDefinitions;

[Binding]
public class LoginSteps
{
    private readonly LoginPage _loginPage;

    public LoginSteps()
    {
        _loginPage = new LoginPage();
    }

    [Given(@"I navigate to the login page")]
    public void GivenINavigateToTheLoginPage()
    {
        _loginPage.NavigateToLoginPage();
    }

    [When(@"I enter username ""(.*)""")]
    public void WhenIEnterUsername(string username)
    {
        _loginPage.EnterUsername(username);
    }

    [When(@"I enter password ""(.*)""")]
    public void WhenIEnterPassword(string password)
    {
        _loginPage.EnterPassword(password);
    }

    [When(@"I click the login button")]
    public void WhenIClickTheLoginButton()
    {
        _loginPage.ClickLoginButton();
    }

    [Then(@"I should be logged in successfully")]
    public void ThenIShouldBeLoggedInSuccessfully()
    {
        _loginPage.IsLoginSuccessful().Should().BeTrue("[INFO] Login verification PASSED");
        Console.WriteLine("[INFO] Login verification PASSED");
    }

    [Then(@"I should see the logout button")]
    public void ThenIShouldSeeTheLogoutButton()
    {
        _loginPage.IsLoggedIn().Should().BeTrue("[INFO] Logout button is visible");
        Console.WriteLine("[INFO] Logout button is visible");
    }

    [Then(@"I should see an error message")]
    public void ThenIShouldSeeAnErrorMessage()
    {
        _loginPage.IsErrorMessageDisplayed().Should().BeTrue("[INFO] Error message is displayed");
        Console.WriteLine("[INFO] Error message is displayed");
    }

    [Then(@"the error message should contain ""(.*)""")]
    public void ThenTheErrorMessageShouldContain(string expectedText)
    {
        string actualMessage = _loginPage.GetErrorMessageText();
        actualMessage.Should().Contain(expectedText, $"[INFO] Error message contains: {expectedText}");
        Console.WriteLine($"[INFO] Error message contains: {expectedText}");
    }

    [When(@"I click the logout button")]
    public void WhenIClickTheLogoutButton()
    {
        _loginPage.ClickLogout();
    }

    [Then(@"I should be logged out successfully")]
    public void ThenIShouldBeLoggedOutSuccessfully()
    {
        // After logout, we should see the login page again
        _loginPage.NavigateToLoginPage();
        Console.WriteLine("[INFO] Logged out successfully");
    }
}


