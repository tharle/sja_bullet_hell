using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;
    public static event Action OnInit;


    // TODO: Load �a via bundle dans le future
    private Item m_ItemApple;
    private Item m_ItemOrange;
    private Item m_ItemBlueberry;

    //GameManager
    private Save m_CurrentSave;
    private PlayerEntity m_PlayerEntity;

    public PlayerEntity PlayerEntity => m_PlayerEntity;

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
        ItemLoader.Instance.LoadAll();
        m_ItemApple = ItemLoader.Instance.Get(EItem.Apple);
        m_ItemBlueberry = ItemLoader.Instance.Get(EItem.Blueberry);
        m_ItemOrange = ItemLoader.Instance.Get(EItem.Orange);
    }

    public void NewGame(Save _save, bool loadScene = true)
    {
        m_CurrentSave = _save;
        if(loadScene) SceneManager.LoadScene(GameParameters.SceneName.GAME);
    }

    public void LoadGame(Save _save, bool loadScene = true)
    {
        m_CurrentSave = _save;
        if (loadScene) SceneManager.LoadScene(GameParameters.SceneName.GAME);
    }

    public void SetPlayer(PlayerEntity _playerEntiy)
    {
        m_PlayerEntity = _playerEntiy;

        if (m_CurrentSave != null && !m_CurrentSave.NewGame)
            m_PlayerEntity.SetSave(m_CurrentSave);
    }

    public void Save()
    {
        if (m_CurrentSave == null)
            return;

        m_CurrentSave.UpdateSave(m_PlayerEntity.Items, m_PlayerEntity.CurrentHealth,m_PlayerEntity.Stats);
        if (SaveManagerJson.Save(m_CurrentSave, m_CurrentSave.SaveName))
            Debug.Log("Saved Sucessfully !!");
    }

    private void OnGUI()
    {
        if (m_PlayerEntity)
        {
            GUILayout.Label($"Player Health : {m_PlayerEntity.CurrentHealth}");
            if (GUILayout.Button("Take Damage"))
                m_PlayerEntity.TakeDamage(5);
            if (GUILayout.Button("Save"))
                GameManager.Instance.Save();

            if (GUILayout.Button("Eat Apple"))
                GameEventSystem.Instance.TriggerEvent(EGameEvent.AddItem, new GameEventMessage(EGameEventMessage.Item, m_ItemApple));
            if (GUILayout.Button("Eat Orange"))
                GameEventSystem.Instance.TriggerEvent(EGameEvent.AddItem, new GameEventMessage(EGameEventMessage.Item, m_ItemOrange));
            if (GUILayout.Button("Eat Blueberry"))
                GameEventSystem.Instance.TriggerEvent(EGameEvent.AddItem, new GameEventMessage(EGameEventMessage.Item, m_ItemBlueberry));

            if (GUILayout.Button("To main menu"))
                SceneManager.LoadScene(GameParameters.SceneName.MAIN_MENU);
        }
    }
}