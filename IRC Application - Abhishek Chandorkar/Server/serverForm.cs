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

namespace Server
{
    //The commands for interaction between the server and the client
    enum Command
    {
        Login,      //Log into the server
        Logout,     //Logout of the server
        Message,    //Send a text message to all the chat clients
        Create,     //Create a Chat Room
        List,       //List of all the available Chat Rooms
        Join,       //Join a Chat Room
        Leave,      //Leave a Chat Room
        Null        //No command
    }

    public partial class serverForm : Form
    {
        //The ClientInfo structure holds the required information about each connected client
        
        struct ClientInfo
        {
            public Socket socket;   //Client Socket
            public string strName;
            public string password; //Name by which the user logged into the chat room
        }
        struct ChannelInfo
        {
            public string channelname;
            public string strName;  //The Username
            public ArrayList Channelmembers;
        }

        //The collection of all clients logged into the room (an array of type ClientInfo)
        ArrayList clientList;
        ArrayList Channellist;

        //The main socket on which the server listens to the clients
        Socket serverSocket;


        //since the send and receive methods for the socket accept data in the form of byte arrays
        byte[] byteData = new byte[1024];

        public serverForm()
        {
            clientList = new ArrayList();
            Channellist = new ArrayList();
           
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            try
            {
                //TCP sockets are being used. So protocol is specified as TCP in the constructor
                serverSocket = new Socket(AddressFamily.InterNetwork, 
                                          SocketType.Stream, 
                                          ProtocolType.Tcp);
                
                //Dynamically assigns any IP address
                //Server listens for connections on port number 7777
                // Any port number > 1023 is acceptable.
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 7777);

                txtbox_Log.Text += "Server is Running..\r\n";
                //Bind and listen on the given address
                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen(5);

                //Accept the incoming clients
                serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "server",                     
                    MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }            
        }

        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = serverSocket.EndAccept(ar);

