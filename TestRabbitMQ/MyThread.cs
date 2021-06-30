using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestRabbitMQ
{
    public abstract class MyThread
    {
        /// <summary>
        /// 线程实体类
        /// </summary>
        private Thread thread = null;

        /// <summary>
        /// 具体的线程工作
        /// </summary>
        public abstract void run();

        /// <summary>
        /// 启动线程之前的准备工作
        /// </summary>
        public abstract void initWork();

        /// <summary>
        /// 启动线程
        /// </summary>
        public void start()
        {
            initWork();
            if (thread == null)
                thread = new Thread(run);
            thread.Start();
        }
    }
}
