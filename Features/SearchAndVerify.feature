@Admin @UserManagement
Feature: Admin User Management - Search and Verify Functionality

  As an Administrator
  I want to search for system users in the Admin module
  So that I can quickly find and verify user information

  Background:
    Given I am logged into the OrangeHRM application
    And I navigate to the Admin User Management page

  # ============================================
  # SMOKE TESTS - Core Functionality
  # ============================================

  @Smoke @Search
  Scenario: Verify search functionality returns results for existing user
    When I capture an existing username from the results table
    And I search for that captured username
    And I click the Search button
    Then I should see search results displayed
    And the results should contain the searched username

  @Smoke @Search
  Scenario: Verify Admin user search returns valid results
    When I search for user with username "Admin"
    And I click the Search button
    Then I should see search results displayed
    And the results should contain username "Admin"

  # ============================================
  # FILTER SCENARIOS - User Role
  # ============================================

  @Search @Filter
  Scenario: Filter users by User Role - Admin
    When I select User Role as "Admin"
    And I click the Search button
    Then I should see search results displayed
    And all results should have User Role "Admin"

  @Search @Filter
  Scenario: Filter users by User Role - ESS
    When I select User Role as "ESS"
    And I click the Search button
    Then I should see search results displayed
    And all results should have User Role "ESS"

  # ============================================
  # FILTER SCENARIOS - Status
  # ============================================

  @Search @Filter
  Scenario: Filter users by Status - Enabled
    When I select Status as "Enabled"
    And I click the Search button
    Then I should see search results displayed
    And all results should have Status "Enabled"

  @Search @Filter
  Scenario: Filter users by Status - Disabled (handles no data gracefully)
    When I select Status as "Disabled"
    And I click the Search button
    Then I should see either results with Status "Disabled" or no records message

  # ============================================
  # COMBINED FILTER SCENARIOS
  # ============================================

  @Search @Regression
  Scenario: Search with combined filters - User Role and Status
    When I select User Role as "ESS"
    And I select Status as "Enabled"
    And I click the Search button
    Then I should see search results displayed
    And all results should have User Role "ESS"
    And all results should have Status "Enabled"

  @Search @Regression
  Scenario: Dynamic search - Capture and verify first ESS user
    When I select User Role as "ESS"
    And I click the Search button
    Then I should see search results displayed
    When I capture the first username from results
    And I click the Reset button
    And I search for the captured username
    And I click the Search button
    Then I should see search results displayed
    And the results should contain the captured username

  # ============================================
  # RESET FUNCTIONALITY
  # ============================================

  @Search @Reset
  Scenario: Reset search filters clears all fields
    When I search for user with username "Admin"
    And I select User Role as "ESS"
    And I select Status as "Enabled"
    And I click the Reset button
    Then all search fields should be cleared
    And the User Role dropdown should show "-- Select --"
    And the Status dropdown should show "-- Select --"

  # ============================================
  # NEGATIVE SCENARIOS
  # ============================================

  @Search @Negative
  Scenario: Search with non-existent username shows no records
    When I search for user with username "nonexistentuser99999xyz"
    And I click the Search button
    Then I should see "No Records Found" message

  @Search @Negative
  Scenario: Search with special characters handles gracefully
    When I search for user with username "!@#$%^&*()"
    And I click the Search button
    Then I should see "No Records Found" message

  # ============================================
  # EMPLOYEE NAME AUTOCOMPLETE
  # ============================================

  @Search @Autocomplete
  Scenario: Verify employee name autocomplete shows suggestions
    When I type partial employee name "a" in the search field
    Then I should see autocomplete suggestions appear
    When I select the first autocomplete suggestion
    And I click the Search button
    Then I should see search results displayed


