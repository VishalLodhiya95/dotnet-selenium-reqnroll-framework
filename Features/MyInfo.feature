@MyInfo @PIM @DataDriven
Feature: My Info - Update Personal Details from Excel

  As an Employee
  I want to update my personal details using data from an Excel sheet
  So that my information is accurately maintained in the HR system

  Background:
    Given I am logged into the OrangeHRM application as Admin
    And I navigate to the My Info page

  # ============================================
  # SMOKE TEST - Complete Workflow
  # ============================================

  @Smoke @ExcelData
  Scenario: Update personal details from Excel data file
    Given I have employee data loaded from "Test Data.xlsx"
    When I clear all personal details fields
    And I fill the personal details form with Excel data
    And I click the Save button to save changes
    Then I should see a success message confirming the save
    And the personal details should be updated successfully

  # ============================================
  # INDIVIDUAL FIELD TESTS
  # ============================================

  @Regression @ExcelData
  Scenario: Update employee name fields from Excel
    Given I have employee data loaded from "Test Data.xlsx"
    When I clear the First Name field
    And I clear the Middle Name field
    And I clear the Last Name field
    And I enter First Name from Excel data
    And I enter Middle Name from Excel data
    And I enter Last Name from Excel data
    And I click the Save button to save changes
    Then I should see a success message confirming the save

  @Regression @ExcelData
  Scenario: Update employee ID from Excel
    Given I have employee data loaded from "Test Data.xlsx"
    When I clear the Employee ID field
    And I enter Employee ID from Excel data
    And I click the Save button to save changes
    Then I should see a success message confirming the save

  @Regression @ExcelData
  Scenario: Update date fields from Excel
    Given I have employee data loaded from "Test Data.xlsx"
    When I clear the License Expiry Date field
    And I clear the Date of Birth field
    And I enter License Expiry Date from Excel data
    And I enter Date of Birth from Excel data
    And I click the Save button to save changes
    Then I should see a success message confirming the save

  # ============================================
  # VERIFICATION SCENARIOS
  # ============================================

  @Smoke @Verify
  Scenario: Verify My Info page loads correctly
    Then I should see the Personal Details section
    And the First Name field should be visible
    And the Last Name field should be visible
    And the Employee ID field should be visible

  @Regression @Verify
  Scenario: Verify form data matches Excel after update
    Given I have employee data loaded from "Test Data.xlsx"
    When I clear all personal details fields
    And I fill the personal details form with Excel data
    And I click the Save button to save changes
    Then I should see a success message confirming the save
    And the form data should match the Excel data

  # ============================================
  # ATTACHMENT UPLOAD SCENARIOS
  # ============================================

  @Smoke @Attachment
  Scenario: Upload PDF document as attachment and verify success
    When I click the Add Attachment button
    And I upload the PDF document "TestDocument.pdf"
    And I click the Save Attachment button
    Then I should see a success message confirming the attachment was saved

  @Regression @Attachment
  Scenario: Upload attachment and verify it appears in attachments list
    When I click the Add Attachment button
    And I upload the PDF document "TestDocument.pdf"
    And I click the Save Attachment button
    Then I should see a success message confirming the attachment was saved
    And the attachment "TestDocument.pdf" should be visible in the attachments list

  @Regression @Attachment
  Scenario: Complete workflow - Update details and add attachment
    Given I have employee data loaded from "Test Data.xlsx"
    When I clear all personal details fields
    And I fill the personal details form with Excel data
    And I click the Save button to save changes
    Then I should see a success message confirming the save
    When I click the Add Attachment button
    And I upload the PDF document "TestDocument.pdf"
    And I click the Save Attachment button
    Then I should see a success message confirming the attachment was saved


