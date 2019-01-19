using System.Windows;
using System.Windows.Controls;

using System.ComponentModel;

namespace Minecraft.Pages
{
    /// <summary>
    /// Interaction logic for Connect.xaml
    /// </summary>
    public partial class Connect : Page
    {
        private bool networkConnectionProblem = false;
        private string networkResponse = "";

        public string username = "";
        public string password = "";

        public Connect()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Length < 2)
            {
                MessageBox.Show("Username too short", "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtPassword.Password.Length < 2)
            {
                MessageBox.Show("Password too short", "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            txtName.IsEnabled = false;
            txtPassword.IsEnabled = false;
            btnConnect.IsEnabled = false;

            username = txtName.Text;
            password = txtPassword.Password;

            BackgroundWorker login_request = new BackgroundWorker();
            login_request.DoWork += new DoWorkEventHandler(login_request_DoWork);
            login_request.RunWorkerCompleted += new RunWorkerCompletedEventHandler(login_request_RunWorkerCompleted);
            login_request.RunWorkerAsync("login");
        }

        void login_request_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!networkConnectionProblem)
            {
                if (networkResponse.Trim() == "LOGIN ERROR")
                {
                    MessageBox.Show("Username / Password wrong.", "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                if (networkResponse.Trim() == "LOGIN OK")
                {
                    // Save account
                    AccountLoader.SaveAccount(txtName.Text, txtPassword.Password);

                    PAGE_STATES.PAGE_HOST.loadWindow(txtName.Text);
                    PAGE_STATES.NAV_FRAME.Navigate(PAGE_STATES.PAGE_HOST);
                }
            }
            else
            {
                MessageBox.Show(networkResponse, "Server issues", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            txtName.IsEnabled = true;
            txtPassword.IsEnabled = true;
            btnConnect.IsEnabled = true;
        }

        void login_request_DoWork(object sender, DoWorkEventArgs e)
        {
            Networking.NetworkRequest networkRequest = new Networking.NetworkRequest();

            networkResponse = networkRequest.Login(PAGE_STATES.PAGE_CONNECT.username, PAGE_STATES.PAGE_CONNECT.password, out networkConnectionProblem);
        }

        private void btnOffline_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Length < 2)
            {
                PAGE_STATES.PAGE_HOST.loadWindow("Player");
                PAGE_STATES.NAV_FRAME.Navigate(PAGE_STATES.PAGE_HOST);
            }
            else
            {
                PAGE_STATES.PAGE_HOST.loadWindow(txtName.Text);
                PAGE_STATES.NAV_FRAME.Navigate(PAGE_STATES.PAGE_HOST);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Length < 2)
            {
                MessageBox.Show("Username too short", "Registration error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtPassword.Password.Length < 2)
            {
                MessageBox.Show("Password too short", "Registration error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            txtName.IsEnabled = false;
            txtPassword.IsEnabled = false;
            btnConnect.IsEnabled = false;

            username = txtName.Text;
            password = txtPassword.Password;

            BackgroundWorker register_request = new BackgroundWorker();
            register_request.DoWork += Register_request_DoWork;
            register_request.RunWorkerCompleted += Register_request_RunWorkerCompleted;
            register_request.RunWorkerAsync("register");
        }

        private void Register_request_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!networkConnectionProblem)
            {
                if (networkResponse.Trim().StartsWith("REGISTER ERROR "))
                {
                    string response = networkResponse.Replace("REGISTER ERROR ", "");
                    MessageBox.Show(response, "Registration error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                if (networkResponse.Trim() == "REGISTER OK")
                {
                    MessageBox.Show("Account successfully registered! You can now log in.", "Registration error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show(networkResponse, "Server issues", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            txtName.IsEnabled = true;
            txtPassword.IsEnabled = true;
            btnConnect.IsEnabled = true;
        }

        private void Register_request_DoWork(object sender, DoWorkEventArgs e)
        {
            Networking.NetworkRequest networkRequest = new Networking.NetworkRequest();

            networkResponse = networkRequest.Register(PAGE_STATES.PAGE_CONNECT.username, PAGE_STATES.PAGE_CONNECT.password, out networkConnectionProblem);
        }
    }
}
