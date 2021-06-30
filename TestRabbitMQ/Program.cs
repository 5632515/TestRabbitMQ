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
    /// <summary>
    /// 用Host类模拟知识源，到时候知识源应该类似操作，不用在构造时传入，但一定要注册，写好处理函数
    /// </summary>
    class Host
    {
        public MqDAO mydao;

        public Host(MqDAO dao) {
            this.mydao = dao;
            dao.Alarm += new MqDAO.DaoNewMsgHandler(HostHandleAlarm);
        }
        void HostHandleAlarm(object sender, EventArgs e)
        {
            Console.WriteLine(this.mydao.mykey+"的dao上有新信息，我主动取出来："+this.mydao.getmsg());
        }
    }
    
 
    
    class Program
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// 
        
        static void Main(string[] args) {
            MqDAO dao1 = new MqDAO("aaa");
            Host host1 = new Host(dao1);
            MqDAO dao2 = new MqDAO("bbb");
            Host host2 = new Host(dao2);  ///一定要跟紧，如果是知识元，就是要马上把自己的dao上的alarm绑定好自己的处理函数，把dao上的数据拿出来
            MqDAO dao3 = new MqDAO("ccc");
            Host host3 = new Host(dao3);
            
            dao1.send("bbb", "hello");
            dao1.send("ccc", "hello");
            dao2.send("ccc","bye");
            dao3.send("bbb","bye");
            dao2.send("aaa","bye");
            dao3.send("aaa", "bye");
            ///host会把dao2里的东西push出来
            /// Console.WriteLine(dao2.getmsg()); 
        }
    }

   
}

