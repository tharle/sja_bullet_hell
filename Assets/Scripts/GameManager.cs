using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;
    public static event Action OnInit;


    // TODO: Load ça via bundle dans le future
    [SerializeField] private Item m_ItemApple;
    [SerializeField] private Item m_ItemOrange;
    [SerializeField] private Item m_ItemBlueberry;

    //GameManager
    private Save currentSave;
    private PlayerEntity playerEntity;

    public PlayerEntity PlayerEntity => playerEntity;

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

    public void NewGame(Save _save, bool loadScene = true)
    {
        currentSave = _save;
        if(loadScene) SceneManager.LoadScene("Game");
    }

    public void LoadGame(Save _save, bool loadScene = true)
    {
        currentSave = _save;
        if (loadScene) SceneManager.LoadScene("Game");
    }

    public void SetPlayer(PlayerEntity _playerEntiy)
    {
        playerEntity = _playerEntiy;

        if (currentSave != null && !currentSave.NewGame)
            playerEntity.SetSave(currentSave);
    }

    public void Save()
    {
        if (currentSave == null)
            return;

        currentSave.UpdateSave(playerEntity.Items, playerEntity.CurrentHealth,playerEntity.Stats);
        if (SaveManagerJson.Save(currentSave, currentSave.SaveName))
            Debug.Log("Saved Sucessfully !!");
    }

    private void OnGUI()
    {
        if (playerEntity)
        {
            GUILayout.Label($"Player Health : {playerEntity.CurrentHealth}");
            if (GUILayout.Button("Take Damage"))
                playerEntity.TakeDamage(5);
            if (GUILayout.Button("Save"))
                GameManager.Instance.Save();

            if (GUILayout.Button("Eat Apple"))
                GameEventSystem.Instance.TriggerEvent(EGameEvent.AddItem, new GameEventMessage(EGameEventMessage.Item, m_ItemApple));
            if (GUILayout.Button("Eat Orange"))
                GameEventSystem.Instance.TriggerEvent(EGameEvent.AddItem, new GameEventMessage(EGameEventMessage.Item, m_ItemOrange));
            if (GUILayout.Button("Eat Blueberry"))
                GameEventSystem.Instance.TriggerEvent(EGameEvent.AddItem, new GameEventMessage(EGameEventMessage.Item, m_ItemBlueberry));

            if (GUILayout.Button("To main menu"))
                SceneManager.LoadScene("MainMenu");
        }
    }
}
