using System;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

namespace Common.Communication
{
    public class Receiver
    {
        private readonly StreamReader _reader;

        public Receiver(Socket socket)
        {
            var stream = new NetworkStream(socket);
            _reader = new StreamReader(stream);
        }

        public T Receive<T>()
        {
            string? line = _reader.ReadLine();
            if (line == null) throw new IOException("Connection closed by peer.");
            return JsonSerializer.Deserialize<T>(line)!;
        }

        public T ReadType<T>(object payload)
        {
            if (payload is JsonElement element)
            {
                return element.Deserialize<T>()!;
            }
            return (T)payload;
        }
    }
}
