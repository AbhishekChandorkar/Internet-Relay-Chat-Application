using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace client
{
    public partial class LoginForm : Form
    {
        public Socket clientSocket;
        public string strName;
        public string password;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtbox_Name.Text.Length > 0 && txtbox_Pwd.Text.Length > 0 && txtbox_ServerIP.Text.Length > 0)
                {
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    IPAddress ipAddress = IPAddress.Parse(txtbox_ServerIP.Text);
                    //Server is listening on port 7777
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 7777);

                    //Connect to the server
                    clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
                }
                else MessageBox.Show("Please Enter All Values");
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "client", MessageBoxButtons.OK, MessageBoxIcon.Error); 
             } 
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
                strName = txtbox_Name.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);

                //Connection to the server is successful
                Data msgToSend = new Data ();
                msgToSend.cmdCommand = Command.Login;
                msgToSend.strName = txtbox_Name.Text;
                msgToSend.strMessage = txtbox_Pwd.Text;

                byte[] b = msgToSend.ToByte ();

                //Send the message to the server
                clientSocket.BeginSend(b, 0, b.Length, SocketFlags.None, new AsyncCallback(OnSend), null);

            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "client", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtbox_Name.Text.Length > 0 && txtbox_ServerIP.Text.Length > 0)
            {
                btn_Submit.Enabled = true;
            }
            else
                btn_Submit.Enabled = false;
        }

        private void txtServerIP_TextChanged(object sender, EventArgs e)
        {
            if (txtbox_Name.Text.Length > 0 && txtbox_ServerIP.Text.Length > 0)
                btn_Submit.Enabled = true;
            else
                btn_Submit.Enabled = false;
        }
    }
}