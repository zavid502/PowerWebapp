using Blazored.LocalStorage;
using Data;
using Microsoft.Extensions.Logging;
using Models;

namespace Logic;

public class AccountService(DataService data, ILocalStorageService localStorage, ILogger logger)
{
    public async Task<(bool success, string? errorMessage)> ChangeUsername(string newUsername)
    {
        TokensModel? tokens = await data.ApiRequest<TokensModel>("account/username", "patch", true, new()
        {
            { "username", newUsername }
        });

        if (tokens is null)
        {
            logger.LogError($"Failed to change account name! {data.GetRequestError()}");
            return (false, data.GetRequestError());
        }

        await SetTokens(tokens);

        return (true, null);
    }
    
    public async Task<(bool success, string? errorMessage)> ChangeEmail(string newEmail)
    {
        TokensModel? tokens = await data.ApiRequest<TokensModel>("account/email", "patch", true, new()
        {
            { "email", newEmail }
        });

        if (tokens is null)
        {
            logger.LogError($"Failed to change account email! {data.GetRequestError()}");
            return (false, data.GetRequestError());
        }

        await SetTokens(tokens);

        return (true, null);
    }
    
    public async Task<(bool success, string? errorMessage)> ChangePassword(string oldPassword, string newPassword)
    {
        TokensModel? tokens = await data.ApiRequest<TokensModel>("account/password", "patch", true, new()
        {
            { "oldPassword", oldPassword },
            { "newPassword", newPassword }
        });

        if (tokens is null)
        {
            logger.LogError($"Failed to change account password! {data.GetRequestError()}");
            return (false, data.GetRequestError());
        }

        await SetTokens(tokens);

        return (true, null);
    }
    
    public async Task<(bool success, string? errorMessage)> GetNewTokens()
    {
        string refreshToken = await localStorage.GetItemAsync<string>("token");
        var tokens = await data.ApiRequest<TokensModel>("account/refreshtokens", "patch", true, new()
        {
            { "refreshToken", refreshToken }
        });

        if (tokens is null)
        {
            logger.LogError($"Failed to refresh tokens! {data.GetRequestError()}");
            return (false, data.GetRequestError());
        }

        await SetTokens(tokens);

        return (true, null);
    }

    private async Task SetTokens(TokensModel tokens)
    {
        await localStorage.SetItemAsync(Constants.LocalTokenName, tokens.Token);
        await localStorage.SetItemAsync(Constants.LocalRefreshTokenName, tokens.RefreshToken);
    }

    private async Task<TokensModel?> GetTokens()
    {
        if (!await TokensExist())
        {
            return null;
        }

        TokensModel model = new TokensModel()
        {
            Token = await localStorage.GetItemAsync<string>(Constants.LocalTokenName),
            RefreshToken = await localStorage.GetItemAsync<string>(Constants.LocalRefreshTokenName)
        };

        return model;
    }

    private async Task<bool> TokensExist()
    {
        bool tokenExists = await localStorage.ContainKeyAsync(Constants.LocalTokenName);
        bool refreshTokenExists = await localStorage.ContainKeyAsync(Constants.LocalRefreshTokenName);
        
        return tokenExists & refreshTokenExists;
    }
    
}