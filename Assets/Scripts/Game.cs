using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;
using BeyondTheDoor.SaveSystem;
using UnityEngine.Events;
using BeyondTheDoor.UI;

public class Game : MonoBehaviour
{
    public static Game instance;
    private void Awake()
    {
        instance = this;
        Settings.Load();
    }


    [Header("Questions")]
    public Conversation q_addToScavengeParty;
    public Conversation q_removeFromScavengeParty;
    public Conversation q_door_options;
    public Conversation q_door_askQuestions;
    public Conversation q_door_openOrKeepClosed;
    public Conversation q_makeOCDecision;
    public Conversation q_kickCharacterOut;

    [Header("Conversations")]
    public Conversation advance;
    public Conversation confirmScavenge;
    public Conversation confirmNoScavenge;

    [Header("Input")]
    [SerializeField] private ConversationCallback advanceCallback;
    [SerializeField] private ConversationCallback scavengeAdvanceCallback;
    [SerializeField] private ConversationCallback openDoorCallback;
    [SerializeField] private ConversationCallback leaveDoorClosedCallback;
    [Space]
    [SerializeField] private ConversationCallback addToScavengePartyCallback;
    [SerializeField] private ConversationCallback removeFromScavengePartyCallback;
    [SerializeField] private ConversationCallback sendToScavenge_withShotgunCallback;
    [SerializeField] private ConversationCallback sendToScavenge_noShotgunCallback;
    [Space]
    // These will call the appropriate response from the DayBehaviour
    [SerializeField] private ConversationCallback asked_q_whoAreYouCallback;
    [SerializeField] private ConversationCallback asked_q_whatDoYouWantCallback;
    [SerializeField] private ConversationCallback asked_q_whyShouldILetYouInCallback;
    [SerializeField] private ConversationCallback asked_q_howCanYouHelpMeCallback;
    [Space]
    [SerializeField] private ConversationCallback makingOCDecisionCallback;
    [SerializeField] private ConversationCallback kickCharacterOutCallback;
    [Space]
    [SerializeField] private ConversationCallback staySilentCallback;
    [SerializeField] private ConversationCallback peepholeCallback;
    [Space]
    [SerializeField] private ConversationCallback clearQueueCallback;
    [SerializeField] private ConversationCallback learnName;

    [Space]
    [SerializeField] private ConversationCallback loadStoredCharacter;


    [Header("Output")]
    [Tooltip("Called after a save file is loaded but before the stage is loaded.")]
    [SerializeField] private UnityEvent onInitialize;

    [Space]
    [Tooltip("Called after the game's stage is changed but before the next stage is loaded.")]
    [SerializeField] private UnityEvent onStageChanged;
    [Tooltip("Called after the game's stage is changed and loaded.")]
    [SerializeField] private UnityEvent onStageLoaded;

    [Space]
    [Tooltip("Called when a new day is started but before it is loaded.")]
    [SerializeField] private UnityEvent onNewDayStarted;

    [Space]
    [Tooltip("Called when returning to the main menu but before the game is unloaded.")]
    [SerializeField] private UnityEvent onGameExit;

    [Space]
    [SerializeField] private UnityEvent onDoorOpened;
    [SerializeField] private UnityEvent onDoorLeftClosed;

    // Do we only allow 1 person to scavenge at a time?
    const bool SingleMemberScavengeParty = true;


    public static Character[] CharacterArrivalOrder { get; private set; } =
    {
        Character.Tutorial_Mom,
        Character.Jessica,
        Character.Bob,
        Character.Bear,
        Character.Violet,
        Character.Dad,
        Character.Hal,
        Character.Raiders
    };

    /// <summary>
    /// What character is arriving today?
    /// </summary>
    public static Character ArrivingCharacter => CharacterArrivalOrder[DayNumber];
    public static int DayNumber => Day.DayNumber;
    public static Stage Stage => Day.Stage;
    public static bool Alone => Cabin.IsAlone;
    public static List<Character> ScavengeParty { get; private set; } = new List<Character>();

