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
    public class ReadPatient
    {
        private string CheckFile()
        {
            string json;
            string path = @"c:\CM\Patients\" + "1" + ".json";
            json = File.ReadAllText(path);
            JObject jon = new JObject();
            JToken value;
            if (jon.TryGetValue("1", out value))
            {
                string finalValue = (string)value;
            }
            return null;
        }

        public ObservableCollection<Patient> GetAllPatients()
        {
            ObservableCollection<Patient> patientcollection = new ObservableCollection<Patient>();

            string uriPath = "file:///C:/CM/Patients/";
            string localPath = new Uri(uriPath).LocalPath;

            bool exists =  Directory.Exists(localPath);

            if (exists)
            {
                List<string> lstFileNames = new List<string>(Directory.EnumerateFiles(localPath, "*.json"));

                foreach (string fileName in lstFileNames)
                {
                    string json;
                    json = File.ReadAllText(fileName);
                    var objects = JArray.Parse(json);
                    foreach (JObject root in objects)
                    {
                        if (root.HasValues)
                        {
                            Patient patient = new Patient();
                            patient.patientId = (int)root["patientId"];
                            patient.patientPrint = (string)root["patientPrint"];
                            patient.patientPic = (string)root["patientPic"];
                            patient.patientName = (string)root["patientName"];
                            patient.patientLastName = (string)root["patientLastName"];
                            patient.patientAge = (string)root["patientAge"];
                            patient.patientBornDate = Convert.ToDateTime((string)root["patientBornDate"]);
                            patient.patientBornWhere = (string)root["patientBornWhere"];
                            patient.patientAdress = (string)root["patientAdress"];
                            patient.patientTel = (string)root["patientTel"];
                            patient.patientMail = (string)root["patientMail"];
                            patient.patientMoreInfo = (string)root["patientMoreInfo"];
                            patient.patientDisease = (string)root["patientDisease"];
                            patient.patientDifficulty = (string)root["patientDifficulty"];
                            patient.patientCreationDate = (DateTime)root["patientCreationDate"];
                            patient.patientLastModificationDate = (DateTime)root["patientLastModificationDate"];
                            patientcollection.Add(patient);
                        }
                    }
                }
            }
                return patientcollection;

        }

        public ObservableCollection<Patient> GetOnePatient(int patientid)
        {
            ObservableCollection<Patient> patientcollection = new ObservableCollection<Patient>();

            string path = @"c:\CM\Patients\" + patientid + ".json";
            string json;
            json = File.ReadAllText(path);
            var objects = JArray.Parse(json);
            foreach (JObject root in objects)
            {
                var id = (int)root["patientId"];
                if (patientid == id)
                {
                    if (root.HasValues)
                    {
                        Patient patient = new Patient();
                        patient.patientId = (int)root["patientId"];
                        patient.patientPrint = (string)root["patientPrint"];
                        patient.patientPic = (string)root["patientPic"];
                        patient.patientName = (string)root["patientName"];
                        patient.patientLastName = (string)root["patientLastName"];
                        patient.patientAge = (string)root["patientAge"];
                        patient.patientBornDate = Convert.ToDateTime((string)root["patientBornDate"]);
                        patient.patientBornWhere = (string)root["patientBornWhere"];
                        patient.patientAdress = (string)root["patientAdress"];
                        patient.patientTel = (string)root["patientTel"];
                        patient.patientMail = (string)root["patientMail"];
                        patient.patientMoreInfo = (string)root["patientMoreInfo"];
                        patient.patientDisease = (string)root["patientDisease"];
                        patient.patientDifficulty = (string)root["patientDifficulty"];
                        patient.patientCreationDate = (DateTime)root["patientCreationDate"];
                        patient.patientLastModificationDate = (DateTime)root["patientLastModificationDate"];
                        patientcollection.Add(patient);
                    }
                }
            }
            return patientcollection;
        }
    }
}