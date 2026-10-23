using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace AutomationFrameWork.Helpers
{
    public class AddCourse
    {
        private readonly IPage _page;
        public AddCourse(IPage page)
            {
             _page = page;
            }
        //Locators
        public ILocator CoursesTab =>
           _page.Locator("//span[text()='Employees']");
        public async Task AddACourse()
        {
            await CoursesTab.ClickAsync();


        }
    }

}
