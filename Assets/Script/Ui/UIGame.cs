using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    public UI_HP[] uihpList = null;
    public UnityEngine.UI.Text uiBullets = null;

    private PlayerController playerRef = null;
    
    bool firstFrame = true;
    private void Start()
    {
        UiManager.Instance?.SetGameplayStatus(true);
    }
    void Update()
    {
        if (firstFrame)
        {
            firstFrame = false;
            //each time that the player gets damage update the ui
            playerRef = UiManager.Instance.playerRef;
            playerRef.characterRecivedDamage += UpdateBar;
            playerRef.hasShoot += UpdateBar;
            UpdateBar();
        }
    }
    void UpdateBar()
    {   
        //Iterate on the hp logo list and if that index is less than the current
        //hp the player has show it, if its bigger, then hide it
        for (int i = 0; i < uihpList.Length; i++)
        {
            if (i <= playerRef.uiMaxHp * uihpList.Length )
            {
                uihpList[i]?.Show();
            }
            else
            {
                uihpList[i]?.Hide();
            }
        }

        //Update Bullets number
        uiBullets.text = playerRef.bullets.ToString();
    }
}
