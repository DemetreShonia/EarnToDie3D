namespace Dreamteck.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;
    using UnityEditor;

    public class ModuleInstaller
    {
        protected const string DREAMTECK_FOLDER_NAME = "Dreamteck";

        /// <summary>
        /// Local directory within the Dreamteck folder of the unitypackage
        /// </summary>
        private string _packageDirectory = "";
        private string _packageName = "";
        private List<string> _scriptingDefines = new List<string>();
        private List<string> _uninstallDirectories = new List<string>();
        private Dictionary<string, List<string>> _assemblyLinks = new Dictionary<string, List<string>>();

        public ModuleInstaller(string packageDirectory, string packageName)
        {
            _packageDirectory = packageDirectory;
            _packageName = packageName;
        }

        public void AddAssemblyLink(string dreamteckAssemblyDirectory, string dreamteckAssemblyName, string addedAssemblyName)
        {
            string localFilePath = Path.Combine(DREAMTECK_FOLDER_NAME, dreamteckAssemblyDirectory, dreamteckAssemblyName + ".asmdef");
            if (_assemblyLinks.ContainsKey(localFilePath))
            {
                _assemblyLinks[localFilePath].Add(addedAssemblyName);
            } else
            {
                _assemblyLinks.Add(localFilePath, new List<string>(new string[] { addedAssemblyName }));
            }
        }

        public void AddUninstallDirectory(string dreamteckLocalDirectory)
        {
            if (!_uninstallDirectories.Contains(dreamteckLocalDirectory))
            {
                _uninstallDirectories.Add(dreamteckLocalDirectory);
            }
        }

        public void AddScriptingDefine(string define)
        {
            if (!_scriptingDefines.Contains(define))
            {
                _scriptingDefines.Add(define);
            }
        }

        public void Install()
        {
            string globalPath = ResourceUtility.FindFolder(Application.dataPath, DREAMTECK_FOLDER_NAME + "/" + _packageDirectory);
            if (!Directory.Exists(globalPath))
            {
                EditorUtility.DisplayDialog("Missing Package", "Package directory not found: " + _packageDirectory, "OK");
                return;
            }
            globalPath = Path.Combine(globalPath, _packageName + ".unitypackage");
            if (!File.Exists(globalPath))
            {
                EditorUtility.DisplayDialog("Missing Package", "Package file not found: " + _packageDirectory, "OK");
                return;
            }

            foreach (var key in _assemblyLinks.Keys)
            {
                for (int i = 0; i < _assemblyLinks[key].Count; i++)
                {
                    AddAssemblyReference(key, _assemblyLinks[key][i]);
                }
            }

            AssetDatabase.ImportPackage(globalPath, false);
            EditorUtility.DisplayDialog("Import Complete", _packageName + " is now installed.", "OK");
            for (int i = 0; i < _scriptingDefines.Count; i++)
            {
                ScriptingDefineUtility.Add(_scriptingDefines[i], EditorUserBuildSettings.selectedBuildTargetGroup, true);
            }
        }

        public void Uninstall()
        {
            string dialogText = "The assets in the following folders will be removed: \n";
            for (int i = 0; i < _uninstallDirectories.Count; i++)
            {
                dialogText += _uninstallDirectories[i] + "\n";
            }
            bool result = EditorUtility.DisplayDialog("Uninstalling", dialogText, "OK", "Cancel");
            if (!result) return;

            for (int i = 0; i < _uninstallDirectories.Count; i++)
            {
                string globalPath = ResourceUtility.FindFolder(Application.dataPath, DREAMTECK_FOLDER_NAME + "/" + _uninstallDirectories[i]);
                string relativePath = "Assets" + globalPath.Substring(Application.dataPath.Length);
                Debug.Log("Uninstalling " + relativePath);
                AssetDatabase.DeleteAsset(relativePath);
            }

            foreach (var key in _assemblyLinks.Keys)
            {
                for (int i = 0; i < _assemblyLinks[key].Count; i++)
                {
                    RemoveAssemblyReference(key, _assemblyLinks[key][i]);
                }
            }

            

            for (int i = 0; i < _scriptingDefines.Count; i++)
            {
                ScriptingDefineUtility.Remove(_scriptingDefines[i], EditorUserBuildSettings.selectedBuildTargetGroup, true);
            }
        }

        private static void AddAssemblyReference(string dreamteckAssemblyPath, string addedAssemblyName)
        {
            var path = Path.Combine(Application.dataPath, dreamteckAssemblyPath);
            var data = "";
            using (var reader = new StreamReader(path))
            {
                data = reader.ReadToEnd();
            }

            var asmDef = AssemblyDefinition.CreateFromJSON(data);
            foreach (var reference in asmDef.references)
            {
                if (reference == addedAssemblyName) return;
            }

            ArrayUtility.Add(ref asmDef.references, addedAssemblyName);
            Debug.Log("Adding " + addedAssemblyName + " to assembly " + dreamteckAssemblyPath);
            using (var writer = new StreamWriter(path, false))
            {
                writer.Write(asmDef.ToString());
            }
        }
        
        private static void RemoveAssemblyReference(string dreamteckAssemblyPath, string addedAssemblyName)
        {
            var path = Path.Combine(Application.dataPath, dreamteckAssemblyPath);
            var data = "";
            using (var reader = new StreamReader(path))
            {
                data = reader.ReadToEnd();
            }

            var asmDef = AssemblyDefinition.CreateFromJSON(data);
            bool contains = false;
            foreach (var reference in asmDef.references)
            {
                if (reference != addedAssemblyName) continue;
                contains = true;
                break;
            }
            if (!contains) return;

            ArrayUtility.Remove(ref asmDef.references, addedAssemblyName);
            Debug.Log("Removing " + addedAssemblyName + " from assembly " + dreamteckAssemblyPath);
            using (var writer = new StreamWriter(path, false))
            {
                writer.Write(asmDef.ToString());
            }
        }

        [System.Serializable]
        public struct AssemblyDefinition
        {
            public string name;
            public string rootNamespace;
            public string[] references;
            public string[] includePlatforms;
            public string[] exludePlatforms;
            public bool allowUnsafeCode;
            public bool overrideReferences;
            public string precompiledReferences;
            public bool autoReferenced;
            public string[] defineConstraints;
            public string[] versionDefines;
            public bool noEngineReferences;

            public static AssemblyDefinition CreateFromJSON(string json)
            {
                return JsonUtility.FromJson<AssemblyDefinition>(json);
            }

            public override string ToString()
            {
                return JsonUtility.ToJson(this, true);
            }
        }

    }
}