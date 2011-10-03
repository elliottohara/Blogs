using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;
using Amazon.Auth.AccessControlPolicy;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Amazon.SQS.Model;
using Amazon.SimpleNotificationService.Model;
using NUnit.Framework;
using Attribute = Amazon.SQS.Model.Attribute;

namespace AwsFun
{
    [TestFixture]
    public class amazon_stuff
    {
        private string _key;
        private string _secret;

        private void GetUserNameAndPassword()
        {
            var streamReader = new StreamReader(@"C:\SpecialSuperSecret\elliott.ohara@gmail.com.txt");
            var dictionary = new Dictionary<string, string>();
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                var keyValue = line.Split(':');
                dictionary.Add(keyValue[0],keyValue[1]);
            }
            _key = dictionary["UserName"];
            _secret = dictionary["Password"];
        }
        [Test]
        public void create_a_topic()
        {
            GetUserNameAndPassword();
            var snsClient = Amazon.AWSClientFactory.CreateAmazonSNSClient(_key, _secret);
            var createTopicRequest = new CreateTopicRequest()
                .WithName("Elliott-has-an-awesome-blog");
            var response = snsClient.CreateTopic(createTopicRequest);
            Trace.WriteLine(response.CreateTopicResult.TopicArn);
            

        }
        [Test]
        public void publish_a_message()
        {
            GetUserNameAndPassword();
            var snsClient = Amazon.AWSClientFactory.CreateAmazonSNSClient(_key, _secret);
            var listTopicsRequest = new ListTopicsRequest();
            var listTopicsResponse = snsClient.ListTopics(listTopicsRequest);
            var topicArn = string.Empty;
            foreach (var result in listTopicsResponse.ListTopicsResult.Topics)
            {
                if (result.TopicArn.EndsWith("Elliott-has-an-awesome-blog"))
                {
                    topicArn = result.TopicArn;
                    break;
                }
            }
            var publishMessageRequest = new PublishRequest()
                .WithMessage("Hello fro, the Amazon cloud")
                .WithSubject("Elliott has an amazing blog")
                .WithTopicArn(topicArn);
            snsClient.Publish(publishMessageRequest);

        }
        [Test]
        public void create_sqs_queue()
        {
            GetUserNameAndPassword();
            var sqsClient = Amazon.AWSClientFactory.CreateAmazonSQSClient(_key, _secret);
            var snsTopic = Amazon.AWSClientFactory.CreateAmazonSNSClient(_key, _secret);
            var topicArn = "arn:aws:sns:us-east-1:451419498740:Elliott-has-an-awesome-blog";
            //Create a new SQS queue
            var createQueueRequest = new CreateQueueRequest().WithQueueName("elliotts-blog");
            var createQueueResponse = sqsClient.CreateQueue(createQueueRequest);
            // keep the queueUrl handy
            var queueUrl = createQueueResponse.CreateQueueResult.QueueUrl;
            // get the Access Resource Name so we can allow the SNS to put messages on it
            var getQueueArnRequest = new GetQueueAttributesRequest()
                .WithQueueUrl(queueUrl)
                .WithAttributeName("QueueArn");
            var getQueueArnResponse = sqsClient.GetQueueAttributes(getQueueArnRequest);
            var queueArn = getQueueArnResponse.GetQueueAttributesResult.Attribute[0].Value;

            //create a Policy for the SQS queue that allows SNS to publish to it
            var allowSnsStatement = new Statement(Statement.StatementEffect.Allow)
                .WithPrincipals(Principal.AllUsers)
                .WithResources(new Resource(queueArn))
                .WithConditions(ConditionFactory.NewSourceArnCondition(topicArn))
                .WithActionIdentifiers(SQSActionIdentifiers.SendMessage);
            var policy = new Policy("allow sns").WithStatements(new[] {allowSnsStatement});
            var attribute = new Attribute().WithName("Policy").WithValue(policy.ToJson());
            var setQueueAttributesRequest =
                new SetQueueAttributesRequest().WithQueueUrl(queueUrl).WithAttribute(attribute);
            sqsClient.SetQueueAttributes(setQueueAttributesRequest);

            // ok, now lets create the subscription for sqs with the queueArn we created
            var subscribeRequest = new SubscribeRequest()
                .WithEndpoint(queueArn)
                .WithTopicArn(topicArn)
                .WithProtocol("sqs");

            snsTopic.Subscribe(subscribeRequest);




        }
        [Test]
        public void pull_from_sqs_queue()
        {
            GetUserNameAndPassword();
            var sqsClient = Amazon.AWSClientFactory.CreateAmazonSQSClient(_key, _secret);
            var queueUrl =
                sqsClient.ListQueues(new ListQueuesRequest().WithQueueNamePrefix("elliotts-blog")).ListQueuesResult.
                    QueueUrl[0];
           
            var timeToRunThisTest = new TimeSpan(0, 0, 0, 5);
            var stopAt = DateTime.Now.Add(timeToRunThisTest);
            while(DateTime.Now < stopAt)
            {
                var recieveMessageRequest = new ReceiveMessageRequest().WithQueueUrl(queueUrl);
                var recieveMessageResult = sqsClient.ReceiveMessage(recieveMessageRequest);
                foreach (var message in recieveMessageResult.ReceiveMessageResult.Message)
                {
                    Trace.WriteLine(message.Body);
                }
            }
        }
    }

}
