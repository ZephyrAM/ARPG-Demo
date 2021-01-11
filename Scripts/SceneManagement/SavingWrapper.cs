using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using ZAM.Saving;
using ZAM.Control;

namespace ZAM.SceneManagement
{
    public class SavingWrapper : MonoBehaviour, InputManager.ISaveSystemActions
    {
        // Setup Variables \\
        InputManager controlInput;
        SavingSystem saveSystem;
        
        Fader fader;
        GameObject player;

        // Adjustable Variables \\
        private string _defaultSaveFile = "SaveFile";
        private string _zoneAutoSave = "AutoSave";

        private float fadeTime = 0.25f;

        // Base Methods - Unity \\
        private void Awake()
        {
            controlInput = new InputManager();
            controlInput.SaveSystem.SetCallbacks(this);

            saveSystem = GetComponent<SavingSystem>();
            fader = FindObjectOfType<Fader>();
        }

        private void OnEnable()
        {
            controlInput.SaveSystem.Enable();
        }

        private void OnDisable()
        {
            controlInput.SaveSystem.Disable();
        }

        // Save/Load Methods \\
        public void Save()
        {
            saveSystem.Save(_defaultSaveFile);
        }

        public void Load()
        {
            StartCoroutine(FadeOnLoadCycle(_defaultSaveFile));
        }

        public void Delete()
        {
            saveSystem.Delete(_defaultSaveFile);
        }

        public void Save(string fileName)
        {
            saveSystem.Save(fileName);
        }

        public void Load(string fileName)
        {
            saveSystem.Load(fileName);
        }

        public void Delete(string filename)
        {
            saveSystem.Delete(filename);
        }

        public IEnumerator SceneLoad()
        {
            yield return saveSystem.LoadLastScene(_defaultSaveFile);
        }

        // Get/Set Methods \\
        public string GetAutoSaveFile()
        {
            return _zoneAutoSave;
        }

        private IEnumerator FadeOnLoadCycle(string fileName)
        {
            DontDestroyOnLoad(gameObject);
            yield return fader.FadeOut(fadeTime);

            yield return saveSystem.LoadLastScene(fileName);

            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;

            yield return new WaitForSeconds(fadeTime);

            yield return fader.FadeIn(fadeTime);

            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
        }

        // Player Controls \\
        public void OnSave(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Debug.Log("File saving!");
                Save();
            }
        }

        public void OnLoad(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (!saveSystem.DoesSaveExist(_defaultSaveFile))
                {
                    Debug.Log("No file to load!");
                    return;
                }
                Debug.Log("File Loading!");
                Load();
            }
            
        }

        public void OnDelete(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (!saveSystem.DoesSaveExist(_defaultSaveFile))
                {
                    Debug.Log("No file to delete!");
                    return;
                }
                Debug.Log("Deleting file!");
                Delete();
            }
        }
    }
}
