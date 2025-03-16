using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Event
{
    public class ContactAddedEvent
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
    }
}
