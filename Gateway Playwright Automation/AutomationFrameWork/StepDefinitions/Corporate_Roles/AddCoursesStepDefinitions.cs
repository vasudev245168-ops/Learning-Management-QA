using AutomationFrameWork.Drivers;
using AutomationFrameWork.PageActions.Corporate_Roles;
using AutomationFrameWork.Utilities;
using Reqnroll;

namespace AutomationFrameWork.StepDefinitions.Corporate_Roles
{
    [Binding]
    public class AddCoursesStepDefinitions
    {
        private readonly TestContext _testContext;
        private readonly ScenarioContext _scenarioContext;
        private readonly AddCoursesPages _launchOfAllApplicationsPages;
        private readonly LoginPages _loginPages;
        private readonly ConfigReader _configReader;

        public AddCoursesStepDefinitions(
            PlaywrightDriver playwrightDriver,
            ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _testContext = new TestContext(
                playwrightDriver.Page,
                scenarioContext);

            _launchOfAllApplicationsPages =
                new AddCoursesPages(
                    _testContext,
                    playwrightDriver.Page,
                    scenarioContext);

            _loginPages = new LoginPages(
                _testContext,
                playwrightDriver.Page);

            _configReader =
                _scenarioContext.Get<ConfigReader>("ConfigReader");
        }

        [When("Corporate user adds a new course")]
        public async Task WhenCorporateUserAddsANewCourse()
        {
            await _launchOfAllApplicationsPages.AddCourseAsync();
        }


        [When("the Corporate user logs out")]
        public async Task WhenTheCorporateUserLogsOut()
        {
            await _loginPages.LogoutAsync();
        }

        [Then("user login with office login")]
        public async Task ThenUserLoginWithOfficeLogin()
        {
            var credentials =
                _configReader.Credentials.Roles["Office"];

            // Corporate logout should have returned to the sign-in screen.
            await _loginPages.LoginAsync(
                credentials.Username,
                credentials.Password);

            Console.WriteLine("** Office login successful");
            await Task.Delay(10000);
        }
    }
}
