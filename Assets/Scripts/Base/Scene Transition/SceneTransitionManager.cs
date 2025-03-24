using Base.Addressable;
using Base.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Base.SceneTransition
{
    public sealed class SceneTransitionManager : PersistentSingleton<SceneTransitionManager>
    {
        [SerializeField] private AssetReferenceT<SceneTransitionBehaviour> sceneTransitionPrefab; //onvalidate

        public static List<AsyncOperationHandle> ongoingOperations = new();
        public static bool sceneChanging;

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            sceneChanging = false;
        }

        public void ChangeScene(SceneNames sceneName)
        {
            sceneChanging = true;
            foreach (AsyncOperationHandle operation in ongoingOperations)
            {
                AddressableManager.ReleaseAsset(operation);
            }
            ongoingOperations.Clear();
            SceneManager.LoadScene(sceneName.ToString());
            //transform child yap prefabi
        }
    }
}