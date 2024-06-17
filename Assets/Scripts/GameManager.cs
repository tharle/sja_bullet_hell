using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    private static GameManager instance;
    public static GameManager Instance => instance;
    public static event Action OnInit;

    //GameManager
    private Save currentSave;
    private PlayerEntity playerEntity;

    public PlayerEntity PlayerEntity => playerEntity;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            OnInit?.Invoke();
        }
        else
            Destroy(gameObject);
    }

    public void NewGame(Save _save)
    {
        currentSave = _save;
        SceneManager.LoadScene("Gym");
    }

    public void LoadGame(Save _save)
    {
        currentSave = _save;
        SceneManager.LoadScene("Gym");
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
        }
    }
}
