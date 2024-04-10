using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

/// <summary>
/// Used to initialize pieces of the dialogue system for a specific day.
/// </summary>
/// <remarks>Basically wraps <seealso cref="Game"/> callbacks.</remarks>
public abstract class DayBehaviour : MonoBehaviour
{
    private static Dictionary<int, DayBehaviour> instances = new Dictionary<int, DayBehaviour>();
    public static DayBehaviour Current => GetCurrentDay();

    protected int DayNumber { get; private set; }
    protected Stage Stage => Day.Stage;
    public bool Active => DayNumber == Day.DayNumber;


    [SerializeField] protected CharacterInit[] _characters;

    [Header("Dawn/Lore")]
// TODO: dayStarted
    public Conversation checkOutside;
    public Conversation radio;
    public Conversation tv;
    public Conversation checkSupplies;

// TODO: enteringTalking

    [Header("Noon/Scavenging")]
    public Conversation enteringScavenging;
// TODO: gotCar
// TODO: sendNoScavengers

    [Header("Afternoon/Overcrowding")]
    public Conversation enteringOvercrowding;

    [Header("Night/Door")]
    public Conversation personArrivedAtDoor;
    [Tooltip("Optional, mainly Jessica reacting")]
    public Conversation reactionToPlayerStayingSilent;
    public Conversation peephole;

    [Space]
    public Conversation q_whoAreYou;
    public Conversation q_whatDoYouWant;
    public Conversation q_whyShouldILetYouIn;
    public Conversation q_howCanYouHelpMe;

    [Space]
    public Conversation letPersonInside;
    public Conversation keepPersonOutside;


    public Dictionary<CharacterID, CharacterInit> Characters { get; private set; } = new Dictionary<CharacterID, CharacterInit>();



    private void Awake()
    {
        DayNumber = GetDay();

        // Make sure there is only one
        if (!instances.ContainsKey(DayNumber))
        {
            instances.Add(DayNumber, this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (CharacterInit ch in _characters)
            Characters.Add(ch.id, ch);
    }

    private void Start()
    {
        RegisterConversationCallbacks();

        AddGameCallbacks();
    }

    private void AddGameCallbacks()
    {
        Game.OnInitialize.AddListener(() => { if (Active) RegisterCharacterCallbacks(); Initialize(); });
        Game.OnStageChanged.AddListener(() => { if (Active) StageChanged(); });
        Game.OnStageLoaded.AddListener(() => { if (Active) StageLoaded(); });
        //Game.OnNewDayStarted.AddListener(NewDayStarted);
        Game.OnGameExit.AddListener(() => { if (Active) GameExit(); });
        Game.OnDoorOpened.AddListener(() => { if (Active) DoorOpened(); });
        Game.OnDoorLeftClosed.AddListener(() => { if (Active) DoorLeftClosed(); });
    }


    public static DayBehaviour GetCurrentDay() => instances[Day.DayNumber];
    public static bool TryGetDay(int dayNumber, out DayBehaviour day) => instances.TryGetValue(dayNumber, out day);

    // Called every time the Character callbacks are cleared
    private void RegisterCharacterCallbacks()
    {
        foreach (CharacterInit ch in _characters)
            Register(ch, Character.All[ch.id]);
    }

    private void Register(CharacterInit events, Character character)
    {
        character.SpokenTo += (ch) => Run(events.wakingUp);
        character.SentToScavenge += (ch, hasShotgun) => Run(events.beingSentScavenging);
    }

    // Used to null-check a conversation in one line
    void Run(Conversation convo)
    {
        if (convo != null)
            convo.Start();
    }


    /// <summary>
    /// Override and return the day that this init should be used for.
    /// </summary>
    /// <returns></returns>
    protected abstract int GetDay();

    /// <summary>
    /// Called only once when application is started.
    /// </summary>
    /// <remarks>Use this to subscribe to persistent events (like ConversationCallbacks)</remarks>
    protected virtual void RegisterConversationCallbacks() { }

    /// <summary>
    /// Called for this day before each stage is loaded.
    /// </summary>
    /// <remarks>Initialize lines and character callbacks here.</remarks>
    protected abstract void Initialize();
    /// <summary>
    /// Called after the game's stage is changed but before the next stage is loaded.
    /// </summary>
    protected virtual void StageChanged() { }
    /// <summary>
    /// Called after the game's stage is changed and loaded.
    /// </summary>
    protected virtual void StageLoaded() { }
    /*
    /// <summary>
    /// Called when a new day is started but before it is loaded.
    /// </summary>
    protected virtual void NewDayStarted() { }
    */
    /// <summary>
    /// Called when returning to the main menu from this day, but before the game is unloaded.
    /// </summary>
    protected virtual void GameExit() { }
    /// <summary>
    /// Called when the door is opened on this day, after the character's state is changed.
    /// </summary>
    protected virtual void DoorOpened() { }
    /// <summary>
    /// Called when the door is kept closed on this day, after the character's state is changed.
    /// </summary>
    protected virtual void DoorLeftClosed() { }


    [System.Serializable]
    public class CharacterInit
    {
        public CharacterID id;

        [Space]
        public Conversation wakingUp;

        [Space]
// TODO: beforeScavengingChoice
// TODO: beforeUnscavengingChoice
// TODO: addedToScavengeParty
// TODO: removedFromScavengeParty
        [Tooltip("Started when the player will pop up the menu to send this character scavenging")]
        public Conversation mightSendScavenging;
        [Tooltip("Started when the player might choose to not send this character scavenging anymore")]
        public Conversation mightNotSendScavenging;
        [Tooltip("LINK TO 'SendWith/WithoutShotgun' or 'ConfirmScavenge'. Started when this character is getting sent out")]
        public Conversation beingSentScavenging;
        [Tooltip("Started after this character is sent with the shotgun, right before they leave")]
        public Conversation sentScavengingWithShotgun;
        [Tooltip("Started after this character is sent without the shotgun, right before they leave")]
        public Conversation sentScavengingWithoutShotgun;

        [Space]
        [Tooltip("Started when this character fails to return. Don't include this character in the dialogue obviously")]
        public Conversation diedWhileScavenging;
        public Conversation returnedFromScavengingWithGun;
        public Conversation returnedFromScavengingWithoutGun;

        [Space]
        [Tooltip("Started when this character is clicked on during overcrowding")]
        public Conversation clickedOnDuringOvercrowding;
        [Tooltip("Started when the player tries to kick this character out")]
        public Conversation tryingToKickOut;
        [Tooltip("The last conversation after this character has been kicked out")]
        public Conversation kickedOut;

        [Space]
        //[Tooltip("Started when this character arrives at the door, asking to be let in.")]
        //public Conversation arrivingAtDoor;
        [Tooltip("Started when the player clicks on this character when someone else is outside.")]
        public Conversation commentOnOtherPersonAtDoor;

        public Character Character => Character.All[id];
    }
}
