using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            

            LoginForm loginForm = new LoginForm();

            Application.Run(loginForm);

            if (loginForm.DialogResult == DialogResult.OK)
            {
                Client ClientForm = new Client();
                ClientForm.clientSocket = loginForm.clientSocket;
                ClientForm.strName = loginForm.strName;
                ClientForm.ShowDialog();
            }

        }
    }
}