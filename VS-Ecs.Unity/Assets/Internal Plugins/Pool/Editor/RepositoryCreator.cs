using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using VS.Pool.Container;
using VS.Pool.Repositories;

namespace VS.Pool.Editor
{
    internal class RepositoryCreator
    {
        [MenuItem("Tools/Pool/Create Repository")]
        public static void CreateRepository()
        {
            if (Application.isPlaying)
                return;

            ShowRepositoryIdWindow();
        }

        private static void ShowRepositoryIdWindow()
        {
            RepositoryIdWindow window = EditorWindow.GetWindow<RepositoryIdWindow>();
            window.Closed += OnRepositoryIdWindowClosed;
        }

        private static void OnRepositoryIdWindowClosed(RepositoryIdWindow window, string id)
        {
            window.Closed -= OnRepositoryIdWindowClosed;

            PoolRepository repository = CreateRepository(id);
            SetRepositories(repository);

            Selection.activeObject = repository;
            CreatePoolsId();
            AssetDatabase.Refresh();
        }

        private static PoolRepository CreateRepository(string id)
        {
            PoolRepository poolRepository = ScriptableObject.CreateInstance<PoolRepository>();
            poolRepository.Id = id;

            string fileName = $"{id}{Constants.Repository}";
            poolRepository.name = fileName;

            string directoryPath = GetPoolSettingsConfig().RepositoryPath;
            string path = Path.Combine(directoryPath, $"{fileName}{Constants.AssetExtensions}");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            AssetDatabase.CreateAsset(poolRepository, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return poolRepository;
        }

        private static void SetRepositories(PoolRepository repository)
        {
            using PrefabUtility.EditPrefabContentsScope editingScope = new(GetPoolPath());

            editingScope.prefabContentsRoot.GetComponent<PoolsContainersEditor>().AddPoolRepository(repository);
        }

        private static string GetPoolPath() => AssetDatabase.GetAssetPath(GetPoolsContainersEditor());

        private static PoolsContainersEditor GetPoolsContainersEditor()
        {
            return AssetDatabase
                .FindAssets("t:prefab")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<MonoBehaviour>)
                .OfType<PoolsContainersEditor>()
                .First();
        }

        private static PoolSettingsConfig GetPoolSettingsConfig()
        {
            return AssetDatabase
                .FindAssets("t:ScriptableObject")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                .OfType<PoolSettingsConfig>()
                .First();
        }

        private static void CreatePoolsId()
        {
            string directoryPath = GetPoolSettingsConfig().GeneratedScriptPath;

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            List<string> id = GetPoolsContainersEditor().GetId();

            StringBuilder stb = new();
            stb.Append("namespace VS.Pool\n");
            stb.Append("{\n");
            stb.Append($"\tpublic class {Constants.ClassName}\n");
            stb.Append("\t{\n");

            foreach (string element in id)
                stb.Append($"\t\tpublic const string {element} = \"{element}\";\n");

            stb.Append("\t}\n");
            stb.Append("}");

            File.WriteAllText($"{directoryPath}/{Constants.ClassName}{Constants.ClassExtensions}", stb.ToString());
        }
    }
}