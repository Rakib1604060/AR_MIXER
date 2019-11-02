using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;


public class ModelController: MonoBehaviour
{
    private DetectedPlane detectedPlane;
    private GameObject modelPrefab;
    private GameObject modelInstance;
    public GameObject tempmodel;
    public GameObject pointer;
    public Camera firstPersonCamera;
    //public ModelDownloader downloader;
    private string modelurl;

    public Text DownloadingText;



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
        if (modelInstance != null)
        {
            DestroyImmediate(modelInstance);
        }

        DownloadingText.text = "SPAWN Called";

        Vector3 pos = detectedPlane.CenterPose.position;

        // Not anchored, it is rigidbody that is influenced by the physics engine.
        modelInstance = Instantiate(tempmodel, pos,
                Quaternion.identity, transform);

        // Pass the head to the slithering component to make movement work.
        GetComponent<Slithering>().Head = modelInstance.transform;


    }

    void Start()
    {

        DownloadingText.text = "START Called";
        modelurl = PlayerPrefs.GetString("CURRENTMODELURL", null);
        if (modelurl != null)
        {
            DownloadingText.text = modelurl;
            StartCoroutine(GetAssetBundle());
        }



    }

    /* IEnumerator GetAssetBundle()
      {
          UnityWebRequest www = new UnityWebRequest(modelurl);
          DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url,uint.MaxValue);
          www.downloadHandler = handler;

          yield return www.SendWebRequest();

          if (www.isNetworkError || www.isHttpError)
          {
              Debug.Log(www.error);
              DownloadingText.text =www.error;
          }
          else
          {
              // Extracts AssetBundle
              AssetBundle bundle = handler.assetBundle;
              string name = PlayerPrefs.GetString("CURRENTMODELNAME");
              modelPrefab = bundle.LoadAsset<GameObject>(name);

              DownloadingText.text = "Download SUccess";




          }


      }
      */
    IEnumerator GetAssetBundle()
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(modelurl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            DownloadingText.text = www.error;

        }
        else
        {
            DownloadingText.text ="SUCCESS FULL DOWNLOAD";
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            string name = PlayerPrefs.GetString("CURRENTMODELNAME");
            modelPrefab = bundle.LoadAsset<GameObject>(name);
        }
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
