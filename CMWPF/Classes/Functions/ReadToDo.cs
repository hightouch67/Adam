using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMWPF.Classes.Functions
{
    public class ReadToDo
    {
        public ObservableCollection<Todo> ReadToDoByPatientId(int patientid)
        {
            string json;
            string path = @"c:\CM\Patients\" + patientid.ToString() + "\\Todo.json";
            ObservableCollection<Todo> todocollection = new ObservableCollection<Todo>();
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
                            Todo todo = new Todo();
                            todo.Subject = (string)root["Subject"];


                            DateTime currentDate = new DateTime();

                            //TODO
                           // //currentDate. = DateTime.Now();
                           //if (todo.EndingDate)
                           // {
                           //     todo.IsPast = true;
                           // }


                            todocollection.Add(todo);
                        }
                    }
                }
            }

            else
            {
                File.Create(path);
            }
            return todocollection;
        }
    }
}
