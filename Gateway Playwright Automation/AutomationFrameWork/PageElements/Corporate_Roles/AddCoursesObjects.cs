using Microsoft.Playwright;

namespace AutomationFrameWork.PageElements.Corporate_Roles
{
    public class AddCoursesObjects
    {
        private readonly IPage _page;
        public AddCoursesObjects(IPage page)
        {
            _page = page;
        }
        public ILocator CoursesTab =>
             _page.Locator("//span[text()='Employees']");
    }
}
