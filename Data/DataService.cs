using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;

namespace Data;

public class DataService(ILocalStorageService localStorage, ILogger logger)
{
    private readonly JsonSerializer _serializer = new();
    private HttpResponseMessage? _response;
    
    private const string ApiUrl = "https://lammers.me/api/";

    public string GetRequestError()
    {
        string error = $"{_response?.StatusCode} -- {_response?.ReasonPhrase}";
        return error;
    }

    public async Task<T?> ApiRequest<T>(string endpoint, string requestType, bool authenticated, Dictionary<string, string>? requestData = null, Dictionary<string, string?>? requestGetParams = null,
        bool returnModel = true)
    {
        var requestUrl = ApiUrl + endpoint;
        
        using HttpClient? client = await ApiHttpClientFactory(authenticated);

        if (client is null)
        {
            logger.LogError("ApiRequest error -- ApiHttpClient factory returned null.");
            return default;
        }
        
        _response = await GetApiResponse(client, requestUrl, requestType, requestData, requestGetParams);
        
        if (!_response.IsSuccessStatusCode)
        {
            logger.LogError($"ApiRequest error -- {_response.StatusCode} - {_response.ReasonPhrase}");
            return default;
        }

        if (!returnModel) return default;
        
        var responseBody = await _response.Content.ReadAsStringAsync();

        var model = await ResponseToModel<T>(responseBody);

        if (model is null)
        {
            logger.LogError("ApiRequest error -- failed to cast API json response to model.");
        }

        return model;
    }

    private async Task<T?> ResponseToModel<T>(string response)
    {
        using var stringReader = new StringReader(response);
        await using var jsonReader = new JsonTextReader(stringReader);

        var apiResponse = _serializer.Deserialize<T>(jsonReader);

        return apiResponse;
    }

    private async Task<HttpClient?> ApiHttpClientFactory(bool authenticated)
    {
        HttpClient client = new();

        if (!authenticated) return client;
        
        if (!await AuthTokensInStorage())
        {
            logger.LogError("Could not create authorized HttpClient because authorization tokens are missing!");
            return null;
        }

        string token = await localStorage.GetItemAsync<string>("token");
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    private async Task<HttpResponseMessage> GetApiResponse(HttpClient client, string requestUrl, string requestType,
        Dictionary<string, string>? requestData = null, Dictionary<string, string?>? requestGetParams= null)
    {
        HttpResponseMessage response;
        
        switch (requestType.ToLower())
        {
            case "get":
                string url = QueryHelpers.AddQueryString(requestUrl, requestGetParams ?? new Dictionary<string, string?>());
                response = await client.GetAsync(url);
                break;
            case "post":
                response = await client.PostAsync(requestUrl, new FormUrlEncodedContent(requestData ?? new()));
                break;
            case "patch":
                response = await client.PatchAsync(requestUrl, new FormUrlEncodedContent(requestData ?? new()));
                break;
            default:
                response = await client.GetAsync(requestUrl);
                break;
        }
        return response;
    }
    
    private async Task<bool> AuthTokensInStorage()
    {
        bool tokensAvailable = await localStorage.ContainKeyAsync("token") & await localStorage.ContainKeyAsync("refreshToken");
        return tokensAvailable;
    }
}