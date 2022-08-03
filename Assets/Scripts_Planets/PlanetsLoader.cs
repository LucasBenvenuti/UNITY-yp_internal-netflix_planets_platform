using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class PlanetsLoader : MonoBehaviour
{
    //     [DllImport("__Internal")]
    //     private static extern bool IsMobile();

    //     [DllImport("__Internal")]
    //     private static extern void UpdateProgress(float progress);

    //     [DllImport("__Internal")]
    //     private static extern void FinishedLoading();

    //     [DllImport("__Internal")]
    //     private static extern string GetEnvironment();

    //     [System.Serializable]
    //     public struct PlanetData
    //     {
    //         public Transform parent;
    //         public string templateName;
    //     }

    //     [SerializeField]
    //     PlanetData[] planetsData;

    //     [SerializeField]
    //     float interval = .2f;


    //     //LOCALHOST

    //     string local_assetBundleNameDesktop = "https://localhost:9002/assets/globalbundle";
    //     string local_assetBundleNameMobile = "https://localhost:9002/assets/globalbundlemobile";

    //     //DEV
    //     [SerializeField]
    //     string dev_assetBundleNameDesktop = "https://cdn-dev-netflix-gaming-app.s3-sa-east-1.amazonaws.com/assets/globalbundle";
    //     string assetBundleNameDesktopEditor = "globalbundlemobile";
    //     // string assetBundleNameDesktopEditor = "globalbundlemobile";

    //     [SerializeField]
    //     string dev_assetBundleNameMobile = "https://cdn-dev-netflix-gaming-app.s3-sa-east-1.amazonaws.com/assets/globalbundlemobile";
    //     string assetBundleNameMobileEditor = "globalbundle";


    //     //STG
    //     [SerializeField]
    //     string stg_assetBundleNameDesktop = "https://cdn-stg-netflix-gaming-app.s3-sa-east-1.amazonaws.com/assets/globalbundle";
    //     [SerializeField]
    //     string stg_assetBundleNameMobile = "https://cdn-stg-netflix-gaming-app.s3-sa-east-1.amazonaws.com/assets/globalbundlemobile";

    //     //PROD
    //     [SerializeField]
    //     string prod_assetBundleNameDesktop = "https://cdn-prod-netflix-gaming-app.s3.amazonaws.com/assets/globalbundle";
    //     [SerializeField]
    //     string prod_assetBundleNameMobile = "https://cdn-prod-netflix-gaming-app.s3.amazonaws.com/assets/globalbundlemobile";

    //     [SerializeField]
    //     string local_assetVersion = "https://localhost:9002/assets/version";

    //     [SerializeField]
    //     string dev_assetVersion = "https://cdn-dev-netflix-gaming-app.s3-sa-east-1.amazonaws.com/assets/version";


    //     //STG
    //     [SerializeField]
    //     string stg_assetVersion = "https://cdn-stg-netflix-gaming-app.s3-sa-east-1.amazonaws.com/assets/version";

    //     //PROD
    //     [SerializeField]
    //     string prod_assetVersion = "https://cdn-prod-netflix-gaming-app.s3.amazonaws.com/assets/version";

    //     string versionUrl
    //     {
    //         get
    //         {
    // #if !UNITY_EDITOR && UNITY_WEBGL

    //             if(GetEnvironment() == "dev")
    //             {
    //                 return dev_assetVersion;
    //             }
    //             else if(GetEnvironment() == "stg")
    //             {
    //                 return stg_assetVersion;
    //             }
    //             else if(GetEnvironment() == "production")
    //             {
    //                 return prod_assetVersion;
    //             }
    //             else if(GetEnvironment() == "localhost")
    //             {
    //                 return local_assetVersion;
    //             }

    // #endif
    //             return dev_assetVersion;
    //         }
    //     }
    //     string assetBundleName
    //     {
    //         get
    //         {
    // #if !UNITY_EDITOR && UNITY_WEBGL
    //             Debug.Log(GetEnvironment());
    //             if(IsMobile())
    //             {
    //                 if(GetEnvironment() == "dev")
    //                 {
    //                     return dev_assetBundleNameMobile;
    //                 }
    //                 else if(GetEnvironment() == "stg")
    //                 {
    //                     return stg_assetBundleNameMobile;
    //                 }
    //                 else if(GetEnvironment() == "production")
    //                 {
    //                     return prod_assetBundleNameMobile;
    //                 }
    //                 else if(GetEnvironment() == "localhost")
    //                 {
    //                     return local_assetBundleNameMobile;
    //                 }
    //             }
    //             else
    //             {
    //                 if(GetEnvironment() == "dev")
    //                 {
    //                     return dev_assetBundleNameDesktop;
    //                 }
    //                 else if(GetEnvironment() == "stg")
    //                 {
    //                     return stg_assetBundleNameDesktop;
    //                 }
    //                 else if(GetEnvironment() == "production")
    //                 {
    //                     return prod_assetBundleNameDesktop;
    //                 }
    //                 else if(GetEnvironment() == "localhost")
    //                 {
    //                     return local_assetBundleNameDesktop;

    //                 }
    //             }
    // #endif
    //             return dev_assetBundleNameDesktop;
    //         }
    //     }


    //     static AssetBundle globalBundle;

    //     // Start is called before the first frame update
    //     void Start()
    //     {
    // #if !UNITY_EDITOR
    //         Debug.Log("Environment = " + GetEnvironment());
    // #endif
    //         StartCoroutine(SparseInitialization());
    //     }

    //     // Update is called once per frame
    //     void Update()
    //     {

    //     }

    //     void ReceiveBundle(AssetBundle bundle, int index)
    //     {
    //         globalBundle = bundle;

    //         if (globalBundle == null)
    //         {
    //             Debug.Log("Failed to load AssetBundle!");
    //             return;
    //         }
    //         GameObject prefab = globalBundle.LoadAsset<GameObject>(planetsData[index].templateName);
    //         Instantiate(prefab, planetsData[index].parent.position, planetsData[index].parent.rotation, planetsData[index].parent);

    //     }
    //     IEnumerator SparseInitialization()
    //     {
    //         for (int i = 0; i < planetsData.Length; i++)
    //         {

    //             yield return new WaitForSeconds(interval);

    //             if (globalBundle == null)
    //             {
    //                 Debug.Log("Streaming Assets Path: " + Application.streamingAssetsPath);
    //                 int version = 1;
    //                 UnityWebRequest w = UnityWebRequest.Get(versionUrl);
    //                 yield return w.SendWebRequest();

    //                 if (w.isNetworkError || w.isHttpError)
    //                 {
    //                     Debug.Log(w.error);
    //                 }
    //                 else
    //                 {
    //                     // Show results as text
    //                     Debug.Log(w.downloadHandler.text);

    //                     // Or retrieve results as binary data
    //                     string versionString = w.downloadHandler.text;
    //                     Debug.Log("Version: " + versionString);
    //                     version = int.Parse(versionString);
    //                 }
    // #if UNITY_EDITOR
    //                 ReceiveBundle(AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleNameDesktopEditor)), i);
    //                 // while (!Caching.ready)
    //                 // yield return null;

    //                 // using (var www = WWW.LoadFromCacheOrDownload(assetBundleName, 0))
    //                 // {
    //                 //     yield return www;
    //                 //     if (!string.IsNullOrEmpty(www.error))
    //                 //     {
    //                 //         Debug.Log(www.error);
    //                 //         yield return null;
    //                 //     }
    //                 //     ReceiveBundle(www.assetBundle, i);

    //                 // }
    // #endif
    // #if !UNITY_EDITOR && UNITY_WEBGL
    // //Streaming Assets
    //                 // string path = Path.Combine(Application.streamingAssetsPath, "globalbundle");
    //                 // Debug.Log("Download path: " + path);
    //                 // UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
    //                 // yield return request.SendWebRequest();

    //                 // if (request.isNetworkError || request.isHttpError)
    //                 // {
    //                 //     Debug.LogError(request.error);
    //                 // }
    //                 // else
    //                 // {
    //                 //     // Get downloaded asset bundle
    //                 //     ReceiveBundle(DownloadHandlerAssetBundle.GetContent(request), i);
    //                 // }

    // //S3
    //                 while (!Caching.ready)
    //                 yield return null;

    //                 Debug.Log(assetBundleName);

    //                 using (var www = WWW.LoadFromCacheOrDownload(assetBundleName, version))
    //                 {
    //                     while (!www.isDone)
    //                     {
    //                         UpdateProgress(www.progress);
    //                         yield return null;
    //                     }
    //                     yield return www;
    //                     if (!string.IsNullOrEmpty(www.error))
    //                     {
    //                         Debug.Log(www.error);
    //                         yield return null;
    //                     }



    //                     ReceiveBundle(www.assetBundle, i);

    //                 }
    // #endif
    //             }
    //             else
    //             {
    //                 ReceiveBundle(globalBundle, i);
    //             }



    //             //lucio costa 9550
    //         }
    // #if UNITY_WEBGL && !UNITY_EDITOR
    //         FinishedLoading();
    //         StartSceneBttn.instance.StartTutorial();
    // #endif
    //         yield return null;
    //         StartSceneBttn.instance.StartTutorial();

    //     }

}
