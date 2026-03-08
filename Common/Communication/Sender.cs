using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common.Communication
{
    public class Sender
    {
        private NetworkStream _stream;
        private BinaryFormatter _formatter;
        private Socket _socket;

        public Sender(Socket socket)
        {
            _socket = socket;
            _stream = new NetworkStream(socket);
            _formatter = new BinaryFormatter();
        }

        public void Send(object argument)
        {
            try
            {
                _formatter.Serialize(_stream, argument);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