    /// <summary>
    /// Called after a save file is loaded but before the stage is loaded.
    /// </summary>
    public static UnityEvent OnInitialize => instance.onInitialize;
    /// <summary>
    /// Called after the game's stage is changed but before the next stage is loaded.
    /// </summary>
    public static UnityEvent OnStageChanged => instance.onStageChanged;
    /// <summary>
    /// Called after the game's stage is changed and loaded.
    /// </summary>
    public static UnityEvent OnStageLoaded => instance.onStageLoaded;
    /// <summary>
    /// Called when a new day is started but before it is loaded.
    /// </summary>
    public static UnityEvent OnNewDayStarted => instance.onNewDayStarted;
    /// <summary>
    /// Called when returning to the main menu but before the game is unloaded.
    /// </summary>
    public static UnityEvent OnGameExit => instance.onGameExit;
    /// <summary>
    /// Called when the door is opened, after the character's state is changed.
    /// </summary>
    public static UnityEvent OnDoorOpened => instance.onDoorOpened;
    /// <summary>
    /// Called when the door is kept closed, after the character's state is changed.
    /// </summary>
    public static UnityEvent OnDoorLeftClosed => instance.onDoorLeftClosed;


    private static uint currentSaveSlot;



    private void Start()
    {
        // Advance when a conversation wants to
        advanceCallback.Callback += (conv, line) => Advance();
        openDoorCallback.Callback += (conv, line) => OpenDoor();
        leaveDoorClosedCallback.Callback += (conv, line) => LeaveDoorClosed();
        addToScavengePartyCallback.Callback += (conv, line) => AddToScavengeParty(Character.Current);
        removeFromScavengePartyCallback.Callback += (conv, line) => RemoveFromScavengeParty(Character.Current);
        sendToScavenge_withShotgunCallback.Callback += (conv, line) => SendToScavenge(true);
        sendToScavenge_noShotgunCallback.Callback += (conv, line) => SendToScavenge(false);
        scavengeAdvanceCallback.Callback += (conv, line) => ScavengeAdvance();

        asked_q_whoAreYouCallback.Callback += (conv, line) => DayBehaviour.Current.q_whoAreYou.TryStart();
        asked_q_whatDoYouWantCallback.Callback += (conv, line) => DayBehaviour.Current.q_whatDoYouWant.TryStart();
        asked_q_whyShouldILetYouInCallback.Callback += (conv, line) => DayBehaviour.Current.q_whyShouldILetYouIn.TryStart();
        asked_q_howCanYouHelpMeCallback.Callback += (conv, line) => DayBehaviour.Current.q_howCanYouHelpMe.TryStart();

        staySilentCallback.Callback += (conv, line) => DayBehaviour.Current.reactionToPlayerStayingSilent.TryStart();
        peepholeCallback.Callback += (conv, line) => DayBehaviour.Current.peephole.TryStart();

        makingOCDecisionCallback.Callback += (conv, line) =>
        {
            //Debug.Log("SAVING CHAR: " + Character.Current != null ? Character.Current.Name : "null :(");
            //Character.ChoppingBlock = Character.Current;
            DayBehaviour.Current.Characters[Character.Current.ID].tryingToKickOut.TryStart();
            q_kickCharacterOut.Enqueue();
        };
        kickCharacterOutCallback.Callback += (conv, line) => KickOutCharacter(Character.Current);

        clearQueueCallback.Callback += (conv, line) => DialogueGUI.ClearQueue();
        learnName.Callback += (conv, line) => LearnName(conv);

        loadStoredCharacter.Callback += (conv, line) => Character.Current = Character.ChoppingBlock;

        // Hook up all characters to the scavenge adding/removal
        foreach (Character character in Character.All.Values)
            character.ClickedOnDuringScavengeStage += AddOrRemoveFromScavengeParty;
    }

    void LearnName(Conversation convo)
    {
        if (Character.Current != null)
            Character.Current.NameKnown = true;
        else if (convo != null)
        {
            foreach (IConversationElement element in convo.elements)
            {
                // Find the first line and reveal that person
                if (element is DialogueElement dialogue)
                    Character.All[Line.All[dialogue.lineID].CharacterID].NameKnown = true;
            }
        }

        // Keep them matched up
        Character.Sal.NameKnown = Character.Hal.NameKnown;
    }



    /// <summary>
    /// Starts the game given the loaded <paramref name="saveState"/>.
    /// </summary>
    /// <param name="saveState"></param>
    public static void Begin(SaveState saveState, uint saveSlot)
    {
        // Load the save
        saveState.Load();
        currentSaveSlot = saveSlot;

        OnInitialize?.Invoke();
        // Change scenes
        LoadStage(Day.Stage);
    }

