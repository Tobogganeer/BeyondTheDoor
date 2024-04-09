using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor
{
    public class ConversationVariables
    {
        static readonly Dictionary<string, Var> VarMap = new Dictionary<string, Var>()
        {
            { "HasCar", Var.HarCar },
            { "Tutorial_Mom", Var.Tutorial_Mom },
            { "Tutorial_Dad", Var.Tutorial_Dad },
            { "Jessica", Var.Jessica },
            { "Bob", Var.Bob },
            { "Violet", Var.Violet },
            { "Hal", Var.Hal },
            { "Sal", Var.Sal },
            { "Dad", Var.Dad },
        };

        public static bool IsTrue(string condition)
        {
            condition = condition.Trim();

            if (string.IsNullOrEmpty(condition)) return false;

            string[] conditions = condition.Split("&&");

            // TODO: Dear god make this actually proper
            if (condition.Length == 1)
                return IsSingleConditionTrue(condition);
            else
                return IsSingleConditionTrue(conditions[0]) && IsSingleConditionTrue(conditions[1]);
        }

        private static bool IsSingleConditionTrue(string condition)
        {
            condition = condition.Trim();

            if (!VarMap.TryGetValue(condition, out var var)) return false;

            return IsTrue(var);
        }

        public static bool IsTrue(Var var)
        {
            if (var == Var.None) return false;
            else if (var == Var.HarCar) return Cabin.HasCar;
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
            // Use same numbers as CharacterIDs to make matching easier
            Tutorial_Mom = -546722264,
            Tutorial_Dad = -486113334,
            Jessica = -1608546876,
            Bob = 1502599511,
            Violet = 990541821,
            Hal = 696030773,
            Sal = 696031022,
            Dad = 696030393,
        }
    }
}
