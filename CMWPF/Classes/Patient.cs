using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMWPF.Classes
{
    public class Patient
    {
        public Patient() { }
        public int patientId { get; set; }
        public string patientPic { get; set; }
        public string patientName { get; set; }
        public string patientLastName { get; set; }
        public string patientGender { get; set; }
        public string patientAge { get; set; }
        public DateTime patientBornDate { get; set; }
        public string patientBornWhere { get; set; }
        public string patientAdress { get; set; }
        public string patientTel { get; set; }
        public string patientMail { get; set; }
        public string patientMoreInfo { get; set; }
        public string patientDisease { get; set; }
        public string patientDifficulty { get; set; }
        public bool patientCanBeEdited { get; set; }
        public string patientPrint { get; set; }
        public DateTime patientCreationDate { get; set; }
        public DateTime patientLastModificationDate { get; set; }
    }
}
