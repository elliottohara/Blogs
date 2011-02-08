using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellemy.Blogs.ItsAboutBehavior_2
{
    public class VisitorsPerDayRepository
    {
        private readonly List<VisitorsPerDayLineItem> _items;

        public VisitorsPerDayRepository()
        {
            _items = new List<VisitorsPerDayLineItem>();
        }
        public VisitorsPerDayLineItem GetReportFor(DateTime date)
        {
            var item = _items.FirstOrDefault(li => li.Date.Date == date.Date);
            if(item == null)
            {
                item = new VisitorsPerDayLineItem(date);
                _items.Add(item);
            }
            return item;
        }
    }
}