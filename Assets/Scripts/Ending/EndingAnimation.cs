using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class EndingAnimation : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI FinalText;
    [SerializeField] private TextMeshProUGUI Logo;
    [SerializeField] private float timeBetweenTransitions;
    [SerializeField] private Image image;
    [SerializeField] private int survivors;

    List<Character> GetCharactersAtBorder() => Character.All.Values.Where((ch) => ch.Status == CharacterStatus.AliveAtBorder).ToList();

    IEnumerator Start()
    {

        Character.Jessica.ChangeStatus(CharacterStatus.AliveAtBorder, true);

        List<Character> peopleAtTheBorder = GetCharactersAtBorder();
        bool only1atBorder = peopleAtTheBorder.Count == 1;
        Character personAtBorder = peopleAtTheBorder[0];



        if (Day.DayNumber == 5 && !Cabin.HasScavengedSuccessfully)
        {
            yield return WriteList(timeBetweenTransitions,"Jessica didn't return from the scavenging", "We've ran out of water, food and heating for days now", "I fear I'm not going to last long", "Somebody", "help","please" );
        }
        //player no car 
        if (Character.Player.Status == CharacterStatus.DeadOnWayToBorder && !Cabin.HasCar)
        {
            Debug.Log("died on the way");
            yield return WriteList(timeBetweenTransitions, "I was feeling very tired", "I felt my leg tremble as I walked", " I blinked and now I’m dead", "Is it all my fault, the raiders?", "everyone they took, was it my fault?", "or it was destined to happen?", "We were at war", "How I could’ve done better?", "I guess theres no more chances for me", "For those who I met in the last days", "I’m sorry", "I’m truly sorry");
            survivors = 0;
            Final();

        }
        //player dead by bear
        if (Character.Player.Status == CharacterStatus.KilledByBear)
        {
            yield return WriteList(timeBetweenTransitions, "What the fuck was going on my head", "for me to let a bear get inside my house","I’m dead now","What makes me even sadder", "is that I’m just a number now","like those who where caught between conflicting ideas","my death is irrelevant", "as it will all be forgotten in the future","I don’t think I can judge them to be honest", "it is in the human nature to not care about stuff", "that doesn’t affect them");
            // change later for text No survivors
            survivors = 0;
            Final();
        }
        if(Character.Player.Status == CharacterStatus.AliveAtBorder && Character.Bob.Status == CharacterStatus.AliveAtBorder && Character.Sal.Status == CharacterStatus.AliveAtBorder && Character.Hal.Status == CharacterStatus.AliveAtBorder)
        {
            //implement later
        }
        //player bob jessica
        if (Character.Player.Status == CharacterStatus.AliveAtBorder && Character.Bob.Status == CharacterStatus.AliveAtBorder && Character.Jessica.Status == CharacterStatus.AliveAtBorder)
        {
            yield return WriteList(timeBetweenTransitions, "6 people", "Hard decisions were made", "People were sent to their death", "Only some left unharmed", "We", "survived", "A new dawn rises", "A new life starts");

            yield return WriteList(timeBetweenTransitions, "I thought Bob was going to break his promise with Jessica", "taking care of Jessica even after reaching the border", "I was wrong", "Very wrong", "They are together now", "I guess that being stuck near someone", "you dislike for such a long time can change their views", " I guess it was worth it in the end");
            survivors = 3;
            yield return new WaitForSeconds(5);
            Final();

        }
        //player bob violet
        if (Character.Player.Status == CharacterStatus.AliveAtBorder && Character.Bob.Status == CharacterStatus.AliveAtBorder && Character.Violet.Status == CharacterStatus.AliveAtBorder)
        {
            yield return WriteList(timeBetweenTransitions, "6 people", "Hard decisions were made", "People were sent to their death", "Only some left unharmed", "We", "survived", "A new dawn rises", "A new life starts");

            yield return WriteList(timeBetweenTransitions, "Violet opened a mechanic shop", "Violet promised Bob this job would turn his life around", "if his life didn’t change for the better then she would let him go to the army", "After some year I stopped there to see how they were going", "Strange, it doesn’t look like the same Bob I knew", "he wasn’t rude anymore", "he wasn’t polite either", "but I can see his making efforts", "It the first time I saw Bob smile", "I guess it was worth it in the end");
            survivors = 3;
            yield return new WaitForSeconds(5);
            Final();

        }
        //player jessica violet
        if (Character.Player.Status == CharacterStatus.AliveAtBorder && Character.Jessica.Status == CharacterStatus.AliveAtBorder && Character.Violet.Status == CharacterStatus.AliveAtBorder)
        {
            yield return WriteList(timeBetweenTransitions, "6 people", "Hard decisions were made", "People were sent to their death", "Only some left unharmed", "We", "survived", "A new dawn rises", "A new life starts");

            yield return WriteList(timeBetweenTransitions, "Violet opened a mechanics shop", "she hired Jessica to keep an eye on her", "as she swore to take care of her", "The place has been going well", "I passed there after some years", "they were glad to see me", "I even got a job offer", "I guess it was worth it in the end");
            survivors = 3;
            yield return new WaitForSeconds(5);
            Final();

        }
        //dad ending
        if (Character.Player.Status == CharacterStatus.AliveAtBorder && Character.Dad.Status == CharacterStatus.LeftWithShotgun)
        {
            yield return WriteList(timeBetweenTransitions, "6 people", "Hard decisions were made", "People were sent to their death", "Only some left unharmed", "We", "survived", "A new dawn rises", "A new life starts");

            yield return WriteList(timeBetweenTransitions, "I was feeling very tired", "I felt my leg tremble as I walked", "I blinked and collapsed", "I was woken up being carried in a sledge by two people", "I heard familiar voices", "As my vision started to go back to normal", "The shapes became defined", "It was Mark and his kid ", "I thought I was done for", "He was happy that I woke up", " He said he found me lying in the ground unconscious", " With the gun I gave him he was able to defend himself until he found his son", " we are approaching the border now", " I guess that there are happy endings", "Even if they are few");
            survivors = 3;
            yield return new WaitForSeconds(5);
            Final();
        }
        //has car alone
        if (Character.Player.Status == CharacterStatus.AliveAtBorder && Cabin.HasCar)
        {
            yield return WriteList(timeBetweenTransitions, "6 people", "Hard decisions were made", "People were sent to their death", "Only some left unharmed", "We", "survived", "A new dawn rises", "A new life starts");

            yield return WriteList(timeBetweenTransitions, "I reached my destination", "finally", "I’m at the border", "I’m alive", "they are all dead", " was it worth it?");
            survivors = 1;
            yield return new WaitForSeconds(5);
            Final();
        }

        //Jessica only
        if(only1atBorder && personAtBorder == Character.Jessica)
        {
            yield return WriteList(timeBetweenTransitions, "6 people", "Hard decisions were made", "People were sent to their death", "Only some left unharmed", "We", "survived", "A new dawn rises", "A new life starts");

            yield return WriteList(timeBetweenTransitions, "We reached the border safely", "finally", "I had to keep an eye on Jesssica", "when we were asked for our id", "they were surprised by Jessica's", "Turns out shes niece of one of the higher ups in Brasnia", "We were taken to a room, wand told to wait for someone", "Turns out we're getting evacuted to the USA for protection");
            survivors = 2;
            yield return new WaitForSeconds(5);
            Final();
        }

        //Violet only
        if(only1atBorder && personAtBorder == Character.Violet)
        {
            yield return WriteList(timeBetweenTransitions, "6 people", "Hard decisions were made", "People were sent to their death", "Only some left unharmed", "We", "survived", "A new dawn rises", "A new life starts");

            yield return WriteList(timeBetweenTransitions, "Turns out that years later violet oppened a workshop", "I went to visit her some years later", "Turns out she's doing pretty well", "I even got a job offer", "good to see that someone is doing pretty well after the war", "I guess it was worth it in the end");
            survivors = 2;
            yield return new WaitForSeconds(5);
            Final();
        }

        //bob only
        if(only1atBorder && personAtBorder == Character.Bob)
        {
            yield return WriteList(timeBetweenTransitions, "6 people", "Hard decisions were made", "People were sent to their death", "Only some left unharmed", "We", "survived", "A new dawn rises", "A new life starts");

            yield return WriteList(timeBetweenTransitions, "some months and the war still rages on", "Bob chose to return to the war and fight as a revolutionary", "I never heard from him ever again", "I hope he's doing alright");
            survivors = 2;
            yield return new WaitForSeconds(5);
            Final();
        }

        //val and sal only
   
        if(personAtBorder == Character.Sal)
        {
            //DO STUFF LATER    
        }
        //Joke ending

        if (Character.Tutorial_Dad.Status == CharacterStatus.LeftOutside && Character.Tutorial_Mom.Status == CharacterStatus.LeftOutside)
        {
             yield return WriteList(timeBetweenTransitions,"I blinked again", "I saw a spaceship floating on above me", "I stated floating", "It was abducting me!", "Now I live in a super advanced society of various alien species", "i didnt want no ice cream" , " take that suckers");
        }
      
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
    private void Final()
    {
        Debug.Log("reacehd me");
        text.gameObject.SetActive(false);
        FinalText.text = "Markistan Border <br> 12 / 05 / 2002 <br> " + survivors + " survivors";
        FinalText.gameObject.SetActive(true);
    }
}

