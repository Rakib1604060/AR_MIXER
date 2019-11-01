using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{


    public InputField emailField, ageField, passwordField;
    public Button loginButton;
    FirebaseAuth auth;
    public GameObject alertDialog;
    

    void Start()
    {
      auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    
    }

    void Update()
    {

        if (emailField.text == "" || passwordField.text == "" || ageField.text == "")
        {
          

        }

    }

    // when some one press login button;

    public void onLoginButtonclick()
    {
         string email = emailField.text.ToString();
         string password = passwordField.text.ToString();
         string age = ageField.text.ToString();
         PlayerPrefs.SetString("PLAYERAGE",age);
         SignInUser(email,password);
         Debug.LogError(email);
      

    }

    void SignInUser(string email,string password)
    {


        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            PlayerPrefs.SetString("SIGNEDIN","TRUE");
            UpdateUi(newUser.Email,newUser.UserId);
           
        });



    }

    private void UpdateUi(string email,string userid)
    {
        Debug.LogFormat("update ui called");
        alertDialog.SetActive(true);
        String[] username = email.Split('@');
        PlayerPrefs.SetString("USERNAME",username[0]);

        Text alerttext = alertDialog.GetComponentInChildren<Text>();
        alerttext.text = "Sign In Successfull . Thank You for Using Our App";
        Button okbutton = alertDialog.GetComponentInChildren<Button>();
        okbutton.onClick.AddListener(() => {
            alertDialog.SetActive(false);
            SceneManager.LoadScene(0);
          
        });
        
  
    }
}
