using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace OrderBackend
{
    public class OrderHub : Hub
    {
        public async Task SendMessageToAll(string message)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error enviando mensaje a todos: {ex.Message}");
            }
        }

        public async Task SendMessageToUser(string userId, string message)
        {
            try
            {
                await Clients.User(userId).SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando mensaje a usuario {userId}: {ex.Message}");
            }
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            try
            {
                await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando mensaje al grupo {groupName}: {ex.Message}");
            }
        }

        public async Task AddToGroup(string groupName)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error agregando a grupo {groupName}: {ex.Message}");
            }
        }

        public async Task RemoveFromGroup(string groupName)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando del grupo {groupName}: {ex.Message}");
            }
        }
    }
}
