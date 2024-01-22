namespace AmazonWebServices.Requests;

public class SendEmailRequest
{
    public string DisplayName { get; set; }

    public string Source { get; set; }

    public Destination Destination { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class Destination
{
    public List<string> ToAddresses { get; set; }

    public List<string> BccAddresses { get; set; }

    public List<string> CcAddresses { get; set; }
}