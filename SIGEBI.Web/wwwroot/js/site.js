// ============================================================
// Toast de notificaciones
// ============================================================
function showToast(message, type = 'info') {
    const container = document.getElementById('toast-container') || (() => {
        const div = document.createElement('div');
        div.id = 'toast-container';
        div.className = 'toast-container';
        document.body.appendChild(div);
        return div;
    })();

    const toast = document.createElement('div');
    toast.className = `toast-item ${type}`;
    const icon = type === 'success' ? 'fa-check-circle' :
        type === 'error' ? 'fa-exclamation-circle' : 'fa-info-circle';
    toast.innerHTML = `<i class="fas ${icon}"></i> ${message}`;
    container.appendChild(toast);

    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(40px)';
        setTimeout(() => toast.remove(), 400);
    }, 4000);
}

function marcarLeida(id, element) {
    var token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
    if (!token) {
        alert('Token CSRF no encontrado. Recarga la página.');
        return false;
    }

    fetch(`/Notificacion/MarcarLeida/${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: '{}'
    })
        .then(response => response.json())
        .then(data => {
            if (data.isSuccess) {
                var badge = document.getElementById(`badge-${id}`);
                if (badge) {
                    badge.textContent = 'Leída';
                    badge.className = 'badge estado-leida'; 
                }

                if (element && element.tagName === 'A') {
                    element.style.color = '#64748B';
                    element.style.textDecoration = 'line-through';
                }

                showToast('Notificación marcada como leída', 'success');
            } else {
                alert('Error: ' + data.message);
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error de conexión. Intente nuevamente.');
        });

    return false;
}

document.addEventListener('DOMContentLoaded', function () {
    // Este espacio queda para futuras inicializaciones.
});