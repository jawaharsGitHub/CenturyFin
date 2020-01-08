using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Common
{
    public static class WhatsAppMessage
    {

        public static void SendMsg()
        {
            var inputData = new Data()
            {
                apikey = "Q/X7XUVJIFA-9mayd3VcxvwV1H9acgNZ9a0VLtFttR",
                data = new Messages()
                {
                    send_channel = "whatsapp",
                    messages = new List<Message>() {

                        new Message()
                        {
                            number = 919566807986,
                            template = new Template()
                            {
                                Id = 475067

                            }

                        }
                    }
                }
            };

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.textlocal.in/bulk_json");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(inputData, Formatting.Indented);

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }








        }

    }


    public class Data
    {
        public Messages data { get; set; }
        public string apikey { get; set; } = "Q/X7XUVJIFA-9mayd3VcxvwV1H9acgNZ9a0VLtFttR";
    }

    public class Messages
    {
        public List<Message> messages { get; set; }
        public string send_channel { get; set; } = "whatsapp";
    }

    public class Message
    {
        public long number { get; set; }
        public Template template { get; set; }
    }

    public class Template
    {
        public int Id { get; set; }
        public object MergeFields { get; set; }
    }
}
