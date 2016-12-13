using System;
using System.IO;
using System.Text;
using System.Net;

using NLog;
using NLog.Config;
using NLog.Targets;
using Newtonsoft.Json;

namespace weatherForRA0UEK
{
    class Program
    {
        static Logger _logger;

        static void Main(string[] args)
        {
            ConfigureLogging();

            _logger.Info("---------------------------------------------------");
            _logger.Info("Start program");

            Settings.Read();

            if (string.IsNullOrEmpty(Settings.WeatherAddress) ||
                string.IsNullOrEmpty(Settings.FtpServer) ||
                string.IsNullOrEmpty(Settings.FtpUsername) ||
                string.IsNullOrEmpty(Settings.FtpPassword))
            {
                _logger.Error("ERROR!!! Missing argument exception. Check settings.txt file!");
                Environment.Exit(-1);
            }

            try
            {
                var jsonString = getJson();

                var weatherCurrent = JsonConvert.DeserializeObject<Weather>(jsonString);

                createFile("temperature.txt", weatherCurrent.Temperature);
                createFile("clouds.txt", weatherCurrent.Translations.Clouds);
                createFile("other.txt", weatherCurrent.Translations.Other);
                createFile("dewpoint.txt", weatherCurrent.Dewpoint);
                createFile("wind_speed.txt", weatherCurrent.WindSpeed);
                createFile("wind_gust.txt", weatherCurrent.WindGust);

                sendToFtp("temperature.txt", "clouds.txt", "other.txt", "dewpoint.txt", "wind_speed.txt", "wind_gust.txt");
                deleteFile("temperature.txt", "clouds.txt", "other.txt", "dewpoint.txt", "wind_speed.txt", "wind_gust.txt");
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "ERROR!!! " + ex.Message);
                Environment.Exit(-1);
            }

            _logger.Info("End program");
            _logger.Info("---------------------------------------------------");
        }

        static void sendToFtp(params string[] files)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(Settings.FtpUsername, Settings.FtpPassword);

                    foreach (var file in files)
                    {
                        _logger.Info($"Send file {file} to ftp {Settings.FtpServer}");

                        client.UploadFile($"ftp://{Settings.FtpServer}/{file}", "STOR", file);
                    }                    
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ERROR!!! " + ex.Message);
                Environment.Exit(-1);
            }
        }

        static void deleteFile(params string[] files)
        {
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        static void createFile(string fileName, string content)
        {
            try
            {
                _logger.Info($"Create file: {fileName} with content {content}");

                TextWriter tw = new StreamWriter(fileName, false, Encoding.Default);
                tw.WriteLine(content);
                tw.Close();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ERROR!!! " + ex.Message);
                Environment.Exit(-1);
            }
        }

        static string getJson()
        {
            try
            {
                WebClient client = new WebClient();
                string reply = client.DownloadString(Settings.WeatherAddress);

                return reply;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ERROR!!! " + ex.Message);
                Environment.Exit(-1);
                return null;
            }
        }

        static void ConfigureLogging()
        {
            var config = new LoggingConfiguration();

            var targetConsole = new ConsoleTarget();
            var targetFile = new FileTarget { FileName = "log.txt" };

            var ruleConsole = new LoggingRule("*", LogLevel.Debug, targetConsole);
            var ruleFile = new LoggingRule("*", LogLevel.Debug, targetFile);

            targetConsole.Layout = "${longdate}|${pad:padding=5:inner=${level:uppercase=true}}|${message}";
            targetFile.Layout = "${longdate}|${logger}|${message} ${exception:format=tostring}";

            config.AddTarget("console", targetConsole);
            config.AddTarget("file", targetFile);

            config.LoggingRules.Add(ruleConsole);
            config.LoggingRules.Add(ruleFile);

            LogManager.Configuration = config;
            _logger = LogManager.GetCurrentClassLogger();
        }
    }
}
