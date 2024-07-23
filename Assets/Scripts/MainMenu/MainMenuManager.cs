using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] Sprite m_IconSlotFull;
    [SerializeField] TextMeshProUGUI m_TitleSlot;


    private Animator m_Animator;

    Save[] m_Saves = { null, null, null};

    private void Start()
    {

        SubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.MainMenuSelectSlot, OnMenuSlotSelect);
    }

    private void OnDestroy()
    {
        GameEventSystem.Instance.UnsubscribeFrom(EGameEvent.MainMenuSelectSlot, OnMenuSlotSelect);
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
        GameManager.Instance.NewGame(slotIndex, false);
        InitGame();
    }

    private void LoadGameFromSlot(int slotIndex)
    {
        Save save = m_Saves[slotIndex];

        if (save == null) return;
        save.IsNewGame = false ;

        GameManager.Instance.LoadGame(save, false);
        InitGame();
    }

    private void InitGame()
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

    private Animator GetAnimator()
    {
        if (gameObject.IsDestroyed()) return null;
        if(m_Animator== null || m_Animator.IsDestroyed()) m_Animator = GetComponent<Animator>();

        return m_Animator;
    }

    private void AnimationMenuLoad(bool enter)
    {
        if(enter) GetAnimator().SetTrigger(GameParameters.AnimationMainMenu.TRIGGER_LOAD_ENTER);
        else GetAnimator().SetTrigger(GameParameters.AnimationMainMenu.TRIGGER_LOAD_EXIT);
    }

    private void AnimationStartGame()
    {

        GetAnimator().SetTrigger(GameParameters.AnimationMainMenu.TRIGGER_FADE_OUT);
    }

    IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(GameParameters.SceneName.GAME);
    }
}
