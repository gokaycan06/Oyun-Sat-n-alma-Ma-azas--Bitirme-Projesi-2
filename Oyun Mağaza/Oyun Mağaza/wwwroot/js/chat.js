// Mesaj gönderme fonksiyonu
function sendMessage() {
    const messageInput = document.getElementById('messageInput');
    const content = messageInput.value.trim();
    
    if (!content) {
        return;
    }
    
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    
    fetch('/Message/SendMessage', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({
            receiverId: friendId,
            content: content
        })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            // Mesajı UI'a ekle
            addMessageToUI(content, true, data.createdAt);
            // Input'u temizle
            messageInput.value = '';
            // Son mesaj ID'sini güncelle
            lastMessageId = data.messageId;
        } else {
            alert('Hata: ' + data.error);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Mesaj gönderilirken hata oluştu');
    });
}

// Mesajı UI'a ekleme
function addMessageToUI(content, isOwn, time) {
    const chatMessages = document.getElementById('chatMessages');
    const messageDiv = document.createElement('div');
    messageDiv.className = `message ${isOwn ? 'message-own' : 'message-other'}`;
    
    messageDiv.innerHTML = `
        <div class="message-content">
            <p class="message-text">${content}</p>
            <span class="message-time">${time}</span>
        </div>
    `;
    
    chatMessages.appendChild(messageDiv);
    scrollToBottom();
}

// Yeni mesajları kontrol etme
function checkNewMessages() {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    
    fetch('/Message/GetNewMessages', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({
            friendId: friendId,
            lastMessageId: lastMessageId
        })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success && data.messages.length > 0) {
            data.messages.forEach(message => {
                addMessageToUI(message.content, message.isOwn, message.createdAt);
                lastMessageId = Math.max(lastMessageId, message.id);
            });
        }
    })
    .catch(error => {
        console.error('Error checking messages:', error);
    });
}

// Sohbetin en altına kaydırma
function scrollToBottom() {
    const chatMessages = document.getElementById('chatMessages');
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

// Enter tuşu ile mesaj gönderme
document.addEventListener('DOMContentLoaded', function() {
    const messageInput = document.getElementById('messageInput');
    
    messageInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            sendMessage();
        }
    });
    
    // Sayfa yüklendiğinde en alta kaydır
    scrollToBottom();
    
    // Yeni mesajları kontrol etmeye başla (3 saniyede bir)
    messageCheckInterval = setInterval(checkNewMessages, 3000);
});

// Sayfa kapatılırken interval'i temizle
window.addEventListener('beforeunload', function() {
    if (messageCheckInterval) {
        clearInterval(messageCheckInterval);
    }
}); 