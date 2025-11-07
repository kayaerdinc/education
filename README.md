# Tıp Eğitim Platformu

Modern tıp temalı bir çevrim içi eğitim platformu. Ön yüzde React, arka planda JWT tabanlı kimlik doğrulaması olan ASP.NET Core API kullanılacak.

## Özellik Özeti

- **İki rol**: Öğrenci ve eğitmen için ayrı paneller.
- **İçerik akışı**: Öğrenciler yüklenen videoları sırayla izler, video bitmeden ileri butonu pasif kalır.
- **Sınav modülü**: Videolar tamamlanınca sabit bir çoktan seçmeli sınav başlar. Sorular, şıklar ve cevap anahtarı veritabanından gelir.
- **Zorunlu seçim**: Her soru için dört şıktan biri seçilmeden sonraki soruya geçilemez.
- **Anket**: Sınav bitince üç alanlı (ör. beğendim/beğenmedim) değerlendirme anketi gösterilir.
- **Sonuçlar**: Öğrenciler doğru/yanlış sayılarını görür; eğitmenler öğrencilerin sınav ve anket sonuçlarını takip eder.
- **Kayıt yönetimi yok**: Öğrenciler ve eğitmenler veritabanında önceden tanımlanmış e-posta ve hash’lenmiş şifrelerle giriş yapar.

## Mimari

| Katman | Teknoloji | Açıklama |
| --- | --- | --- |
| İstemci | React + Vite + TypeScript | Role göre yönlendirme, video & sınav akışları, tıp temalı modern UI. |
| Stil | Tailwind CSS + Headless UI | Hızlı ve erişilebilir tasarım bileşenleri. |
| API | ASP.NET Core 8 Web API | JWT kimlik doğrulaması, sınav ve anket uç noktaları. |
| Veritabanı | PostgreSQL | Videolar, sorular, kullanıcılar ve sonuçlar için şema. |
| ORM | Entity Framework Core | Kod-öncelikli modelleme, veri tohumlama. |

## Veritabanı Şeması (Taslak)

- `Users (Id, Email, PasswordHash, FullName, Role)`
- `Videos (Id, Title, Description, Order, Url, DurationSeconds)`
- `ExamQuestions (Id, Order, Text)`
- `ExamOptions (Id, QuestionId, Order, Text, IsCorrect)`
- `ExamSessions (Id, StudentId, StartedAt, CompletedAt, Score)`
- `ExamAnswers (Id, SessionId, QuestionId, OptionId, IsCorrect)`
- `Surveys (Id, SessionId, Satisfaction, Comment, RecommendScore)`

> Not: Şifreler API içinde `BCrypt` veya `PBKDF2` ile hashlenerek tohumlanacak. Videolar ve sorular manuel olarak seed edilecek.

## API Taslağı

| Metot | Yol | Yetki | Açıklama |
| --- | --- | --- | --- |
| POST | `/api/auth/login` | Anonim | Email/şifre ile giriş, JWT döner. |
| GET | `/api/videos/next` | Öğrenci | Öğrencinin sıradaki videoyu getirir. |
| POST | `/api/videos/{id}/complete` | Öğrenci | Video izlendi bilgisini kaydeder. |
| GET | `/api/exam/questions/{order}` | Öğrenci | Sıradaki soruyu ve şıkları getirir. |
| POST | `/api/exam/questions/{id}/answer` | Öğrenci | Cevabı kaydeder, doğruluğu döner. |
| POST | `/api/exam/finish` | Öğrenci | Sınavı tamamlar, sonuçları hesaplar. |
| POST | `/api/surveys` | Öğrenci | Anket yanıtını kaydeder. |
| GET | `/api/instructor/dashboard` | Eğitmen | Öğrenci bazlı sınav ve anket özetlerini döner. |

JWT doğrulaması için refresh token kullanılmayacak; kısa süreli access token + auto logout planlanıyor.

## Frontend Akışı

1. **Giriş**: E-posta ve şifre ile oturum açılır; rol bazlı yönlendirme.
2. **Video Akışı**: Player (örn. `react-player`) kullanarak videolar sıralı gösterilir, süre tamamlandığında `İleri` aktif olur.
3. **Sınav**: Tek soru/ekran, Progress bar, şık seçimi zorunlu, otomatik kaydetme.
4. **Anket**: Beğendim/Beğenmedim + kısa yorum + tavsiye puanı.
5. **Özet**: Doğru/yanlış sayıları ve anket gönderim onayı.
6. **Eğitmen Paneli**: Öğrenci listesi, sınav skorları, anket sonuçları ve video ilerlemeleri.

## UI Kılavuzu

- Renkler: Pastel mavi & yeşil tonları, steril beyaz arka plan, tıp ikonografisi.
- Tipografi: `Inter` başlıklar, `Roboto` içerik.
- Kullanılabilirlik: Mobil uyum, tuş takımı ile gezilebilirlik, WCAG AA kontrastı.
- Bileşenler: Kart tabanlı yerleşim, stepper, mini dashboard kartları.

## Güvenlik

- JWT ile rol bazlı yetkilendirme (policy tabanlı).
- HTTPS zorunlu (deployment ortamında).
- Rate limiting ve temel audit logları.
- Şifreler hashing + salt ile saklanacak, plaintext tutulmayacak.

## Geliştirme Yol Haritası

1. Proje iskeletlerinin oluşturulması (`backend/`, `frontend/` klasörleri).
2. ASP.NET Core API’da kimlik doğrulama ve temel uç noktalar.
3. Entity Framework ile veritabanı şeması ve seed veriler.
4. React uygulamasında routing, layout ve UI bileşenleri.
5. Video, sınav ve anket akışlarının uçtan uca entegrasyonu.
6. Eğitmen paneli ve yönetim ekranları.
7. Otomatik testler ve son kullanıcı denemeleri.

## Sonraki Adımlar

- Backend ve frontend proje başlangıcı için ortam hazırlanacak.
- Seed verileri örnek içeriklerle doldurulacak.
- UI prototipi için temel renk/komponent paleti belirlenecek.
