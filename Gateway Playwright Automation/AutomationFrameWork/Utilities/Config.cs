using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AutomationFrameWork.Utilities
{
    public static class Config
    {
        public static string DataFilesPath =>
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "DataFiles",
                "CreateAnnouncementData.xlsx"
            );
    }
}
