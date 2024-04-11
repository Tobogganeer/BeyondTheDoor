using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EndingAnimation : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI FinalText;
    [SerializeField] private TextMeshProUGUI Logo;
    [SerializeField] private float timeBetweenTransitions;
    [SerializeField] private Image image;
    //[SerializeField] private int survivors;

    List<Character> GetCharactersAtBorder() => Character.All.Values.Where((ch) => ch.Status == CharacterStatus.AliveAtBorder).ToList();

    IEnumerator Start()
    {
        Debug.Log("player " + Character.Player.Status);
        Debug.Log("bob " + Character.Bob.Status);
        Debug.Log("violet " + Character.Violet.Status);
        Debug.Log("Jessica " + Character.Jessica.Status);
        List<Character> peopleAtTheBorder = GetCharactersAtBorder();
        bool only1atBorder = peopleAtTheBorder.Count == 2;
        Character personAtBorder = null;
        if (only1atBorder)
            personAtBorder = peopleAtTheBorder[0];

        // Starved
        if (Day.DayNumber == 5 && !Cabin.HasScavengedSuccessfully)
        {
            yield return Starved();
        }

        // Died on the way to the border
        if (Character.Player.DiedOnWayToBorder)
        {
            yield return DeadOnWayToBorder();
        }

        //player dead by bear
        else if (Character.Player.Status == CharacterStatus.KilledByBear )
        {
            yield return KilledByBear();
        }

        else if (Character.Player.AliveAtBorder)
        {
            yield return PlayerMadeItToBorder();
        }

        else if (Character.Tutorial_Dad.Status == CharacterStatus.LeftOutside && Character.Tutorial_Mom.Status == CharacterStatus.LeftOutside)
        {
            yield return JokeEnding();
        }

        else if (only1atBorder || peopleAtTheBorder.Count == 2 && peopleAtTheBorder[0] == Character.Hal)
        {
            yield return SingleCharacterAtBorder(personAtBorder);
        }
    }


    private IEnumerator Starved()
    {
        yield return WriteList(timeBetweenTransitions, "Jessica didn't return from the scavenging", "We've ran out of water, food and heating for days now", "I fear I'm not going to last long", "Somebody", "help", "please");
    }

    private IEnumerator KilledByBear()
    {
        yield return WriteList(timeBetweenTransitions, "What the fuck was going on my head", "for me to let a bear get inside my house", "I’m dead now", "What makes me even sadder", "is that I’m just a number now", "like those who where caught between conflicting ideas", "my death is irrelevant", "as it will all be forgotten in the future", "I don’t think I can judge them to be honest", "it is in the human nature to not care about stuff", "that doesn’t affect them");
        // change later for text No survivors
        yield return Final(0);
    }

    private IEnumerator DeadOnWayToBorder()
    {
        // if !Cabin.HasCar
        yield return WriteList(timeBetweenTransitions, "I was feeling very tired", "I felt my leg tremble as I walked", " I blinked and now I’m dead", "Is it all my fault, the raiders?", "everyone they took, was it my fault?", "or it was destined to happen?", "We were at war", "How I could’ve done better?", "I guess theres no more chances for me", "For those who I met in the last days", "I’m sorry", "I’m truly sorry");
        yield return Final(0);
    }

    private IEnumerator PlayerMadeItToBorder()
    {
        // player bob twins
        if (Character.Bob.AliveAtBorder && Character.Sal.AliveAtBorder && Character.Hal.AliveAtBorder)
        {
            // Twins and  bob are alive
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "Bob looked over those kids.", "Raised them like he was never raised", "and they taught him a thing or two", "about patience", "about trust", "about being human", "in a warzone.");
            yield return Final(4);
        }
        // player jessica twins
        else if (Character.Jessica.AliveAtBorder && Character.Sal.AliveAtBorder && Character.Hal.AliveAtBorder)
        {
            // Twins and  bob are alive
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "Jessica needed people", "her parents were gone", "and this big, new world was scary.", "Hal and Sal were ready", "they helped her learn", "they had some good memories", "but in the end, this country is at war.", "Rest in peace, Jessica.");
            yield return Final(4);
        }
        // player violet twins
        else if (Character.Violet.AliveAtBorder && Character.Sal.AliveAtBorder && Character.Hal.AliveAtBorder)
        {
            // Twins and  bob are alive
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "Violet opened a car shop", "she got the twins to keep her company", "they were quite handy", "eager to learn", "and tough as nails.", "Hard times", "make hard people.");
            yield return Final(4);
        }
        //player bob jessica
        else if(Character.Bob.AliveAtBorder && Character.Jessica.AliveAtBorder)
        {
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "I thought Bob was going to break his promise with Jessica", "taking care of Jessica even after reaching the border", "I was wrong", "Very wrong", "They are together now", "I guess that being stuck near someone", "you dislike for such a long time can change their views", " I guess it was worth it in the end");
            yield return Final(3);

        }
        //player bob violet
        else if(Character.Bob.AliveAtBorder && Character.Violet.AliveAtBorder)
        {
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "Violet opened a mechanic shop", "Violet promised Bob this job would turn his life around", "if his life didn’t change for the better then she would let him go to the army", "After some year I stopped there to see how they were going", "Strange, it doesn’t look like the same Bob I knew", "he wasn’t rude anymore", "he wasn’t polite either", "but I can see his making efforts", "It the first time I saw Bob smile", "I guess it was worth it in the end");
            yield return Final(3);

        }
        //player jessica violet
        else if(Character.Jessica.AliveAtBorder && Character.Violet.AliveAtBorder)
        {
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "Violet opened a mechanics shop", "she hired Jessica to keep an eye on her", "as she swore to take care of her", "The place has been going well", "I passed there after some years", "they were glad to see me", "I even got a job offer", "I guess it was worth it in the end");
            yield return Final(3);

        }
        else if (Character.Hal.AliveAtBorder && Character.Sal.AliveAtBorder)
        {
            // Twins are alive
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "Sal got better", "Hal got bigger.", "They were inseperable", "until", "Sal got sent to the front", "and there was", "nothing", "for Hal to do about it.", "They will never know a home.");
            yield return Final(3);
        }
        //dad ending
        else if (Character.Dad.AliveAtBorder)
        {
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "I was feeling very tired", "I felt my leg tremble as I walked", "I blinked and collapsed", "I was woken up being carried in a sledge by two people", "I heard familiar voices", "As my vision started to go back to normal", "The shapes became defined", "It was Mark and his kid ", "I thought I was done for", "He was happy that I woke up", " He said he found me lying in the ground unconscious", " With the gun I gave him he was able to defend himself until he found his son", " we are approaching the border now", " I guess that there are happy endings", "Even if they are few");
            yield return Final(3);
        }

        //has car alone
        else if (Cabin.HasCar)
        {
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "I reached my destination", "finally", "I’m at the border", "I’m alive", "they are all dead", " was it worth it?");
            yield return Final(1);
        }

        if (Character.Jessica.AliveAtBorder) 
        {
            Debug.Log("jessica only");
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "We reached the border safely", "finally", "I had to keep an eye on Jesssica", "when we were asked for our id", "they were surprised by Jessica's", "Turns out shes niece of one of the higher ups in Brasnia", "We were taken to a room, wand told to wait for someone", "Turns out we're getting evacuted to the USA for protection");
            yield return Final(2);
        }

        //Violet only
        if (Character.Violet.AliveAtBorder)
        {
            Debug.Log("violet only");
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "Turns out that years later violet oppened a workshop", "I went to visit her some years later", "Turns out she's doing pretty well", "I even got a job offer", "good to see that someone is doing pretty well after the war", "I guess it was worth it in the end");
            yield return Final(2);
        }

        //bob only
        if (Character.Bob.AliveAtBorder)
        {
            Debug.Log("bob only");
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "some months passed and the war still rages on", "Bob chose to return to the war and fight as a revolutionary", "I never heard from him ever again", "I hope he's doing alright");
            yield return Final(2);

        }


    }

    private IEnumerator JokeEnding()
    {
        yield return WriteList(timeBetweenTransitions, "I blinked again", "I saw a spaceship floating on above me", "I stated floating", "It was abducting me!", "Now I live in a super advanced society of various alien species", "i didnt want no ice cream", " take that suckers");
    }

    private IEnumerator SingleCharacterAtBorder(Character personAtBorder)
    {

        //Jessica only
        if (personAtBorder == Character.Jessica )
        {
            Debug.Log("jessica only");
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "We reached the border safely", "finally", "I had to keep an eye on Jesssica", "when we were asked for our id", "they were surprised by Jessica's", "Turns out shes niece of one of the higher ups in Brasnia", "We were taken to a room, wand told to wait for someone", "Turns out we're getting evacuted to the USA for protection");
            yield return Final(2);
        }

        //Violet only
        if (personAtBorder == Character.Violet)
        {
            Debug.Log("violet only");
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "Turns out that years later violet oppened a workshop", "I went to visit her some years later", "Turns out she's doing pretty well", "I even got a job offer", "good to see that someone is doing pretty well after the war", "I guess it was worth it in the end");
            yield return Final(2);
        }

        //bob only
        if (personAtBorder == Character.Bob)
        {
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "some months and the war still rages on", "Bob chose to return to the war and fight as a revolutionary", "I never heard from him ever again", "I hope he's doing alright");
            yield return Final(2);

        }

        //val and sal only

        if (personAtBorder == Character.Hal)
        {
            yield return Write6PeoplePreamble();

            yield return WriteList(timeBetweenTransitions, "Sal got better", "Hal got bigger.", "They were inseperable", "until", "Sal got sent to the front", "and there was", "nothing", "for Hal to do about it.", "They will never know a home.");
            yield return Final(3);
        }
    }



    private IEnumerator Write6PeoplePreamble()
    {
        yield return WriteList(timeBetweenTransitions, "Six people.", "Hard decisions were made.", "People were sent to their death;", "Only some left unharmed", "We", "survived.", "A new dawn rises;", "A new life starts.");
    }

    private IEnumerator Final(int survivors)
    {
        yield return new WaitForSeconds(5);

        Debug.Log("reacehd me");
        text.gameObject.SetActive(false);
        FinalText.text = "Markistan Border \n 12 / 05 / 2002 \n " + survivors + " survivors";
        FinalText.gameObject.SetActive(true);
    }

    IEnumerator WriteList(float delay, params string[] textToDisplay)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);
        for (int i = 0; i < textToDisplay.Length; i++)
        {
            text.text = textToDisplay[i];
            yield return wait;
        }
    }
}

