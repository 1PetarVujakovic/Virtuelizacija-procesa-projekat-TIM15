using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Load
    {
        private int id;
        private string timeStamp;
        private double forecastValue;
        private double measuredValue;
        private DateTime creationTime;

        public Load(int id, string timeStamp, double forecastValue, double measuredValue)
        {
            this.id = id;
            this.timeStamp = timeStamp;
            this.forecastValue = forecastValue;
            this.measuredValue = measuredValue;
            this.creationTime = DateTime.Now;
        }
        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public string TimeStamp { get => timeStamp; set => timeStamp = value; }
        [DataMember]
        public double ForecastValue { get => forecastValue; set => forecastValue = value; }
        [DataMember]
        public double MeasuredValue { get => measuredValue; set => measuredValue = value; }
        public DateTime CreationTime { get => creationTime; set => creationTime = value; }

        public override string ToString()
        {
            return $"Id: {id} | Vreme: {timeStamp} | Predvidjena vrednost: {forecastValue} | Izmerena vrednost: {measuredValue};";
        }
    }
}
