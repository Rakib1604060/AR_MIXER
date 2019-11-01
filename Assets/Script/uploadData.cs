using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase;

using Firebase.Unity.Editor;

public class uploadData : MonoBehaviour
{
    public InputField modelnametext, modelsizetext;
    public Button submitButton;


  
    DatabaseReference reference;
    int counter = 0;

    void Start()
    {
        

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://arzoo-c2f52.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;



        submitButton.onClick.AddListener(()=>
        {
            SendData();

        });
    }

    private void SendData()
    {
        counter++;
        string name = modelnametext.text;
        string size = modelsizetext.text;
        
        Animal modemyl = new Animal(name,size);
        string myjson = JsonUtility.ToJson(modemyl);
        Debug.LogFormat("MDEL",modemyl);

         reference.Child("Animals").Child(counter+"").Child("name").SetValueAsync(name);
         reference.Child("Animals").Child(counter+"").Child("size").SetValueAsync(size);
      
        Debug.LogFormat(" UPDATED ");
       


       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
