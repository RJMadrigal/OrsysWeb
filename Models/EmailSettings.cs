﻿namespace SistemaOrdenes.Models
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Password { get; set; }
    }
}
