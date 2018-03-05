using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfScheduler;

namespace CMWPF.Classes.Functions
{
    public class ReadEvent
    {
        public ObservableCollection<Event> ReadEventByPatientId(int patientid)
        {
            string json;
            string path = @"c:\CM\Patients\" + patientid.ToString() + "\\Events.json";
            ObservableCollection<Event> eventcollection = new ObservableCollection<Event>();
            bool exists = File.Exists(path);

            if (exists)
            {
                json = File.ReadAllText(path);
                if (json != "")
                {
                    var objects = JArray.Parse(json);
                    foreach (JObject root in objects)
                    {
                        if (root.HasValues)
                        {
                            Event ev = new Event();
                            ev.Subject = (string)root["Subject"];
                            ev.AllDay = (bool)root["AllDay"];
                            ev.Description = (string)root["Description"];
                            ev.Start = (DateTime)root["Start"];
                            ev.End = (DateTime)root["End"];
                            ev.IsDone = (bool)root["IsDone"];
                            ev.Commentary = (string)root["Commentary"];
                            var converter = new BrushConverter();
                            var brush = (Brush)converter.ConvertFromString((string)root["Color"]);
                            ev.Color = brush;
                            eventcollection.Add(ev);
                        }
                    }
                }
            }

            else
            {
                File.Create(path);
            }
            return eventcollection;
        }
    }
}
