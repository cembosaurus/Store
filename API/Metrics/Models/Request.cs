﻿namespace Metrics.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Path { get; set; }

    }
}
