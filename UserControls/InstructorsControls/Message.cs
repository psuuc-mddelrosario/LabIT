using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing; 

namespace Laboratory_Management_System.UserControls.InstructorsControls
{
    public partial class Message : Form
    {
        private List<string> hostnames;
        private Label loadingLabel; 

        public Message(List<string> hostnames)
        {
            InitializeComponent();
            this.hostnames = hostnames;

            loadingLabel = new Label
            {
                Text = "Sending message...",
                AutoSize = true,
                Visible = false,
                Location = new System.Drawing.Point(10, 150),
                Font = new Font("Montserrat", 8, FontStyle.Regular) 
            };
            this.Controls.Add(loadingLabel);
        }

        private void send_Click(object sender, EventArgs e)
        {
            string messageToSend = messageTextbox.Text;

            loadingLabel.Visible = true;
            send.Enabled = false;

            Thread sendThread = new Thread(() =>
            {
                foreach (var hostname in hostnames)
                {
                    SendMessage(hostname, messageToSend);
                }

                this.Invoke((MethodInvoker)(() =>
                {
                    loadingLabel.Visible = false;
                    send.Enabled = true;                    
                }));

                this.Invoke((MethodInvoker)(() => messageTextbox.Clear()));
            });
            sendThread.Start();
        }

        private void SendMessage(string hostname, string message)
        {
            try
            {
                using (TcpClient client = new TcpClient(hostname, 12345))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);

                    stream.WriteByte(0); 
                    stream.Write(lengthBytes, 0, lengthBytes.Length);
                    stream.Write(messageBytes, 0, messageBytes.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message to {hostname}: {ex.Message}");
            }
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
