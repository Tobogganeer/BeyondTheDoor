using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBOE.Dialogue
{
    public class Choice
    {
        public Line Prompt { get; private set; }
        public Line FollowingLine { get; private set; }
        public event Action<Line> OnChosen;

        public Choice(Line prompt, Line followingLine, Action<Line> onChosen)
        {
            Prompt = prompt;
            FollowingLine = followingLine;
            OnChosen = onChosen;
        }

        /// <summary>
        /// Opens the <paramref name="followingLine"/> if chosen.
        /// </summary>
        /// <param name="prompt">The line displayed for the player to click.</param>
        /// <param name="followingLine">The line that will be opened if chosen.</param>
        /// <returns></returns>
        public static Choice Line(Line prompt, Line followingLine)
        {
            return new Choice(prompt, followingLine, null);
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
        /// Opens the <paramref name="followingLine"/> and calls the <paramref name="onChosen"/> callback if chosen.
        /// </summary>
        /// <param name="prompt">The line displayed for the player to click.</param>
        /// <param name="followingLine">The line that will be opened if chosen.</param>
        /// <param name="onChosen">The function called if chosen. The <paramref name="prompt"/> is passed through.</param>
        /// <returns></returns>
        public static Choice LineAndAction(Line prompt, Line followingLine, Action<Line> onChosen)
        {
            return new Choice(prompt, followingLine, onChosen);
        }
    }
}
