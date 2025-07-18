# EGCGP (Ertuğrul Gökay Can Oyun Platformu) Proje Raporu

## 1. Veritabanı Yapısı
- Entity Framework Core kullanılarak 22 tablo oluşturuldu
  * Users
  * Games
  * Developers
  * Publishers
  * Categories
  * ve diğerleri...
- SQL Server bağlantısı "GÖKAYCAN,1433" sunucusuna yapılandırıldı
- Code-First yaklaşımı ile migration'lar oluşturuldu ve uygulandı

## 2. Kullanıcı Arayüzü

### 2.1 Giriş Sayfası (Login)
- Mavi-yeşil gradyan tema
- 18 saniyelik arkaplan slayt gösterisi
- EGCGP logosu
- Özellikler:
  * Kullanıcı adı veya e-posta ile giriş
  * Şifre alanı
  * "Beni Hatırla" seçeneği
  * Şifremi Unuttum linki
  * Hesap Oluştur linki
- Validation:
  * Boş alan kontrolü
  * Hatalı giriş mesajları
  * Başarılı kayıt mesajları

### 2.2 Kayıt Sayfası (Register)
- Turuncu-sarı gradyan tema
- 18 saniyelik arkaplan slayt gösterisi
- Özellikler:
  * Kullanıcı adı (benzersiz)
  * E-posta (benzersiz)
  * Şifre ve şifre tekrarı
  * Görünen ad (isteğe bağlı)
  * Kullanım koşulları onayı
- Validation:
  * Zorunlu alan kontrolleri
  * E-posta format kontrolü
  * Şifre eşleşme kontrolü
  * Minimum karakter uzunluğu
  * Benzersizlik kontrolleri

### 2.3 Şifremi Unuttum Sayfası
- Mor-pembe gradyan tema
- 18 saniyelik arkaplan slayt gösterisi
- E-posta ile şifre sıfırlama özelliği
- Güvenlik önlemleri:
  * Hesap varlığını gizleme
  * Bilgilendirme mesajları

### 2.4 Ortak Özellikler
- Yarı saydam, buzlu cam efekti
- Responsive tasarım
- Modern form elemanları
- Hover efektleri
- Bootstrap entegrasyonu

## 3. Güvenlik Özellikleri

### 3.1 Kimlik Doğrulama (Authentication)
- Cookie tabanlı authentication sistemi
- Güvenli cookie yapılandırması:
  * HttpOnly özelliği
  * Güvenli isim (EGCGP.Auth)
  * 12 saat varsayılan süre
  * 30 günlük "Beni Hatırla" seçeneği
  * Sliding expiration

### 3.2 Şifre Güvenliği
- SHA256 ile şifre hashleme
- Güvenli şifre doğrulama
- Minimum 6 karakter zorunluluğu

### 3.3 Veri Güvenliği
- SQL injection koruması (Entity Framework)
- Cross-Site Scripting (XSS) koruması
- Cross-Site Request Forgery (CSRF) koruması

## 4. Veritabanı Modelleri

### 4.1 User Modeli
- Temel bilgiler:
  * UserID (Primary Key)
  * Username (benzersiz)
  * Email (benzersiz)
  * PasswordHash
  * DisplayName
  * RegisterDate
  * LastLoginDate
  * IsActive
  * EmailConfirmed
  * WalletBalance
  * ProfilePicture
  * AboutMe
  * Country

- İlişkisel özellikler:
  * UserLibrary (Oyun kütüphanesi)
  * WishList (İstek listesi)
  * Reviews (İncelemeler)
  * Achievements (Başarımlar)
  * Purchases (Satın alımlar)
  * UserDLCs (DLC'ler)
  * UserBundles (Paketler)
  * ForumPosts (Forum gönderileri)
  * Screenshots (Ekran görüntüleri)
  * NewsComments (Haber yorumları)
  * Inventory (Envanter)

## 5. Hata Yönetimi
- Detaylı validation mesajları
- Kullanıcı dostu hata bildirimleri
- Exception logging sistemi
- Güvenli hata mesajları (hassas bilgileri gizleme)

## 6. Performans Optimizasyonları
- Asenkron veritabanı işlemleri
- Lazy loading ilişkisel veriler
- Entity Framework optimizasyonları

## 7. Gelecek Özellikler
- E-posta doğrulama sistemi
- İki faktörlü kimlik doğrulama
- Sosyal medya ile giriş
- Profil sayfası
- Oyun mağazası ana sayfası
- Oyun detay sayfaları
- Satın alma sistemi
- Forum sistemi
- Arkadaşlık sistemi
- Başarım sistemi
- DLC ve paket yönetimi
- Envanter sistemi
- Haber ve yorum sistemi 