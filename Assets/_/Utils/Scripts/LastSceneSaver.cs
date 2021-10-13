using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastSceneSaver : MonoBehaviour
{
    private Dictionary<string, object> levelstart = new Dictionary<string, object>() { };
    public Dictionary<string, object> levelfinish = new Dictionary<string, object>() { };

    private void Awake(){
        PlayerPrefs.SetInt("lastLevel", SceneManager.GetActiveScene().buildIndex);

        levelstart.Add("level_number", SceneManager.GetActiveScene().buildIndex);
        levelstart.Add("level_name", SceneManager.GetActiveScene().name);
        levelstart.Add("level_count", SceneManager.GetActiveScene().buildIndex);
        levelstart.Add("level_diff", "easy");
        levelstart.Add("level_loop", 1);
        levelstart.Add("level_random", 0);
        levelstart.Add("level_type", "normal");
        levelstart.Add("game_mode", "normal");
        Invoke("level_start", 1);

        levelfinish.Add("level_number", SceneManager.GetActiveScene().buildIndex);
        levelfinish.Add("level_name", SceneManager.GetActiveScene().name);
        levelfinish.Add("level_count", SceneManager.GetActiveScene().buildIndex);
        levelfinish.Add("level_diff", "easy");
        levelfinish.Add("level_loop", 1);
        levelfinish.Add("level_random", 0);
        levelfinish.Add("level_type", "normal");
        levelfinish.Add("game_mode", "normal");
        levelfinish.Add("time", 1);
        levelfinish.Add("progress", SceneManager.GetActiveScene().buildIndex);
        levelfinish.Add("continue", 0);
    }

    void level_start()
    {
        //AppMetrica.Instance.ReportEvent("level_start", levelstart);
    }

    private void OnDestroy()
    {
        //AppMetrica.Instance.ReportEvent("level_finish", levelfinish);
    }
}
