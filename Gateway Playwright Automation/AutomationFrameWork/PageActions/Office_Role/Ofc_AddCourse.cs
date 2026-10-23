using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomationFrameWork.Helpers;
using AutomationFrameWork.Pages;
using AutomationFrameWork.Utilities;
using Microsoft.Playwright;

namespace AutomationFrameWork.PageActions.Office_Role
{
    public class Ofc_AddCourse
    {
        private readonly TestContext _context;
        private readonly IPage _page;
        private readonly ScenarioContext _scenarioContext;
        private readonly CommonActionsPage _commonActionsPage;
        private readonly AddCourse _addCourse;

        public Ofc_AddCourse(TestContext context, IPage page, ScenarioContext scenarioContext) 
        {
            _context = context;
            _page = page;
            _addCourse = new AddCourse(_page);
            _scenarioContext = scenarioContext;
            _commonActionsPage = new CommonActionsPage(_page);
        }
       
         public async Task OfficeAddCourse()
        {
            await _addCourse.AddACourse();
        }
    }
}
