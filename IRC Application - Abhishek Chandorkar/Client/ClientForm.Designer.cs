namespace client
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            this.btn_Send = new System.Windows.Forms.Button();
            this.txtbox_Chat = new System.Windows.Forms.TextBox();
            this.txtbox_MsgBox = new System.Windows.Forms.TextBox();
            this.listbox_Users = new System.Windows.Forms.ListBox();
            this.ListBox_Room = new System.Windows.Forms.ComboBox();
            this.txtbox_Roomname = new System.Windows.Forms.TextBox();
            this.Btn_CreateRoom = new System.Windows.Forms.Button();
            this.btn_JoinRoom = new System.Windows.Forms.Button();
            this.btn_Leave = new System.Windows.Forms.Button();
            this.label_RoomList = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Send
            // 
            resources.ApplyResources(this.btn_Send, "btn_Send");
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // txtbox_Chat
            // 
            this.txtbox_Chat.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtbox_Chat, "txtbox_Chat");
            this.txtbox_Chat.Name = "txtbox_Chat";
            this.txtbox_Chat.ReadOnly = true;
            // 
            // txtbox_MsgBox
            // 
            resources.ApplyResources(this.txtbox_MsgBox, "txtbox_MsgBox");
            this.txtbox_MsgBox.Name = "txtbox_MsgBox";
            this.txtbox_MsgBox.TextChanged += new System.EventHandler(this.txtbox_MsgBox_TextChanged);
            this.txtbox_MsgBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbox_MsgBox_KeyDown);
            // 
            // listbox_Users
            // 
            this.listbox_Users.FormattingEnabled = true;
            resources.ApplyResources(this.listbox_Users, "listbox_Users");
            this.listbox_Users.Name = "listbox_Users";
            this.listbox_Users.SelectedIndexChanged += new System.EventHandler(this.lstChatters_SelectedIndexChanged);
            // 
            // ListBox_Room
            // 
            this.ListBox_Room.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ListBox_Room.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ListBox_Room.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ListBox_Room.FormattingEnabled = true;
            resources.ApplyResources(this.ListBox_Room, "ListBox_Room");
            this.ListBox_Room.Name = "ListBox_Room";
            this.ListBox_Room.SelectedIndexChanged += new System.EventHandler(this.DropDownRoom_SelectedIndexChanged);
            // 
            // txtbox_Roomname
            // 
            resources.ApplyResources(this.txtbox_Roomname, "txtbox_Roomname");
            this.txtbox_Roomname.Name = "txtbox_Roomname";
            this.txtbox_Roomname.TextChanged += new System.EventHandler(this.txtBox_Roomname_TextChanged);
            // 
            // Btn_CreateRoom
            // 
            resources.ApplyResources(this.Btn_CreateRoom, "Btn_CreateRoom");
            this.Btn_CreateRoom.Name = "Btn_CreateRoom";
            this.Btn_CreateRoom.UseVisualStyleBackColor = true;
            this.Btn_CreateRoom.Click += new System.EventHandler(this.btn_CreateRoom_Click);
            // 
            // btn_JoinRoom
            // 
            resources.ApplyResources(this.btn_JoinRoom, "btn_JoinRoom");
            this.btn_JoinRoom.Name = "btn_JoinRoom";
            this.btn_JoinRoom.UseVisualStyleBackColor = true;
            this.btn_JoinRoom.Click += new System.EventHandler(this.btn_JoinRoom_Click);
            // 
            // btn_Leave
            // 
            resources.ApplyResources(this.btn_Leave, "btn_Leave");
            this.btn_Leave.Name = "btn_Leave";
            this.btn_Leave.UseVisualStyleBackColor = true;
            // 
            // label_RoomList
            // 
            resources.ApplyResources(this.label_RoomList, "label_RoomList");
            this.label_RoomList.Name = "label_RoomList";
            // 
            // Client
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_RoomList);
            this.Controls.Add(this.btn_Leave);
            this.Controls.Add(this.btn_JoinRoom);
            this.Controls.Add(this.Btn_CreateRoom);
            this.Controls.Add(this.txtbox_Roomname);
            this.Controls.Add(this.ListBox_Room);
            this.Controls.Add(this.listbox_Users);
            this.Controls.Add(this.txtbox_MsgBox);
            this.Controls.Add(this.txtbox_Chat);
            this.Controls.Add(this.btn_Send);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.TextBox txtbox_Chat;
        private System.Windows.Forms.TextBox txtbox_MsgBox;
        private System.Windows.Forms.ListBox listbox_Users;
        private System.Windows.Forms.ComboBox ListBox_Room;
        private System.Windows.Forms.TextBox txtbox_Roomname;
        private System.Windows.Forms.Button Btn_CreateRoom;
        private System.Windows.Forms.Button btn_JoinRoom;
        private System.Windows.Forms.Button btn_Leave;
        private System.Windows.Forms.Label label_RoomList;
    }
}

