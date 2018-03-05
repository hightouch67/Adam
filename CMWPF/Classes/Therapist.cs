using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMWPF.Classes
{
    public class Therapist
    {
        public Therapist() { }
        public int therapistId { get; set; }
        public string therapistPic { get; set; }
        public string therapistName { get; set; }
        public string therapistLastName { get; set; }
        public string therapistFunction { get; set; }
        public string therapistAdress { get; set; }
        public string therapistTel { get; set; }
        public string therapistMail { get; set; }
        public string therapistMoreInfo { get; set; }
        public bool therapistCanBeEdited { get; set; }
        public DateTime therapistCreationDate { get; set; }
        public DateTime therapistLastModificationDate { get; set; }
        public List<string> patientsList { get; set; }
    }
}
