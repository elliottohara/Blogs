using System;
using Ellemy.CQRS.Event;
using NUnit.Framework;

namespace Ellemy.Blogs.ItsAboutBehavior
{
    [TestFixture]
    public class Visitor_tests
    {
        [Test]
        public void the_last_visit_is_recorded()
        {
            var when = DateTime.Now;
            _visitor.Visit(when);

            Assert.AreEqual(when, _visitor._lastVisit);
        }
        [Test]
        public void a_new_visit_for_the_day_raises_a_UniqueVisitorAdded()
        {
            //Arrange
            var yesterday = DateTime.Now.AddDays(-1);
            _visitor.Visit(yesterday);
            var eventWasRaised = false;
            DomainEvents.Register<VisitorForFirstTimeInDay>(@event=> eventWasRaised = true);
            
            //act
            _visitor.Visit();

            //assert
            Assert.IsTrue(eventWasRaised);
        }
        [Test]
        public void the_report_writer_adds_unique_visitor_count()
        {
            var when = DateTime.Now;

            var theEvent = new VisitorForFirstTimeInDay("SomeSessionId", when);
            _reporter.Handle(theEvent);

            var count = _repository.GetByDate(when).UniqueVisitorsCount;
            Assert.AreEqual(1,count);
        }
        [Test]
        public void only_one_visitor_per_day_per_identifier_is_added()
        {
            var eventRaisedCount = 0;
            DomainEvents.Register<VisitorForFirstTimeInDay>(e => eventRaisedCount++);
            var visitor = new Visitor("SomeId");
            var today = DateTime.Today;
            var atNoon = today.AddHours(12);
            var atDinnerTime = today.AddHours(17);
            visitor.Visit(today);
            visitor.Visit(atNoon);
            visitor.Visit(atDinnerTime);

            Assert.AreEqual(1,eventRaisedCount);

           
        }
        private Visitor _visitor;
        private VisitorsByDateRepository _repository;
        private ReportWriter _reporter;

        [SetUp]
        public void Arrange()
        {
            _visitor = new Visitor("SomeSessionId");
            _repository = new VisitorsByDateRepository();
            _reporter = new ReportWriter(_repository);
        }
    }
}