using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Serializable]
    public class Room
    {
        
        public EnemyManager enemyManager = null;
        public Interactuble interactuble = null;
        public CameraTransition transitionToNextLevel = null;

        public void Start()
        {
            if (enemyManager != null)
            {
                foreach (var enemy in enemyManager.listOfEnemies)
                {
                    enemy.characterIsDead += CheckAndOpenTransition;
                }
            }
            if (interactuble != null)
            {
                interactuble.Interacted += OpenTransition;
            }
            transitionToNextLevel?.gameObject.SetActive(false);

        }
        void CheckAndOpenTransition()
        {
            if (enemyManager.CheckIfAllEnemiesAreDead())
            {
                OpenTransition();
            }
        }
        void OpenTransition()
        {
            transitionToNextLevel?.gameObject.SetActive(true);
        }

    }
    public static LevelManager Instance { get; private set; }

    public CameraRails gameCamera = null;

    public List<Room> rooms = new List<Room>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        rooms.ForEach((a) =>
        {
            a.Start();
        });
        gameCamera = Camera.main.gameObject.GetComponent<CameraRails>();
    }
}
