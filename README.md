# Helios - Screen Keep-Alive Application

Helios is a lightweight .NET 8.0 console application that prevents your screen from going to sleep and keeps your system active. The application runs as a console app with system tray integration and uses Windows API calls to maintain system activity.

## Table of Contents

- [Features](#features)
- [How It Works](#how-it-works)
- [Usage](#usage)
- [Building from Source](#building-from-source)
- [Icon Generation](#icon-generation)
- [Technical Details](#technical-details)
- [System Requirements](#system-requirements)
- [Architecture](#architecture)
- [License](#license)
- [Contributing](#contributing)

## Features

- **Screen Keep-Alive**: Prevents the screen from turning off and the system from going to sleep
- **Console Interface**: Simple console-based interface with real-time feedback
- **System Tray Integration**: Runs quietly in the notification area with right-click menu
- **Custom Icon**: Features a custom sun icon representing the Greek god Helios
- **Minimal Resource Usage**: Lightweight application with low system impact
- **Flexible Control**: Console commands and system tray options for control

## How It Works

Helios uses the Windows API function `SetThreadExecutionState` from `kernel32.dll` to inform the system that it should remain active. This prevents the system from entering sleep mode and keeps the display active.

## Usage

1. **Launch the Application**: Run `Helios.exe` or `dotnet run`
2. **Console Interface**: The application will show startup messages and status
3. **System Tray**: Look for the Helios icon in your notification area
4. **Console Commands**:
   - Press 'q' and Enter to quit the application
   - Minimize the console window to run in background
5. **System Tray Options**: Right-click the tray icon for:
   - Show Console: Bring the console window to front
   - Hide Console: Hide the console window
   - Exit: Completely close the application

## Building from Source

### Prerequisites

- .NET 8.0 SDK or later
- Windows operating system

### Build Instructions

```bash
# Clone the repository
git clone <repository-url>
cd Helios

# Restore packages and build
dotnet restore
dotnet build

# Run the application
dotnet run
```

### Build Release Version

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## Icon Generation

The application uses a custom multi-resolution icon file (`helios.ico`) that is generated from a source PNG image. The icon includes multiple sizes for optimal display across different contexts (taskbar, system tray, file explorer, etc.).

### Prerequisites for Icon Generation

- **ImageMagick**: Download and install from [https://imagemagick.org/script/download.php](https://imagemagick.org/script/download.php)
- **Source PNG**: Place your high-resolution PNG file in `assets/icons/helios-logo.png`

### Generate Icon Command

To create the highest quality multi-resolution icon file, run the following command from the project root:

```bash
magick assets/icons/helios-logo.png -define icon:auto-resize=256,128,64,48,32,16 -quality 100 -filter Lanczos -sampling-factor 1x1 helios.ico
```

### Command Explanation

- **`assets/icons/helios-logo.png`**: Source PNG file (should be high resolution, ideally 256x256 or larger)
- **`-define icon:auto-resize=256,128,64,48,32,16`**: Creates multiple icon sizes in one ICO file
- **`-quality 100`**: Maximum quality setting for best visual result
- **`-filter Lanczos`**: High-quality resampling filter for scaling down
- **`-sampling-factor 1x1`**: Prevents chroma subsampling for better color quality
- **`helios.ico`**: Output file that will be embedded in the executable

The generated icon file will contain the following resolutions:

- 256×256 (high DPI displays)
- 128×128 (large icons)
- 64×64 (medium icons)
- 48×48 (standard desktop icons)
- 32×32 (small icons)
- 16×16 (system tray and small UI elements)

## Technical Details

- **Framework**: .NET 8.0
- **Application Type**: Console Application with Windows Forms components for system tray
- **Platform**: Windows only (uses Windows-specific API calls)
- **API Calls**: Uses `SetThreadExecutionState` from `kernel32.dll`
- **System Tray**: Uses Windows Forms `NotifyIcon` for system tray integration
- **Icon**: Custom sun icon embedded in executable and used in system tray

## System Requirements

- Windows 10 or later
- .NET 8.0 Runtime (if not using self-contained deployment)

## Architecture

- **Main Program**: Console application entry point with system tray setup
- **Screen Keep-Alive**: Uses Windows API calls for power management
- **System Tray**: Windows Forms NotifyIcon for background operation
- **Console Management**: Win32 API calls for console window show/hide

## License

This project is open source and available under the MIT License.

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.
