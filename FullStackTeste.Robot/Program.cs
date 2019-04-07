using FullStackTeste.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                    //Save in json file
                    var fullFilename = GetFullFileName();
                    SaveNewMessage(fullFilename, msg);
                    //Save in SQL
                    var objMsg = JsonConvert.DeserializeObject<Statistics>(msg);
                    SaveNewMessage(objMsg);
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
        /// <summary>
        /// Save the message in json file
        /// </summary>
        /// <param name="fullfilename"></param>
        /// <param name="message"></param>
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


        private static void SaveNewMessage(Statistics msg)
        {
            using (var con = new SqlConnection())
            {
                try
                {
                    con.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=FullStackTest;Integrated Security=True";
                    con.Open();

                    SqlCommand command = con.CreateCommand();
                    SqlTransaction transaction;

                    transaction = con.BeginTransaction("fullStackTransation");

                    command.Connection = con;
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText = "Statistic_Save";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@IP", msg.IP ));
                        command.Parameters.Add(new SqlParameter("@Page", msg.Page));

                        int? StatisticId = (int?)command.ExecuteScalar();

                        command.Dispose();

                        if (StatisticId.HasValue)
                        {
                            foreach(var param in msg.Parameters)
                            {
                                command = new SqlCommand("Statistic_Parameters_Save", con, transaction);
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@StatisticId", StatisticId.GetValueOrDefault(0)));
                                command.Parameters.Add(new SqlParameter("@Name", param.Name ));
                                command.Parameters.Add(new SqlParameter("@Value", param.Value ));
                                command.ExecuteNonQuery();
                                command.Dispose();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fail to save at SQL: {ex.Message}");
                }
            }
            
        }
    }
}