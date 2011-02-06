using System;
using Ellemy.CQRS.Event;

namespace Ellemy.Blogs.ItsAboutBehavior
{
    public class VisitorForFirstTimeInDay : IDomainEvent
    {
        public string WebIdentifier { get; set; }
        public DateTime When { get; set; }

        public VisitorForFirstTimeInDay(string webIdentifier, DateTime when)
        {
            WebIdentifier = webIdentifier;
            When = when;
        }
    }
}