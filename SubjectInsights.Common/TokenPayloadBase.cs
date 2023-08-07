using System;

namespace SubjectInsights.Common
{
    public abstract class TokenPayloadBase
    {
        public DateTime Created { get; set; }
        public DateTime Expiration { get; set; }
        public virtual bool Validate()
        {
            return DateTime.UtcNow < Expiration;
        }
    }
}
