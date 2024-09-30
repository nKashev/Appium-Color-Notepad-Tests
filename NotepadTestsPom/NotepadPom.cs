using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers; // For ExpectedConditions

namespace NotepadTestsPom
{
    public class NotepadPage
    {
        private readonly AndroidDriver<AppiumElement> _driver; // Use AppiumElement in Appium 5.x
        private readonly WebDriverWait _wait;

        public NotepadPage(AndroidDriver<AppiumElement> driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); // Wait for up to 10 seconds for elements
        }

        // Define elements
        public IWebElement SkipTutorialButton => _wait.Until(ExpectedConditions.ElementToBeClickable(
            MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/btn_start_skip")));
        
        public IWebElement AddNoteButton => _wait.Until(ExpectedConditions.ElementToBeClickable(
            MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/main_btn1")));
        
        public IWebElement CreateTextNoteOption => _wait.Until(ExpectedConditions.ElementToBeClickable(
            MobileBy.AndroidUIAutomator("new UiSelector().text(\"Text\")")));
        
        public IWebElement WriteNoteField => _wait.Until(ExpectedConditions.ElementIsVisible(
            MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_note")));
        
        public IWebElement BackButton => _wait.Until(ExpectedConditions.ElementToBeClickable(
            MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/back_btn")));
        
        public IWebElement NoteTitle(string title) => _wait.Until(ExpectedConditions.ElementIsVisible(
            By.XPath($"//android.widget.TextView[@resource-id='com.socialnmobile.dictapps.notepad.color.note:id/title' and @text='{title}']")));
        
        public IWebElement MenuButton => _wait.Until(ExpectedConditions.ElementToBeClickable(
            MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/menu_btn")));
        
        public IWebElement DeleteOption => _wait.Until(ExpectedConditions.ElementToBeClickable(
            MobileBy.AndroidUIAutomator("new UiSelector().text(\"Delete\")")));
        
        public IWebElement ConfirmDeleteButton => _wait.Until(ExpectedConditions.ElementToBeClickable(
            MobileBy.Id("android:id/button1")));

        // Define actions
        public void SkipTutorial()
        {
            try
            {
                SkipTutorialButton.Click();
            }
            catch (NoSuchElementException)
            {
                // Tutorial skip button not found, continue with setup
            }
        }

        public void AddNote() => AddNoteButton.Click();
        public void CreateTextNote() => CreateTextNoteOption.Click();
        public void WriteNoteContent(string content) => WriteNoteField.SendKeys(content);
        public void ClickBackButton() => BackButton.Click();
        public void OpenMenu() => MenuButton.Click();
        public void ClickDeleteOption() => DeleteOption.Click();
        public void ConfirmDelete() => ConfirmDeleteButton.Click();
    }
}
