using System;
using System.Collections.Generic;
using System.Text;

namespace TestRabbitMQ
{
    /// <summary>
    /// MqDAO作为控制器、知识源之间信息交换的通道，仅需要创建一个MqDAO对象，并实现一个委托函数，注册事件即可
    /// </summary>
    class MqDAO
    {
        private Producer dirctProducer = new Producer();
        private Consumer directConsumer;
        private Queue<String> msgList = new Queue<string>();

        public delegate void DaoNewMsgHandler(object sender, EventArgs e);

        public event DaoNewMsgHandler Alarm;

        const string EXCHANGE = "all";
        public string mykey;
        /// <summary>
        /// 统一路由器，不同key
        /// </summary>
        public MqDAO(String mykey)
        {
            this.mykey = mykey;
            Console.WriteLine(mykey + " can send");
            if (directConsumer == null)
            {
                directConsumer = new Consumer();
                directConsumer.setDirectMode(EXCHANGE, mykey);
                directConsumer.start();
                Console.WriteLine(mykey + " start listening");
                directConsumer.NewMsg += new Consumer.NewMsgHandler(DAOgetNewMsg);
            }
        }
        ///<summary>
        ///从Consumer取回到DAO,同时往外push出去
        /// </summary>
        void DAOgetNewMsg(object sender, EventArgs e)
        {
            Console.WriteLine(mykey+" get the new msg in queue");
            String msg_tmp = directConsumer.getAStringMsg();
            msgList.Enqueue(msg_tmp);
            Console.WriteLine(this.mykey+" get "+msg_tmp);
            if (this.Alarm != null) {
                Console.WriteLine(this.mykey + " gonna push new msg out");
                this.Alarm(this, new EventArgs());
            }
        }

        public void send(String target, String content)
        {
            if (dirctProducer != null)
            {
                dirctProducer.TestDirectSendOnce(EXCHANGE, target, content+" from "+this.mykey);
            }
        }
        /// <summary>
        /// 对于DAO要轮巡检查
        /// </summary>
        /// <returns></returns>
        public string getmsg()
        {
            if (this.msgList != null & this.msgList.Count != 0)
            {
                return this.msgList.Dequeue();
            }
            else {
                return null;
            }
        }
    }
}
