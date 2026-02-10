# Entra Demo Prüfungsapp

Eine ASP.NET Core Razor Pages Webanwendung zur Demonstration von Microsoft Entra ID (ehemals Azure AD) mit rollenbasierter Zugriffskontrolle für ein Prüfungssystem.

## Technologie-Stack

- .NET 8.0
- ASP.NET Core Razor Pages
- Microsoft Identity Web
- Bootstrap 5 inklusive Bootstrap Icons


## Features

- **Authentifizierung**: Microsoft Entra ID Integration mit OpenID Connect
- **Rollenbasierte Autorisierung**: Drei Rollen (Participant, Examiner, Admin)
- **Prüfungssystem**:
  - Multiple-Choice-Fragen mit automatischer Bewertung
  - Freitext-Fragen mit manueller Bewertung durch Prüfer
  - Ergebnisübersicht für Teilnehmer
  - Bewertungsansicht für Prüfer mit Feedback-Funktion

## Projektstruktur

```
EntraPruefungsApp/
├── Pages/
│   ├── Admin/         # Admin-Bereich
│   │   └── Index.cshtml
│   ├── Auth/          # Login/Logout Seiten
│   │   ├── Login.cshtml
│   │   └── Logout.cshtml
│   ├── Exams/         # Prüfungsbezogene Seiten
│   │   ├── Index.cshtml        # Prüfungsübersicht
│   │   ├── Exam.cshtml         # Prüfung durchführen (Participant)
│   │   ├── ExamReview.cshtml   # Bewertung (Examiner)
│   │   └── Results.cshtml      # Ergebnisse (Participant)
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   └── _LoginPartial.cshtml
│   └── Index.cshtml   # Startseite
├── Models/
│   └── ExamModels.cs  # Datenmodelle (Exam, Question, ExamResult)
├── Services/
│   ├── ExamService.cs # In-Memory Datenspeicher
│   └── UserService.cs # User-Tracking
└── Program.cs         # App-Konfiguration
```

## Rollen

### Participant
- Prüfungen ablegen
- eigene Ergebnisse einsehen

### Examiner
- Prüfungen der Teilnehmer inklusive deren Antworten einsehen und bewerten

### Admin
- Admin-Bereich mit Übersicht über die User und deren Rollen

## Routing

- `/` - Startseite
- `/Exams/Index` - Prüfungsübersicht
- `/Exams/Exam?id={id}` - Prüfung durchführen
- `/Exams/Results` - Meine Ergebnisse
- `/Exams/ExamReview?id={id}` - Prüfung bewerten (Examiner)
- `/Admin/Index` - Admin-Bereich
- `/Auth/Login` - Anmeldung
- `/Auth/Logout` - Abmeldung

## Konfiguration

### Entra ID Setup

1. App-Registrierung in Microsoft Entra ID erstellen
2. Redirect URI konfigurieren: `https://localhost:5152/signin-oidc`
3. App-Rollen definieren:
   - `Participant` - Für Prüfungsteilnehmer
   - `Examiner` - Für Prüfer
   - `Admin` - Für Administratoren
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
- **Bewertung**: Multiple-Choice-Fragen werden beispielhaft bei richtiger Antwort automatisch mit 2 Punkten bewertet. Freitext-Fragen werden manuell durch den Prüfer bewertet (max. 3 Punkte).
- **Demo-Zweck**: Diese Anwendung dient ausschließlich zu Demonstrationszwecken
