using System;
using System.Windows;

using System.Xml;
using System.ComponentModel;

namespace Minecraft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool networkConnectionProblem = false;
        private string networkResponse = "";

        public MainWindow()
        {
            InitializeComponent();

            LoadXML();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MenuGrid.Height = 0;

            PAGE_STATES.PAGE_HOST = new Pages.PageWindowHost();
            PAGE_STATES.PAGE_CONNECT= new Pages.Connect();
            PAGE_STATES.PAGE_WB = new Pages.Webbrowser();
            PAGE_STATES.NAV_FRAME = frame;
            PAGE_STATES.MENU_GRID = MenuGrid;

            string username;
            string password;
            AccountLoader.LoadAccount(out username, out password);

            PAGE_STATES.PAGE_CONNECT.txtName.Text = username;
            PAGE_STATES.PAGE_CONNECT.txtPassword.Password = password;

            frame.Navigate(PAGE_STATES.PAGE_CONNECT);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PAGE_STATES.PAGE_HOST.close();
        }

        public static void LoadXML()
        {
            // Load XML document
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load("settings.xml");
                XmlNode node = doc.DocumentElement.SelectNodes("/config")[0];

                Networking.Network.PORT = Convert.ToInt32(node.SelectSingleNode("PORT").InnerText);
                Networking.Network.ADDRESS = System.Net.IPAddress.Parse(node.SelectSingleNode("ADDRESS").InnerText);
            }
            catch
            {
                MessageBox.Show("Unable to read your settings.xml", "Config error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowStyle == WindowStyle.SingleBorderWindow)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }

        private void btnRegisterRegion_Click(object sender, RoutedEventArgs e)
        {
            frmNewRegionInfo info = new frmNewRegionInfo();
            info.Show();
        }

        private void btnOpenMap_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(PAGE_STATES.PAGE_WB);
            PAGE_STATES.PAGE_WB.navigate("http://" + Networking.Network.ADDRESS.ToString() + ":8123");
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
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
                    MessageBox.Show("Successfully re-authenticated.", "Login success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show(networkResponse, "Server issues", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void login_request_DoWork(object sender, DoWorkEventArgs e)
        {
            Networking.NetworkRequest networkRequest = new Networking.NetworkRequest();

            networkResponse = networkRequest.Login(PAGE_STATES.PAGE_CONNECT.username, PAGE_STATES.PAGE_CONNECT.password, out networkConnectionProblem);
        }
    }
}
