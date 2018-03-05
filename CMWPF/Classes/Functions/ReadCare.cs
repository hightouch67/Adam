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
    public class ReadCare
    {
        private string CheckFile()
        {
            string json;
            string path = @"c:\CM\Patients\care.json";
            json = File.ReadAllText(path);
            JObject jon = new JObject();
            JToken value;
            if (jon.TryGetValue("1", out value))
            {
                string finalValue = (string)value;
            }
            return null;
        }

        public ObservableCollection<Care> GetAllCare(int id)
        {
            ObservableCollection<Care> carecollection = new ObservableCollection<Care>();

            string uriPath = "file:///C:/CM/Patients/" + id;
            string localPath = new Uri(uriPath).LocalPath;
            bool exists = Directory.Exists(localPath);

            if (exists)
            {
                List<string> lstFileNames = new List<string>(Directory.EnumerateFiles(localPath, "care.json"));

                foreach (string fileName in lstFileNames)
                {
                    string json;
                    json = File.ReadAllText(fileName);
                    var objects = JArray.Parse(json);
                    foreach (JObject root in objects)
                    {
                        if (root.HasValues)
                        {
                            Care care = new Care();
                            care.careId = (int)root["careId"];
                            care.careName = (string)root["careName"];
                            care.carePic1 = (string)root["carePic1"];
                            care.carePic2 = (string)root["carePic2"];
                            care.careFunction = (string)root["careFunction"];
                            care.carePrescriptor = (string)root["carePrescriptor"];
                            care.carePoso = (string)root["carePoso"];
                            care.careDuring = (string)root["careDuring"];
                            care.careEndDate = (string)root["careEndDate"];
                            care.careSecondaryEffect = (string)root["careSecondaryEffect"];
                            care.careComplementaryInfo = (string)root["careComplementaryInfo"];
                            carecollection.Add(care);
                        }
                    }
                }
            }
            return carecollection;

        }

        public ObservableCollection<Therapist> GetOneTherapist(int therapistid)
        {
            ObservableCollection<Therapist> therapistcollection = new ObservableCollection<Therapist>();

            string path = @"c:\CM\therapists.json";
            string json;
            json = File.ReadAllText(path);
            var objects = JArray.Parse(json);
            foreach (JObject root in objects)
            {
                var id = (int)root["therapistId"];
                if (therapistid == id)
                {
                    if (root.HasValues)
                    {
                        Therapist therapist = new Therapist();
                        therapist.therapistId = (int)root["therapistId"];
                        //therapist.therapistPrint = (string)root["therapistPrint"];
                        therapist.therapistPic = (string)root["therapistPic"];
                        therapist.therapistName = (string)root["therapistName"];
                        therapist.therapistLastName = (string)root["therapistLastName"];
                        therapist.therapistAdress = (string)root["therapistAdress"];
                        therapist.therapistFunction = (string)root["therapistFunction"];
                        therapist.therapistTel = (string)root["therapistTel"];
                        therapist.therapistMail = (string)root["therapistMail"];
                        therapist.therapistMoreInfo = (string)root["therapistMoreInfo"];
                        therapist.therapistCreationDate = (DateTime)root["therapistCreationDate"];
                        therapist.therapistLastModificationDate = (DateTime)root["therapistLastModificationDate"];
                        therapistcollection.Add(therapist);
                    }
                }
            }
            return therapistcollection;
        }
    }
}
