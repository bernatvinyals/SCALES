using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    
    public static UiManager Instance { get; private set; }

    public LoadingScreen loadingScreen;

    private int sceneToLoadIndex = 0;
    private int lastLoadedScene = 0;
    private bool unloadPrevious = true;
    public PlayerController playerRef { get; private set; }

    public delegate void GamePaused();
    public event GamePaused gamePaused;
    public bool isOnGameplay { get; private set; }

    void Awake()
    {
        isOnGameplay = false;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
        ChangeScene(1); // Load Main Menu

    }

    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadIndex,LoadSceneMode.Additive);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (asyncLoad.isDone)
        {
            loadingScreen.Hide();
            if (lastLoadedScene != 0 && unloadPrevious)
            {
                AsyncOperation asyncunload = SceneManager.UnloadSceneAsync(lastLoadedScene);
            }
            lastLoadedScene = sceneToLoadIndex;

        }
    }
    public void ChangeScene(int index, bool unload = true)
    {
        unloadPrevious = unload;
        sceneToLoadIndex = index;
        loadingScreen.Show();
        StartCoroutine(LoadAsyncScene());
    }

    public void SetPlayerRef(PlayerController character)
    {
        playerRef = character;
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void HideCursor()
    {
        Cursor.visible = false;
    }
    public void ShowCursor()
    {
        Cursor.visible = true;
    }

    public void SetGameplayStatus(bool status = false)
    {
        isOnGameplay = status;
    }

    public void PauseGame()
    {
        isOnGameplay = true;
        Cursor.visible = true;
        gamePaused?.Invoke();
    }
    public void RestartScene()
    {
        loadingScreen.Show();
        StartCoroutine(ReLoadAsyncScene());
    }
    private IEnumerator ReLoadAsyncScene()
    {
        AsyncOperation asyncunload = SceneManager.UnloadSceneAsync(lastLoadedScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncunload.isDone)
        {
            yield return null;
        }
        if (asyncunload.isDone)
        {
            loadingScreen.Hide();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadIndex, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(500, 10, 200, 200), "FPS: "+((int)(1.0f / Time.smoothDeltaTime)).ToString());
    }
}
