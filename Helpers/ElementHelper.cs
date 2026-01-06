using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumReqnrollFramework.Helpers;

/// <summary>
/// ElementHelper - Common helper methods for interacting with web elements
/// </summary>
public static class ElementHelper
{
    #region Click Methods

    /// <summary>
    /// Wait for element to be clickable and click it with retry logic
    /// </summary>
    public static void SafeClickElement(IWebDriver driver, By locator, int maxRetries = 3, int timeoutInSeconds = 10)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                element.Click();
                Console.WriteLine($"[INFO] Successfully clicked element: {locator} (attempt {attempt})");
                return;
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine($"[WARN] Stale element, retrying... (attempt {attempt}/{maxRetries})");
                if (attempt == maxRetries)
                    throw new Exception($"Failed to click element after {maxRetries} attempts: {locator}");
            }
            catch (ElementClickInterceptedException)
            {
                Console.WriteLine($"[WARN] Element click intercepted, trying JavaScript click... (attempt {attempt})");
                try
                {
                    var element = driver.FindElement(locator);
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
                    Console.WriteLine($"[INFO] Successfully clicked element using JavaScript: {locator}");
                    return;
                }
                catch (Exception jsEx)
                {
                    if (attempt == maxRetries)
                        throw new Exception($"Failed to click element: {locator}", jsEx);
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"[WARN] Timeout waiting for element... (attempt {attempt}/{maxRetries})");
                if (attempt == maxRetries)
                    throw new Exception($"Element not clickable after {timeoutInSeconds}s: {locator}");
            }
            Sleep(500);
        }
    }

    #endregion

    #region Send Keys Methods

    /// <summary>
    /// Wait for element and send keys
    /// </summary>
    public static void SafeSendKeys(IWebDriver driver, By locator, string text, int timeoutInSeconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            element.Clear();
            element.SendKeys(text);
            Console.WriteLine($"[INFO] Successfully entered text: '{text}'");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to send keys to element: {locator}", e);
        }
    }

    /// <summary>
    /// Append text without clearing first
    /// </summary>
    public static void AppendText(IWebDriver driver, By locator, string text, int timeoutInSeconds = 10)
    {
        try
        {
            var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
            element.SendKeys(text);
            Console.WriteLine($"[INFO] Appended text: '{text}'");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to append text: {locator}", e);
        }
    }

    #endregion

    #region Wait Methods

    /// <summary>
    /// Wait for element to be visible
    /// </summary>
    public static IWebElement WaitForElementToBeVisible(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
        catch (WebDriverTimeoutException e)
        {
            throw new Exception($"Element not visible after {timeoutInSeconds}s: {locator}", e);
        }
    }

    /// <summary>
    /// Wait for element to be interactable (clickable)
    /// </summary>
    public static IWebElement WaitForElementToBeInteractable(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }
        catch (WebDriverTimeoutException e)
        {
            throw new Exception($"Element not interactable after {timeoutInSeconds}s: {locator}", e);
        }
    }

    /// <summary>
    /// Wait for element to disappear
    /// </summary>
    public static void WaitForElementToDisappear(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
            Console.WriteLine($"[INFO] Element disappeared: {locator}");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine($"[WARN] Element still visible after {timeoutInSeconds}s: {locator}");
        }
    }

    /// <summary>
    /// Wait for loader to disappear
    /// </summary>
    public static void WaitForLoaderToDisappear(IWebDriver driver, By loaderLocator, int timeoutInSeconds = 15)
    {
        try
        {
            var loaders = driver.FindElements(loaderLocator);
            if (loaders.Count > 0 && loaders[0].Displayed)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loaderLocator));
                Console.WriteLine("[INFO] Loader disappeared");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"[WARN] Loader wait issue: {e.Message}");
        }
    }

    /// <summary>
    /// Wait for page to load completely
    /// </summary>
    public static void WaitForPageLoad(IWebDriver driver, int timeoutInSeconds = 30)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            Console.WriteLine("[INFO] Page loaded completely");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[WARN] Page load timeout: {e.Message}");
        }
    }

    #endregion

    #region Element State Methods

    /// <summary>
    /// Check if element is displayed
    /// </summary>
    public static bool IsElementDisplayed(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    /// <summary>
    /// Check if element exists (without waiting)
    /// </summary>
    public static bool ElementExists(IWebDriver driver, By locator)
    {
        return driver.FindElements(locator).Count > 0;
    }

    /// <summary>
    /// Get count of elements matching locator
    /// </summary>
    public static int GetElementCount(IWebDriver driver, By locator)
    {
        var elements = driver.FindElements(locator);
        Console.WriteLine($"[INFO] Found {elements.Count} element(s)");
        return elements.Count;
    }

    #endregion

    #region Text Methods

    /// <summary>
    /// Get text from element
    /// </summary>
    public static string GetTextFromElement(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var element = WaitForElementToBeVisible(driver, locator, timeoutInSeconds);
        string text = element.Text.Trim();
        Console.WriteLine($"[INFO] Got text from element: '{text}'");
        return text;
    }

    /// <summary>
    /// Get attribute value from element
    /// </summary>
    public static string GetAttributeFromElement(IWebDriver driver, By locator, string attributeName, int timeoutInSeconds = 10)
    {
        var element = WaitForElementToBeVisible(driver, locator, timeoutInSeconds);
        string? value = element.GetAttribute(attributeName);
        Console.WriteLine($"[INFO] Got attribute '{attributeName}': '{value}'");
        return value ?? string.Empty;
    }

    /// <summary>
    /// Get 'value' attribute from input element
    /// </summary>
    public static string GetInputValue(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        return GetAttributeFromElement(driver, locator, "value", timeoutInSeconds);
    }

    /// <summary>
    /// Get text from multiple elements
    /// </summary>
    public static List<string> GetTextFromElements(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
            var elements = driver.FindElements(locator);
            var texts = elements.Select(e => e.Text.Trim()).ToList();
            Console.WriteLine($"[INFO] Got text from {texts.Count} elements");
            return texts;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to get text from elements: {locator}", e);
        }
    }

    #endregion

    #region Clear Methods

    /// <summary>
    /// Clear input field
    /// </summary>
    public static void ClearField(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        try
        {
            var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
            element.Clear();
            Console.WriteLine($"[INFO] Field cleared: {locator}");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to clear field: {locator}", e);
        }
    }

    /// <summary>
    /// Clear field using keyboard (Ctrl+A, Delete)
    /// </summary>
    public static void ClearFieldWithKeyboard(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        try
        {
            var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);
            Console.WriteLine($"[INFO] Field cleared with keyboard: {locator}");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to clear field with keyboard: {locator}", e);
        }
    }

    #endregion

    #region Scroll Methods

    /// <summary>
    /// Scroll element into view
    /// </summary>
    public static void ScrollIntoView(IWebDriver driver, By locator)
    {
        try
        {
            var element = driver.FindElement(locator);
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
            Sleep(500);
            Console.WriteLine($"[INFO] Scrolled to element: {locator}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[WARN] Could not scroll to element: {e.Message}");
        }
    }

    #endregion

    #region Dropdown Methods

    /// <summary>
    /// Select dropdown option by visible text
    /// </summary>
    public static void SelectByVisibleText(IWebDriver driver, By locator, string text, int timeoutInSeconds = 10)
    {
        try
        {
            var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
            var dropdown = new SelectElement(element);
            dropdown.SelectByText(text);
            Console.WriteLine($"[INFO] Selected dropdown option by text: '{text}'");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to select dropdown option by text: {text}", e);
        }
    }

    /// <summary>
    /// Select dropdown option by index (0-based)
    /// </summary>
    public static void SelectByIndex(IWebDriver driver, By locator, int index, int timeoutInSeconds = 10)
    {
        try
        {
            var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
            var dropdown = new SelectElement(element);
            dropdown.SelectByIndex(index);
            Console.WriteLine($"[INFO] Selected dropdown option by index: {index}");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to select dropdown option by index: {index}", e);
        }
    }

    /// <summary>
    /// Select dropdown option by value attribute
    /// </summary>
    public static void SelectByValue(IWebDriver driver, By locator, string value, int timeoutInSeconds = 10)
    {
        try
        {
            var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
            var dropdown = new SelectElement(element);
            dropdown.SelectByValue(value);
            Console.WriteLine($"[INFO] Selected dropdown option by value: '{value}'");
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to select dropdown option by value: {value}", e);
        }
    }

    /// <summary>
    /// Get selected option text from dropdown
    /// </summary>
    public static string GetSelectedOptionText(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var element = WaitForElementToBeVisible(driver, locator, timeoutInSeconds);
        var dropdown = new SelectElement(element);
        string selectedText = dropdown.SelectedOption.Text.Trim();
        Console.WriteLine($"[INFO] Selected option: '{selectedText}'");
        return selectedText;
    }

    #endregion

    #region Checkbox and Radio Methods

    /// <summary>
    /// Check a checkbox (if not already checked)
    /// </summary>
    public static void CheckCheckbox(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var checkbox = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
        if (!checkbox.Selected)
        {
            checkbox.Click();
            Console.WriteLine($"[INFO] Checkbox checked: {locator}");
        }
        else
        {
            Console.WriteLine($"[INFO] Checkbox already checked: {locator}");
        }
    }

    /// <summary>
    /// Uncheck a checkbox (if checked)
    /// </summary>
    public static void UncheckCheckbox(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var checkbox = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
        if (checkbox.Selected)
        {
            checkbox.Click();
            Console.WriteLine($"[INFO] Checkbox unchecked: {locator}");
        }
        else
        {
            Console.WriteLine($"[INFO] Checkbox already unchecked: {locator}");
        }
    }

    #endregion

    #region Key Press Methods

    /// <summary>
    /// Press Enter key on element
    /// </summary>
    public static void PressEnter(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
        element.SendKeys(Keys.Enter);
        Console.WriteLine("[INFO] Pressed Enter key");
    }

    /// <summary>
    /// Press Tab key on element
    /// </summary>
    public static void PressTab(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
        element.SendKeys(Keys.Tab);
        Console.WriteLine("[INFO] Pressed Tab key");
    }

    #endregion

    #region Actions Methods

    /// <summary>
    /// Hover over element
    /// </summary>
    public static void HoverOverElement(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var element = WaitForElementToBeVisible(driver, locator, timeoutInSeconds);
        var actions = new Actions(driver);
        actions.MoveToElement(element).Perform();
        Console.WriteLine($"[INFO] Hovered over element: {locator}");
    }

    /// <summary>
    /// Double click on element
    /// </summary>
    public static void DoubleClick(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
        var actions = new Actions(driver);
        actions.DoubleClick(element).Perform();
        Console.WriteLine($"[INFO] Double clicked element: {locator}");
    }

    /// <summary>
    /// Right click on element
    /// </summary>
    public static void RightClick(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var element = WaitForElementToBeInteractable(driver, locator, timeoutInSeconds);
        var actions = new Actions(driver);
        actions.ContextClick(element).Perform();
        Console.WriteLine($"[INFO] Right clicked element: {locator}");
    }

    #endregion

    #region Alert Methods

    /// <summary>
    /// Accept alert
    /// </summary>
    public static void AcceptAlert(IWebDriver driver, int timeoutInSeconds = 5)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.AlertIsPresent());
            driver.SwitchTo().Alert().Accept();
            Console.WriteLine("[INFO] Alert accepted");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[WARN] No alert present or could not accept: {e.Message}");
        }
    }

    /// <summary>
    /// Dismiss alert
    /// </summary>
    public static void DismissAlert(IWebDriver driver, int timeoutInSeconds = 5)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.AlertIsPresent());
            driver.SwitchTo().Alert().Dismiss();
            Console.WriteLine("[INFO] Alert dismissed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[WARN] No alert present or could not dismiss: {e.Message}");
        }
    }

    /// <summary>
    /// Get alert text
    /// </summary>
    public static string GetAlertText(IWebDriver driver, int timeoutInSeconds = 5)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.Until(ExpectedConditions.AlertIsPresent());
        string alertText = driver.SwitchTo().Alert().Text;
        Console.WriteLine($"[INFO] Alert text: {alertText}");
        return alertText;
    }

    #endregion

    #region Frame Methods

    /// <summary>
    /// Switch to frame by locator
    /// </summary>
    public static void SwitchToFrame(IWebDriver driver, By locator, int timeoutInSeconds = 10)
    {
        var frame = WaitForElementToBeVisible(driver, locator, timeoutInSeconds);
        driver.SwitchTo().Frame(frame);
        Console.WriteLine($"[INFO] Switched to frame: {locator}");
    }

    /// <summary>
    /// Switch to default content (exit frame)
    /// </summary>
    public static void SwitchToDefaultContent(IWebDriver driver)
    {
        driver.SwitchTo().DefaultContent();
        Console.WriteLine("[INFO] Switched to default content");
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Thread sleep helper
    /// </summary>
    public static void Sleep(int milliseconds)
    {
        Thread.Sleep(milliseconds);
    }

    #endregion
}


