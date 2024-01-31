using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace ToBOE.Dialogue.CodeGen
{
    public class CodeFile
    {
        public string Path { get; private set; }
        public uint IndentLevel { get; private set; }


        private StringBuilder builder;
    }
}
