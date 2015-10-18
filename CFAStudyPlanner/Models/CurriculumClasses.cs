using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace CFAStudyPlanner.Models
{
    public class Reading
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("pages")]
        public int Pages { get; set; }

        [XmlAttribute("completed")]
        public bool Completed { get; set; }
    }

    public class StudySession
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("Readings")]
        [XmlArrayItem("Reading")]
        public ObservableCollection<Reading> Readings { get; set; }
    }

    public class Topic
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("weighting")]
        public double Weighting { get; set; }

        [XmlArray("StudySessions")]
        [XmlArrayItem("StudySession")]
        public ObservableCollection<StudySession> StudySessions { get; set; }
    }

    [XmlRoot("Curriculum")]
    public class Curriculum
    {
        [XmlArray("Topics")]
        [XmlArrayItem("Topic")]
        public ObservableCollection<Topic> Topics { get; set; }

        [XmlAttribute("Date")]
        public string DateString { get; set; } 
    }
}
