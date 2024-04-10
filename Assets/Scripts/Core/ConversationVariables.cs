using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor
{
    public class ConversationVariables
    {
        static readonly Dictionary<string, Var> VarMap = new Dictionary<string, Var>()
        {
            { "hascar", Var.HarCar },
            { "car", Var.HarCar },
            { "hasshotgun", Var.HasShotgun },
            { "shotgun", Var.HasShotgun },
            { "onepartymember", Var.OnePartyMember },
            { "tutorial_mom", Var.Tutorial_Mom },
            { "tutorial_dad", Var.Tutorial_Dad },
            { "jessica", Var.Jessica },
            { "bob", Var.Bob },
            { "violet", Var.Violet },
            { "hal", Var.Hal },
            { "sal", Var.Sal },
            { "dad", Var.Dad },
            {"bobdead",Var.BobDead }
        };

        public static bool IsTrue(string condition)
        {
            condition = condition.Trim().ToLower();

            if (string.IsNullOrEmpty(condition)) return false;

            string[] conditions = condition.Split("&&");

            for (int i = 0; i < conditions.Length; i++)
            {
                if (!IsSingleConditionTrue(condition))
                    return false;
            }

            return true;
        }

        private static bool IsSingleConditionTrue(string condition)
        {
            condition = condition.Trim();
            bool invert = false;

            // Check if we want the condition flipped
            if (condition.StartsWith('!'))
            {
                invert = true;
                condition = condition.Substring(1, condition.Length - 1);
            }

            if (!VarMap.TryGetValue(condition, out var var)) return false;

            return invert ? !IsTrue(var) : IsTrue(var);
        }

        public static bool IsTrue(Var var)
        {
            if (var == Var.None) return false;
            else if (var == Var.HarCar) return Cabin.HasCar;
            else if (var == Var.HasShotgun) return Cabin.HasShotgun;
            else if (var == Var.OnePartyMember) return Cabin.NumCurrentPartyMembers() == 1;
            else if (var == Var.BobDead) return Character.Bob.Status == CharacterStatus.KilledByBear;
            else
                return CharacterPresent((CharacterID)var);
        }

        public static bool CharacterPresent(CharacterID character)
        {
            return Character.All[character].InsideCabin;
        }

        public enum Var
        {
            None,
            HarCar,
            HasShotgun,
            OnePartyMember,
            // Use same numbers as CharacterIDs to make matching easier
            Tutorial_Mom = -546722264,
            Tutorial_Dad = -486113334,
            Jessica = -1608546876,
            Bob = 1502599511,
            Violet = 990541821,
            Hal = 696030773,
            Sal = 696031022,
            Dad = 696030393,
            BobDead,
        }
    }
}
