using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class HomeScript : MonoBehaviour
{
    public Button animalButton, digitButton, alphabetButton, exitButton, soundButton,testButton;

    public AudioSource backgruondmusic;
    public Text welcometext;
    FirebaseAuth auth;
    FirebaseUser user;
    public Button logoutbtn;


    [SerializeField]
    private GameObject alertdialog;




    void Start()
    {

        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        if (user == null)
        {
           
            SceneManager.LoadScene(1);
        }
        else
        {
            welcometext.text = "Welcome " + PlayerPrefs.GetString("USERNAME",null);
        }

     



        //addlistener to buttons
        testButton.onClick.AddListener(()=> {


        });
        animalButton.onClick.AddListener(() =>{

            SceneManager.LoadScene(2);


        });
        digitButton.onClick.AddListener(() => {



        });

        animalButton.onClick.AddListener(() => {

            Logoutnow();


        });

        alphabetButton.onClick.AddListener(() => {

            

        });
        exitButton.onClick.AddListener(() => {

            Application.Quit();

        });

        soundButton.onClick.AddListener(()=> {
            if (backgruondmusic.isPlaying)
            {
                backgruondmusic.Stop();
                soundButton.GetComponentInChildren<Text>().text="Sound OFF";
            }
            else
            {
                backgruondmusic.Play();
                soundButton.GetComponentInChildren<Text>().text = "Sound ON";
            }
        });

    }

    private void Logoutnow()
    {
     
     
        alertdialog.SetActive(true);
       
        PlayerPrefs.SetString("USERNAME", null);

        Text alerttext = alertdialog.GetComponentInChildren<Text>();
        alerttext.text = "Do You Really Want to logout?";
        Button okbutton = alertdialog.GetComponentInChildren<Button>();
        okbutton.onClick.AddListener(() => {
            alertdialog.SetActive(false);
            auth.SignOut();
            Application.Quit();

        });

    }
}
