Feature: Login Functionality

  As a user
  I want to be able to login to the OrangeHRM application
  So that I can access the HR management features

  Background:
    Given I navigate to the login page

  @Smoke @Login
  Scenario: Successful login with valid credentials
    When I enter username "Admin"
    And I enter password "admin123"
    And I click the login button
    Then I should be logged in successfully
    And I should see the logout button

  @Login @Negative
  Scenario: Failed login with invalid username
    When I enter username "InvalidUser"
    And I enter password "admin123"
    And I click the login button
    Then I should see an error message
    And the error message should contain "Invalid credentials"

  @Login @Negative
  Scenario: Failed login with invalid password
    When I enter username "Admin"
    And I enter password "wrongpassword"
    And I click the login button
    Then I should see an error message
    And the error message should contain "Invalid credentials"

  @Login
  Scenario: Logout after successful login
    When I enter username "Admin"
    And I enter password "admin123"
    And I click the login button
    Then I should be logged in successfully
    When I click the logout button
    Then I should be logged out successfully


