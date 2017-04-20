using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using Newtonsoft.Json;

using HTTPLoot.LootHandling;
using HTTPLoot.Models;

namespace HTTPLoot.RequestHandling
{
    static class Methods
    {
        public static void Loot(HttpListenerContext context)
        {
            byte[] response;
            if (context.Request.HttpMethod == "GET")
            {
                try
                {
                    response = Encoding.UTF8.GetBytes(LootHandler.Instance.GetLootJSON());
                }
                catch
                {
                    response = Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(
                            new LootWrapper() { Looter = null, LootItem = "No Matching Loot found" }
                            )
                        );
                }
            }
            else
                response = Encoding.UTF8.GetBytes("HTTP Method Not Supported");

            context.Response.OutputStream.Write(response, 0, response.Length);
            context.Response.OutputStream.Close();
        }

        public static void Config(HttpListenerContext context)
        {
            byte[] response;
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(LootHandler.Config, Formatting.Indented));
                    break;
                case "POST":
                    using (StreamReader sr = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        try
                        {
                            LootHandler.Config = JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());

                            response = Encoding.UTF8.GetBytes("Successfully Changed Config");
                        }
                        catch
                        {
                            response = Encoding.UTF8.GetBytes("An Error Occurred, Your Config Sux");
                        }
                    }
                    break;
                default:
                    response = Encoding.UTF8.GetBytes("HTTP Method Not Supported");
                    break;
            }
            context.Response.OutputStream.Write(response, 0, response.Length);
            context.Response.OutputStream.Close();
        }
    }
}
