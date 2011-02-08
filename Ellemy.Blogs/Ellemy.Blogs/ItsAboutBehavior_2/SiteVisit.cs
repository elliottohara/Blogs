using System;

namespace Ellemy.Blogs.ItsAboutBehavior_2
{
    public class SiteVisit
    {
        public string SessionId { get; set; }
        public DateTime When { get; set; }

        public SiteVisit(string sessionId,DateTime when)
        {
            SessionId = sessionId;
            When = when;
        }
    }
}