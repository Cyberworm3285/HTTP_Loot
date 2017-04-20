using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using Loot3Framework.ExtensionMethods.CollectionOperations;
using Loot3Framework.Types.Classes.Algorithms.Looting;
using Loot3Framework.Types.Classes.Algorithms.Filter;
using Loot3Framework.Types.Classes.RarityTables;
using Loot3Framework.Types.Exceptions;
using Newtonsoft.Json;

using HTTPLoot.LootHandling;
using HTTPLoot.Models;
using static HTTPLoot.RequestHandling.Methods;

namespace HTTPLoot
{
    class Program
    {
        private static HttpListener listener;
        private static Dictionary<string, Action<HttpListenerContext>> URL_Map;
        private const string BaseURL = "http://127.0.0.1:1337";

        static void Main(string[] args)
        {
            Console.WriteLine("All lootable Items/Rarities:");
            int c = 1;
            LootHandler.Instance.AllLoot.DoAction(i => Console.WriteLine(c++ + ": " + i.Name + "/" + i.RarityName));

            URL_Map = new Dictionary<string, Action<HttpListenerContext>>(StringComparer.CurrentCultureIgnoreCase)
            {
                { "/Loot/", Loot },
                { "/Config/", Config },
            };

            listener = new HttpListener();
            listener.Prefixes.Add(BaseURL + "/");
            listener.Start();
            listener.BeginGetContext(new AsyncCallback(ContextReceivedCallback), null);
            Console.WriteLine("Server started successfully");

            string input = Console.ReadLine();
            for(;;)
            {
                switch (input)
                {
                    case "end":
                        return;
                    case "loot":
                        Console.WriteLine(LootHandler.Instance.GetLoot(new PR_PartionLoot<string, PartitionLoot<string>>(DefaultRarityTable.SharedInstance, PartitionLoot<string>.SharedInstance)));
                        break;
                }
                input = Console.ReadLine();
            } 
        }

        private static void ContextReceivedCallback(IAsyncResult asyncResult)
        {
            HttpListenerContext context = listener.EndGetContext(asyncResult);
            listener.BeginGetContext(new AsyncCallback(ContextReceivedCallback), null);
            ProcessRequest(context);
        }

        public static void ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine(context.Request.Url);
            try
            {
                URL_Map[context.Request.RawUrl](context);
            }
            catch (Exception)
            {
                byte[] error = Encoding.UTF8.GetBytes("Invalid URL");
                context.Response.OutputStream.Write(error, 0, error.Length);
                context.Response.OutputStream.Close();
            }
        }
    }
}
