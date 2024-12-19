using System;
using System.Collections.Generic;
using System.Linq;
using VS.Pool.Repositories;
using UnityEditor;
using UnityEngine;

namespace VS.Pool.Editor
{
    internal class RepositoryIdWindow : EditorWindow
    {
        private string _id;
        private bool _isAvailableId = true;

        public event Action<RepositoryIdWindow, string> Closed;

        private void OnGUI()
        {
            _id = EditorGUILayout.TextField("Enter Id Repository", _id);

            EditorGUILayout.Space(5);

            _isAvailableId = IsAvailableId();
            if (!_isAvailableId)
                EditorGUILayout.HelpBox("Id empty or exists", MessageType.Error);
            else if (GUILayout.Button("Create Repository"))
                Close();
        }

        private bool IsAvailableId()
        {
            if (string.IsNullOrEmpty(_id))
                return false;

            IEnumerable<string> ids = GetIds();
            return ids.All(id => !_id.Equals(id));
        }

        private static IEnumerable<string> GetIds()
        {
            return AssetDatabase
                .FindAssets("t:ScriptableObject")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                .OfType<PoolRepository>()
                .Select(repository => repository.Id);
        }

        private void OnDestroy()
        {
            if (_isAvailableId)
                Closed?.Invoke(this, _id);

            Closed = null;

            _isAvailableId = true;
            _id = string.Empty;
        }
    }
}