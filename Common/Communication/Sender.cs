using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

namespace Common.Communication
{
    /// <summary>Serijalizuje objekat u JSON i šalje ga kroz TCP socket linijskim frejmingom.</summary>
    public class Sender
    {
        private readonly StreamWriter _writer;

        /// <summary>Inicijalizuje novu instancu klase <see cref="Sender"/> za dati socket.</summary>
        /// <param name="socket">Povezan TCP socket kroz koji se šalju poruke.</param>
        public Sender(Socket socket)
        {
            var stream = new NetworkStream(socket);
            _writer = new StreamWriter(stream) { AutoFlush = true };
        }

        /// <summary>Serijalizuje objekat u JSON i zapisuje ga kao jednu liniju na mrežni tok.</summary>
        /// <param name="argument">Objekat koji se šalje; serijalizuje se koristeći stvarni runtime tip.</param>
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
