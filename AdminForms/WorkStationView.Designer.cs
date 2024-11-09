namespace Laboratory_Management_System.AdminForms
{
    partial class WorkStationView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkStationView));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.componentsPage = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.workstation = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.monitor = new System.Windows.Forms.Label();
            this.keyboard = new System.Windows.Forms.Label();
            this.mouse = new System.Windows.Forms.Label();
            this.avr = new System.Windows.Forms.Label();
            this.motherboard = new System.Windows.Forms.Label();
            this.cpu = new System.Windows.Forms.Label();
            this.ram = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.gpu = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.psu = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.sucase = new System.Windows.Forms.Label();
            this.infoPage = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.systemUnitId = new System.Windows.Forms.Label();
            this.operatingSystem = new System.Windows.Forms.Label();
            this.hostname = new System.Windows.Forms.Label();
            this.room = new System.Windows.Forms.Label();
            this.dateAdded = new System.Windows.Forms.Label();
            this.addedBy = new System.Windows.Forms.Label();
            this.dateUpdated = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.updatedBy = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.conponentsPage = new System.Windows.Forms.TabControl();
            this.label1 = new System.Windows.Forms.Label();
            this.componentsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.infoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.conponentsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.BackColor = System.Drawing.Color.White;
            this.okButton.FlatAppearance.BorderSize = 0;
            this.okButton.Font = new System.Drawing.Font("Montserrat", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.ForeColor = System.Drawing.Color.Black;
            this.okButton.Location = new System.Drawing.Point(358, 387);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.Font = new System.Drawing.Font("Montserrat", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(439, 387);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // componentsPage
            // 
            this.componentsPage.Controls.Add(this.sucase);
            this.componentsPage.Controls.Add(this.label13);
            this.componentsPage.Controls.Add(this.psu);
            this.componentsPage.Controls.Add(this.label30);
            this.componentsPage.Controls.Add(this.gpu);
            this.componentsPage.Controls.Add(this.label12);
            this.componentsPage.Controls.Add(this.ram);
            this.componentsPage.Controls.Add(this.cpu);
            this.componentsPage.Controls.Add(this.motherboard);
            this.componentsPage.Controls.Add(this.avr);
            this.componentsPage.Controls.Add(this.mouse);
            this.componentsPage.Controls.Add(this.keyboard);
            this.componentsPage.Controls.Add(this.monitor);
            this.componentsPage.Controls.Add(this.label21);
            this.componentsPage.Controls.Add(this.label22);
            this.componentsPage.Controls.Add(this.label23);
            this.componentsPage.Controls.Add(this.label25);
            this.componentsPage.Controls.Add(this.label26);
            this.componentsPage.Controls.Add(this.label27);
            this.componentsPage.Controls.Add(this.label28);
            this.componentsPage.Controls.Add(this.workstation);
            this.componentsPage.Controls.Add(this.pictureBox2);
            this.componentsPage.Location = new System.Drawing.Point(4, 25);
            this.componentsPage.Name = "componentsPage";
            this.componentsPage.Padding = new System.Windows.Forms.Padding(3);
            this.componentsPage.Size = new System.Drawing.Size(510, 345);
            this.componentsPage.TabIndex = 1;
            this.componentsPage.Text = "Components";
            this.componentsPage.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(8, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 54);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // workstation
            // 
            this.workstation.AutoSize = true;
            this.workstation.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.workstation.Location = new System.Drawing.Point(74, 25);
            this.workstation.Name = "workstation";
            this.workstation.Size = new System.Drawing.Size(125, 21);
            this.workstation.TabIndex = 12;
            this.workstation.Text = "Workstation ID:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(29, 76);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(58, 16);
            this.label28.TabIndex = 21;
            this.label28.Text = "Monitor:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(29, 103);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(69, 16);
            this.label27.TabIndex = 22;
            this.label27.Text = "Keyboard:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(29, 129);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(50, 16);
            this.label26.TabIndex = 23;
            this.label26.Text = "Mouse:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(29, 154);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(36, 16);
            this.label25.TabIndex = 24;
            this.label25.Text = "AVR:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(29, 180);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(90, 16);
            this.label23.TabIndex = 26;
            this.label23.Text = "Motherboard:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(29, 207);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(38, 16);
            this.label22.TabIndex = 27;
            this.label22.Text = "CPU:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(29, 233);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(39, 16);
            this.label21.TabIndex = 28;
            this.label21.Text = "RAM:";
            // 
            // monitor
            // 
            this.monitor.AutoSize = true;
            this.monitor.Location = new System.Drawing.Point(155, 76);
            this.monitor.Name = "monitor";
            this.monitor.Size = new System.Drawing.Size(101, 16);
            this.monitor.TabIndex = 29;
            this.monitor.Text = "System Unit ID:";
            // 
            // keyboard
            // 
            this.keyboard.AutoSize = true;
            this.keyboard.Location = new System.Drawing.Point(155, 103);
            this.keyboard.Name = "keyboard";
            this.keyboard.Size = new System.Drawing.Size(119, 16);
            this.keyboard.TabIndex = 30;
            this.keyboard.Text = "Operating System:";
            // 
            // mouse
            // 
            this.mouse.AutoSize = true;
            this.mouse.Location = new System.Drawing.Point(155, 129);
            this.mouse.Name = "mouse";
            this.mouse.Size = new System.Drawing.Size(74, 16);
            this.mouse.TabIndex = 31;
            this.mouse.Text = "Hostname:";
            // 
            // avr
            // 
            this.avr.AutoSize = true;
            this.avr.Location = new System.Drawing.Point(155, 154);
            this.avr.Name = "avr";
            this.avr.Size = new System.Drawing.Size(48, 16);
            this.avr.TabIndex = 32;
            this.avr.Text = "Room:";
            // 
            // motherboard
            // 
            this.motherboard.AutoSize = true;
            this.motherboard.Location = new System.Drawing.Point(155, 180);
            this.motherboard.Name = "motherboard";
            this.motherboard.Size = new System.Drawing.Size(69, 16);
            this.motherboard.TabIndex = 34;
            this.motherboard.Text = "Added By:";
            // 
            // cpu
            // 
            this.cpu.AutoSize = true;
            this.cpu.Location = new System.Drawing.Point(155, 207);
            this.cpu.Name = "cpu";
            this.cpu.Size = new System.Drawing.Size(82, 16);
            this.cpu.TabIndex = 35;
            this.cpu.Text = "Date Added:";
            // 
            // ram
            // 
            this.ram.AutoSize = true;
            this.ram.Location = new System.Drawing.Point(155, 233);
            this.ram.Name = "ram";
            this.ram.Size = new System.Drawing.Size(92, 16);
            this.ram.TabIndex = 36;
            this.ram.Text = "Device Status:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(29, 259);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 16);
            this.label12.TabIndex = 37;
            this.label12.Text = "GPU: ";
            // 
            // gpu
            // 
            this.gpu.AutoSize = true;
            this.gpu.Location = new System.Drawing.Point(155, 259);
            this.gpu.Name = "gpu";
            this.gpu.Size = new System.Drawing.Size(92, 16);
            this.gpu.TabIndex = 38;
            this.gpu.Text = "Device Status:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(29, 284);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(36, 16);
            this.label30.TabIndex = 39;
            this.label30.Text = "PSU:";
            this.label30.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // psu
            // 
            this.psu.AutoSize = true;
            this.psu.Location = new System.Drawing.Point(155, 284);
            this.psu.Name = "psu";
            this.psu.Size = new System.Drawing.Size(92, 16);
            this.psu.TabIndex = 40;
            this.psu.Text = "Device Status:";
            this.psu.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(29, 306);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(39, 16);
            this.label13.TabIndex = 41;
            this.label13.Text = "Case:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // sucase
            // 
            this.sucase.AutoSize = true;
            this.sucase.Location = new System.Drawing.Point(155, 306);
            this.sucase.Name = "sucase";
            this.sucase.Size = new System.Drawing.Size(92, 16);
            this.sucase.TabIndex = 42;
            this.sucase.Text = "Device Status:";
            this.sucase.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // infoPage
            // 
            this.infoPage.Controls.Add(this.label1);
            this.infoPage.Controls.Add(this.label31);
            this.infoPage.Controls.Add(this.label32);
            this.infoPage.Controls.Add(this.updatedBy);
            this.infoPage.Controls.Add(this.label11);
            this.infoPage.Controls.Add(this.dateUpdated);
            this.infoPage.Controls.Add(this.addedBy);
            this.infoPage.Controls.Add(this.dateAdded);
            this.infoPage.Controls.Add(this.room);
            this.infoPage.Controls.Add(this.hostname);
            this.infoPage.Controls.Add(this.operatingSystem);
            this.infoPage.Controls.Add(this.systemUnitId);
            this.infoPage.Controls.Add(this.label9);
            this.infoPage.Controls.Add(this.label8);
            this.infoPage.Controls.Add(this.label7);
            this.infoPage.Controls.Add(this.label5);
            this.infoPage.Controls.Add(this.label4);
            this.infoPage.Controls.Add(this.label3);
            this.infoPage.Controls.Add(this.label2);
            this.infoPage.Controls.Add(this.pictureBox1);
            this.infoPage.Font = new System.Drawing.Font("Montserrat", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoPage.Location = new System.Drawing.Point(4, 25);
            this.infoPage.Name = "infoPage";
            this.infoPage.Padding = new System.Windows.Forms.Padding(3);
            this.infoPage.Size = new System.Drawing.Size(510, 345);
            this.infoPage.TabIndex = 0;
            this.infoPage.Text = "Info";
            this.infoPage.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(8, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 54);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "System Unit ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Operating System:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Hostname:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 16);
            this.label5.TabIndex = 5;
            this.label5.Text = "Room:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 180);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 16);
            this.label7.TabIndex = 7;
            this.label7.Text = "Date Added:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 207);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 16);
            this.label8.TabIndex = 8;
            this.label8.Text = "Added By:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 233);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 16);
            this.label9.TabIndex = 9;
            this.label9.Text = "Last Updated:";
            // 
            // systemUnitId
            // 
            this.systemUnitId.AutoSize = true;
            this.systemUnitId.Location = new System.Drawing.Point(192, 76);
            this.systemUnitId.Name = "systemUnitId";
            this.systemUnitId.Size = new System.Drawing.Size(101, 16);
            this.systemUnitId.TabIndex = 11;
            this.systemUnitId.Text = "System Unit ID:";
            // 
            // operatingSystem
            // 
            this.operatingSystem.AutoSize = true;
            this.operatingSystem.Location = new System.Drawing.Point(192, 103);
            this.operatingSystem.Name = "operatingSystem";
            this.operatingSystem.Size = new System.Drawing.Size(119, 16);
            this.operatingSystem.TabIndex = 12;
            this.operatingSystem.Text = "Operating System:";
            // 
            // hostname
            // 
            this.hostname.AutoSize = true;
            this.hostname.Location = new System.Drawing.Point(192, 129);
            this.hostname.Name = "hostname";
            this.hostname.Size = new System.Drawing.Size(74, 16);
            this.hostname.TabIndex = 13;
            this.hostname.Text = "Hostname:";
            // 
            // room
            // 
            this.room.AutoSize = true;
            this.room.Location = new System.Drawing.Point(192, 154);
            this.room.Name = "room";
            this.room.Size = new System.Drawing.Size(48, 16);
            this.room.TabIndex = 14;
            this.room.Text = "Room:";
            // 
            // dateAdded
            // 
            this.dateAdded.AutoSize = true;
            this.dateAdded.Location = new System.Drawing.Point(192, 180);
            this.dateAdded.Name = "dateAdded";
            this.dateAdded.Size = new System.Drawing.Size(69, 16);
            this.dateAdded.TabIndex = 16;
            this.dateAdded.Text = "Added By:";
            // 
            // addedBy
            // 
            this.addedBy.AutoSize = true;
            this.addedBy.Location = new System.Drawing.Point(192, 207);
            this.addedBy.Name = "addedBy";
            this.addedBy.Size = new System.Drawing.Size(82, 16);
            this.addedBy.TabIndex = 17;
            this.addedBy.Text = "Date Added:";
            // 
            // dateUpdated
            // 
            this.dateUpdated.AutoSize = true;
            this.dateUpdated.Location = new System.Drawing.Point(192, 233);
            this.dateUpdated.Name = "dateUpdated";
            this.dateUpdated.Size = new System.Drawing.Size(92, 16);
            this.dateUpdated.TabIndex = 18;
            this.dateUpdated.Text = "Device Status:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(29, 259);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 16);
            this.label11.TabIndex = 19;
            this.label11.Text = "Updated By:";
            // 
            // updatedBy
            // 
            this.updatedBy.AutoSize = true;
            this.updatedBy.Location = new System.Drawing.Point(192, 259);
            this.updatedBy.Name = "updatedBy";
            this.updatedBy.Size = new System.Drawing.Size(92, 16);
            this.updatedBy.TabIndex = 20;
            this.updatedBy.Text = "Device Status:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(29, 284);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(92, 16);
            this.label32.TabIndex = 21;
            this.label32.Text = "Device Status:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(192, 284);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(92, 16);
            this.label31.TabIndex = 22;
            this.label31.Text = "Device Status:";
            // 
            // conponentsPage
            // 
            this.conponentsPage.Controls.Add(this.infoPage);
            this.conponentsPage.Controls.Add(this.componentsPage);
            this.conponentsPage.Dock = System.Windows.Forms.DockStyle.Top;
            this.conponentsPage.Font = new System.Drawing.Font("Montserrat", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conponentsPage.Location = new System.Drawing.Point(0, 0);
            this.conponentsPage.Margin = new System.Windows.Forms.Padding(10);
            this.conponentsPage.Name = "conponentsPage";
            this.conponentsPage.SelectedIndex = 0;
            this.conponentsPage.Size = new System.Drawing.Size(518, 374);
            this.conponentsPage.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(74, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 21);
            this.label1.TabIndex = 23;
            this.label1.Text = "Workstation ID:";
            // 
            // WorkStationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 423);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.conponentsPage);
            this.Name = "WorkStationView";
            this.Text = "Workstation Properties";
            this.Load += new System.EventHandler(this.WorkStationView_Load);
            this.componentsPage.ResumeLayout(false);
            this.componentsPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.infoPage.ResumeLayout(false);
            this.infoPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.conponentsPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TabPage componentsPage;
        private System.Windows.Forms.Label sucase;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label psu;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label gpu;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label ram;
        private System.Windows.Forms.Label cpu;
        private System.Windows.Forms.Label motherboard;
        private System.Windows.Forms.Label avr;
        private System.Windows.Forms.Label mouse;
        private System.Windows.Forms.Label keyboard;
        private System.Windows.Forms.Label monitor;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label workstation;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TabPage infoPage;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label updatedBy;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label dateUpdated;
        private System.Windows.Forms.Label addedBy;
        private System.Windows.Forms.Label dateAdded;
        private System.Windows.Forms.Label room;
        private System.Windows.Forms.Label hostname;
        private System.Windows.Forms.Label operatingSystem;
        private System.Windows.Forms.Label systemUnitId;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl conponentsPage;
        private System.Windows.Forms.Label label1;
    }
}