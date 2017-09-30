using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace SLS.Score.Task
{
    public class Message
    {
        private string messageType = "";

        public Message(string messagetype)
        {
            messageType = messagetype;
        }

        public void Send(string msg)
        {
            string queuePath = ".\\private$\\SLS_Allcai_Task_Alipay_" + messageType;

            if (!MessageQueue.Exists(queuePath))
            {
                return;
            }

            MessageQueue mq = null;

            try
            {
                mq = new MessageQueue(queuePath);
            }
            catch
            {
                return;
            }

            if (mq == null)
            {
                return;
            }

            System.Messaging.Message m = new System.Messaging.Message();

            m.Body = msg;
            m.Formatter = new System.Messaging.BinaryMessageFormatter();

            mq.Send(m);
        }
    }
}
