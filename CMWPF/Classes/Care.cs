using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMWPF.Classes
{
    public class Care
    {
        public Care() { }
        public int careId { get; set; }
        public string carePic1 { get; set; }
        public string carePic2 { get; set; }
        public string careName { get; set; }
        public string careFunction { get; set; }
        public string carePrescriptor { get; set; }
        public string carePoso { get; set; }
        public string careDuring { get; set; }
        public string careEndDate { get; set; }
        public string careSecondaryEffect { get; set; }
        public string careComplementaryInfo { get; set; }
    }
}
