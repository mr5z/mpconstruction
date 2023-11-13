using System;

namespace MPConstruction.Models
{
    class Report
    {
        public string Comment { get; set; }
        public DateTime DateTime { get; set; }
        public string SelectedArea { get; set; }
        public string TaskCategory { get; set; }
        public string Tags { get; set; }
        public bool LinkToExistingEvent { get; set; }
        public string SelectedEvent { get; set; }
    }
}
