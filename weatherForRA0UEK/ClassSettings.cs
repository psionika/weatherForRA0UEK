using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLog;

namespace weatherForRA0UEK
{
    static class Settings
    {
        static string weatherAddress;
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static public string WeatherAddress 
        {
            get
            {
                return weatherAddress;
            }
        }

        static string ftpServer;
        static public string FtpServer
        {
            get
            {
                return ftpServer;
            }
        }

        static string ftpUsername;
        static public string FtpUsername
        {
            get
            {
                return ftpUsername;
            }
        }

        static string ftpPassword;
        static public string FtpPassword
        {
            get
            {
                return ftpPassword;
            }
        }

        public static void Read()
        {
            try
            {
                string[] lines = File.ReadAllLines("settings.txt");

                if (lines.Count() == 4)
                {
                    weatherAddress = lines[0];
                    ftpServer = lines[1];
                    ftpUsername = lines[2];
                    ftpPassword = lines[3];
                }
                else
                {
                    _logger.Error("ERROR!!! Bad settings.txt file!");
                    Environment.Exit(-1);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ERROR!!! " + ex.Message);
                Environment.Exit(-1);
            }
        }
    }
}
