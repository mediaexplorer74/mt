// Program

using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Windows.Forms;

namespace GameManager
{
    internal static class Program1
    {
        public const bool IsServer = false;
        public static Dictionary<string, string> LaunchParameters = new Dictionary<string, string>();
        public static void Main1(string[] args)
        {
            try
            {
                using (Main main = new Main())
                {
                    Program1.LaunchParameters = Utils.ParseArguements(args);
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].ToLower() == "-port" || args[i].ToLower() == "-p")
                        {
                            i++;
                            try
                            {
                                int listenPort = Convert.ToInt32(args[i]);
                                Netplay.ListenPort = listenPort;
                            }
                            catch { }
                        }
                        if (args[i].ToLower() == "-join" || args[i].ToLower() == "-j")
                        {
                            i++;
                            try
                            {
                                main.AutoJoin(args[i]);
                            }
                            catch { }
                        }
                        if (args[i].ToLower() == "-pass" || args[i].ToLower() == "-password")
                        {
                            i++;
                            Netplay.ServerPassword = args[i];
                            main.AutoPass();
                        }
                        if (args[i].ToLower() == "-host")
                        {
                            main.AutoHost();
                        }
                        if (args[i].ToLower() == "-loadlib")
                        {
                            i++;
                            string path = args[i];
                            main.loadLib(path);
                        }
                    }
                    main.Run();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //RnD
                    using (StreamWriter streamWriter = new StreamWriter(/*"client-crashlog.txt"*/default, 
                            /*true*/default))
                    {
                        streamWriter.WriteLine(DateTime.Now);
                        streamWriter.WriteLine(ex);
                        streamWriter.WriteLine("");
                    }
                    //RnD
                    //MessageBox.Show(ex.ToString(), "Terraria: Error");
                }
                catch { }
            }
        }
    }
}
