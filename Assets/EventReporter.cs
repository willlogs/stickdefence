using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventReporter : MonoBehaviour
{
    private Dictionary<string, object> levelstart = new Dictionary<string, object>() { }
          ;
    public Dictionary<string, object> levelfinish = new Dictionary<string, object>() { }
    ;
    // Start is called before the first frame update
    void Start()
    {
        levelstart.Add("level_number", SceneManager.GetActiveScene().buildIndex + 1);
        levelstart.Add("level_name", "pyramids");
        levelstart.Add("level_count", SceneManager.GetActiveScene().buildIndex + 1);
        levelstart.Add("level_diff", "easy");
        levelstart.Add("level_loop", 1);
        levelstart.Add("level_random", 0);
        levelstart.Add("level_type", "normal");
        levelstart.Add("game_mode", "normal");

        levelfinish.Add("level_number", SceneManager.GetActiveScene().buildIndex + 1);
        levelfinish.Add("level_name", "pyramids");
        levelfinish.Add("level_count", SceneManager.GetActiveScene().buildIndex + 1);
        levelfinish.Add("level_diff", "easy");
        levelfinish.Add("level_loop", 1);
        levelfinish.Add("level_random", 0);
        levelfinish.Add("level_type", "normal");
        levelfinish.Add("game_mode", "normal");
        levelfinish.Add("result", "win");
        levelfinish.Add("time", 1);
        levelfinish.Add("progress", SceneManager.GetActiveScene().buildIndex);
        levelfinish.Add("continue", 0);

        AppMetrica.Instance.ReportEvent("level_start", levelstart);
    }
    public void ReportEvent(string name)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>() { };

        dic.Add("level_number", SceneManager.GetActiveScene().buildIndex + 1);
        dic.Add("level_name", "pyramids");
        dic.Add("level_count", SceneManager.GetActiveScene().buildIndex + 1);
        dic.Add("level_diff", "easy");
        dic.Add("level_loop", 1);
        dic.Add("level_random", 0);
        dic.Add("level_type", "normal");
        dic.Add("game_mode", "normal");
        dic.Add("result", "win");
        dic.Add("time", 1);
        dic.Add("progress", SceneManager.GetActiveScene().buildIndex);
        dic.Add("continue", 0);

        AppMetrica.Instance.ReportEvent(name, dic);
    }
    public void LevelFinish()
    {
        AppMetrica.Instance.ReportEvent("level_finish", levelfinish);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
