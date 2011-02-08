using System.Linq;
using Ellemy.CQRS.Event;

namespace Ellemy.Blogs.ItsAboutBehavior_2
{
    public class VisitorsPerDayReportWriter : IDomainEventHandler<SiteVisted>
    {
        private readonly SiteVisitRepository _repository;
        private readonly VisitorsPerDayRepository _reportRepository;

        public VisitorsPerDayReportWriter(SiteVisitRepository repository, VisitorsPerDayRepository reportRepository)
        {
            _repository = repository;
            _reportRepository = reportRepository;
        }


        public void Handle(SiteVisted @event)
        {
            if (_repository
                .GetWithoutIncluding(@event)
                .Any(s => s.When.Date == @event.When.Date && s.SessionId == @event.SessionId))
            {
                return;
            }

            _reportRepository.GetReportFor(@event.When).UniqueVisitors++;

        }
    }
}