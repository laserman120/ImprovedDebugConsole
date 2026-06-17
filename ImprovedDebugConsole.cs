using ImprovedDebugConsole.Managers;
using RedLoader;
using SonsSdk;


namespace ImprovedDebugConsole;

public class ImprovedDebugConsole : SonsMod
{
    public ImprovedDebugConsole()
    {
        HarmonyPatchAll = true;
    }

    protected override void OnInitializeMod()
    {
        Config.Init();

        AdvancedConsoleEngine.InitializeRegistry();
    }

    protected override void OnSdkInitialized()
    {
    }

    protected override void OnGameStart()
    {
        AdvancedConsoleEngine.IndexGameDatabases();
    }

    public void RegisterAdvancedCommand(string commandName, string description, string[] args, Dictionary<string, string[]> customLists, bool logging)
    {
        bool success = AdvancedConsoleEngine.RegisterExternalCommand(commandName, description, args, customLists, logging);
        if (success && logging)
        {
            RLog.Msg($"[ImprovedDebugConsole] Successfully registered external command: {commandName}");
        }
    }
}