    /// <summary>
    /// Advances to the next stage of the day, if possible. Saves progress.
    /// </summary>
    public static void Advance()
    {
        if (CanAdvance())
        {
            DialogueGUI.Close();

            // Execute stage-specific behaviour
            Stage next = GetNextStage();
            BeforeLoadNewStage(Stage, next);

            Day.Stage = next;

            OnLoadNewStage();

            // Save the state after we switch stages (but before changing scenes)
            SaveState currentState = new SaveState();
            currentState.SaveCurrentState();
            ByteBuffer savedBuf = SaveSystem.Save(currentState, currentSaveSlot);
            savedBuf.Read<byte>(); // Skip over the version number

            // Load the new day (resets state)
            Begin(new SaveState(savedBuf), currentSaveSlot);
            //LoadStage(Day.Stage);
        }
    }

    /// <summary>
    /// Can we advance to the next stage currently?
    /// </summary>
    /// <returns></returns>
    public static bool CanAdvance()
    {
        switch (Stage)
        {
            case Stage.MorningSupplies:
                // Just lore, you can advance any time
                return true;
            case Stage.SpeakingWithParty:
                // You can always advance from speaking
                return true;
            case Stage.SendingScavengers:
                // You can always advance without sending anyone
                return true;
            case Stage.FixingOvercrowding:
                // You can advance after you fix the overcrowding
                return !Cabin.IsOvercrowded();
            case Stage.DealingWithArrival:
                // You can advance once you've decided what to do with today's character
                return CharacterArrivalOrder[DayNumber].DoorDecisionMade;
        }

        throw new InvalidProgramException("Tried to check advancement on some goofy state that doesn't exist?");
    }

    private static void BeforeLoadNewStage(Stage current, Stage next)
    {
        switch (current)
        {
            case Stage.MorningSupplies:
                break;
            case Stage.SpeakingWithParty:
                break;
            case Stage.SendingScavengers:
                break;
            case Stage.FixingOvercrowding:
                break;
            case Stage.DealingWithArrival:
                break;
        }
    }

    private static void OnLoadNewStage()
    {
        switch (Stage)
        {
            case Stage.MorningSupplies:
                // Wrap around if we went to the next morning
                Day.StartDay(DayNumber + 1);
                OnNewDayStarted?.Invoke();
                break;
            case Stage.SpeakingWithParty:
                break;
            case Stage.SendingScavengers:
                break;
            case Stage.FixingOvercrowding:
                // See what happened to the scavengers
                if (ScavengeParty.Count > 0)
                {
                    DetermineScavengerFates();
                    // We have few enough people - get outta here
                    if (!Cabin.IsOvercrowded())
                        Advance();
                }
                break;
            case Stage.DealingWithArrival:
                // Make the character "arrive"
                ArrivingCharacter.ChangeStatus(CharacterStatus.AtDoor);
                break;
        }
    }

    /*
    private static Ending GetEndingIfAdvance()
    {
        // Uh oh, you might be starving
        if (DayNumber == 5 && !Cabin.HasScavengedSuccessfully)
        {
            // You are all alone - no way to scavenge
            if (Alone && Stage == Stage.SpeakingWithParty)
                return Ending.Starve;
            // You didn't send anyone to scavenge (or Jessica died) and it's the afternoon now
            else if (Stage == Stage.FixingOvercrowding)
                return Ending.Starve;
        }
        else if (DayNumber == Day.LastDay && Stage == Stage.DealingWithArrival)
            return Ending.

        return Ending.None;
    }
    */

    private static Stage GetNextStage()
    {
        switch (Stage)
        {
            case Stage.MorningSupplies:
                // Skip right to the end of the day if you are alone
                return Alone ? Stage.DealingWithArrival : Stage.SpeakingWithParty;
            case Stage.SpeakingWithParty:
                return Stage.SendingScavengers;
            case Stage.SendingScavengers:
                bool peopleScavenging = ScavengeParty.Count > 0;
                bool overcrowded = Cabin.IsOvercrowded();
                return peopleScavenging || overcrowded ? Stage.FixingOvercrowding : Stage.DealingWithArrival;
            case Stage.FixingOvercrowding:
                return Stage.DealingWithArrival;
            case Stage.DealingWithArrival:
                return Stage.MorningSupplies;
            default:
                return Stage.MorningSupplies;
        }
    }


