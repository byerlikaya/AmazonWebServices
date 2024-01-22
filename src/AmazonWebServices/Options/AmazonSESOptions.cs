namespace AmazonWebServices.Options;

// ReSharper disable once InconsistentNaming
public class AmazonSESOptions
{
    public string ConfigurationSetName { get; set; }

    public SmtpOptions SmtpOptions { get; set; }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class SmtpOptions
{
    public string Host { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }
}