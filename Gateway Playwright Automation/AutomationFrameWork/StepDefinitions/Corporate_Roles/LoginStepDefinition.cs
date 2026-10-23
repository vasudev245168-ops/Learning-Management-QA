using AutomationFrameWork.Drivers;
using AutomationFrameWork.PageActions.Corporate_Roles;
using AutomationFrameWork.Utilities;
using Reqnroll;

namespace AutomationFrameWork.StepDefinitions.Corporate_Roles
{
    [Binding]
    public class LoginStepDefinitions
    {
        private readonly LoginPages _loginPages;
        private readonly ScenarioContext _scenarioContext;
        private readonly ConfigReader _configReader;

        public LoginStepDefinitions(
            PlaywrightDriver playwrightDriver,
            ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _configReader =
                _scenarioContext.Get<ConfigReader>("ConfigReader");

            // Get the Page initialized by Hooks/PlaywrightDriver.
            var page = playwrightDriver.Page;

            // Create TestContext manually; do not ask Reqnroll
            // to resolve IPage inside TestContext.
            var testContext = new TestContext(
                page,
                scenarioContext);

            _loginPages = new LoginPages(
                testContext,
                page);
        }
       

        [Given("the user logs in as {string}")]
        public async Task GivenTheUserLogsInAs(string role)
        {
            var roleKey = role.Replace(" ", string.Empty);

            if (!_configReader.Credentials.Roles.TryGetValue(
                roleKey, out var credentials))
            {
                throw new InvalidOperationException(
                    $"Credentials not found for role: {role}");
            }

            await _loginPages.NavigateAsync(_configReader.Urls.QALMS);

            await _loginPages.LoginAsync(
                credentials.Username,
                credentials.Password);
        }


        //[Given("the user logs in as Corporate")]
        //public async Task GivenTheUserLogsInAsCorporate()
        //{
        //    var credentials =
        //        _configReader.Credentials.Roles["Corporate"];

        //    await _loginPages.NavigateAsync(
        //        _configReader.Urls.QALMS);

        //    await _loginPages.LoginAsync(
        //        credentials.Username,
        //        credentials.Password);
        //}
        [When("the user logs out")]
        public async Task  WhenTheUserLogsOut()
        {
            await _loginPages.LogoutAsync();
        }


    }
}
