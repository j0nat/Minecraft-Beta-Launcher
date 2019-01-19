using System;
using System.Windows.Controls;

namespace Minecraft.Pages
{
    /// <summary>
    /// Interaction logic for Webbrowser.xaml
    /// </summary>
    public partial class Webbrowser : Page
    {
        private System.Windows.Forms.WebBrowser wb;

        public Webbrowser()
        {
            //WebBrowserHelper.FixBrowserVersion();

            InitializeComponent();

            wb = new System.Windows.Forms.WebBrowser();
            FormsHost.Child = wb;
            wb.ScriptErrorsSuppressed = true;

            wb.Navigate("about:blank");

            System.Windows.Forms.Button btn = button();
            wb.Controls.Add(btn);
            btn.BringToFront();
        }

        private System.Windows.Forms.Button button()
        {
            System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
            btn.Text = "BACK ->";
            btn.Size = new System.Drawing.Size(115, 25);
            btn.Location = new System.Drawing.Point(0, 0);
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btn.ForeColor = System.Drawing.Color.White;
            btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            btn.Font = new System.Drawing.Font("SimHei", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btn.Click += new EventHandler(btn_Click);

            return btn;
        }

        void btn_Click(object sender, EventArgs e)
        {
            PAGE_STATES.MENU_GRID.Height = 75;
            PAGE_STATES.NAV_FRAME.Navigate(PAGE_STATES.PAGE_HOST);
        }

        public void navigate(string url)
        {
            if (wb.Url.AbsoluteUri == "about:blank")
            {
                wb.Navigate(url);
            }

            Minecraft.PAGE_STATES.MENU_GRID.Height = 0;
        }
    }
}
