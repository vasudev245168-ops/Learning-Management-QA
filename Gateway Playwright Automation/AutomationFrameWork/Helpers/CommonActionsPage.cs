using DocumentFormat.OpenXml.InkML;
using Microsoft.Playwright;

namespace AutomationFrameWork.Pages
{
    public class CommonActionsPage
    {
        private readonly IPage _page;
        public string ProjectRoot;

        public CommonActionsPage(IPage page)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
            ProjectRoot = GetProjectRoot();
        }

        public async Task WaitAsync(float seconds)
        {
            if (seconds > 0)
            {
                await _page.WaitForTimeoutAsync(seconds * 1000);
            }
        }

        public void LogCurrentPageUrl()
        {
            var currentUrl = _page.Url;
            Console.WriteLine("** Navigated to the URL : " + currentUrl);
        }

        public static string GetProjectRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory != null && !directory.GetFiles("*.csproj").Any())
            {
                directory = directory.Parent;
            }

            if (directory == null)
                throw new DirectoryNotFoundException("Could not find project root (.csproj)");

            return directory.FullName;
        }

        public async Task ClearBrowserCache()
        {
            // Clear storage (localStorage, sessionStorage, IndexedDB)
            await _page.EvaluateAsync(@"() => {
            try {
                localStorage.clear();
                sessionStorage.clear();
                
                // Clear IndexedDB databases
                if (window.indexedDB) {
                    window.indexedDB.databases().then(databases => {
                        databases.forEach(db => {
                            window.indexedDB.deleteDatabase(db.name);
                        });
                    });
                }
                
                // Clear cache storage if available
                if ('caches' in window) {
                    caches.keys().then(keyList => {
                        return Promise.all(keyList.map(key => {
                            return caches.delete(key);
                        }));
                    });
                }
            } catch(e) {
                console.error('Error clearing storage:', e);
            }
        }");

            // Optional: Clear service workers
            await _page.EvaluateAsync(@"() => {
            if ('serviceWorker' in navigator) {
                navigator.serviceWorker.getRegistrations().then(registrations => {
                    registrations.forEach(registration => {
                        registration.unregister();
                    });
                });
            }
        }");
        }

    }
}
