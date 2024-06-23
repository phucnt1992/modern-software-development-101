using Common.Messages;
using Refit;

namespace WebServer.Clients;

public interface IUlvisApi
{
    [Get("/write/get?url={url}")]
    Task<UlvisResponse<UrlShorteningResponse>> ConvertUrl(string url);
}
