using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft
{
    public static class PAGE_STATES
    {
        public static Pages.Connect PAGE_CONNECT { get; set; }
        public static Pages.PageWindowHost PAGE_HOST { get; set; }
        public static Pages.Webbrowser PAGE_WB { get; set; }

        public static System.Windows.Controls.Grid MENU_GRID { get; set; }
        public static System.Windows.Controls.Frame NAV_FRAME { get; set; }
    }
}
