using DroneRacerFpv.Controller;
using DroneRacerFpv.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DroneRacerFpv.Match
{
    public class GameManager : MonoBehaviour
    {
        static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        List<Transform> slots = new List<Transform>();

        GameObject droneRacerGO;

        bool started;

        void Awake()
        {
            //Debug.Log("Start");

            InitSettings.Initialize();

            if (instance == null)
            {
                instance = this;

                FindStartPositions();

                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("GameManager has already been created.");
            }
        }

        void FindStartPositions()
        {
            GameObject startPositions = null;

            startPositions = GameObject.Find(Constants.StartPositionsName);

            foreach (Transform child in startPositions.transform)
            {
                slots.Add(child);
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        void HandleStartGame()
        {
            droneRacerGO = DroneRacerSpawner.Spawn();

            droneRacerGO.SetActive(true);

            started = true;
        }

        Transform GetStartPos()
        {
            Transform pos = null;

            pos = slots[0];

            return pos;
        }

        public static bool IsReady()
        {
            return instance != null && instance.started == true;
        }

        public static Transform GetStartPosition()
        {
            Transform pos = null;

            if (IsReady() == true)
            {
                pos = instance.GetStartPos();
            }

            return pos;
        }

        public static void StartGame()
        {
            if (instance != null)
            {
                instance.gameObject.SetActive(true);
                instance.HandleStartGame();
            }
        }

        public static void StopGame()
        {
            if (instance != null)
            {
                instance.gameObject.SetActive(false);

                if (instance.droneRacerGO != null)
                {
                    GameObject.Destroy(instance.droneRacerGO);
                    instance.droneRacerGO = null;
                }
            }
        }
    }
}