using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
#if UNITY_EDITOR
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.UncompressedAssetBundle,
        BuildTarget.WebGL);
        //BuildTarget.StandaloneWindows64);
    }
#endif
}