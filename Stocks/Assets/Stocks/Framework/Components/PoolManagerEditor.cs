#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Components.Internal
{
    [CustomEditor(typeof(PoolManager), true)]
    public class PoolManagerEditor : Editor
    {
        private Color _backgroundColor;
        private Color _red = (Color.red * .5f);
        private Color _green = (Color.green * .5f);
        private GUIContent _minusSign = new GUIContent("-", "Delete");
        private GUIContent _plusSign = new GUIContent("+", "Add");
        private int _btnSize = 25;

        private SerializedProperty _templates;

        private void OnEnable()
        {
            _templates = serializedObject.FindProperty("PoolConfigs");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Pools", EditorStyles.boldLabel);

            for (int i = 0; i < _templates.arraySize; i++)
            {
                RenderTemplate(i);
            }

            BeginColorButton(_green);
            if (GUILayout.Button(_plusSign, EditorStyles.miniButton))
            {
                int size = _templates.arraySize;
                _templates.InsertArrayElementAtIndex(_templates.arraySize);
            }
            EndColorButton();
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        void RenderTemplate(int index)
        {
            var dto = _templates.GetArrayElementAtIndex(index);
            var initCount = dto.FindPropertyRelative("InitCount");
            var maxCount = dto.FindPropertyRelative("MaxCount");
            var view = dto.FindPropertyRelative("View");

            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();

            var w = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 35;
            EditorGUILayout.PropertyField(initCount, new GUIContent("Init"));
            EditorGUILayout.PropertyField(maxCount, new GUIContent("Max"));
            EditorGUIUtility.labelWidth = w;

            // VM
            EditorGUILayout.ObjectField(view, typeof(GameObject), GUIContent.none);

            // delete
            BeginColorButton(_red);

            if (GUILayout.Button(_minusSign, EditorStyles.miniButton, GUILayout.Width(_btnSize)))
            {
                _templates.DeleteArrayElementAtIndex(index);
            }
            EndColorButton();

            //
            EditorGUILayout.EndHorizontal();
        }

        void BeginColorButton(Color bg)
        {
            _backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = bg;
        }

        void EndColorButton()
        {
            GUI.backgroundColor = _backgroundColor;
        }

        int[] GetIntArray(SerializedProperty property)
        {
            var states = property.FindPropertyRelative("States");
            var vals = new int[states.arraySize];

            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = states.GetArrayElementAtIndex(i).intValue;
            }

            return vals;
        }

        static List<int> _myList = new List<int>();

        int[] GetIndexedArray(int[] options, int[] values)
        {
            _myList.Clear();

            for (int i = 0; i < values.Length; i++)
            {
                var o = Array.IndexOf(options, values[i]);
                if (o != -1 && _myList.IndexOf(o) == -1)
                    _myList.Add(o);
            }

            return _myList.ToArray();
        }

        void SetIndexedArray(SerializedProperty property, int flags, int[] values)
        {
            var states = property.FindPropertyRelative("States");
            states.ClearArray();

            if (flags == 0)
                return;

            int counter = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (flags == -1 || BinaryHelper.IsBitSet(flags, i))
                {
                    states.InsertArrayElementAtIndex(counter);
                    states.GetArrayElementAtIndex(counter).intValue = values[i];
                    counter++;
                }
            }
        }
    }
}
#endif