using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectInsights.Application.Entities
{
    public class FilterEntityModel
    {
        public string TitleName { get; set; }
        public string State { get; set; }
        public string Tag { get; set; }
        public bool IsPublished { get; set; }
    }
    public class ReportFilter
    {
        public string ReportDate { get; set; }
    }
}
