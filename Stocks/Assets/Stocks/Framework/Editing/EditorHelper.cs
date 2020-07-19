#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Framework
{
    public static class EditorExt
    {
        [MenuItem("Framework/Delete PlayerPrefs (All)")]
        static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
#endif