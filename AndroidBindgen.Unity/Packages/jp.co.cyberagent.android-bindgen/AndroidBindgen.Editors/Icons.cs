// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AndroidBindgen.Editors
{
    internal static class Icons
    {
        private static readonly Dictionary<string, Texture2D> CachedIcons = new();

        public static Texture2D GetIcon(string name)
        {
            if (EditorGUIUtility.isProSkin) name += "_dark";
            name += ".png";

            if (!CachedIcons.TryGetValue(name, out var cached) || !cached)
                CachedIcons[name] = cached = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(
                    "Packages/jp.co.cyberagent.android-bindgen/AndroidBindgen.Editors/Icons", name));

            return cached;
        }
    }
}
