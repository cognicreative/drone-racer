using UnityEngine;

namespace DroneRacerFpv.Controller
{
    public class DroneRacerSpawner : MonoBehaviour
    {
        static DroneRacerSpawner instance;

        public GameObject droneRacerInScene;
        public GameObject droneRacerPrefab;

        void Awake()
        {
            instance = this;
        }

        GameObject HandleSpawn()
        {
            GameObject go;

            if (droneRacerInScene == null)
            {
                go = GameObject.Instantiate(droneRacerPrefab);
            }
            else
            {
                go = droneRacerInScene;
            }

            go.name = droneRacerPrefab.name;

            return go;
        }

        public static GameObject Spawn()
        {
            return instance.HandleSpawn();
        }
    }
}