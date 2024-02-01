using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ToBOE.Dialogue.Importer.CodeGen.CodeFile;

namespace ToBOE.Dialogue.Importer.CodeGen
{
    /// <summary>
    /// Wrapper for a CodeFile used to generate enums.
    /// </summary>
    public class EnumGenerator
    {
        public string Namespace { get; set; }
        public string BackingType { get; set; }
        public Modifiers Modifiers { get; set; }

        CodeFile cf;
        string name;
        List<Member> members;


        public EnumGenerator(string name, string folder, string _namespace = null, Modifiers mods =
            Modifiers.Public, string backingType = null)
        {
            cf = new CodeFile(name, folder);
            cf.AddAutoGeneratedPreamble();

            members = new List<Member>();

            this.name = name;
            Modifiers = mods;
            Namespace = _namespace;
            BackingType = backingType;
        }

        public void Add(string name, int? value = null)
        {
            members.Add(new Member { Name = name, Value = value });
        }

        public void Generate()
        {
            // Ugly code repetition, but idc really

            Scope namespaceScope = null;

            if (!string.IsNullOrEmpty(Namespace))
                namespaceScope = cf.Namespace(Namespace);

            using (cf.Enum(Modifiers, name, string.IsNullOrEmpty(BackingType) ? null : BackingType))
            {
                foreach (Member m in members)
                    cf.EnumValue(m.Name, m.Value);
            }

            namespaceScope?.Dispose();

            cf.SaveToDisk();
        }

        struct Member
        {
            public string Name { get; set; }
            public int? Value { get; set; }
        }
    }
}
