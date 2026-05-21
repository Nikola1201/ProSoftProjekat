using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Client
{
    internal static class AppResources
    {
        private const string AutoskolaResourceName = "Client.Resources.autoskola.jpg";
        private static Image _autoskolaBackground;

        internal static Image AutoskolaBackground
        {
            get
            {
                if (_autoskolaBackground == null)
                {
                    Assembly asm = typeof(AppResources).Assembly;
                    using Stream stream = asm.GetManifestResourceStream(AutoskolaResourceName)
                        ?? throw new InvalidOperationException(AutoskolaResourceName + " embedded resource not found.");

                    MemoryStream copy = new MemoryStream();
                    stream.CopyTo(copy);
                    copy.Position = 0;
                    _autoskolaBackground = Image.FromStream(copy);
                }
                return _autoskolaBackground;
            }
        }
    }
}
