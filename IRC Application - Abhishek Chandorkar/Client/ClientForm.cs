using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace client
{
    //The commands for interaction between the server and the client
    enum Command
    {
        Login,      //Log into the server
        Logout,     //Logout of the server
        Message,    //Send a text message to all the chat clients
        Create,    //Create channel
        List,       //channel list
        Join,       // join channel
        Leave,  //leave channel
        Null        //No command
    }
   
    public partial class Client : Form
    {
        public string channels = "select channel";
        public Socket clientSocket; //The main client socket
        public string strName;      //Username

        
        // Since the methods of socket require data in the form of bytes
        private byte[] byteData = new byte[1024];

        public Client()
        {
 
            InitializeComponent();
           
        }

        //Broadcast the message typed by the user to everyone
        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                if (!channels.Equals((string)ListBox_Room.SelectedItem))
                {
                    // Fill in the necessary message details
                    Data msgToSend = new Data();

                    msgToSend.strName = (string)ListBox_Room.SelectedItem;
                    msgToSend.strMessage = "(" + msgToSend.strName + ") " + strName + " : " + txtbox_MsgBox.Text;
                    msgToSend.cmdCommand = Command.Message;

                    byte[] byteData = msgToSend.ToByte();

                    //Send it to the server
                    clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                }
                else MessageBox.Show("Select the room from the drop down list");
                txtbox_MsgBox.Text = null;
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to send message to the server.", "client: " + strName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "client: " + strName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndReceive(ar);

                Data msgReceived = new Data(byteData);
                //Accordingly process the message received
                switch (msgReceived.cmdCommand)
                {
                    case Command.Leave:
                        if (msgReceived.strMessage != null)
                        {
                            txtbox_Chat.Text += "(" + msgReceived.strMessage + ") " + msgReceived.strName + " has left the room \r\n";

                            if (strName.Equals(msgReceived.strName))
                            {
                                ListBox_Room.Items.Remove(msgReceived.strMessage);
                            }
                        }
                        ListBox_Room.SelectedItem = channels; 
                        break;
                    case Command.Join:
                         int counter = 0;
                        foreach (string li in ListBox_Room.Items)
                        {
                            if (li.Equals(msgReceived.strMessage))
                            {
                                counter = 1;
                                break;
                            }
                        }
                        if(counter==0)
                        {
                            ListBox_Room.Items.Add(msgReceived.strMessage);
                        }
                        txtbox_Chat.Text +=  "("+ msgReceived.strMessage + ") " + msgReceived.strName + " joined the room\r\n";
                        
                        break;
                    case Command.Login:
                        
                        break;

                    case Command.Logout:
                       
                        break;

                    case Command.Message:

                        if (msgReceived.strMessage != null)
                        {
                            txtbox_Chat.Text += msgReceived.strMessage + "\r\n";

                        }
                        break;
                    case Command.Create:
                        listbox_Users.Items.Add(msgReceived.strMessage);
                        txtbox_Chat.Text += "("+msgReceived.strMessage + ") room created \r\n";
                        if (strName.Equals(msgReceived.strName))
                        {
                            ListBox_Room.Items.Add(msgReceived.strMessage);
                            txtbox_Chat.Text += "(" + msgReceived.strMessage + ") " + msgReceived.strName + " joined the room \r\n";
                        }
                          
                        break;

                    case Command.List:

                        if (msgReceived.strMessage != null)
                        {
                            listbox_Users.Items.AddRange(msgReceived.strMessage.Split('*'));
                            listbox_Users.Items.RemoveAt(listbox_Users.Items.Count - 1);
                        }
                     
                        break;
                }

               

                byteData = new byte[1024];

                clientSocket.BeginReceive(byteData,
                                          0,
                                          byteData.Length,
                                          SocketFlags.None,
                                          new AsyncCallback(OnReceive),
                                          null);

            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "client: " + strName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListBox_Room.Items.Add(channels);
            ListBox_Room.SelectedItem = channels; 
            this.Text = "client: " + strName;

            //The user has logged into the system so we now request the server to send
            //the names of all users who are in the chat room
            Data msgToSend = new Data();
            msgToSend.cmdCommand = Command.List;
            msgToSend.strName = strName;
            msgToSend.strMessage = null;

            byteData = msgToSend.ToByte();

           clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);

            byteData = new byte[1024];
            //Start listening to the data asynchronously
            clientSocket.BeginReceive(byteData,
                                       0,
                                       byteData.Length,
                                       SocketFlags.None,
                                       new AsyncCallback(OnReceive),
                                       null);

        }

        private void txtbox_MsgBox_TextChanged(object sender, EventArgs e)
        {
            if (txtbox_MsgBox.Text.Length == 0)
                btn_Send.Enabled = false;
            else
                btn_Send.Enabled = true;
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "client: " + strName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            try
            {
                string toSendmsg = null;
                Data msgToSend = new Data();
                msgToSend.cmdCommand = Command.Logout;
                msgToSend.strName = strName;
                msgToSend.strMessage=null;
                foreach (string channel in ListBox_Room.Items)
                {
                    if (!channels.Equals(channel))
                    {
                        toSendmsg += channel + ",";
                    }
                }
                if (toSendmsg != null)
                {
                    msgToSend.strMessage = toSendmsg.Substring(0, toSendmsg.Length - 1);
                }
                //Send a message to logout of the server
                byte[] b = msgToSend.ToByte();
                clientSocket.Send(b, 0, b.Length, SocketFlags.None);
                clientSocket.Close();
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "client: " + strName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtbox_MsgBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Send_Click(sender, null);
            }
        }

     
        private void btn_CreateRoom_Click(object sender, EventArgs e)
        {

            try
            {

                int counter = 0;
                foreach (string li in listbox_Users.Items)
                {
                    if (li.Equals((string)txtbox_Roomname.Text))
                    {
                        counter = 1;
                    }
                }
                //Fill the info for the message to be send
                if (counter == 0)
                {
                    Data msgToSend = new Data();

                    msgToSend.strName = strName;
                    msgToSend.strMessage = txtbox_Roomname.Text;
                    msgToSend.cmdCommand = Command.Create;

                    byte[] byteData = msgToSend.ToByte();

                    //Send it to the server
                    clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);

                }
                else
                {
                    MessageBox.Show("Room '" + txtbox_Roomname.Text+ "' exists"); 
                }
                txtbox_Roomname.Text = null;
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to create Room", "client: " + strName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBox_Roomname_TextChanged(object sender, EventArgs e)
        {
            if (txtbox_Roomname.Text.Length == 0)
                Btn_CreateRoom.Enabled = false;
            else
                Btn_CreateRoom.Enabled = true;
        }

        private void btn_JoinRoom_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedroom = (string)listbox_Users.SelectedItem;
                int counter = 0;
                foreach (string li in ListBox_Room.Items)
                {
                    if (li.Equals(selectedroom ))
                 
                    {
                        counter = 1;
                    }
                }
                    if(counter==0)
                    {
                        //Fill the info for the message to be send
                        Data msgToSend = new Data();

                        msgToSend.strName = strName;
                        msgToSend.strMessage = selectedroom;
                        msgToSend.cmdCommand = Command.Join;

                        byte[] byteData = msgToSend.ToByte();

                        //Send it to the server
                        clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                    }
                    else
                    {
                        MessageBox.Show("You are already a member of  : " + selectedroom);
                    }
        }
            
            catch (Exception)
            {
                MessageBox.Show("Unable to communicate to server try again", "client: " + strName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstChatters_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (listbox_Users.SelectedIndex!= -1)
            {
                btn_JoinRoom.Enabled = true;
            }
            else
                btn_JoinRoom.Enabled = false;
            

        }

        private void DropDownRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ListBox_Room.SelectedItem.Equals(channels))
            {
                btn_Leave.Enabled = true;
            }
            else
                btn_Leave.Enabled = false;
        }

        private void btn_Leave_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedroom = (string)ListBox_Room.SelectedItem;

                    //Fill the info for the message to be send
                    Data msgToSend = new Data();

                    msgToSend.strName = strName;
                    msgToSend.strMessage = selectedroom;
                    msgToSend.cmdCommand = Command.Leave;

                    byte[] byteData = msgToSend.ToByte();

                    //Send it to the server
                    clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
             
            }

            catch (Exception)
            {
                MessageBox.Show("Unable to communicate to server try again", "client: " + strName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}