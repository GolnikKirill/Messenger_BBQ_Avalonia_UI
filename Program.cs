using Avalonia;
using System;

namespace BBQ;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    public static int LaunchedPort{get; private set;}=8080;
    public static string LaunchedNickname{get; private set;}="LocalUser";
    [STAThread]
    public static void Main(string[] args)
    {
        int PortIdx=Array.IndexOf(args,"--port");
        if(PortIdx!=-1 && PortIdx + 1 < args.Length)
        {
            if(int.TryParse(args[PortIdx+1],out int customPort))
            {
                LaunchedPort=customPort;
            }
        }
        int UserNameIndx=Array.IndexOf(args,"--username");
        if(UserNameIndx!=-1 && UserNameIndx + 1 < args.Length)
        {
            LaunchedNickname=args[UserNameIndx+1];
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
}
