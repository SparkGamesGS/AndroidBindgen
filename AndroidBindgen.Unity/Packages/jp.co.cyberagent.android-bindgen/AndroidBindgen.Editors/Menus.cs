// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace AndroidBindgen.Editors
{
    internal class Menus
    {
        [MenuItem("Assets/Create/Android Bindgen/Profile with Assembly Definition", false, 1)]
        private static void CreateNewAsset()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<AndroidBindgenProfile>(), "Android.asset");
            ProjectWindowUtil.CreateAssetWithContent(
                "Android.asmdef",
                "{ \"name\": \"Android\", \"rootNamespace\": \"\", \"references\": [ \"GUID:279c2e19bba7d40e584c2fd198c61765\" ], \"includePlatforms\": [], \"excludePlatforms\": [], \"allowUnsafeCode\": false, \"overrideReferences\": false, \"precompiledReferences\": [], \"autoReferenced\": true, \"defineConstraints\": [], \"versionDefines\": [], \"noEngineReferences\": false}");
        }
    }
}
