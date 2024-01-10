using Models;

namespace Logic;

public class StationService
{
    private readonly ApiRequestService _api;
    private ILogger<StationService> _logger;
    public StationService(ApiRequestService api, ILogger<StationService> logger)
    {
        _api = api;
        _logger = logger;
    }

    public async Task<List<StationModel>?> GetStations()
    {
        StationsModel? stations = await _api.ApiRequest<StationsModel>("station", "get", true);

        if (stations?.Stations is null)
        {
            _logger.LogError("GetStations failed to retrieve station model!");
            return null;
        }

        return stations.Stations;
    }

    public async Task<StationModel?> GetStation(int id)
    {
        StationModel? station = await _api.ApiRequest<StationModel>("station", "get", true, null, new()
        {
            { "id", id.ToString() }
        });

        if (station is null)
        {
            _logger.LogError("GetStation failed to retrieve station model!");
            return null;
        }

        return station;
    }
}