    private static void LoadStage(Stage stage)
    {
        OnStageChanged?.Invoke();
        Level level = GameStageToLevel(stage);
        SceneManager.LoadLevel(level);
        OnStageLoaded?.Invoke();
    }


    public static void OpenDoor()
    {
        ArrivingCharacter.ChangeStatus(CharacterStatus.InsideCabin);
        if (ArrivingCharacter == Character.Hal)
            Character.Sal.ChangeStatus(CharacterStatus.InsideCabin);
        OnDoorOpened?.Invoke();
        DayBehaviour.Current.letPersonInside.TryStart();

        // Advance afterwards
        instance.advance.Enqueue();
    }

    public static void LeaveDoorClosed()
    {
        ArrivingCharacter.ChangeStatus(CharacterStatus.LeftOutside);
        if (ArrivingCharacter == Character.Hal)
            Character.Sal.ChangeStatus(CharacterStatus.LeftOutside);
        OnDoorLeftClosed?.Invoke();
        DayBehaviour.Current.keepPersonOutside.TryStart();

        // Advance afterwards
        instance.advance.Enqueue();
    }

    /// <summary>
    /// Opens the prompt to add/remove the character from the scavenge party
    /// </summary>
    /// <param name="character"></param>
    public static void AddOrRemoveFromScavengeParty(Character character)
    {
        if (ScavengeParty.Contains(character))
        {
            DayBehaviour.Current.Characters[character.ID]?.beforeUnscavengingChoice.TryStart();
            instance.q_removeFromScavengeParty.Enqueue();
        }
        else
        {
            DayBehaviour.Current.Characters[character.ID]?.beforeScavengingChoice.TryStart();
            instance.q_addToScavengeParty.Enqueue();
        }
    }

    public static void AddToScavengeParty(Character character)
    {
        if (character != null && !ScavengeParty.Contains(character))
        {
            if (SingleMemberScavengeParty)
                ScavengeParty.Clear();
            ScavengeParty.Add(character);
            // The twins are a combo deal
            if (character == Character.Hal)
                ScavengeParty.Add(Character.Sal);
            else if (character == Character.Sal)
                ScavengeParty.Add(Character.Hal);
            character.Invoke_AddedToScavengeParty();
        }
    }

    public static void RemoveFromScavengeParty(Character character)
    {
        if (character != null && ScavengeParty.Contains(character))
        {
            ScavengeParty.Remove(character);
            if (character == Character.Hal)
                ScavengeParty.Remove(Character.Sal);
            else if (character == Character.Sal)
                ScavengeParty.Remove(Character.Hal);
            character.Invoke_RemovedFromScavengeParty();
        }
    }

    public static void KickOutCharacter(Character character)
    {
        DayBehaviour.Current.Characters[character.ID].kickedOut.TryStart(); 
        character.ChangeStatus(CharacterStatus.KickedOut);
        if (character == Character.Hal)
            Character.Sal.ChangeStatus(CharacterStatus.KickedOut);
        else if (character == Character.Sal)
            Character.Hal.ChangeStatus(CharacterStatus.KickedOut);

        // Advance
        instance.advance.Enqueue();
    }


    private static void ScavengeAdvance()
    {
        // Handle sending some or no scavengers
        if (ScavengeParty.Count == 0)
        {
            // Let the characters discuss this first
            DayBehaviour.Current.sendNoScavengers.TryStart();
            // Confirm this is what the player wants
            instance.confirmNoScavenge.Enqueue();
        }
        else
        {
            Conversation beingSentScavenging = DayBehaviour.Current.Characters[ScavengeParty[0].ID].beingSentScavenging;
            beingSentScavenging.TryStart();
            // Start the default one if none is provided
            if (beingSentScavenging == null)
                instance.confirmScavenge.Start();
        }
    }


    /// <summary>
    /// Sends out the current scavenging party.
    /// </summary>
    /// <param name="withShotgun"></param>
    public static void SendToScavenge(bool withShotgun)
    {
        if (ScavengeParty.Count > 0)
        {
            // Only allow the shotgun if it's in the cabin
            bool sendingShotgun = withShotgun && Cabin.HasShotgun;

            // Remove the shotgun from the cabin
            if (sendingShotgun)
                Cabin.HasShotgun = false;

            // Send all scavengers out
            foreach (Character scavenger in ScavengeParty)
                scavenger.ChangeStatus(sendingShotgun ? CharacterStatus.ScavengingWithShotgun : CharacterStatus.ScavengingDefenseless);

            // Call their callbacks
            foreach (Character scavenger in ScavengeParty)
                scavenger.Invoke_SentToScavenge(withShotgun);
        }

        // Move along to the next stage
        instance.advance.Enqueue();
    }

