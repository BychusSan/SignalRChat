// Función para agregar un mensaje y desplazar hacia abajo
function agregarMensaje() {
    var listaMensajes = document.getElementById('lstMensajes');
    var nuevoMensaje = document.createElement('li');
    nuevoMensaje.textContent = 'Nuevo mensaje';
    listaMensajes.appendChild(nuevoMensaje);

    setTimeout(function () {
        scrollToBottom(); // Desplazar al final después de un breve retraso
    }, 50);
}

// Función para desplazar el contenedor de mensajes hacia abajo
function scrollToBottom() {
    var messagesContainer = document.querySelector('.messages-container');
    messagesContainer.scrollTop = messagesContainer.scrollHeight;
}


// Simulación de agregar un mensaje cada segundo (solo como ejemplo)
//setInterval(agregarMensaje, 500);
