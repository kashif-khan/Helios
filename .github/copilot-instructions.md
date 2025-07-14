<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

# Helios - Screen Keep-Alive Application

This is a .NET 8.0 console application that prevents the screen from going to sleep using kernel32.dll calls.

## Key Features:
- Uses SetThreadExecutionState from kernel32.dll to keep the screen active
- Console-based interface with system tray integration
- Uses Windows Forms NotifyIcon for system tray presence
- Provides console commands and system tray menu for user interaction
- Handles proper cleanup of resources

## Development Guidelines

- Follow console application best practices
- Use P/Invoke for Windows API calls
- Implement proper resource disposal
- Handle system tray interactions using Windows Forms components
- Ensure the application can run in the background without user intervention
- Use thread-safe operations for console and system tray interactions

## Architecture

- Main program (`Program.cs`) handles the console interface and system tray integration
- Uses NotifyIcon from Windows Forms for system tray presence
- Implements console window show/hide functionality using Win32 API
- Uses Windows API calls for power management
- Runs message pump for system tray while maintaining console functionality
