using AutomationFrameWork.PageElements.Corporate_Roles;
using AutomationFrameWork.Pages;
using AutomationFrameWork.Utilities;
using AventStack.ExtentReports;
using Microsoft.Playwright;
using AutomationFrameWork.Helpers;
using AutomationFrameWork.PageActions.Office_Role;
namespace AutomationFrameWork.PageActions.Corporate_Roles
{
    public class AddCoursesPages : AddCoursesObjects
    {
        private readonly TestContext _context;
        private readonly IPage _page;
        private readonly ScenarioContext _scenarioContext;
        private readonly  CommonActionsPage _commonActionsPage;
        private readonly AddCourse _addCourse;

        public AddCoursesPages(TestContext context, IPage page, ScenarioContext scenarioContext) : base(page)
        {
            _context = context;
            _page = page;
            _addCourse = new AddCourse(_page);
            _scenarioContext = scenarioContext;
            _commonActionsPage = new CommonActionsPage(_page);
        }
        private Dictionary<string, string> GetTestData()
        {
            var tagName = _context.ScenarioContext.ScenarioInfo.Tags
                            .First(t => !IsFrameworkTag(t));

            return ExcelHelper.ReadExcel(
                Config.DataFilesPath,
                "MyApplications",
                tagName
            );
        }
        private bool IsFrameworkTag(string tag)
        {
            var frameworkTags = new[] { "smoke", "regression", "sanity" };
            return frameworkTags.Contains(tag.ToLower());
        }
        public async Task AddCourseAsync()
        {
            await _addCourse.AddACourse();
        }
        //public async Task AddCourseAsync()
        //{
        //    await CoursesTab.WaitForAsync(new()
        //    {
        //        State = WaitForSelectorState.Visible,
        //        Timeout = 30000
        //    });

        //    await CoursesTab.ClickAsync();
        //}
        }
    }
