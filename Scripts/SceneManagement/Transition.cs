using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

using ZAM.Control;

namespace ZAM.SceneManagement
{
    public class Transition : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E, F, G, H, I, K
        }

        // Assigned Variables \\
        [SerializeField] private int _sceneLoadIndex = -1;
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] DestinationIdentifier destination;

        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;

        // Setup Variables \\
        PlayerController player = null;
        //GameObject playerPrevious = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") 
            {
                player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                StartCoroutine(ZoneChange()); 
            }
        }

        private void UpdatePlayer(Transition otherZone)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            
            // player.GetComponent<NavMeshAgent>().enabled = false;
            // player.transform.position = spawnPoint.position;
            // player.GetComponent<NavMeshAgent>().enabled = true;
            player.GetComponent<NavMeshAgent>().Warp(otherZone.spawnPoint.position);

            player.transform.rotation = otherZone.spawnPoint.rotation;
        }

        private Transition GetExitZone()
        {
            foreach (Transition transition in FindObjectsOfType<Transition>())
            {
                if (transition == this) continue;
                if (transition.destination != destination) continue;

                return transition;
            }
            Debug.LogError("No portals found.");
            return null;
        }

        private IEnumerator ZoneChange()
        {
            if (_sceneLoadIndex < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();

            // playerPrevious = GameObject.FindWithTag("Player");
            // playerPrevious.GetComponent<PlayerController>().enabled = false;
            player.StopAllControl();
            player.enabled = false;

            yield return fader.FadeOut(fadeOutTime);
            
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save(wrapper.GetAutoSaveFile());

            yield return SceneManager.LoadSceneAsync(_sceneLoadIndex);

            wrapper.Load(wrapper.GetAutoSaveFile());

            Transition otherZone = GetExitZone();
            UpdatePlayer(otherZone);

            wrapper.Save(wrapper.GetAutoSaveFile());

            player.enabled = false;

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            player.enabled = true;

            Destroy(gameObject);
        }
    }
}