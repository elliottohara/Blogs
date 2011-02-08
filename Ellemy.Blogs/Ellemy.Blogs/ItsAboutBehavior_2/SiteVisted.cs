using System;
using Ellemy.CQRS.Event;

namespace Ellemy.Blogs.ItsAboutBehavior_2
{
    public class SiteVisted : IDomainEvent
    {
        public string SessionId { get; set; }
        public DateTime When { get; set; }

        public SiteVisted(string sessionId,DateTime when)
        {
            SessionId = sessionId;
            When = when;
        }
    }
}