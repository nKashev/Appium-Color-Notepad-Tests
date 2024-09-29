using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Appium.Service;

namespace NotepadTestsPom
{
    [TestFixture]
    public class NotepadTests
    {
        private AndroidDriver<AndroidElement> _driver;
        private NotepadPage _notepadPage;
        private AppiumLocalService _appiumLocalService;

        [OneTimeSetUp]
        public void Setup()
        {
            try
            {
                // Start the Appium service
                _appiumLocalService = new AppiumServiceBuilder()
                    .WithIPAddress("127.0.0.1")
                    .UsingPort(4723)
                    .Build();
                _appiumLocalService.Start();

                // Setup Android options
                var androidOptions = new AppiumOptions
                {
                    PlatformName = "Android",
                    AutomationName = "UIAutomator2",
                    DeviceName = "Pixel_7",
                    App = @"apk/Notepad.apk"
                };
                androidOptions.AddAdditionalAppiumOption("autoGrantPermissions", true);

                // Initialize the driver
                _driver = new AndroidDriver<AndroidElement>(new Uri("http://127.0.0.1:4723"), androidOptions);
                _notepadPage = new NotepadPage(_driver);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                _notepadPage.SkipTutorial(); // Adjust this according to your app flow
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during setup: {ex.Message}");
                throw; // Rethrow to fail the test if setup fails
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            try
            {
                _driver?.Quit(); // Quit the driver
                _appiumLocalService?.Dispose(); // Dispose the service
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during teardown: {ex.Message}");
            }
        }

        [Test, Order(1)]
        public void Test_CreateNote()
        {
            _notepadPage.AddNote();
            _notepadPage.CreateTextNote();
            _notepadPage.WriteNoteContent("Test_1");
            _notepadPage.ClickBackButton();
            _notepadPage.ClickBackButton();

            var note = _notepadPage.NoteTitle("Test_1");

            Assert.That(note, Is.Not.Null, "The note was not created successfully.");
            Assert.That(note.Text, Is.EqualTo("Test_1"), "The note content does not match.");
        }

        [Test, Order(2)]
        public void Test_EditNote()
        {
            _notepadPage.AddNote();
            _notepadPage.CreateTextNote();
            _notepadPage.WriteNoteContent("Test_2");
            _notepadPage.ClickBackButton();
            _notepadPage.ClickBackButton();

            var note = _notepadPage.NoteTitle("Test_2");
            note.Click();

            var editButton = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_btn"));
            editButton.Click();

            var editNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_note"));
            editNote.Click();
            editNote.Clear();
            editNote.SendKeys("Edited");

            _notepadPage.ClickBackButton();
            _notepadPage.ClickBackButton();

            var editedNote = _notepadPage.NoteTitle("Edited");

            Assert.That(editedNote.Text, Is.EqualTo("Edited"), "The note content does not match.");
        }

        [Test, Order(3)]
        public void Test_DeleteNote()
        {
            _notepadPage.AddNote();
            _notepadPage.CreateTextNote();
            _notepadPage.WriteNoteContent("Note for Delete");
            _notepadPage.ClickBackButton();

            _notepadPage.OpenMenu();
            _notepadPage.ClickDeleteOption();
            _notepadPage.ConfirmDelete();

            var deletedNote = _driver.FindElements(By.XPath("//android.widget.TextView[@text='Note for Delete']"));
            Assert.That(deletedNote, Is.Empty, "The note was not deleted successfully.");
        }
    }
}
