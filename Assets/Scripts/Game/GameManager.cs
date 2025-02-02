using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;
    public static event Action OnInit;

    //GameManager
    private Save m_CurrentSave;
    private PlayerEntity m_PlayerEntity;
    [SerializeField] bool m_NewGameSlot1;

    public PlayerEntity PlayerEntity => m_PlayerEntity;
    public int LastWaveIndex => m_CurrentSave != null? m_CurrentSave.WaveIndex : 0;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);

            OnInit?.Invoke();
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if(m_NewGameSlot1 || !LoadGame(1, false)) NewGame(1, false);
    }

    public void NewGame(int index, bool loadScene = true)
    {
        // if exist data, delete
        SaveManagerJson.DeleteSave(index.ToString()); // TODO pe trater lerreur apres
        m_CurrentSave = new Save(true, index, index.ToString(), new List<Item>(), 0, 0);
        if(loadScene) SceneManager.LoadScene(GameParameters.SceneName.GAME);
    }

    public bool LoadGame(int slotIndex, bool loadScene = true)
    {
        Save save = SaveManagerJson.Load<Save>(slotIndex.ToString());
        if (save != null) 
        {
            LoadGame(save, loadScene);
            return true;
        }

        return false;
    }

    public void LoadGame(Save save, bool loadScene = true)
    {
        m_CurrentSave = save;
        if (loadScene) SceneManager.LoadScene(GameParameters.SceneName.GAME);
    }

    public void ReloadCurrentGame()
    {
        LoadGame(m_CurrentSave.Index);
    }

    public void SetPlayer(PlayerEntity playerEntiy)
    {
        m_PlayerEntity = playerEntiy;

        if (m_CurrentSave != null && !m_CurrentSave.IsNewGame)
            m_PlayerEntity.SetSave(m_CurrentSave);
    }

    public void Save(int WaveIndex)
    {
        if (m_CurrentSave == null)
            return;

        m_CurrentSave.UpdateSave(m_PlayerEntity.Items, m_PlayerEntity.CurrentHealth,m_PlayerEntity.Stats, WaveIndex);
        if (SaveManagerJson.Save(m_CurrentSave, m_CurrentSave.SaveName))
            Debug.Log("Saved Sucessfully !!");
    }
}
