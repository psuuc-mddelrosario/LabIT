using Laboratory_Management_System.UserControls.AdminControls.Crud;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratory_Management_System.UserControls.InstructorsControls
{
    public partial class uc_remoteview : UserControl
    {
        private DatabaseHelper dbHelper;
        private List<TcpClient> clients = new List<TcpClient>();
        private List<NetworkStream> streams = new List<NetworkStream>();
        private const int Port = 12345;
        private string instructorId;
        private Dictionary<string, bool> workstationLockStates = new Dictionary<string, bool>(); 

        public uc_remoteview(string instructorId)
        {
            InitializeComponent();
            this.instructorId = instructorId;
            dbHelper = new DatabaseHelper();
            flowLayoutPanel1.AutoScroll = true; 
        }

        private string GetAssignedRoomForCurrentUser()
        {
            string currentUserId = instructorId;
            string currentDay = DateTime.Now.DayOfWeek.ToString();
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            string tempScheduleQuery = $@"
    SELECT room 
    FROM temporary_schedules 
    WHERE instructor = '{currentUserId}'
        AND day = '{currentDay}'
        AND status = 'Approved'
        AND '{currentTime}' BETWEEN class_start AND class_end";

            DataTable tempResult = dbHelper.GetData(tempScheduleQuery);

            if (tempResult.Rows.Count > 0)
            {
                return tempResult.Rows[0]["room"].ToString();
            }

            string scheduleQuery = $@"
    SELECT room 
    FROM schedules 
    WHERE instructor = '{currentUserId}'
        AND day = '{currentDay}'
        AND '{currentTime}' BETWEEN class_start AND class_end";

            DataTable result = dbHelper.GetData(scheduleQuery);

            if (result.Rows.Count > 0)
            {
                return result.Rows[0]["room"].ToString();
            }
            return "No Schedule";
        }


        private async Task ConnectToAllSystemUnitsAsync(string lab)
        {
            try
            {
                string query = "SELECT hostname FROM system_units WHERE location = '" + lab + "'";
                DataTable systemUnitsData = dbHelper.GetData(query);

                CloseAllConnections();

                List<Task> connectionTasks = new List<Task>();

                foreach (DataRow row in systemUnitsData.Rows)
                {
                    string hostname = row["hostname"].ToString();

                    Panel hostPanel = new Panel
                    {
                        Size = new Size(250, 170),
                        Margin = new Padding(7),
                        Tag = new { Hostname = hostname, IsOnline = false }
                    };

                    PictureBox pictureBox = new PictureBox
                    {
                        Size = new Size(250, 145),
                        BorderStyle = BorderStyle.Fixed3D
                    };

                    CheckBox checkBox = new CheckBox
                    {
                        Size = new Size(60, 20) 
                    };

                    Label hostLabel = new Label
                    {
                        Text = hostname,
                        Size = new Size(150, 20),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Font = new Font("Montserrat", 9, FontStyle.Regular)
                    };

                    int totalWidth = hostPanel.Width;
                    int checkBoxWidth = checkBox.Width;
                    int labelWidth = hostLabel.Width;

                    int totalControlsWidth = checkBoxWidth + labelWidth;
                    int spacing = 1; 

                    checkBox.Location = new Point((totalWidth - totalControlsWidth - spacing) / 2, 150); 
                    hostLabel.Location = new Point(checkBox.Location.X + checkBoxWidth + spacing, 150); 

                    hostPanel.Click += HostPanel_Click;
                    pictureBox.Click += HostPanel_Click;
                    hostLabel.Click += HostPanel_Click;

                    hostPanel.Controls.Add(pictureBox);
                    hostPanel.Controls.Add(checkBox); 
                    hostPanel.Controls.Add(hostLabel);
                    flowLayoutPanel1.Controls.Add(hostPanel);

                    DisplayConnectingStatus(pictureBox);

                    connectionTasks.Add(ConnectToSystemUnitAsync(hostname, pictureBox, hostPanel));
                }

                await Task.WhenAll(connectionTasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection error: " + ex.Message);
            }
        }


        private void HostPanel_Click(object sender, EventArgs e)
        {
            Control clickedControl = sender as Control;
            Panel clickedPanel = clickedControl as Panel ?? clickedControl.Parent as Panel;

            if (clickedPanel != null)
            {
                var tagInfo = (dynamic)clickedPanel.Tag;
                string hostname = tagInfo.Hostname;
                bool isOnline = tagInfo.IsOnline;

                if (!isOnline)
                {
                    MessageBox.Show("The device is offline.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Fullscreen fullscreenForm = new Fullscreen();
                fullscreenForm.SetHostname(hostname);
                fullscreenForm.Show();
            }
        }
        private async Task ConnectToSystemUnitAsync(string hostname, PictureBox pictureBox, Panel hostPanel)
        {
            try
            {
                TcpClient client = new TcpClient();
                await client.ConnectAsync(hostname, Port);

                NetworkStream stream = client.GetStream();
                clients.Add(client);
                streams.Add(stream);

                hostPanel.Tag = new { Hostname = hostname, IsOnline = true };

                await Task.Run(() => ReceiveImages(stream, pictureBox));
            }
            catch (Exception)
            {
                hostPanel.Tag = new { Hostname = hostname, IsOnline = false };
                DisplayOfflineImage(pictureBox);
            }
        }

        private async Task ReceiveImages(NetworkStream stream, PictureBox pictureBox)
        {
            try
            {
                while (true) 
                {
                    byte[] lengthBytes = new byte[4];
                    int bytesRead = await stream.ReadAsync(lengthBytes, 0, lengthBytes.Length);
                    if (bytesRead < 4) throw new Exception("Failed to read image length.");

                    int imageLength = BitConverter.ToInt32(lengthBytes, 0);
                    if (imageLength <= 0) throw new Exception("Invalid image length received.");

                    byte[] imageBytes = new byte[imageLength];
                    int totalBytesRead = 0;

                    while (totalBytesRead < imageLength)
                    {
                        bytesRead = await stream.ReadAsync(imageBytes, totalBytesRead, imageLength - totalBytesRead);
                        if (bytesRead == 0) break; 
                        totalBytesRead += bytesRead;
                    }

                    if (totalBytesRead != imageLength) throw new Exception("Incomplete image data received.");

                    using (var ms = new MemoryStream(imageBytes))
                    {
                        var bitmap = new Bitmap(ms);
                        Invoke(new Action(() => DisplayImage(bitmap, pictureBox)));
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving image: " + ex.Message);
            }
        }


        private void DisplayImage(Bitmap bitmap, PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
            }

            var resizedBitmap = ResizeImage(bitmap, pictureBox.Width, pictureBox.Height);
            pictureBox.Image = resizedBitmap;
        }

        private Bitmap ResizeImage(Bitmap originalImage, int width, int height)
        {
            var ratioX = (double)width / originalImage.Width;
            var ratioY = (double)height / originalImage.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(originalImage.Width * ratio);
            var newHeight = (int)(originalImage.Height * ratio);

            var newBitmap = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return newBitmap;
        }

        private void DisplayConnectingStatus(PictureBox pictureBox)
        {
            Bitmap connectingImage = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(connectingImage))
            {
                g.Clear(Color.LightGray);

                string text = "Connecting...";
                Font font = new Font("Montserrat", 13, FontStyle.Regular);
                SizeF textSize = g.MeasureString(text, font);

                PointF textPosition = new PointF(
                    (pictureBox.Width - textSize.Width) / 2,
                    (pictureBox.Height - textSize.Height) / 2
                );

                g.DrawString(text, font, Brushes.Black, textPosition);
            }
            pictureBox.Image = connectingImage;
        }

        private void DisplayOfflineImage(PictureBox pictureBox)
        {
            Bitmap offlineImage = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(offlineImage))
            {
                g.Clear(Color.Gray);

                string text = "Offline";
                Font font = new Font("Montserrat", 13, FontStyle.Regular);
                SizeF textSize = g.MeasureString(text, font);

                PointF textPosition = new PointF(
                    (pictureBox.Width - textSize.Width) / 2,
                    (pictureBox.Height - textSize.Height) / 2
                );

                g.DrawString(text, font, Brushes.White, textPosition);
            }
            pictureBox.Image = offlineImage;
        }

        private void CloseAllConnections()
        {
            foreach (var stream in streams)
            {
                try
                {
                    stream?.Close();  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error closing stream: " + ex.Message);
                }
            }

            foreach (var client in clients)
            {
                try
                {
                    client?.Close();  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error closing client: " + ex.Message);
                }
            }

            streams.Clear();
            clients.Clear();

            flowLayoutPanel1.Controls.Clear();
        }
        private void messageButton_Click(object sender, EventArgs e)
        {
            List<string> onlineCheckedHostnames = new List<string>();
            List<string> offlineWorkstations = new List<string>();

            foreach (Panel hostPanel in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                CheckBox checkBox = hostPanel.Controls.OfType<CheckBox>().FirstOrDefault();

                if (checkBox != null && checkBox.Checked)
                {
                    var tagInfo = (dynamic)hostPanel.Tag;
                    string hostname = tagInfo.Hostname;
                    bool isOnline = tagInfo.IsOnline;

                    if (isOnline)
                    {
                        onlineCheckedHostnames.Add(hostname);
                    }
                    else
                    {
                        offlineWorkstations.Add(hostname);
                    }
                }
            }

            if (offlineWorkstations.Count > 0)
            {
                MessageBox.Show("The following workstations are offline and cannot receive messages: "
                    + string.Join(", ", offlineWorkstations),
                    "Offline Workstations",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            if (onlineCheckedHostnames.Count > 0)
            {
                Message messageForm = new Message(onlineCheckedHostnames);
                messageForm.ShowDialog(this);
            }
            else if (offlineWorkstations.Count == 0)
            {
                MessageBox.Show("No workstation selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void titleLabel_Click(object sender, EventArgs e)
        {

        }

        private async void uc_remoteview_Load(object sender, EventArgs e)
        {
            string selectedLab = GetAssignedRoomForCurrentUser();
            if (!string.IsNullOrEmpty(selectedLab))
            {
                titleLabel.Text = $"Remote View - {selectedLab}";
                await ConnectToAllSystemUnitsAsync(selectedLab);
            }
            else
            {
                titleLabel.Text = "Remote View - No Schedule";
            }
        }

        private async void reconnectButton_Click(object sender, EventArgs e)
        {
            string selectedLab = GetAssignedRoomForCurrentUser();
            if (!string.IsNullOrEmpty(selectedLab))
            {
                titleLabel.Text = $"Remote View - {selectedLab}";
                await ConnectToAllSystemUnitsAsync(selectedLab);
            }
            else
            {
                titleLabel.Text = "Remote View - No Schedule";
            }
        }

        private void requestSchedule_Click(object sender, EventArgs e)
        {
            RequestSchedule requestSchedule = new RequestSchedule(instructorId);

            if (requestSchedule.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show("Request sent successfully.");
            }
        }

        private void shutdownButton_Click(object sender, EventArgs e)
        {
            List<string> selectedHostnames = new List<string>();

            foreach (Panel hostPanel in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                CheckBox checkBox = hostPanel.Controls.OfType<CheckBox>().FirstOrDefault();

                if (checkBox != null && checkBox.Checked)
                {
                    var tagInfo = (dynamic)hostPanel.Tag;
                    string hostname = tagInfo.Hostname;
                    bool isOnline = tagInfo.IsOnline;

                    if (isOnline)
                    {
                        selectedHostnames.Add(hostname);
                    }
                }
            }

            if (selectedHostnames.Count == 0)
            {
                MessageBox.Show("No online workstation selected to shutdown.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Are you sure you want to shutdown the selected workstations: {string.Join(", ", selectedHostnames)}?",
                "Confirm Shutdown",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmResult == DialogResult.Yes)
            {
                foreach (string hostname in selectedHostnames)
                {
                    SendShutdownCommand(hostname);
                }
            }
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            List<string> selectedHostnames = new List<string>();

            foreach (Panel hostPanel in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                CheckBox checkBox = hostPanel.Controls.OfType<CheckBox>().FirstOrDefault();

                if (checkBox != null && checkBox.Checked)
                {
                    var tagInfo = (dynamic)hostPanel.Tag;
                    string hostname = tagInfo.Hostname;
                    bool isOnline = tagInfo.IsOnline;

                    if (isOnline)
                    {
                        selectedHostnames.Add(hostname);
                    }
                }
            }

            if (selectedHostnames.Count == 0)
            {
                MessageBox.Show("No online workstation selected to restart.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Are you sure you want to restart the selected workstations: {string.Join(", ", selectedHostnames)}?",
                "Confirm Restart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmResult == DialogResult.Yes)
            {
                foreach (string hostname in selectedHostnames)
                {
                    SendRestartCommand(hostname);
                }
            }
        }

        private void lockButton_Click(object sender, EventArgs e)
        {
            List<string> selectedHostnames = new List<string>();

            foreach (Panel hostPanel in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                CheckBox checkBox = hostPanel.Controls.OfType<CheckBox>().FirstOrDefault();

                if (checkBox != null && checkBox.Checked)
                {
                    var tagInfo = (dynamic)hostPanel.Tag;
                    string hostname = tagInfo.Hostname;
                    bool isOnline = tagInfo.IsOnline;

                    if (isOnline)
                    {
                        selectedHostnames.Add(hostname);
                    }
                }
            }

            if (selectedHostnames.Count == 0)
            {
                MessageBox.Show("No online workstation selected to lock.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (string hostname in selectedHostnames)
            {
                if (!workstationLockStates.ContainsKey(hostname) || !workstationLockStates[hostname]) 
                {
                    SendLockCommand(hostname);
                    workstationLockStates[hostname] = true; 
                }
                else
                {
                    MessageBox.Show($"The workstation {hostname} is already locked.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void SendLockCommand(string hostname)
        {
            try
            {
                using (TcpClient client = new TcpClient(hostname, 12345))
                using (NetworkStream stream = client.GetStream())
                {
                    string command = "LOCK";
                    byte[] commandBytes = Encoding.UTF8.GetBytes(command);
                    byte[] lengthBytes = BitConverter.GetBytes(commandBytes.Length);

                    stream.WriteByte(1);
                    stream.Write(lengthBytes, 0, lengthBytes.Length);
                    stream.Write(commandBytes, 0, commandBytes.Length);
                }
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.HostNotFound)
            {
                MessageBox.Show($"The workstation {hostname} is offline.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending lock command to {hostname}: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SendShutdownCommand(string hostname)
        {
            try
            {
                using (TcpClient client = new TcpClient(hostname, 12345))
                using (NetworkStream stream = client.GetStream())
                {
                    string command = "SHUTDOWN";
                    byte[] commandBytes = Encoding.UTF8.GetBytes(command);

                    byte[] lengthBytes = BitConverter.GetBytes(commandBytes.Length);
                    stream.WriteByte(1);
                    stream.Write(lengthBytes, 0, lengthBytes.Length);
                    stream.Write(commandBytes, 0, commandBytes.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending shutdown command to {hostname}: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendRestartCommand(string hostname)
        {
            try
            {
                using (TcpClient client = new TcpClient(hostname, 12345))
                using (NetworkStream stream = client.GetStream())
                {
                    string command = "RESTART";
                    byte[] commandBytes = Encoding.UTF8.GetBytes(command);

                    byte[] lengthBytes = BitConverter.GetBytes(commandBytes.Length);
                    stream.WriteByte(1);
                    stream.Write(lengthBytes, 0, lengthBytes.Length);
                    stream.Write(commandBytes, 0, commandBytes.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending restart command to {hostname}: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void unlockButton_Click(object sender, EventArgs e)
        {
            List<string> selectedHostnames = new List<string>();

            foreach (Panel hostPanel in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                CheckBox checkBox = hostPanel.Controls.OfType<CheckBox>().FirstOrDefault();

                if (checkBox != null && checkBox.Checked)
                {
                    var tagInfo = (dynamic)hostPanel.Tag;
                    string hostname = tagInfo.Hostname;
                    bool isOnline = tagInfo.IsOnline;

                    if (isOnline)
                    {
                        selectedHostnames.Add(hostname);
                    }
                }
            }

            if (selectedHostnames.Count == 0)
            {
                MessageBox.Show("No online workstation selected to unlock.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (string hostname in selectedHostnames)
            {
                if (workstationLockStates.ContainsKey(hostname) && workstationLockStates[hostname]) // If locked
                {
                    SendUnlockCommand(hostname);
                    workstationLockStates[hostname] = false; // Mark as unlocked
                }
                else
                {
                    MessageBox.Show($"The workstation {hostname} is already unlocked.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void SendUnlockCommand(string hostname)
        {
            try
            {
                using (TcpClient client = new TcpClient(hostname, 12345))
                using (NetworkStream stream = client.GetStream())
                {
                    string command = "UNLOCK";
                    byte[] commandBytes = Encoding.UTF8.GetBytes(command);
                    byte[] lengthBytes = BitConverter.GetBytes(commandBytes.Length);

                    stream.WriteByte(1);
                    stream.Write(lengthBytes, 0, lengthBytes.Length);
                    stream.Write(commandBytes, 0, commandBytes.Length);
                }
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.HostNotFound)
            {
                MessageBox.Show($"The workstation {hostname} is offline.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending unlock command to {hostname}: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
