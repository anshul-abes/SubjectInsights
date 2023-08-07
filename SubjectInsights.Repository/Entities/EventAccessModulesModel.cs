using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectInsights.Application.Entities
{
    public class SubjectInsightsAccessModulesModel : BaseEntityModel
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public int EventAccessId { get; set; }
    }
}
