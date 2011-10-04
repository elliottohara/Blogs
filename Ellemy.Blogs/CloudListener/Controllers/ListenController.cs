using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amazon.SQS.Model;

namespace CloudListener.Controllers
{
    public class PubSubSettingsFromFile 
    {
        private readonly Dictionary<string, string> _dictionary;

        public PubSubSettingsFromFile(string fileName)
        {
            var lines = new List<String>();
            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
                reader.Close();
            }
            _dictionary = lines.ToDictionary(line => line.Substring(0, line.IndexOf(':')),
                                             line => line.Substring(line.IndexOf(':') + 1), StringComparer.InvariantCultureIgnoreCase);

        }

        public string UserName
        {
            get { return _dictionary["UserName"]; }
        }

        public string Password
        {
            get { return _dictionary["Password"]; }
        }

        public string ApplicationName
        {
            get { return _dictionary["ApplicationName"]; }
        }
    }
    public class ListenController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Poll()
        {
            var settings = new PubSubSettingsFromFile(@"C:\SpecialSuperSecret\elliott.ohara@gmail_web.com.txt");
            var client = Amazon.AWSClientFactory.CreateAmazonSQSClient(settings.UserName, settings.Password);
            var queueUrl =
                client.ListQueues(new ListQueuesRequest().WithQueueNamePrefix("elliotts-blog")).ListQueuesResult.
                    QueueUrl[0];
            var messages = new List<Message>();
            
            foreach (var message  in client.ReceiveMessage(new ReceiveMessageRequest().WithQueueUrl(queueUrl)).ReceiveMessageResult.Message)
            {
                messages.Add(message);
                client.DeleteMessage(
                    new DeleteMessageRequest().WithQueueUrl(queueUrl).WithReceiptHandle(message.ReceiptHandle));
            }
            return Json(messages);
        }

    }
}
