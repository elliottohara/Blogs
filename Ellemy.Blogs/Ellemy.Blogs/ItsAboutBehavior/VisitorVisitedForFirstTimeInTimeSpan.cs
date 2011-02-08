using System;
using Ellemy.CQRS.Event;

namespace Ellemy.Blogs.ItsAboutBehavior
{
    public class VisitorVisitedForFirstTimeInTimeSpan : IDomainEvent
    {
        public Guid TimeSpanId { get; set; }
        public string WebIdentifier { get; set; }

        public VisitorVisitedForFirstTimeInTimeSpan(Guid timeSpanId, string webIdentifier)
        {
            TimeSpanId = timeSpanId;
            WebIdentifier = webIdentifier;
        }
    }
}