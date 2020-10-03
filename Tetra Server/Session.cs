using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tetra_Server
{
    public class Session
    {
        public int UserID { get; set; }
        public Socket ClientSocket { get; set; }
        public byte[] Buffer { get; set; } = new byte[1024];
        public DateTime CreateTime { get; } = DateTime.Now;
        public byte[] FileInfo { get; set; } = new byte[1024];
        public Dictionary<int, FileInformation> files { get; set; } = new Dictionary<int, FileInformation>();
    }
}
