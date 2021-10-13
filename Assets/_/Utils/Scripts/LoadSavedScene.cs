using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSavedScene : MonoBehaviour
{
    private void Start(){
        int bidx = -1;

        try{
            bidx = PlayerPrefs.GetInt("lastLevel", 1);
        }
        catch{

        }

        if(bidx == -1){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else{
            SceneManager.LoadScene(bidx);
        }
    }
}
