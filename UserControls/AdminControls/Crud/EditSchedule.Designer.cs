namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    partial class EditSchedule
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.subjectDropdown = new System.Windows.Forms.ComboBox();
            this.yearDropdown = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.blockDropdown = new System.Windows.Forms.ComboBox();
            this.update = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.dayDropdown = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.classStart = new System.Windows.Forms.DateTimePicker();
            this.classEnd = new System.Windows.Forms.DateTimePicker();
            this.instructorDropdown = new System.Windows.Forms.ComboBox();
            this.courseDropdown = new System.Windows.Forms.ComboBox();
            this.roomDropdown = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Montserrat", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 29);
            this.label1.TabIndex = 41;
            this.label1.Text = "Edit Schedule";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 18);
            this.label2.TabIndex = 42;
            this.label2.Text = "Instructor:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 18);
            this.label4.TabIndex = 46;
            this.label4.Text = "Subject:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 18);
            this.label5.TabIndex = 48;
            this.label5.Text = "Course:";
            // 
            // subjectDropdown
            // 
            this.subjectDropdown.DropDownWidth = 250;
            this.subjectDropdown.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subjectDropdown.FormattingEnabled = true;
            this.subjectDropdown.Location = new System.Drawing.Point(127, 219);
            this.subjectDropdown.Name = "subjectDropdown";
            this.subjectDropdown.Size = new System.Drawing.Size(171, 29);
            this.subjectDropdown.TabIndex = 49;
            this.subjectDropdown.SelectedIndexChanged += new System.EventHandler(this.subjectDropdown_SelectedIndexChanged);
            // 
            // yearDropdown
            // 
            this.yearDropdown.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yearDropdown.FormattingEnabled = true;
            this.yearDropdown.Location = new System.Drawing.Point(127, 263);
            this.yearDropdown.Name = "yearDropdown";
            this.yearDropdown.Size = new System.Drawing.Size(171, 29);
            this.yearDropdown.TabIndex = 50;
            this.yearDropdown.SelectedIndexChanged += new System.EventHandler(this.yearDropdown_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(23, 267);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 18);
            this.label6.TabIndex = 51;
            this.label6.Text = "Year:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(23, 310);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 18);
            this.label7.TabIndex = 53;
            this.label7.Text = "Block:";
            // 
            // blockDropdown
            // 
            this.blockDropdown.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blockDropdown.FormattingEnabled = true;
            this.blockDropdown.Location = new System.Drawing.Point(127, 306);
            this.blockDropdown.Name = "blockDropdown";
            this.blockDropdown.Size = new System.Drawing.Size(171, 29);
            this.blockDropdown.TabIndex = 52;
            this.blockDropdown.SelectedIndexChanged += new System.EventHandler(this.blockDropdown_SelectedIndexChanged);
            // 
            // update
            // 
            this.update.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(74)))), ((int)(((byte)(173)))));
            this.update.FlatAppearance.BorderSize = 0;
            this.update.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.update.Font = new System.Drawing.Font("Montserrat", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.update.ForeColor = System.Drawing.Color.White;
            this.update.Location = new System.Drawing.Point(139, 476);
            this.update.Name = "update";
            this.update.Size = new System.Drawing.Size(75, 23);
            this.update.TabIndex = 54;
            this.update.Text = "Update";
            this.update.UseVisualStyleBackColor = false;
            this.update.Click += new System.EventHandler(this.update_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.White;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Montserrat", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.Black;
            this.cancelButton.Location = new System.Drawing.Point(223, 476);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 55;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(74)))), ((int)(((byte)(173)))));
            this.panel1.Location = new System.Drawing.Point(26, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(3, 30);
            this.panel1.TabIndex = 56;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(23, 352);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 18);
            this.label8.TabIndex = 58;
            this.label8.Text = "Day:";
            // 
            // dayDropdown
            // 
            this.dayDropdown.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dayDropdown.FormattingEnabled = true;
            this.dayDropdown.Location = new System.Drawing.Point(127, 348);
            this.dayDropdown.Name = "dayDropdown";
            this.dayDropdown.Size = new System.Drawing.Size(171, 29);
            this.dayDropdown.TabIndex = 57;
            this.dayDropdown.SelectedIndexChanged += new System.EventHandler(this.dayDropdown_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(23, 431);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 18);
            this.label9.TabIndex = 60;
            this.label9.Text = "Class End:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(23, 393);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(78, 18);
            this.label10.TabIndex = 59;
            this.label10.Text = "Class Start:";
            // 
            // classStart
            // 
            this.classStart.CalendarFont = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.classStart.Cursor = System.Windows.Forms.Cursors.Default;
            this.classStart.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.classStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.classStart.Location = new System.Drawing.Point(127, 391);
            this.classStart.Name = "classStart";
            this.classStart.ShowUpDown = true;
            this.classStart.Size = new System.Drawing.Size(171, 26);
            this.classStart.TabIndex = 61;
            this.classStart.ValueChanged += new System.EventHandler(this.classStart_ValueChanged);
            // 
            // classEnd
            // 
            this.classEnd.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.classEnd.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.classEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.classEnd.Location = new System.Drawing.Point(127, 429);
            this.classEnd.Name = "classEnd";
            this.classEnd.ShowUpDown = true;
            this.classEnd.Size = new System.Drawing.Size(171, 26);
            this.classEnd.TabIndex = 62;
            this.classEnd.ValueChanged += new System.EventHandler(this.classEnd_ValueChanged);
            // 
            // instructorDropdown
            // 
            this.instructorDropdown.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructorDropdown.FormattingEnabled = true;
            this.instructorDropdown.Location = new System.Drawing.Point(127, 85);
            this.instructorDropdown.Name = "instructorDropdown";
            this.instructorDropdown.Size = new System.Drawing.Size(171, 29);
            this.instructorDropdown.TabIndex = 63;
            this.instructorDropdown.SelectedIndexChanged += new System.EventHandler(this.instructorDropdown_SelectedIndexChanged);
            // 
            // courseDropdown
            // 
            this.courseDropdown.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.courseDropdown.FormattingEnabled = true;
            this.courseDropdown.Location = new System.Drawing.Point(127, 173);
            this.courseDropdown.Name = "courseDropdown";
            this.courseDropdown.Size = new System.Drawing.Size(171, 29);
            this.courseDropdown.TabIndex = 65;
            this.courseDropdown.SelectedIndexChanged += new System.EventHandler(this.courseDropdown_SelectedIndexChanged);
            // 
            // roomDropdown
            // 
            this.roomDropdown.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roomDropdown.FormattingEnabled = true;
            this.roomDropdown.Location = new System.Drawing.Point(127, 129);
            this.roomDropdown.Name = "roomDropdown";
            this.roomDropdown.Size = new System.Drawing.Size(171, 29);
            this.roomDropdown.TabIndex = 67;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 18);
            this.label3.TabIndex = 66;
            this.label3.Text = "Room:";
            // 
            // EditSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 521);
            this.ControlBox = false;
            this.Controls.Add(this.roomDropdown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.courseDropdown);
            this.Controls.Add(this.instructorDropdown);
            this.Controls.Add(this.classEnd);
            this.Controls.Add(this.classStart);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dayDropdown);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.update);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.blockDropdown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.yearDropdown);
            this.Controls.Add(this.subjectDropdown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(341, 560);
            this.MinimumSize = new System.Drawing.Size(341, 560);
            this.Name = "EditSchedule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Schedule";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox subjectDropdown;
        private System.Windows.Forms.ComboBox yearDropdown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox blockDropdown;
        private System.Windows.Forms.Button update;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox dayDropdown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker classStart;
        private System.Windows.Forms.DateTimePicker classEnd;
        private System.Windows.Forms.ComboBox instructorDropdown;
        private System.Windows.Forms.ComboBox courseDropdown;
        private System.Windows.Forms.ComboBox roomDropdown;
        private System.Windows.Forms.Label label3;
    }
}