# 🗺️ EventMap – Master Issue für Copilot Workspace

**Labels:** `epic`, `backend`, `mobile`  
**Project:** `EventMap Development`  

---

## 1. Ziel dieses Issues  
Dieses Issue dient als **zentraler Steuerbefehl** für GitHub Copilot Workspace.  
Es beschreibt **komplett**, wie das Projekt umgesetzt werden soll und wie die Arbeitsschritte organisiert sind.  
Copilot soll daraus **Epics, Features und Subtasks** ableiten, die automatisch ins **Project Board** (`EventMap Development`) wandern und mit den passenden **Labels** versehen werden.

---

## 2. Projektziel (Kurzfassung)  
Eine **produktreife Event-Karten-App** in **.NET 9 MAUI** (Android/iOS) + **ASP.NET Core Web API** mit **PostGIS** und **SignalR**.  
Kartenansicht mit **MapLibre GL (OSM)**, Events als **Pins**, links Slide-In-Panel mit Eventdetails, Echtzeit-Updates, Filter, Favoriten, Push-Benachrichtigungen.

---

## 3. Copilot Arbeitsanweisung

### 3.1 Allgemeine Regeln
1. Verwende **das leistungsstärkste verfügbare Modell** (bevorzugt GPT-5).  
2. Arbeite **inkrementell**: große Features zuerst als **Epic**, in kleine **Subtasks** zerlegen (max. 2–4 Stunden Aufwand pro Subtask).  
3. **Jeder Subtask** bekommt:
   - Aussagekräftigen Titel
   - Passende Labels (z. B. `feature`, `backend`, `mobile`, `good-first-subtasks`)
   - Zugehörigkeit zum Project Board
4. **Jeder PR** muss:
   - Alle Tests bestehen (`dotnet test`)
   - Erfolgreich durch CI/CD laufen
   - Eine kurze **Testanleitung** enthalten
5. Fehler während der Arbeit **selbstständig beheben**, bevor der PR erstellt wird.

---

### 3.2 Iterative Umsetzung
Copilot soll folgende Hauptschritte (Epics) umsetzen:

#### Epic 1 – MVP Skeleton  
- Solution + Projektstruktur anlegen (Clean Architecture)  
- MAUI MapPage mit Dummy-Pins  
- API `/events?bbox=…` (Mock-Daten)  
- CI/CD Workflow (Build + Test)  

#### Epic 2 – PostGIS & EF Core Integration  
- PostgreSQL + PostGIS (Docker Compose)  
- EF Core + NetTopologySuite konfigurieren  
- Migrationen (Events, Venues)  
- BBox-Query implementieren  

#### Epic 3 – SignalR Echtzeit-Updates  
- SignalR Hub `/hubs/events`  
- MAUI-Client: Echtzeit-Service implementieren  
- Live-Update bei Create/Update/Delete  

#### Epic 4 – Tests & Fehlerbehandlung  
- Unit-Tests (Domain)  
- Integrationstests (API)  
- UI-Tests (MAUI)  

#### Epic 5 – Offlinefähigkeit  
- SQLite-Cache  
- Stale-While-Revalidate-Strategie  

---

### 3.3 Technische Eckpunkte
- **Frontend (Mobile)**: .NET 9 MAUI, MVVM (CommunityToolkit.Mvvm), MapLibre GL (WebView), SignalR-Client  
- **Backend**: ASP.NET Core 9, EF Core 9, PostGIS, SignalR  
- **API**: REST + SignalR, Swagger/OpenAPI  
- **Auth**: JWT (Access + Refresh), später optional Social OAuth  
- **CI/CD**: GitHub Actions mit Build, Test, PostGIS-Service  

---

## 4. Projektorganisation in GitHub

### Labels
- `epic` → Große Features/Meilensteine  
- `feature` → Einzelne Funktionen  
- `backend` → Backend-Implementierung  
- `mobile` → Mobile App Features  
- `good-first-subtasks` → Kleine, klar abgegrenzte Aufgaben  

### Project Board
- `Backlog` → noch nicht gestartet  
- `In Progress` → in Bearbeitung  
- `In Review` → PR offen, Review läuft  
- `Done` → gemerged & abgeschlossen  

---

## 5. Definition of Done (für jedes Feature)
- Funktion laut Anforderung implementiert  
- Tests bestehen lokal + in CI  
- Code-Stil entspricht `.editorconfig`  
- README und ARCHITECTURE.md bei Bedarf aktualisiert  
- Keine offenen TODOs oder temporären Hacks  

---

## 6. Nächste Schritte (direkt durch Copilot umsetzen)
1. **Epic 1 – MVP Skeleton** starten  
2. Alle Subtasks im Project Board erstellen  
3. Branch `feature/mvp-skeleton` anlegen  
4. PR eröffnen, Review, Merge  
