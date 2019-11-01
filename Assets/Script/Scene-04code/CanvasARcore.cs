using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CanvasARcore : MonoBehaviour
{

    public Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(()=>
        {
            SceneManager.LoadScene(0);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