                //Start listening for more clients
                serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);

                //Once the client connects then start receiving the commands from her
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, 
                    new AsyncCallback(OnReceive), clientSocket);                
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "server", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                clientSocket.EndReceive(ar);

                //Convert the received byte data into user readable
                Data msgReceived = new Data(byteData);

                //This object will be sent as a response to the user's request
                Data msgToSend = new Data();

                byte [] message;
                
                // In the case of login, logout or the normal text messages, the type remains the same
                msgToSend.cmdCommand = msgReceived.cmdCommand;
                msgToSend.strName = msgReceived.strName;


                switch (msgReceived.cmdCommand)
                {
                    
                    case Command.Join:
                        foreach (ChannelInfo channelinfo in Channellist)
                        {
                            if (channelinfo.channelname.Equals(msgReceived.strMessage))
                            {
                                channelinfo.Channelmembers.Add(msgReceived.strName);
                                break;
                            }
                        }
                        msgToSend.cmdCommand = Command.Join;
                        
                        msgToSend.strName = msgReceived.strName;
                        msgToSend.strMessage = msgReceived.strMessage;
                        message = msgToSend.ToByte();

                        foreach (ChannelInfo channelinfo in Channellist)
                        {
                            if (channelinfo.channelname.Equals(msgReceived.strMessage))
                            {
                                foreach (string s in channelinfo.Channelmembers)
                                {
                                    foreach (ClientInfo client in clientList)
                                    {
                                        if (s.Equals(client.strName))
                                        {
                                            client.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                                new AsyncCallback(OnSend), client.socket);
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                        txtbox_Log.Text += " " + msgReceived.strName + " joined the room '" + msgReceived.strMessage + "' \r\n";
                        break;
                    case Command.Create:

                        ChannelInfo channelInfo = new ChannelInfo();
                        channelInfo.channelname = msgReceived.strMessage;
                        channelInfo.strName = msgReceived.strName;
                        channelInfo.Channelmembers = new ArrayList();
                        channelInfo.Channelmembers.Add(msgReceived.strName);
                        Channellist.Add(channelInfo);

                        msgToSend.strMessage = msgReceived.strMessage;

                        txtbox_Log.Text += msgReceived.strName + ": created room :  " +msgReceived.strMessage+" \r\n";
                        txtbox_Log.Text += msgReceived.strName + ": joined room :  " + msgReceived.strMessage + " \r\n";
                        break;
                       
                    case Command.Login:
                        
                        //When a user logs in to the server then we add him to the list of clients

                              ClientInfo clientInfo = new ClientInfo();
                            clientInfo.socket = clientSocket;
                            clientInfo.strName = msgReceived.strName;
                            clientInfo.password = msgReceived.strMessage;

                            clientList.Add(clientInfo);
                            txtbox_Log.Text += msgReceived.strName + ": has logged in \r\n";
   
                        break;

                    case Command.Logout:                    
                        
                        //When a user logs out, we remove him from the memory

                       int nIndex = 0;
                        foreach (ClientInfo client in clientList)
                        {
                            if (client.socket == clientSocket)
                            {
                                clientList.RemoveAt(nIndex);
                                break;
                            }
                            ++nIndex;
                        }
                        

                        clientSocket.Close();
                        txtbox_Log.Text += " " + msgReceived.strName + ": logged out \r\n";
                       if (msgReceived.strMessage != null)
                        {
                        string[] Channels = msgReceived.strMessage.Split(',');
                        foreach(string channels in Channels)
                        {
                         nIndex = 0;
                        msgToSend.cmdCommand = Command.Leave;
                        msgToSend.strName = msgReceived.strName;
                        msgToSend.strMessage = null;
                        foreach (ChannelInfo channel in Channellist)
                        {
                            if (channel.channelname.Equals(channels))
                            {

                                int i = 0;
                                foreach (string s in channel.Channelmembers)
                                {
                                    if (s.Equals(msgReceived.strName))
                                    {
                                        channel.Channelmembers.RemoveAt(i);
                                        msgToSend.strMessage = channels;
                                        break;
                                    }
                                    i++;
                                }

                                break;
                            }
                            ++nIndex;
                        }

                        
                        // Inform the other clients that the client has left the room(s) which he was a part of    
                        message = msgToSend.ToByte();
                        foreach (ChannelInfo channelinfo in Channellist)
                        {
                            if (channelinfo.channelname.Equals(channels))
                            {
                                foreach (string s in channelinfo.Channelmembers)
                                {
                                    foreach (ClientInfo client in clientList)
                                    {
                                        if (s.Equals(client.strName))
                                        {
                                            client.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                                new AsyncCallback(OnSend), client.socket);
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                        
                        }
                        txtbox_Log.Text +=
                                " " + msgReceived.strName + ": has left the room(s) '" + msgReceived.strMessage +
                                   "'  \r\n";
                       }
                        break;
                    case Command.Leave:
                        nIndex = 0;
                        msgToSend.cmdCommand = Command.Leave;
                        msgToSend.strName = msgReceived.strName;
                        msgToSend.strMessage = null;
                        
                        // Remove the client from the list of users in the room
                        foreach (ChannelInfo channel in Channellist)
                        {
                            if (channel.channelname.Equals(msgReceived.strMessage))
                            {

                                int i = 0;
                                foreach (string s in channel.Channelmembers)
                                {
                                    if (s.Equals(msgReceived.strName))
                                    {
                                        channel.Channelmembers.RemoveAt(i);
                                        msgToSend.strMessage = msgReceived.strMessage;
                                        break;
                                    }
                                    i++;
                                }

                                break;
                            }
                            ++nIndex;
                        }


                        // Inform the other clients about the leaving of the client
                        message = msgToSend.ToByte();
                        foreach (ChannelInfo channelinfo in Channellist)
                        {
                            if (channelinfo.channelname.Equals(msgReceived.strMessage))
                            {
                                foreach (string s in channelinfo.Channelmembers)
                                {
                                    foreach (ClientInfo client in clientList)
                                    {
                                        if (s.Equals(client.strName))
                                        {
                                            client.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                                new AsyncCallback(OnSend), client.socket);
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                        txtbox_Log.Text +=
                            " " + msgReceived.strName + ": has left the room '" + msgReceived.strMessage +
                            "'  \r\n";
                        break;
                    case Command.Message:
                        msgToSend.cmdCommand = Command.Message;                
                        msgToSend.strName = msgReceived.strName;
                        msgToSend.strMessage = msgReceived.strMessage;
                       
                        
                        message = msgToSend.ToByte();
                        //Set the text of the message that is sent to all users in the room
                        foreach (ChannelInfo channelinfo in Channellist)
                        {
                            if (channelinfo.channelname.Equals(msgReceived.strName))
                            {
                                foreach (string s in channelinfo.Channelmembers)
                                {
                                    foreach (ClientInfo client in clientList)
                                    {
                                        if (s.Equals(client.strName))
                                        {
                                            client.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                                new AsyncCallback(OnSend), client.socket);
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }

                        break;

                    case Command.List:

                        //Send the names of all the chatrooms to the new user
                        msgToSend.cmdCommand = Command.List;
                        msgToSend.strName = msgReceived.strName;
                        msgToSend.strMessage = null;

                        foreach (ChannelInfo channel in Channellist)
                        {
                         
                            // The * symbol is used to differentiate between the rooms and other text
                            msgToSend.strMessage += channel.channelname + "*";   
                        }

                        message = msgToSend.ToByte();

                        //Send the name of the chat rooms
                        clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                new AsyncCallback(OnSend), clientSocket);                        
                        break;
                }

                if (msgToSend.cmdCommand != Command.List && msgToSend.cmdCommand != Command.Join
                    && msgToSend.cmdCommand != Command.Message
                    && msgToSend.cmdCommand != Command.Login
                    && msgToSend.cmdCommand != Command.Logout)   
                {
                    message = msgToSend.ToByte();

                    //This message is only sent to the specific client who asks for it and is not broadcasted

                    foreach (ClientInfo clientInfo in clientList)
                    {
                       
                         
                        if (clientInfo.socket != clientSocket ||
                            msgToSend.cmdCommand != Command.Login)
                        {
                            
                            clientInfo.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                new AsyncCallback(OnSend), clientInfo.socket);                            
                        }
                    }
                }

                if (msgReceived.cmdCommand != Command.Logout)
                {
                    // Receive the data sent by the user
                    clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "server", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        public void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);                
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "server", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

     }
}