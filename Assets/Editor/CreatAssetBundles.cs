using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using static GameParameters;

public class CreatAssetBundles
{
    [MenuItem("Tharle/Builld AssetBundles")]
    static void BuildAllAssetBundles()
    {
        List<AssetBundleBuild> assetBundleDefinitionList = new();

        // FOR SFX
        {
            AssetBundleBuild ab = new();
            ab.assetBundleName = BundleNames.SFX;
            ab.assetNames = RecursiveGetAllAssetsInDirectory(BundlePath.BUNDLE_ASSETS + BundlePath.SFX).ToArray();
            assetBundleDefinitionList.Add(ab);
        }

        // SCRIPT OBJETS
        {
            AssetBundleBuild ab = new();
            ab.assetBundleName = BundleNames.SCRIT_OBJETS;
            ab.assetNames = RecursiveGetAllAssetsInDirectory(BundlePath.BUNDLE_ASSETS + BundlePath.SCRIT_OBJETS).ToArray();
            assetBundleDefinitionList.Add(ab);
        }

        // Create if not exist streaming Assets directory
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        // Build all bundles from 'BundleAssets' to streaming directory
        AssetBundleManifest manifest =  BuildPipeline.BuildAssetBundles(BundlePath.STREAMING_ASSETS, assetBundleDefinitionList.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        
        // Look at the results
        if (manifest != null)
        {
            foreach (var bundleName in manifest.GetAllAssetBundles())
            {
                string projectRelativePath = BundlePath.STREAMING_ASSETS + "/" + bundleName;
                Debug.Log($"Size of AssetBundle {projectRelativePath} is {(float)new FileInfo(projectRelativePath).Length / (1024)} KB");
            }
        }
        else
        {
            Debug.Log("Build failed, see Console and Editor log for details");
        }
    }

    static List<string> RecursiveGetAllAssetsInDirectory(string path)
    {
        List<string> assets = new();
        // "Assets/BundleAssets/Sounds"
        foreach (string asset in Directory.GetFiles(path))
                assets.Add(asset);
        return assets;
    }
}
