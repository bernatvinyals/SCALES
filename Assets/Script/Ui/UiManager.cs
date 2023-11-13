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
    void Awake()
    {
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
            if (lastLoadedScene != 0)
            {
                AsyncOperation asyncunload = SceneManager.UnloadSceneAsync(lastLoadedScene);
            }
            lastLoadedScene = sceneToLoadIndex;

        }
    }
    public void ChangeScene(int index)
    {
        sceneToLoadIndex = index;
        loadingScreen.Show();
        StartCoroutine(LoadAsyncScene());
    }

    public void SetScreenInfo(CharacterController character)
    {

    }

    public void QuitApp()
    {
        Application.Quit();
    }




    private void OnGUI()
    {
        GUI.Label(new Rect(500, 10, 200, 200), "FPS: "+((int)(1.0f / Time.smoothDeltaTime)).ToString());
    }
}
