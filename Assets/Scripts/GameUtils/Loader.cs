using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public enum SceneName
{
    GameSceneFourByFour,
    Loading,
    MainMenuScene
}

public static class Loader
{
    class LoadingMono : MonoBehaviour { }

    static Action OnLoaderCallback;
    static AsyncOperation loadingAsyncOperation;

    public static void LoadSceneAsync(SceneName sceneName)
    {
        GameObject loadingGO = new GameObject();
        loadingGO.AddComponent<LoadingMono>().StartCoroutine(LoadSceneAsyncCoroutine(sceneName));


    }

    public static void LoadWithLoadingScene(SceneName sceneName)
    {
        OnLoaderCallback = () => 
        {
            GameObject loadingGO = new GameObject();
            loadingGO.AddComponent<LoadingMono>().StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
        };

        SceneManager.LoadScene(SceneName.Loading.ToString());

    }

    static IEnumerator LoadSceneAsyncCoroutine(SceneName sceneName)
    {
        yield return null;
        loadingAsyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());


        while (!loadingAsyncOperation.isDone)
        {
            Debug.Log(loadingAsyncOperation.progress);
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {

        return loadingAsyncOperation != null ? loadingAsyncOperation.progress : 1f;
    }

    public static void LoaderCallback()
    {
        if (OnLoaderCallback != null)
        {
            OnLoaderCallback();
            OnLoaderCallback = null;
        }
    }

}
