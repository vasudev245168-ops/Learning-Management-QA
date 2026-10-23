using AutomationFrameWork.Pages;
using AutomationFrameWork.Utilities;
using Microsoft.Playwright;

namespace AutomationFrameWork.Drivers
{
    public class PlaywrightDriver
    {
        public IPlaywright Playwright { get; private set; }
        public IBrowser Browser { get; private set; }
        public IBrowserContext BrowserContext { get; private set; }
        public IPage Page { get; private set; }

        private readonly ScenarioContext _scenarioContext;

        public PlaywrightDriver(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        public async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            var config = _scenarioContext.Get<ConfigReader>("ConfigReader");

            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = config.AutomationSettings.Headless,
                SlowMo = 1000,
                Args = new[] { "--start-maximized", "--disable-cache" }
            };

            string browserName = config.AutomationSettings.Browser?.ToLower();

            // -------- Browser Validation & Launch --------
            if (!string.IsNullOrWhiteSpace(browserName))
            {
                switch (browserName)
                {
                    case "chrome":
                        //launchOptions.Channel = "chrome";
                        launchOptions.ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                        Browser = await Playwright.Chromium.LaunchAsync(launchOptions);
                        break;
                    case "msedge":
                        launchOptions.Channel = "msedge";
                        Browser = await Playwright.Chromium.LaunchAsync(launchOptions);
                        break;
                    case "chromium":
                        Browser = await Playwright.Chromium.LaunchAsync(launchOptions);
                        break;

                    case "firefox":
                        Browser = await Playwright.Firefox.LaunchAsync(launchOptions);
                        break;

                    case "webkit":
                        Browser = await Playwright.Webkit.LaunchAsync(launchOptions);
                        break;

                    default:
                        throw new ArgumentException(
                            $"Unsupported browser: {config.AutomationSettings.Browser}");
                }

            }

           
            var contextOptions = new BrowserNewContextOptions
            {
                ViewportSize = ViewportSize.NoViewport
            };

            // ⭐ If auth.json exists, load it
            if (File.Exists("auth.json"))
            {
                contextOptions.StorageStatePath = "auth.json";
            }

            BrowserContext = await Browser.NewContextAsync(contextOptions);
            Page = await BrowserContext.NewPageAsync();


            /* Code to start playwright tracking (reporting) -- Do not Uncomment this, implemented this to show that,
             * We can generate a playwright trace file in the Automation Code demo.
             * 
             * --> This generates a separate trace file for each scenario and consumes lot of space on disk.
             *
            //await BrowserContext.Tracing.StartAsync(new TracingStartOptions
            //{
            //    Screenshots = true,
            //    Snapshots = true,
            //    Sources = true
            //});
 
            */

            //await BrowserContext.ClearCookiesAsync();
            //await BrowserContext.ClearPermissionsAsync();

            if (browserName == "chrome" || browserName == "chromium" || browserName == "msedge")
            {
                var tempPage = await BrowserContext.NewPageAsync();
                var session = await BrowserContext.NewCDPSessionAsync(tempPage);

                await session.SendAsync("Network.enable");
                await session.SendAsync("Network.setCacheDisabled", new Dictionary<string, object>
                {
                    ["cacheDisabled"] = true
                });

                await tempPage.CloseAsync();
            }

            BrowserContext.SetDefaultNavigationTimeout(config.AutomationSettings.Timeout);
            BrowserContext.SetDefaultTimeout(config.AutomationSettings.Timeout);

            //Page = await BrowserContext.NewPageAsync();
        }

        public async Task StopTracingAsync(ScenarioContext scenarioContext)
        {
            if (BrowserContext == null)
                return;

            string scenarioName = scenarioContext.ScenarioInfo.Title;

            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                scenarioName = scenarioName.Replace(invalidChar, '_');
            }

            scenarioName = scenarioName.Replace(" ", "_");
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string projectRoot = CommonActionsPage.GetProjectRoot();
            string tracesDirectory = Path.Combine(projectRoot, "Traces");

            Directory.CreateDirectory(tracesDirectory);

            string tracePath = Path.Combine(
                tracesDirectory,
                $"{scenarioName}_{timestamp}.zip"
            );

            if (scenarioContext.TestError != null)
            {
                await BrowserContext.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = tracePath
                });

                Console.WriteLine($"Playwright trace saved at: {Path.GetFullPath(tracePath)}");
            }
            else
            {
                await BrowserContext.Tracing.StopAsync();
            }
        }

        public async Task CleanupAsync()
        {
            if (Page != null)
            {
                await Page.CloseAsync();
            }
            if (BrowserContext != null)
            {
                await BrowserContext.CloseAsync();
            }
            if (Browser != null)
            {
                await Browser.CloseAsync();
            }

            Playwright?.Dispose();
        }

    }
}