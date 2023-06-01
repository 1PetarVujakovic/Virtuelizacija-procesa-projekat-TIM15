using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum MsgType { INFO, WARNING, ERROR}

    [DataContract]
    public class Audit
    {
        private int id;
        private string timeStamp;
        private MsgType messageType;
        private string message;

        public Audit(int id, string timeStamp, MsgType messageType, string message)
        {
            this.id = id;
            this.timeStamp = timeStamp;
            this.messageType = messageType;
            this.message = message;
        }

        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public string TimeStamp { get => timeStamp; set => timeStamp = value; }
        [DataMember]
        public MsgType MessageType { get => messageType; set => messageType = value; }
        [DataMember]
        public string Message { get => message; set => message = value; }
    }
}
