# 🎯 EventMap – MAUI + ASP.NET Core (Clean Architecture)

## 1. Projektziel
Eine **produktreife, modulare Event-Karten-App** (.NET 9 MAUI Android/iOS) mit **ASP.NET Core Web API**, **PostGIS** und **SignalR**.  
Die App zeigt **Events** (Konzerte, Straßenmusik, Clubs) als **Pins** auf einer **MapLibre-OSM-Karte**.  
Tippt der Nutzer einen Pin an, öffnet sich ein **linkes Slide-In Info-Panel** mit Eventdetails.  
Events werden in **Echtzeit** aktualisiert, können gefiltert, favorisiert und als Benachrichtigung abonniert werden.

---

## 2. Arbeitsanweisung für GitHub Copilot / Copilot Workspace

### Modellvorgabe
- **Verwende das leistungsstärkste verfügbare Modell** (bevorzugt `GPT-5` oder gleichwertig).  
- Falls nicht verfügbar, automatisch bestes verfügbares Modell wählen.

### Arbeitsprinzip
Copilot muss dieses Projekt **vollständig umsetzen** – vom Skeleton bis zu fertigen Features – **in wiederholbaren, getesteten Schritten**.

#### 2.1 Analyse & Planung
1. Lese `docs/ISSUE_MASTER_PROMPT.md` vollständig.  
2. Analysiere die Projektziele, Architektur und Technologien.  
3. Erstelle einen **Implementierungsplan** mit klarer Feature-Reihenfolge.  
4. Lege für jeden Hauptschritt einen **Branch** `feature/<name>` an.

#### 2.2 Iterative Umsetzung
- Implementiere Features **schrittweise**:
  1. **MVP Skeleton**: Solution + Projektstruktur + Basis-MapPage (Dummy-Pins) + API `/events?bbox=…`
  2. SignalR Hub `/hubs/events` für Echtzeit-Updates
  3. EF Core + PostGIS-Migrationen
  4. Tests (Domain, API, UI)
  5. CI/CD (GitHub Actions)
- **Nach jedem Feature**:
  - Tests lokal ausführen (`dotnet test`)  
  - Fehler **selbstständig beheben**, bis alles fehlerfrei läuft.

#### 2.3 Debugging & Self-Check
- Logs lesen und Fehler analysieren.
- API-Endpunkte mit Beispiel-Requests testen (Swagger/OpenAPI).
- Map-Funktion im Emulator überprüfen.
- Erst PR erstellen, wenn:
  - Alle Tests grün
  - CI-Workflow erfolgreich

#### 2.4 PR-Workflow
- Branch: `feature/<feature-name>`
- PR-Beschreibung:
  - Übersicht der umgesetzten Features
  - Start-/Test-Anleitung
  - Bekannte offene Punkte
  - Nächste Schritte
- Merge erst nach erfolgreichem Review & CI.

#### 2.5 Dokumentation
- README.md & ARCHITECTURE.md **laufend aktualisieren**.
- Technische Entscheidungen dokumentieren (z. B. Wahl MapLibre, SignalR, PostGIS).
- Setup-Anleitungen in README ergänzen.

---

## 3. Architektur & Technologien

### 3.1 Projektstruktur (Clean Architecture)
```
src/
  Client.Maui/           → MAUI Mobile App (MVVM, MapLibre)
  Server.Api/            → ASP.NET Core API (Swagger, SignalR, Controllers)
  Server.Infrastructure/ → EF Core + PostGIS
  Server.Domain/         → Entitäten, ValueObjects
  Shared/                → Contracts, Enums, Results
tests/
  Domain.Tests/          → Unit-Tests Domain
  Api.IntegrationTests/  → Integrationstests API
  Client.UITests/        → MAUI UI-Tests
docs/
  ISSUE_MASTER_PROMPT.md → Master-Prompt für Copilot
```

### 3.2 Kerntechnologien
- **Frontend (Mobile)**: .NET 9 MAUI, MVVM (CommunityToolkit.Mvvm), MapLibre GL JS (im WebView), SignalR-Client.
- **Backend**: ASP.NET Core 9, EF Core 9 (Npgsql + NetTopologySuite), PostGIS (GiST-Indexe), SignalR.
- **API-Design**: REST + SignalR, Swagger/OpenAPI.
- **Auth**: JWT (Access + Refresh), Social OAuth optional später.
- **CI/CD**: GitHub Actions (Build, Test, PostGIS-Dienst).

---

## 4. Umsetzungsschritte (Backlog)

### Iteration 1 – MVP Skeleton
- Lösung + Projekte anlegen
- MAUI MapPage mit Dummy-Pins
- API `/events?bbox=…` (Mock-Daten)
- CI/CD Workflow (Build + Test)

### Iteration 2 – PostGIS & EF Core
- PostgreSQL + PostGIS Docker-Compose
- EF Core + NetTopologySuite konfigurieren
- Migrationen (Events, Venues)
- BBox-Query implementieren

### Iteration 3 – SignalR Echtzeit
- SignalR Hub `/hubs/events`
- Client-MAUI SignalR-Service
- Realtime-Update beim Erstellen/Ändern/Löschen von Events

### Iteration 4 – Tests & Fehlerbehandlung
- Domain-Tests (Validation)
- API-Integrationstests (BBox-Query)
- MAUI-UI-Tests (Pin → Panel)

### Iteration 5 – Offlinefähigkeit
- SQLite-Cache
- Stale-While-Revalidate Strategie

---

## 5. Definition of Done (MVP)
- Karte zeigt Events aus API für aktuellen Viewport
- Pin-Tap → Slide-In Panel mit Details
- `/events?bbox=…` performant via GiST
- SignalR liefert Updates in Echtzeit
- Tests lokal + in CI erfolgreich
- CI/CD ohne Fehler
- Dokumentation aktuell

---

## 6. Lokale Entwicklung

### Setup Backend
```bash
cd src/Server.Api
dotnet restore
dotnet ef database update
dotnet run
```

### Setup Mobile
```bash
cd src/Client.Maui
dotnet restore
dotnet build
dotnet maui run -t Android
```

### Docker PostGIS starten
```bash
docker run --name eventmap-db -e POSTGRES_PASSWORD=devpass -p 5432:5432 postgis/postgis
```

---

## 7. CI/CD Workflow
- **Build** aller Projekte
- **Unit- & Integrationstests**
- PostGIS-Service in GitHub Actions starten
- Artefakte für Mobile & API erstellen

---

## 8. Copilot Hinweise
- Implementiere **kleine, abgeschlossene Features** pro PR.
- Prüfe und dokumentiere **jede Architekturentscheidung**.
- Schreibe **aussagekräftige Commit-Messages**.
- Halte dich an **.editorconfig** & Code-Stil.
- **Kein Merge**, wenn CI rot ist oder Tests fehlen.
- Nutze **Selbst-Review** vor PR-Erstellung.

---

## 9. Ressourcen
- [MapLibre GL JS Docs](https://maplibre.org/maplibre-gl-js-docs/api/)
- [PostGIS Docs](https://postgis.net/documentation/)
- [SignalR ASP.NET Core](https://learn.microsoft.com/aspnet/core/signalr)
- [EF Core + NetTopologySuite](https://learn.microsoft.com/ef/core/modeling/spatial)
