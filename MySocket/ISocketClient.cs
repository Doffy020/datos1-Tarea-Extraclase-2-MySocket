using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocket
{
    public interface ISocketClient
    {
        void SendMessage(string message);
    }
}
