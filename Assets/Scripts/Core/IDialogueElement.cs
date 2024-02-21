using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor
{
    public interface IDialogueElement
    {
        /// <summary>
        /// Opens and displays this element on screen.
        /// </summary>
        void Open();
    }
}
