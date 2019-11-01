using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using System;

public class ModelController: MonoBehaviour
{
    private DetectedPlane detectedPlane;
    private GameObject modelPrefab;
    private GameObject modelInstance;
    public GameObject pointer;
    public Camera firstPersonCamera;
    public ModelDownloader downloader;

   
    // Speed to move.
    public float speed = 20f;

    public void SetPlane(DetectedPlane plane)
    {
        detectedPlane = plane;
        // Spawn a new snake.
         SpawnModel();
    }



    private void SpawnModel()
    {
       string filepath=  downloader.downloadFileNow();
        if (System.IO.File.Exists(filepath))
        {
            string name = PlayerPrefs.GetString("CURRENTMODELNAME");
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(filepath, name));
            if (myLoadedAssetBundle == null)
            {
                return;
            }
            modelPrefab = myLoadedAssetBundle.LoadAsset<GameObject>(name);


        }
        else
        {
            return;
        }

       
        if (modelInstance != null)
        {
            DestroyImmediate(modelInstance);
        }

        Vector3 pos = detectedPlane.CenterPose.position;

        // Not anchored, it is rigidbody that is influenced by the physics engine.
        modelInstance = Instantiate(modelPrefab, pos,
                Quaternion.identity, transform);

        // Pass the head to the slithering component to make movement work.
        GetComponent<Slithering>().Head = modelInstance.transform;
    }

    void Start()
    {
        
    }

  
    void Update()
    {
        if (modelInstance == null || modelInstance.activeSelf == false)
        {
            pointer.SetActive(false);
            return;
        }
        else
        {
            pointer.SetActive(true);
        }




        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;

        if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
        {
            Vector3 pt = hit.Pose.position;
            //Set the Y to the Y of the snakeInstance
            pt.y = modelInstance.transform.position.y;
            // Set the y position relative to the plane and attach the pointer to the plane
            Vector3 pos = pointer.transform.position;
            pos.y = pt.y;
            pointer.transform.position = pos;

            // Now lerp to the position                                         
            pointer.transform.position = Vector3.Lerp(pointer.transform.position, pt,
              Time.smoothDeltaTime * speed);
        }


        float dist = Vector3.Distance(pointer.transform.position,
        modelInstance.transform.position) - 0.05f;
        if (dist < 0)
        {
            dist = 0;
        }

        Rigidbody rb = modelInstance.GetComponent<Rigidbody>();
        rb.transform.LookAt(pointer.transform.position);
        rb.velocity = modelInstance.transform.localScale.x *
            modelInstance.transform.forward * dist / .01f;



    }
}