    private static void DetermineScavengerFates()
    {
        if (ScavengeParty.Count == 0)
            return;

        // Kill Jessica if she is alone
        if (ScavengeParty.Count == 1 && ScavengeParty[0] == Character.Jessica)
        {
            // RIP bozo you will not be missed
            Character.Jessica.ChangeStatus(CharacterStatus.DeadWhileScavenging);
            DayBehaviour.Current.Characters[CharacterID.Jessica].diedWhileScavenging.TryStart();
        }
        // Check if Hal was eaten by a bear
        else if (ScavengeParty.Count == 2 && (ScavengeParty[0] == Character.Hal || ScavengeParty[0] == Character.Sal))
        {
            if (Character.Hal.Status == CharacterStatus.ScavengingDefenseless)
            {
                // :(
                Character.Hal.ChangeStatus(CharacterStatus.DeadWhileScavenging);
                Character.Sal.ChangeStatus(CharacterStatus.InsideCabin);
                DayBehaviour.Current.Characters[CharacterID.Hal].diedWhileScavenging.TryStart();
            }
        }
        else
        {
            // Return the shotgun if they had it
            if (ScavengeParty[0].Status == CharacterStatus.ScavengingWithShotgun)
                Cabin.HasShotgun = true;

            Cabin.HasScavengedSuccessfully = true;

            bool hadCar = Cabin.HasCar;

            // Welcome back valued party members
            foreach (Character scavenger in ScavengeParty)
            {
                // Call our callbacks
                if (scavenger.Status == CharacterStatus.ScavengingDefenseless)
                    DayBehaviour.Current.Characters[scavenger.ID].returnedFromScavengingWithoutGun.TryStart();
                else
                    DayBehaviour.Current.Characters[scavenger.ID].returnedFromScavengingWithGun.TryStart();

                scavenger.ChangeStatus(CharacterStatus.InsideCabin);
                if (scavenger == Character.Hal)
                    Character.Sal.ChangeStatus(CharacterStatus.InsideCabin);
                else if (scavenger == Character.Sal)
                    Character.Hal.ChangeStatus(CharacterStatus.InsideCabin);
                // Violet gets the car
                if (scavenger == Character.Violet)
                    Cabin.HasCar = true;
            }

            // We just got the car
            if (!hadCar && Cabin.HasCar)
                DayBehaviour.Current.gotCar.TryStart();
        }

        ScavengeParty.Clear();
    }


    public static void ExitToMenu()
    {
        OnGameExit?.Invoke();
        DialogueGUI.Close(); // Turn off the GUI if it's playing a line
        Character.ResetAll(); // Reset them (doesn't really matter but meh)

        // Game is saved automatically
        SceneManager.LoadLevel(Level.MainMenu);
    }

    public static Level GameStageToLevel(Stage stage) => stage switch
    {
        Stage.MorningSupplies => Level.Dawn,
        Stage.SpeakingWithParty => Level.Morning,
        Stage.SendingScavengers => Level.Noon,
        Stage.FixingOvercrowding => Level.Afternoon,
        Stage.DealingWithArrival => Level.Door,
        _ => throw new ArgumentException("Invalid stage: " + stage)
    };
}

public enum Ending
{
    None,
    /// <summary>
    /// Failed to let your parents back in.
    /// </summary>
    JokeEnding,
    /// <summary>
    /// Killed by the bear.
    /// </summary>
    Bear,
    /// <summary>
    /// Didn't scavenge in time.
    /// </summary>
    Starve,
    /// <summary>
    /// Didn't have the resources to make it.
    /// </summary>
    DieOnWayToBorder,
    /// <summary>
    /// The dad and his son saved you.
    /// </summary>
    SavedByDad,
    /// <summary>
    /// You walked to the border successfully.
    /// </summary>
    ReachBorder,
    /// <summary>
    /// You drove to the border.
    /// </summary>
    DriveToBorder
}