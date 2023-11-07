using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        foreach (EnemyController enemy in listOfEnemies)
        {
            if (enemy == null) return false;
            if (enemy?.GetState() == CharacterController.CharacterSTATES.DEAD) return true;
        }
        return false;
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
