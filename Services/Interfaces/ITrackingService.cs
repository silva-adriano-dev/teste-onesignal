using TrackingRealtime.DTOs;

namespace TrackingRealtime.Services.Interfaces;

public interface ITrackingService
{
    Task ProcessLocation(LocationMessageDto location);
}