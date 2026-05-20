using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

namespace Common.Communication
{
    public class Sender
    {
        private readonly StreamWriter _writer;

        public Sender(Socket socket)
        {
            var stream = new NetworkStream(socket);
            _writer = new StreamWriter(stream) { AutoFlush = true };
        }

        public void Send(object argument)
        {
            try
            {
                string json = JsonSerializer.Serialize(argument, argument.GetType());
                _writer.WriteLine(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
