using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace IPAddressFinder
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private PictureBox pictureBox;
        private Label lblHeader;
        private TextBox txtIpAddress;
        private Button btnAction;
        private Label lblFooter;
        private bool isIpDisplayed = false;

        public MainForm()
        {
            this.Text = "Public Private IP Address Finder";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            pictureBox = new PictureBox
            {
                Image = Image.FromFile("output-onlinepngtools.png"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(300, 150),
                Location = new Point((this.ClientSize.Width - 300) / 2, 30)
            };
            this.Controls.Add(pictureBox);

            lblHeader = new Label
            {
                Text = "Tokat Gaziosmanpasa University Information Technology Department",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkBlue,
                Size = new Size(400, 30),
                Location = new Point((this.ClientSize.Width - 400) / 2, 200)
            };
            this.Controls.Add(lblHeader);

            txtIpAddress = new TextBox
            {
                ReadOnly = true,
                Text = "",
                Font = new Font("Arial", 12),
                TextAlign = HorizontalAlignment.Center,
                Multiline = true,
                Size = new Size(400, 100),
                Location = new Point((this.ClientSize.Width - 400) / 2, 250)
            };
            this.Controls.Add(txtIpAddress);

            btnAction = new Button
            {
                Text = "Find My IP Address",
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightBlue,
                ForeColor = Color.DarkBlue,
                Size = new Size(150, 40),
                Location = new Point((this.ClientSize.Width - 150) / 2, 370)
            };
            btnAction.Click += BtnAction_Click;
            this.Controls.Add(btnAction);

            lblFooter = new Label
            {
                Text = "2024 Summer Internship Students",
                Font = new Font("Arial", 12, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkGray,
                Size = new Size(400, 30),
                Location = new Point((this.ClientSize.Width - 400) / 2, 450)
            };
            this.Controls.Add(lblFooter);

            this.Resize += (sender, e) => CenterControls();
        }

        private async void BtnAction_Click(object sender, EventArgs e)
        {
            if (!isIpDisplayed)
            {
                try
                {

                    string localIP = Dns.GetHostAddresses(Dns.GetHostName())
                        .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        ?.ToString();

                    string publicIP = await GetPublicIPAddressAsync();

                    if (!string.IsNullOrEmpty(localIP) && !string.IsNullOrEmpty(publicIP))
                    {
                        txtIpAddress.Text = $"Private IP Address: {localIP}\r\nPublic IP Address: {publicIP}";
                        btnAction.Text = "Close";
                        isIpDisplayed = true;
                    }
                    else
                    {
                        MessageBox.Show("IP addresses could not be retrieved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Application.Exit();
            }
        }

        private async System.Threading.Tasks.Task<string> GetPublicIPAddressAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync("https://api.ipify.org");
                    return response.Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving the public IP address: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void CenterControls()
        {
            pictureBox.Location = new Point((this.ClientSize.Width - pictureBox.Width) / 2, 30);
            lblHeader.Location = new Point((this.ClientSize.Width - lblHeader.Width) / 2, 200);
            txtIpAddress.Location = new Point((this.ClientSize.Width - txtIpAddress.Width) / 2, 250);
            btnAction.Location = new Point((this.ClientSize.Width - btnAction.Width) / 2, 370);
            lblFooter.Location = new Point((this.ClientSize.Width - lblFooter.Width) / 2, 450);
        }
    }
}
