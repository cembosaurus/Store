﻿namespace Metrics.Models
{
    public class Service
    {
        public string Name { get; set; }
        //Id - for multiple K8 replicas of one service:
        public Guid Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public DateTime Deployed { get; set; }
        public DateTime Terminated { get; set; }


    }
}
