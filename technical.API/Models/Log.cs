using System;
using System.Collections.Generic;

namespace technical.API.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public int OrdId { get; set; }
        public int CotId { get; set; }
        public int LnkId { get; set; }
        public int CId { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public DateTime Stamp { get; set; }
    }
}
