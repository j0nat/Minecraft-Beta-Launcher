using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Runtime.InteropServices;

class StationWindow
{
    #region DLL-IMPORTS
    [DllImport("USER32.DLL")]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll")]
    static extern IntPtr SetFocus(IntPtr hWnd);

    [DllImport("User32")]
    private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll")]
    static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    #endregion

    #region CONST
    private const int SWP_NOZORDER = 0x0004;
    private const int SWP_NOACTIVATE = 0x0010;
    private const int GWL_STYLE = -16;
    private const int WS_CAPTION = 0x00C00000;
    private const int WS_THICKFRAME = 0x00040000;
    private const UInt32 WM_CLOSE = 0x0010;
    #endregion

    private System.Windows.Forms.Panel WindowHostPanel;
    private IntPtr[] Windows;

    public StationWindow(System.Windows.Forms.Integration.WindowsFormsHost WinForm)
    {
        WindowHostPanel = new System.Windows.Forms.Panel();
        WinForm.Child = WindowHostPanel;

        WindowHostPanel.SizeChanged += new EventHandler(WindowHostPanel_SizeChanged);
    }

    public void SetWindowFocus()
    {
        SetFocus(Windows[0]);
    }

    private void WindowHostPanel_SizeChanged(object sender, EventArgs e)
    {
        MoveWindow(Windows[1], 0, 0, WindowHostPanel.Width, WindowHostPanel.Height, true);
    }

    public void Close()
    {
        try
        {
            SendMessage(Windows[1], WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            SendMessage(Windows[0], WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        catch
        {

        }
    }

    public void LoadClient(string Name)
    {
        Client Game = new Client();
        Windows = Game.Window(Name);

        SetParent(Windows[0], WindowHostPanel.Handle);

        int style = GetWindowLong(Windows[1], GWL_STYLE);
        style = style & ~WS_CAPTION & ~WS_THICKFRAME & ~SWP_NOZORDER & ~SWP_NOACTIVATE;
        SetWindowLong(Windows[1], GWL_STYLE, style);

        MoveWindow(Windows[1], 0, 0, WindowHostPanel.Width, WindowHostPanel.Height, true);

        SetWindowFocus();

        ShowWindow(Windows[1], 0);

        System.Windows.Forms.Button btn = button();
        WindowHostPanel.Controls.Add(btn);
        btn.BringToFront();
    }

    private System.Windows.Forms.Button button()
    {
        System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
        btn.Text = "MENU";
        btn.Size = new System.Drawing.Size(115, 25);
        btn.Location = new System.Drawing.Point(0, 0);
        btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        btn.ForeColor = System.Drawing.Color.White;
        btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
        btn.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        btn.Click += new EventHandler(btn_Click);
        
        return btn;
    }

    void btn_Click(object sender, EventArgs e)
    {
        if (Minecraft.PAGE_STATES.MENU_GRID.Height == 0)
        {
            Minecraft.PAGE_STATES.MENU_GRID.Height = 75;
        }
        else
        {
            Minecraft.PAGE_STATES.MENU_GRID.Height = 0;
        }
    }
}