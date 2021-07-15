using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver=false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && isGameOver == true)
        {
            Application.LoadLevel(Application.loadedLevel);//current scene
        }
    }
    public void GameOver()
    {
        isGameOver = true;
    }
}
