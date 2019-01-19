using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Client : ClientStart
{
    public IntPtr[] Window(string Name)
    {
        string usrName = Name;
        string binDir = Path.Combine(Environment.CurrentDirectory, "Data");
        string mcDir = Path.Combine(Environment.CurrentDirectory, ".minecraft");
        binDir = Path.Combine(binDir, @"bin\");

        try
        {
            SetAddress(mcDir);
        }
        catch (Exception e)
        {
            System.Windows.MessageBox.Show(e.ToString());
        }

        ProcessStartInfo sInfo = new ProcessStartInfo();
        sInfo.FileName = "javaw";
        sInfo.Arguments = String.Format("-Xmx1024M -Xms1024M -cp \"{0}minecraft.jar\";\"{0}lwjgl.jar\";\"{0}lwjgl_util.jar\" -Djava.library.path=\"{0}natives\" net.minecraft.client.Minecraft \"{1}\"", binDir, usrName);
        sInfo.WindowStyle = ProcessWindowStyle.Hidden;
        sInfo.UseShellExecute = false;
        sInfo.EnvironmentVariables["AppData"] = Environment.CurrentDirectory;

        Process proc = Process.Start(sInfo);
        proc.EnableRaisingEvents = true;
        proc.WaitForInputIdle();
        proc.Exited += Proc_Exited;
        
        bool isClientStarted = OpenClient();

        if (isClientStarted)
        {
            return GetWindow();
        }

        return null;
    }

    private void Proc_Exited(object sender, EventArgs e)
    {
        // Close window when minecraft closes.
        Environment.Exit(0);
    }

    public void SetAddress(string dir)
    {
        string file = File.ReadAllText(dir + @"\options.txt");
        string text = "";

        string[] split = file.Split('\n');

        foreach (string str in split)
        {
            if (str.Contains("lastServer"))
            {
                text += "lastServer:" + Minecraft.Networking.Network.ADDRESS.ToString() + Environment.NewLine;
            }
            else
            {
                if (str.Trim().Length != 0)
                {
                    text += str + Environment.NewLine;
                }
            }
        }

        File.WriteAllText(dir + @"\options.txt", text);
    }
}