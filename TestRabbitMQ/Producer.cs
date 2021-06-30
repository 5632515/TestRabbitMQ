using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace TestRabbitMQ
{
    class Producer
    {
        /// <summary>
        /// 发送计数器
        /// </summary>
        private int send_cnt = 0;

        /// <summary>
        /// MQ主机名
        /// </summary>
        const string Hostname = "45.11.2.149";

        /// <summary>
        /// MQ用户名
        /// </summary>
        const string UserName = "root"; //用户名

        /// <summary>
        /// MQ密码
        /// </summary>
        const string Password = "123456"; //密码

        /// <summary>
        /// 工厂模式
        /// </summary>
        ConnectionFactory factory;

        public Producer()
        {

        }


        /// <summary>
        /// 这是一个测试接口，正式使用应该带队列名称、发送内容等
        /// </summary>
        //public void TestQueueSendOnce(string queueName, string content)
        //{
        //    if (factory == null)
        //    {
        //        factory = new ConnectionFactory();
        //        factory.HostName = Hostname;
        //        factory.UserName = UserName;
        //        factory.Password = Password;
        //    }

        //    using (var connection = factory.CreateConnection())
        //    {
        //        using (var channel = connection.CreateModel())
        //        {
        //            channel.QueueDeclare(queueName, false, false, false, null); //创建一个名称为hello的消息队列
        //            send_cnt++;
        //            string message = content + send_cnt.ToString(); //传递的消息内容
        //            var body = Encoding.UTF8.GetBytes(message);
        //            //byte[] data = new byte[] {0x30, 0x31, 0x32 };
        //            channel.BasicPublish("", queueName, null, body); //开始传递

        //        }
        //    }
        //}

        /// <summary>
        /// 广播模式发送一次
        /// </summary>
        //public void TestFanSendOnce(string fanName)
        //{
        //    if (factory == null)
        //    {
        //        factory = new ConnectionFactory();
        //        factory.HostName = Hostname;
        //        factory.UserName = UserName;
        //        factory.Password = Password;
        //    }

        //    using (var connection = factory.CreateConnection())
        //    {
        //        using (var channel = connection.CreateModel())
        //        {
        //            var queueName = channel.QueueDeclare().QueueName;
        //            //使用fanout exchange type，指定exchange名称
        //            channel.ExchangeDeclare(exchange: fanName, type: "fanout");
        //            send_cnt++;
        //            var message = "广播消息  #" + send_cnt.ToString();
        //            var body = Encoding.UTF8.GetBytes(message);
        //            //发布到指定exchange，fanout类型无需指定routingKey
        //            channel.BasicPublish(exchange: fanName, routingKey: "", basicProperties: null, body: body);
        //        }
        //    }
        //}

        /// <summary>
        /// Direct模式发送一次
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        ///<param name="routingKey">路由Key</param>
        ///<param name="content">内容</param>
        public void TestDirectSendOnce(string exchangeName, string routingKey, string content)
        {
            if (factory == null)
            {
                factory = new ConnectionFactory();
                factory.HostName = Hostname;
                factory.UserName = UserName;
                factory.Password = Password;
            }

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var queueName = channel.QueueDeclare().QueueName;
                    //使用fanout exchange type，指定exchange名称
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
                    send_cnt++;
                    /// var message = content + send_cnt.ToString();
                    var message = content;
                    var body = Encoding.UTF8.GetBytes(message);
                    //发布到指定exchange，fanout类型无需指定routingKey
                    channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);
                }
            }
        }

        /// <summary>
        /// Topic模式发送一次
        /// </summary>
        //public void TestTopicSendOnce(string exchangeName, string routingKey)
        //{
        //    if (factory == null)
        //    {
        //        factory = new ConnectionFactory();
        //        factory.HostName = Hostname;
        //        factory.UserName = UserName;
        //        factory.Password = Password;
        //    }

        //    using (var connection = factory.CreateConnection())
        //    {
        //        using (var channel = connection.CreateModel())
        //        {
        //            var queueName = channel.QueueDeclare().QueueName;
        //            //使用fanout exchange type，指定exchange名称
        //            channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
        //            send_cnt++;
        //            var message = "Topic消息  #" + send_cnt.ToString();
        //            var body = Encoding.UTF8.GetBytes(message);
        //            //发布到指定exchange，fanout类型无需指定routingKey
        //            channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);
        //        }
        //    }
        //}
    }
}
