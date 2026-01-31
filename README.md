# Entra Demo Prüfungsapp

Eine ASP.NET Core Razor Pages Webanwendung zur Demonstration von Microsoft Entra ID (ehemals Azure AD) mit rollenbasierter Zugriffskontrolle für ein Prüfungssystem.

## Features

- **Authentifizierung**: Microsoft Entra ID Integration mit OpenID Connect
- **Rollenbasierte Autorisierung**: Zwei Rollen (User, Examiner)
- **Prüfungssystem**:
  - Multiple-Choice-Fragen mit automatischer Bewertung
  - Freitext-Fragen mit manueller Bewertung durch Prüfer
  - Ergebnisübersicht für Teilnehmer
  - Bewertungsansicht für Prüfer mit Feedback-Funktion

## Technologie-Stack

- .NET 8.0
- ASP.NET Core Razor Pages
- Microsoft Identity Web
- Bootstrap 5 inklusive Bootstrap Icons

## Projektstruktur

```
EntraPruefungsApp/
├── Features/          # Feature-basierte Organisation
│   ├── Home/         # Startseite
│   ├── Auth/         # Login/Logout
│   ├── Exams/        # Prüfungsfunktionalität
│   │   ├── Index.cshtml      # Prüfungsübersicht
│   │   ├── Take.cshtml       # Prüfung durchführen (Participant)
│   │   ├── Review.cshtml     # Bewertung (Examiner)
│   │   └── MyResults.cshtml  # Ergebnisse (Participant)
│   └── Admin/        # Admin-Panel
├── Models/
│   └── ExamModels.cs # Datenmodelle
├── Services/
│   ├── ExamService.cs   # Prüfungsverwaltung
│   └── UserService.cs   # Benutzerverwaltung
├── Pages/
│   └── Shared/       # Gemeinsame Layouts
└── Program.cs        # App-Konfiguration
```

## Rollen

### User
- Prüfungen ablegen
- eigene Ergebnisse einsehen

### Examiner
- Prüfungen der User inklusive deren Antworten einsehen und bewerten

## Routing

- `/` - Startseite
- `/exams` - Prüfungsübersicht
- `/exam/{id}` - Prüfung durchführen
- `/myresults` - Meine Ergebnisse
- `/examreview/{id}` - Prüfung bewerten (Examiner)

## Konfiguration

### Entra ID Setup

1. App-Registrierung in Microsoft Entra ID erstellen
2. Redirect URI konfigurieren: `https://localhost:5152/signin-oidc`
3. App-Rollen definieren:
   - `User` - Für Prüfungsteilnehmer
   - `Examiner` - Für Prüfer
4. `appsettings.json` anpassen:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<your-tenant-id>",
    "ClientId": "<your-client-id>",
    "CallbackPath": "/signin-oidc"
  }
}
```

## Installation & Start

```bash
# Repository klonen
git clone <repository-url>
cd entra-demo-pruefungsapp/EntraPruefungsApp

# Abhängigkeiten wiederherstellen
dotnet restore

# Anwendung starten
dotnet run
```

Anwendung läuft unter: `https://localhost:5152`

## Hinweise

- **Datenspeicherung**: Alle Prüfungsergebnisse werden In-Memory gespeichert und gehen beim Neustart verloren
- **Bewertung**: Multiple-Choice-Fragen werden beispielhaft bei richtiger Antwort automatisch mit 2 Punkten bewertet. Freitext-Fragen werden manuell durch den Prüfer bewertet.
- **Demo-Zweck**: Diese Anwendung dient ausschließlich zu Demonstrationszwecken
