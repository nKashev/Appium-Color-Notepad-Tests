using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;

namespace NotepadTestsPom
{
    [TestFixture]
    public class NotepadTests
    {
        private AndroidDriver _driver;
        private NotepadPage _notepadPage;
        // private AppiumLocalService _appiumLocalService; // Comment this out if Appium server is already running

        [OneTimeSetUp]
        public void Setup()
        {
            /* _appiumLocalService = new AppiumServiceBuilder()
                  .WithIPAddress("127.0.0.1")
                  .UsingPort(4723)
                  .Build();
              _appiumLocalService.Start();*/

            var androidOptions = new AppiumOptions
            {
                PlatformName = "Android",
                AutomationName = "UIAutomator2",
                DeviceName = "Pixel_7",
                App = "apk/Notepad.apk"
            };
            androidOptions.AddAdditionalAppiumOption("autoGrantPermissions", true);

            // Use the existing Appium server URL
            _driver = new AndroidDriver(new Uri("http://127.0.0.1:4723"), androidOptions);
            _notepadPage = new NotepadPage(_driver);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            _notepadPage.SkipTutorial();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver?.Quit();
            //_appiumLocalService.Dispose();// Comment this out if Appium server is already running
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
