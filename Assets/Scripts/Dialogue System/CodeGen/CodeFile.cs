using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

namespace ToBOE.Dialogue.CodeGen
{
    public class CodeFile
    {
        public string FileName { get; private set; }
        public string FolderPath { get; private set; }
        public string FullPath { get; private set; }
        
        public uint IndentLevel { get; private set; }

        private StringBuilder builder;

        public CodeFile(string fileName, string relativeFolderPath)
        {
            FileName = fileName;
            FolderPath = Path.Combine(Application.dataPath, relativeFolderPath);
            FullPath = Path.Combine(FolderPath, FileName);

            IndentLevel = 0;
            builder = new StringBuilder();
        }
    }
}
