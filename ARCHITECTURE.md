# 🏗 EventMap – Architektur-Übersicht

## 1. Überblick
EventMap ist eine **produktreife Event-Karten-App**, bestehend aus:
- **Mobile App (Frontend)**: .NET 9 MAUI (Android/iOS) – MapLibre GL (OSM) Integration.
- **Backend (API)**: ASP.NET Core 9 Web API mit EF Core + PostGIS.
- **Echtzeit-Kommunikation**: SignalR für Event-Updates.
- **Datenbank**: PostgreSQL 16 + PostGIS (räumliche Abfragen).

Ziel ist eine **Clean Architecture** mit klarer Trennung von UI, Anwendungslogik, Domain und Infrastruktur.

---

## 2. Architektur-Übersicht

```plaintext
+----------------------------------------------------------+
|                      Mobile App (MAUI)                   |
|  - MVVM (CommunityToolkit.Mvvm)                          |
|  - MapLibre GL (OSM)                                     |
|  - SignalR Client                                        |
|  - SQLite Cache (Offline)                                |
+------------------------↑---------------------------------+
                         | HTTP (REST) / WebSocket (SignalR)
+------------------------↓---------------------------------+
|                ASP.NET Core Web API                      |
|  - Controllers (REST)                                    |
|  - SignalR Hub (/hubs/events)                            |
|  - Application Layer (Use Cases, DTOs, CQRS)             |
|  - Domain Layer (Entities, ValueObjects)                 |
|  - Infrastructure Layer (EF Core, PostGIS, Blob Storage) |
+------------------------↓---------------------------------+
                         | Npgsql + NetTopologySuite
+------------------------↓---------------------------------+
|          PostgreSQL 16 + PostGIS (Geo-Indexe)             |
+----------------------------------------------------------+
```

---

## 3. Clean Architecture Layer

### 3.1 **Client.Maui** (UI Layer)
- **MVVM-Pattern** mit CommunityToolkit.Mvvm.
- **MapLibre GL** im WebView für interaktive Karten.
- **SignalR-Client** für Echtzeit-Updates.
- **SQLite** für Offlinefähigkeit.
- **Dependency Injection** für Services (ApiClient, MapService, AuthService).
- **Navigation** über MAUI Shell.

**Wichtige Komponenten:**
- `MapPage.xaml` – Kartenanzeige, Filter, Pin-Interaktion.
- `EventInfoPanel.xaml` – Linkes Slide-In-Panel mit Eventdetails.
- `ViewModels/MapPageViewModel.cs` – Bindings für Karte, Filter, Auswahl.
- `Services/ApiClient.cs` – REST-API-Aufrufe.
- `Services/SignalRService.cs` – Echtzeit-Updates.

---

### 3.2 **Server.Api** (Presentation Layer)
- **Controllers** (REST-API) → Validierung + Aufruf von Application-Services.
- **SignalR Hub** für Broadcast von Event-Änderungen.
- **Swagger** zur API-Dokumentation.
- **JWT-Authentifizierung** (Access + Refresh Token).
- **Role-based Authorization** (User, Artist, Venue, Admin).

---

### 3.3 **Application Layer**
- Enthält **Use Cases** als Services (z. B. `CreateEventService`).
- **DTOs** (Request/Response-Modelle).
- **Mapping** (AutoMapper).
- **Validierungen** (FluentValidation).
- Keine direkten Abhängigkeiten von EF Core oder anderen Infrastrukturkomponenten.

---

### 3.4 **Domain Layer**
- **Entities**: `User`, `Event`, `Venue`, `Rating`, `Favorite`.
- **Value Objects**: `Location`, `RecurrenceRule`.
- **Business Rules** direkt hier implementiert.
- Keine Abhängigkeiten zu Infrastruktur oder Frameworks.

---

### 3.5 **Infrastructure Layer**
- **Datenbankzugriff** mit EF Core + NetTopologySuite für Geodaten.
- **PostGIS**-Integration für räumliche Abfragen.
- **Repositories** für persistente Speicherung.
- **Azure Blob Storage** für Medienuploads.
- **Migrations** (EF Core).

---

## 4. API-Design

### Beispiel-Endpoints

| Endpoint | Methode | Auth | Beschreibung |
|----------|--------|------|--------------|
| `/events` | GET | Public | Events im aktuellen Viewport (BBox) |
| `/events/{id}` | GET | Public | Eventdetails |
| `/events` | POST | Artist/Venue/Admin | Neues Event erstellen |
| `/venues` | GET | Public | Venue-Liste |
| `/venues/{id}` | GET | Public | Venue-Details inkl. Programm |
| `/auth/register` | POST | Public | Benutzerregistrierung |
| `/auth/login` | POST | Public | Login & Token erhalten |
| `/hubs/events` | WS | Auth | Echtzeit-Event-Updates |

---

## 5. Datenbank-Design

### Tabellenübersicht
- **Users** (`Id`, `Email`, `PasswordHash`, `DisplayName`, `Role`, …)
- **Artists** (`Id`, `UserId`, `Bio`, …)
- **Venues** (`Id`, `Name`, `Location`, `Address`, …) → Geo-Index
- **Events** (`Id`, `Title`, `Genres[]`, `StartUtc`, `EndUtc`, `Location`, `VenueId?`, …) → Geo-Index
- **Ratings**, **Favorites**, **NotificationSubscriptions**, **Media**

**Geo-Index-Beispiel (PostGIS):**
```sql
CREATE INDEX idx_events_location ON "Events"
USING GIST ("Location");
```

---

## 6. Echtzeit-Architektur

- **SignalR** Hub `/hubs/events`:
  - `events.created`
  - `events.updated`
  - `events.deleted`
- MAUI-Client abonniert diese Channels.
- Updates → UI-Refresh ohne API-Reload.

---

## 7. Offline & Caching

- **SQLite** speichert zuletzt geladene Events + Favoriten.
- **Stale-While-Revalidate**:
  1. Zeige gecachte Daten sofort.
  2. Lade neue Daten im Hintergrund.

---

## 8. Tests & Qualitätssicherung

### Testarten:
- **Domain-Tests**: Validierung & Business Rules.
- **API-Integrationstests**: REST- & SignalR-Endpoints.
- **MAUI-UI-Tests**: Nutzerflows (Pin → Panel).

---

## 9. Entwicklungsrichtlinien
- Code-Stil: `.editorconfig` einhalten.
- PR nur mergen, wenn **CI grün**.
- Commit-Messages: `feat:`, `fix:`, `refactor:` Prefix.
- Jeder PR **muss** Tests enthalten.

---

## 10. Erweiterungsplan (nach MVP)
- **Push Notifications** (Firebase Cloud Messaging).
- **Social Login** (Apple/Google).
- **Recurrence Rules** (wiederkehrende Events).
- **Multi-City-Support** mit Suchfunktion.
- **Moderation** (Admin kann Events freigeben).

---
