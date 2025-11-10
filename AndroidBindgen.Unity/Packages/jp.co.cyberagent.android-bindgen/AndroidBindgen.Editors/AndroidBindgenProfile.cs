// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cecla;
using UnityEditor;
using UnityEngine;

namespace AndroidBindgen.Editors
{
    [CreateAssetMenu(fileName = "AndroidBindgen Profile", menuName = "Android Bindgen/Profile")]
    public class AndroidBindgenProfile : ScriptableObject, ISerializationCallbackReceiver
    {
        private readonly Dictionary<Hash128, int> _generatedItemKeysSet = new();

        #region Event Functions

        private void OnValidate()
        {
            if (targetLibraries != null)
                for (var i = 0; i < targetLibraries.Length; i++)
                {
                    var targetLibrary = targetLibraries[i];
                    if (targetLibrary == null) continue;
                    var path = AssetDatabase.GetAssetPath(targetLibrary);
                    var importerType = AssetDatabase.GetDefaultImporter(path);
                    var ext = Path.GetExtension(path);
                    if (importerType != typeof(PluginImporter) || ext is not (".aar" or ".jar"))
                        targetLibraries[i] = null;
                }

            if (javaSourceLibraries != null)
                for (var i = 0; i < javaSourceLibraries.Length; i++)
                {
                    var javaSourceLibrary = javaSourceLibraries[i];
                    if (javaSourceLibrary == null) continue;
                    var path = AssetDatabase.GetAssetPath(javaSourceLibrary);
                    var importerType = AssetDatabase.GetDefaultImporter(path);
                    var ext = Path.GetExtension(path);
                    if (importerType != typeof(PluginImporter) || ext is not ".jar")
                        javaSourceLibraries[i] = null;
                }
        }

        #endregion

        private void EnsureKeysSetInitialized()
        {
            if (_generatedItemKeysSet.Count > 0) return;
            for (var i = 0; i < generatedItemKeys.Count; i++)
                _generatedItemKeysSet.Add(generatedItemKeys[i], i);
        }

        public void SetGenerated(string key, bool value)
        {
            EnsureKeysSetInitialized();
            var hash = Hash128.Compute(key);
            if (value && _generatedItemKeysSet.TryAdd(hash, generatedItemKeys.Count))
            {
                Undo.RecordObject(this, "Set Generated");
                generatedItemKeys.Add(hash);
                EditorUtility.SetDirty(this); // NOTE: Undo.RecordObject seems to not mark the object as dirty
            }
            else if (!value && _generatedItemKeysSet.Remove(hash, out var index))
            {
                Undo.RecordObject(this, "Set Generated");
                var endIndex = generatedItemKeys.Count - 1;
                if (endIndex == index)
                {
                    generatedItemKeys.RemoveAt(index);
                    return;
                }

                var end = generatedItemKeys[endIndex];
                generatedItemKeys[index] = end;
                generatedItemKeys.RemoveAt(endIndex);
                _generatedItemKeysSet[end] = index;
                EditorUtility.SetDirty(this); // NOTE: Undo.RecordObject seems to not mark the object as dirty
            }
        }

        public bool GetGenerated(string key)
        {
            EnsureKeysSetInitialized();
            var hash = Hash128.Compute(key);
            return _generatedItemKeysSet.ContainsKey(hash);
        }

        private string GetAndroidPlatformPath(string playbackEngineDir)
        {
            var platformsDir = Path.Combine(playbackEngineDir, "SDK", "platforms");

            var maxVer = 0;
            string maxVerDir = null;
            foreach (var platformDir in Directory.EnumerateDirectories(platformsDir, "android-*",
                         SearchOption.TopDirectoryOnly))
                if (int.TryParse(Path.GetFileName(platformDir).Substring("android-".Length), out var ver) &&
                    ver > maxVer)
                {
                    maxVer = ver;
                    maxVerDir = platformDir;
                }

            return maxVerDir;
        }

