using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Synapse_Z
{
    public static class GlobalVariables
    {
        private static bool topMostGlobal = true;
        private static Action<bool> onTopMostGlobalChanged;

        public static bool injecting { get; set; } = false;
        public static bool isDone { get; set; } = false;
        private static bool autoInject;
        public static bool TabClosingPrompt { get; set; } = true;
        public static bool ClearEditorPrompt { get; set; } = true;
        public static bool UnlockFPS { get; set; } = false;
        public static string CurrentEditorTheme { get; set; } = "tomorrow_night_eighties";

        public static bool AutoInject
        {
            get => autoInject;
            set
            {
                if (autoInject != value)
                {
                    autoInject = value;
                    if (autoInject)
                    {
                        AutoInjectManager.StartAutoInjectTask();
                    }
                    else
                    {
                        AutoInjectManager.StopAutoInjectTask();
                    }
                }
            }
        }

        public static bool TopMostGlobal
        {
            get => topMostGlobal;
            set
            {
                if (topMostGlobal != value)
                {
                    topMostGlobal = value;
                    onTopMostGlobalChanged?.Invoke(value);
                }
            }
        }

        public static void RegisterOnTopMostGlobalChanged(Action<bool> callback)
        {
            onTopMostGlobalChanged += callback;
        }

        private static readonly string SettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "settings.txt");

        static GlobalVariables()
        {
            var libDirectory = Path.GetDirectoryName(SettingsFilePath);
            if (!Directory.Exists(libDirectory))
            {
                Directory.CreateDirectory(libDirectory);
            }
        }

        public static async Task SaveSettingsAsync()
        {
            var settings = new Settings
            {
                injecting = injecting,
                isDone = isDone,
                AutoInject = AutoInject,
                TabClosingPrompt = TabClosingPrompt,
                ClearEditorPrompt = ClearEditorPrompt,
                TopMostGlobal = TopMostGlobal,
                UnlockFPS = UnlockFPS,
                CurrentEditorTheme = CurrentEditorTheme
            };

            using (var stream = new FileStream(SettingsFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteLineAsync(settings.AutoInject.ToString());
                await writer.WriteLineAsync(settings.TabClosingPrompt.ToString());
                await writer.WriteLineAsync(settings.ClearEditorPrompt.ToString());
                await writer.WriteLineAsync(settings.TopMostGlobal.ToString());
                await writer.WriteLineAsync(settings.UnlockFPS.ToString());
                await writer.WriteLineAsync(settings.CurrentEditorTheme);
            }
        }

        public static async Task LoadSettingsAsync()
        {
            if (File.Exists(SettingsFilePath))
            {
                using (var stream = new FileStream(SettingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
                using (var reader = new StreamReader(stream))
                {
                    bool.TryParse(await reader.ReadLineAsync(), out bool autoInjectValue);
                    bool.TryParse(await reader.ReadLineAsync(), out bool tabClosingPromptValue);
                    bool.TryParse(await reader.ReadLineAsync(), out bool clearEditorPromptValue);
                    bool.TryParse(await reader.ReadLineAsync(), out bool topMostGlobalValue);
                    bool.TryParse(await reader.ReadLineAsync(), out bool unlockFPSValue);
                    string currentEditorThemeValue = await reader.ReadLineAsync();

                    AutoInject = autoInjectValue;
                    TabClosingPrompt = tabClosingPromptValue;
                    ClearEditorPrompt = clearEditorPromptValue;
                    TopMostGlobal = topMostGlobalValue;
                    UnlockFPS = unlockFPSValue;
                    CurrentEditorTheme = currentEditorThemeValue;
                }
            }
        }

        private class Settings
        {
            public bool injecting { get; set; }
            public bool isDone { get; set; }
            public bool AutoInject { get; set; }
            public bool TabClosingPrompt { get; set; }
            public bool ClearEditorPrompt { get; set; }
            public bool TopMostGlobal { get; set; }
            public bool UnlockFPS { get; set; }
            public string CurrentEditorTheme { get; set; }
        }
    }
}
