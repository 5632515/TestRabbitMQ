using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace TestRabbitMQ
{
    class Consumer : MyThread
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        private bool toRun = true;


        //private delegate void InvokeListBox(string strCon);    // 刷新ListBox的委托。
        //private InvokeListBox invokeListBox;

        /// <summary>
        /// MQ主机名
        /// </summary>
        const  string Hostname = "45.11.2.149";

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
        private ConnectionFactory factory;

        /// <summary>
        /// 监听的队列名称
        /// </summary>
        /// private string queueName;

        /// <summary>
        /// 监听的广播名称
        /// </summary>
        private string fanName;

        /// <summary>
        /// 显式队列消费者
        /// </summary>
        private EventingBasicConsumer consumer;

        /// <summary>
        /// MQ连接
        /// </summary>
        private IConnection connection;

        /// <summary>
        /// MQ通道
        /// </summary>
        private IModel channel;

        /// <summary>
        /// 监听到的消息
        /// </summary>
        private Queue<String> msgList = new Queue<string>();

        /// <summary>
        /// 事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NewMsgHandler(object sender, EventArgs e);
        /// <summary>
        /// 声明的事件，代表有新消息
        /// </summary>
        public event NewMsgHandler NewMsg;


        public Consumer()
        {
        }

        /// <summary>
        /// 启动监听器,每100ms监听一次
        /// </summary>
        public override void run()
        {
            while (toRun)
            {
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 停止监听MQ 
        /// </summary>
        public void stop()
        {
            if (toRun)
            {
                toRun = false;
                //Thread.Sleep(100);

                if (channel != null)
                {
                    channel.Close();
                    channel = null;
                }


                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }


        public override void initWork()
        {
        }

        /// <summary>
        /// 设置为单队列侦听消费者模式
        /// 侦听队列 queueName 中的消息
        /// 路由的名称留空
        /// </summary>
        /// <param name="queueName"></param>
        //public void setSingleQueueMode(string queueName)
        //{
        //    this.queueName = queueName;

        //    //如果改变运行模式，则先停止旧的监听
        //    if ((connection != null) || (channel != null))
        //    {
        //        stop();
        //    }


        //    factory = new ConnectionFactory();
        //    factory.HostName = Hostname;
        //    factory.UserName = UserName;
        //    factory.Password = Password;

        //    connection = factory.CreateConnection();

        //    channel = connection.CreateModel();

        //    channel.QueueDeclare(queueName, false, false, false, null);

        //    consumer = new EventingBasicConsumer(channel);
        //    //channel.BasicConsume("hello", false, consumer);
        //    channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
        //    //invokeListBox = new InvokeListBox(update_rtb_msg);
        //    consumer.Received += (model, ea) =>
        //    {
        //        var body = ea.Body;
        //        var message = Encoding.UTF8.GetString(body);
        //        msgList.Enqueue(message);
        //        //rtb_msg.Invoke(invokeListBox, new object[] { message });
        //        //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        //    };
        //}

        /// <summary>
        /// 设置为接收广播的消费者模式
        /// 接收名称为 exchangeName 的路由上的广播
        /// 不用指定队列名字
        /// </summary>
        /// <param name="fanName">路由名称</param>
        //public void setFanMode(string fanName)
        //{
        //    this.fanName = fanName;

        //    //如果改变运行模式，则先停止旧的监听
        //    if ((connection != null) || (channel != null))
        //    {
        //        stop();
        //    }


        //    factory = new ConnectionFactory();
        //    factory.HostName = Hostname;
        //    factory.UserName = UserName;
        //    factory.Password = Password;

        //    connection = factory.CreateConnection();

        //    channel = connection.CreateModel();

        //    //申明fanout类型exchange
        //    channel.ExchangeDeclare(exchange: fanName, type: "fanout");
        //    //申明随机队列名称
        //    var queuename = channel.QueueDeclare().QueueName;
        //    //绑定队列到指定fanout类型exchange，无需指定路由键
        //    channel.QueueBind(queue: queuename, exchange: fanName, routingKey: "");

        //    consumer = new EventingBasicConsumer(channel);
            
        //    channel.BasicConsume(queue: queuename, autoAck: true, consumer: consumer);
          
        //    consumer.Received += (model, ea) =>
        //    {
        //        var body = ea.Body;
        //        var message = Encoding.UTF8.GetString(body);
        //        msgList.Enqueue(message);
              
        //        //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        //    };
        //}

        /// <summary>
        /// Direct模式
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由Key</param>
        public void setDirectMode(string exchangeName, string routingKey)
        {
            this.fanName = exchangeName;

            //如果改变运行模式，则先停止旧的监听
            if ((connection != null) || (channel != null))
            {
                stop();
            }


            factory = new ConnectionFactory();
            factory.HostName = Hostname;
            factory.UserName = UserName;
            factory.Password = Password;

            connection = factory.CreateConnection();

            channel = connection.CreateModel();

            //申明fanout类型exchange
            channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
            //申明随机队列名称
            var queuename = channel.QueueDeclare().QueueName;
            //绑定队列到指定fanout类型exchange，无需指定路由键
            channel.QueueBind(queue: queuename, exchange: exchangeName, routingKey: routingKey);

            consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queue: queuename, autoAck: true, consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray(); 
                var message = Encoding.UTF8.GetString(body);
                msgList.Enqueue(message);
                Console.WriteLine("A new msg!");
                if (this.NewMsg != null) {
                    this.NewMsg(this, new EventArgs());
                }

                //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
        }

        /// <summary>
        /// Topic模式
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由Key</param>
        //public void setTopicMode(string exchangeName, string routingKey)
        //{
        //    this.fanName = exchangeName;

        //    //如果改变运行模式，则先停止旧的监听
        //    if ((connection != null) || (channel != null))
        //    {
        //        stop();
        //    }


        //    factory = new ConnectionFactory();
        //    factory.HostName = Hostname;
        //    factory.UserName = UserName;
        //    factory.Password = Password;

        //    connection = factory.CreateConnection();

        //    channel = connection.CreateModel();

        //    //申明fanout类型exchange
        //    channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
        //    //申明随机队列名称
        //    var queuename = channel.QueueDeclare().QueueName;
        //    //绑定队列到指定fanout类型exchange，无需指定路由键
        //    channel.QueueBind(queue: queuename, exchange: exchangeName, routingKey: routingKey);

        //    consumer = new EventingBasicConsumer(channel);

        //    channel.BasicConsume(queue: queuename, autoAck: true, consumer: consumer);

        //    consumer.Received += (model, ea) =>
        //    {
        //        var body = ea.Body;
        //        var message = Encoding.UTF8.GetString(body);
        //        msgList.Enqueue(message);

        //        //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        //    };
        //}

        /// <summary>
        /// 返回一个消息
        /// </summary>
        /// <returns></returns>
        public string getAStringMsg()
        {
            if (msgList != null)
            {
                if (msgList.Count > 0)
                {
                    return msgList.Dequeue();
                }
            }

            return null;
        }
    }
}