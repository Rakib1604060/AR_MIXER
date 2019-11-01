using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LoadAnimals : MonoBehaviour
{
    public GameObject prefab;
    DatabaseReference reference;
    public Button backButton;

    List<Animal> animalList = new List<Animal>();


    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://arzoo-c2f52.firebaseio.com/");
        backButton.onClick.AddListener(() => { SceneManager.LoadScene(0); });
        //load Animals name from FIREBASE REALTIME DB
        LoadData();
    }



    private void LoadData()
    {

        reference = FirebaseDatabase.DefaultInstance.GetReference("Animals");
        reference.KeepSynced(true);
        
        reference
       .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
        {
            if (e2.DatabaseError != null)
            {
                return;
            }
            else
            {
                if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
                {
                    foreach (var childSnapshot in e2.Snapshot.Children)
                    {

                        GameObject button = Instantiate(prefab,transform) as GameObject;
                       
                        string size= childSnapshot.Child("size").Value.ToString();
                        string name= childSnapshot.Child("name").Value.ToString();

                        Text btnText = button.GetComponentInChildren<Text>();
                        btnText.text = name;
                        btnText.fontSize = 30;
                        
                        

                        button.GetComponent<Button>().onClick.AddListener(()=> {

                          PlayerPrefs.SetString("CURRENTMODELNAME",name);
                           PlayerPrefs.SetString("CURRENTMODELSIZE",size);

                           SceneManager.LoadScene(4);

                        });


                      
                    }
                }

            }

        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
