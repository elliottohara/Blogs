using Ellemy.CQRS.Event;

namespace Ellemy.Blogs.ItsAboutBehavior_2
{
    public class VisitRecorderService : IDomainEventHandler<SiteVisted>
    {
        private readonly SiteVisitRepository _siteVisitRepository;

        public VisitRecorderService(SiteVisitRepository siteVisitRepository)
        {
            _siteVisitRepository = siteVisitRepository;
        }
        public void Handle(SiteVisted @event)
        {
           
            _siteVisitRepository.Add(new SiteVisit(@event.SessionId,@event.When));
        }
    }
}