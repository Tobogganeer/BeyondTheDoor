using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBOE.Dialogue
{
    public class Choice
    {
        public Line Prompt { get; private set; }
        public IDialogueElement FollowingElement { get; private set; }
        public event Action<Line> OnChosen;

        public Choice(Line prompt, IDialogueElement followingElement, Action<Line> onChosen)
        {
            Prompt = prompt;
            FollowingElement = followingElement;
            OnChosen = onChosen;
        }

        /// <summary>
        /// Opens the <paramref name="followingElement"/> if chosen.
        /// </summary>
        /// <param name="prompt">The line displayed for the player to click.</param>
        /// <param name="followingElement">The line that will be opened if chosen.</param>
        /// <returns></returns>
        public static Choice Line(Line prompt, IDialogueElement followingElement)
        {
            return new Choice(prompt, followingElement, null);
        }

        /// <summary>
        /// Calls the <paramref name="onChosen"/> callback if chosen.
        /// </summary>
        /// <param name="prompt">The line displayed for the player to click.</param>
        /// <param name="onChosen">The function called if chosen. The <paramref name="prompt"/> is passed through.</param>
        /// <returns></returns>
        public static Choice Action(Line prompt, Action<Line> onChosen)
        {
            return new Choice(prompt, null, onChosen);
        }

        /// <summary>
        /// Opens the <paramref name="followingElement"/> and calls the <paramref name="onChosen"/> callback if chosen.
        /// </summary>
        /// <param name="prompt">The line displayed for the player to click.</param>
        /// <param name="followingElement">The line that will be opened if chosen.</param>
        /// <param name="onChosen">The function called if chosen. The <paramref name="prompt"/> is passed through.</param>
        /// <returns></returns>
        public static Choice LineAndAction(Line prompt, IDialogueElement followingElement, Action<Line> onChosen)
        {
            return new Choice(prompt, followingElement, onChosen);
        }


        /// <summary>
        /// Called when this choice has been selected.
        /// </summary>
        internal void OnChoiceChosen()
        {
            // Call back to user code first
            OnChosen?.Invoke(Prompt);

            if (FollowingElement != null)
                FollowingElement.Open();
        }
    }

    public class ChoiceCollection : IDialogueElement
    {
        public List<Choice> Choices { get; private set; }

        public ChoiceCollection(List<Choice> choices)
        {
            Choices = choices;
        }

        public ChoiceCollection(Choice[] choices)
        {
            Choices = new List<Choice>(choices);
        }


        public void Open()
        {
            // TODO: Implementation
            throw new NotImplementedException();
        }
    }
}
