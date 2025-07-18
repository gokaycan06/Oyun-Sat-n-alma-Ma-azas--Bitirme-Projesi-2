using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Models;
using Oyun_Mağaza.Helpers;

namespace Oyun_Mağaza.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            Console.WriteLine("SeedData.Initialize başlatıldı!");
            
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Veritabanında veri olup olmadığını kontrol et
            if (context.Games.Any())
            {
                Console.WriteLine("Veritabanında zaten veri mevcut. Seed data atlanıyor.");
                return;
            }

            Console.WriteLine("Veritabanı boş. Seed data başlatılıyor...");

            // Admin kullanıcısını ekle veya güncelle
            var adminUser = context.Users.FirstOrDefault(u => u.Username == "admin");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@egcgp.com",
                    DisplayName = "Sistem Yöneticisi",
                    PasswordHash = PasswordHasher.HashPassword("admin123"), // Şifreyi hash'le
                    IsAdmin = true,
                    IsActive = true,
                    EmailConfirmed = true,
                    RegisterDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                    ProfilePicture = "/images/avatars/admin-avatar.png",
                    Bio = "EGCGP Sistem Yöneticisi",
                    WalletBalance = 0,
                    BirthDate = new DateTime(1990, 1, 1)
                };

                context.Users.Add(adminUser);
            }
            else
            {
                // Mevcut admin kullanıcısının şifresini güncelle
                adminUser.PasswordHash = PasswordHasher.HashPassword("admin123");
                adminUser.IsAdmin = true;
                adminUser.IsActive = true;
                context.Users.Update(adminUser);
            }
            
            await context.SaveChangesAsync();

            // Test kredi kartı ekle (admin kullanıcısı için)
            var testCreditCard = new CreditCard
            {
                UserId = adminUser.UserId,
                CardNumber = "1111 1111 1111 1111",
                NameOnCard = "Test User",
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                Cvv = "111",
                CreatedAt = DateTime.Now
            };

            // Eğer test kartı yoksa ekle
            if (!context.CreditCards.Any(cc => cc.CardNumber == testCreditCard.CardNumber))
            {
                context.CreditCards.Add(testCreditCard);
                await context.SaveChangesAsync();
                Console.WriteLine("Test kredi kartı eklendi!");
            }

            // Geliştirici ve yayıncıları ekle
            var valve = new Developer { Name = "Valve Corporation", Country = "ABD", FoundedYear = 1996 };
            var rockstar = new Developer { Name = "Rockstar Games", Country = "ABD", FoundedYear = 1998 };
            var ea = new Developer { Name = "EA Sports", Country = "ABD", FoundedYear = 1991 };
            var pubgCorp = new Developer { Name = "PUBG Corporation", Country = "Güney Kore", FoundedYear = 2009 };
            var cdProjekt = new Developer { Name = "CD Projekt Red", Country = "Polonya", FoundedYear = 2002 };
            var fromSoftware = new Developer { Name = "FromSoftware", Country = "Japonya", FoundedYear = 1986 };
            var ubisoft = new Developer { Name = "Ubisoft", Country = "Fransa", FoundedYear = 1986 };
            var bethesda = new Developer { Name = "Bethesda Game Studios", Country = "ABD", FoundedYear = 1986 };
            var mojang = new Developer { Name = "Mojang Studios", Country = "İsveç", FoundedYear = 2009 };
            var riotGames = new Developer { Name = "Riot Games", Country = "ABD", FoundedYear = 2006 };
            var activision = new Developer { Name = "Activision", Country = "ABD", FoundedYear = 1979 };
            var infinityWard = new Developer { Name = "Infinity Ward", Country = "ABD", FoundedYear = 2002 };
            var treyarch = new Developer { Name = "Treyarch", Country = "ABD", FoundedYear = 1996 };
            var facepunch = new Developer { Name = "Facepunch Studios", Country = "İngiltere", FoundedYear = 2004 };
            var studioWildcard = new Developer { Name = "Studio Wildcard", Country = "ABD", FoundedYear = 2014 };
            var bethesdaGameStudios = new Developer { Name = "Bethesda Game Studios", Country = "ABD", FoundedYear = 2001 };
            var turtleRock = new Developer { Name = "Turtle Rock Studios", Country = "ABD", FoundedYear = 2002 };
            var reLogic = new Developer { Name = "Re-Logic", Country = "ABD", FoundedYear = 2011 };
            var concernedApe = new Developer { Name = "ConcernedApe", Country = "ABD", FoundedYear = 2012 };
            var innerSloth = new Developer { Name = "InnerSloth", Country = "ABD", FoundedYear = 2015 };
            var kineticGames = new Developer { Name = "Kinetic Games", Country = "İngiltere", FoundedYear = 2020 };
            var behaviourInteractive = new Developer { Name = "Behaviour Interactive", Country = "Kanada", FoundedYear = 1992 };
            var ubisoftMontreal = new Developer { Name = "Ubisoft Montreal", Country = "Kanada", FoundedYear = 1997 };
            var digitalExtremes = new Developer { Name = "Digital Extremes", Country = "Kanada", FoundedYear = 1993 };
            var grindingGearGames = new Developer { Name = "Grinding Gear Games", Country = "Yeni Zelanda", FoundedYear = 2006 };
            var gaijinEntertainment = new Developer { Name = "Gaijin Entertainment", Country = "Rusya", FoundedYear = 2002 };
            var bungie = new Developer { Name = "Bungie", Country = "ABD", FoundedYear = 1991 };
            var respawnEntertainment = new Developer { Name = "Respawn Entertainment", Country = "ABD", FoundedYear = 2010 };
            var psyonix = new Developer { Name = "Psyonix", Country = "ABD", FoundedYear = 2000 };
            var mediatonic = new Developer { Name = "Mediatonic", Country = "İngiltere", FoundedYear = 2005 };
            var supergiantGames = new Developer { Name = "Supergiant Games", Country = "ABD", FoundedYear = 2009 };
            var zaumStudio = new Developer { Name = "ZA/UM", Country = "Estonya", FoundedYear = 2016 };
            var larianStudios = new Developer { Name = "Larian Studios", Country = "Belçika", FoundedYear = 1996 };
            var capcom = new Developer { Name = "Capcom", Country = "Japonya", FoundedYear = 1983 };
            var avalancheSoftware = new Developer { Name = "Avalanche Software", Country = "ABD", FoundedYear = 1995 };
            var neowiz = new Developer { Name = "Neowiz", Country = "Güney Kore", FoundedYear = 1997 };
            var gunfireGames = new Developer { Name = "Gunfire Games", Country = "ABD", FoundedYear = 2014 };
            var sledgehammerGames = new Developer { Name = "Sledgehammer Games", Country = "ABD", FoundedYear = 2009 };

            var valvePublisher = new Publisher { Name = "Valve Corporation", Country = "ABD", FoundedYear = 1996 };
            var rockstarPublisher = new Publisher { Name = "Rockstar Games", Country = "ABD", FoundedYear = 1998 };
            var eaPublisher = new Publisher { Name = "Electronic Arts", Country = "ABD", FoundedYear = 1982 };
            var kraftonPublisher = new Publisher { Name = "Krafton", Country = "Güney Kore", FoundedYear = 2007 };
            var cdProjektPublisher = new Publisher { Name = "CD Projekt", Country = "Polonya", FoundedYear = 1994 };
            var bandaiNamco = new Publisher { Name = "Bandai Namco", Country = "Japonya", FoundedYear = 2006 };
            var ubisoftPublisher = new Publisher { Name = "Ubisoft", Country = "Fransa", FoundedYear = 1986 };
            var bethesdaPublisher = new Publisher { Name = "Bethesda Softworks", Country = "ABD", FoundedYear = 1986 };
            var microsoft = new Publisher { Name = "Microsoft", Country = "ABD", FoundedYear = 1975 };
            var riotPublisher = new Publisher { Name = "Riot Games", Country = "ABD", FoundedYear = 2006 };
            var activisionPublisher = new Publisher { Name = "Activision", Country = "ABD", FoundedYear = 1979 };
            var facepunchPublisher = new Publisher { Name = "Facepunch Studios", Country = "İngiltere", FoundedYear = 2004 };
            var studioWildcardPublisher = new Publisher { Name = "Studio Wildcard", Country = "ABD", FoundedYear = 2014 };
            var turtleRockPublisher = new Publisher { Name = "Turtle Rock Studios", Country = "ABD", FoundedYear = 2002 };
            var reLogicPublisher = new Publisher { Name = "Re-Logic", Country = "ABD", FoundedYear = 2011 };
            var concernedApePublisher = new Publisher { Name = "ConcernedApe", Country = "ABD", FoundedYear = 2012 };
            var innerSlothPublisher = new Publisher { Name = "InnerSloth", Country = "ABD", FoundedYear = 2015 };
            var kineticGamesPublisher = new Publisher { Name = "Kinetic Games", Country = "İngiltere", FoundedYear = 2020 };
            var behaviourInteractivePublisher = new Publisher { Name = "Behaviour Interactive", Country = "Kanada", FoundedYear = 1992 };
            var digitalExtremesPublisher = new Publisher { Name = "Digital Extremes", Country = "Kanada", FoundedYear = 1993 };
            var grindingGearGamesPublisher = new Publisher { Name = "Grinding Gear Games", Country = "Yeni Zelanda", FoundedYear = 2006 };
            var gaijinEntertainmentPublisher = new Publisher { Name = "Gaijin Entertainment", Country = "Rusya", FoundedYear = 2002 };
            var bungiePublisher = new Publisher { Name = "Bungie", Country = "ABD", FoundedYear = 1991 };
            var respawnEntertainmentPublisher = new Publisher { Name = "Respawn Entertainment", Country = "ABD", FoundedYear = 2010 };
            var psyonixPublisher = new Publisher { Name = "Psyonix", Country = "ABD", FoundedYear = 2000 };
            var mediatonicPublisher = new Publisher { Name = "Mediatonic", Country = "İngiltere", FoundedYear = 2005 };
            var supergiantGamesPublisher = new Publisher { Name = "Supergiant Games", Country = "ABD", FoundedYear = 2009 };
            var zaumStudioPublisher = new Publisher { Name = "ZA/UM", Country = "Estonya", FoundedYear = 2016 };
            var larianStudiosPublisher = new Publisher { Name = "Larian Studios", Country = "Belçika", FoundedYear = 1996 };
            var capcomPublisher = new Publisher { Name = "Capcom", Country = "Japonya", FoundedYear = 1983 };
            var avalancheSoftwarePublisher = new Publisher { Name = "Avalanche Software", Country = "ABD", FoundedYear = 1995 };
            var neowizPublisher = new Publisher { Name = "Neowiz", Country = "Güney Kore", FoundedYear = 1997 };
            var gunfireGamesPublisher = new Publisher { Name = "Gunfire Games", Country = "ABD", FoundedYear = 2014 };
            var sledgehammerGamesPublisher = new Publisher { Name = "Sledgehammer Games", Country = "ABD", FoundedYear = 2009 };

            context.Developers.AddRange(valve, rockstar, ea, pubgCorp, cdProjekt, fromSoftware, ubisoft, bethesda, mojang, riotGames, activision, infinityWard, treyarch, facepunch, studioWildcard, bethesdaGameStudios, turtleRock, reLogic, concernedApe, innerSloth, kineticGames, behaviourInteractive, ubisoftMontreal, digitalExtremes, grindingGearGames, gaijinEntertainment, bungie, respawnEntertainment, psyonix, mediatonic, supergiantGames, zaumStudio, larianStudios, capcom, avalancheSoftware, neowiz, gunfireGames, sledgehammerGames);
            context.Publishers.AddRange(valvePublisher, rockstarPublisher, eaPublisher, kraftonPublisher, cdProjektPublisher, bandaiNamco, ubisoftPublisher, bethesdaPublisher, microsoft, riotPublisher, activisionPublisher, facepunchPublisher, studioWildcardPublisher, turtleRockPublisher, reLogicPublisher, concernedApePublisher, innerSlothPublisher, kineticGamesPublisher, behaviourInteractivePublisher, digitalExtremesPublisher, grindingGearGamesPublisher, gaijinEntertainmentPublisher, bungiePublisher, respawnEntertainmentPublisher, psyonixPublisher, mediatonicPublisher, supergiantGamesPublisher, zaumStudioPublisher, larianStudiosPublisher, capcomPublisher, avalancheSoftwarePublisher, neowizPublisher, gunfireGamesPublisher, sledgehammerGamesPublisher);
            await context.SaveChangesAsync();

            // Kategorileri ekle
            var fps = new Category { Name = "FPS", Description = "First Person Shooter" };
            var action = new Category { Name = "Aksiyon", Description = "Aksiyon Oyunları" };
            var sports = new Category { Name = "Spor", Description = "Spor Oyunları" };
            var moba = new Category { Name = "MOBA", Description = "Multiplayer Online Battle Arena" };
            var openWorld = new Category { Name = "Açık Dünya", Description = "Açık Dünya Oyunları" };
            var rpg = new Category { Name = "RPG", Description = "Role Playing Game" };
            var survival = new Category { Name = "Hayatta Kalma", Description = "Survival Games" };
            var racing = new Category { Name = "Yarış", Description = "Racing Games" };
            var strategy = new Category { Name = "Strateji", Description = "Strategy Games" };
            var horror = new Category { Name = "Korku", Description = "Horror Games" };

            context.Categories.AddRange(fps, action, sports, moba, openWorld, rpg, survival, racing, strategy, horror);
            await context.SaveChangesAsync();

            // Oyunları ekle
            var csgo = new Game
            {
                Title = "Counter-Strike 2",
                Description = "Counter-Strike 2, Valve'ın efsanevi taktiksel FPS oyununun en son sürümüdür. Source 2 motoru ile yeniden tasarlanmış oyun mekanikleri ve grafikleriyle, rekabetçi FPS türünün zirvesinde yer almaktadır.\n\n" +
                            "Oyunda iki takım arasında geçen yoğun taktiksel savaşlar, bombalama ve rehine kurtarma görevleri bulunmaktadır. Her silahın kendine özgü karakteristiği ve geri tepme paterni vardır.\n\n" +
                            "Gelişmiş grafik motoru sayesinde daha gerçekçi ışık efektleri, parçacık sistemleri ve su yansımaları görülebilir. Oyun içi ekonomi sistemi ile takımlar her round'da stratejik kararlar vermek zorundadır.\n\n" +
                            "Rekabetçi maçlarda 5v5 formatında oynanan oyun, profesyonel e-spor sahnesinde büyük turnuvalara ev sahipliği yapmaktadır. Oyuncuların beceri seviyelerine göre sıralandığı ranking sistemi bulunmaktadır.\n\n" +
                            "Topluluk tarafından oluşturulan haritalar ve skin'ler ile oyun sürekli yenilenmektedir. Steam Workshop entegrasyonu ile oyuncular kendi içeriklerini paylaşabilmektedir.\n\n" +
                            "Ücretsiz oyun modeli ile herkesin erişebileceği bu oyun, dünya çapında milyonlarca aktif oyuncuya sahiptir ve FPS türünün en popüler oyunlarından biridir.",
                Developer = valve,
                Publisher = valvePublisher,
                ReleaseDate = new DateTime(2023, 9, 27),
                Price = 0, // Ücretsiz oyun
                CoverImage = "cs2-cover.jpg",
                IsActive = true
            };

            

            var pubg = new Game
            {
                Title = "PUBG: BATTLEGROUNDS",
                Description = "PUBG: BATTLEGROUNDS, 100 oyuncunun hayatta kalma mücadelesi verdiği bir battle royale oyunudur. Stratejik oynanış ve gerçekçi silah mekanikleriyle türünün öncüsüdür.\n\n" +
                            "Her maçta oyuncular uçaktan atlayarak geniş haritalara iniş yapar ve hayatta kalmak için silah, zırh ve tıbbi malzeme aramak zorundadır. Giderek küçülen güvenli bölge sistemi oyuncuları birbirine yaklaştırır.\n\n" +
                            "Gerçekçi fizik motoru sayesinde silahların geri tepmesi, mermi yolu ve ses sistemi oldukça detaylıdır. Araç kullanımı, bina içi savaşlar ve uzun mesafeli nişancılık oyunun temel unsurlarıdır.\n\n" +
                            "Farklı haritalar (Erangel, Miramar, Sanhok, Vikendi) her biri kendine özgü coğrafi özellikler ve oynanış dinamikleri sunar. Hava durumu koşulları ve zaman değişimleri stratejik kararları etkiler.\n\n" +
                            "Squad, Duo ve Solo modları ile farklı oynanış deneyimleri sunulur. Takım çalışması ve iletişim başarı için kritik öneme sahiptir. Ranking sistemi ile oyuncular kendi seviyelerindeki rakiplerle eşleşir.\n\n" +
                            "Sürekli güncellemeler ile yeni silahlar, araçlar ve özellikler eklenmektedir. Ücretsiz oyun modeli ile geniş bir oyuncu kitlesine ulaşan PUBG, battle royale türünün en etkili oyunlarından biridir.",
                Developer = pubgCorp,
                Publisher = kraftonPublisher,
                ReleaseDate = new DateTime(2017, 3, 23),
                Price = 0, // Ücretsiz oyun
                CoverImage = "pubg-cover.jpg",
                IsActive = true
            };

            

            var fifa24 = new Game
            {
                Title = "EA SPORTS FC 24",
                Description = "EA SPORTS FC 24, futbol simülasyonunun zirvesi. Gerçekçi oyuncu hareketleri, gelişmiş maç motoru ve çeşitli oyun modlarıyla tam bir futbol deneyimi sunuyor.\n\n" +
                            "HyperMotionV teknolojisi ile gerçek maçlardan alınan veriler kullanılarak oyuncu animasyonları ve hareketleri daha gerçekçi hale getirilmiştir. Her oyuncunun kendine özgü oynanış stili vardır.\n\n" +
                            "Ultimate Team modunda oyuncular kendi hayali takımlarını oluşturabilir, transfer pazarında alım-satım yapabilir ve çeşitli turnuvalara katılabilir. Chemistry sistemi ile takım uyumu önemlidir.\n\n" +
                            "Career Mode'da oyuncu veya menajer olarak kariyer yapabilirsiniz. Genç oyuncular yetiştirme, takım yönetimi ve transfer stratejileri ile gerçekçi bir futbol yöneticiliği deneyimi yaşarsınız.\n\n" +
                            "Volta Football ile sokak futbolu deneyimi sunulur. Küçük sahalarda, özel kurallarla oynanan bu modda farklı beceriler ve taktikler ön plana çıkar. Pro Clubs ile arkadaşlarınızla takım kurarak online liglerde yarışabilirsiniz.\n\n" +
                            "Lisanslı ligler, takımlar ve oyuncular ile gerçek futbol dünyasının tam bir simülasyonu sunulur. Sürekli güncellemeler ile oyuncu formları ve takım kadroları gerçek zamanlı olarak güncellenir.",
                Developer = ea,
                Publisher = eaPublisher,
                ReleaseDate = new DateTime(2023, 9, 29),
                Price = 999.99M, // TL cinsinden
                CoverImage = "fc24-cover.jpg",
                IsActive = true
            };

            

            var dota2 = new Game
            {
                Title = "Dota 2",
                Description = "Dota 2, zengin karakter kadrosu ve derin strateji unsurlarıyla MOBA türünün en popüler oyunlarından biri. Her maç benzersiz bir deneyim sunuyor.\n\n" +
                            "120'den fazla hero ile oyuncular farklı roller üstlenebilir: Carry, Support, Mid, Offlaner ve Jungle. Her hero'nun kendine özgü yetenekleri, güçlü ve zayıf yanları vardır.\n\n" +
                            "Karmaşık item sistemi ile hero'ları güçlendirebilir, farklı oynanış stilleri geliştirebilirsiniz. Active ve passive item'lar stratejik derinlik katar. Denying, last hitting ve creep stacking gibi mekanikler beceri gerektirir.\n\n" +
                            "Map kontrolü, ward yerleştirme ve gank stratejileri oyunun kritik unsurlarıdır. Roshan, ancients ve jungle creeps gibi neutral hedefler takımlar için önemli kaynaklar sağlar.\n\n" +
                            "Ranked matchmaking sistemi ile oyuncular kendi seviyelerindeki rakiplerle eşleşir. MMR (Matchmaking Rating) sistemi ile oyuncu performansı ölçülür ve sürekli iyileştirme teşvik edilir.\n\n" +
                            "Uluslararası turnuvalar ve The International gibi büyük etkinlikler ile e-spor sahnesinde önemli bir yere sahiptir. Topluluk tarafından oluşturulan custom game'ler ve mod'lar oyunu daha da zenginleştirir.",
                Developer = valve,
                Publisher = valvePublisher,
                ReleaseDate = new DateTime(2013, 7, 9),
                Price = 0, // Ücretsiz oyun
                CoverImage = "dota2-cover.jpg",
                IsActive = true
            };

            

            var gta5 = new Game
            {
                Title = "Grand Theft Auto V",
                Description = "Grand Theft Auto V, Los Santos şehrinde geçen, üç ana karakterin hikayesini anlatan açık dünya aksiyon oyunu. Zengin içeriği ve çevrimiçi modu ile milyonlarca oyuncuya ev sahipliği yapıyor.\n\n" +
                            "Michael De Santa, Franklin Clinton ve Trevor Philips karakterlerinin hayatları kesişir ve büyük bir suç hikayesi ortaya çıkar. Her karakterin kendine özgü yetenekleri ve oynanış stili vardır.\n\n" +
                            "Los Santos ve Blaine County'nin geniş haritasında özgürce dolaşabilir, yan görevler yapabilir ve çeşitli aktivitelere katılabilirsiniz. Araç kullanımı, silah savaşları ve stealth mekanikleri oyunun temel unsurlarıdır.\n\n" +
                            "GTA Online modunda arkadaşlarınızla birlikte oynayabilir, kendi suç imparatorluğunuzu kurabilirsiniz. Heist'ler, CEO işleri, MC kulüpleri ve Nightclub yönetimi gibi çeşitli iş modları bulunur.\n\n" +
                            "Sürekli güncellemeler ile yeni araçlar, silahlar, kıyafetler ve oyun modları eklenmektedir. Custom race'ler, deathmatch'ler ve roleplay sunucuları ile oyun deneyimi çeşitlendirilir.\n\n" +
                            "Gelişmiş grafik motoru ve detaylı ses tasarımı ile Los Santos'un canlı atmosferi başarıyla yansıtılmıştır. NPC'lerin gerçekçi davranışları ve dinamik dünya sistemi oyuna derinlik katar.",
                Developer = rockstar,
                Publisher = rockstarPublisher,
                ReleaseDate = new DateTime(2015, 4, 14),
                Price = 499.99M, // TL cinsinden
                CoverImage = "gtav-cover.jpg",
                IsActive = true
            };

            

            // Yeni oyunları ekle
            var witcher3 = new Game
            {
                Title = "The Witcher 3: Wild Hunt",
                Description = "The Witcher 3: Wild Hunt, Geralt of Rivia'nın destansı macerasını konu alan, ödüllü açık dünya RPG oyunudur. CD Projekt Red'in yarattığı bu başyapıt, oyun dünyasında yeni standartlar belirlemiştir.\n\n" +
                            "Velen, Novigrad, Skellige ve Toussaint gibi geniş bölgelerde geçen hikaye, Ciri'yi arama görevi etrafında şekillenir. Her bölgenin kendine özgü kültürü, iklimi ve halkları vardır.\n\n" +
                            "Geralt'ın witcher yetenekleri, silah kullanımı ve büyü sistemi ile çeşitli düşmanlarla savaşabilirsiniz. Mutasyon sistemi ile karakterinizi özelleştirebilir, farklı oynanış stilleri geliştirebilirsiniz.\n\n" +
                            "Yan görevler ana hikayeden daha detaylı ve etkileyicidir. Her görev kendi hikayesine sahiptir ve oyuncunun ahlaki seçimleri sonuçları etkiler. Witcher kontratları ile tehlikeli yaratıkları avlayabilirsiniz.\n\n" +
                            "Gwent kart oyunu oyun içinde ayrı bir deneyim sunar. Kart toplama, strateji geliştirme ve turnuvalara katılma ile saatlerce eğlenceli vakit geçirebilirsiniz.\n\n" +
                            "Blood and Wine ve Hearts of Stone DLC'leri ile oyun içeriği daha da zenginleşir. Toussaint'in güzel manzaraları ve yeni hikayeler oyuna yeni bir boyut katar. Mod desteği ile topluluk tarafından oluşturulan içerikler oyunu daha da geliştirir.",
                Developer = cdProjekt,
                Publisher = cdProjektPublisher,
                ReleaseDate = new DateTime(2015, 5, 19),
                Price = 299.99M,
                CoverImage = "witcher3-cover.jpg",
                IsActive = true,
                IsFeatured = true
            };

            var eldenRing = new Game
            {
                Title = "Elden Ring",
                Description = "Elden Ring, FromSoftware'in yarattığı zorlu ve büyüleyici açık dünya aksiyon RPG oyunudur. George R.R. Martin'in katkılarıyla oluşturulan hikaye dünyası, Souls serisinin zorlu oynanışıyla birleşir.\n\n" +
                            "The Lands Between'in geniş haritasında özgürce dolaşabilir, keşfedilmemiş bölgeleri araştırabilirsiniz. Her bölge kendine özgü düşmanları, boss'ları ve hazineleri barındırır.\n\n" +
                            "Souls benzeri zorlu savaş sistemi ile düşmanlarla stratejik mücadeleler verirsiniz. Parry, dodge ve counter-attack mekanikleri ile becerilerinizi geliştirmeniz gerekir.\n\n" +
                            "Çeşitli silah türleri, büyüler ve incantation'lar ile karakterinizi özelleştirebilirsiniz. Ashes of War sistemi ile silahlarınıza özel yetenekler ekleyebilirsiniz.\n\n" +
                            "Spirit Ashes ile yanınızda savaşacak ruhlar çağırabilir, çoklu oyuncu modunda arkadaşlarınızla birlikte boss'ları yenebilirsiniz. PvP arenasında diğer oyuncularla düello yapabilirsiniz.\n\n" +
                            "Görsel sanatlar ve müzik açısından da mükemmel olan oyun, FromSoftware'in en büyük projesidir. Shadow of the Erdtree DLC'si ile oyun içeriği daha da genişletilmiştir.",
                Developer = fromSoftware,
                Publisher = bandaiNamco,
                ReleaseDate = new DateTime(2022, 2, 25),
                Price = 699.99M,
                CoverImage = "elden-ring-cover.jpg",
                IsActive = true,
                IsFeatured = true
            };

            var valorant = new Game
            {
                Title = "VALORANT",
                Description = "VALORANT, Riot Games'in taktiksel 5v5 karakter tabanlı FPS oyunudur. CS:GO'nun taktiksel derinliği ile Overwatch'ın karakter çeşitliliğini birleştiren benzersiz bir deneyim sunar.\n\n" +
                            "Her agent'ın kendine özgü yetenekleri vardır: Duelist'ler saldırı odaklı, Sentinel'ler savunma odaklı, Controller'lar alan kontrolü sağlar, Initiator'lar ise takım için bilgi toplar.\n\n" +
                            "Spike defuse ve plant mekanikleri CS:GO'dan esinlenmiştir. Ekonomi sistemi, weapon buy rounds ve tactical gameplay oyunun temel unsurlarıdır. Her round stratejik kararlar vermenizi gerektirir.\n\n" +
                            "Farklı haritalar (Ascent, Bind, Haven, Split, Icebox, Breeze, Fracture, Pearl, Lotus) her biri kendine özgü oynanış dinamikleri sunar. Map-specific lineups ve stratejiler geliştirilir.\n\n" +
                            "Ranked sistem ile oyuncular Iron'dan Radiant'a kadar sıralanır. Unrated, Spike Rush, Deathmatch ve Custom Game modları ile farklı deneyimler yaşayabilirsiniz.\n\n" +
                            "Sürekli güncellemeler ile yeni agent'lar, haritalar ve oyun modları eklenmektedir. VCT (VALORANT Champions Tour) ile profesyonel e-spor sahnesinde büyük turnuvalara ev sahipliği yapar.",
                Developer = riotGames,
                Publisher = riotPublisher,
                ReleaseDate = new DateTime(2020, 6, 2),
                Price = 0,
                CoverImage = "valorant-cover.jpg",
                IsActive = true,
                IsFeatured = true
            };

            var minecraft = new Game
            {
                Title = "Minecraft",
                Description = "Minecraft, Mojang Studios tarafından geliştirilen dünya çapında fenomen olmuş sandbox oyunudur. 3D blok tabanlı dünyada sınırsız yaratıcılık ve keşif imkanı sunar.\n\n" +
                            "Survival modunda kaynak toplama, crafting, hayatta kalma ve düşmanlarla mücadele ederken, Creative modunda sınırsız kaynaklarla hayal gücünüzü serbest bırakabilirsiniz. Her blok türünün kendine özgü özellikleri vardır.\n\n" +
                            "Redstone sistemi ile karmaşık makineler, otomasyonlar ve devreler oluşturabilirsiniz. Command blocks ile özel komutlar yazabilir, mini-game'ler tasarlayabilirsiniz.\n\n" +
                            "Multiplayer sunucularında arkadaşlarınızla birlikte oynayabilir, büyük projeler gerçekleştirebilirsiniz. PvP arenaları, parkour kursları ve roleplay sunucuları bulunur.\n\n" +
                            "Sürekli güncellemeler ile yeni mob'lar, bloklar, eşyalar ve özellikler eklenmektedir. Nether, End ve Overworld boyutları farklı deneyimler sunar.\n\n" +
                            "Eğitim alanında da kullanılan Minecraft, kodlama, matematik ve tarih derslerinde interaktif öğrenme aracı olarak değerlendirilir. Mod desteği ile oyun deneyimi sürekli genişletilir.",
                Developer = mojang,
                Publisher = microsoft,
                ReleaseDate = new DateTime(2011, 11, 18),
                Price = 299.99M,
                CoverImage = "minecraft-cover.jpg",
                IsActive = true,
                IsFeatured = true
            };

            var starfield = new Game
            {
                Title = "Starfield",
                Description = "Starfield, Bethesda Game Studios'un uzay temalı epik RPG oyunudur. Constellation organizasyonuna katılarak galaksinin en büyük gizemini keşfetmeye çıkarsınız.\n\n" +
                            "1000'den fazla gezegen ve ay keşfedilebilir, her biri kendine özgü ekosistem, flora, fauna ve kaynaklar barındırır. Procedural generation teknolojisi ile her oyuncuya benzersiz deneyimler sunar.\n\n" +
                            "Karakter oluşturma sistemi ile background, traits ve skills seçerek kendi hikayenizi yazabilirsiniz. Farklı fraksiyonlara katılabilir, çeşitli kariyer yolları izleyebilirsiniz.\n\n" +
                            "Uzay gemisi tasarımı ve yönetimi oyunun önemli unsurlarıdır. Geminizi özelleştirebilir, crew üyeleri işe alabilir ve uzay savaşlarına katılabilirsiniz. Outpost kurma sistemi ile kendi üslerinizi oluşturabilirsiniz.\n\n" +
                            "Combat sistemi hem FPS hem de melee savaşları destekler. Silah modifikasyonu, armor crafting ve skill tree sistemi ile karakterinizi geliştirebilirsiniz.\n\n" +
                            "Bethesda'nın geleneksel storytelling yaklaşımı ile zengin yan görevler, karmaşık karakter ilişkileri ve çoklu son seçenekleri bulunur. Mod desteği ile topluluk tarafından oluşturulan içerikler oyunu daha da zenginleştirir.",
                Developer = bethesda,
                Publisher = bethesdaPublisher,
                ReleaseDate = new DateTime(2023, 9, 6),
                Price = 999.99M,
                CoverImage = "starfield-cover.jpg",
                IsActive = true,
                IsFeatured = true
            };

            var rdr2 = new Game
            {
                Title = "Red Dead Redemption 2",
                Description = "Red Dead Redemption 2, Rockstar Games'in Vahşi Batı'da geçen epik açık dünya aksiyon-macera oyunudur. Arthur Morgan ve Van der Linde çetesinin hikayesini anlatır.\n\n" +
                            "1899 yılında geçen oyun, Amerika'nın endüstrileşme öncesi son günlerini yansıtır. Geniş haritada özgürce dolaşabilir, yan görevler yapabilir ve çeşitli aktivitelere katılabilirsiniz.\n\n" +
                            "Honor sistemi ile karakterinizin ahlaki seçimleri oyun dünyasını ve NPC'lerin tepkilerini etkiler. Hunting, fishing, gambling ve crafting gibi aktiviteler oyuna derinlik katar.\n\n" +
                            "At bakımı ve bonding sistemi ile sadık bir yol arkadaşı edinebilirsiniz. Dead Eye sistemi ile zamanı yavaşlatıp hassas nişan alabilirsiniz. Çeşitli silahlar ve taktikler kullanabilirsiniz.\n\n" +
                            "Red Dead Online modunda arkadaşlarınızla birlikte oynayabilir, kendi karakterinizi oluşturabilirsiniz. Bounty Hunter, Trader, Collector ve Moonshiner rolleri ile farklı kariyer yolları izleyebilirsiniz.\n\n" +
                            "Görsel sanatlar, ses tasarımı ve oyuncu performansları açısından oyun dünyasında yeni standartlar belirleyen RDR2, sinematik deneyim ve oynanış dengesini mükemmel şekilde kurar.",
                Developer = rockstar,
                Publisher = rockstarPublisher,
                ReleaseDate = new DateTime(2019, 12, 5),
                Price = 699.99M,
                CoverImage = "rdr2-cover.jpg",
                IsActive = true,
                IsFeatured = true
            };

            var acValhalla = new Game
            {
                Title = "Assassin's Creed Valhalla",
                Description = "Assassin's Creed Valhalla, Ubisoft'un Viking çağında geçen aksiyon-RPG oyunudur. Eivor adlı Viking savaşçısı olarak İngiltere'yi fethetmeye çıkarsınız.\n\n" +
                            "9. yüzyıl İngiltere'sinde geçen oyun, Norveç, İngiltere ve Vinland bölgelerini kapsar. Her bölge kendine özgü kültür, mimari ve tarihi olaylar barındırır. Settlement sistemi ile kendi Viking yerleşiminizi geliştirebilirsiniz.\n\n" +
                            "Dual-wielding sistemi ile iki silahı aynı anda kullanabilir, çeşitli savaş stilleri geliştirebilirsiniz. Raid mekanikleri ile manastırlara saldırabilir, ganimet toplayabilirsiniz.\n\n" +
                            "RPG elementleri güçlendirilmiştir: skill tree, gear customization ve character progression sistemi bulunur. Hidden Ones ve Order of the Ancients arasındaki gizli savaş oyunun temel hikayesini oluşturur.\n\n" +
                            "Mythology DLC'leri ile Asgard, Jotunheim ve Svartalfheim boyutlarını keşfedebilirsiniz. River Raids, Mastery Challenges ve seasonal events ile sürekli yeni içerik sunulur.\n\n" +
                            "Görsel sanatlar ve müzik açısından da etkileyici olan oyun, Viking kültürünü ve mitolojisini başarıyla yansıtır. Stealth ve combat dengesi ile Assassin's Creed serisinin en iyi oyunlarından biridir.",
                Developer = ubisoft,
                Publisher = ubisoftPublisher,
                ReleaseDate = new DateTime(2020, 11, 10),
                Price = 599.99M,
                CoverImage = "ac-valhalla-cover.jpg",
                IsActive = true
            };

            var forza5 = new Game
            {
                Title = "Forza Horizon 5",
                Description = "Forza Horizon 5, Playground Games'in Meksika'da geçen açık dünya yarış oyunudur. Horizon Festival'in en büyük etkinliğine katılarak dünyanın en iyi sürücüleriyle yarışırsınız.\n\n" +
                            "Meksika'nın çeşitli bölgelerini kapsayan geniş harita: çöller, yağmur ormanları, dağlar, plajlar ve şehirler bulunur. Dinamik hava sistemi ile farklı koşullarda yarışabilirsiniz.\n\n" +
                            "700'den fazla lisanslı araç bulunur: hypercar'lar, rally araçları, off-road araçları ve klasik otomobiller. Her araç kendine özgü handling ve performans özelliklerine sahiptir.\n\n" +
                            "EventLab ile kendi yarış rotalarınızı, oyun modlarınızı ve challenge'larınızı oluşturabilirsiniz. Horizon Arcade ile mini-game'ler oynayabilir, drift, speed trap ve stunt challenge'larına katılabilirsiniz.\n\n" +
                            "Multiplayer modlarında arkadaşlarınızla birlikte oynayabilir, eliminator battle royale moduna katılabilirsiniz. Car meets ve photo mode ile topluluk etkileşimi sağlanır.\n\n" +
                            "Ray tracing desteği ve 4K grafikler ile görsel açıdan etkileyici olan oyun, Xbox Game Pass ile de mevcuttur. Sürekli güncellemeler ile yeni araçlar, etkinlikler ve özellikler eklenmektedir.",
                Developer = new Developer { Name = "Playground Games", Country = "İngiltere", FoundedYear = 2010 },
                Publisher = microsoft,
                ReleaseDate = new DateTime(2021, 11, 9),
                Price = 599.99M,
                CoverImage = "forza5-cover.jpg",
                IsActive = true
            };

            var cyberpunk = new Game
            {
                Title = "Cyberpunk 2077",
                Description = "Cyberpunk 2077, CD Projekt Red'in Night City'de geçen gelecek temalı açık dünya RPG oyunudur. V adlı mercenary olarak şehrin en tehlikeli görevlerini üstlenirsiniz.\n\n" +
                            "2077 yılında geçen oyun, teknoloji ve insanlığın iç içe geçtiği distopik bir dünyayı yansıtır. Night City'nin altı farklı bölgesi her biri kendine özgü atmosfer ve karakter barındırır.\n\n" +
                            "Karakter oluşturma sistemi ile V'nin background'ını, görünümünü ve başlangıç yeteneklerini belirleyebilirsiniz. Street Kid, Nomad ve Corpo olmak üzere üç farklı başlangıç hikayesi bulunur.\n\n" +
                            "Combat sistemi FPS, melee ve hacking elementlerini birleştirir. Cyberware implantları ile karakterinizi geliştirebilir, yeni yetenekler kazanabilirsiniz. Stealth ve agresif oynanış stilleri desteklenir.\n\n" +
                            "Keanu Reeves'in canlandırdığı Johnny Silverhand karakteri oyunun merkezi figürlerinden biridir. Ana hikaye ve yan görevler oyuncunun ahlaki seçimlerini test eder.\n\n" +
                            "Phantom Liberty DLC'si ile yeni bölge, hikaye ve özellikler eklenmiştir. 2.0 güncellemesi ile oyun mekanikleri tamamen yenilenmiş, police system ve vehicle combat gibi yeni özellikler eklenmiştir.",
                Developer = cdProjekt,
                Publisher = cdProjektPublisher,
                ReleaseDate = new DateTime(2020, 12, 10),
                Price = 699.99M,
                CoverImage = "cyberpunk-cover.jpg",
                IsActive = true
            };

            var godOfWar = new Game
            {
                Title = "God of War",
                Description = "God of War, Santa Monica Studio'nun Kratos'un İskandinav mitolojisindeki destansı yolculuğunu anlatan aksiyon-macera oyunudur. Atreus ile birlikte dokuz diyarı keşfedersiniz.\n\n" +
                            "Yunan mitolojisinden İskandinav mitolojisine geçiş yapan Kratos, artık bir baba olarak oğlu Atreus'u yetiştirmeye çalışır. Hikaye, Faye'nin küllerini dağıtma görevi etrafında şekillenir.\n\n" +
                            "Over-the-shoulder kamera açısı ile daha yakın ve kişisel bir deneyim sunar. Leviathan Axe ve Blades of Chaos ile çeşitli savaş kombinasyonları yapabilirsiniz. Atreus'un ok atma yetenekleri savaşlarda kritik öneme sahiptir.\n\n" +
                            "Dokuz diyarın her biri kendine özgü düşmanları, bulmacaları ve hikayeleri barındırır. Norse mitolojisinin tanrıları, devleri ve yaratıkları ile karşılaşırsınız.\n\n" +
                            "Puzzle çözme, exploration ve combat dengesi mükemmel şekilde kurulmuştur. RPG elementleri ile Kratos'un yeteneklerini geliştirebilir, yeni armor ve talisman'lar edinebilirsiniz.\n\n" +
                            "Görsel sanatlar, müzik ve oyuncu performansları açısından oyun dünyasında yeni standartlar belirleyen God of War, 2018'de Game of the Year ödülünü kazanmıştır. Ragnarök ile hikaye devam etmiştir.",
                Developer = new Developer { Name = "Santa Monica Studio", Country = "ABD", FoundedYear = 1999 },
                Publisher = new Publisher { Name = "Sony Interactive Entertainment", Country = "Japonya", FoundedYear = 1993 },
                ReleaseDate = new DateTime(2022, 1, 14),
                Price = 799.99M,
                CoverImage = "god-of-war-cover.jpg",
                IsActive = true
            };

            // Yeni Steam oyunları
            var teamFortress2 = new Game
            {
                Title = "Team Fortress 2",
                Description = "Team Fortress 2, Valve'ın klasik takım tabanlı FPS oyunudur. 9 farklı karakter sınıfı ile stratejik savaşlar verirsiniz. Her sınıfın kendine özgü yetenekleri, zayıf yanları ve rolleri vardır.\n\n" +
                            "Scout hızlı ama zayıf, Heavy güçlü ama yavaş, Medic iyileştirme yapabilir, Engineer savunma yapıları kurabilir. Sniper uzun mesafeli, Spy gizli operasyonlar yapabilir.\n\n" +
                            "Payload, Capture Point, King of the Hill gibi farklı oyun modları bulunur. Her mod farklı taktikler ve stratejiler gerektirir. Takım çalışması ve koordinasyon başarı için kritiktir.\n\n" +
                            "Ücretsiz oyun modeli ile herkesin erişebileceği oyun, sürekli güncellemeler ile yeni hat'lar, silahlar ve cosmetic item'lar eklenmektedir. Mann vs Machine modu ile bot'larla savaşabilirsiniz.\n\n" +
                            "Trading sistemi ile oyuncular item'larını takas edebilir, Steam Market'te satabilir. Unusual hat'lar ve rare item'lar koleksiyoncular için değerli olabilir.\n\n" +
                            "Topluluk tarafından oluşturulan haritalar, mod'lar ve custom server'lar ile oyun deneyimi sürekli genişletilir. 15 yılı aşkın süredir aktif olan oyun, FPS türünün en uzun ömürlü oyunlarından biridir.",
                Developer = valve,
                Publisher = valvePublisher,
                ReleaseDate = new DateTime(2007, 10, 10),
                Price = 0,
                CoverImage = "tf2-cover.jpg",
                IsActive = true
            };

            var rust = new Game
            {
                Title = "Rust",
                Description = "Rust, Facepunch Studios'un hayatta kalma temalı multiplayer oyunudur. Kaynak toplama, üs kurma ve diğer oyuncularla mücadele etme üzerine kurulu zorlu bir deneyim sunar.\n\n" +
                            "Her sunucu reset'inde tüm oyuncular sıfırdan başlar. Taş, odun, metal gibi kaynakları toplar, araçlar ve silahlar craft edersiniz. Base building sistemi ile güvenli üsler kurmanız gerekir.\n\n" +
                            "PvP savaşları oyunun merkezi unsurlarından biridir. Raid'ler, clan savaşları ve solo oyuncu mücadeleleri sürekli devam eder. Stealth ve strateji hayatta kalma için kritiktir.\n\n" +
                            "Farklı biyomlar (desert, arctic, temperate) her biri kendine özgü kaynaklar ve zorluklar sunar. Monument'lar özel loot'lar barındırır ama tehlikeli NPC'ler tarafından korunur.\n\n" +
                            "Helicopter, Bradley APC ve Scientist NPC'leri ile PvE elementleri de bulunur. Cargo Ship ve Oil Rig gibi event'ler özel ödüller sunar. Workbench sistemi ile item'ları geliştirebilirsiniz.\n\n" +
                            "Sürekli güncellemeler ile yeni içerikler eklenmektedir. Electricity system, modular vehicle system ve underwater base building gibi özellikler oyunu daha da karmaşık hale getirir.",
                Developer = facepunch,
                Publisher = facepunchPublisher,
                ReleaseDate = new DateTime(2018, 2, 8),
                Price = 399.99M,
                CoverImage = "rust-cover.jpg",
                IsActive = true
            };

            var arkSurvival = new Game
            {
                Title = "ARK: Survival Evolved",
                Description = "ARK: Survival Evolved, Studio Wildcard'ın dinozorlarla dolu açık dünyada hayatta kalma mücadelesi sunan oyunudur. Dinozorları evcilleştirip üs kurarak hayatta kalmaya çalışırsınız.\n\n" +
                            "150'den fazla dinozor türü bulunur, her biri kendine özgü yeteneklere sahiptir. T-Rex güçlü savaşçı, Pteranodon uçma, Brontosaurus taşıma kapasitesi sunar. Taming sistemi ile dinozorları evcilleştirebilirsiniz.\n\n" +
                            "Base building sistemi ile karmaşık üsler kurabilir, elektrik sistemi, otomasyon ve farming yapabilirsiniz. Tribe sistemi ile arkadaşlarınızla birlikte çalışabilirsiniz.\n\n" +
                            "Farklı haritalar (The Island, Scorched Earth, Aberration, Extinction) her biri kendine özgü dinozorlar ve zorluklar sunar. Boss fight'lar ile end-game içerik bulunur.\n\n" +
                            "PvP ve PvE sunucuları mevcuttur. PvP'de tribe savaşları, raid'ler ve territory control önemlidir. PvE'de daha çok exploration ve building odaklı oynanış vardır.\n\n" +
                            "Mod desteği ile topluluk tarafından oluşturulan haritalar, dinozorlar ve özellikler eklenebilir. Survival of the Fittest battle royale modu da bulunur.",
                Developer = studioWildcard,
                Publisher = studioWildcardPublisher,
                ReleaseDate = new DateTime(2017, 8, 29),
                Price = 599.99M,
                CoverImage = "ark-cover.jpg",
                IsActive = true
            };

            var fallout4 = new Game
            {
                Title = "Fallout 4",
                Description = "Fallout 4, Bethesda Game Studios'un nükleer savaş sonrası Boston'da geçen açık dünya RPG oyunudur. Sole Survivor olarak Vault 111'den çıkarak oğlunuzu aramaya başlarsınız.\n\n" +
                            "Commonwealth'in geniş haritasında özgürce dolaşabilir, yan görevler yapabilir ve çeşitli fraksiyonlarla etkileşime girebilirsiniz. Institute, Brotherhood of Steel, Railroad ve Minutemen arasında seçim yapmanız gerekir.\n\n" +
                            "Settlement building sistemi ile kendi şehirlerinizi kurabilir, NPC'leri yerleştirebilir ve savunma sistemleri oluşturabilirsiniz. Vault-Tec Workshop DLC'si ile kendi vault'unuzu inşa edebilirsiniz.\n\n" +
                            "VATS sistemi ile zamanı yavaşlatıp hassas nişan alabilirsiniz. Power Armor sistemi ile güçlü zırhlar kullanabilirsiniz. Crafting ve modding sistemi ile silahlarınızı özelleştirebilirsiniz.\n\n" +
                            "Far Harbor, Nuka-World ve Automatron DLC'leri ile yeni bölgeler, hikayeler ve özellikler eklenir. Companion sistemi ile farklı karakterlerle yolculuk yapabilirsiniz.\n\n" +
                            "Görsel sanatlar ve atmosfer açısından post-apocalyptic dünyayı başarıyla yansıtan oyun, Fallout serisinin en kapsamlı oyunlarından biridir.",
                Developer = bethesdaGameStudios,
                Publisher = bethesdaPublisher,
                ReleaseDate = new DateTime(2015, 11, 10),
                Price = 499.99M,
                CoverImage = "fallout4-cover.jpg",
                IsActive = true
            };

            var skyrim = new Game
            {
                Title = "The Elder Scrolls V: Skyrim",
                Description = "The Elder Scrolls V: Skyrim, Bethesda Game Studios'un fantastik dünya Tamriel'de geçen epik RPG oyunudur. Dragonborn olarak ejderhaları yenmek ve Alduin'u durdurmak için maceraya atılırsınız.\n\n" +
                            "Skyrim'in geniş haritasında özgürce dolaşabilir, yan görevler yapabilir ve çeşitli guild'lere katılabilirsiniz. Thieves Guild, Dark Brotherhood, College of Winterhold ve Companions gibi organizasyonlar bulunur.\n\n" +
                            "Shout sistemi ile ejderha dilini öğrenip güçlü yetenekler kazanabilirsiniz. Magic, combat ve stealth sistemleri ile farklı oynanış stilleri geliştirebilirsiniz. Skill tree sistemi ile karakterinizi özelleştirebilirsiniz.\n\n" +
                            "Modding topluluğu ile oyun sürekli yenilenmektedir. Visual mod'lar, gameplay mod'ları ve total conversion mod'ları ile oyun deneyimi tamamen değiştirilebilir.\n\n" +
                            "Dawnguard, Hearthfire ve Dragonborn DLC'leri ile yeni bölgeler, hikayeler ve özellikler eklenir. Vampire ve Werewolf olma seçenekleri bulunur.\n\n" +
                            "Görsel sanatlar, müzik ve atmosfer açısından oyun dünyasında yeni standartlar belirleyen Skyrim, 2011'de Game of the Year ödülünü kazanmıştır ve hala aktif bir topluluğa sahiptir.",
                Developer = bethesdaGameStudios,
                Publisher = bethesdaPublisher,
                ReleaseDate = new DateTime(2011, 11, 11),
                Price = 399.99M,
                CoverImage = "skyrim-cover.jpg",
                IsActive = true
            };

            var portal2 = new Game
            {
                Title = "Portal 2",
                Description = "Portal 2, Valve'ın portal silahı ile fizik bulmacalarını çözdüğünüz zeka ve yaratıcılık gerektiren oyunudur. Chell olarak Aperture Science laboratuvarında test odalarından geçmeye çalışırsınız.\n\n" +
                            "Portal silahı ile iki nokta arasında geçiş yapabilir, momentum'u koruyarak yüksek hızlara ulaşabilirsiniz. Gel, Repulsion Gel ve Propulsion Gel gibi özel malzemeler bulmacaları çeşitlendirir.\n\n" +
                            "Single-player kampanyası GLaDOS ile devam eden hikayeyi anlatır. Wheatley adlı yeni karakter ile Aperture Science'ın derinliklerine inersiniz. Cooperative modunda iki oyuncu birlikte çalışır.\n\n" +
                            "Test Chamber'lar giderek karmaşıklaşır, fizik kurallarını anlamak ve yaratıcı çözümler bulmak gerekir. Portal placement, timing ve momentum hesaplamaları kritik öneme sahiptir.\n\n" +
                            "Level editor ile kendi bulmacalarınızı oluşturabilir, topluluk ile paylaşabilirsiniz. Workshop entegrasyonu ile binlerce kullanıcı tarafından oluşturulan seviyeler oynanabilir.\n\n" +
                            "Humor, storytelling ve puzzle design açısından mükemmel olan Portal 2, oyun dünyasında yeni standartlar belirlemiştir ve puzzle türünün en iyi örneklerinden biridir.",
                Developer = valve,
                Publisher = valvePublisher,
                ReleaseDate = new DateTime(2011, 4, 19),
                Price = 199.99M,
                CoverImage = "portal2-cover.jpg",
                IsActive = true
            };

            var left4Dead2 = new Game
            {
                Title = "Left 4 Dead 2",
                Description = "Left 4 Dead 2, Valve'ın zombi apokaliptik dünyada geçen 4 kişilik kooperatif hayatta kalma oyunudur. Survivor'lar olarak güvenli bölgeye ulaşmaya çalışırsınız.\n\n" +
                            "Dört farklı survivor karakteri ile oynayabilirsiniz: Coach, Ellis, Nick ve Rochelle. Her karakterin kendine özgü kişiliği ve diyalogları vardır. AI Director sistemi ile her oynanış farklı deneyim sunar.\n\n" +
                            "Çeşitli silahlar kullanabilirsiniz: melee silahları, tüfekler, pompalı tüfekler ve özel silahlar. Molotov, pipe bomb ve bile bomb gibi throwable item'lar stratejik öneme sahiptir.\n\n" +
                            "Special Infected'lar oyunu zorlaştırır: Tank, Witch, Boomer, Hunter, Smoker, Jockey, Charger ve Spitter. Her biri farklı yeteneklere sahiptir ve takım çalışması gerektirir.\n\n" +
                            "Campaign modunda farklı haritalar bulunur: Dead Center, Dark Carnival, Swamp Fever, Hard Rain, The Parish. Versus modunda survivor veya infected olarak oynayabilirsiniz.\n\n" +
                            "Survival modunda mümkün olduğunca uzun süre hayatta kalmaya çalışırsınız. Scavenge modunda gaz bidonlarını toplar, generator'ları çalıştırırsınız. Mod desteği ile topluluk tarafından oluşturulan haritalar oynanabilir.",
                Developer = valve,
                Publisher = valvePublisher,
                ReleaseDate = new DateTime(2009, 11, 17),
                Price = 199.99M,
                CoverImage = "l4d2-cover.jpg",
                IsActive = true
            };

            var terraria = new Game
            {
                Title = "Terraria",
                Description = "Terraria, Re-Logic'in 2D sandbox oyunudur. Keşfet, inşa et, savaş ve hayatta kalma elementlerini birleştiren benzersiz bir deneyim sunar.\n\n" +
                            "Procedural generation ile her dünya benzersizdir. Yeraltı mağaraları, floating islands, dungeon'lar ve temple'lar keşfedilmeyi bekler. Mining, building ve crafting oyunun temel unsurlarıdır.\n\n" +
                            "500'den fazla düşman türü bulunur, her biri farklı loot ve crafting material'ları düşürür. Boss fight'lar ile end-game içerik sunulur. Hardmode ile oyun daha da zorlaşır.\n\n" +
                            "NPC sistemi ile kendi şehrinizi kurabilirsiniz. Her NPC'nin kendine özgü ihtiyaçları ve satış yaptığı item'lar vardır. Pylons sistemi ile hızlı seyahat edebilirsiniz.\n\n" +
                            "Multiplayer modunda arkadaşlarınızla birlikte oynayabilir, büyük projeler gerçekleştirebilirsiniz. PvP arena'ları, adventure maps ve roleplay sunucuları bulunur.\n\n" +
                            "Sürekli güncellemeler ile yeni içerikler eklenmektedir. Journey's End güncellemesi ile master mode, golf, bestiary ve daha fazlası eklendi. Mod desteği ile oyun deneyimi sürekli genişletilir.",
                Developer = reLogic,
                Publisher = reLogicPublisher,
                ReleaseDate = new DateTime(2011, 5, 16),
                Price = 99.99M,
                CoverImage = "terraria-cover.jpg",
                IsActive = true
            };

            var stardewValley = new Game
            {
                Title = "Stardew Valley",
                Description = "Stardew Valley, ConcernedApe'in çiftlik simülasyonu RPG oyunudur. Büyükbabadan miras kalan çiftliği canlandırmaya çalışırsınız.\n\n" +
                            "Çiftlik yönetimi oyunun merkezi unsurlarından biridir. Mahsul yetiştirme, hayvan bakımı, ağaç dikme ve balıkçılık yapabilirsiniz. Her mevsim farklı mahsuller ve aktiviteler sunar.\n\n" +
                            "Pelican Town'da yaşayan NPC'lerle ilişki kurabilir, evlenebilir ve aile kurabilirsiniz. Her karakterin kendine özgü kişiliği, hikayesi ve favori item'ları vardır.\n\n" +
                            "Mining sistemi ile yeraltı mağaralarını keşfedebilir, mineral ve cevher toplayabilirsiniz. Combat sistemi ile düşmanlarla savaşabilir, nadir item'lar elde edebilirsiniz.\n\n" +
                            "Crafting sistemi ile araçlar, eşyalar ve binalar inşa edebilirsiniz. Community Center'ı tamamlayarak şehri geliştirebilirsiniz. Joja Mart ile rekabet edebilirsiniz.\n\n" +
                            "Multiplayer modunda arkadaşlarınızla birlikte çiftlik işletmek mümkündür. Mod desteği ile yeni içerikler, karakterler ve özellikler eklenebilir. Relaxing gameplay ve charming art style ile stress relief sağlar.",
                Developer = concernedApe,
                Publisher = concernedApePublisher,
                ReleaseDate = new DateTime(2016, 2, 26),
                Price = 149.99M,
                CoverImage = "stardew-valley-cover.jpg",
                IsActive = true
            };

            var amongUs = new Game
            {
                Title = "Among Us",
                Description = "Among Us, InnerSloth'un sosyal dedüksiyon oyunudur. Crewmate'lar olarak görevleri tamamlamaya çalışırken, Impostor gizlice sabotaj yapmaya çalışır.\n\n" +
                            "4-15 oyuncu arasında oynanabilir. Crewmate'lar görevleri tamamlayarak oyunu kazanabilir, Impostor ise diğer oyuncuları öldürerek veya sabotaj yaparak kazanabilir.\n\n" +
                            "Emergency meetings ile şüpheli davranışları tartışabilir, voting sistemi ile Impostor'u atmaya çalışabilirsiniz. Vent kullanımı, sabotaj ve kill cooldown Impostor'ın temel yetenekleridir.\n\n" +
                            "Farklı haritalar bulunur: The Skeld, MIRA HQ, Polus ve The Airship. Her harita kendine özgü görevler ve vent sistemine sahiptir. Custom game ayarları ile oyun deneyimini özelleştirebilirsiniz.\n\n" +
                            "Cosmetic item'lar ile karakterinizi özelleştirebilirsiniz. Pets, hats ve skins ile kişiselleştirme yapabilirsiniz. Colorblind support ve accessibility options bulunur.\n\n" +
                            "Pandemic döneminde popülerlik kazanan oyun, sosyal etkileşim ve strateji gerektiren gameplay'i ile dikkat çekmiştir. Cross-platform desteği ile farklı cihazlardan oynanabilir.",
                Developer = innerSloth,
                Publisher = innerSlothPublisher,
                ReleaseDate = new DateTime(2018, 6, 15),
                Price = 49.99M,
                CoverImage = "among-us-cover.jpg",
                IsActive = true
            };

            var phasmophobia = new Game
            {
                Title = "Phasmophobia",
                Description = "Phasmophobia, Kinetic Games'in 4 kişilik korku oyunudur. Hayalet avcısı olarak paranormal aktiviteleri araştırır, hayalet türünü belirlemeye çalışırsınız.\n\n" +
                            "Çeşitli hayalet türleri bulunur: Spirit, Demon, Poltergeist, Banshee, Jinn, Mare, Revenant ve daha fazlası. Her hayalet türünün kendine özgü özellikleri ve davranışları vardır.\n\n" +
                            "Farklı ekipmanlar kullanabilirsiniz: EMF Reader, Spirit Box, UV Light, Video Camera, Ouija Board gibi. Her ekipman farklı kanıt türleri sağlar. Sanity sistemi ile karakterinizin akıl sağlığını takip edersiniz.\n\n" +
                            "Çeşitli haritalar bulunur: farmhouse'lar, okullar, hastaneler ve hapishaneler. Her harita farklı boyut ve zorluk seviyesine sahiptir. Professional difficulty'de hayalet daha agresif olur.\n\n" +
                            "Voice recognition sistemi ile hayaletle konuşabilirsiniz. Hayaletin adını söyleyerek tepkisini test edebilirsiniz. Ouija Board kullanarak sorular sorabilirsiniz.\n\n" +
                            "VR desteği ile daha immersif deneyim yaşayabilirsiniz. Multiplayer modunda arkadaşlarınızla birlikte korku dolu anlar yaşayabilirsiniz. Sürekli güncellemeler ile yeni hayalet türleri ve özellikler eklenmektedir.",
                Developer = kineticGames,
                Publisher = kineticGamesPublisher,
                ReleaseDate = new DateTime(2020, 9, 18),
                Price = 199.99M,
                CoverImage = "phasmophobia-cover.jpg",
                IsActive = true
            };

            var deadByDaylight = new Game
            {
                Title = "Dead by Daylight",
                Description = "Dead by Daylight, Behaviour Interactive'in asimetrik korku oyunudur. 4 Survivor veya 1 Killer olarak oynayabilirsiniz. Survivor'lar generator'ları tamamlayarak kaçmaya çalışır.\n\n" +
                            "Killer'lar farklı yeteneklere sahiptir: Trapper tuzak kurabilir, Wraith görünmez olabilir, Hillbilly chainsaw kullanabilir. Her Killer'ın kendine özgü power'ı ve add-on'ları vardır.\n\n" +
                            "Survivor'lar farklı perk'ler kullanabilir: Sprint Burst, Self-Care, Decisive Strike, Borrowed Time gibi. Perk sistemi ile karakterinizi özelleştirebilirsiniz. Item'lar ve offering'ler stratejik öneme sahiptir.\n\n" +
                            "Çeşitli haritalar bulunur: Autohaven Wreckers, Coldwind Farm, Crotus Prenn Asylum, Haddonfield gibi. Her harita farklı tile set'leri ve pallet spawn'larına sahiptir.\n\n" +
                            "Licensed Killer'lar ve Survivor'lar bulunur: Michael Myers, Freddy Krueger, Leatherface, Ghostface gibi. Original karakterler de mevcuttur. Cosmetic item'lar ile karakterinizi özelleştirebilirsiniz.\n\n" +
                            "Ranked sistem ile oyuncular kendi seviyelerindeki rakiplerle eşleşir. Events ve seasonal content ile sürekli yeni içerik sunulur. Cross-play desteği ile farklı platformlardan oyuncular birlikte oynayabilir.",
                Developer = behaviourInteractive,
                Publisher = behaviourInteractivePublisher,
                ReleaseDate = new DateTime(2016, 6, 14),
                Price = 299.99M,
                CoverImage = "dbd-cover.jpg",
                IsActive = true
            };

            var rainbowSixSiege = new Game
            {
                Title = "Tom Clancy's Rainbow Six Siege",
                Description = "Rainbow Six Siege, Ubisoft Montreal'in taktiksel FPS oyunudur. 5v5 formatında oynanan oyunda Attack ve Defense takımları arasında yoğun savaşlar geçer.\n\n" +
                            "60'dan fazla Operator bulunur, her biri kendine özgü gadget'ları ve yetenekleri vardır. Ash saldırı odaklı, Rook savunma odaklı, Thermite hard breach yapabilir. Her Operator'ın kendine özgü loadout'u vardır.\n\n" +
                            "Destructible environment sistemi ile duvarları, zeminleri ve tavanları yıkabilirsiniz. Reinforced wall'lar, barricade'lar ve trap'lar savunma için kritiktir. Drone kullanımı bilgi toplama için önemlidir.\n\n" +
                            "Farklı oyun modları bulunur: Bomb, Secure Area, Hostage. Ranked, Unranked, Quick Match ve Custom Game seçenekleri mevcuttur. Map pool sürekli güncellenir.\n\n" +
                            "Seasonal updates ile yeni Operator'lar, haritalar ve özellikler eklenir. Battle Pass sistemi ile cosmetic item'lar kazanabilirsiniz. Pro League ile profesyonel e-spor sahnesinde yer alır.\n\n" +
                            "Tactical gameplay ve teamwork odaklı oynanış ile FPS türünde yeni standartlar belirleyen oyun, sürekli güncellemeler ile aktif tutulmaktadır. Cross-play ve cross-progression desteği bulunur.",
                Developer = ubisoftMontreal,
                Publisher = ubisoftPublisher,
                ReleaseDate = new DateTime(2015, 12, 1),
                Price = 399.99M,
                CoverImage = "r6-siege-cover.jpg",
                IsActive = true
            };

            var warframe = new Game
            {
                Title = "Warframe",
                Description = "Warframe, Digital Extremes'in ücretsiz sci-fi aksiyon oyunudur. Tenno savaşçısı olarak galaksiyi keşfeder, düşmanlarla savaşır ve güçlü Warframe'ler kullanırsınız.\n\n" +
                            "40'dan fazla Warframe bulunur, her biri kendine özgü yeteneklere sahiptir. Excalibur melee odaklı, Mag magnetic güçler kullanır, Rhino tank rolü üstlenir. Modding sistemi ile Warframe'lerinizi özelleştirebilirsiniz.\n\n" +
                            "Çeşitli silah türleri mevcuttur: Primary, Secondary ve Melee silahları. Her silah kendine özgü handling ve damage türüne sahiptir. Riven mod'ları ile silahlarınızı güçlendirebilirsiniz.\n\n" +
                            "Farklı mission türleri bulunur: Exterminate, Survival, Defense, Interception, Spy gibi. Open world bölgeleri (Plains of Eidolon, Orb Vallis, Cambion Drift) geniş keşif alanları sunar.\n\n" +
                            "Clan sistemi ile arkadaşlarınızla birlikte oynayabilir, dojo inşa edebilirsiniz. Trading sistemi ile platinum kazanabilir, rare item'lar satabilirsiniz. Fashion Frame ile karakterinizi özelleştirebilirsiniz.\n\n" +
                            "Sürekli güncellemeler ile yeni içerikler eklenmektedir. New War, Duviri Paradox gibi büyük expansion'lar oyunu genişletir. Cross-platform play ile farklı cihazlardan oynanabilir.",
                Developer = digitalExtremes,
                Publisher = digitalExtremesPublisher,
                ReleaseDate = new DateTime(2013, 3, 25),
                Price = 0,
                CoverImage = "warframe-cover.jpg",
                IsActive = true
            };

            var pathOfExile = new Game
            {
                Title = "Path of Exile",
                Description = "Path of Exile, Grinding Gear Games'in ücretsiz aksiyon RPG oyunudur. Karanlık fantastik dünya Wraeclast'ta geçen oyun, Diablo serisinin spiritual successor'ı olarak kabul edilir.\n\n" +
                            "7 farklı karakter sınıfı bulunur: Marauder, Ranger, Witch, Duelist, Templar, Shadow ve Scion. Her sınıf farklı başlangıç noktalarına sahiptir. Passive skill tree ile karakterinizi özelleştirebilirsiniz.\n\n" +
                            "Gem sistemi ile skill'leri özelleştirebilirsiniz. Support gem'ler ile skill'lerinizi güçlendirebilir, farklı efektler ekleyebilirsiniz. Socket ve link sistemi ile gem'leri birleştirebilirsiniz.\n\n" +
                            "League sistemi ile 3 ayda bir yeni içerikler eklenir. Standard, Hardcore, SSF (Solo Self-Found) gibi farklı oyun modları bulunur. Endgame content olarak Maps, Delve, Heist gibi sistemler vardır.\n\n" +
                            "Trading sistemi ile item'ları takas edebilirsiniz. Currency item'ları (Orbs) hem crafting hem de trading için kullanılır. Guild sistemi ile arkadaşlarınızla birlikte oynayabilirsiniz.\n\n" +
                            "Sürekli güncellemeler ile yeni expansion'lar eklenmektedir. Synthesis, Metamorph, Ritual gibi league'ler oyuna yeni mekanikler getirir. Microtransaction'lar sadece cosmetic item'lar için kullanılır.",
                Developer = grindingGearGames,
                Publisher = grindingGearGamesPublisher,
                ReleaseDate = new DateTime(2013, 10, 23),
                Price = 0,
                CoverImage = "poe-cover.jpg",
                IsActive = true
            };

            var warThunder = new Game
            {
                Title = "War Thunder",
                Description = "War Thunder, Gaijin Entertainment'in ücretsiz askeri simülasyon oyunudur. Tank, uçak ve gemi savaşlarını bir arada sunan benzersiz bir deneyim sağlar.\n\n" +
                            "Çeşitli ülkelerin askeri araçları bulunur: ABD, Almanya, Sovyetler Birliği, İngiltere, Japonya, İtalya, Fransa, İsveç, Çin ve İsrail. Her ülke kendine özgü araç filosuna sahiptir.\n\n" +
                            "Farklı araç türleri mevcuttur: Light tanks, medium tanks, heavy tanks, tank destroyers, SPAA (Self-Propelled Anti-Aircraft). Aircraft'lar fighter, bomber, attacker kategorilerine ayrılır.\n\n" +
                            "Realistic ve Simulator modları ile gerçekçi savaş deneyimi yaşayabilirsiniz. Arcade modu daha casual oyuncular için tasarlanmıştır. Naval forces ile deniz savaşları da mümkündür.\n\n" +
                            "Research tree sistemi ile araçları geliştirebilirsiniz. Crew skills ve vehicle modifications ile performansı artırabilirsiniz. Squadron sistemi ile arkadaşlarınızla birlikte oynayabilirsiniz.\n\n" +
                            "Sürekli güncellemeler ile yeni araçlar, haritalar ve özellikler eklenmektedir. Historical accuracy önem verilen oyun, askeri tarih meraklıları için ideal bir platform sunar.",
                Developer = gaijinEntertainment,
                Publisher = gaijinEntertainmentPublisher,
                ReleaseDate = new DateTime(2013, 8, 15),
                Price = 0,
                CoverImage = "war-thunder-cover.jpg",
                IsActive = true
            };

            var destiny2 = new Game
            {
                Title = "Destiny 2",
                Description = "Destiny 2, Bungie'nin ücretsiz looter shooter oyunudur. Guardian olarak güneş sistemini korur, güçlü silahlar ve armor'lar toplarsınız.\n\n" +
                            "Üç farklı sınıf bulunur: Hunter, Titan ve Warlock. Her sınıfın kendine özgü ability'leri ve super'ları vardır. Subclass sistemi ile farklı element'ler kullanabilirsiniz: Solar, Arc, Void, Stasis ve Strand.\n\n" +
                            "Çeşitli aktivite türleri mevcuttur: Strikes, Raids, Dungeons, Gambit, Crucible. PvE ve PvP içerikleri dengeli şekilde sunulur. Seasonal content ile sürekli yeni hikayeler eklenir.\n\n" +
                            "Weapon crafting sistemi ile kendi silahlarınızı oluşturabilirsiniz. Exotic weapon'lar ve armor'lar özel yeteneklere sahiptir. Mod sistemleri ile build'lerinizi özelleştirebilirsiniz.\n\n" +
                            "Clan sistemi ile arkadaşlarınızla birlikte oynayabilirsiniz. Cross-play desteği ile farklı platformlardan oyuncular birlikte oynayabilir. Transmog sistemi ile görünümünüzü özelleştirebilirsiniz.\n\n" +
                            "Expansion'lar ile yeni bölgeler, hikayeler ve özellikler eklenir: Beyond Light, The Witch Queen, Lightfall gibi. Free-to-play modeli ile temel içerik ücretsizdir, expansion'lar ayrıca satın alınır.",
                Developer = bungie,
                Publisher = bungiePublisher,
                ReleaseDate = new DateTime(2019, 10, 1),
                Price = 0,
                CoverImage = "destiny2-cover.jpg",
                IsActive = true
            };

            var apexLegends = new Game
            {
                Title = "Apex Legends",
                Description = "Apex Legends, Respawn Entertainment'in ücretsiz battle royale FPS oyunudur. Karakter tabanlı yetenekler ve taktiksel oynanış ile türünün öncülerinden biridir.\n\n" +
                            "20'den fazla Legend bulunur, her biri kendine özgü tactical, passive ve ultimate ability'lere sahiptir. Wraith portal açabilir, Gibraltar shield kurabilir, Lifeline iyileştirme yapabilir.\n\n" +
                            "Squad tabanlı oynanış ile 3 kişilik takımlar halinde yarışırsınız. Ping sistemi ile iletişim kurmadan takım arkadaşlarınızla koordinasyon sağlayabilirsiniz. Respawn sistemi ile ölen takım arkadaşınızı geri getirebilirsiniz.\n\n" +
                            "Çeşitli silah türleri mevcuttur: Assault Rifles, SMG'ler, Sniper Rifles, Shotgun'lar ve LMG'ler. Her silah kendine özgü recoil pattern'ına sahiptir. Attachment sistemi ile silahlarınızı özelleştirebilirsiniz.\n\n" +
                            "Farklı haritalar bulunur: World's Edge, Storm Point, Olympus, Broken Moon ve Kings Canyon. Her harita kendine özgü özellikler ve oynanış dinamikleri sunar. Ring sistemi oyuncuları birbirine yaklaştırır.\n\n" +
                            "Ranked sistem ile oyuncular kendi seviyelerindeki rakiplerle eşleşir. Seasonal updates ile yeni Legend'lar, silahlar ve haritalar eklenir. ALGS (Apex Legends Global Series) ile profesyonel e-spor sahnesinde yer alır.",
                Developer = respawnEntertainment,
                Publisher = respawnEntertainmentPublisher,
                ReleaseDate = new DateTime(2020, 11, 4),
                Price = 0,
                CoverImage = "apex-legends-cover.jpg",
                IsActive = true
            };

            var rocketLeague = new Game
            {
                Title = "Rocket League",
                Description = "Rocket League, Psyonix'in futbol ve araba yarışının birleşimi olan fizik tabanlı spor oyunudur. Araçlarla futbol oynayarak gol atmaya çalışırsınız.\n\n" +
                            "Fizik motoru oyunun merkezi unsurlarından biridir. Aerials, wall shots, ceiling shots gibi gelişmiş teknikler öğrenmeniz gerekir. Boost management ve positioning kritik öneme sahiptir.\n\n" +
                            "Farklı oyun modları bulunur: Soccar (klasik futbol), Hoops (basketbol), Snow Day (hokey), Dropshot (volleyball). Rumble modunda power-up'lar kullanabilirsiniz.\n\n" +
                            "Ranked sistem ile oyuncular kendi seviyelerindeki rakiplerle eşleşir. 1v1, 2v2, 3v3 ve 4v4 formatları mevcuttur. Tournament sistemi ile düzenli turnuvalara katılabilirsiniz.\n\n" +
                            "Customization sistemi ile araçlarınızı özelleştirebilirsiniz: body, wheels, boost, trail, goal explosion gibi. Trading sistemi ile item'ları takas edebilirsiniz.\n\n" +
                            "Esports sahnesinde büyük turnuvalara ev sahipliği yapar. RLCS (Rocket League Championship Series) ile profesyonel oyuncular yarışır. Cross-platform play ile farklı cihazlardan oynanabilir.",
                Developer = psyonix,
                Publisher = psyonixPublisher,
                ReleaseDate = new DateTime(2015, 7, 7),
                Price = 0,
                CoverImage = "rocket-league-cover.jpg",
                IsActive = true
            };

            var fallGuys = new Game
            {
                Title = "Fall Guys: Ultimate Knockout",
                Description = "Fall Guys: Ultimate Knockout, Mediatonic'in ücretsiz battle royale platform oyunudur. 60 oyuncu ile başlayan eğlenceli yarışlarda son hayatta kalan olmaya çalışırsınız.\n\n" +
                            "Çeşitli round türleri bulunur: Race (yarış), Survival (hayatta kalma), Team (takım), Hunt (av) ve Logic (mantık). Her round farklı beceriler gerektirir: koordinasyon, strateji ve şans.\n\n" +
                            "Fizik tabanlı oynanış ile karakteriniz gerçekçi şekilde hareket eder. Grabbing, diving ve jumping mekanikleri ile diğer oyuncularla etkileşime girebilirsiniz. Obstacle course'lar giderek zorlaşır.\n\n" +
                            "Customization sistemi ile karakterinizi özelleştirebilirsiniz: costume'lar, pattern'lar, color'lar ve emote'lar. Battle Pass sistemi ile seasonal content kazanabilirsiniz.\n\n" +
                            "Squad mode ile arkadaşlarınızla birlikte oynayabilirsiniz. Creative mode ile kendi level'larınızı oluşturabilirsiniz. Party system ile büyük gruplar halinde oynayabilirsiniz.\n\n" +
                            "Cross-platform play ile farklı cihazlardan oynanabilir. Seasonal events ve collaborations ile sürekli yeni içerik sunulur. Family-friendly gameplay ile her yaştan oyuncuya hitap eder.",
                Developer = mediatonic,
                Publisher = mediatonicPublisher,
                ReleaseDate = new DateTime(2022, 6, 21),
                Price = 0,
                CoverImage = "fall-guys-cover.jpg",
                IsActive = true
            };

            var hades = new Game
            {
                Title = "Hades",
                Description = "Hades, Supergiant Games'in roguelike aksiyon oyunudur. Zagreus olarak Yunan mitolojisindeki cehennemden kaçmaya çalışırsınız. Her run farklı deneyim sunar.\n\n" +
                            "Roguelike mekanikleri ile her ölümde baştan başlarsınız, ancak permanent upgrades ile güçlenirsiniz. Mirror of Night ile karakterinizi geliştirebilirsiniz. Keepsakes ve Companions ile ek güçler kazanabilirsiniz.\n\n" +
                            "Çeşitli silah türleri mevcuttur: Stygian Blade, Heart-Seeking Bow, Shield of Chaos, Adamant Rail, Twin Fists ve Varatha. Her silahın kendine özgü attack pattern'ı vardır.\n\n" +
                            "Olympian gods'dan boon'lar alarak yeteneklerinizi geliştirebilirsiniz: Zeus lightning, Poseidon water, Athena deflect gibi. Boon kombinasyonları ile güçlü build'ler oluşturabilirsiniz.\n\n" +
                            "Hikaye anlatımı oyunun güçlü yanlarından biridir. Her run'da yeni diyaloglar ve karakter gelişimi görürsünüz. NPC'lerle ilişki kurabilir, romantik bağlar geliştirebilirsiniz.\n\n" +
                            "Görsel sanatlar ve müzik açısından mükemmel olan oyun, 2020'de Game of the Year ödüllerini kazanmıştır. God Mode ile zorluk seviyesini ayarlayabilirsiniz. Mod desteği ile topluluk tarafından oluşturulan içerikler eklenebilir.",
                Developer = supergiantGames,
                Publisher = supergiantGamesPublisher,
                ReleaseDate = new DateTime(2020, 9, 17),
                Price = 299.99M,
                CoverImage = "hades-cover.jpg",
                IsActive = true
            };

            var discoElysium = new Game
            {
                Title = "Disco Elysium - The Final Cut",
                Description = "Disco Elysium - The Final Cut, ZA/UM'un detektif RPG oyunudur. Revachol şehrinde geçen oyunda, amnezi yaşayan bir dedektif olarak cinayet çözmeye çalışırsınız.\n\n" +
                            "Dice-based skill system ile karakterinizi özelleştirebilirsiniz: Intellect, Psyche, Physique ve Motorics. Her skill'in kendine özgü diyalog seçenekleri ve yetenekleri vardır.\n\n" +
                            "Thought Cabinet sistemi ile karakterinizin düşünce süreçlerini geliştirebilirsiniz. Internalized thoughts karakterinizin kişiliğini ve yeteneklerini etkiler. Political alignment sistemi ile farklı ideolojileri benimseyebilirsiniz.\n\n" +
                            "Detaylı diyalog sistemi ile NPC'lerle derinlemesine konuşabilirsiniz. Her seçim karakterinizin gelişimini etkiler. Multiple endings ile farklı sonlar elde edebilirsiniz.\n\n" +
                            "Görsel sanatlar ve atmosfer açısından benzersiz olan oyun, point-and-click adventure elementleri ile RPG mekaniklerini birleştirir. Voice acting ile tüm diyaloglar seslendirilmiştir.\n\n" +
                            "Political commentary ve social criticism içeren hikaye, oyun dünyasında yeni standartlar belirlemiştir. 2019'da Game of the Year ödüllerini kazanmıştır. Mod desteği ile topluluk tarafından oluşturulan içerikler eklenebilir.",
                Developer = zaumStudio,
                Publisher = zaumStudioPublisher,
                ReleaseDate = new DateTime(2021, 3, 30),
                Price = 399.99M,
                CoverImage = "disco-elysium-cover.jpg",
                IsActive = true
            };

            var divinityOriginalSin2 = new Game
            {
                Title = "Divinity: Original Sin 2 - Definitive Edition",
                Description = "Divinity: Original Sin 2 - Definitive Edition, Larian Studios'un taktiksel RPG oyunudur. Rivellon dünyasında geçen oyunda, özgürlük ve seçimlerle dolu bir macera yaşarsınız.\n\n" +
                            "Turn-based combat sistemi ile stratejik savaşlar verirsiniz. Elemental interaction sistemi ile ateş, su, hava ve toprak elementlerini birleştirebilirsiniz. Environmental effects savaşları dinamik hale getirir.\n\n" +
                            "6 farklı origin character bulunur: Red Prince, Sebille, Ifan, Lohse, Beast ve Fane. Her karakterin kendine özgü hikayesi ve yetenekleri vardır. Custom character oluşturma da mümkündür.\n\n" +
                            "Cooperative multiplayer modunda 4 oyuncu birlikte oynayabilir. PvP arena modu ile diğer oyuncularla savaşabilirsiniz. Game Master mode ile kendi hikayelerinizi oluşturabilirsiniz.\n\n" +
                            "Detaylı crafting sistemi ile item'lar oluşturabilirsiniz. Skill tree sistemi ile karakterinizi özelleştirebilirsiniz. Multiple endings ile farklı sonlar elde edebilirsiniz.\n\n" +
                            "Görsel sanatlar ve müzik açısından etkileyici olan oyun, RPG türünün en iyi örneklerinden biridir. Mod desteği ile topluluk tarafından oluşturulan içerikler eklenebilir. Baldur's Gate 3'ün öncüsü olarak kabul edilir.",
                Developer = larianStudios,
                Publisher = larianStudiosPublisher,
                ReleaseDate = new DateTime(2017, 9, 14),
                Price = 499.99M,
                CoverImage = "divinity-os2-cover.jpg",
                IsActive = true
            };

            var baldursGate3 = new Game
            {
                Title = "Baldur's Gate 3",
                Description = "Baldur's Gate 3, Larian Studios'un D&D 5e kurallarına dayalı RPG oyunudur. Faerûn dünyasında geçen epik macerada, Mind Flayer tadpoles'ı ile enfekte olmuş bir karakter olarak yolculuk yaparsınız.\n\n" +
                            "Turn-based combat sistemi ile D&D kurallarına uygun savaşlar verirsiniz. Action economy, advantage/disadvantage ve spell casting sistemi oyunun temel unsurlarıdır. Environmental interaction savaşları dinamik hale getirir.\n\n" +
                            "12 farklı sınıf ve 11 farklı ırk seçeneği bulunur. Multiclassing ile sınıfları birleştirebilirsiniz. Origin characters ile önceden oluşturulmuş karakterler oynayabilir veya custom character oluşturabilirsiniz.\n\n" +
                            "Detaylı diyalog sistemi ile NPC'lerle derinlemesine konuşabilirsiniz. Choice and consequence sistemi ile her seçiminiz hikayeyi etkiler. Romance options ile karakterlerle romantik ilişkiler kurabilirsiniz.\n\n" +
                            "Cooperative multiplayer modunda 4 oyuncu birlikte oynayabilir. Split-screen local co-op desteği bulunur. Modding tools ile topluluk tarafından oluşturulan içerikler eklenebilir.\n\n" +
                            "Görsel sanatlar, müzik ve oyuncu performansları açısından oyun dünyasında yeni standartlar belirleyen Baldur's Gate 3, 2023'te Game of the Year ödülünü kazanmıştır. CRPG türünün modern temsilcisi olarak kabul edilir.",
                Developer = larianStudios,
                Publisher = larianStudiosPublisher,
                ReleaseDate = new DateTime(2023, 8, 3),
                Price = 699.99M,
                CoverImage = "baldurs-gate-3-cover.jpg",
                IsActive = true
            };

            var residentEvil4Remake = new Game
            {
                Title = "Resident Evil 4 Remake",
                Description = "Resident Evil 4 Remake, Capcom'un klasik survival horror oyununun yeniden yapımıdır. Leon S. Kennedy'nin İspanya'da geçen macerasını modern grafikler ve oynanış ile yeniden yaşayabilirsiniz.\n\n" +
                            "Over-the-shoulder kamera açısı ile daha yakın ve immersif deneyim sunar. Modernized combat sistemi ile daha akıcı savaşlar verirsiniz. Parry sistemi ile düşman saldırılarını engelleyebilirsiniz.\n\n" +
                            "Ganados, Las Plagas ile enfekte olmuş köylüler, oyunun ana düşmanlarıdır. Farklı enemy türleri bulunur: Dr. Salvador, El Gigante, Garrador gibi. Boss fight'lar epik ve zorlu geçer.\n\n" +
                            "Merchant sistemi ile silahlarınızı geliştirebilir, yeni ekipman satın alabilirsiniz. Attaché Case ile inventory yönetimi yapabilirsiniz. Crafting sistemi ile healing item'lar oluşturabilirsiniz.\n\n" +
                            "Ada Wong ve Ashley Graham karakterleri ile birlikte yolculuk yaparsınız. Escort mission'lar stratejik düşünme gerektirir. Multiple difficulty levels ile farklı zorluk seviyeleri sunar.\n\n" +
                            "Görsel sanatlar ve ses tasarımı açısından mükemmel olan remake, orijinal oyunun atmosferini korurken modern standartlara uygun hale getirilmiştir. Separate Ways DLC'si ile Ada Wong'un hikayesi de oynanabilir.",
                Developer = capcom,
                Publisher = capcomPublisher,
                ReleaseDate = new DateTime(2023, 3, 24),
                Price = 899.99M,
                CoverImage = "re4-remake-cover.jpg",
                IsActive = true
            };

            var hogwartsLegacy = new Game
            {
                Title = "Hogwarts Legacy",
                Description = "Hogwarts Legacy, Avalanche Software'in Harry Potter evreninde geçen açık dünya RPG oyunudur. 1800'lerde geçen oyunda, Hogwarts School of Witchcraft and Wizardry'de öğrenci olarak maceraya atılırsınız.\n\n" +
                            "Hogwarts Castle'ın detaylı haritasında özgürce dolaşabilir, sınıflara katılabilir ve büyü öğrenebilirsiniz. Hogsmeade, Forbidden Forest ve çevre bölgeler keşfedilmeyi bekler.\n\n" +
                            "Çeşitli büyü türleri öğrenebilirsiniz: Combat spells, utility spells ve dark arts. Spell combination sistemi ile güçlü kombinasyonlar oluşturabilirsiniz. Potion brewing ve herbology ile crafting yapabilirsiniz.\n\n" +
                            "Flying mechanics ile broomstick kullanabilir, Hippogriff ile uçabilirsiniz. Beast taming sistemi ile magical creatures'ları evcilleştirebilirsiniz. Room of Requirement ile kendi alanınızı özelleştirebilirsiniz.\n\n" +
                            "Choice and consequence sistemi ile karakterinizin ahlaki seçimleri hikayeyi etkiler. House selection ile Gryffindor, Slytherin, Hufflepuff veya Ravenclaw'a katılabilirsiniz.\n\n" +
                            "Görsel sanatlar ve atmosfer açısından Harry Potter evrenini başarıyla yansıtan oyun, fan'lar için nostaljik bir deneyim sunar. Open world design ve RPG elementleri ile modern oyun standartlarını karşılar.",
                Developer = avalancheSoftware,
                Publisher = avalancheSoftwarePublisher,
                ReleaseDate = new DateTime(2023, 2, 10),
                Price = 999.99M,
                CoverImage = "hogwarts-legacy-cover.jpg",
                IsActive = true
            };

            var liesOfP = new Game
            {
                Title = "Lies of P",
                Description = "Lies of P, Neowiz'in Pinokyo temalı souls-like oyunudur. Karanlık ve zorlu macera.",
                Developer = neowiz,
                Publisher = neowizPublisher,
                ReleaseDate = new DateTime(2023, 9, 19),
                Price = 699.99M,
                CoverImage = "lies-of-p-cover.jpg",
                IsActive = true
            };

            var remnant2 = new Game
            {
                Title = "Remnant 2",
                Description = "Remnant 2, Gunfire Games'in souls-like shooter oyunudur. Çoklu evrenlerde geçen oyunda, Root invasion'ına karşı savaşır ve dünyaları keşfedersiniz.\n\n" +
                            "Procedural generation ile her oynanış farklı deneyim sunar. Farklı biomes bulunur: Yaesha (jungle), N'Erud (desert), Losomn (gothic) ve Root Earth. Her biome kendine özgü düşmanları ve boss'ları barındırır.\n\n" +
                            "Gunplay ve melee combat birleştirilmiştir. Çeşitli silah türleri mevcuttur: Long Guns, Hand Guns ve Melee Weapons. Mod system ile silahlarınıza özel yetenekler ekleyebilirsiniz.\n\n" +
                            "Archetype system ile karakterinizi özelleştirebilirsiniz: Gunslinger, Hunter, Challenger, Handler gibi. Dual archetype ile iki sınıfı birleştirebilirsiniz. Skill tree ile yeteneklerinizi geliştirebilirsiniz.\n\n" +
                            "Cooperative multiplayer modunda 3 oyuncu birlikte oynayabilir. Scaling system ile oyuncu sayısına göre zorluk ayarlanır. Adventure mode ile belirli bölgeleri tekrar oynayabilirsiniz.\n\n" +
                            "Görsel sanatlar ve atmosfer açısından etkileyici olan oyun, souls-like ve shooter türlerini başarıyla birleştirir. Post-apocalyptic setting ve cosmic horror elementleri ile benzersiz bir deneyim sunar.",
                Developer = gunfireGames,
                Publisher = gunfireGamesPublisher,
                ReleaseDate = new DateTime(2023, 7, 25),
                Price = 599.99M,
                CoverImage = "remnant-2-cover.jpg",
                IsActive = true
            };

            var armoredCore6 = new Game
            {
                Title = "ARMORED CORE VI FIRES OF RUBICON",
                Description = "ARMORED CORE VI FIRES OF RUBICON, FromSoftware'in mecha aksiyon oyunudur. Rubicon 3 gezegeninde geçen oyunda, mercenary pilot olarak dev robotlarla epik savaşlar verirsiniz.\n\n" +
                            "Mecha customization sistemi oyunun merkezi unsurlarından biridir. Head, core, arms ve legs parçalarını özelleştirebilirsiniz. Weight, energy ve EN load sistemleri ile denge kurmanız gerekir.\n\n" +
                            "Combat sisteminde mobility ve positioning kritik öneme sahiptir. Boost, dodge ve quick turn mekanikleri ile hareket edebilirsiniz. Assault boost ile havada savaşabilirsiniz.\n\n" +
                            "Çeşitli silah türleri mevcuttur: Kinetic, Energy, Explosive ve Melee weapons. Shoulder weapons ile ek silahlar kullanabilirsiniz. Ammunition ve energy management stratejik öneme sahiptir.\n\n" +
                            "Mission-based structure ile farklı görevler üstlenebilirsiniz. Arena mode ile diğer AC'lerle savaşabilirsiniz. Multiple endings ile farklı sonlar elde edebilirsiniz.\n\n" +
                            "Görsel sanatlar ve müzik açısından etkileyici olan oyun, mecha türünün modern temsilcisi olarak kabul edilir. FromSoftware'in geleneksel zorlu oynanışı ile mecha action'ı başarıyla birleştirir.",
                Developer = fromSoftware,
                Publisher = bandaiNamco,
                ReleaseDate = new DateTime(2023, 8, 25),
                Price = 799.99M,
                CoverImage = "armored-core-6-cover.jpg",
                IsActive = true
            };

            var streetFighter6 = new Game
            {
                Title = "Street Fighter 6",
                Description = "Street Fighter 6, Capcom'un klasik dövüş oyun serisinin en yeni halkasıdır. Modern grafikler ve yenilikçi mekaniklerle dövüş deneyimini bir üst seviyeye taşır.\n\n" +
                            "World Tour modu ile kendi karakterinizi oluşturup açık dünyada dövüşçülerle tanışabilir, yeni yetenekler öğrenebilirsiniz. Battle Hub ile online turnuvalara katılabilirsiniz.\n\n" +
                            "Klasik karakterler (Ryu, Chun-Li, Ken) ve yeni dövüşçülerle zengin bir kadro sunar. Her karakterin kendine özgü hareket seti ve süper komboları vardır.\n\n" +
                            "Drive System ile saldırı ve savunma arasında stratejik denge kurabilirsiniz. Modern ve klasik kontrol şemaları ile her seviyeden oyuncuya hitap eder.\n\n" +
                            "Gelişmiş eğitim modları ile yeni başlayanlar için öğretici içerikler sunar. Grafikler ve animasyonlar serinin en yüksek kalitesindedir.\n\n" +
                            "E-spor desteği ve topluluk etkinlikleriyle Street Fighter 6, dövüş oyunu tutkunları için uzun ömürlü bir deneyim vaat eder.",
                Developer = capcom,
                Publisher = capcomPublisher,
                ReleaseDate = new DateTime(2023, 6, 2),
                Price = 699.99M,
                CoverImage = "street-fighter-6-cover.jpg",
                IsActive = true
            };

            var callOfDutyWarzone = new Game
            {
                Title = "Call of Duty: Warzone",
                Description = "Call of Duty: Warzone, Infinity Ward ve Activision'ın ücretsiz battle royale FPS oyunudur. Modern Warfare evreninde geçen devasa haritalarda 150 oyuncu mücadele eder.\n\n" +
                            "Verdansk, Caldera ve Al Mazrah gibi haritalarda farklı stratejiler geliştirebilirsiniz. Gulag sistemi ile elenen oyuncular ikinci bir şans için düello yapar.\n\n" +
                            "Çeşitli silahlar, killstreak'ler, araçlar ve ekipmanlar ile taktiksel derinlik sunar. Loadout sistemi ile kendi silah setlerinizi oluşturabilirsiniz.\n\n" +
                            "Squad, duo ve solo modları ile farklı oynanış deneyimleri sunar. Cross-play desteği ile farklı platformlardan oyuncular birlikte oynayabilir.\n\n" +
                            "Sürekli güncellemeler ile yeni içerikler, sezonlar ve etkinlikler eklenir. Battle Pass sistemi ile kozmetik ödüller kazanabilirsiniz.\n\n" +
                            "E-spor turnuvaları ve topluluk etkinlikleriyle Warzone, battle royale türünün en popüler oyunlarından biri olmuştur.",
                Developer = infinityWard,
                Publisher = activisionPublisher,
                ReleaseDate = new DateTime(2020, 3, 10),
                Price = 0,
                CoverImage = "cod-warzone-cover.jpg",
                IsActive = true
            };

            var callOfDutyMW3 = new Game
            {
                Title = "Call of Duty: Modern Warfare III",
                Description = "Call of Duty: Modern Warfare III, Sledgehammer Games ve Activision'ın efsanevi FPS serisinin en yeni oyunudur. Yoğun aksiyon ve taktiksel savaşlar sunar.\n\n" +
                            "Kampanya modunda sinematik anlatım ve sürükleyici görevler bulunur. Task Force 141 ekibiyle dünyayı tehdit eden yeni bir düşmana karşı mücadele edersiniz.\n\n" +
                            "Çok oyunculu modda klasik haritalar ve yeni mekanikler bir arada sunulur. Silah özelleştirme, killstreak'ler ve yeni hareket sistemleri ile rekabetçi oynanış geliştirilmiştir.\n\n" +
                            "Zombi modu ile kooperatif oynanış ve hayatta kalma deneyimi yaşanır. Farklı haritalar ve görevlerle zengin içerik sunar.\n\n" +
                            "Cross-play ve cross-progression desteği ile farklı platformlarda ilerlemenizi sürdürebilirsiniz. Battle Pass ile sezonluk ödüller kazanabilirsiniz.\n\n" +
                            "Görsel ve ses tasarımıyla Modern Warfare III, serinin en gerçekçi ve etkileyici oyunlarından biri olarak öne çıkar. E-spor desteği ile rekabetçi sahnede de yer alır.",
                Developer = sledgehammerGames,
                Publisher = activisionPublisher,
                ReleaseDate = new DateTime(2023, 11, 10),
                Price = 999.99M,
                CoverImage = "cod-mw3-cover.jpg",
                IsActive = true
            };

            context.Games.AddRange(csgo, pubg, fifa24, dota2, gta5, witcher3, eldenRing, valorant, minecraft, starfield, rdr2, acValhalla, forza5, cyberpunk, godOfWar, teamFortress2, rust, arkSurvival, fallout4, skyrim, portal2, left4Dead2, terraria, stardewValley, amongUs, phasmophobia, deadByDaylight, rainbowSixSiege, warframe, pathOfExile, warThunder, destiny2, apexLegends, rocketLeague, fallGuys, hades, discoElysium, divinityOriginalSin2, baldursGate3, residentEvil4Remake, hogwartsLegacy, liesOfP, remnant2, armoredCore6, streetFighter6, callOfDutyWarzone, callOfDutyMW3);
            
            // Tüm oyunlar için sistem gereksinimleri
            var systemRequirements = new List<SystemRequirement>
            {
                // CS2 - Minimum
                new SystemRequirement { Game = csgo, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-4460 or better", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 960 or better", Storage = "85 GB" },
                // CS2 - Önerilen
                new SystemRequirement { Game = csgo, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-8700K or better", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce RTX 3060 or better", Storage = "85 GB" },
                
                // PUBG - Minimum
                new SystemRequirement { Game = pubg, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-4430 or better", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 960 2GB or better", Storage = "40 GB" },
                // PUBG - Önerilen
                new SystemRequirement { Game = pubg, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-6600K / AMD Ryzen 5 1600", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 3GB / AMD Radeon RX 580 4GB", Storage = "40 GB" },
                
                // FIFA 24 - Minimum
                new SystemRequirement { Game = fifa24, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-6600K / AMD Ryzen 5 1600", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 1050 Ti / AMD Radeon RX 570", Storage = "100 GB" },
                // FIFA 24 - Önerilen
                new SystemRequirement { Game = fifa24, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-8700 / AMD Ryzen 7 2700X", Memory = "12 GB RAM", Graphics = "NVIDIA GeForce RTX 2060 / AMD RX 5600 XT", Storage = "100 GB" },
                
                // Dota 2 - Minimum
                new SystemRequirement { Game = dota2, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core 2 Duo E7400 / AMD Athlon 64 X2 5600+", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce 8600 GT / AMD Radeon HD 2600 Pro", Storage = "15 GB" },
                // Dota 2 - Önerilen
                new SystemRequirement { Game = dota2, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-8400 / AMD Ryzen 5 2600", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 / AMD RX 580", Storage = "15 GB" },
                
                // GTA V - Minimum
                new SystemRequirement { Game = gta5, Type = "Minimum", OS = "Windows 8.1 64-bit", Processor = "Intel Core 2 Quad CPU Q6600 / AMD Phenom 9850", Memory = "4 GB RAM", Graphics = "NVIDIA 9800 GT 1GB / AMD HD 4870 1GB", Storage = "72 GB" },
                // GTA V - Önerilen
                new SystemRequirement { Game = gta5, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5 3470 / AMD X8 FX-8350", Memory = "8 GB RAM", Graphics = "NVIDIA GTX 660 2GB / AMD HD 7870 2GB", Storage = "72 GB" },
                
                // Witcher 3 - Minimum
                new SystemRequirement { Game = witcher3, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel CPU Core i5-2500K 3.3GHz / AMD CPU Phenom II X4 940", Memory = "6 GB RAM", Graphics = "Nvidia GPU GeForce GTX 660 / AMD GPU Radeon HD 7870", Storage = "50 GB" },
                // Witcher 3 - Önerilen
                new SystemRequirement { Game = witcher3, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel CPU Core i7-3770 3.4 GHz / AMD CPU AMD FX-8350 4 GHz", Memory = "8 GB RAM", Graphics = "Nvidia GPU GeForce GTX 1060 / AMD GPU Radeon RX 580", Storage = "50 GB" },
                
                // Elden Ring - Minimum
                new SystemRequirement { Game = eldenRing, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-8400 / AMD Ryzen 3 3300X", Memory = "12 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 3GB / AMD Radeon RX 580 4GB", Storage = "60 GB" },
                // Elden Ring - Önerilen
                new SystemRequirement { Game = eldenRing, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-8700K / AMD Ryzen 5 3600X", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce GTX 1070 8GB / AMD Radeon RX VEGA 56 8GB", Storage = "60 GB" },
                
                // Valorant - Minimum
                new SystemRequirement { Game = valorant, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core 2 Duo E8400 / AMD Athlon 200GE", Memory = "4 GB RAM", Graphics = "Intel HD 4000 / AMD Radeon R5 200", Storage = "8 GB" },
                // Valorant - Önerilen
                new SystemRequirement { Game = valorant, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i3-4150 / AMD Ryzen 3 1200", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GT 730 / AMD Radeon R7 240", Storage = "8 GB" },
                
                // Minecraft - Minimum
                new SystemRequirement { Game = minecraft, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core i3-3210 / AMD A8-7600 APU", Memory = "2 GB RAM", Graphics = "Intel HD Graphics 4000 / AMD Radeon R5", Storage = "1 GB" },
                // Minecraft - Önerilen
                new SystemRequirement { Game = minecraft, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-4690 / AMD A10-7800", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce 700 Series / AMD Radeon Rx 200 Series", Storage = "4 GB" },
                
                // Starfield - Minimum
                new SystemRequirement { Game = starfield, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "AMD Ryzen 5 2600X / Intel Core i7-6800K", Memory = "16 GB RAM", Graphics = "AMD Radeon RX 5700 / NVIDIA GeForce GTX 1070 Ti", Storage = "125 GB" },
                // Starfield - Önerilen
                new SystemRequirement { Game = starfield, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "AMD Ryzen 5 3600X / Intel Core i5-10600K", Memory = "16 GB RAM", Graphics = "AMD Radeon RX 6800 XT / NVIDIA GeForce RTX 2080", Storage = "125 GB" },
                
                // RDR2 - Minimum
                new SystemRequirement { Game = rdr2, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-2500K / AMD FX-6300", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 770 2GB / AMD Radeon R9 280 3GB", Storage = "150 GB" },
                // RDR2 - Önerilen
                new SystemRequirement { Game = rdr2, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-4770K / AMD Ryzen 5 1500X", Memory = "12 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 6GB / AMD Radeon RX 480 4GB", Storage = "150 GB" },
                
                // AC Valhalla - Minimum
                new SystemRequirement { Game = acValhalla, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "AMD Ryzen 3 1200 / Intel Core i5-4460", Memory = "8 GB RAM", Graphics = "AMD Radeon R9 380 / NVIDIA GeForce GTX 960", Storage = "50 GB" },
                // AC Valhalla - Önerilen
                new SystemRequirement { Game = acValhalla, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "AMD Ryzen 5 1600 / Intel Core i7-4790", Memory = "8 GB RAM", Graphics = "AMD Radeon RX 570 / NVIDIA GeForce GTX 1060", Storage = "50 GB" },
                
                // Forza 5 - Minimum
                new SystemRequirement { Game = forza5, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-4460 / AMD Ryzen 3 1200", Memory = "8 GB RAM", Graphics = "NVIDIA GTX 970 / AMD RX 470", Storage = "110 GB" },
                // Forza 5 - Önerilen
                new SystemRequirement { Game = forza5, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-8400 / AMD Ryzen 5 2600", Memory = "16 GB RAM", Graphics = "NVIDIA RTX 2060 / AMD RX 5600 XT", Storage = "110 GB" },
                
                // Cyberpunk - Minimum
                new SystemRequirement { Game = cyberpunk, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-3570K / AMD FX-8310", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 970 / AMD Radeon RX 470", Storage = "70 GB" },
                // Cyberpunk - Önerilen
                new SystemRequirement { Game = cyberpunk, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-4790 / AMD Ryzen 3 3200G", Memory = "12 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 / AMD Radeon RX 590", Storage = "70 GB" },
                
                // God of War - Minimum
                new SystemRequirement { Game = godOfWar, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel i5-6600k / AMD Ryzen 5 2400 G", Memory = "8 GB RAM", Graphics = "NVIDIA GTX 1060 6GB / AMD RX 570 4GB", Storage = "70 GB" },
                // God of War - Önerilen
                new SystemRequirement { Game = godOfWar, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel i7-8700 / AMD Ryzen 7 2700X", Memory = "16 GB RAM", Graphics = "NVIDIA RTX 2070 Super / AMD RX 5700 XT", Storage = "70 GB" },
                
                // Team Fortress 2 - Minimum
                new SystemRequirement { Game = teamFortress2, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "1.7 GHz Processor", Memory = "512 MB RAM", Graphics = "DirectX 8.1 level Graphics Card", Storage = "15 GB" },
                // Team Fortress 2 - Önerilen
                new SystemRequirement { Game = teamFortress2, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i3 / AMD equivalent", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 750 / AMD equivalent", Storage = "15 GB" },
                
                // Rust - Minimum
                new SystemRequirement { Game = rust, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i7-3770 / AMD FX-9590", Memory = "10 GB RAM", Graphics = "NVIDIA GTX 670 2GB / AMD R9 280", Storage = "20 GB" },
                // Rust - Önerilen
                new SystemRequirement { Game = rust, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-4790K / AMD Ryzen 5 1600", Memory = "16 GB RAM", Graphics = "NVIDIA GTX 980 / AMD R9 Fury", Storage = "20 GB" },
                
                // ARK Survival - Minimum
                new SystemRequirement { Game = arkSurvival, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core i5-2400 / AMD FX-8320", Memory = "8 GB RAM", Graphics = "NVIDIA GTX 670 2GB / AMD Radeon HD 7870 2GB", Storage = "60 GB" },
                // ARK Survival - Önerilen
                new SystemRequirement { Game = arkSurvival, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-4770 / AMD FX-9590", Memory = "16 GB RAM", Graphics = "NVIDIA GTX 970 / AMD R9 290", Storage = "60 GB" },
                
                // Fallout 4 - Minimum
                new SystemRequirement { Game = fallout4, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core i5-2300 / AMD Phenom II X4 945", Memory = "8 GB RAM", Graphics = "NVIDIA GTX 550 Ti 2GB / AMD Radeon HD 7870 2GB", Storage = "30 GB" },
                // Fallout 4 - Önerilen
                new SystemRequirement { Game = fallout4, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7 4790 / AMD FX-9590", Memory = "8 GB RAM", Graphics = "NVIDIA GTX 780 3GB / AMD Radeon R9 290X 4GB", Storage = "30 GB" },
                
                // Skyrim - Minimum
                new SystemRequirement { Game = skyrim, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core 2 Duo E4400 / AMD Athlon 64 X2 4000+", Memory = "2 GB RAM", Graphics = "NVIDIA GeForce 7600 GT / AMD Radeon X1300", Storage = "6 GB" },
                // Skyrim - Önerilen
                new SystemRequirement { Game = skyrim, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-750 / AMD Phenom II X4 945", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 260 / AMD Radeon HD 4890", Storage = "6 GB" },
                
                // Portal 2 - Minimum
                new SystemRequirement { Game = portal2, Type = "Minimum", OS = "Windows 7 32-bit", Processor = "3.0 GHz P4, Dual Core 2.0 GHz", Memory = "1 GB RAM", Graphics = "DirectX 9 compatible video card", Storage = "8 GB" },
                // Portal 2 - Önerilen
                new SystemRequirement { Game = portal2, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i3 / AMD equivalent", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 750 / AMD equivalent", Storage = "8 GB" },
                
                // Left 4 Dead 2 - Minimum
                new SystemRequirement { Game = left4Dead2, Type = "Minimum", OS = "Windows 7 32-bit", Processor = "Intel Pentium 4 3.0GHz", Memory = "2 GB RAM", Graphics = "DirectX 9 compatible video card", Storage = "13 GB" },
                // Left 4 Dead 2 - Önerilen
                new SystemRequirement { Game = left4Dead2, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i3 / AMD equivalent", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 750 / AMD equivalent", Storage = "13 GB" },
                
                // Terraria - Minimum
                new SystemRequirement { Game = terraria, Type = "Minimum", OS = "Windows XP", Processor = "1.6 GHz", Memory = "512 MB RAM", Graphics = "128 MB video memory", Storage = "200 MB" },
                // Terraria - Önerilen
                new SystemRequirement { Game = terraria, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i3 / AMD equivalent", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 750 / AMD equivalent", Storage = "200 MB" },
                
                // Stardew Valley - Minimum
                new SystemRequirement { Game = stardewValley, Type = "Minimum", OS = "Windows Vista", Processor = "2 Ghz", Memory = "2 GB RAM", Graphics = "256 MB video memory", Storage = "500 MB" },
                // Stardew Valley - Önerilen
                new SystemRequirement { Game = stardewValley, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i3 / AMD equivalent", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 750 / AMD equivalent", Storage = "500 MB" },
                
                // Among Us - Minimum
                new SystemRequirement { Game = amongUs, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core 2 Duo E4400", Memory = "1 GB RAM", Graphics = "Intel HD Graphics 4000", Storage = "250 MB" },
                // Among Us - Önerilen
                new SystemRequirement { Game = amongUs, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i3 / AMD equivalent", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 750 / AMD equivalent", Storage = "250 MB" },
                
                // Phasmophobia - Minimum
                new SystemRequirement { Game = phasmophobia, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-4590 / AMD FX 8350", Memory = "8 GB RAM", Graphics = "NVIDIA GTX 970 / AMD Radeon R9 280", Storage = "18 GB" },
                // Phasmophobia - Önerilen
                new SystemRequirement { Game = phasmophobia, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-8700K / AMD Ryzen 5 2600", Memory = "16 GB RAM", Graphics = "NVIDIA GTX 1070 / AMD RX 580", Storage = "18 GB" },
                
                // Dead by Daylight - Minimum
                new SystemRequirement { Game = deadByDaylight, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i3-4170 / AMD FX-8120", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 460 / AMD Radeon HD 6850", Storage = "50 GB" },
                // Dead by Daylight - Önerilen
                new SystemRequirement { Game = deadByDaylight, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-8400 / AMD Ryzen 5 2600", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 / AMD Radeon RX 580", Storage = "50 GB" },
                
                // Rainbow Six Siege - Minimum
                new SystemRequirement { Game = rainbowSixSiege, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i3 560 / AMD Phenom II X4 945", Memory = "6 GB RAM", Graphics = "NVIDIA GeForce GTX 460 / AMD Radeon HD 5870", Storage = "61 GB" },
                // Rainbow Six Siege - Önerilen
                new SystemRequirement { Game = rainbowSixSiege, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-2500K / AMD FX-8350", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 670 / AMD Radeon HD 7970", Storage = "61 GB" },
                
                // Warframe - Minimum
                new SystemRequirement { Game = warframe, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core 2 Duo E6400 / AMD Athlon 64 X2 4000+", Memory = "2 GB RAM", Graphics = "NVIDIA GeForce 8600 GT / AMD Radeon HD 3600", Storage = "50 GB" },
                // Warframe - Önerilen
                new SystemRequirement { Game = warframe, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-750 / AMD Phenom II X4 945", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 260 / AMD Radeon HD 4890", Storage = "50 GB" },
                
                // Path of Exile - Minimum
                new SystemRequirement { Game = pathOfExile, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core i3-4130 / AMD FX-4100", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 650 Ti / AMD Radeon HD 7790", Storage = "20 GB" },
                // Path of Exile - Önerilen
                new SystemRequirement { Game = pathOfExile, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-4460 / AMD FX-6300", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 750 Ti / AMD Radeon R7 260X", Storage = "20 GB" },
                
                // War Thunder - Minimum
                new SystemRequirement { Game = warThunder, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core i3 / AMD equivalent", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 650 / AMD equivalent", Storage = "17 GB" },
                // War Thunder - Önerilen
                new SystemRequirement { Game = warThunder, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5 / AMD equivalent", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 / AMD equivalent", Storage = "17 GB" },
                
                // Destiny 2 - Minimum
                new SystemRequirement { Game = destiny2, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i3-3250 / AMD FX-4350", Memory = "6 GB RAM", Graphics = "NVIDIA GeForce GTX 660 / AMD Radeon HD 7850", Storage = "105 GB" },
                // Destiny 2 - Önerilen
                new SystemRequirement { Game = destiny2, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-7400 / AMD Ryzen 5 1600", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 / AMD Radeon RX 580", Storage = "105 GB" },
                
                // Apex Legends - Minimum
                new SystemRequirement { Game = apexLegends, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core i3-6300 / AMD FX-4350", Memory = "6 GB RAM", Graphics = "NVIDIA GeForce GT 640 / AMD Radeon HD 7730", Storage = "56 GB" },
                // Apex Legends - Önerilen
                new SystemRequirement { Game = apexLegends, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-3570K / AMD FX-8310", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 970 / AMD Radeon R9 290", Storage = "56 GB" },
                
                // Rocket League - Minimum
                new SystemRequirement { Game = rocketLeague, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "2.5 GHz Dual-core", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce 760 / AMD Radeon R7 270X", Storage = "20 GB" },
                // Rocket League - Önerilen
                new SystemRequirement { Game = rocketLeague, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "3.0+ GHz Quad-core", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 / AMD Radeon RX 580", Storage = "20 GB" },
                
                // Fall Guys - Minimum
                new SystemRequirement { Game = fallGuys, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-2300 / AMD FX-6300", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 660 / AMD Radeon HD 7790", Storage = "2 GB" },
                // Fall Guys - Önerilen
                new SystemRequirement { Game = fallGuys, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-8400 / AMD Ryzen 5 2600", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 / AMD Radeon RX 580", Storage = "2 GB" },
                
                // Hades - Minimum
                new SystemRequirement { Game = hades, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Dual Core 2.4 GHz", Memory = "4 GB RAM", Graphics = "1GB VRAM / DirectX 10+ support", Storage = "15 GB" },
                // Hades - Önerilen
                new SystemRequirement { Game = hades, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Dual Core 3.0 GHz", Memory = "8 GB RAM", Graphics = "2GB VRAM / DirectX 10+ support", Storage = "15 GB" },
                
                // Disco Elysium - Minimum
                new SystemRequirement { Game = discoElysium, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core 2 Duo E8400 / AMD Phenom II X2 550", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 460 / AMD Radeon HD 5770", Storage = "20 GB" },
                // Disco Elysium - Önerilen
                new SystemRequirement { Game = discoElysium, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-750 / AMD Phenom II X4 945", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 660 / AMD Radeon HD 7850", Storage = "20 GB" },
                
                // Divinity Original Sin 2 - Minimum
                new SystemRequirement { Game = divinityOriginalSin2, Type = "Minimum", OS = "Windows 7 64-bit", Processor = "Intel Core i5 or equivalent", Memory = "4 GB RAM", Graphics = "NVIDIA GeForce GTX 550 / AMD Radeon HD 6XXX", Storage = "60 GB" },
                // Divinity Original Sin 2 - Önerilen
                new SystemRequirement { Game = divinityOriginalSin2, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7 or equivalent", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 760 / AMD Radeon R9 270", Storage = "60 GB" },
                
                // Baldur's Gate 3 - Minimum
                new SystemRequirement { Game = baldursGate3, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-4690 / AMD FX 8350", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 970 / AMD Radeon RX 480", Storage = "150 GB" },
                // Baldur's Gate 3 - Önerilen
                new SystemRequirement { Game = baldursGate3, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-8700K / AMD Ryzen 5 3600", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce RTX 2060 Super / AMD RX 5700 XT", Storage = "150 GB" },
                
                // Resident Evil 4 Remake - Minimum
                new SystemRequirement { Game = residentEvil4Remake, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "AMD Ryzen 3 1200 / Intel Core i5-7500", Memory = "8 GB RAM", Graphics = "AMD Radeon RX 560 / NVIDIA GeForce GTX 1050 Ti", Storage = "60 GB" },
                // Resident Evil 4 Remake - Önerilen
                new SystemRequirement { Game = residentEvil4Remake, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "AMD Ryzen 5 3600 / Intel Core i7-8700", Memory = "16 GB RAM", Graphics = "AMD Radeon RX 6700 XT / NVIDIA GeForce RTX 2060", Storage = "60 GB" },
                
                // Hogwarts Legacy - Minimum
                new SystemRequirement { Game = hogwartsLegacy, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-6600 / AMD Ryzen 5 1400", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce GTX 960 / AMD Radeon RX 470", Storage = "85 GB" },
                // Hogwarts Legacy - Önerilen
                new SystemRequirement { Game = hogwartsLegacy, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-8700 / AMD Ryzen 5 3600", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce RTX 2080 Ti / AMD Radeon RX 6800 XT", Storage = "85 GB" },
                
                // Lies of P - Minimum
                new SystemRequirement { Game = liesOfP, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "AMD Ryzen 3 1200 / Intel Core i3-6300", Memory = "8 GB RAM", Graphics = "AMD Radeon RX 560 / NVIDIA GeForce GTX 750 Ti", Storage = "50 GB" },
                // Lies of P - Önerilen
                new SystemRequirement { Game = liesOfP, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "AMD Ryzen 5 3600 / Intel Core i7-8700", Memory = "16 GB RAM", Graphics = "AMD Radeon RX 6700 XT / NVIDIA GeForce RTX 2070", Storage = "50 GB" },
                
                // Remnant 2 - Minimum
                new SystemRequirement { Game = remnant2, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-7600 / AMD Ryzen 5 2600", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce GTX 1650 / AMD Radeon RX 590", Storage = "80 GB" },
                // Remnant 2 - Önerilen
                new SystemRequirement { Game = remnant2, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-10600K / AMD Ryzen 5 3600X", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce RTX 2060 / AMD Radeon RX 5600 XT", Storage = "80 GB" },
                
                // Armored Core 6 - Minimum
                new SystemRequirement { Game = armoredCore6, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i7-4790K / AMD Ryzen 5 1500X", Memory = "12 GB RAM", Graphics = "NVIDIA GeForce GTX 1650 / AMD Radeon RX 480", Storage = "60 GB" },
                // Armored Core 6 - Önerilen
                new SystemRequirement { Game = armoredCore6, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-8700K / AMD Ryzen 5 3600X", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce RTX 2070 Super / AMD Radeon RX 6700 XT", Storage = "60 GB" },
                
                // Street Fighter 6 - Minimum
                new SystemRequirement { Game = streetFighter6, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-7500 / AMD Ryzen 3 1200", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 1060 / AMD Radeon RX 580", Storage = "60 GB" },
                // Street Fighter 6 - Önerilen
                new SystemRequirement { Game = streetFighter6, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-8700 / AMD Ryzen 5 3600", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce RTX 2070 / AMD Radeon RX 5700 XT", Storage = "60 GB" },
                
                // Call of Duty Warzone - Minimum
                new SystemRequirement { Game = callOfDutyWarzone, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i3-4340 / AMD FX-6300", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 670 / AMD Radeon HD 7950", Storage = "175 GB" },
                // Call of Duty Warzone - Önerilen
                new SystemRequirement { Game = callOfDutyWarzone, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i5-2500K / AMD Ryzen R5 1600X", Memory = "12 GB RAM", Graphics = "NVIDIA GeForce GTX 970 / AMD Radeon RX 580", Storage = "175 GB" },
                
                // Call of Duty MW3 - Minimum
                new SystemRequirement { Game = callOfDutyMW3, Type = "Minimum", OS = "Windows 10 64-bit", Processor = "Intel Core i5-6600 / AMD Ryzen 5 1400", Memory = "8 GB RAM", Graphics = "NVIDIA GeForce GTX 960 / AMD Radeon RX 470", Storage = "125 GB" },
                // Call of Duty MW3 - Önerilen
                new SystemRequirement { Game = callOfDutyMW3, Type = "Önerilen", OS = "Windows 10 64-bit", Processor = "Intel Core i7-8700K / AMD Ryzen 5 3600X", Memory = "16 GB RAM", Graphics = "NVIDIA GeForce RTX 3080 / AMD Radeon RX 6800 XT", Storage = "125 GB" }
            };
            
            context.SystemRequirements.AddRange(systemRequirements);
            await context.SaveChangesAsync();

            // Oyun kategorilerini ekle
            var gameCategories = new List<GameCategory>
            {
                new GameCategory { Game = csgo, Category = fps },
                new GameCategory { Game = csgo, Category = action },
                new GameCategory { Game = pubg, Category = fps },
                new GameCategory { Game = pubg, Category = action },
                new GameCategory { Game = fifa24, Category = sports },
                new GameCategory { Game = dota2, Category = moba },
                new GameCategory { Game = dota2, Category = action },
                new GameCategory { Game = gta5, Category = action },
                new GameCategory { Game = gta5, Category = openWorld },
                // Yeni oyunlar için kategoriler
                new GameCategory { Game = witcher3, Category = rpg },
                new GameCategory { Game = witcher3, Category = action },
                new GameCategory { Game = witcher3, Category = openWorld },
                new GameCategory { Game = eldenRing, Category = rpg },
                new GameCategory { Game = eldenRing, Category = action },
                new GameCategory { Game = eldenRing, Category = openWorld },
                new GameCategory { Game = valorant, Category = fps },
                new GameCategory { Game = valorant, Category = action },
                new GameCategory { Game = minecraft, Category = survival },
                new GameCategory { Game = minecraft, Category = openWorld },
                new GameCategory { Game = starfield, Category = rpg },
                new GameCategory { Game = starfield, Category = action },
                new GameCategory { Game = starfield, Category = openWorld },
                new GameCategory { Game = rdr2, Category = action },
                new GameCategory { Game = rdr2, Category = openWorld },
                new GameCategory { Game = acValhalla, Category = action },
                new GameCategory { Game = acValhalla, Category = rpg },
                new GameCategory { Game = acValhalla, Category = openWorld },
                new GameCategory { Game = forza5, Category = racing },
                new GameCategory { Game = forza5, Category = openWorld },
                new GameCategory { Game = cyberpunk, Category = rpg },
                new GameCategory { Game = cyberpunk, Category = action },
                new GameCategory { Game = cyberpunk, Category = openWorld },
                new GameCategory { Game = godOfWar, Category = action },
                new GameCategory { Game = godOfWar, Category = rpg },
                new GameCategory { Game = teamFortress2, Category = fps },
                new GameCategory { Game = teamFortress2, Category = action },
                new GameCategory { Game = rust, Category = survival },
                new GameCategory { Game = rust, Category = action },
                new GameCategory { Game = arkSurvival, Category = survival },
                new GameCategory { Game = arkSurvival, Category = openWorld },
                new GameCategory { Game = fallout4, Category = survival },
                new GameCategory { Game = fallout4, Category = openWorld },
                new GameCategory { Game = skyrim, Category = survival },
                new GameCategory { Game = skyrim, Category = openWorld },
                new GameCategory { Game = portal2, Category = action },
                new GameCategory { Game = left4Dead2, Category = action },
                new GameCategory { Game = terraria, Category = survival },
                new GameCategory { Game = stardewValley, Category = survival },
                new GameCategory { Game = amongUs, Category = moba },
                new GameCategory { Game = amongUs, Category = action },
                new GameCategory { Game = phasmophobia, Category = horror },
                new GameCategory { Game = phasmophobia, Category = action },
                new GameCategory { Game = deadByDaylight, Category = horror },
                new GameCategory { Game = deadByDaylight, Category = action },
                new GameCategory { Game = rainbowSixSiege, Category = fps },
                new GameCategory { Game = rainbowSixSiege, Category = action },
                new GameCategory { Game = warframe, Category = moba },
                new GameCategory { Game = warframe, Category = action },
                new GameCategory { Game = pathOfExile, Category = moba },
                new GameCategory { Game = pathOfExile, Category = action },
                new GameCategory { Game = warThunder, Category = action },
                new GameCategory { Game = warThunder, Category = openWorld },
                new GameCategory { Game = destiny2, Category = moba },
                new GameCategory { Game = destiny2, Category = action },
                new GameCategory { Game = apexLegends, Category = moba },
                new GameCategory { Game = apexLegends, Category = action },
                new GameCategory { Game = rocketLeague, Category = racing },
                new GameCategory { Game = rocketLeague, Category = action },
                new GameCategory { Game = fallGuys, Category = moba },
                new GameCategory { Game = fallGuys, Category = action },
                new GameCategory { Game = hades, Category = horror },
                new GameCategory { Game = hades, Category = action },
                new GameCategory { Game = discoElysium, Category = rpg },
                new GameCategory { Game = discoElysium, Category = action },
                new GameCategory { Game = divinityOriginalSin2, Category = rpg },
                new GameCategory { Game = divinityOriginalSin2, Category = action },
                new GameCategory { Game = baldursGate3, Category = rpg },
                new GameCategory { Game = baldursGate3, Category = action },
                new GameCategory { Game = residentEvil4Remake, Category = horror },
                new GameCategory { Game = residentEvil4Remake, Category = action },
                new GameCategory { Game = hogwartsLegacy, Category = rpg },
                new GameCategory { Game = hogwartsLegacy, Category = action },
                new GameCategory { Game = liesOfP, Category = horror },
                new GameCategory { Game = liesOfP, Category = action },
                new GameCategory { Game = remnant2, Category = survival },
                new GameCategory { Game = remnant2, Category = action },
                new GameCategory { Game = armoredCore6, Category = action },
                new GameCategory { Game = armoredCore6, Category = openWorld },
                new GameCategory { Game = streetFighter6, Category = action },
                new GameCategory { Game = streetFighter6, Category = openWorld },
                new GameCategory { Game = callOfDutyWarzone, Category = fps },
                new GameCategory { Game = callOfDutyWarzone, Category = action },
                new GameCategory { Game = callOfDutyMW3, Category = action },
                new GameCategory { Game = callOfDutyMW3, Category = openWorld }
            };

            context.GameCategories.AddRange(gameCategories);
            await context.SaveChangesAsync();

            // Screenshot'ları ekle
            var screenshots = new List<Screenshot>
            {
                // CS2 Screenshots
                new Screenshot { Game = csgo, ImageUrl = "/screenshots/cs2-screenshot-1.jpg", Caption = "CS2 Oyun Görüntüsü 1" },
                new Screenshot { Game = csgo, ImageUrl = "/screenshots/cs2-screenshot-2.jpg", Caption = "CS2 Oyun Görüntüsü 2" },
                new Screenshot { Game = csgo, ImageUrl = "/screenshots/cs2-screenshot-3.jpg", Caption = "CS2 Oyun Görüntüsü 3" },
                new Screenshot { Game = csgo, ImageUrl = "/screenshots/cs2-screenshot-4.jpg", Caption = "CS2 Oyun Görüntüsü 4" },
                
                // PUBG Screenshots
                new Screenshot { Game = pubg, ImageUrl = "/screenshots/pubg-screenshot-1.jpg", Caption = "PUBG Oyun Görüntüsü 1" },
                new Screenshot { Game = pubg, ImageUrl = "/screenshots/pubg-screenshot-2.jpg", Caption = "PUBG Oyun Görüntüsü 2" },
                new Screenshot { Game = pubg, ImageUrl = "/screenshots/pubg-screenshot-3.jpg", Caption = "PUBG Oyun Görüntüsü 3" },
                new Screenshot { Game = pubg, ImageUrl = "/screenshots/pubg-screenshot-4.jpg", Caption = "PUBG Oyun Görüntüsü 4" },
                
                // FIFA 24 Screenshots
                new Screenshot { Game = fifa24, ImageUrl = "/screenshots/fc24-screenshot-1.jpg", Caption = "FIFA 24 Oyun Görüntüsü 1" },
                new Screenshot { Game = fifa24, ImageUrl = "/screenshots/fc24-screenshot-2.jpg", Caption = "FIFA 24 Oyun Görüntüsü 2" },
                new Screenshot { Game = fifa24, ImageUrl = "/screenshots/fc24-screenshot-3.jpg", Caption = "FIFA 24 Oyun Görüntüsü 3" },
                new Screenshot { Game = fifa24, ImageUrl = "/screenshots/fc24-screenshot-4.jpg", Caption = "FIFA 24 Oyun Görüntüsü 4" },
                
                // Dota 2 Screenshots
                new Screenshot { Game = dota2, ImageUrl = "/screenshots/dota2-screenshot-1.jpg", Caption = "Dota 2 Oyun Görüntüsü 1" },
                new Screenshot { Game = dota2, ImageUrl = "/screenshots/dota2-screenshot-2.jpg", Caption = "Dota 2 Oyun Görüntüsü 2" },
                new Screenshot { Game = dota2, ImageUrl = "/screenshots/dota2-screenshot-3.jpg", Caption = "Dota 2 Oyun Görüntüsü 3" },
                new Screenshot { Game = dota2, ImageUrl = "/screenshots/dota2-screenshot-4.jpg", Caption = "Dota 2 Oyun Görüntüsü 4" },
                
                // GTA V Screenshots
                new Screenshot { Game = gta5, ImageUrl = "/screenshots/gtav-screenshot-1.jpg", Caption = "GTA V Oyun Görüntüsü 1" },
                new Screenshot { Game = gta5, ImageUrl = "/screenshots/gtav-screenshot-2.jpg", Caption = "GTA V Oyun Görüntüsü 2" },
                new Screenshot { Game = gta5, ImageUrl = "/screenshots/gtav-screenshot-3.jpg", Caption = "GTA V Oyun Görüntüsü 3" },
                new Screenshot { Game = gta5, ImageUrl = "/screenshots/gtav-screenshot-4.jpg", Caption = "GTA V Oyun Görüntüsü 4" },
                
                // Witcher 3 Screenshots
                new Screenshot { Game = witcher3, ImageUrl = "/screenshots/the-witcher3-screenshot-1.jpg", Caption = "Witcher 3 Oyun Görüntüsü 1" },
                new Screenshot { Game = witcher3, ImageUrl = "/screenshots/the-witcher3-screenshot-2.jpg", Caption = "Witcher 3 Oyun Görüntüsü 2" },
                new Screenshot { Game = witcher3, ImageUrl = "/screenshots/the-witcher3-screenshot-3.jpg", Caption = "Witcher 3 Oyun Görüntüsü 3" },
                new Screenshot { Game = witcher3, ImageUrl = "/screenshots/the-witcher3-screenshot-4.jpg", Caption = "Witcher 3 Oyun Görüntüsü 4" },
                
                // Elden Ring Screenshots
                new Screenshot { Game = eldenRing, ImageUrl = "/screenshots/elden-ring-screenshot-1.jpg", Caption = "Elden Ring Oyun Görüntüsü 1" },
                new Screenshot { Game = eldenRing, ImageUrl = "/screenshots/elden-ring-screenshot-2.jpg", Caption = "Elden Ring Oyun Görüntüsü 2" },
                new Screenshot { Game = eldenRing, ImageUrl = "/screenshots/elden-ring-screenshot-3.jpg", Caption = "Elden Ring Oyun Görüntüsü 3" },
                new Screenshot { Game = eldenRing, ImageUrl = "/screenshots/elden-ring-screenshot-4.jpg", Caption = "Elden Ring Oyun Görüntüsü 4" },
                
                // Valorant Screenshots
                new Screenshot { Game = valorant, ImageUrl = "/screenshots/valorant-screenshot-1.jpg", Caption = "Valorant Oyun Görüntüsü 1" },
                new Screenshot { Game = valorant, ImageUrl = "/screenshots/valorant-screenshot-2.jpg", Caption = "Valorant Oyun Görüntüsü 2" },
                new Screenshot { Game = valorant, ImageUrl = "/screenshots/valorant-screenshot-3.jpg", Caption = "Valorant Oyun Görüntüsü 3" },
                new Screenshot { Game = valorant, ImageUrl = "/screenshots/valorant-screenshot-4.jpg", Caption = "Valorant Oyun Görüntüsü 4" },
                
                // Minecraft Screenshots
                new Screenshot { Game = minecraft, ImageUrl = "/screenshots/minecraft-screenshot-1.jpg", Caption = "Minecraft Oyun Görüntüsü 1" },
                new Screenshot { Game = minecraft, ImageUrl = "/screenshots/minecraft-screenshot-2.jpg", Caption = "Minecraft Oyun Görüntüsü 2" },
                new Screenshot { Game = minecraft, ImageUrl = "/screenshots/minecraft-screenshot-3.webp", Caption = "Minecraft Oyun Görüntüsü 3" },
                new Screenshot { Game = minecraft, ImageUrl = "/screenshots/minecraft-screenshot-4.jpg", Caption = "Minecraft Oyun Görüntüsü 4" },
                
                // Starfield Screenshots
                new Screenshot { Game = starfield, ImageUrl = "/screenshots/starfield-screenshot-1.jpg", Caption = "Starfield Oyun Görüntüsü 1" },
                new Screenshot { Game = starfield, ImageUrl = "/screenshots/starfield-screenshot-2.jpg", Caption = "Starfield Oyun Görüntüsü 2" },
                new Screenshot { Game = starfield, ImageUrl = "/screenshots/starfield-screenshot-3.jpg", Caption = "Starfield Oyun Görüntüsü 3" },
                new Screenshot { Game = starfield, ImageUrl = "/screenshots/starfield-screenshot-4.jpg", Caption = "Starfield Oyun Görüntüsü 4" },
                
                // RDR2 Screenshots
                new Screenshot { Game = rdr2, ImageUrl = "/screenshots/rdr2-screenshot-1.jpg", Caption = "RDR2 Oyun Görüntüsü 1" },
                new Screenshot { Game = rdr2, ImageUrl = "/screenshots/rdr2-screenshot-2.jpg", Caption = "RDR2 Oyun Görüntüsü 2" },
                new Screenshot { Game = rdr2, ImageUrl = "/screenshots/rdr2-screenshot-3.jpg", Caption = "RDR2 Oyun Görüntüsü 3" },
                new Screenshot { Game = rdr2, ImageUrl = "/screenshots/rdr2-screenshot-4.jpg", Caption = "RDR2 Oyun Görüntüsü 4" },
                
                // AC Valhalla Screenshots
                new Screenshot { Game = acValhalla, ImageUrl = "/screenshots/ac-valhalla-screenshot-1.jpg", Caption = "AC Valhalla Oyun Görüntüsü 1" },
                new Screenshot { Game = acValhalla, ImageUrl = "/screenshots/ac-valhalla-screenshot-2.jpg", Caption = "AC Valhalla Oyun Görüntüsü 2" },
                new Screenshot { Game = acValhalla, ImageUrl = "/screenshots/ac-valhalla-screenshot-3.jpg", Caption = "AC Valhalla Oyun Görüntüsü 3" },
                new Screenshot { Game = acValhalla, ImageUrl = "/screenshots/ac-valhalla-screenshot-4.jpg", Caption = "AC Valhalla Oyun Görüntüsü 4" },
                
                // Forza 5 Screenshots
                new Screenshot { Game = forza5, ImageUrl = "/screenshots/forza5-screenshot-1.jpg", Caption = "Forza 5 Oyun Görüntüsü 1" },
                new Screenshot { Game = forza5, ImageUrl = "/screenshots/forza5-screenshot-2.jpg", Caption = "Forza 5 Oyun Görüntüsü 2" },
                new Screenshot { Game = forza5, ImageUrl = "/screenshots/forza5-screenshot-3.jpg", Caption = "Forza 5 Oyun Görüntüsü 3" },
                new Screenshot { Game = forza5, ImageUrl = "/screenshots/forza5-screenshot-4.jpg", Caption = "Forza 5 Oyun Görüntüsü 4" },
                
                // Cyberpunk Screenshots
                new Screenshot { Game = cyberpunk, ImageUrl = "/screenshots/cyberpunk-screenshot-1.jpg", Caption = "Cyberpunk Oyun Görüntüsü 1" },
                new Screenshot { Game = cyberpunk, ImageUrl = "/screenshots/cyberpunk-screenshot-2.jpg", Caption = "Cyberpunk Oyun Görüntüsü 2" },
                new Screenshot { Game = cyberpunk, ImageUrl = "/screenshots/cyberpunk-screenshot-3.jpg", Caption = "Cyberpunk Oyun Görüntüsü 3" },
                new Screenshot { Game = cyberpunk, ImageUrl = "/screenshots/cyberpunk-screenshot-4.jpg", Caption = "Cyberpunk Oyun Görüntüsü 4" },
                
                // God of War Screenshots
                new Screenshot { Game = godOfWar, ImageUrl = "/screenshots/god-of-war-screenshot-1.jpg", Caption = "God of War Oyun Görüntüsü 1" },
                new Screenshot { Game = godOfWar, ImageUrl = "/screenshots/god-of-war-screenshot-2.jpg", Caption = "God of War Oyun Görüntüsü 2" },
                new Screenshot { Game = godOfWar, ImageUrl = "/screenshots/god-of-war-screenshot-3.jpg", Caption = "God of War Oyun Görüntüsü 3" },
                new Screenshot { Game = godOfWar, ImageUrl = "/screenshots/god-of-war-screenshot-4.jpg", Caption = "God of War Oyun Görüntüsü 4" },
                new Screenshot { Game = teamFortress2, ImageUrl = "/screenshots/tf2-screenshot-1.jpg", Caption = "Team Fortress 2 Oyun Görüntüsü 1" },
                new Screenshot { Game = teamFortress2, ImageUrl = "/screenshots/tf2-screenshot-2.jpg", Caption = "Team Fortress 2 Oyun Görüntüsü 2" },
                new Screenshot { Game = teamFortress2, ImageUrl = "/screenshots/tf2-screenshot-3.jpg", Caption = "Team Fortress 2 Oyun Görüntüsü 3" },
                new Screenshot { Game = teamFortress2, ImageUrl = "/screenshots/tf2-screenshot-4.jpg", Caption = "Team Fortress 2 Oyun Görüntüsü 4" },
                new Screenshot { Game = rust, ImageUrl = "/screenshots/rust-screenshot-1.jpg", Caption = "Rust Oyun Görüntüsü 1" },
                new Screenshot { Game = rust, ImageUrl = "/screenshots/rust-screenshot-2.jpg", Caption = "Rust Oyun Görüntüsü 2" },
                new Screenshot { Game = rust, ImageUrl = "/screenshots/rust-screenshot-3.jpg", Caption = "Rust Oyun Görüntüsü 3" },
                new Screenshot { Game = rust, ImageUrl = "/screenshots/rust-screenshot-4.jpg", Caption = "Rust Oyun Görüntüsü 4" },
                new Screenshot { Game = arkSurvival, ImageUrl = "/screenshots/ark-screenshot-1.jpg", Caption = "ARK: Survival Evolved Oyun Görüntüsü 1" },
                new Screenshot { Game = arkSurvival, ImageUrl = "/screenshots/ark-screenshot-2.jpg", Caption = "ARK: Survival Evolved Oyun Görüntüsü 2" },
                new Screenshot { Game = arkSurvival, ImageUrl = "/screenshots/ark-screenshot-3.jpg", Caption = "ARK: Survival Evolved Oyun Görüntüsü 3" },
                new Screenshot { Game = arkSurvival, ImageUrl = "/screenshots/ark-screenshot-4.jpg", Caption = "ARK: Survival Evolved Oyun Görüntüsü 4" },
                new Screenshot { Game = fallout4, ImageUrl = "/screenshots/fallout4-screenshot-1.jpg", Caption = "Fallout 4 Oyun Görüntüsü 1" },
                new Screenshot { Game = fallout4, ImageUrl = "/screenshots/fallout4-screenshot-2.jpg", Caption = "Fallout 4 Oyun Görüntüsü 2" },
                new Screenshot { Game = fallout4, ImageUrl = "/screenshots/fallout4-screenshot-3.jpg", Caption = "Fallout 4 Oyun Görüntüsü 3" },
                new Screenshot { Game = fallout4, ImageUrl = "/screenshots/fallout4-screenshot-4.jpg", Caption = "Fallout 4 Oyun Görüntüsü 4" },
                new Screenshot { Game = skyrim, ImageUrl = "/screenshots/skyrim-screenshot-1.jpg", Caption = "The Elder Scrolls V: Skyrim Oyun Görüntüsü 1" },
                new Screenshot { Game = skyrim, ImageUrl = "/screenshots/skyrim-screenshot-2.jpg", Caption = "The Elder Scrolls V: Skyrim Oyun Görüntüsü 2" },
                new Screenshot { Game = skyrim, ImageUrl = "/screenshots/skyrim-screenshot-3.jpg", Caption = "The Elder Scrolls V: Skyrim Oyun Görüntüsü 3" },
                new Screenshot { Game = skyrim, ImageUrl = "/screenshots/skyrim-screenshot-4.jpg", Caption = "The Elder Scrolls V: Skyrim Oyun Görüntüsü 4" },
                new Screenshot { Game = portal2, ImageUrl = "/screenshots/portal2-screenshot-1.jpg", Caption = "Portal 2 Oyun Görüntüsü 1" },
                new Screenshot { Game = portal2, ImageUrl = "/screenshots/portal2-screenshot-2.jpg", Caption = "Portal 2 Oyun Görüntüsü 2" },
                new Screenshot { Game = portal2, ImageUrl = "/screenshots/portal2-screenshot-3.jpg", Caption = "Portal 2 Oyun Görüntüsü 3" },
                new Screenshot { Game = portal2, ImageUrl = "/screenshots/portal2-screenshot-4.jpg", Caption = "Portal 2 Oyun Görüntüsü 4" },
                new Screenshot { Game = left4Dead2, ImageUrl = "/screenshots/l4d2-screenshot-1.jpg", Caption = "Left 4 Dead 2 Oyun Görüntüsü 1" },
                new Screenshot { Game = left4Dead2, ImageUrl = "/screenshots/l4d2-screenshot-2.jpg", Caption = "Left 4 Dead 2 Oyun Görüntüsü 2" },
                new Screenshot { Game = left4Dead2, ImageUrl = "/screenshots/l4d2-screenshot-3.jpg", Caption = "Left 4 Dead 2 Oyun Görüntüsü 3" },
                new Screenshot { Game = left4Dead2, ImageUrl = "/screenshots/l4d2-screenshot-4.jpg", Caption = "Left 4 Dead 2 Oyun Görüntüsü 4" },
                new Screenshot { Game = terraria, ImageUrl = "/screenshots/terraria-screenshot-1.jpg", Caption = "Terraria Oyun Görüntüsü 1" },
                new Screenshot { Game = terraria, ImageUrl = "/screenshots/terraria-screenshot-2.jpg", Caption = "Terraria Oyun Görüntüsü 2" },
                new Screenshot { Game = terraria, ImageUrl = "/screenshots/terraria-screenshot-3.jpg", Caption = "Terraria Oyun Görüntüsü 3" },
                new Screenshot { Game = terraria, ImageUrl = "/screenshots/terraria-screenshot-4.jpg", Caption = "Terraria Oyun Görüntüsü 4" },
                new Screenshot { Game = stardewValley, ImageUrl = "/screenshots/stardew-valley-screenshot-1.jpg", Caption = "Stardew Valley Oyun Görüntüsü 1" },
                new Screenshot { Game = stardewValley, ImageUrl = "/screenshots/stardew-valley-screenshot-2.jpg", Caption = "Stardew Valley Oyun Görüntüsü 2" },
                new Screenshot { Game = stardewValley, ImageUrl = "/screenshots/stardew-valley-screenshot-3.jpg", Caption = "Stardew Valley Oyun Görüntüsü 3" },
                new Screenshot { Game = stardewValley, ImageUrl = "/screenshots/stardew-valley-screenshot-4.jpg", Caption = "Stardew Valley Oyun Görüntüsü 4" },
                new Screenshot { Game = amongUs, ImageUrl = "/screenshots/among-us-screenshot-1.jpg", Caption = "Among Us Oyun Görüntüsü 1" },
                new Screenshot { Game = amongUs, ImageUrl = "/screenshots/among-us-screenshot-2.jpg", Caption = "Among Us Oyun Görüntüsü 2" },
                new Screenshot { Game = amongUs, ImageUrl = "/screenshots/among-us-screenshot-3.jpg", Caption = "Among Us Oyun Görüntüsü 3" },
                new Screenshot { Game = amongUs, ImageUrl = "/screenshots/among-us-screenshot-4.jpg", Caption = "Among Us Oyun Görüntüsü 4" },
                new Screenshot { Game = phasmophobia, ImageUrl = "/screenshots/phasmophobia-screenshot-1.jpg", Caption = "Phasmophobia Oyun Görüntüsü 1" },
                new Screenshot { Game = phasmophobia, ImageUrl = "/screenshots/phasmophobia-screenshot-2.jpg", Caption = "Phasmophobia Oyun Görüntüsü 2" },
                new Screenshot { Game = phasmophobia, ImageUrl = "/screenshots/phasmophobia-screenshot-3.jpg", Caption = "Phasmophobia Oyun Görüntüsü 3" },
                new Screenshot { Game = phasmophobia, ImageUrl = "/screenshots/phasmophobia-screenshot-4.jpg", Caption = "Phasmophobia Oyun Görüntüsü 4" },
                new Screenshot { Game = deadByDaylight, ImageUrl = "/screenshots/dbd-screenshot-1.jpg", Caption = "Dead by Daylight Oyun Görüntüsü 1" },
                new Screenshot { Game = deadByDaylight, ImageUrl = "/screenshots/dbd-screenshot-2.jpg", Caption = "Dead by Daylight Oyun Görüntüsü 2" },
                new Screenshot { Game = deadByDaylight, ImageUrl = "/screenshots/dbd-screenshot-3.jpg", Caption = "Dead by Daylight Oyun Görüntüsü 3" },
                new Screenshot { Game = deadByDaylight, ImageUrl = "/screenshots/dbd-screenshot-4.jpg", Caption = "Dead by Daylight Oyun Görüntüsü 4" },
                new Screenshot { Game = rainbowSixSiege, ImageUrl = "/screenshots/r6-siege-screenshot-1.jpg", Caption = "Tom Clancy's Rainbow Six Siege Oyun Görüntüsü 1" },
                new Screenshot { Game = rainbowSixSiege, ImageUrl = "/screenshots/r6-siege-screenshot-2.jpg", Caption = "Tom Clancy's Rainbow Six Siege Oyun Görüntüsü 2" },
                new Screenshot { Game = rainbowSixSiege, ImageUrl = "/screenshots/r6-siege-screenshot-3.jpg", Caption = "Tom Clancy's Rainbow Six Siege Oyun Görüntüsü 3" },
                new Screenshot { Game = rainbowSixSiege, ImageUrl = "/screenshots/r6-siege-screenshot-4.jpg", Caption = "Tom Clancy's Rainbow Six Siege Oyun Görüntüsü 4" },
                new Screenshot { Game = warframe, ImageUrl = "/screenshots/warframe-screenshot-1.jpg", Caption = "Warframe Oyun Görüntüsü 1" },
                new Screenshot { Game = warframe, ImageUrl = "/screenshots/warframe-screenshot-2.jpg", Caption = "Warframe Oyun Görüntüsü 2" },
                new Screenshot { Game = warframe, ImageUrl = "/screenshots/warframe-screenshot-3.jpg", Caption = "Warframe Oyun Görüntüsü 3" },
                new Screenshot { Game = warframe, ImageUrl = "/screenshots/warframe-screenshot-4.jpg", Caption = "Warframe Oyun Görüntüsü 4" },
                new Screenshot { Game = pathOfExile, ImageUrl = "/screenshots/poe-screenshot-1.jpg", Caption = "Path of Exile Oyun Görüntüsü 1" },
                new Screenshot { Game = pathOfExile, ImageUrl = "/screenshots/poe-screenshot-2.jpg", Caption = "Path of Exile Oyun Görüntüsü 2" },
                new Screenshot { Game = pathOfExile, ImageUrl = "/screenshots/poe-screenshot-3.jpg", Caption = "Path of Exile Oyun Görüntüsü 3" },
                new Screenshot { Game = pathOfExile, ImageUrl = "/screenshots/poe-screenshot-4.jpg", Caption = "Path of Exile Oyun Görüntüsü 4" },
                new Screenshot { Game = warThunder, ImageUrl = "/screenshots/war-thunder-screenshot-1.jpg", Caption = "War Thunder Oyun Görüntüsü 1" },
                new Screenshot { Game = warThunder, ImageUrl = "/screenshots/war-thunder-screenshot-2.jpg", Caption = "War Thunder Oyun Görüntüsü 2" },
                new Screenshot { Game = warThunder, ImageUrl = "/screenshots/war-thunder-screenshot-3.jpg", Caption = "War Thunder Oyun Görüntüsü 3" },
                new Screenshot { Game = warThunder, ImageUrl = "/screenshots/war-thunder-screenshot-4.jpg", Caption = "War Thunder Oyun Görüntüsü 4" },
                new Screenshot { Game = destiny2, ImageUrl = "/screenshots/destiny2-screenshot-1.jpg", Caption = "Destiny 2 Oyun Görüntüsü 1" },
                new Screenshot { Game = destiny2, ImageUrl = "/screenshots/destiny2-screenshot-2.jpg", Caption = "Destiny 2 Oyun Görüntüsü 2" },
                new Screenshot { Game = destiny2, ImageUrl = "/screenshots/destiny2-screenshot-3.jpg", Caption = "Destiny 2 Oyun Görüntüsü 3" },
                new Screenshot { Game = destiny2, ImageUrl = "/screenshots/destiny2-screenshot-4.jpg", Caption = "Destiny 2 Oyun Görüntüsü 4" },
                new Screenshot { Game = apexLegends, ImageUrl = "/screenshots/apex-legends-screenshot-1.jpg", Caption = "Apex Legends Oyun Görüntüsü 1" },
                new Screenshot { Game = apexLegends, ImageUrl = "/screenshots/apex-legends-screenshot-2.jpg", Caption = "Apex Legends Oyun Görüntüsü 2" },
                new Screenshot { Game = apexLegends, ImageUrl = "/screenshots/apex-legends-screenshot-3.jpg", Caption = "Apex Legends Oyun Görüntüsü 3" },
                new Screenshot { Game = apexLegends, ImageUrl = "/screenshots/apex-legends-screenshot-4.jpg", Caption = "Apex Legends Oyun Görüntüsü 4" },
                new Screenshot { Game = rocketLeague, ImageUrl = "/screenshots/rocket-league-screenshot-1.jpg", Caption = "Rocket League Oyun Görüntüsü 1" },
                new Screenshot { Game = rocketLeague, ImageUrl = "/screenshots/rocket-league-screenshot-2.jpg", Caption = "Rocket League Oyun Görüntüsü 2" },
                new Screenshot { Game = rocketLeague, ImageUrl = "/screenshots/rocket-league-screenshot-3.jpg", Caption = "Rocket League Oyun Görüntüsü 3" },
                new Screenshot { Game = rocketLeague, ImageUrl = "/screenshots/rocket-league-screenshot-4.jpg", Caption = "Rocket League Oyun Görüntüsü 4" },
                new Screenshot { Game = fallGuys, ImageUrl = "/screenshots/fall-guys-screenshot-1.jpg", Caption = "Fall Guys: Ultimate Knockout Oyun Görüntüsü 1" },
                new Screenshot { Game = fallGuys, ImageUrl = "/screenshots/fall-guys-screenshot-2.jpg", Caption = "Fall Guys: Ultimate Knockout Oyun Görüntüsü 2" },
                new Screenshot { Game = fallGuys, ImageUrl = "/screenshots/fall-guys-screenshot-3.jpg", Caption = "Fall Guys: Ultimate Knockout Oyun Görüntüsü 3" },
                new Screenshot { Game = fallGuys, ImageUrl = "/screenshots/fall-guys-screenshot-4.jpg", Caption = "Fall Guys: Ultimate Knockout Oyun Görüntüsü 4" },
                new Screenshot { Game = hades, ImageUrl = "/screenshots/hades-screenshot-1.jpg", Caption = "Hades Oyun Görüntüsü 1" },
                new Screenshot { Game = hades, ImageUrl = "/screenshots/hades-screenshot-2.jpg", Caption = "Hades Oyun Görüntüsü 2" },
                new Screenshot { Game = hades, ImageUrl = "/screenshots/hades-screenshot-3.jpg", Caption = "Hades Oyun Görüntüsü 3" },
                new Screenshot { Game = hades, ImageUrl = "/screenshots/hades-screenshot-4.jpg", Caption = "Hades Oyun Görüntüsü 4" },
                new Screenshot { Game = discoElysium, ImageUrl = "/screenshots/disco-elysium-screenshot-1.jpg", Caption = "Disco Elysium - The Final Cut Oyun Görüntüsü 1" },
                new Screenshot { Game = discoElysium, ImageUrl = "/screenshots/disco-elysium-screenshot-2.jpg", Caption = "Disco Elysium - The Final Cut Oyun Görüntüsü 2" },
                new Screenshot { Game = discoElysium, ImageUrl = "/screenshots/disco-elysium-screenshot-3.jpg", Caption = "Disco Elysium - The Final Cut Oyun Görüntüsü 3" },
                new Screenshot { Game = discoElysium, ImageUrl = "/screenshots/disco-elysium-screenshot-4.jpg", Caption = "Disco Elysium - The Final Cut Oyun Görüntüsü 4" },
                new Screenshot { Game = divinityOriginalSin2, ImageUrl = "/screenshots/divinity-os2-screenshot-1.jpg", Caption = "Divinity: Original Sin 2 - Definitive Edition Oyun Görüntüsü 1" },
                new Screenshot { Game = divinityOriginalSin2, ImageUrl = "/screenshots/divinity-os2-screenshot-2.jpg", Caption = "Divinity: Original Sin 2 - Definitive Edition Oyun Görüntüsü 2" },
                new Screenshot { Game = divinityOriginalSin2, ImageUrl = "/screenshots/divinity-os2-screenshot-3.jpg", Caption = "Divinity: Original Sin 2 - Definitive Edition Oyun Görüntüsü 3" },
                new Screenshot { Game = divinityOriginalSin2, ImageUrl = "/screenshots/divinity-os2-screenshot-4.jpg", Caption = "Divinity: Original Sin 2 - Definitive Edition Oyun Görüntüsü 4" },
                new Screenshot { Game = baldursGate3, ImageUrl = "/screenshots/baldurs-gate-3-screenshot-1.jpg", Caption = "Baldur's Gate 3 Oyun Görüntüsü 1" },
                new Screenshot { Game = baldursGate3, ImageUrl = "/screenshots/baldurs-gate-3-screenshot-2.jpg", Caption = "Baldur's Gate 3 Oyun Görüntüsü 2" },
                new Screenshot { Game = baldursGate3, ImageUrl = "/screenshots/baldurs-gate-3-screenshot-3.jpg", Caption = "Baldur's Gate 3 Oyun Görüntüsü 3" },
                new Screenshot { Game = baldursGate3, ImageUrl = "/screenshots/baldurs-gate-3-screenshot-4.jpg", Caption = "Baldur's Gate 3 Oyun Görüntüsü 4" },
                new Screenshot { Game = residentEvil4Remake, ImageUrl = "/screenshots/re4-remake-screenshot-1.jpg", Caption = "Resident Evil 4 Remake Oyun Görüntüsü 1" },
                new Screenshot { Game = residentEvil4Remake, ImageUrl = "/screenshots/re4-remake-screenshot-2.jpg", Caption = "Resident Evil 4 Remake Oyun Görüntüsü 2" },
                new Screenshot { Game = residentEvil4Remake, ImageUrl = "/screenshots/re4-remake-screenshot-3.jpg", Caption = "Resident Evil 4 Remake Oyun Görüntüsü 3" },
                new Screenshot { Game = residentEvil4Remake, ImageUrl = "/screenshots/re4-remake-screenshot-4.jpg", Caption = "Resident Evil 4 Remake Oyun Görüntüsü 4" },
                new Screenshot { Game = hogwartsLegacy, ImageUrl = "/screenshots/hogwarts-legacy-screenshot-1.jpg", Caption = "Hogwarts Legacy Oyun Görüntüsü 1" },
                new Screenshot { Game = hogwartsLegacy, ImageUrl = "/screenshots/hogwarts-legacy-screenshot-2.jpg", Caption = "Hogwarts Legacy Oyun Görüntüsü 2" },
                new Screenshot { Game = hogwartsLegacy, ImageUrl = "/screenshots/hogwarts-legacy-screenshot-3.jpg", Caption = "Hogwarts Legacy Oyun Görüntüsü 3" },
                new Screenshot { Game = hogwartsLegacy, ImageUrl = "/screenshots/hogwarts-legacy-screenshot-4.jpg", Caption = "Hogwarts Legacy Oyun Görüntüsü 4" },
                new Screenshot { Game = liesOfP, ImageUrl = "/screenshots/lies-of-p-screenshot-1.jpg", Caption = "Lies of P Oyun Görüntüsü 1" },
                new Screenshot { Game = liesOfP, ImageUrl = "/screenshots/lies-of-p-screenshot-2.jpg", Caption = "Lies of P Oyun Görüntüsü 2" },
                new Screenshot { Game = liesOfP, ImageUrl = "/screenshots/lies-of-p-screenshot-3.jpg", Caption = "Lies of P Oyun Görüntüsü 3" },
                new Screenshot { Game = liesOfP, ImageUrl = "/screenshots/lies-of-p-screenshot-4.jpg", Caption = "Lies of P Oyun Görüntüsü 4" },
                new Screenshot { Game = remnant2, ImageUrl = "/screenshots/remnant-2-screenshot-1.jpg", Caption = "Remnant 2 Oyun Görüntüsü 1" },
                new Screenshot { Game = remnant2, ImageUrl = "/screenshots/remnant-2-screenshot-2.jpg", Caption = "Remnant 2 Oyun Görüntüsü 2" },
                new Screenshot { Game = remnant2, ImageUrl = "/screenshots/remnant-2-screenshot-3.jpg", Caption = "Remnant 2 Oyun Görüntüsü 3" },
                new Screenshot { Game = remnant2, ImageUrl = "/screenshots/remnant-2-screenshot-4.jpg", Caption = "Remnant 2 Oyun Görüntüsü 4" },
                new Screenshot { Game = armoredCore6, ImageUrl = "/screenshots/armored-core-6-screenshot-1.jpg", Caption = "ARMORED CORE VI FIRES OF RUBICON Oyun Görüntüsü 1" },
                new Screenshot { Game = armoredCore6, ImageUrl = "/screenshots/armored-core-6-screenshot-2.jpg", Caption = "ARMORED CORE VI FIRES OF RUBICON Oyun Görüntüsü 2" },
                new Screenshot { Game = armoredCore6, ImageUrl = "/screenshots/armored-core-6-screenshot-3.jpg", Caption = "ARMORED CORE VI FIRES OF RUBICON Oyun Görüntüsü 3" },
                new Screenshot { Game = armoredCore6, ImageUrl = "/screenshots/armored-core-6-screenshot-4.jpg", Caption = "ARMORED CORE VI FIRES OF RUBICON Oyun Görüntüsü 4" },
                new Screenshot { Game = streetFighter6, ImageUrl = "/screenshots/street-fighter-6-screenshot-1.jpg", Caption = "Street Fighter 6 Oyun Görüntüsü 1" },
                new Screenshot { Game = streetFighter6, ImageUrl = "/screenshots/street-fighter-6-screenshot-2.jpg", Caption = "Street Fighter 6 Oyun Görüntüsü 2" },
                new Screenshot { Game = streetFighter6, ImageUrl = "/screenshots/street-fighter-6-screenshot-3.jpg", Caption = "Street Fighter 6 Oyun Görüntüsü 3" },
                new Screenshot { Game = streetFighter6, ImageUrl = "/screenshots/street-fighter-6-screenshot-4.jpg", Caption = "Street Fighter 6 Oyun Görüntüsü 4" },
                new Screenshot { Game = callOfDutyWarzone, ImageUrl = "/screenshots/cod-warzone-screenshot-1.jpg", Caption = "Call of Duty: Warzone Oyun Görüntüsü 1" },
                new Screenshot { Game = callOfDutyWarzone, ImageUrl = "/screenshots/cod-warzone-screenshot-2.jpg", Caption = "Call of Duty: Warzone Oyun Görüntüsü 2" },
                new Screenshot { Game = callOfDutyWarzone, ImageUrl = "/screenshots/cod-warzone-screenshot-3.jpg", Caption = "Call of Duty: Warzone Oyun Görüntüsü 3" },
                new Screenshot { Game = callOfDutyWarzone, ImageUrl = "/screenshots/cod-warzone-screenshot-4.jpg", Caption = "Call of Duty: Warzone Oyun Görüntüsü 4" },
                new Screenshot { Game = callOfDutyMW3, ImageUrl = "/screenshots/cod-mw3-screenshot-1.jpg", Caption = "Call of Duty: Modern Warfare III Oyun Görüntüsü 1" },
                new Screenshot { Game = callOfDutyMW3, ImageUrl = "/screenshots/cod-mw3-screenshot-2.jpg", Caption = "Call of Duty: Modern Warfare III Oyun Görüntüsü 2" },
                new Screenshot { Game = callOfDutyMW3, ImageUrl = "/screenshots/cod-mw3-screenshot-3.jpg", Caption = "Call of Duty: Modern Warfare III Oyun Görüntüsü 3" },
                new Screenshot { Game = callOfDutyMW3, ImageUrl = "/screenshots/cod-mw3-screenshot-4.jpg", Caption = "Call of Duty: Modern Warfare III Oyun Görüntüsü 4" }
            };

            context.Screenshots.AddRange(screenshots);
            await context.SaveChangesAsync();

            // Oyun dillerini ekle
            var gameLanguages = new List<GameLanguage>
            {
                // Tüm oyunlar için Türkçe ve İngilizce
                new GameLanguage { Game = csgo, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = csgo, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = pubg, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = pubg, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = fifa24, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = fifa24, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = dota2, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = dota2, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = gta5, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = gta5, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = witcher3, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = witcher3, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = eldenRing, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = eldenRing, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = valorant, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = valorant, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = minecraft, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = minecraft, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = starfield, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = starfield, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = rdr2, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = rdr2, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = acValhalla, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = acValhalla, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = forza5, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = forza5, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = cyberpunk, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = cyberpunk, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = godOfWar, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = godOfWar, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = teamFortress2, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = teamFortress2, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = rust, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = rust, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = arkSurvival, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = arkSurvival, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = fallout4, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = fallout4, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = skyrim, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = skyrim, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = portal2, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = portal2, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = left4Dead2, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = left4Dead2, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = terraria, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = terraria, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = stardewValley, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = stardewValley, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = amongUs, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = amongUs, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = phasmophobia, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = phasmophobia, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = deadByDaylight, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = deadByDaylight, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = rainbowSixSiege, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = rainbowSixSiege, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = warframe, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = warframe, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = pathOfExile, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = pathOfExile, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = warThunder, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = warThunder, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = destiny2, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = destiny2, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = apexLegends, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = apexLegends, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = rocketLeague, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = rocketLeague, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = fallGuys, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = fallGuys, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = hades, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = hades, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = discoElysium, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = discoElysium, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = divinityOriginalSin2, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = divinityOriginalSin2, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = baldursGate3, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = baldursGate3, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = residentEvil4Remake, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = residentEvil4Remake, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = hogwartsLegacy, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = hogwartsLegacy, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = liesOfP, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = liesOfP, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = remnant2, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = remnant2, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = armoredCore6, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = armoredCore6, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = streetFighter6, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = streetFighter6, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = callOfDutyWarzone, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = callOfDutyWarzone, Language = "English", Interface = true, Audio = true, Subtitles = true },
                new GameLanguage { Game = callOfDutyMW3, Language = "Türkçe", Interface = true, Audio = false, Subtitles = true },
                new GameLanguage { Game = callOfDutyMW3, Language = "English", Interface = true, Audio = true, Subtitles = true }
            };

            context.GameLanguages.AddRange(gameLanguages);
            await context.SaveChangesAsync();

            // Video verilerini ekle
            var videos = new List<Video>
            {
                // CS2 Video
                new Video { Game = csgo, Title = "CS2 Gameplay", Url = "/screenshots/cs2-video-1.webm", Description = "Counter-Strike 2 oyun görüntüleri", IsFeatured = true },
                
                // PUBG Video
                new Video { Game = pubg, Title = "PUBG Gameplay", Url = "/screenshots/pubg-video-1.webm", Description = "PUBG: Battlegrounds oyun görüntüleri", IsFeatured = true },
                
                // FIFA 24 Video
                new Video { Game = fifa24, Title = "FIFA 24 Gameplay", Url = "/screenshots/fc24-video-1.webm", Description = "EA FC 24 oyun görüntüleri", IsFeatured = true },
                
                // Dota 2 Video
                new Video { Game = dota2, Title = "Dota 2 Gameplay", Url = "/screenshots/dota2-video-1.webm", Description = "Dota 2 oyun görüntüleri", IsFeatured = true },
                
                // GTA V Video
                new Video { Game = gta5, Title = "GTA V Gameplay", Url = "/screenshots/gtav-video-1.webm", Description = "Grand Theft Auto V oyun görüntüleri", IsFeatured = true },
                
                // Witcher 3 Video
                new Video { Game = witcher3, Title = "Witcher 3 Gameplay", Url = "/screenshots/the-witcher3-video-1.webm", Description = "The Witcher 3: Wild Hunt oyun görüntüleri", IsFeatured = true },
                
                // Elden Ring Video
                new Video { Game = eldenRing, Title = "Elden Ring Gameplay", Url = "/screenshots/elden-ring-video-1.webm", Description = "Elden Ring oyun görüntüleri", IsFeatured = true },
                
                // Valorant Video
                new Video { Game = valorant, Title = "Valorant Gameplay", Url = "/screenshots/valorant-video-1.webm", Description = "Valorant oyun görüntüleri", IsFeatured = true },
                
                // Minecraft Video
                new Video { Game = minecraft, Title = "Minecraft Gameplay", Url = "/screenshots/minecraft-video-1.webm", Description = "Minecraft oyun görüntüleri", IsFeatured = true },
                
                // Starfield Video
                new Video { Game = starfield, Title = "Starfield Gameplay", Url = "/screenshots/starfield-video-1.webm", Description = "Starfield oyun görüntüleri", IsFeatured = true },
                
                // RDR2 Video
                new Video { Game = rdr2, Title = "RDR2 Gameplay", Url = "/screenshots/rdr2-video-1.webm", Description = "Red Dead Redemption 2 oyun görüntüleri", IsFeatured = true },
                
                // AC Valhalla Video
                new Video { Game = acValhalla, Title = "AC Valhalla Gameplay", Url = "/screenshots/ac-valhalla-video-1.webm", Description = "Assassin's Creed Valhalla oyun görüntüleri", IsFeatured = true },
                
                // Forza 5 Video
                new Video { Game = forza5, Title = "Forza 5 Gameplay", Url = "/screenshots/forza5-video-1.webm", Description = "Forza Horizon 5 oyun görüntüleri", IsFeatured = true },
                
                // Cyberpunk Video
                new Video { Game = cyberpunk, Title = "Cyberpunk Gameplay", Url = "/screenshots/cyberpunk-video-1.webm", Description = "Cyberpunk 2077 oyun görüntüleri", IsFeatured = true },
                
                // God of War Video
                new Video { Game = godOfWar, Title = "God of War Gameplay", Url = "/screenshots/god-of-war-video-1.webm", Description = "God of War oyun görüntüleri", IsFeatured = true },
                
                // Team Fortress 2 Video
                new Video { Game = teamFortress2, Title = "TF2 Gameplay", Url = "/screenshots/tf2-video-1.webm", Description = "Team Fortress 2 oyun görüntüleri", IsFeatured = true },
                
                // Rust Video
                new Video { Game = rust, Title = "Rust Gameplay", Url = "/screenshots/rust-video-1.webm", Description = "Rust oyun görüntüleri", IsFeatured = true },
                
                // ARK Video
                new Video { Game = arkSurvival, Title = "ARK Gameplay", Url = "/screenshots/ark-video-1.webm", Description = "ARK: Survival Evolved oyun görüntüleri", IsFeatured = true },
                
                // Fallout 4 Video
                new Video { Game = fallout4, Title = "Fallout 4 Gameplay", Url = "/screenshots/fallout4-video-1.webm", Description = "Fallout 4 oyun görüntüleri", IsFeatured = true },
                
                // Skyrim Video
                new Video { Game = skyrim, Title = "Skyrim Gameplay", Url = "/screenshots/skyrim-video-1.webm", Description = "The Elder Scrolls V: Skyrim oyun görüntüleri", IsFeatured = true },
                
                // Portal 2 Video
                new Video { Game = portal2, Title = "Portal 2 Gameplay", Url = "/screenshots/portal2-video-1.webm", Description = "Portal 2 oyun görüntüleri", IsFeatured = true },
                
                // Left 4 Dead 2 Video
                new Video { Game = left4Dead2, Title = "L4D2 Gameplay", Url = "/screenshots/l4d2-video-1.webm", Description = "Left 4 Dead 2 oyun görüntüleri", IsFeatured = true },
                
                // Terraria Video
                new Video { Game = terraria, Title = "Terraria Gameplay", Url = "/screenshots/terraria-video-1.webm", Description = "Terraria oyun görüntüleri", IsFeatured = true },
                
                // Stardew Valley Video
                new Video { Game = stardewValley, Title = "Stardew Valley Gameplay", Url = "/screenshots/stardew-valley-video-1.webm", Description = "Stardew Valley oyun görüntüleri", IsFeatured = true },
                
                // Among Us Video
                new Video { Game = amongUs, Title = "Among Us Gameplay", Url = "/screenshots/among-us-video-1.webm", Description = "Among Us oyun görüntüleri", IsFeatured = true },
                
                // Phasmophobia Video
                new Video { Game = phasmophobia, Title = "Phasmophobia Gameplay", Url = "/screenshots/phasmophobia-video-1.webm", Description = "Phasmophobia oyun görüntüleri", IsFeatured = true },
                
                // Dead by Daylight Video
                new Video { Game = deadByDaylight, Title = "DBD Gameplay", Url = "/screenshots/dbd-video-1.webm", Description = "Dead by Daylight oyun görüntüleri", IsFeatured = true },
                
                // Rainbow Six Siege Video
                new Video { Game = rainbowSixSiege, Title = "R6 Siege Gameplay", Url = "/screenshots/r6-siege-video-1.webm", Description = "Tom Clancy's Rainbow Six Siege oyun görüntüleri", IsFeatured = true },
                
                // Warframe Video
                new Video { Game = warframe, Title = "Warframe Gameplay", Url = "/screenshots/warframe-video-1.webm", Description = "Warframe oyun görüntüleri", IsFeatured = true },
                
                // Path of Exile Video
                new Video { Game = pathOfExile, Title = "PoE Gameplay", Url = "/screenshots/poe-video-1.webm", Description = "Path of Exile oyun görüntüleri", IsFeatured = true },
                
                // War Thunder Video
                new Video { Game = warThunder, Title = "War Thunder Gameplay", Url = "/screenshots/war-thunder-video-1.webm", Description = "War Thunder oyun görüntüleri", IsFeatured = true },
                
                // Destiny 2 Video
                new Video { Game = destiny2, Title = "Destiny 2 Gameplay", Url = "/screenshots/destiny2-video-1.webm", Description = "Destiny 2 oyun görüntüleri", IsFeatured = true },
                
                // Apex Legends Video
                new Video { Game = apexLegends, Title = "Apex Legends Gameplay", Url = "/screenshots/apex-legends-video-1.webm", Description = "Apex Legends oyun görüntüleri", IsFeatured = true },
                
                // Rocket League Video
                new Video { Game = rocketLeague, Title = "Rocket League Gameplay", Url = "/screenshots/rocket-league-video-1.webm", Description = "Rocket League oyun görüntüleri", IsFeatured = true },
                
                // Fall Guys Video
                new Video { Game = fallGuys, Title = "Fall Guys Gameplay", Url = "/screenshots/fall-guys-video-1.mp4", Description = "Fall Guys: Ultimate Knockout oyun görüntüleri", IsFeatured = true },
                
                // Hades Video
                new Video { Game = hades, Title = "Hades Gameplay", Url = "/screenshots/hades-video-1.webm", Description = "Hades oyun görüntüleri", IsFeatured = true },
                
                // Disco Elysium Video
                new Video { Game = discoElysium, Title = "Disco Elysium Gameplay", Url = "/screenshots/disco-elysium-video-1.webm", Description = "Disco Elysium - The Final Cut oyun görüntüleri", IsFeatured = true },
                
                // Divinity Original Sin 2 Video
                new Video { Game = divinityOriginalSin2, Title = "Divinity OS2 Gameplay", Url = "/screenshots/divinity-os2-video-1.webm", Description = "Divinity: Original Sin 2 - Definitive Edition oyun görüntüleri", IsFeatured = true },
                
                // Baldur's Gate 3 Video
                new Video { Game = baldursGate3, Title = "BG3 Gameplay", Url = "/screenshots/baldurs-gate-3-video-1.webm", Description = "Baldur's Gate 3 oyun görüntüleri", IsFeatured = true },
                
                // Resident Evil 4 Remake Video
                new Video { Game = residentEvil4Remake, Title = "RE4 Remake Gameplay", Url = "/screenshots/re4-remake-video-1.webm", Description = "Resident Evil 4 Remake oyun görüntüleri", IsFeatured = true },
                
                // Hogwarts Legacy Video
                new Video { Game = hogwartsLegacy, Title = "Hogwarts Legacy Gameplay", Url = "/screenshots/hogwarts-legacy-video-1.webm", Description = "Hogwarts Legacy oyun görüntüleri", IsFeatured = true },
                
                // Lies of P Video
                new Video { Game = liesOfP, Title = "Lies of P Gameplay", Url = "/screenshots/lies-of-p-video-1.webm", Description = "Lies of P oyun görüntüleri", IsFeatured = true },
                
                // Remnant 2 Video
                new Video { Game = remnant2, Title = "Remnant 2 Gameplay", Url = "/screenshots/remnant-2-video-1.webm", Description = "Remnant 2 oyun görüntüleri", IsFeatured = true },
                
                // Armored Core 6 Video
                new Video { Game = armoredCore6, Title = "AC6 Gameplay", Url = "/screenshots/armored-core-6-video-1.webm", Description = "ARMORED CORE VI FIRES OF RUBICON oyun görüntüleri", IsFeatured = true },
                
                // Street Fighter 6 Video
                new Video { Game = streetFighter6, Title = "SF6 Gameplay", Url = "/screenshots/street-fighter-6-video-1.webm", Description = "Street Fighter 6 oyun görüntüleri", IsFeatured = true },
                
                // Call of Duty Warzone Video
                new Video { Game = callOfDutyWarzone, Title = "COD Warzone Gameplay", Url = "/screenshots/cod-warzone-video-1.webm", Description = "Call of Duty: Warzone oyun görüntüleri", IsFeatured = true },
                
                // Call of Duty MW3 Video
                new Video { Game = callOfDutyMW3, Title = "COD MW3 Gameplay", Url = "/screenshots/cod-mw3-video-1.webm", Description = "Call of Duty: Modern Warfare III oyun görüntüleri", IsFeatured = true }
            };

            context.Videos.AddRange(videos);
            await context.SaveChangesAsync();

            // Bundle'ları ekle
            var valveBundle = new Bundle
            {
                Name = "Valve Complete Pack",
                Description = "Valve'ın en popüler ücretli oyunlarının koleksiyonu. Portal 2, Left 4 Dead 2 ve daha fazlası.",
                DiscountPercentage = 25,
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true,
                CoverImage = "/games/portal2-cover.jpg"
            };

            var rockstarBundle = new Bundle
            {
                Name = "Rockstar Classics",
                Description = "Rockstar Games'in efsanevi oyunları. Grand Theft Auto V ve Red Dead Redemption 2 ile açık dünya deneyiminin zirvesi.",
                DiscountPercentage = 30,
                StartDate = DateTime.Now.AddDays(-15),
                EndDate = DateTime.Now.AddDays(45),
                IsActive = true,
                CoverImage = "/games/gtav-cover.jpg"
            };

            var bethesdaBundle = new Bundle
            {
                Name = "Bethesda RPG Collection",
                Description = "Bethesda'nın efsanevi RPG oyunları. Starfield, Fallout 4, Skyrim ile rol yapma oyunlarının en iyileri.",
                DiscountPercentage = 35,
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(25),
                IsActive = true,
                CoverImage = "/games/starfield-cover.jpg"
            };

            var indieBundle = new Bundle
            {
                Name = "Indie Gems",
                Description = "Bağımsız geliştiricilerin en iyi oyunları. Hades, Stardew Valley, Terraria ile benzersiz deneyimler.",
                DiscountPercentage = 40,
                StartDate = DateTime.Now.AddDays(-20),
                EndDate = DateTime.Now.AddDays(10),
                IsActive = true,
                CoverImage = "/games/hades-cover.jpg"
            };

            var horrorBundle = new Bundle
            {
                Name = "Horror Collection",
                Description = "En korkunç oyunların koleksiyonu. Resident Evil 4 Remake, Dead by Daylight, Phasmophobia ve daha fazlası.",
                DiscountPercentage = 25,
                StartDate = DateTime.Now.AddDays(-12),
                EndDate = DateTime.Now.AddDays(18),
                IsActive = true,
                CoverImage = "/games/re4-remake-cover.jpg"
            };

            var racingBundle = new Bundle
            {
                Name = "Racing Legends",
                Description = "En hızlı yarış oyunları. Forza Horizon 5, Rocket League ile hız tutkunları için mükemmel paket.",
                DiscountPercentage = 20,
                StartDate = DateTime.Now.AddDays(-8),
                EndDate = DateTime.Now.AddDays(22),
                IsActive = true,
                CoverImage = "/games/forza5-cover.jpg"
            };

            context.Bundles.AddRange(valveBundle, rockstarBundle, bethesdaBundle, indieBundle, horrorBundle, racingBundle);
            await context.SaveChangesAsync();

            // Bundle'lara oyunları ekle
            var valveGames = new List<Game> { portal2, left4Dead2 }; // CS2 ve TF2 ücretsiz olduğu için çıkartıldı
            var rockstarGames = new List<Game> { gta5, rdr2 };
            var bethesdaGames = new List<Game> { starfield, fallout4, skyrim };
            var indieGames = new List<Game> { hades, stardewValley, terraria }; // Among Us ücretsiz olduğu için çıkartıldı
            var horrorGames = new List<Game> { residentEvil4Remake, deadByDaylight, phasmophobia };
            var racingGames = new List<Game> { forza5, rocketLeague };

            foreach (var game in valveGames)
            {
                valveBundle.Games.Add(game);
            }
            foreach (var game in rockstarGames)
            {
                rockstarBundle.Games.Add(game);
            }
            foreach (var game in bethesdaGames)
            {
                bethesdaBundle.Games.Add(game);
            }
            foreach (var game in indieGames)
            {
                indieBundle.Games.Add(game);
            }
            foreach (var game in horrorGames)
            {
                horrorBundle.Games.Add(game);
            }
            foreach (var game in racingGames)
            {
                racingBundle.Games.Add(game);
            }

            // Bundle fiyatlarını oyunların toplamı olarak ayarla
            valveBundle.Price = valveBundle.Games.Sum(g => g.Price);
            rockstarBundle.Price = rockstarBundle.Games.Sum(g => g.Price);
            bethesdaBundle.Price = bethesdaBundle.Games.Sum(g => g.Price);
            indieBundle.Price = indieBundle.Games.Sum(g => g.Price);
            horrorBundle.Price = horrorBundle.Games.Sum(g => g.Price);
            racingBundle.Price = racingBundle.Games.Sum(g => g.Price);

            await context.SaveChangesAsync();

            // Bundle indirimlerini ekle
            Console.WriteLine("Bundle indirimleri ekleniyor...");
            
            var bundleDiscounts = new List<BundleDiscount>
            {
                new BundleDiscount
                {
                    BundleId = valveBundle.BundleId,
                    DiscountPercentage = valveBundle.DiscountPercentage,
                    StartDate = valveBundle.StartDate,
                    EndDate = valveBundle.EndDate,
                    IsActive = valveBundle.IsActive,
                    Description = "Valve Complete Pack için özel indirim"
                },
                new BundleDiscount
                {
                    BundleId = rockstarBundle.BundleId,
                    DiscountPercentage = rockstarBundle.DiscountPercentage,
                    StartDate = rockstarBundle.StartDate,
                    EndDate = rockstarBundle.EndDate,
                    IsActive = rockstarBundle.IsActive,
                    Description = "Rockstar Classics için özel indirim"
                },
                new BundleDiscount
                {
                    BundleId = bethesdaBundle.BundleId,
                    DiscountPercentage = bethesdaBundle.DiscountPercentage,
                    StartDate = bethesdaBundle.StartDate,
                    EndDate = bethesdaBundle.EndDate,
                    IsActive = bethesdaBundle.IsActive,
                    Description = "Bethesda RPG Collection için özel indirim"
                },
                new BundleDiscount
                {
                    BundleId = indieBundle.BundleId,
                    DiscountPercentage = indieBundle.DiscountPercentage,
                    StartDate = indieBundle.StartDate,
                    EndDate = indieBundle.EndDate,
                    IsActive = indieBundle.IsActive,
                    Description = "Indie Gems için özel indirim"
                },
                new BundleDiscount
                {
                    BundleId = horrorBundle.BundleId,
                    DiscountPercentage = horrorBundle.DiscountPercentage,
                    StartDate = horrorBundle.StartDate,
                    EndDate = horrorBundle.EndDate,
                    IsActive = horrorBundle.IsActive,
                    Description = "Horror Collection için özel indirim"
                },
                new BundleDiscount
                {
                    BundleId = racingBundle.BundleId,
                    DiscountPercentage = racingBundle.DiscountPercentage,
                    StartDate = racingBundle.StartDate,
                    EndDate = racingBundle.EndDate,
                    IsActive = racingBundle.IsActive,
                    Description = "Racing Legends için özel indirim"
                }
            };

            context.BundleDiscounts.AddRange(bundleDiscounts);
            await context.SaveChangesAsync();

            Console.WriteLine($"Toplam {bundleDiscounts.Count} bundle indirimi eklendi!");

            // Achievement'ları ekle
            Console.WriteLine("Achievement'lar ekleniyor...");
            
            var allGames = context.Games.ToList();
            var achievements = new List<Achievement>();

            foreach (var game in allGames)
            {
                // 1. Oyunu Başlatma Achievement'ı
                var startGameAchievement = new Achievement
                {
                    GameId = game.GameId,
                    Name = "İlk Adım",
                    Description = $"{game.Title} oyununu başlat",
                    Icon = "🎮",
                    IsHidden = false,
                    UnlockRate = 0.0
                };
                achievements.Add(startGameAchievement);

                // 2. 10 Saniye Oynama Achievement'ı
                var play10SecondsAchievement = new Achievement
                {
                    GameId = game.GameId,
                    Name = "Başlangıç",
                    Description = $"{game.Title} oyununu 10 saniye oyna",
                    Icon = "⏱️",
                    IsHidden = false,
                    UnlockRate = 0.0
                };
                achievements.Add(play10SecondsAchievement);

                // 3. 1 Dakika Oynama Achievement'ı
                var play1MinuteAchievement = new Achievement
                {
                    GameId = game.GameId,
                    Name = "Tutkulu Oyuncu",
                    Description = $"{game.Title} oyununu 1 dakika oyna",
                    Icon = "🏆",
                    IsHidden = false,
                    UnlockRate = 0.0
                };
                achievements.Add(play1MinuteAchievement);
            }

            context.Achievements.AddRange(achievements);
            await context.SaveChangesAsync();

            Console.WriteLine($"Toplam {achievements.Count} achievement eklendi!");

            // DLC'leri ekle
            Console.WriteLine("DLC'ler ekleniyor...");
            
            var dlcs = new List<DLC>
            {
                // The Witcher 3 DLC
                new DLC
                {
                    Name = "Hearts of Stone",
                    Description = "The Witcher 3: Wild Hunt'ın ilk genişletme paketi. Oxenfurt ve Novigrad'da geçen yeni bir hikaye, yeni karakterler ve görevler.",
                    Price = 9.99m,
                    ReleaseDate = new DateTime(2015, 10, 13),
                    GameId = context.Games.First(g => g.Title == "The Witcher 3: Wild Hunt").GameId,
                    CoverImage = "witcher3-hearts-of-stone-cover.jpg"
                },
                // GTA V DLC
                new DLC
                {
                    Name = "The Cayo Perico Heist",
                    Description = "Grand Theft Auto V Online için yeni bir heist görevi. Cayo Perico adasında geçen bu görevde yeni araçlar, silahlar ve ödüller.",
                    Price = 0.00m, // Ücretsiz DLC
                    ReleaseDate = new DateTime(2020, 12, 15),
                    GameId = context.Games.First(g => g.Title == "Grand Theft Auto V").GameId,
                    CoverImage = "gtav-cayo-perico-cover.jpg"
                },
                // Skyrim DLC
                new DLC
                {
                    Name = "Dawnguard",
                    Description = "The Elder Scrolls V: Skyrim'in ilk DLC'si. Vampir lordları ve Dawnguard savaşçıları arasındaki çatışmada yer alın.",
                    Price = 19.99m,
                    ReleaseDate = new DateTime(2012, 8, 2),
                    GameId = context.Games.First(g => g.Title == "The Elder Scrolls V: Skyrim").GameId,
                    CoverImage = "skyrim-dawnguard-cover.jpg"
                },
                // Fallout 4 DLC
                new DLC
                {
                    Name = "Far Harbor",
                    Description = "Fallout 4'ün en büyük DLC'si. Maine kıyılarındaki gizemli Far Harbor adasında yeni bir hikaye ve macera.",
                    Price = 24.99m,
                    ReleaseDate = new DateTime(2016, 5, 19),
                    GameId = context.Games.First(g => g.Title == "Fallout 4").GameId,
                    CoverImage = "fallout4-far-harbor-cover.jpg"
                },
                // Red Dead Redemption 2 DLC
                new DLC
                {
                    Name = "Red Dead Online: Naturalist",
                    Description = "Red Dead Redemption 2 Online için yeni bir kariyer. Doğa bilimci olarak hayvanları inceleyin ve koruyun.",
                    Price = 0.00m, // Ücretsiz DLC
                    ReleaseDate = new DateTime(2020, 7, 28),
                    GameId = context.Games.First(g => g.Title == "Red Dead Redemption 2").GameId,
                    CoverImage = "rdr2-naturalist-cover.jpg"
                },
                // Cyberpunk 2077 DLC
                new DLC
                {
                    Name = "Phantom Liberty",
                    Description = "Cyberpunk 2077'nin ilk ve tek genişletme paketi. Yeni bir bölge, hikaye ve karakterlerle Night City'ye geri dönün.",
                    Price = 29.99m,
                    ReleaseDate = new DateTime(2023, 9, 26),
                    GameId = context.Games.First(g => g.Title == "Cyberpunk 2077").GameId,
                    CoverImage = "cyberpunk-phantom-liberty-cover.jpg"
                },
                // Resident Evil 4 Remake DLC
                new DLC
                {
                    Name = "Separate Ways",
                    Description = "Resident Evil 4 Remake için Ada Wong'un hikayesini anlatan DLC. Leon'un görevini paralel olarak takip edin.",
                    Price = 9.99m,
                    ReleaseDate = new DateTime(2023, 9, 21),
                    GameId = context.Games.First(g => g.Title == "Resident Evil 4 Remake").GameId,
                    CoverImage = "re4-separate-ways-cover.jpg"
                },
                // Hades DLC
                new DLC
                {
                    Name = "Hades: The Blood Price",
                    Description = "Hades için yeni silahlar, yetenekler ve hikaye içeriği. Underworld'de yeni maceralar yaşayın.",
                    Price = 4.99m,
                    ReleaseDate = new DateTime(2021, 3, 19),
                    GameId = context.Games.First(g => g.Title == "Hades").GameId,
                    CoverImage = "hades-blood-price-cover.jpg"
                },
                // Stardew Valley DLC
                new DLC
                {
                    Name = "Ginger Island",
                    Description = "Stardew Valley için yeni bir ada bölgesi. Yeni mahsuller, hayvanlar ve gizemler keşfedin.",
                    Price = 7.99m,
                    ReleaseDate = new DateTime(2020, 11, 26),
                    GameId = context.Games.First(g => g.Title == "Stardew Valley").GameId,
                    CoverImage = "stardew-ginger-island-cover.jpg"
                },
                // Terraria DLC
                new DLC
                {
                    Name = "Journey's End",
                    Description = "Terraria'nın son büyük güncellemesi. Yeni boss'lar, silahlar, bloklar ve oynanış mekanikleri.",
                    Price = 0.00m, // Ücretsiz DLC
                    ReleaseDate = new DateTime(2020, 5, 16),
                    GameId = context.Games.First(g => g.Title == "Terraria").GameId,
                    CoverImage = "terraria-journeys-end-cover.jpg"
                }
            };

            context.DLCs.AddRange(dlcs);
            await context.SaveChangesAsync();

            Console.WriteLine($"Toplam {dlcs.Count} DLC eklendi!");

            Console.WriteLine("Seed data başarıyla tamamlandı!");
        }
    }
} 