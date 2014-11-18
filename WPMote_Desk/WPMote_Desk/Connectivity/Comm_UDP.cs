using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace WPMote_Desk.Connectivity
{
    class Comm_UDP
    {
        #region "Events"

        public delegate void DOnDataReceived(byte[] buffer);
        public event DOnDataReceived OnDataReceived;

        #endregion

        #region "Common variables"

        int intPort = 8007;

        //2 different sockets to avoid thread conflict
        Socket objSend;
        Socket objRecv;

        Task tskMessages;
        CancellationTokenSource objCancelSource;
        CancellationToken objCancelToken;

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

        public Comm_UDP()
        {
            objSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            objRecv = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            objCancelSource = new CancellationTokenSource();
            objCancelToken = objCancelSource.Token;
            tskMessages = Task.Factory.StartNew(() => ReceiveThread(), objCancelSource.Token);
        }

        #endregion


        #region "Public methods"

        public void SendBytes(string strServerName, byte[] buffer)
        {
            if (objSend != null)
            {
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = new DnsEndPoint(strServerName, intPort);

                socketEventArg.SetBuffer(buffer, 0, buffer.Length);
                objSend.SendToAsync(socketEventArg);
            }
        }

        #endregion

        #region "Private methods"

        private void ReceiveThread()
        {
            if (objRecv != null)
            {
                while (!objCancelToken.IsCancellationRequested)
                {
                    SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                    socketEventArg.RemoteEndPoint = new IPEndPoint(IPAddress.Any, intPort);

                    socketEventArg.SetBuffer(new Byte[Comm_Common.BUFFER_SIZE], 0, Comm_Common.BUFFER_SIZE);


                    socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
                    {
                        if (e.SocketError == SocketError.Success)
                        {
                            // Retrieve the data from the buffer
                            if (OnDataReceived != null) OnDataReceived.Invoke(e.Buffer);
                        }
                        else
                        {
                            // TODO: Error handling
                        }
                    });

                    objRecv.ReceiveFromAsync(socketEventArg);

                    tskMessages.Wait(Comm_Common.TIMEOUT_MILLISECONDS); //Receive in intervals of TIMEOUT
                }
            }
        }

        #endregion
    }
}
