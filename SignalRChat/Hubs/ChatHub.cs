using Microsoft.AspNetCore.SignalR;
using SignalRChat.Classes;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub<IChat>
    {
        public static List<Connection> conexiones { get; set; } = new List<Connection>();
        // Clients son los clientes conectados al Hub. En Clients hay métodos para manejar usuarios
        // GetMessage es un evento que debe estar registrado en el cliente
        // Los argumentos son la información que va a ir al cliente. Pueden ir objetos 
        public async Task SendMessage(Message message)
        {
            if (!string.IsNullOrEmpty(message.Text))
            {
                // Enviar un mensaje a todos
                //await Clients.All.GetMessage(message);
                // Enviar un mensaje a los miembros de la sala
                await Clients.Group(message.Room).GetMessage(message);
            }
            else if (!string.IsNullOrEmpty(message.User))
            {
                conexiones.Add(new Connection { Id = Context.ConnectionId, User = message.User, Room = message.Room });
                // Asignamos el usuario a la sala
                await Groups.AddToGroupAsync(Context.ConnectionId, message.Room);
                // Enviar a todos excepto al usuario que se conecta
                // await Clients.AllExcept(Context.ConnectionId).GetMessage(new Message() { User = message.User, Text = " se ha conectado!" });
                // Enviar a todos los de su sala excepto al propio usuario
                await Clients.GroupExcept(message.Room, Context.ConnectionId).GetMessage(new Message() { User = message.User, Text = " se ha conectado!" });
                await Clients.All.GetUsers(conexiones);
            }
        }

        // Sobrescribimos (override) algunos métodos para añadirle algo más de lógica
        public override async Task OnConnectedAsync()
        {
            // Cuando un usuario se conecta, se le da la bienvenida solo a ese por su id
            await Clients.Client(Context.ConnectionId).GetMessage(new Message() { User = "Host", Text = "Hola, Bienvenido al Chat" });
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var conexion = conexiones.Where(x => x.Id == Context.ConnectionId).FirstOrDefault();
            // Enviamos un mensaje a todos excepto al usuario que se desconecta
            // await Clients.AllExcept(Context.ConnectionId).GetMessage(new Message() { User = "Host", Text = $"{conexion.User} ha salido del chat" });
            // Enviamos un mensaje a los miembros de la sala del usuario desconectado excepto al usuario que se desconecta
            await Clients.GroupExcept(conexion.Room, Context.ConnectionId).GetMessage(new Message() { User = "Host", Text = $"{conexion.User} ha salido del chat" });

            conexiones.Remove(conexion);
            await Clients.All.GetUsers(conexiones);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
