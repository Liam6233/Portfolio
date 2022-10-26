using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ChangeToMainMenu()
    {
       SceneManager.LoadScene("MainMenu");
    }
}
