using System;
using System.Collections.Generic;
using Ellemy.CQRS.Event;

namespace Ellemy.Blogs.ItsAboutBehavior
{
    public class TimeFrameRepository
    {
        
    }
    public class TimeFrame
    {
        private readonly List<Visitor> _visitors;
        private readonly List<Visit> _visits;
        public Guid Id { get; set; }

        public TimeFrame(Guid id)
        {
            Id = id;
            _visitors = new List<Visitor>();
            _visits = new List<Visit>();
        }
        public void AddVisitor(string webIdentifier)
        {
            var theVisitor = new Visitor(webIdentifier);
            _visits.Add(new Visit(DateTime.Now,theVisitor));
            if (_visitors.Contains(theVisitor)) return;
            _visitors.Add(theVisitor);
            DomainEvents.Raise(new VisitorForFirstTimeInDay(webIdentifier,DateTime.Now));
        }
    }
    public class Visit
    {
        public DateTime When { get; private set; }
        public Visitor Visitor { get; private set; }

        public Visit(DateTime when,Visitor visitor)
        {
            When = when;
            Visitor = visitor;
        }
    }
}