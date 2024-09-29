using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium;
using NUnit.Framework;
using System;

namespace NotepadTestsNoPom
{
    [TestFixture]
    public class NotepadTests
    {
        private AndroidDriver _driver;
        private AppiumLocalService _appiumLocalService;

        [OneTimeSetUp]
        public void Setup()
        {
            // Start Appium Local Service
            _appiumLocalService = new AppiumServiceBuilder()
                  .WithIPAddress("127.0.0.1")
                  .UsingPort(4723)
                  .Build();
            _appiumLocalService.Start();

            // Setup Appium options
            var androidOptions = new AppiumOptions
            {
                PlatformName = "Android",
                AutomationName = "UIAutomator2",
                DeviceName = "Pixel_7",
                App = @"apk/Notepad.apk" // Adjust this path as necessary
            };
            androidOptions.AddAdditionalAppiumOption("autoGrantPermissions", true);

            // Use the existing Appium server URL
            _driver = new AndroidDriver(new Uri("http://127.0.0.1:4723"), androidOptions);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Handle tutorial skipping
            try
            {
                var skipTutorial = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/btn_start_skip"));
                skipTutorial.Click();
            }
            catch (NoSuchElementException)
            {
                // Tutorial skip button not found, continue with setup
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _appiumLocalService.Dispose(); // Ensure the Appium service is stopped
        }
        
        [Test, Order(1)]
        public void Test_CreateNote()

        {
            var addNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/main_btn1"));
            addNote.Click();
            var createTextNote = _driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"Text\")"));
            createTextNote.Click();

            var writeNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_note"));
            writeNote.SendKeys("Test_1");

            var back = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/back_btn"));
            back.Click();
            back.Click();

            var note = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/title"));

            Assert.That(note, Is.Not.Null, "The note was not created successfully.");

            Assert.That(note.Text, Is.EqualTo("Test_1"), "The note content does not match.");
        }

        [Test,Order(2)]
        public void Test_EditNote()
        {
            var addNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/main_btn1"));
            addNote.Click();

            var createTextNote = _driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"Text\")"));
            createTextNote.Click();

            var writeNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_note"));
            writeNote.SendKeys("Test_2");

            var backButton = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/back_btn"));
            backButton.Click();
            backButton.Click();

            var note = _driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"Test_2\")"));
            note.Click();

            var editButton = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_btn"));
            editButton.Click();

            var editNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_note"));
            editNote.Click();
            editNote.Clear();
            editNote.SendKeys("Edited");

            var back = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/back_btn"));
            back.Click();
            back.Click();

            var editedNote = _driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"Edited\")"));


            // Assertion: Verify the note was edited by checking the updated content
            Assert.That(editedNote.Text, Is.EqualTo("Edited"), "The note content does not match.");
        }

        [Test,Order(3)]
        public void Test_DeleteNote()
        {
            var addNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/main_btn1"));
            addNote.Click();

            var createTextNote = _driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"Text\")"));
            createTextNote.Click();
            var writeNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_note"));
            writeNote.SendKeys("Note for Delete");

            var backButton = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/back_btn"));
            backButton.Click();

            var menu = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/menu_btn"));
            menu.Click();

            var deleteOption = _driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"Delete\")"));
            deleteOption.Click();

            var ok = _driver.FindElement(MobileBy.Id("android:id/button1"));
            ok.Click();

            var deletedNote = _driver.FindElements(By.XPath("//android.widget.TextView[@text='Note for Delete']"));
            Assert.That(deletedNote, Is.Empty, "The note was not deleted successfully.");

        }
    }
}
