namespace Logic;

public class EventService
{
    public event Action<bool>? OnLoginStateChanged;

    public void LoginStateChange(bool loggedIn)
    {
        OnLoginStateChanged?.Invoke(loggedIn);
    }
}