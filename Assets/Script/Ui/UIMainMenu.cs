using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public List<Button> menuItemsHideInGame;
    public List<Button> menuItemsHideMenu;

    public GameObject backgroundGFX;
    private void Start()
    {
        Show();
        UiManager.Instance.gamePaused += ToggleView;
    }
    public void Show()
    {
        UiManager.Instance.ShowCursor();
        this.gameObject.SetActive(true);

        ToggleStates();
    }
    public void Hide()
    {
        UiManager.Instance.HideCursor();
        this.gameObject.SetActive(false);
    }
    public void ToggleView()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        if (this.gameObject.activeSelf)
        {
            UiManager.Instance.ShowCursor();
        }
        else
        {
            UiManager.Instance.HideCursor();
        }
        ToggleStates();
    }
    public void ToggleStates()
    {
        
        bool a = UiManager.Instance.isOnGameplay;
        backgroundGFX.SetActive(!a);
        foreach (var item in menuItemsHideInGame)
        {
            //Items that will be hidden during gameplay
            item.gameObject.SetActive(!a);
        }
        foreach (var item in menuItemsHideMenu)
        {
            //Items that will be hidden if theres no gameplay, ie Main Menu
            item.gameObject.SetActive(a);
        }
    }



    public void StartNewGame()
    {
        UiManager.Instance.ChangeScene(3,false);
        Hide();
    }
    public void RestartGame()
    {
        UiManager.Instance.RestartScene();
        Hide();

    }

    public void QuitApp()
    {
        UiManager.Instance.QuitApp();
    }
}
