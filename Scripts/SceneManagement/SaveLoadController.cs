using UnityEngine;
using UnityEngine.InputSystem;

namespace ZAM.SceneManagement
{
    public class SaveLoadController : MonoBehaviour, InputManager.ISaveSystemActions
    {
        // Setup Variables \\
        InputManager saveLoadInput;
        SavingWrapper saveWrapper;
        Fader fader;
        GameObject player;

        // Adjustable Variables \\
        //private float fadeTime = 0.5f;

        // Base Methods - Unity \\
        private void Awake()
        {
            saveLoadInput = new InputManager();
            saveLoadInput.SaveSystem.SetCallbacks(this);

            saveWrapper = GetComponent<SavingWrapper>();
            fader = FindObjectOfType<Fader>();
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            saveLoadInput.SaveSystem.Enable();
        }

        private void OnDisable()
        {
            saveLoadInput.SaveSystem.Disable();
        }

        // Player Controls \\
        public void OnSave(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Debug.Log("File saving!");
                saveWrapper.Save();
            }
        }

        public void OnLoad(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Debug.Log("File Loading...");
                saveWrapper.Load();
            }

        }

        public void OnDelete(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Debug.Log("Deleting file...");
                saveWrapper.Delete();
            }
        }
    }
}