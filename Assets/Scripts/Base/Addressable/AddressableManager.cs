using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using Base.SceneTransition;
using System.Collections.Generic;
using System;

namespace Base.Addressable
{
    public static class AddressableManager
    {
        private static AsyncOperationHandle<T> LoadAddressableLogic<T>(Func<AsyncOperationHandle<T>> loadFunc, string errorMessage, Action<T> onSuccess, Action onFail)
        {
            if (SceneTransitionManager.sceneChanging)
            {
                Debug.LogWarning("Scene transition in progress. New loads are cancelled.");
                return default;
            }

            AsyncOperationHandle<T> asyncOperation = loadFunc();
            SceneTransitionManager.ongoingOperations.Add(asyncOperation);

            asyncOperation.Completed += operation =>
            {
                if (SceneTransitionManager.ongoingOperations.Contains(operation))
                {
                    SceneTransitionManager.ongoingOperations.Remove(operation);
                    if (operation.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.LogError($"{errorMessage}\n{operation.OperationException}");
                        onFail?.Invoke();
                    }
                    else
                    {
                        onSuccess?.Invoke(operation.Result);
                    }
                }
                else
                {
                    Debug.LogError("Addressable load completed after scene transition.");
                    onFail?.Invoke();
                }
            };

            return asyncOperation;
        }

        public static AsyncOperationHandle<T> LoadAddressableAssetAsync<T>(object addressableReference, Action<T> onSuccess = null, Action onFail = null) where T : class
        {
            if (addressableReference == null)
            {
                Debug.LogError("Addressable reference is null! Cannot load the asset.");
                onFail?.Invoke();
                return default;
            }

            return LoadAddressableLogic(() => Addressables.LoadAssetAsync<T>(addressableReference), $"Error loading asset: {addressableReference}", onSuccess, onFail);
        }

        public static AsyncOperationHandle<IList<T>> LoadAddressableAssetsAsync<T>(AddressableLabelNames labelName, Action<IList<T>> onSuccess = null, Action onFail = null) where T : class
        {
            return LoadAddressableLogic(() => Addressables.LoadAssetsAsync<T>(labelName.ToString(), null), $"Error loading assets with label: {labelName}", onSuccess, onFail);
        }

        public static void ReleaseAsset(AsyncOperationHandle operation)
        {
            if (operation.IsValid())
            {
                Addressables.Release(operation);
            }
        }
    }
}