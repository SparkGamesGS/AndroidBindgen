// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AndroidBindgen.Editors
{
    internal class ProfileWindow : EditorWindow, IGenerationStatusProvider
    {
        private readonly Dictionary<VisualElement, IDisposable> _bindings = new();

        private readonly Dictionary<string /* key */, ReactiveProperty<bool>> _isGenerated = new();
        private CancellationTokenSource _cancelCurrentBuildOnSearch;
        private JavaLibraryItem[] _rootLibraryItems;
        private string _searchText = "";
        private MultiColumnTreeView _treeViewMain;

        #region IGenerationStatusProvider Members

        public IReactiveProperty<bool> GetIsGenerated(string key)
        {
            if (!_isGenerated.TryGetValue(key, out var cached))
            {
                _isGenerated[key] = cached = new ReactiveProperty<bool>();
                if (profile.GetGenerated(key))
                    cached.Value = true;

                cached.Subscribe((_, newValue) => { profile.SetGenerated(key, newValue); });
            }

            return cached;
        }

        #endregion

        private Func<ITreeViewItem, bool> GetFilter(bool withSearch)
        {
            var text = withSearch ? _searchText : "";
            return string.IsNullOrEmpty(text) && showDeprecatedItems
                ? _ => true
                : item => (showDeprecatedItems || !item.IsDeprecated) &&
                          item.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase);
        }

        private async void ApplyCurrentFilter()
        {
            if (libraries == null) return;

            try
            {
                _cancelCurrentBuildOnSearch?.Cancel();
            }
            catch (ObjectDisposedException)
            {
            }

            _cancelCurrentBuildOnSearch = null;
            var cts = new CancellationTokenSource();
            _cancelCurrentBuildOnSearch = cts;

            var treeViewMain = _treeViewMain;
            var filterItem = GetFilter(true);

            IList<TreeViewItemData<ITreeViewItem>> roots;
            try
            {
                roots = await BuildItemsAsync(filterItem, cts.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            finally
            {
                cts.Dispose();
            }

            var selectedIndex = treeViewMain.selectedIndex;
            var selectedId = selectedIndex >= 0 ? treeViewMain.GetIdForIndex(selectedIndex) : -1;

            treeViewMain.Clear();
            treeViewMain.SetRootItems(roots);
            treeViewMain.Rebuild();

            if (selectedId >= 0)
            {
#if UNITY_2023_2_OR_NEWER
                treeViewMain.ScrollToItemById(selectedId);
#else
                // The scrolling need to be deferred because the viewport layout is not updated yet.
                // This is fixed in 2023.2 and later: https://issuetracker.unity3d.com/issues/scrolltoitem-should-work-on-the-same-frame-the-layout-size-is-updated
                treeViewMain.schedule.Execute(() => { treeViewMain.ScrollToItemById(selectedId); });
#endif
            }
        }

        private void OnUndoRedo()
        {
            foreach (var (key, isGenerated) in _isGenerated)
                isGenerated.Value = profile.GetGenerated(key);
        }

        public void Init(IEnumerable<JavaLibrary> libraries, AndroidBindgenProfile profile)
        {
            this.profile = profile;
            _isGenerated.Clear();

            _treeViewMain.CollapseAll();
            titleContent = new GUIContent(profile.name);

            this.libraries = libraries.ToArray();
            Rebuild();
        }

        private void Rebuild()
        {
            if (libraries == null) return;
            var treeViewMain = _treeViewMain;

            treeViewMain.viewDataKey = $"{nameof(ProfileWindow)}._treeViewMain";

            _rootLibraryItems = libraries
                .Select(library => new JavaLibraryItem(library, new GenerationStatusScope(this))).ToArray();

            var roots = BuildItemsCore(GetFilter(true), default);

            treeViewMain.Clear();
            treeViewMain.SetRootItems(roots);
            treeViewMain.Rebuild();
        }

        private async Task<IList<TreeViewItemData<ITreeViewItem>>> BuildItemsAsync(Func<ITreeViewItem, bool> filterItem,
            CancellationToken ct)
        {
            return await Task.Run(() => BuildItemsCore(filterItem, ct), ct).ConfigureAwait(false);
        }

        private IList<TreeViewItemData<ITreeViewItem>> BuildItemsCore(Func<ITreeViewItem, bool> filterItem,
            CancellationToken ct)
        {
            static TreeViewItemData<ITreeViewItem>? CreateData(ITreeViewItem item, ref int id,
                Func<ITreeViewItem, bool> filterItem, CancellationToken ct)
            {
                ct.ThrowIfCancellationRequested();

                List<TreeViewItemData<ITreeViewItem>> children = new();
                var any = false;
                foreach (var child in item.Children)
                {
                    var data = CreateData(child, ref id, filterItem, ct);
                    if (!data.HasValue) continue;
                    any = true;
                    children.Add(data.Value);
                }

                id++;

                if (!any && (filterItem == null || !filterItem(item))) return null;

                return new TreeViewItemData<ITreeViewItem>(id, item, children);
            }

            List<TreeViewItemData<ITreeViewItem>> roots = new();
            {
                var id = 0;
                foreach (var library in _rootLibraryItems)
                {
                    var data = CreateData(library, ref id,
                        filterItem,
                        ct);
                    if (!data.HasValue) continue;
                    roots.Add(data.Value);
                }
            }

            return roots;
        }

        #region Serialized Fields

        [SerializeField] private VisualTreeAsset mainUxml;
        [SerializeField] private VisualTreeAsset nameCellUxml;
        [SerializeField] private JavaLibrary[] libraries;
        [SerializeField] private AndroidBindgenProfile profile;
        [SerializeField] private bool showDeprecatedItems;

        #endregion

        #region Event Functions

        private void OnEnable()
        {
            if (Undo.undoRedoPerformed != null) Undo.undoRedoPerformed += OnUndoRedo;
            else Undo.undoRedoPerformed = OnUndoRedo;
        }

        private void OnDisable()
        {
            try
            {
                _cancelCurrentBuildOnSearch?.Cancel();
            }
            catch (ObjectDisposedException)
            {
            }

            if (Undo.undoRedoPerformed != null) Undo.undoRedoPerformed -= OnUndoRedo;
        }

        private void CreateGUI()
        {
            var root = rootVisualElement;
            VisualElement uxml = mainUxml.Instantiate();
            uxml.style.flexGrow = 1f;
            root.Add(uxml);

            var treeViewMain = _treeViewMain =
                root.Q<MultiColumnTreeView>("treeview-main") ?? throw new InvalidOperationException();
            var nameColumn = treeViewMain.columns["treeview-column-name"];
            var kindColumn = treeViewMain.columns["treeview-column-kind"];
            var typeColumn = treeViewMain.columns["treeview-column-type"];
            var parametersColumn = treeViewMain.columns["treeview-column-parameters"];

            var generateButton = root.Q<Button>("button-generate");
            var selectButton = root.Q<Button>("button-select");
            var searchFieldMain = root.Q<ToolbarSearchField>("searchfield-main");
            var deprecatedToggle = root.Q<ToolbarToggle>("toggle-deprecated");

            nameColumn.makeCell += () =>
            {
                var container = nameCellUxml.CloneTree();
                container.style.flexGrow = 1f;
                return container;
            };
            nameColumn.bindCell += (element, index) =>
            {
                var item = treeViewMain.GetItemDataForIndex<ITreeViewItem>(index);
                var toggle = element.Q<Toggle>();
                toggle.text = item.Name;

                // icon
                var image = toggle.Q<Image>();
                if (image == null)
                {
                    image = new Image
                    {
                        scaleMode = ScaleMode.ScaleToFit,
                        style =
                        {
                            flexGrow = 0,
                            flexShrink = 0,
                            marginLeft = 5,
                            width = 16
                        }
                    };
                    toggle.Q<Label>()?.parent.Insert(1, image);
                }

                image.image = Icons.GetIcon(item.Kind.ToLower());

                var manipulatable = item.IsManipulatable;
                toggle.SetEnabled(manipulatable);

                void OnNext(GenerationStatusKind newValue)
                {
                    toggle.showMixedValue = newValue is GenerationStatusKind.Partial;
                    toggle.SetValueWithoutNotify(newValue is GenerationStatusKind.Enabled
                        or GenerationStatusKind.Partial);
                }

                OnNext(item.Status.Value);

                var subscription = item.Status.Subscribe((_, newValue) => OnNext(newValue));

                EventCallback<ChangeEvent<bool>> callback = null;
                if (manipulatable)
                {
                    callback = evt =>
                    {
                        item.SetStatus(evt.newValue, GetFilter(false));
                        OnNext(item.Status.Value);
                    };
                    toggle.RegisterValueChangedCallback(callback);
                }

                var disposable = Disposable.Create<object>(null, _ =>
                {
                    subscription.Dispose();
                    if (callback != null) toggle.UnregisterValueChangedCallback(callback);
                });

                // on undo / redo

                _bindings.Add(element, disposable);
            };

            nameColumn.unbindCell += (element, _) =>
            {
                if (_bindings.TryGetValue(element, out var disposable))
                {
                    disposable.Dispose();
                    _bindings.Remove(element);
                }
            };

            kindColumn.bindCell += (element, index) =>
            {
                var item = treeViewMain.GetItemDataForIndex<ITreeViewItem>(index);
                element.Q<Label>().text = item.Kind;
            };

            typeColumn.bindCell += (element, index) =>
            {
                var item = treeViewMain.GetItemDataForIndex<ITreeViewItem>(index);
                element.Q<Label>().text = item.Type;
            };

            parametersColumn.bindCell += (element, index) =>
            {
                var item = treeViewMain.GetItemDataForIndex<ITreeViewItem>(index);
                element.Q<Label>().text = item.IsDeprecated ? "Deprecated" : "";
            };

            generateButton.clicked += () =>
            {
                if (!profile) return;
                profile.Generate();
            };

            selectButton.clicked += () =>
            {
                if (!profile) return;
                EditorGUIUtility.PingObject(profile);
            };

            searchFieldMain.RegisterValueChangedCallback(evt =>
            {
                var newValue = evt.newValue;
                if (_searchText == newValue) return;
                _searchText = evt.newValue;
                ApplyCurrentFilter();
            });

            deprecatedToggle.value = showDeprecatedItems;

            deprecatedToggle.RegisterValueChangedCallback(evt =>
            {
                var newValue = evt.newValue;
                if (showDeprecatedItems == newValue) return;
                showDeprecatedItems = newValue;
                ApplyCurrentFilter();
            });

            Rebuild();
        }

        #endregion
    }
}