        private IEnumerable<string> GetArchivePaths()
        {
            var playbackEngineDir =
                BuildPipeline.GetPlaybackEngineDirectory(BuildTarget.Android, BuildOptions.None);

            var targetLibraryPaths = targetLibraries.Select(AssetDatabase.GetAssetPath);

            var androidJarPath = GetAndroidPlatformPath(playbackEngineDir);
            if (androidJarPath != null)
                targetLibraryPaths = targetLibraryPaths.Append(Path.Combine(androidJarPath, "android.jar"));

            targetLibraryPaths = targetLibraryPaths.Append(Path.Combine(playbackEngineDir, "Variations",
                "il2cpp", "Release", "Classes", "classes.jar"));

            return targetLibraryPaths;
        }

        private IEnumerable<string> GetSourceArchivePaths()
        {
            var playbackEngineDir =
                BuildPipeline.GetPlaybackEngineDirectory(BuildTarget.Android, BuildOptions.None);

            var sourceLirabryPaths = javaSourceLibraries.Select(AssetDatabase.GetAssetPath);

            var androidJarPath = GetAndroidPlatformPath(playbackEngineDir);
            if (androidJarPath != null)
                sourceLirabryPaths = sourceLirabryPaths.Append(Path.Combine(androidJarPath, "android-stubs-src.jar"));

            return sourceLirabryPaths;
        }

        private IEnumerable<(JavaArchive archive, string path)> GetArchives()
        {
            List<(JavaArchive, string)> parsedLibraries = new();
            foreach (var library in GetArchivePaths())
                switch (Path.GetExtension(library))
                {
                    case ".jar":
                    {
                        parsedLibraries.Add((new JavaArchive(library), Path.GetFileName(library)));
                        break;
                    }
                    case ".aar":
                    {
                        parsedLibraries.Add((new AndroidArchive(library), Path.GetFileName(library)));
                        break;
                    }
                    default:
                    {
                        Debug.LogWarning($"Unsupported library type: {library}");
                        continue;
                    }
                }

            return parsedLibraries;
        }

        public void Generate()
        {
            EnsureKeysSetInitialized();
            var archives = GetArchives().ToArray();

            // javadoc
            JavadocManager javadocManager = null;
            var sourcePaths = GetSourceArchivePaths().ToArray();
            if (enableJavaSourceAnalysis && sourcePaths.Length != 0)
            {
                // copy java-source-utils.jar to library
                var src =
                    "Packages/jp.co.cyberagent.android-bindgen/AndroidBindgen.Editors/Executables~/java-source-utils.jar";
                var dest = "Library/java-source-utils.jar";

                File.Copy(src, dest, true);

                var javaSourceUtils = new JavaSourceUtils(dest, GetArchivePaths().ToArray(), sourcePaths,
                    out var stdout,
                    out var stderr);
                StringBuilder stdoutBuilder = new();
                StringBuilder stderrBuilder = new();
                EditorUtility.DisplayProgressBar("Android Bindgen", "Generating javadoc...", 0);
                string javadocPath;
                try
                {
                    async Task ReadStreamAsync(StreamReader reader, StringBuilder builder)
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            EditorUtility.DisplayProgressBar("Android Bindgen", line, 0);
                            builder.Append(line);
                        }
                    }

                    _ = ReadStreamAsync(stdout, stdoutBuilder);
                    _ = ReadStreamAsync(stderr, stderrBuilder);
                    javadocPath = javaSourceUtils.GetResultPath();
                }
                catch
                {
                    var stderrContent = stderrBuilder.ToString();
                    if (!string.IsNullOrEmpty(stderrContent)) Debug.Log(stderrContent);
                    throw;
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                    var stdoutContent = stdoutBuilder.ToString();
                    if (!string.IsNullOrEmpty(stdoutContent)) Debug.Log(stdoutContent);
                }

                javadocManager = new JavadocManager(new[] { javadocPath });
            }

            var generator = new Generator(new ClassManager(archives.Select(t => t.archive)), true, javadocManager,
                customHeader);

