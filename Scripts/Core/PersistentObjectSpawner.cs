using UnityEngine;

namespace ZAM.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        // Setup Variables \\
        [SerializeField] GameObject persistentObjectPrefab = null;

        // Adjustable Variables \\
        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) { return; }
            Debug.Log("Spawner awake");
            SpawnPersistentObjects();
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);

            hasSpawned = true;
            Debug.Log("Spawning complete = " + hasSpawned);
        }
    }
}
