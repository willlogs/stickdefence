using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PT.Utils
{
    // scene 0 should be init scene, this scene manager circulates
    public class LevelManager : MonoBehaviour
    {
        bool _isPaused = false;

        public void Next()
        {
            int idx = SceneManager.GetActiveScene().buildIndex + 1;
            if(idx == SceneManager.sceneCountInBuildSettings)
            {
                idx = 1;
            }
            print("loading next : " + idx);

            SceneManager.LoadScene(idx);
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.01f;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void PauseGame()
        {
            _isPaused = true;
        }

        private void Update()
        {
            if (_isPaused)
            {
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0;
            }
        }
    }
}