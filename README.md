# Selenium Reqnroll Automation Framework

A robust BDD test automation framework built with C#, Selenium WebDriver, Reqnroll, and xUnit. Designed for maintainability, scalability, and professional reporting.

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running Tests](#running-tests)
- [Parallel Execution](#parallel-execution)
- [Reports](#reports)
- [Test Data Management](#test-data-management)
- [Author](#author)

## Overview

This framework automates the OrangeHRM demo application using the Page Object Model design pattern with BDD approach. It supports data-driven testing, parallel execution, and generates both HTML and PDF reports with email notifications.

**Target Application:** [OrangeHRM Demo](https://opensource-demo.orangehrmlive.com)

## Tech Stack

| Category | Technology |
|----------|------------|
| Language | C# (.NET 8.0) |
| BDD Framework | Reqnroll 2.0 (SpecFlow successor) |
| Test Runner | xUnit |
| Browser Automation | Selenium WebDriver 4.26 |
| Assertions | FluentAssertions |
| Excel Processing | EPPlus |
| PDF Generation | PdfSharpCore |
| Email Integration | Mailtrap API |
| Driver Management | WebDriverManager |

## Project Structure

```
DotNet Automation Framework/
|-- Drivers/
|   |-- DriverFactory.cs              # Thread-safe WebDriver management
|-- Features/
|   |-- Login.feature                 # Authentication scenarios
|   |-- MyInfo.feature                # Personal details management
|   |-- SearchAndVerify.feature       # User search functionality
|-- Helpers/
|   |-- ElementHelper.cs              # Selenium utility methods
|-- Hooks/
|   |-- ApplicationHooks.cs           # Test lifecycle hooks
|-- Pages/
|   |-- BasePage.cs                   # Common page functionality
|   |-- LoginPage.cs                  # Login page interactions
|   |-- MyInfoPage.cs                 # My Info page interactions
|   |-- SearchAndVerifyPage.cs        # Admin search page interactions
|-- Reports/
|   |-- HtmlReportGenerator.cs        # HTML report generation
|   |-- PdfReportGenerator.cs         # PDF report generation
|   |-- TestResult.cs                 # Result data model
|   |-- TestResultCollector.cs        # Result aggregation
|-- StepDefinitions/
|   |-- LoginSteps.cs                 # Login step implementations
|   |-- MyInfoSteps.cs                # My Info step implementations
|   |-- SearchAndVerifySteps.cs       # Search step implementations
|-- TestData/
|   |-- Test Data.xlsx                # Test data source
|   |-- TestDocument.pdf              # Sample file for upload tests
|-- Utilities/
|   |-- ConfigReader.cs               # Configuration management
|   |-- EmailHelper.cs                # Email notification service
|   |-- ExcelReader.cs                # Excel data reader
|-- appsettings.json                  # Application settings
|-- xunit.runner.json                 # xUnit runner configuration
```

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Chrome, Firefox, or Edge browser

## Installation

Clone and restore packages:

```bash
git clone https://github.com/yourusername/dotnet-selenium-reqnroll-framework.git
cd dotnet-selenium-reqnroll-framework
dotnet restore
dotnet build
```

## Configuration

Settings are managed in `appsettings.json`:

```json
{
  "TestSettings": {
    "Browser": "chrome",
    "BaseUrl": "https://opensource-demo.orangehrmlive.com",
    "DefaultTimeout": 10,
    "PageLoadTimeout": 30,
    "HeadlessMode": false
  },
  "EmailSettings": {
    "Enabled": true,
    "MailtrapApiToken": "your-api-token",
    "MailtrapInboxId": "your-inbox-id"
  }
}
```

**Supported Browsers:** chrome, firefox, edge, headless

## Running Tests

```bash
# Run all tests
dotnet test

# Run by category
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=Login"
dotnet test --filter "Category=Regression"

# Run specific feature
dotnet test --filter "FeatureTitle=Login"
dotnet test --filter "FeatureTitle=MyInfo"

# Verbose output
dotnet test --logger "console;verbosity=detailed"
```

## Parallel Execution

Configure parallel settings in `xunit.runner.json`:

```json
{
  "parallelizeTestCollections": true,
  "maxParallelThreads": 4
}
```

| Setting | Options |
|---------|---------|
| parallelizeTestCollections | true / false |
| maxParallelThreads | "default", "unlimited", or number (1, 2, 4, 8) |

## Reports

Reports are generated in `TestResults/Reports/` after each test run:

- **HTML Report:** Interactive report with test details and screenshots
- **PDF Report:** Printable summary report
- **Email Notification:** Automated email with attached reports via Mailtrap

Report Contents:
- Execution summary (Total, Passed, Failed, Pass Rate)
- Individual scenario results with duration
- Screenshots for failed tests
- Environment and browser details

## Test Data Management

Test data is stored in Excel format (`TestData/Test Data.xlsx`) and read using EPPlus library. This enables:

- Separation of test logic and test data
- Easy data updates without code changes
- Support for multiple data sets

## Test Scenarios

### Login Module
- Valid credentials authentication
- Invalid credentials handling
- Session management

### My Info Module
- Personal details update from Excel
- Form field validation
- PDF attachment upload
- Data persistence verification

### Admin User Search
- Username search
- Role-based filtering (Admin/ESS)
- Status filtering (Enabled/Disabled)
- Combined filter operations
- Empty results handling

## Tags Reference

| Tag | Purpose |
|-----|---------|
| @Smoke | Critical path tests |
| @Regression | Complete test suite |
| @Login | Authentication tests |
| @MyInfo | Personal details tests |
| @ExcelData | Data-driven tests |
| @Attachment | File upload tests |
| @Search | Search functionality |
| @Filter | Filter operations |
| @Negative | Error handling tests |

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Selenium.WebDriver | 4.26.1 | Browser automation |
| Reqnroll | 2.0.3 | BDD framework |
| Reqnroll.xUnit | 2.0.3 | xUnit integration |
| xUnit | 2.9.2 | Test runner |
| EPPlus | 7.5.0 | Excel processing |
| PdfSharpCore | 1.3.65 | PDF generation |
| FluentAssertions | 6.12.2 | Assertion library |
| WebDriverManager | 2.17.4 | Driver management |
| MailKit | 4.8.0 | Email support |

## Author

**Vishal Lodhiya**  
QA Engineer | ISTQB Certified

