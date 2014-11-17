using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace WPMote.Connectivity
{
    class Comm_UDP
    {
        #region "Common variables"

        int intPort = 8007;
        Socket objClient;

        #endregion
                
        #region "Class properties"

        public int Port
        {
            get
            {
                return intPort;
            }
            set
            {
                if (value > 0) intPort = value;
            }
        }

        #endregion

        #region "Class constructors"

        public Comm_UDP(int _port)
        {
            objClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        #endregion


        #region "Public methods"

        public void SendBytes(string strServerName, byte[] buffer)
        {
            if (objClient!=null)
            {
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = new DnsEndPoint(strServerName, intPort);

                socketEventArg.SetBuffer(buffer, 0, buffer.Length);
                objClient.SendToAsync(socketEventArg);
            }
        }

        

        #endregion
    }
}
