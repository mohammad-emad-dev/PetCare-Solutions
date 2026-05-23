using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace bitcINTERFACE
{
    internal static class UserMessages
    {
        internal static void ShowDatabaseError(string action, Exception exception)
        {
            Debug.WriteLine($"{action}: {exception}");
            MessageBox.Show($"{action}. Please try again later.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        internal static void ShowTechnicalError(string action, Exception exception)
        {
            Debug.WriteLine($"{action}: {exception}");
            MessageBox.Show($"{action}. Please try again later.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }
}
