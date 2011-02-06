using System;
using System.Collections.Generic;
using System.Linq;
using Ellemy.CQRS.Event;

namespace Ellemy.Blogs.ItsAboutBehavior
{
    public class ReportWriter : IDomainEventHandler<VisitorForFirstTimeInDay>
    {
        private readonly VisitorsByDateRepository _repository;

        public ReportWriter(VisitorsByDateRepository repository)
        {
            _repository = repository;
        }

        public void Handle(VisitorForFirstTimeInDay @event)
        {
            _repository.GetByDate(@event.When.Date).UniqueVisitorsCount++;
        }
    }
    public class VisitorsByDateRepository
    {
        private readonly List<VisitorsByTimeFrameReadModel> _allVisitors;

        public VisitorsByDateRepository()
        {
            _allVisitors = new List<VisitorsByTimeFrameReadModel>();
        }
        public VisitorsByTimeFrameReadModel GetByDate(DateTime date)
        {
            var dateWeCareAbout = date.Date;
            var item = _allVisitors.FirstOrDefault(v => v.Date == dateWeCareAbout);
            if(item == null)
            {
                item = new VisitorsByTimeFrameReadModel(dateWeCareAbout);
                _allVisitors.Add(item);
            }
            return item;
        }
    }
    public class VisitorsByTimeFrameReadModel
    {
        public VisitorsByTimeFrameReadModel(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; private set; }
        public int UniqueVisitorsCount { get; set; }

    }
}