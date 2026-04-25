using Microsoft.AspNetCore.SignalR;

using Microsoft.AspNetCore.Authorization;

namespace PokeLeague.Web.Hubs
{
    [Authorize]
    public class AuctionHub : Hub
    {
        public async Task JoinAuctionGroup(string auctionId) 
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, auctionId);
        }

        public async Task LeaveAuctionGroup(string auctionId) 
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, auctionId);
        }
    }
}
