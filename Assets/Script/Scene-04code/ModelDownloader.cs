using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Storage;
using Firebase;
using System.Threading;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System;
using UnityEngine.UI;




public class ModelDownloader : MonoBehaviour
{

    public Text DownloadingText;

    string url = "gs://arzoo-c2f52.appspot.com/Models";
    string local_url= "file:///local/models/";


    void Start()
    {

        DownloadingText.text = "START Called";

        string modelname = PlayerPrefs.GetString("CURRENTMODELNAME");
        url = url + "/" + modelname;
        local_url = local_url + "/" + modelname;

       
    

    }

    public String downloadFileNow()
    {
        if (System.IO.File.Exists(local_url))
        {
            DownloadingText.text = "File Loaded From Memory";
            return local_url;
        }
        else
        {
            DownloadingText.text = "DOWNLOADING MODEL... Wait";

          return DownloadFile(url);


        }

    }

    string  DownloadFile(String url)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storage_ref = storage.GetReferenceFromUrl(url);


        

        // Start downloading a file
        Task task = storage_ref.GetFileAsync(local_url,
          new Firebase.Storage.StorageProgress<DownloadState>((DownloadState state) => {

              DownloadingText.text= String.Format(
              "Progress: {0} of {1} bytes transferred.",
              state.BytesTransferred,
              state.TotalByteCount
               );

          }), CancellationToken.None);

        task.ContinueWith(resultTask => {
            if (!resultTask.IsFaulted && !resultTask.IsCanceled)
            {
                Debug.Log("Download finished.");
                DownloadingText.text = "DOWnload Finished";
               

            }
        });
        return local_url;


    }

    


    void Update()
    {
        
    }
}
