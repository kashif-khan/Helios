using System.Runtime.InteropServices;

namespace Helios;

class Program
{
    private static NotifyIcon? notifyIcon;
    private static bool isExiting = false;

    // P/Invoke declarations for kernel32.dll
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern uint SetThreadExecutionState(uint esFlags);

    // Execution state flags
    private const uint ES_CONTINUOUS = 0x80000000;
    private const uint ES_SYSTEM_REQUIRED = 0x00000001;
    private const uint ES_DISPLAY_REQUIRED = 0x00000002;

    [STAThread]
    static void Main()
    {
        Console.WriteLine("Helios - Screen Keep-Alive Application");
        Console.WriteLine("======================================");
        Console.WriteLine("Starting system tray application...");
        
        // Initialize Windows Forms application for system tray
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        // Start keeping the screen active
        StartScreenKeepAlive();
        Console.WriteLine("✓ Screen keep-alive activated");
        
        // Initialize system tray
        InitializeNotifyIcon();
        Console.WriteLine("✓ System tray icon created");
        
        Console.WriteLine("\nHelios is now running in the background.");
        Console.WriteLine("Look for the Helios icon in your system tray.");
        Console.WriteLine("Right-click the tray icon for options.");
        Console.WriteLine("\nPress 'q' and Enter to quit, or minimize this window...");
        
        // Hide the console window after initialization
        HideConsole();
        
        // Handle console input in a separate thread
        var inputThread = new Thread(HandleConsoleInput) { IsBackground = true };
        inputThread.Start();
        
        // Run the Windows Forms message loop for system tray
        Application.Run();
    }

    private static void HandleConsoleInput()
    {
        while (!isExiting)
        {
            try
            {
                var key = Console.ReadKey(true);
                if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                {
                    Console.WriteLine("\nShutting down Helios...");
                    ExitApplication();
                    break;
                }
            }
            catch (InvalidOperationException)
            {
                // Console input is not available (e.g., running as a Windows service)
                break;
            }
        }
    }

    private static void InitializeNotifyIcon()
    {
        notifyIcon = new NotifyIcon();
        
        // Try to load the custom icon, fallback to system icon if not found
        try
        {
            // Get the path to the icon file relative to the application's directory
            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var iconPath = Path.Combine(applicationDirectory, "helios.ico");
            
            if (File.Exists(iconPath))
            {
                notifyIcon.Icon = new Icon(iconPath);
            }
            else
            {
                // Fallback to system icon if custom icon not found
                notifyIcon.Icon = SystemIcons.Application;
            }
        }
        catch
        {
            notifyIcon.Icon = SystemIcons.Application;
        }
        
        notifyIcon.Text = "Helios - Screen Keep-Alive Active";
        notifyIcon.Visible = true;

        // Create context menu
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Show Console", null, (s, e) => ShowConsole());
        contextMenu.Items.Add("Hide Console", null, (s, e) => HideConsole());
        contextMenu.Items.Add("-");
        contextMenu.Items.Add("Exit", null, (s, e) => ExitApplication());
        
        notifyIcon.ContextMenuStrip = contextMenu;
        notifyIcon.DoubleClick += (s, e) => ShowConsole();
    }

    private static void StartScreenKeepAlive()
    {
        // Prevent system from going to sleep and keep display active
        SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
    }

    private static void StopScreenKeepAlive()
    {
        // Allow system to go to sleep normally
        SetThreadExecutionState(ES_CONTINUOUS);
    }

    private static void ShowConsole()
    {
        var handle = GetConsoleWindow();
        if (handle != IntPtr.Zero)
        {
            ShowWindow(handle, SW_SHOW);
            SetForegroundWindow(handle);
        }
    }

    private static void HideConsole()
    {
        var handle = GetConsoleWindow();
        if (handle != IntPtr.Zero)
        {
            ShowWindow(handle, SW_HIDE);
        }
    }

    private static void ExitApplication()
    {
        if (isExiting) return;
        
        isExiting = true;
        Console.WriteLine("Stopping screen keep-alive...");
        StopScreenKeepAlive();
        
        if (notifyIcon != null)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }
        
        Console.WriteLine("Helios has been stopped.");
        Application.Exit();
        Environment.Exit(0);
    }

    // Windows API functions for console window manipulation
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;
}