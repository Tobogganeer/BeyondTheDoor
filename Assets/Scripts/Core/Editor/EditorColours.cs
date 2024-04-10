using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Properties;
using UnityEngine;

namespace BeyondTheDoor.Editor
{
    internal static class EditorColours
    {
        internal const string CharacterNameColour = "#4ed476";
        internal const string TextColour = "#d1d1d1";
        internal const string ConversationNameColour = "#4287f5";
        internal const string ChoicePromptColour = "#ed7e4e";
        internal const string CallbackColour = "#c975b1";
        internal const string ConditionalColour = "#c0de5f";
        internal const string GotoColour = "#c476e8";

        internal static string Format(string text, string colour)
        {
            return $"<color={colour}>{text}</color>";
        }

        internal static string CharacterName(string text) => Format(text, CharacterNameColour);
        internal static string Text(string text) => Format(text, TextColour);
        internal static string ConversationName(string text) => Format(text, CharacterNameColour);
        internal static string ChoicePrompt(string text) => Format(text, ChoicePromptColour);
        internal static string Callback(string text) => Format(text, CallbackColour);
        internal static string Conditional(string text) => Format(text, ConditionalColour);
        internal static string Goto(string text) => Format(text, GotoColour);
    }
}
