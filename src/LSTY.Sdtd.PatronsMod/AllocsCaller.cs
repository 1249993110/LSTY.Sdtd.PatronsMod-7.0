using HarmonyLib;
using LSTY.Sdtd.PatronsMod.HarmonyPatchers;
using System.Reflection;

namespace LSTY.Sdtd.PatronsMod
{
    internal static class AllocsCaller
    {
        private static Action<Chunk> _renderSingleChunkDelegate;
        private static Action _renderFullMapDelegate;
        private static Action _shutdownDelegate;
        private static string _mapDirectory;
        private static Func<string, byte[]> _getMapTileDelegate;

        public static string MapDirectory => _mapDirectory;
        public static Func<string, byte[]> GetMapTileDelegate => _getMapTileDelegate;

        /// <summary>
        /// 必须在主线程进行初始化AllocsCaller
        /// </summary>
        public static void Initialize()
        {
            try
            {
                string serverFixesPath = Path.Combine(ModApi.ModDirectory, "Allocs/7dtd-server-fixes.dll");
                string mapRenderingPath = Path.Combine(ModApi.ModDirectory, "Allocs/MapRendering.dll");

                var serverFixesAssembly = Assembly.LoadFrom(serverFixesPath);
                var mapRenderingAssembly = Assembly.LoadFrom(mapRenderingPath);

                var mapRenderingType = mapRenderingAssembly.GetType("AllocsFixes.MapRendering.MapRendering");
                var mapRenderingIns = mapRenderingType.GetProperty("Instance").GetValue(null);

                _renderSingleChunkDelegate = (Action<Chunk>)mapRenderingType.GetMethod("RenderSingleChunk").CreateDelegate(typeof(Action<Chunk>));
                _renderFullMapDelegate = (Action)mapRenderingType.GetMethod("RenderFullMap").CreateDelegate(typeof(Action), mapRenderingIns);
                _shutdownDelegate = (Action)mapRenderingType.GetMethod("Shutdown").CreateDelegate(typeof(Action));

                // _mapDirectory = Path.Combine(GameIO.GetSaveGameDir(), "map");
                var constantsType = mapRenderingAssembly.GetType("AllocsFixes.MapRendering.Constants");
                _mapDirectory = constantsType.GetField("MAP_DIRECTORY").GetValue(null).ToString();

                var mapTileCacheIns = mapRenderingType.GetMethod("GetTileCache").Invoke(mapRenderingIns, null);
                var mapTileCacheType = serverFixesAssembly.GetType("AllocsFixes.FileCache.MapTileCache");
                _getMapTileDelegate = (Func<string, byte[]>)mapTileCacheType.GetMethod("GetFileContent").CreateDelegate(typeof(Func<string,byte[]>), mapTileCacheIns);

                CustomLogger.Info("Successfully initialize AllocsCallerd, map directory: " + _mapDirectory);
            }
            catch (Exception ex)
            {
                CustomLogger.Error(ex, "Initialize AllocsCaller failed");
            }
        }

        public static void GameShutdown()
        {
            _shutdownDelegate.Invoke();
        }

        public static void CalcChunkColorsDone(Chunk _chunk)
        {
            _renderSingleChunkDelegate.Invoke(_chunk);
        }

        public static void RenderFullMap()
        {
            _renderFullMapDelegate.Invoke();
        }
    }
}