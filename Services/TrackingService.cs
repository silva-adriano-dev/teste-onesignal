using TrackingRealtime.DTOs;
using TrackingRealtime.Services.Interfaces;

namespace TrackingRealtime.Services;

public class TrackingService : ITrackingService
{
    private readonly ILogger<TrackingService> _logger;

    public TrackingService(ILogger<TrackingService> logger)
    {
        _logger = logger;
    }

    public Task ProcessLocation(LocationMessageDto location)
    {
        _logger.LogInformation(
            "Vehicle {Id} -> Lat:{Lat} Lng:{Lng}",
            location.Id,
            location.Latitude,
            location.Longitude
        );

        return Task.CompletedTask;
    }
}