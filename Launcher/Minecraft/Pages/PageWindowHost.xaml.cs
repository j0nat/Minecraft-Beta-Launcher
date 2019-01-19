using System.Windows.Controls;

namespace Minecraft.Pages
{
    /// <summary>
    /// Interaction logic for PageWindowHost.xaml
    /// </summary>
    public partial class PageWindowHost : Page
    {
        StationWindow MC_Window;

        public PageWindowHost()
        {
            InitializeComponent();
        }

        public void loadWindow(string Name)
        {
            MC_Window = new StationWindow(FormsHost);
            MC_Window.LoadClient(Name);
        }

        public void close()
        {
            try
            {
                MC_Window.Close();
            }
            catch
            {
            }
        }
    }
}
