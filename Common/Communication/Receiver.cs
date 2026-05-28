using System;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

namespace Common.Communication
{
    /// <summary>Čita JSON linije sa TCP socketa i deserijalizuje ih u zadate tipove.</summary>
    public class Receiver
    {
        private readonly StreamReader _reader;

        /// <summary>Inicijalizuje novu instancu klase <see cref="Receiver"/> za dati socket.</summary>
        /// <param name="socket">Povezan TCP socket sa kojeg se čitaju poruke.</param>
        public Receiver(Socket socket)
        {
            var stream = new NetworkStream(socket);
            _reader = new StreamReader(stream);
        }

        /// <summary>Čita jednu JSON liniju sa mrežnog toka i deserijalizuje je u tip <typeparamref name="T"/>.</summary>
        /// <typeparam name="T">Tip u koji se deserijalizuje primljeni JSON.</typeparam>
        /// <returns>Deserijalizovana instanca tipa <typeparamref name="T"/>.</returns>
        /// <exception cref="IOException">Baca se kada je konekcija zatvorena od strane druge strane.</exception>
        public T Receive<T>()
        {
            string? line = _reader.ReadLine();
            if (line == null) throw new IOException("Connection closed by peer.");
            return JsonSerializer.Deserialize<T>(line)!;
        }

        /// <summary>
        /// Konvertuje payload (najčešće <see cref="JsonElement"/>) u konkretan tip <typeparamref name="T"/>.
        /// Koristi se na serverskoj strani za deserijalizaciju <c>Request.Argument</c>.
        /// </summary>
        /// <typeparam name="T">Ciljni tip u koji se payload konvertuje.</typeparam>
        /// <param name="payload">Payload objekat koji je stigao kroz wire — obično <see cref="JsonElement"/>.</param>
        /// <returns>Instanca tipa <typeparamref name="T"/> hidratisana iz payloada.</returns>
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
