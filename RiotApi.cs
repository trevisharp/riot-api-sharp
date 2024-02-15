/* Date: 15/02/2024
 * Author: Leonardo Trevisan Silio
 */
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RiotApi;

using Jsons;
using Exceptions;

public class Api
{
    private readonly HttpClient http;

    public readonly string ApiKey;

    public Api(ApiKey apiKey, Server server)
    {
        this.ApiKey = apiKey.key;
        var service = server switch
        {
            Server.Americas => "americas",
            Server.Asia => "asia",
            Server.Europe => "europe",
            Server.Esports => "esports",
            _ => throw new InvalidRegionException()
        };
        var url = $"https://{service}.api.riotgames.com/";
        this.http = new HttpClient{
            BaseAddress = new Uri(url)
        };
    }
    
    public async Task<string> GetPlayerId(string gameName, string tagLine)
    {
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
        
        var json = await response.Content.ReadFromJsonAsync<Player>();
        return json.Puuid;
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