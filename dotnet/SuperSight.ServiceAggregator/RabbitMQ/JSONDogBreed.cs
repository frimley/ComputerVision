using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSight.ServiceAggregator.RabbitMQ
{
    class JSONDogBreed
    {
        public int top { get; set; }
	    public int right { get; set; }
	    public int bottom { get; set; }
	    public int left { get; set; }
	    public string breeds { get; set; }
	    public double process_time { get; set; }
        public int ReceivedTimeStamp { get; set; }
    }
}
