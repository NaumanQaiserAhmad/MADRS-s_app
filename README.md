# MADRSApp

**MADRSApp** is a mobile application built with **.NET MAUI** and **.NET 9.0**. It enables users to complete a self-assessment using the Montgomery-Åsberg Depression Rating Scale (MADRS). The app supports both iOS and Android platforms, offering an interactive and user-friendly interface for mental health evaluation.

---

## Features

- **Dynamic Questionnaire**: Questions are loaded dynamically from a backend API.
- **Option Selection**: option selection for a seamless experience.
- **Progress Tracking**: Displays progress throughout the questionnaire.
- **Result Calculation**: Results with severity levels are calculated and displayed.
- **Cross-Platform**: Built with .NET MAUI for iOS and Android compatibility.

---

## Technology Stack

- **Frontend**: .NET MAUI
- **Backend Integration**: REST API
- **Programming Languages**: C#
- **Design Pattern**: MVVM (Model-View-ViewModel)

---

## Prerequisites

1. **.NET 9.0** installed on your machine.
2. **Android/iOS emulator** or a physical device for testing.
3. **Visual Studio Code**

---

## Getting Started

### Clone the Repository

git clone https://github.com/your-repository/madrsapp.git
cd madrsapp


### dotnet build
dotnet run --framework net9.0-android  # For Android
dotnet run --framework net9.0-ios     # For iOS


### Project Structure
MADRSApp/
├── Models/          # Data models (Question, AnswerOption, Result)
├── Services/        # Service layer for API interactions
├── ViewModels/      # ViewModels implementing business logic
├── Views/           # XAML pages for UI
├── Converters/      # Custom value converters
├── Resources/       # Fonts, images, and raw assets
├── App.xaml         # Application-wide resource dictionary
├── launch.json      # Debug configuration for VS Code
└── README.md        # Project documentation
