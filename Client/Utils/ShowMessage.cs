using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Utils
{
    internal static class ShowMessage
    {
        internal static void Error(string message, string title = "Greska")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        internal static void Error(string message, Control control)
        {
            MessageBox.Show(message, "Greska pri unosu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        internal static void Warning(string message, string title = "Upozorenje")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        internal static void Info(string message, string title = "Informacija")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal static void Success(string message)
        {
            MessageBox.Show(message, "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        internal static DialogResult Dialog(string message)
        {
            return MessageBox.Show(message, "Potvrda", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }


        internal static void ServerDown()
        {
            Error("Server je ugasen!");
        }

    }
}
