using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMWPF.Classes
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public bool IsDone { get; set; }
        public Todo()
        {
            Id = Guid.NewGuid();
        }
        public DateTime CreatedDate { get; set; }
        public DateTime EndingDate { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsPast { get; set; }

    }
}
