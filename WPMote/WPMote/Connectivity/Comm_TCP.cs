using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Networking;

namespace WPMote.Connectivity
{
    class Comm_TCP
    {
        #region "Common variables"

        string strHost = "127.0.0.1";
        int intPort = 8019;
        StreamSocket objClient;

        public event Connectivity.Comm_Common.ConnectedEvent Connected;

        #endregion

        #region "Class properties"
        
        public string Host
        {
            get
            {
                return strHost;
            }
        }

        public int Port
        {
            get
            {
                return intPort;
            }
        }

        #endregion

        #region "Public methods"
        
        public async void Connect(string _host, int _port)
        {
            try
            {
                strHost = _host;
                intPort = _port;

                objClient = new StreamSocket();
                await objClient.ConnectAsync(new HostName(_host), _port.ToString());

                if (Connected != null) Connected(objClient);
            }
            catch (Exception ex)
            {
                if (SocketError.GetStatus(ex.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                if (objClient != null) objClient.Dispose();
            }
        }


        #endregion

        #region "Private methods"

        #endregion
        
    }
}
