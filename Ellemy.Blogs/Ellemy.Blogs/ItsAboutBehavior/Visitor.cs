using System;
using Ellemy.CQRS.Event;

namespace Ellemy.Blogs.ItsAboutBehavior
{
    public class Visitor
    {
        internal DateTime _lastVisit;

        public Visitor(string identifier)
        {
            WebIdentifier = identifier;
            _lastVisit = DateTime.MinValue;
        }

        public string WebIdentifier { get; private set; }
        public void Visit(DateTime? when = null)
        {
            when = when ?? DateTime.Now;
            if (_lastVisit.Date != DateTime.Today)
                DomainEvents.Raise(new VisitorForFirstTimeInDay(WebIdentifier,when.Value));
            
            _lastVisit = when.Value;

        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Visitor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.WebIdentifier, WebIdentifier);
        }

        public override int GetHashCode()
        {
            return (WebIdentifier != null ? WebIdentifier.GetHashCode() : 0);
        }

        
    }
}