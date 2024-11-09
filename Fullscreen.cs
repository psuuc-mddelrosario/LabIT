using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratory_Management_System
{
    public partial class Fullscreen : Form
    {
        private string hostname;
        private const int Port = 12345; 
        private TcpClient client;
        private NetworkStream stream;

        public Fullscreen()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; 
            this.KeyDown += Fullscreen_KeyDown;           
        }

        public void SetHostname(string hostname)
        {
            this.hostname = hostname;
            DisplayContent();
        }

        private async void DisplayContent()
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(hostname, Port);
                stream = client.GetStream();

                await ReceiveImagesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to " + hostname + ": " + ex.Message);
            }
        }

        private async Task ReceiveImagesAsync()
        {
            try
            {
                while (true)
                {
                    var lengthBytes = new byte[4];
                    int bytesRead = await stream.ReadAsync(lengthBytes, 0, lengthBytes.Length);
                    if (bytesRead < lengthBytes.Length) break;

                    var imageLength = BitConverter.ToInt32(lengthBytes, 0);
                    var imageBytes = new byte[imageLength];

                    bytesRead = 0;
                    while (bytesRead < imageLength)
                    {
                        int read = await stream.ReadAsync(imageBytes, bytesRead, imageLength - bytesRead);
                        if (read == 0) break;
                        bytesRead += read;
                    }

                    using (var ms = new MemoryStream(imageBytes))
                    {
                        if (ms.Length > 0)
                        {
                            try
                            {
                                var bitmap = new Bitmap(ms);

                                // Check if the form handle is created and valid before invoking
                                if (this.IsHandleCreated)
                                {
                                    Invoke(new Action(() => DisplayImage(bitmap))); // Pass bitmap to display
                                }
                                else
                                {
                                    // Form handle is not valid; we should stop receiving images
                                    break;
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show("Received invalid image data: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Received empty image data.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Prevent message boxes if the form is closing
                if (this.IsHandleCreated)
                {
                    MessageBox.Show("Error receiving image: " + ex.Message);
                }
            }
        }

        private void DisplayImage(Bitmap bitmap)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }

            // Resize the image to fit the screen resolution
            var resizedBitmap = new Bitmap(bitmap, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            pictureBox1.Image = resizedBitmap;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // or PictureBoxSizeMode.Zoom
        }


        private void Fullscreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void Fullscreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stream != null)
            {
                stream.Close();
            }

            if (client != null)
            {
                client.Close();
            }
        }
    }
}
