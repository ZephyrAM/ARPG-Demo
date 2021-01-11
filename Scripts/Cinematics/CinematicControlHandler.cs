using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

using ZAM.Control;

namespace ZAM.Cinematics
{
    public class CinematicControlHandler : MonoBehaviour, InputManager.ICinematicControlActions
    {
        // Adjustable Variables \\
        private bool _isPlaying = false;

        // Setup Variables \\
        GameObject player;
        InputManager cinematicInput;
        PlayableDirector playable;

        // Base Methods - Unity \\
        private void Awake()
        {
            cinematicInput = new InputManager();
            cinematicInput.CinematicControl.SetCallbacks(this);

            playable = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            cinematicInput.CinematicControl.Enable();
            playable.played += DisableControl;
            playable.stopped += EnableControl;
        }

        private void OnDisable()
        {
            cinematicInput.CinematicControl.Disable();
            playable.played -= DisableControl;
            playable.stopped -= EnableControl;
        }

        // Cinematic Control Handling \\
        private void DisableControl(PlayableDirector play)
        {
            player.GetComponent<PlayerController>().StopAllControl();
            player.GetComponent<PlayerController>().enabled = false;
            _isPlaying = true;
        }

        private void EnableControl(PlayableDirector stop)
        {
            player.GetComponent<PlayerController>().enabled = true;
            _isPlaying = false;
        }

        public void OnEscapeAction(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (_isPlaying)
                {
                    playable.Stop();
                }
            }
        }
    }
}