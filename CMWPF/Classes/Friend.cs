using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMWPF.Classes
{
    public class Friend
    {
        public Friend() { }
        public Guid friendId { get; set; }
        public string friendPic { get; set; }
        public string friendName { get; set; }
        public string friendLastName { get; set; }
        public int friendAge { get; set; }
        public int friendBornDate { get; set; }
        public string friendJob { get; set; }
        public string friendAdress { get; set; }
        public int friendTel { get; set; }
        public string friendMoreInfo { get; set; }
        public bool friendCanBeEdited { get; set; }
        public DateTime friendCreationDate { get; set; }
    }
}
