using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

class ClientStart
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll")]
    static extern int GetWindowText(int hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern IntPtr SetFocus(HandleRef hWnd);

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    const uint WM_KEYDOWN = 0x0100;
    const uint WM_KEYUP = 0x0101;
    const int BM_CLICK = 0x00F5;
    const int KEY_SPACEBAR = 0x20;

    public bool OpenClient()
    {
        try
        {
            Sleep(1500);

            while (!isHandleLoaded())
            {
                Sleep(50);
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public void Sleep(int time)
    {
        Thread doSleep = new Thread(new ParameterizedThreadStart(SleepWaiter));
        doSleep.Start(time);
        while (doSleep.IsAlive)
        {
            // Do nothing!
        }
    }

    public void SleepWaiter(object time)
    {
        Thread.Sleep(Convert.ToInt32(time));
    }

    public IntPtr[] GetWindow()
    {
        while (true)
        {
            IntPtr[] Window = findWindow();

            if (Window != null)
            {
                return Window;
            }
        }
    }

    public IntPtr[] findWindow()
    {
        IntPtr frame = FindWindow("SunAwtFrame", "Minecraft");
        IntPtr root_canvas = FindWindowEx(frame, IntPtr.Zero, "SunAwtCanvas", (string)null);

        if (frame.ToInt32() == 0)
        {
            return null;
        }

        IntPtr[] frames = new IntPtr[2];
        frames[0] = root_canvas;
        frames[1] = frame;

        return frames;
    }

    private void clickHandle(string Caption)
    {
        IntPtr frame = FindWindow("SunAwtFrame", (string)null);
        IntPtr root_canvas = FindWindowEx(frame, IntPtr.Zero, "SunAwtCanvas", (string)null);
        IntPtr canvas = FindWindowEx(root_canvas, IntPtr.Zero, "SunAwtCanvas", (string)null);

        StringBuilder caption = new StringBuilder();
        List<IntPtr> child_canvas = GetAllChildrenWindowHandles(canvas, 100);

        foreach (IntPtr child in child_canvas)
        {
            List<IntPtr> list_handles = GetAllChildrenWindowHandles(child, 100);

            for (int i = 0; i < list_handles.Count; ++i)
            {
                GetWindowText(list_handles[i].ToInt32(), caption, 255);

                if (caption.ToString().ToLower() == Caption.ToLower())
                {
                    SetForegroundWindow(list_handles[i]);

                    SendMessage(list_handles[i], BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                    Sleep(150);
                    PostMessage(frame, WM_KEYDOWN, (IntPtr)KEY_SPACEBAR, IntPtr.Zero);
                    PostMessage(frame, WM_KEYUP, (IntPtr)KEY_SPACEBAR, IntPtr.Zero);

                    return;
                }
            }
        }
    }

    private bool isHandleLoaded()
    {
        IntPtr frame = FindWindow("SunAwtFrame", (string)null);
        if (frame.ToInt32() == 0)
        {
            return false;
        }

        IntPtr root_canvas = FindWindowEx(frame, IntPtr.Zero, "SunAwtCanvas", (string)null);
        if (root_canvas.ToInt32() == 0)
        {
            return false;
        }

        return true;
    }

    static List<IntPtr> GetAllChildrenWindowHandles(IntPtr hParent, int maxCount)
    {
        List<IntPtr> result = new List<IntPtr>();
        int ct = 0;
        IntPtr prevChild = IntPtr.Zero;
        IntPtr currChild = IntPtr.Zero;
        while (true && ct < maxCount)
        {
            currChild = FindWindowEx(hParent, prevChild, null, null);
            if (currChild == IntPtr.Zero) break;
            result.Add(currChild);
            prevChild = currChild;
            ++ct;
        }
        return result;
    }
}