using System;

namespace Ellemy.Blogs.ItsAboutBehavior_2
{
    public class VisitorsPerDayLineItem
    {
        public DateTime Date { get; private set; }
        public int UniqueVisitors { get; set; }

        public VisitorsPerDayLineItem(DateTime date)
        {
            Date = date;
        }
    }
}