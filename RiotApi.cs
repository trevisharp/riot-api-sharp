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
        if (string.IsNullOrEmpty(gameName))
            throw new ArgumentNullException(nameof(gameName));

        if (string.IsNullOrEmpty(tagLine))
            throw new ArgumentNullException(nameof(tagLine));

        if (tagLine.Contains("#"))
            tagLine = tagLine.Replace("#", "");

        if (limiter is not null)
            await limiter.ControlRequest();

        var response = await get(
            "/riot/account/v1/accounts/by-riot-id", 
            gameName, tagLine
        );

        if (!response.IsSuccessStatusCode)
            throw new RequestErrorException(response.StatusCode, "");
        var obj = await response.Content.ReadFromJsonAsync<Player>();
        return obj.Puuid;
    }

    public async Task<string[]> GetPlayerMatches(
        string playerId
    ) => await GetPlayerMatches(playerId, null, null);

    public async Task<string[]> GetPlayerMatches(
        string playerId, int? count
    ) => await GetPlayerMatches(playerId, null, count);

    public async Task<string[]> GetPlayerMatches(
        string playerId, int? start, int? count
    )
    {
        if (string.IsNullOrEmpty(playerId))
            throw new ArgumentNullException(nameof(playerId));
        
        if (limiter is not null)
            await limiter.ControlRequest();

        var response = await get(
            "/lol/match/v5/matches/by-puuid", 
            playerId, "ids", 
            q("start", start), q("count", count)
        );

        if (!response.IsSuccessStatusCode)
            throw new RequestErrorException(response.StatusCode, "");
        return await response.Content.ReadFromJsonAsync<string[]>();
    }

    public async Task<Match> GetMatch(string matchId)
    {
        if (string.IsNullOrEmpty(matchId))
            throw new ArgumentNullException(nameof(matchId));
        
        if (limiter is not null)
            await limiter.ControlRequest();

        var response = await get(
            "/lol/match/v5/matches/", matchId
        );

        if (!response.IsSuccessStatusCode)
            throw new RequestErrorException(response.StatusCode, "");
        return await response.Content.ReadFromJsonAsync<Match>();
    }

    private async Task<HttpResponseMessage> get(params object[] data)
    {
        var path = buildPath(data);
        return await http.GetAsync(path);
    }

    internal class QueryParameter
    {
        public string Value { get; set; }
    }

    private QueryParameter q(string key, object value)
    {
        if (value is null)
            return null;
        
        return new() { Value = $"{key}={format(value)}" };
    }

    private string format(object value)
        => value switch
        {
            QueryParameter p => p.Value,
            _ => value?.ToString()
        };

    private string buildPath(params object[] data)
    {
        var values = data
            .Append(q("api_key", this.ApiKey))
            .Where(data => data is not null);
        
        var pathEl = values
            .TakeWhile(p => p is not QueryParameter)
            .Select(format)
            .ToArray();
        var path = Path.Combine(pathEl);

        var queryEl = values
            .SkipWhile(p => p is not QueryParameter)
            .Select(format)
            .ToArray();
        if (queryEl.Length == 0)
            return path;
        
        return path + "?" + string.Join('&', queryEl);
    }
}