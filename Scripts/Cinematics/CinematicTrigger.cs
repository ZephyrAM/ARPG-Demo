using UnityEngine;
using UnityEngine.Playables;

using ZAM.Saving;

namespace ZAM.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        bool hasTriggered = false;

        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag != "Player") { return; }

            if (hasTriggered == false) 
            {
                GetComponent<PlayableDirector>().Play();
                hasTriggered = true;
                GetComponent<Collider>().enabled = false;
            }
            
        }

        // Save Component \\
        [System.Serializable]
        struct CinematicSaveData
        {
            public bool saveTrigger;
        }

        public object CaptureState()
        {
            CinematicSaveData data = new CinematicSaveData();
            data.saveTrigger = hasTriggered;

            return data;
        }

        public void RestoreState(object state)
        {
            CinematicSaveData data = (CinematicSaveData)state;
            hasTriggered = data.saveTrigger;
            GetComponent<Collider>().enabled = !hasTriggered;
        }
    }
}
