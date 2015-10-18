using CFAStudyPlanner.Models;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace CFAStudyPlanner
{
    public class MainViewModel : BindableBase
    {
        private XmlDocument _doc = new XmlDocument();
        private static Curriculum _curriculum;

        public static ObservableCollection<Topic> ProgressData { get { return _curriculum.Topics; } private set { _curriculum.Topics = value; } }
        public string Progress { get { return CalculateProgressNumber(); } }
        public string RemainingMessage{ get { return CalculateProgressMsg(); } }
        public string ExamDate
        {
            get
            { 
                var exam = DateTime.Parse(_curriculum.DateString);
                return exam.ToLongDateString();
            }
        }

        public string TodaysDate
        {
            get
            {
                return DateTime.Today.ToLongDateString();
            }
        }

        public string RemainingDays
        {
            get
            {
                var exam = DateTime.Parse(_curriculum.DateString);
                var today = DateTime.Today;
                return (exam - today).TotalDays.ToString();
            }
        }

        public string RemainingWeeks
        {
            get
            {
                var exam = DateTime.Parse(_curriculum.DateString);
                var today = DateTime.Today;
                return ((int)((exam - today).TotalDays / 7)).ToString();
            }
        }

        public MainViewModel()
        {
            LoadCurriculum();
            //ShowProgress();
        }

        ~MainViewModel()
        {
            SaveProgress();
        }

        public static string CalculateProgressMsg()
        {
            int remSessions = 0, remReadings = 0;
            foreach (var topic in _curriculum.Topics)
            {
                foreach (var session in topic.StudySessions)
                {
                    var readings = ((IEnumerable<Reading>)session.Readings).ToList();
                    remReadings += readings.Count;
                    var completed = readings.FindAll(x => x.Completed).Count;
                    remReadings -= completed;
                    if (completed < readings.Count) ++remSessions;
                }
            }
            
            return string.Format("{0} Sessions, {1} Readings to go!", remSessions, remReadings);
        }
        public static string CalculateProgressNumber()
        {
            double progress = 0;
            foreach (var topic in _curriculum.Topics)
            {
                double weight = topic.Weighting;
                int totalPages = topic.StudySessions.Sum(x => x.Readings.Sum(y => y.Pages));
                foreach (var session in topic.StudySessions)
                {
                    foreach (var reading in session.Readings)
                    {
                        if (reading.Completed) progress += weight * reading.Pages / totalPages;
                    }
                }
            }

            return string.Format("{0:0.0}%", progress);
        }

        private void LoadCurriculum()
        {
            _doc.Load(ConfigurationManager.AppSettings["CurriculumXmlPath"]);
            var serial = new XmlSerializer(typeof(Curriculum));
            using (var reader = XmlReader.Create(ConfigurationManager.AppSettings["CurriculumXmlPath"]))
            {
                _curriculum = (Curriculum)serial.Deserialize(reader);
            }
        }

        private void SaveProgress()
        {
            var serial = new XmlSerializer(typeof(Curriculum));
            var settings = new XmlWriterSettings();
            settings.CloseOutput = true;
            using (var writer = XmlWriter.Create(File.Create(ConfigurationManager.AppSettings["CurriculumXmlPath"]), settings))
            {
                serial.Serialize(writer, _curriculum);
                writer.Close();
            }
        }
    }
}
