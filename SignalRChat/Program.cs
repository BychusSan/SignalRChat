
using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

#region SERVICES

// Add services to the container.

builder.Services.AddControllers();

// Para utilizar SignalR debemos agregar el servicio SignalR mediante un m�todo especial para ello
builder.Services.AddSignalR();

#endregion

var app = builder.Build();

#region MIDDLEWARES
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseFileServer();  //PARA QUE TE LLEVE AL INDEX.HTML DEL WWWROOT
app.UseStaticFiles();


// Para que la relaci�n cliente/servidor en tiempo real sea posible, debemos hacer esta configuraci�n aqu�
// app.UseRouting() especifa que va a haber una ruta a la que un cliente se conecte a nuestro servidor en tiempo real
app.UseRouting();
// app.UseEndpoints describe cual va a ser esa ruta
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub"); // El acceso al hub ser�a v�a https://localhost:puerto/chatHub
});

app.UseAuthorization();

#endregion

app.MapControllers();

app.Run();
