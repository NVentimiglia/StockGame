using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// Reusable
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    public static class ConfigService<TConfig> where TConfig : ScriptableObject
    {
        private static Dictionary<string, TConfig[]> _folders = new Dictionary<string, TConfig[]>();
        private static Dictionary<string, TConfig> _files = new Dictionary<string, TConfig>();

        /// <summary>
        /// returns an entire folder
        /// </summary>
        public static TConfig[] GetFolder(string folder)
        {
            TConfig[] value;
            if (!_folders.TryGetValue(folder, out value))
            {
                value = Resources.LoadAll<TConfig>($"Data/{ folder }");
                _folders.Add(folder, value);
            }
            return value;
        }

        /// <summary>
        /// Return a config from a folder
        /// </summary>
        public static TConfig GetConfig(string folder, string key)
        {
            var files = GetFolder(folder);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].name == key)
                {
                    return files[i];
                }
            }
            UnityEngine.Debug.LogError($"Config not found {folder}/{key}");
            return null;
        }

        /// <summary>
        /// Return loose / standalone config datas
        /// </summary>
        public static TConfig GetConfig(string key)
        {
            TConfig value;
            if (!_files.TryGetValue(key, out value))
            {
                value = Resources.Load<TConfig>($"{ key }");
                _files.Add(key, value);
            }
            return value;
        }
    }
}