            foreach (var tuple in archives)
            {
                var library = new JavaLibrary(tuple.archive, tuple.path);
                var libraryKey = library.GenerationScopeKey;

                foreach (var package in library.Packages)
                {
                    var packageKey = $"/{libraryKey}/{package.GenerationScopeKey}";
                    foreach (var @class in package.Classes)
                    {
                        List<FieldInfo> fields = null;
                        List<MethodInfo> methods = null;
                        var classKey = $"{packageKey}/{@class.GenerationScopeKey}";
                        foreach (var field in @class.Fields)
                        {
                            var fieldKey = $"{classKey}/{field.GenerationScopeKey}";
                            var hash = Hash128.Compute(fieldKey);
                            if (!_generatedItemKeysSet.ContainsKey(hash)) continue;
                            (fields ??= new List<FieldInfo>()).Add(field.Source);
                        }

                        foreach (var method in @class.Methods)
                        {
                            var methodKey = $"{classKey}/{method.GenerationScopeKey}";
                            var hash = Hash128.Compute(methodKey);
                            if (!_generatedItemKeysSet.ContainsKey(hash)) continue;
                            (methods ??= new List<MethodInfo>()).Add(method.Source);
                        }

                        bool generateProxy;
                        {
                            var proxyKey = $"{classKey}/I:Proxy";
                            var hash = Hash128.Compute(proxyKey);
                            generateProxy = _generatedItemKeysSet.ContainsKey(hash);
                        }

                        if (fields != null || methods != null || generateProxy)
                            generator.Request(@class.Source.ThisClass.Name ?? throw new InvalidOperationException(),
                                new ApiFilter(fields, methods, generateProxy));
                    }
                }
            }

            generator.Generate(Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(this)), "generated"));
        }

        #region Nested type: AndroidBindgenProfileEditor

        [CustomEditor(typeof(AndroidBindgenProfile))]
        internal class AndroidBindgenProfileEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(targetLibraries)));

                var enableJavaSourceAnalysisProp = serializedObject.FindProperty(nameof(enableJavaSourceAnalysis));
                EditorGUILayout.PropertyField(enableJavaSourceAnalysisProp);

                if (enableJavaSourceAnalysisProp.boolValue)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(javaSourceLibraries)));

                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(customHeader)));

                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("Open Settings"))
                {
                    var libraries =
                        (target as AndroidBindgenProfile)?.GetArchives()
                        .Select(t => new JavaLibrary(t.archive, t.path));

                    var window = EditorWindow.GetWindow<ProfileWindow>();
                    window.Init(libraries, target as AndroidBindgenProfile);
                }

                if (GUILayout.Button("Generate"))
                    (target as AndroidBindgenProfile)?.Generate();
            }
        }

        #endregion

        #region Nested type: ApiFilter

        private class ApiFilter : IApiFilter
        {
            private readonly HashSet<FieldInfo> _fields;
            private readonly bool _generateProxy;
            private readonly HashSet<MethodInfo> _methods;

            public ApiFilter(IEnumerable<FieldInfo> fields, IEnumerable<MethodInfo> methods, bool generateProxy)
            {
                _generateProxy = generateProxy;
                _fields = new HashSet<FieldInfo>(fields ?? Array.Empty<FieldInfo>());
                _methods = new HashSet<MethodInfo>(methods ?? Array.Empty<MethodInfo>());
            }

            #region IApiFilter Members

            public bool ShouldInclude(FieldInfo field)
            {
                return _fields.Contains(field);
            }

            public bool ShouldInclude(MethodInfo method)
            {
                return _methods.Contains(method);
            }

            public bool ShouldIncludeProxy()
            {
                return _generateProxy;
            }

            #endregion
        }

        #endregion

        #region Serialized Fields

        [SerializeField] private bool enableJavaSourceAnalysis = true;
        [SerializeField] private DefaultAsset[] targetLibraries = Array.Empty<DefaultAsset>();
        [SerializeField] private DefaultAsset[] javaSourceLibraries = Array.Empty<DefaultAsset>();
        [SerializeField] [Multiline] private string customHeader;
        [SerializeField] private List<Hash128> generatedItemKeys = new();

        #endregion

        #region ISerializationCallbackReceiver Members

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _generatedItemKeysSet.Clear();
        }

        #endregion
    }
}
