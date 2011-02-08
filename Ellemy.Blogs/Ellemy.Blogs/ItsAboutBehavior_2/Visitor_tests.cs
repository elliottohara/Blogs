using System;
using Ellemy.Blogs.ItsAboutBehavior;
using Ellemy.CQRS.Event;
using NUnit.Framework;

namespace Ellemy.Blogs.ItsAboutBehavior_2
{
    [TestFixture]
    public class Visitor_tests
    {
        private Visitor _visitor;
        private VisitorsPerDayRepository _repository;
        private VisitorsPerDayRepository _visitorsByDateRepository;
        private VisitorsPerDayReportWriter _reporter;
        private SiteVisitRepository _siteVisitRepository;
        private VisitRecorderService _visitRecorder;

        [Test]
        public void the_last_visit_is_recorded()
        {
            var when = DateTime.Now;
            _visitor.Visit(when);

            Assert.AreEqual(when, _visitor._lastVisit);
        }
        [Test]
        public void a_visit_raises_the_SiteVistedEvent()
        {
            //Arrange
            var when = DateTime.Now.AddDays(-1);
            var sessionId = String.Empty;
            var eventTime = DateTime.MinValue;
            DomainEvents.Register<SiteVisted>(@event=>
                                                  {
                                                      sessionId = @event.SessionId;
                                                      eventTime = @event.When;
                                                  });
            
            //act
            _visitor.Visit(when);

            //assert
            Assert.AreEqual(sessionId,_visitor.WebIdentifier);
            Assert.AreEqual(when,eventTime);

        }
        [Test]
        public void the_reportwriter_records_unique_visitor_for_his_first_visit()
        {
            var sessionId = "THESESSION";
            var reallyEarly = DateTime.Today.AddHours(1);
            var theEvent = new SiteVisted(sessionId, reallyEarly);

            _reporter.Handle(theEvent);

            var visitorsToday = _visitorsByDateRepository.GetReportFor(DateTime.Today).UniqueVisitors;
            Assert.AreEqual(1,visitorsToday);
        }
        [Test]
        public void the_reportwriter_does_not_double_count()
        {
            var sessionId = "THESESSION";
            var reallyEarly = DateTime.Today.AddHours(1);
            var theEvent = new SiteVisted(sessionId, reallyEarly);
            var atNoon = reallyEarly.AddHours(11);
            // An IOC container will locate all handlers, but we know that they'll all 
            // get located so lets manually execute them
            var event1 = new SiteVisted(sessionId, reallyEarly);
            _reporter.Handle(event1);
            _visitRecorder.Handle(event1);
            var event2 = new SiteVisted(sessionId, atNoon);
            _reporter.Handle(event2);
            _visitRecorder.Handle(event2);

            var visitorsToday = _visitorsByDateRepository.GetReportFor(DateTime.Today).UniqueVisitors;
            Assert.AreEqual(1, visitorsToday);
        }
       
        [SetUp]
        public void Arrange()
        {
            _visitor = new Visitor("SomeSessionId");
            _visitorsByDateRepository = new VisitorsPerDayRepository();
            _siteVisitRepository = new SiteVisitRepository();

            _reporter = new VisitorsPerDayReportWriter(_siteVisitRepository,_visitorsByDateRepository);
            _visitRecorder = new VisitRecorderService(_siteVisitRepository);
        }
    }
}