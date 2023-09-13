namespace AmazonWebServices.Options
{
    public class AmazonSESOptions
    {
        public string ConfigurationSetName { get; set; }

        public SmtpOptions SmtpOptions { get; set; }
    }

    public class SmtpOptions
    {
        public string Host { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
