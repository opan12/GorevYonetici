## GörevYönetici Uygulaması

Bu proje, kullanıcıların görevlerini yönetmelerine yardımcı olan bir görev yönetim sistemidir. Kullanıcılar görevlerini oluşturabilir, güncelleyebilir, tamamlayabilir ve silebilir. Ayrıca JWT (JSON Web Token) kullanılarak kimlik doğrulama sağlanmıştır. Uygulama, Swagger entegrasyonu ile API belgelerine kolay erişim sunar.

# Özellikler

Görev Yönetimi: Görev ekleme, güncelleme, silme ve listeleme işlemleri.

Kimlik Doğrulama: JWT ile kullanıcı kimlik doğrulaması.

Rol Yönetimi: Kullanıcı rolleri (Admin, Kullanıcı) desteği.

Pagination: Görevlerin sayfa bazlı görüntülenmesi.

Swagger Desteği: API'yi test etmek ve belgelemek için Swagger UI.

# Teknolojiler

Backend: ASP.NET Core 8.0

Veritabanı: Microsoft SQL Server

ORM: Entity Framework Core

Authentication: JWT (JSON Web Token)

Dependency Injection: ASP.NET Core DI

Diğer: IMemoryCache, ChangeTracker
# Proje Mimarisi

Proje, Onion Architecture prensiplerine uygun olarak geliştirilmiştir:

Domain: Temel iş kuralları ve varlıklar.

Application: İş mantığı ve servis katmanı.

Infrastructure: Veritabanı erişimi ve dış bağımlılıklar.

Presentation: API Controller'lar
