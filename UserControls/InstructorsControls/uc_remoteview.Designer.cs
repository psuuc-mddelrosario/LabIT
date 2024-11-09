namespace Laboratory_Management_System.UserControls.InstructorsControls
{
    partial class uc_remoteview
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_remoteview));
            this.panel1 = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.reconnectButton = new System.Windows.Forms.Button();
            this.restartButton = new System.Windows.Forms.Button();
            this.shutdownButton = new System.Windows.Forms.Button();
            this.lockButton = new System.Windows.Forms.Button();
            this.messageButton = new System.Windows.Forms.Button();
            this.requestSchedule = new System.Windows.Forms.Button();
            this.unlockButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.titleLabel);
            this.panel1.Location = new System.Drawing.Point(29, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(896, 61);
            this.panel1.TabIndex = 2;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Montserrat", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(74)))), ((int)(((byte)(173)))));
            this.titleLabel.Location = new System.Drawing.Point(13, 16);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(161, 27);
            this.titleLabel.TabIndex = 2;
            this.titleLabel.Text = "Remote View ";
            this.titleLabel.Click += new System.EventHandler(this.titleLabel_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(29, 180);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(896, 520);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // reconnectButton
            // 
            this.reconnectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(74)))), ((int)(((byte)(173)))));
            this.reconnectButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.reconnectButton.FlatAppearance.BorderSize = 0;
            this.reconnectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reconnectButton.Font = new System.Drawing.Font("Montserrat", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reconnectButton.ForeColor = System.Drawing.Color.White;
            this.reconnectButton.Location = new System.Drawing.Point(29, 132);
            this.reconnectButton.Name = "reconnectButton";
            this.reconnectButton.Size = new System.Drawing.Size(99, 29);
            this.reconnectButton.TabIndex = 14;
            this.reconnectButton.Text = "Reconnect";
            this.reconnectButton.UseVisualStyleBackColor = false;
            this.reconnectButton.Click += new System.EventHandler(this.reconnectButton_Click);
            // 
            // restartButton
            // 
            this.restartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.restartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(179)))), ((int)(((byte)(71)))));
            this.restartButton.FlatAppearance.BorderSize = 0;
            this.restartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.restartButton.Image = ((System.Drawing.Image)(resources.GetObject("restartButton.Image")));
            this.restartButton.Location = new System.Drawing.Point(890, 126);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(35, 35);
            this.restartButton.TabIndex = 22;
            this.restartButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.restartButton.UseVisualStyleBackColor = false;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // shutdownButton
            // 
            this.shutdownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.shutdownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(62)))), ((int)(((byte)(63)))));
            this.shutdownButton.FlatAppearance.BorderSize = 0;
            this.shutdownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.shutdownButton.Image = ((System.Drawing.Image)(resources.GetObject("shutdownButton.Image")));
            this.shutdownButton.Location = new System.Drawing.Point(849, 126);
            this.shutdownButton.Name = "shutdownButton";
            this.shutdownButton.Size = new System.Drawing.Size(35, 35);
            this.shutdownButton.TabIndex = 21;
            this.shutdownButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.shutdownButton.UseVisualStyleBackColor = false;
            this.shutdownButton.Click += new System.EventHandler(this.shutdownButton_Click);
            // 
            // lockButton
            // 
            this.lockButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lockButton.BackColor = System.Drawing.Color.Gray;
            this.lockButton.FlatAppearance.BorderSize = 0;
            this.lockButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lockButton.Image = ((System.Drawing.Image)(resources.GetObject("lockButton.Image")));
            this.lockButton.Location = new System.Drawing.Point(726, 126);
            this.lockButton.Name = "lockButton";
            this.lockButton.Size = new System.Drawing.Size(35, 35);
            this.lockButton.TabIndex = 23;
            this.lockButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.lockButton.UseVisualStyleBackColor = false;
            this.lockButton.Click += new System.EventHandler(this.lockButton_Click);
            // 
            // messageButton
            // 
            this.messageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.messageButton.BackColor = System.Drawing.Color.CornflowerBlue;
            this.messageButton.FlatAppearance.BorderSize = 0;
            this.messageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.messageButton.Image = ((System.Drawing.Image)(resources.GetObject("messageButton.Image")));
            this.messageButton.Location = new System.Drawing.Point(808, 126);
            this.messageButton.Name = "messageButton";
            this.messageButton.Size = new System.Drawing.Size(35, 35);
            this.messageButton.TabIndex = 24;
            this.messageButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.messageButton.UseVisualStyleBackColor = false;
            this.messageButton.Click += new System.EventHandler(this.messageButton_Click);
            // 
            // requestSchedule
            // 
            this.requestSchedule.BackColor = System.Drawing.SystemColors.Control;
            this.requestSchedule.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(74)))), ((int)(((byte)(173)))));
            this.requestSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.requestSchedule.Font = new System.Drawing.Font("Montserrat", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requestSchedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(74)))), ((int)(((byte)(173)))));
            this.requestSchedule.Location = new System.Drawing.Point(136, 132);
            this.requestSchedule.Name = "requestSchedule";
            this.requestSchedule.Size = new System.Drawing.Size(152, 29);
            this.requestSchedule.TabIndex = 25;
            this.requestSchedule.Text = "Request Schedule";
            this.requestSchedule.UseVisualStyleBackColor = false;
            this.requestSchedule.Click += new System.EventHandler(this.requestSchedule_Click);
            // 
            // unlockButton
            // 
            this.unlockButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unlockButton.BackColor = System.Drawing.Color.YellowGreen;
            this.unlockButton.FlatAppearance.BorderSize = 0;
            this.unlockButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.unlockButton.Image = ((System.Drawing.Image)(resources.GetObject("unlockButton.Image")));
            this.unlockButton.Location = new System.Drawing.Point(767, 126);
            this.unlockButton.Name = "unlockButton";
            this.unlockButton.Size = new System.Drawing.Size(35, 35);
            this.unlockButton.TabIndex = 26;
            this.unlockButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.unlockButton.UseVisualStyleBackColor = false;
            this.unlockButton.Click += new System.EventHandler(this.unlockButton_Click);
            // 
            // uc_remoteview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.unlockButton);
            this.Controls.Add(this.requestSchedule);
            this.Controls.Add(this.messageButton);
            this.Controls.Add(this.lockButton);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.shutdownButton);
            this.Controls.Add(this.reconnectButton);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "uc_remoteview";
            this.Size = new System.Drawing.Size(955, 731);
            this.Load += new System.EventHandler(this.uc_remoteview_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button reconnectButton;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.Button shutdownButton;
        private System.Windows.Forms.Button lockButton;
        private System.Windows.Forms.Button messageButton;
        private System.Windows.Forms.Button requestSchedule;
        private System.Windows.Forms.Button unlockButton;
    }
}
