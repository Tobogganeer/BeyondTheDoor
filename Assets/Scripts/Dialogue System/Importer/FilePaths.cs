using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.IO.Path;

namespace ToBOE.Dialogue
{
    public static class FilePaths
    {
        public static readonly string Assets = Application.dataPath;
        public static readonly string Scripts = Combine(Assets, "Scripts");
        public static readonly string Lines = Combine(Assets, "Lines");
        public static readonly string DialogueSystem = Combine(Scripts, "Dialogue System");
        public static readonly string Autogenerated = Combine(DialogueSystem, "Autogenerated");

        public static readonly string CharacterEnumFileName = "Character.cs";
        public static readonly string LineIDEnumFileName = "LineID.cs";
        public static readonly string LineStatusEnumFileName = "LineStatus.cs";

        public static readonly string DialogueNamespace = "ToBOE.Dialogue";
    }
}
