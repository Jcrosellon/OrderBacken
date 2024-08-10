using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OrderBackend
{
    public class OrderHub : Hub
    {
        // Enviar un mensaje a todos los clientes conectados
        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        // Enviar un mensaje a un cliente específico
        public async Task SendMessageToUser(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", message);
        }

        // Enviar un mensaje a un grupo específico
        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        // Agregar un cliente a un grupo
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Eliminar un cliente de un grupo
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
