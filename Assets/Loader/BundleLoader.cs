using UnityEngine;
using System.IO;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

public class BundleLoader: MonoBehaviour
{

    private static BundleLoader m_Instance;
    public static BundleLoader Instance 
    {
        get { 
            if (m_Instance == null) 
            {
                GameObject go = new GameObject("BundleLoader");
                m_Instance = go.AddComponent<BundleLoader>();
            } 

            return m_Instance;
        }
    }

    public T Load<T>(string bundleName, string assetName) where T : UnityEngine.Object
    {
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));

        if (localAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return default(T);
        }
        T originalAsset = localAssetBundle.LoadAsset<T>(assetName);
        if (originalAsset == null)
        {
            Debug.LogError($"Failed to load Asset with name {assetName} from the bundle {bundleName}!");
            return default(T);
        }

        T asset = Instantiate(originalAsset);
        asset.name = assetName;

        localAssetBundle.Unload(false);

        return asset;
    }

    public List<T> LoadAll<T>(string bundleName, bool IsCallUnload, params string[] assetNames) where T : UnityEngine.Object
    {
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        List<T> assets = new List<T>();

        if (localAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return assets;
        }

        if(assetNames.Length > 0)
        {
            foreach(string assetName in assetNames)
            {
                T asset = localAssetBundle.LoadAsset<T>(assetName);
                assets.Add(asset);
            }

        } else
        {
            assets = new List<T>(localAssetBundle.LoadAllAssets<T>());
        }


        if(IsCallUnload) localAssetBundle.Unload(false);

        return assets;
    }

    private string[] GetSFXAssetNames()
    {
        string[] assetNames = {
    
            nameof(EAudio.SFXAmbianceSound), 
            nameof(EAudio.SFXCard), 
            nameof(EAudio.SFXConfirm), 
            nameof(EAudio.SFXEnterWellcome),
            nameof(EAudio.SFXFishingRod),
            nameof(EAudio.SFXMiniGameLose),
            nameof(EAudio.SFXMiniGameWin),
            nameof(EAudio.SFXMenuHide),
            nameof(EAudio.SFXMenuShow),
            nameof(EAudio.SFXWalkDirty),
            nameof(EAudio.SFXWinGame)
        };

        return assetNames;
    }

    public Dictionary<EAudio, AudioClip> LoadSFX()
    {
        Dictionary<EAudio, AudioClip> audioClipsBundle = new();

        string[] assetNames = GetSFXAssetNames();
        List<AudioClip> audioClips = LoadAll<AudioClip>(GameParameters.BundleNames.SFX, false, assetNames);
        foreach (AudioClip clip in audioClips)
        {
            EAudio audioId = EAudio.SFXConfirm;
            switch (clip.name)
            {
                case nameof(EAudio.SFXAmbianceSound):
                    audioId = EAudio.SFXAmbianceSound;
                    break;
                case nameof(EAudio.SFXCard):
                    audioId = EAudio.SFXCard;
                    break;
                case nameof(EAudio.SFXConfirm):
                    audioId = EAudio.SFXConfirm;
                    break;
                case nameof(EAudio.SFXEnterWellcome):
                    audioId = EAudio.SFXEnterWellcome;
                    break;
                case nameof(EAudio.SFXFishingRod):
                    audioId = EAudio.SFXFishingRod;
                    break;
                case nameof(EAudio.SFXMiniGameLose):
                    audioId = EAudio.SFXMiniGameLose;
                    break;
                case nameof(EAudio.SFXMiniGameWin):
                    audioId = EAudio.SFXMiniGameWin;
                    break;
                case nameof(EAudio.SFXMenuHide):
                    audioId = EAudio.SFXMenuHide;
                    break;
                case nameof(EAudio.SFXMenuShow):
                    audioId = EAudio.SFXMenuShow;
                    break;
                case nameof(EAudio.SFXWalkDirty):
                    audioId = EAudio.SFXWalkDirty;
                    break;
                case nameof(EAudio.SFXWinGame):
                    audioId = EAudio.SFXWinGame;
                    break;

            }

            AudioClip newClip = Instantiate(clip);
            newClip.name = clip.name;
            audioClipsBundle.Add(audioId, newClip);
        }

        return audioClipsBundle;
    }

}
