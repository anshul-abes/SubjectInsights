using System;

namespace SubjectInsights.Application.Entities
{
    public class SubjectInsightsBaseFormBuilderModel : BaseEntityModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public string Tag { get; set; }
        public string SubjectImage { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedByEmail { get; set; }
    }

    public class SubjectInsightsFormBuilderModel: SubjectInsightsBaseFormBuilderModel
    {
        public string SubjectContentForm { get; set; }
    }
}
