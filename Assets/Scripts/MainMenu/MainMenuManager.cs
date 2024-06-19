using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] Sprite m_IconSlotFull;
    [SerializeField] TextMeshProUGUI m_TitleSlot;


    private Animator m_Animator;

    Save[] m_Saves = { null, null, null};

    private void Start()
    {
        m_Animator = GetComponent<Animator>();

        SubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.MainMenuSelectSlot, OnMenuSlotSelect);
    }

    private void OnMenuSlotSelect(GameEventMessage message)
    {
        if(message.Contains<int>(EGameEventMessage.SlotIndex, out int SlotIndex))
        {
            bool hasData = false;
            message.Contains<bool>(EGameEventMessage.HasData, out hasData);

            if(hasData) LoadGameFromSlot(SlotIndex);
            else StartGameFromSlot(SlotIndex);
        }
    }

    private void StartGameFromSlot(int slotIndex)
    {
        // if exist data, delete
        SaveManagerJson.DeleteSave(slotIndex.ToString()); // TODO pe trater lerreur apres
        Save newSave = new Save(true, slotIndex, slotIndex.ToString(), new List<Item>(), 0);
        GameManager.Instance.NewGame(newSave, false);
        LoadGame();
    }

    private void LoadGameFromSlot(int slotIndex)
    {
        Save save = m_Saves[slotIndex];

        if (save == null) return;
        save.NewGame = false ;

        GameManager.Instance.LoadGame(save, false);
        LoadGame();
    }

    public void LoadGame()
    {
        AnimationStartGame();
        StartCoroutine(StartGameRoutine());
    }


    // -----------------------------------------------
    // ON BUTTONS EVENTS
    // -----------------------------------------------

    public void OnStartGame()
    {
        LoadAllSaves(true);
        AudioManager.Instance.Play(EAudio.SFXConfirm, transform.position);
    }

    public void OnLoadMenuEnter()
    {
        LoadAllSaves(false);
        AudioManager.Instance.Play(EAudio.SFXConfirm, transform.position);
    }

    private void LoadAllSaves(bool newGame)
    {

        m_TitleSlot.text = newGame? "New Game" : "Load Game";

        for (int i = 0; i < 3; i++)
        {
            m_Saves[i] = SaveManagerJson.Load<Save>(i.ToString());
            GameEventMessage message = new GameEventMessage(EGameEventMessage.SlotIndex, i);
            message.Add(EGameEventMessage.IsNewGame, newGame);
            if (m_Saves[i] != null)
            {
                SaveSlotData slotData;
                slotData.Info = "Has data!";
                slotData.Icon = m_IconSlotFull;
                message.Add(EGameEventMessage.SlotData, slotData);
            }
            GameEventSystem.Instance.TriggerEvent(EGameEvent.MainMenuLoadSlot, message);
        }

        
        AnimationMenuLoad(true);

    }

    public void OnLoadMenuExit()
    {
        AudioManager.Instance.Play(EAudio.SFXConfirm, transform.position);
        AnimationMenuLoad(false);
    }

    public void OnQuit()
    {
        AudioManager.Instance.Play(EAudio.SFXConfirm, transform.position);
        Application.Quit();
    }

    private void AnimationMenuLoad(bool enter)
    {
        if(enter) m_Animator.SetTrigger("menu_load_enter");
        else m_Animator.SetTrigger("menu_load_exit");
    }

    private void AnimationStartGame()
    {
        m_Animator.SetTrigger("menu_fade_out");
    }

    IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Game");
    }
}
