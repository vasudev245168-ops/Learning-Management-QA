using AutomationFrameWork.Drivers;
using AutomationFrameWork.Pages;
using AutomationFrameWork.Utilities;
using AventStack.ExtentReports;
using Microsoft.Extensions.Configuration;
using Reqnroll;

namespace AutomationFrameWork.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly PlaywrightDriver _playwrightDriver;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;

        private static ExtentReports _extentReports;
        private ExtentTest _extentScenario;

        private static string? _testRunException;

        public Hooks(
            PlaywrightDriver playwrightDriver,
            ScenarioContext scenarioContext,
            FeatureContext featureContext)
        {
            _playwrightDriver = playwrightDriver;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _extentReports = ExtentReportManager.GetExtentReport();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _extentReports?.Flush();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            var feature =
                _extentReports.CreateTest(featureContext.FeatureInfo.Title);

            featureContext.Set(feature, "ExtentFeature");
        }

        [BeforeScenario]
        public async Task BeforeScenarioAsync()
        {
            var extentFeature =
                _featureContext.Get<ExtentTest>("ExtentFeature");

            _extentScenario =
                extentFeature.CreateNode(
                    _scenarioContext.ScenarioInfo.Title);

            ExtentTestManager.Test = _extentScenario;

            _scenarioContext["ExtentScenario"] = _extentScenario;

            try
            {
                LoadConfiguration();

                await _playwrightDriver.InitializeAsync();
            }
            catch (Exception ex)
            {
                _testRunException = ex.Message;

                _extentScenario.Fail(
                    $"Scenario setup failed: {ex.Message}");

                throw;
            }
        }

        [AfterScenario]
        public async Task AfterScenarioAsync()
        {
            await _playwrightDriver.CleanupAsync();
        }

        [AfterStep]
        public async Task AfterStep()
        {
            var extentScenario =
                _scenarioContext.Get<ExtentTest>("ExtentScenario");

            var stepText =
                _scenarioContext.StepContext.StepInfo.Text;

            if (_scenarioContext.TestError == null)
            {
                extentScenario.Pass(stepText);
            }
            else
            {
                try
                {
                    var screenshot =
                        await _playwrightDriver.Page.ScreenshotAsync(
                            new() { FullPage = true });

                    var base64 =
                        Convert.ToBase64String(screenshot);

                    extentScenario.Fail(
                        $"{stepText}: {_scenarioContext.TestError.Message}",
                        MediaEntityBuilder
                            .CreateScreenCaptureFromBase64String(base64)
                            .Build());
                }
                catch (Exception ex)
                {
                    extentScenario.Fail(
                        $"{stepText}: {_scenarioContext.TestError.Message}. " +
                        $"Screenshot failed: {ex.Message}");
                }
            }
        }

        private void LoadConfiguration()
        {
            var projectRoot = CommonActionsPage.GetProjectRoot();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectRoot)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var configReader = new ConfigReader();

            configuration.Bind(configReader);

            _scenarioContext["ConfigReader"] = configReader;
        }
    }
}