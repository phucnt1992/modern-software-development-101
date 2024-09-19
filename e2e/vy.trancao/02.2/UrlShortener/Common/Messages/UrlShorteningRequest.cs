using System.ComponentModel.DataAnnotations;

namespace Common.Messages;

public class UrlShorteningRequest(string url)
{
    [Required]
    public string Url { get; set; } = url;
}
