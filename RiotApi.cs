/* Date: 15/02/2024
 * Author: Leonardo Trevisan Silio
 */
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RiotApi;

using Jsons;
using Exceptions;

public class Api
{
    public readonly string ApiKey;
    private readonly HttpClient http;
    private readonly RateLimit limiter;

    public Api(ApiKey apiKey, Server server, RateLimit limiter = null)
    {
        this.limiter = limiter;

        this.ApiKey = apiKey.key;
        var service = server switch
        {
            Server.Americas => "americas",
            Server.Asia => "asia",
            Server.Europe => "europe",
            Server.Esports => "esports",
            Server.Sea => "sea",
            _ => throw new InvalidRegionException()
        };
        var url = $"https://{service}.api.riotgames.com/";
        this.http = new HttpClient{
            BaseAddress = new Uri(url)
        };
    }
    
    public async Task<string> GetPlayerId(string gameName, string tagLine)
    {
        if (limiter is not null)
            await limiter.ControlRequest();

        const string basePath = "/riot/account/v1/accounts/by-riot-id";
        if (string.IsNullOrEmpty(gameName))
            throw new ArgumentNullException(nameof(gameName));

        if (string.IsNullOrEmpty(tagLine))
            throw new ArgumentNullException(nameof(tagLine));

        if (tagLine.Contains("#"))
            tagLine = tagLine.Replace("#", "");

        var response = await get(basePath, gameName, tagLine);
        if (!response.IsSuccessStatusCode)
            throw new RequestErrorException(response.StatusCode, "");
        
        var obj = await response.Content.ReadFromJsonAsync<Player>();
        return obj.Puuid;
    }

    public async Task<string[]> GetPlayerMatches(string playerId)
    {
        if (limiter is not null)
            await limiter.ControlRequest();

        const string basePath = "/lol/match/v5/matches/by-puuid";
        if (string.IsNullOrEmpty(playerId))
            throw new ArgumentNullException(nameof(playerId));

        var response = await get(basePath, playerId, "ids");
        if (!response.IsSuccessStatusCode)
            throw new RequestErrorException(response.StatusCode, "");
        
        return await response.Content.ReadFromJsonAsync<string[]>();
    }
    
    private async Task<HttpResponseMessage> get(params object[] data)
    {
        var path = buildPath(data);
        return await http.GetAsync(path);
    }

    private string format(object value)
        => value switch
        {
            _ => value.ToString()
        };

    private string buildPath(params object[] data)
        => buildPath(data.Select(format).ToArray());

    private string buildPath(params string[] pathData)
    {
        var basePath = Path.Combine(pathData);
        return basePath + $"?api_key={this.ApiKey}";
    }

}