using System;
using System.IO;

namespace Minecraft
{
    class AccountLoader
    {
        public static void LoadAccount(out string username, out string password)
        {
            string file = GetFileLocation();

            try
            {
                if (File.Exists(file))
                {
                    string[] account = File.ReadAllText(file).Split('\n');

                    username = account[0];
                    password = account[1];
                }
                else
                {
                    username = "";
                    password = "";
                }
            }
            catch
            {
                username = "";
                password = "";
            }
        }

        public static void SaveAccount(string username, string password)
        {
            string file = GetFileLocation();

            try
            {
                File.WriteAllText(file, username + "\n" + password);
            }
            catch
            {

            }
        }

        private static string GetFileLocation()
        {
            string executableLocation = AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(executableLocation, "account.txt");
        }
    }
}
