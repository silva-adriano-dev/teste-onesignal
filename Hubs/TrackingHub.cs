using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TrackingRealtime.DTOs;
using TrackingRealtime.Services.Interfaces;

namespace TrackingRealtime.Hubs;

//[Authorize]
public class TrackingHub : Hub
{
    private readonly ITrackingService _trackingService;

    public TrackingHub(ITrackingService trackingService)
    {
        _trackingService = trackingService;
    }

    public override async Task OnConnectedAsync()
{
    // 1. Pega o grupo da QueryString apenas UMA VEZ na conexão
    var idGrupo = Context.GetHttpContext()?.Request.Query["grupo"].ToString();

    if (!string.IsNullOrEmpty(idGrupo))
    {
        // 2. Armazena no Context.Items para persistir durante toda a sessão
        Context.Items["idGrupo"] = idGrupo;
        
        await Groups.AddToGroupAsync(Context.ConnectionId, idGrupo);
    }

    await base.OnConnectedAsync();
}

public async Task SendLocation(LocationMessageDto location)
{
    // 3. Recupera do Context.Items (muito mais rápido e seguro)
    if (!Context.Items.TryGetValue("idGrupo", out var idGrupoObj) || 
        idGrupoObj is not string idGrupo)
    {
        throw new HubException("Conexão sem grupo associado.");
    }

    await _trackingService.ProcessLocation(location);

    // 4. Notifica o grupo
    await Clients.Group(idGrupo).SendAsync("LocationReceived", location);
}
}