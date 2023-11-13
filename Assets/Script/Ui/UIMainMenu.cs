using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    private void Start()
    {
        Show();
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void StartNewGame()
    {
        UiManager.Instance.ChangeScene(3);
        Hide();

    }
    public void QuitApp()
    {
        UiManager.Instance.QuitApp();
    }
}
