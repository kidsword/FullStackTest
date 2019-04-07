using FullStackTeste.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FullStackTeste.Robot
{
    class Program
    {
        private const string template = "messages_{0}.json";
        private static string filename = "messages_{0}.json";
        private static object locker = new object();

        static void Main(string[] args)
        {
            filename = string.Format(template, DateTime.Now.ToString("yyyyMMdd"));
            Console.WriteLine($"*** Json file: {filename}");

            var receiver = RabbitMQ.Helper.Receiver
                .Create("localhost", "statistic")
                .SetCallback((o, msg) =>
                {
                    var fullFilename = GetFullFileName();
                    SaveNewMessage(fullFilename, msg);



                })
                .Start();

            

            Console.WriteLine(" Press [enter] to stop robot.");
            Console.WriteLine();
            Console.ReadKey();

            receiver.Dispose();
        }

        private static string GetFullFileName()
        {
            var fullPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "files");
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
            return Path.Combine(fullPath, filename);
        }

        private static void SaveNewMessage(string fullfilename, string message)
        {
            lock (locker)
            {
                string allMessages = "";
                List<Statistics> msgList;

                if (File.Exists(fullfilename))
                {
                    allMessages = File.ReadAllText(fullfilename);
                    msgList = JsonConvert.DeserializeObject<List<Statistics>>(allMessages);
                }
                else
                    msgList = new List<Statistics>();

                var msg = JsonConvert.DeserializeObject<Statistics>(message);
                msgList.Add(msg);

                allMessages = JsonConvert.SerializeObject(msgList);
                File.WriteAllText(fullfilename, allMessages);
            }
        }
    }
}