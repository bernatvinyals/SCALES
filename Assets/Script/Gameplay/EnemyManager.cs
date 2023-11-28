using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyController> listOfEnemies;


    private void Start()
    {
        DeactivateEnemies();
    }
    public void ActivateEnemies()
    {
        for (int i = 0; i < listOfEnemies.Count; i++)
        {
            if (listOfEnemies[i] != null)
            {
                listOfEnemies[i].SetState(CharacterController.CharacterSTATES.IDLE);
            }
        }
    }
    public void DeactivateEnemies() { 
        for (int i = 0; i < listOfEnemies.Count; i++)
        {
            if (listOfEnemies[i] != null)
            {
                listOfEnemies[i].SetState(CharacterController.CharacterSTATES.DISABLED);
            }
        }
    }


    public bool CheckIfAllEnemiesAreDead()
    {
        int count = 0;
        for(int i = 0;i < listOfEnemies.Count; i++)
        {
            if (listOfEnemies[i] == null)
            {
                count += 1;
            }
            else if(listOfEnemies[i].GetState() == CharacterController.CharacterSTATES.DEAD)
            {
                count += 1;
            }
        }
        return count == listOfEnemies.Count;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            ActivateEnemies();
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            DeactivateEnemies();
        }
    }
}
