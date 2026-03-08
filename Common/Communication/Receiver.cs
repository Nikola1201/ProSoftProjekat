using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common.Communication
{
    public class Receiver
    {
        private BinaryFormatter _formatter;
        private NetworkStream _stream;
        private Socket _socket;

        public Receiver(Socket socket)
        {
            _socket = socket;
            _stream = new NetworkStream(socket);
            _formatter = new BinaryFormatter();
        }

        public object Receive()
        {
            return _formatter.Deserialize(_stream);
        }
    }
}
