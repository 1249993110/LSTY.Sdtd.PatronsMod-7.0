using HarmonyLib;
using LSTY.Sdtd.PatronsMod.SignalR;
using Microsoft.Owin.Hosting;

namespace LSTY.Sdtd.PatronsMod
{
    public class ModApi : IModApi
    {
        internal const string ModIdentity = "LSTY.Sdtd.PatronsMod";

        private static Harmony _harmony;
        private static bool _isGameStartDone;
        private static Mod _modInstance;
        private static IDisposable _webApp;

        public static Harmony Harmony => _harmony;
        public static bool IsGameStartDone => _isGameStartDone;
        public static SynchronizationContext MainThreadSyncContext { get; private set; }

        public static string ModDirectory => _modInstance.Path;
        public void InitMod(Mod modInstance)
        {
            try
            {
                _modInstance = modInstance;

                MainThreadSyncContext = SynchronizationContext.Current;

                StartupSignalR();

                PatchByHarmony();

                AllocsCaller.Initialize();

                RegisterModEventHandlers();
            }
            catch (Exception ex)
            {
                CustomLogger.Error(ex, "Initialize mod " + ModIdentity + " failed");
            }
        }

        private static void PatchByHarmony()
        {
            try
            {
                _harmony = new Harmony(ModIdentity);
                _harmony.PatchAll();

                CustomLogger.Info("Successfully patch all by harmony");
            }
            catch (Exception ex)
            {
                CustomLogger.Error(ex, "Patch by harmony failed");
                throw;
            }
        }

        private static void RegisterModEventHandlers()
        {
            try
            {
                Log.LogCallbacks += ModEventHook.LogCallback;
                ModEvents.GameAwake.RegisterHandler(ModEventHook.GameAwake);
                ModEvents.GameStartDone.RegisterHandler(() =>
                {
                    _isGameStartDone = true;
                    ModEventHook.GameStartDone();
                });
                ModEvents.GameShutdown.RegisterHandler(() =>
                {
                    ModEventHook.GameShutdown();
                    AllocsCaller.GameShutdown();
                    _webApp.Dispose();
                });
                ModEvents.PlayerSpawnedInWorld.RegisterHandler(ModEventHook.PlayerSpawnedInWorld);
                ModEvents.EntityKilled.RegisterHandler(ModEventHook.EntityKilled);
                ModEvents.PlayerDisconnected.RegisterHandler(ModEventHook.PlayerDisconnected);
                ModEvents.SavePlayerData.RegisterHandler(ModEventHook.SavePlayerData);
                ModEvents.ChatMessage.RegisterHandler(ModEventHook.ChatMessage);
                ModEvents.PlayerSpawning.RegisterHandler(ModEventHook.PlayerSpawning);
                ModEvents.CalcChunkColorsDone.RegisterHandler(AllocsCaller.CalcChunkColorsDone);

                CustomLogger.Info("Successfully registered mod event handlers");
            }
            catch (Exception ex)
            {
                CustomLogger.Error(ex, "Register mod event handlers failed");
                throw;
            }
        }

        private static void StartupSignalR()
        {
            try
            {
                string url = AppSettings.SignalRUrl;
                _webApp = WebApp.Start<SignalRStartup>(url);
                CustomLogger.Info("SignalR Server running on " + url);
            }
            catch (Exception ex)
            {
                CustomLogger.Error(ex, "Startup signalR server failed");
                throw;
            }
        }
    }
}