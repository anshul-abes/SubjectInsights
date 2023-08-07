using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectInsights.Application.Entities
{
    public abstract class BaseEntityModel
    {
        public Int16 IsActive { get; set; }
        public Int16 IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata"));
        public DateTime UpdatedAt { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata"));
    }
}
