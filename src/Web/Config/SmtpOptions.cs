using System;
namespace Web.Config
{
    public class SmtpOptions
    {
        public string? Host { get; set; }
        public string? ApiKey { get; set; }
        public string? SecretKey { get; set; }
        public int Port { get; set; }
    }
}

