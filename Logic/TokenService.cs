using Models;
using Blazored.LocalStorage;
namespace Logic;

public class StorageService(ILocalStorageService localStorage)
{
    public async Task<TokensModel?> GetTokens()
    {
        if (!await TokensExist())
        {
            return null;
        }

        var model = new TokensModel()
        {
            Token = await localStorage.GetItemAsync<string>(Constants.LocalTokenName),
            RefreshToken = await localStorage.GetItemAsync<string>(Constants.LocalRefreshTokenName)
        };

        return model;
    }
    
    public async Task<bool> TokensExist()
    {
        var tokenExists = await localStorage.ContainKeyAsync(Constants.LocalTokenName);
        var refreshTokenExists = await localStorage.ContainKeyAsync(Constants.LocalRefreshTokenName);
        
        return tokenExists & refreshTokenExists;
    }
    
    public async Task SetTokens(TokensModel tokens)
    {
        await localStorage.SetItemAsync(Constants.LocalTokenName, tokens.Token);
        await localStorage.SetItemAsync(Constants.LocalRefreshTokenName, tokens.RefreshToken);
    }
}