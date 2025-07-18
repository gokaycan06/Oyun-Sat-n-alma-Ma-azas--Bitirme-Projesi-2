// Kullanıcı arama fonksiyonu
function searchUsers() {
    const searchTerm = document.getElementById('friendSearch').value.trim();
    
    if (searchTerm === '') {
        alert('Lütfen bir kullanıcı adı girin');
        return;
    }
    
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    
    fetch('/Home/SearchUsersAjax', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ query: searchTerm })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            displaySearchResults(data.users);
        } else {
            alert('Hata: ' + data.error);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Arama sırasında hata oluştu');
    });
}

// Arama sonuçlarını göster
function displaySearchResults(users) {
    const searchResults = document.getElementById('searchResults');
    const searchResultsList = document.getElementById('searchResultsList');
    
    if (users.length === 0) {
        searchResultsList.innerHTML = '<p class="no-results">Kullanıcı bulunamadı</p>';
    } else {
        let html = '';
        users.forEach(user => {
            html += '<div class="search-result-item">';
            html += '<div class="search-result-info">';
            html += '<img src="/images/avatars/default-avatar.jpg" alt="Avatar" class="search-result-avatar">';
            html += '<div class="search-result-details">';
            html += '<span class="search-result-name">' + (user.displayName || user.username) + '</span>';
            html += '<span class="search-result-username">@' + user.username + '</span>';
            html += '</div>';
            html += '</div>';
            html += '<button class="btn-send-request" onclick="sendFriendRequest(' + user.userId + ')">';
            html += '<i class="fas fa-user-plus"></i>';
            html += 'Arkadaşlık İsteği Gönder';
            html += '</button>';
            html += '</div>';
        });
        searchResultsList.innerHTML = html;
    }
    
    searchResults.style.display = 'block';
}

// Arkadaşlık isteği gönderme
function sendFriendRequest(userId) {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    
    fetch('/Home/SendFriendRequest', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ targetUserId: userId })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert('Arkadaşlık isteği gönderildi!');
            // Arama kutusunu temizle
            document.getElementById('friendSearch').value = '';
            // Arama sonuçlarını gizle
            document.getElementById('searchResults').style.display = 'none';
        } else {
            alert('Hata: ' + data.error);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Bir hata oluştu');
    });
}

// Arkadaşlık isteği yanıtlama
function respondToFriendRequest(requestId, accept) {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    
    fetch('/Home/RespondToFriendRequest', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ 
            requestId: requestId, 
            accept: accept 
        })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert(data.message);
            // Arkadaş listesini yenile
            loadFriendsList();
            // Gelen istekleri yenile
            loadFriendRequests();
        } else {
            alert('Hata: ' + data.error);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Bir hata oluştu');
    });
}

// Arkadaş listesini yükle
function loadFriendsList() {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    
    fetch('/Home/GetFriendsList', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            updateFriendsList(data.onlineFriends, data.offlineFriends);
        } else {
            console.error('Error loading friends:', data.error);
        }
    })
    .catch(error => {
        console.error('Error:', error);
    });
}

// Arkadaş listesini güncelle
function updateFriendsList(onlineFriends, offlineFriends) {
    const onlineContainer = document.getElementById('online-friends');
    const offlineContainer = document.getElementById('offline-friends');
    
    // Çevrimiçi arkadaşları güncelle
    onlineContainer.innerHTML = '';
    if (onlineFriends.length === 0) {
        onlineContainer.innerHTML = '<p class="no-friends">Çevrimiçi arkadaşınız yok</p>';
    } else {
        onlineFriends.forEach(friend => {
            const friendElement = createFriendElement(friend, true);
            onlineContainer.appendChild(friendElement);
        });
    }
    
    // Çevrimdışı arkadaşları güncelle
    offlineContainer.innerHTML = '';
    if (offlineFriends.length === 0) {
        offlineContainer.innerHTML = '<p class="no-friends">Çevrimdışı arkadaşınız yok</p>';
    } else {
        offlineFriends.forEach(friend => {
            const friendElement = createFriendElement(friend, false);
            offlineContainer.appendChild(friendElement);
        });
    }
}

