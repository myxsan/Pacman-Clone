using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{

    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Space))
       {
           SceneManager.LoadScene(1);
       } 
    }
}
