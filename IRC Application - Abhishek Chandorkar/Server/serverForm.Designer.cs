namespace Server
{
    partial class serverForm
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
            this.txtbox_Log = new System.Windows.Forms.TextBox();
            this.lbl_ActivityLog = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtbox_Log
            // 
            this.txtbox_Log.BackColor = System.Drawing.SystemColors.Window;
            this.txtbox_Log.Location = new System.Drawing.Point(25, 91);
            this.txtbox_Log.Multiline = true;
            this.txtbox_Log.Name = "txtbox_Log";
            this.txtbox_Log.ReadOnly = true;
            this.txtbox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtbox_Log.Size = new System.Drawing.Size(326, 259);
            this.txtbox_Log.TabIndex = 0;
            
            // 
            // lbl_ActivityLog
            // 
            this.lbl_ActivityLog.AutoSize = true;
            this.lbl_ActivityLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ActivityLog.Location = new System.Drawing.Point(34, 29);
            this.lbl_ActivityLog.Name = "lbl_ActivityLog";
            this.lbl_ActivityLog.Size = new System.Drawing.Size(166, 31);
            this.lbl_ActivityLog.TabIndex = 1;
            this.lbl_ActivityLog.Text = "Activity Log";
            // 
            // serverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 374);
            this.Controls.Add(this.lbl_ActivityLog);
            this.Controls.Add(this.txtbox_Log);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "serverForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbox_Log;
        private System.Windows.Forms.Label lbl_ActivityLog;

    }
}