// Arkadaş elementi oluştur
function createFriendElement(friend, isOnline) {
    const friendDiv = document.createElement('div');
    friendDiv.className = 'friend-item friend-dropdown';
    
    const statusClass = isOnline ? 'online' : 'offline';
    const statusText = isOnline ? 'Çevrimiçi' : 'Çevrimdışı';
    
    friendDiv.innerHTML = `
        <div class="friend-avatar">
            <img src="/images/avatars/default-avatar.jpg" alt="${friend.displayName}">
            <span class="status-indicator ${statusClass}"></span>
        </div>
        <div class="friend-info">
            <h4 class="friend-name">${friend.displayName}</h4>
            <p class="friend-status">${statusText}</p>
        </div>
        <div class="friend-dropdown-menu">
            <a href="#" class="friend-dropdown-item" onclick="sendMessage(${friend.userId})">
                <div class="friend-dropdown-icon">💬</div>
                <span class="friend-dropdown-text">Mesaj Gönder</span>
            </a>
            <a href="#" class="friend-dropdown-item danger" onclick="removeFriend(${friend.userId})">
                <div class="friend-dropdown-icon">❌</div>
                <span class="friend-dropdown-text">Arkadaşlıktan Kaldır</span>
            </a>
        </div>
    `;
    
    // Dropdown menü için click event
    friendDiv.addEventListener('click', function(e) {
        e.stopPropagation();
        toggleFriendDropdown(this);
    });
    
    return friendDiv;
}

// Dropdown menüyü aç/kapat
function toggleFriendDropdown(friendElement) {
    const dropdown = friendElement.querySelector('.friend-dropdown-menu');
    const isOpen = dropdown.classList.contains('show');
    
    // Tüm dropdown'ları kapat
    document.querySelectorAll('.friend-dropdown-menu').forEach(menu => {
        menu.classList.remove('show');
    });
    
    // Eğer bu dropdown kapalıysa aç
    if (!isOpen) {
        dropdown.classList.add('show');
    }
}

// Sayfa dışına tıklandığında dropdown'ları kapat
document.addEventListener('click', function(e) {
    if (!e.target.closest('.friend-dropdown')) {
        document.querySelectorAll('.friend-dropdown-menu').forEach(menu => {
            menu.classList.remove('show');
        });
    }
});

// Mesaj gönderme fonksiyonu
function sendMessage(userId) {
    window.location.href = '/Message/Chat?friendId=' + userId;
}

// Arkadaşlıktan kaldırma fonksiyonu
function removeFriend(userId) {
    if (confirm('Bu arkadaşınızı arkadaş listesinden kaldırmak istediğinizden emin misiniz?')) {
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
        
        fetch('/Home/RemoveFriend', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify({ friendId: userId })
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Arkadaşınız arkadaş listesinden kaldırıldı');
                // Arkadaş listesini yenile
                loadFriendsList();
            } else {
                alert('Hata: ' + data.error);
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Bir hata oluştu');
        });
    }
}

// Gelen arkadaşlık isteklerini yükle
function loadFriendRequests() {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    
    fetch('/Home/GetFriendRequests', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            updateFriendRequests(data.requests);
        } else {
            console.error('Error loading friend requests:', data.error);
        }
    })
    .catch(error => {
        console.error('Error:', error);
    });
}

// Gelen arkadaşlık isteklerini güncelle
function updateFriendRequests(requests) {
    const requestsContainer = document.getElementById('friend-requests');
    
    requestsContainer.innerHTML = '';
    if (requests.length === 0) {
        requestsContainer.innerHTML = '<p class="no-requests">Gelen arkadaşlık isteği yok</p>';
    } else {
        requests.forEach(request => {
            const requestElement = createRequestElement(request);
            requestsContainer.appendChild(requestElement);
        });
    }
}

// İstek elementi oluştur
function createRequestElement(request) {
    const requestDiv = document.createElement('div');
    requestDiv.className = 'friend-request-item';
    
    requestDiv.innerHTML = `
        <div class="request-info">
            <img src="/images/avatars/default-avatar.jpg" alt="Avatar" class="request-avatar">
            <div class="request-details">
                <h4 class="request-name">${request.displayName || request.username}</h4>
                <p class="request-username">@${request.username}</p>
            </div>
        </div>
        <div class="request-actions">
            <button class="btn-accept" onclick="respondToFriendRequest(${request.requestId}, true)">Kabul Et</button>
            <button class="btn-reject" onclick="respondToFriendRequest(${request.requestId}, false)">Reddet</button>
        </div>
    `;
    
    return requestDiv;
}

// Sayfa yüklendiğinde arkadaş listesini ve istekleri yükle
document.addEventListener('DOMContentLoaded', function() {
    loadFriendsList();
    loadFriendRequests();
}); 