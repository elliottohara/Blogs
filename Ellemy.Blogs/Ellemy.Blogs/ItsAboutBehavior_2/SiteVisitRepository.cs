using System.Collections.Generic;
using System.Linq;

namespace Ellemy.Blogs.ItsAboutBehavior_2
{
    public class SiteVisitRepository
    {
        private readonly List<SiteVisit> _siteVisits;

        public SiteVisitRepository()
        {
            _siteVisits = new List<SiteVisit>();
        }
        public IQueryable<SiteVisit> Get()
        {
            return _siteVisits.AsQueryable();
        }
        public IQueryable<SiteVisit> GetWithoutIncluding(SiteVisted @event)
        {
            return _siteVisits.Where(sv => sv.SessionId != @event.SessionId || sv.When != @event.When).AsQueryable();
        }
        public void Add(SiteVisit visit)
        {
            _siteVisits.Add(visit);
        }
    }
}