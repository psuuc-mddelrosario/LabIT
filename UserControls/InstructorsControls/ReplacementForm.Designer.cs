namespace Laboratory_Management_System.UserControls.AdminControls.Crud
{
    partial class ReplacementForm
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
            this.replace = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.newAsset = new System.Windows.Forms.TextBox();
            this.currentAsset = new System.Windows.Forms.Label();
            this.assetType = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // replace
            // 
            this.replace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(74)))), ((int)(((byte)(173)))));
            this.replace.FlatAppearance.BorderSize = 0;
            this.replace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.replace.Font = new System.Drawing.Font("Montserrat", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.replace.ForeColor = System.Drawing.Color.White;
            this.replace.Location = new System.Drawing.Point(116, 152);
            this.replace.Name = "replace";
            this.replace.Size = new System.Drawing.Size(75, 23);
            this.replace.TabIndex = 54;
            this.replace.Text = "Replace";
            this.replace.UseVisualStyleBackColor = false;
            this.replace.Click += new System.EventHandler(this.replace_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.White;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Montserrat", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.Black;
            this.cancelButton.Location = new System.Drawing.Point(200, 152);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 55;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 18);
            this.label5.TabIndex = 48;
            this.label5.Text = "New asset:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 18);
            this.label1.TabIndex = 69;
            this.label1.Text = "Current asset:";
            // 
            // newAsset
            // 
            this.newAsset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newAsset.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newAsset.Location = new System.Drawing.Point(132, 105);
            this.newAsset.Multiline = true;
            this.newAsset.Name = "newAsset";
            this.newAsset.Size = new System.Drawing.Size(143, 29);
            this.newAsset.TabIndex = 71;
            // 
            // currentAsset
            // 
            this.currentAsset.AutoSize = true;
            this.currentAsset.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentAsset.Location = new System.Drawing.Point(129, 21);
            this.currentAsset.Name = "currentAsset";
            this.currentAsset.Size = new System.Drawing.Size(97, 18);
            this.currentAsset.TabIndex = 72;
            this.currentAsset.Text = "Current asset:";
            // 
            // assetType
            // 
            this.assetType.AutoSize = true;
            this.assetType.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetType.Location = new System.Drawing.Point(129, 64);
            this.assetType.Name = "assetType";
            this.assetType.Size = new System.Drawing.Size(97, 18);
            this.assetType.TabIndex = 74;
            this.assetType.Text = "Current asset:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 18);
            this.label3.TabIndex = 73;
            this.label3.Text = "Asset type:";
            // 
            // ReplacementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 196);
            this.ControlBox = false;
            this.Controls.Add(this.assetType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.currentAsset);
            this.Controls.Add(this.newAsset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.replace);
            this.Controls.Add(this.label5);
            this.MaximumSize = new System.Drawing.Size(310, 235);
            this.MinimumSize = new System.Drawing.Size(310, 235);
            this.Name = "ReplacementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Replacement Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button replace;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox newAsset;
        private System.Windows.Forms.Label currentAsset;
        private System.Windows.Forms.Label assetType;
        private System.Windows.Forms.Label label3;
    }
}