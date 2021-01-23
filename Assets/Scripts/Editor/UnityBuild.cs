using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SailorStrike
{
    public class UnityBuild
    {
        private static string[] ActiveScenes => EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

        private static string ProductName => PlayerSettings.productName;

        [MenuItem("Build/WebGL")]
        public static void WebGL()
            => Build(BuildTargetGroup.WebGL, BuildTarget.WebGL);

        [MenuItem("Build/Windows")]
        public static void Windows()
            => Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        [MenuItem("Build/Windows x64")]
        public static void Windowsx64()
            => Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

        [MenuItem("Build/Linux")]
        public static void Linux()
            => Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64);

        [MenuItem("Build/Android")]
        public static void Android()
            => Build(BuildTargetGroup.Android, BuildTarget.Android);

        [MenuItem("Build/UWP")]
        public static void UWP()
            => Build(BuildTargetGroup.WSA, BuildTarget.WSAPlayer);

        [MenuItem("Build/All")]
        public static void All()
        {
            if (Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows).summary.totalErrors > 0)
                return;
            if (Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64).summary.totalErrors > 0)
                return;
            if (Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64).summary.totalErrors > 0)
                return;
            if (Build(BuildTargetGroup.Android, BuildTarget.Android).summary.totalErrors > 0)
                return;
            if (Build(BuildTargetGroup.WebGL, BuildTarget.WebGL).summary.totalErrors > 0)
                return;
        }

        [MenuItem("Build/Publish")]
        public static void TagAndIncrementVersion()
        {
            var gameSettingsGUIDs = AssetDatabase.FindAssets("t:GameSettings");
            var gameSettingsPath = AssetDatabase.GUIDToAssetPath(gameSettingsGUIDs[0]);
            var gameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>(gameSettingsPath);
            var tag = $"v{gameSettings.Version}";
            UnityEngine.Debug.Log($"Tagging git repository as: {tag}");
            Process.Start("git", $"tag -a {tag} -m \"\"");
            UnityEngine.Debug.Log($"Pushing tags...");
            Process.Start("git", "push --tags");
            UnityEngine.Debug.Log($"Pushing tags...Complete!");
            gameSettings.microVersion++;
            UnityEngine.Debug.Log($"Game Settings updated to: v{gameSettings.Version}");
            EditorUtility.SetDirty(gameSettings);
            UnityEngine.Debug.Log($"https://www.etdofresh.com/SailorStrike/{tag}");
        }

        private static BuildReport Build(BuildTargetGroup targetGroup, BuildTarget target)
        {
            var editorBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var editorBuildTarget = EditorUserBuildSettings.selectedStandaloneTarget;

            var buildPath = "Build";
            if (target == BuildTarget.StandaloneWindows)
                buildPath += $"/Windows/{ProductName}.exe";
            else if (target == BuildTarget.StandaloneWindows64)
                buildPath += $"/Windowsx64/{ProductName}.exe";
            else if (target == BuildTarget.StandaloneLinux64)
                buildPath += $"/Linux/{ProductName}.x86_64";
            else if (target == BuildTarget.Android)
                buildPath += $"/Android/{ProductName}.apk";
            else if (target == BuildTarget.WebGL)
                buildPath = @"W:\SailorStrike";
            else if (target == BuildTarget.WSAPlayer)
                buildPath += "/UWP";

            if (File.Exists(buildPath))
                File.Delete(buildPath);

            var buildOptions = BuildOptions.None;
            //buildOptions |= BuildOptions.Development;
            if (target == BuildTarget.WebGL)
                buildOptions |= BuildOptions.AutoRunPlayer;

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = ActiveScenes,
                targetGroup = targetGroup,
                target = target,
                locationPathName = buildPath,
                options = buildOptions,
            };
            
            var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
            {
                var exePath = Path.Combine(Directory.GetCurrentDirectory(), buildPath);
                Process.Start(new ProcessStartInfo(exePath));
                Process.Start(new ProcessStartInfo(exePath));
            }

            EditorUserBuildSettings.SwitchActiveBuildTarget(editorBuildTargetGroup, editorBuildTarget);
            return buildReport;
        }
    }
}