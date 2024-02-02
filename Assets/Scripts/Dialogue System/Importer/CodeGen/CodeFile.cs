using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToBOE.Dialogue.Importer.CodeGen
{
    public class CodeFile
    {
        const int AutoGenPreambleMinLength = 50;
        const int AutoGenPreambleNameSpacing = 8;

        public string FileName { get; private set; }
        public string FolderPath { get; private set; }
        public string FullPath { get; private set; }
        
        public int IndentLevel { get; private set; }

        private StringBuilder builder;

        public CodeFile(string fileName, string relativeFolderPath)
        {
            FileName = fileName.ToLower().EndsWith(".cs") ? fileName : fileName + ".cs";
            FolderPath = Path.Combine(Application.dataPath, relativeFolderPath);
            FullPath = Path.Combine(FolderPath, FileName);

            IndentLevel = 0;
            builder = new StringBuilder();
        }

        /// <summary>
        /// Adds a 'using (namespace);' statement.
        /// </summary>
        /// <param name="_namespace"></param>
        public void AddUsingDirective(string _namespace)
        {
            builder.Append("using ").Append(_namespace).AppendLine(";");
        }

        /// <summary>
        /// Adds a note that the file is pre-generated and should not be edited.
        /// </summary>
        public void AddAutoGeneratedPreamble()
        {
            // Ensure we have enough space
            int lineLength = Mathf.Max(AutoGenPreambleMinLength, FileName.Length + AutoGenPreambleNameSpacing);

            builder.Append("// ").Append('=', lineLength - 1); // Top row
            builder.AppendLine();
            AppendPreambleLine(builder, lineLength, ""); // Empty spacer line
            AppendPreambleLine(builder, lineLength, FileName);
            AppendPreambleLine(builder, lineLength, ""); // Spacer
            AppendPreambleLine(builder, lineLength, "AUTO-GENERATED FILE - Do not edit!");
            AppendPreambleLine(builder, lineLength, ""); // Spacer
            builder.Append("// ").Append('=', lineLength - 1); // Bottom row
            builder.AppendLine().AppendLine(); // Trailing newlines

            static void AppendPreambleLine(StringBuilder builder, int lineLength, string text)
            {
                builder.Append("// = ").Append(text);

                int textLength = text.Length;
                textLength += 3; // Beginning // and space
                int spaces = lineLength - textLength;
                spaces--; // End =

                builder.Append(' ', spaces).Append('=');
                builder.AppendLine();
            }
        }

        /// <summary>
        /// Apply tabs up until the current level of indentation.
        /// </summary>
        /// <returns></returns>
        public StringBuilder ApplyIndent()
        {
            // Pick your poison
            //return builder.Append('\t', IndentLevel);
            return builder.Append(' ', IndentLevel * 4);
        }

        public void IncreaseIndent(int increase = 1) => IndentLevel = Mathf.Max(IndentLevel + increase, 0);
        public void DecreaseIndent(int decrease = 1) => IndentLevel = Mathf.Max(IndentLevel - decrease, 0);

        /// <summary>
        /// Adds a comment
        /// </summary>
        /// <param name="comment"></param>
        public void AddComment(string comment)
        {
            ApplyIndent().Append("// ").AppendLine(comment);
        }

        /// <summary>
        /// Adds an empty line.
        /// </summary>
        public void Space()
        {
            builder.AppendLine();
        }

        /// <summary>
        /// Adds empty lines.
        /// </summary>
        /// <param name="numSpaces"></param>
        public void Space(int numSpaces)
        {
            for (int i = 0; i < numSpaces; i++)
                builder.AppendLine();
        }

        /// <summary>
        /// Writes the specified access modifiers.
        /// </summary>
        /// <param name="mods"></param>
        /// <returns></returns>
        public StringBuilder AddModifiers(Modifiers mods)
        {
            if (mods == Modifiers.None) return builder;
            if (mods.HasFlag(Modifiers.Private)) builder.Append("private ");
            if (mods.HasFlag(Modifiers.Public)) builder.Append("public ");
            if (mods.HasFlag(Modifiers.Protected)) builder.Append("protected ");
            if (mods.HasFlag(Modifiers.Static)) builder.Append("static ");
            if (mods.HasFlag(Modifiers.Virtual)) builder.Append("virtual ");
            if (mods.HasFlag(Modifiers.Abstract)) builder.Append("abstract ");
            if (mods.HasFlag(Modifiers.Partial)) builder.Append("partial ");

            return builder;
        }

        /// <summary>
        /// Adds a namespace block.
        /// </summary>
        /// <param name="_namespace"></param>
        /// <returns></returns>
        public Scope Namespace(string _namespace)
        {
            builder.Append("namespace ").Append(_namespace);
            return new Scope(this);
        }

        public Scope Class(Modifiers mods, string className)
        {
            ApplyIndent();
            AddModifiers(mods);
            builder.Append("class ").Append(className);
            return new Scope(this);
        }

        public Scope Method(Modifiers mods, string returnType, string methodName, string args)
        {
            ApplyIndent();
            AddModifiers(mods);
            builder.Append(returnType).Append(' ').Append(methodName).Append('(').Append(args).Append(')');
            return new Scope(this);
        }

        public Scope Enum(Modifiers mods, string enumName, string backingType = null)
        {
            ApplyIndent();
            AddModifiers(mods);
            builder.Append("enum ").Append(enumName);
            if (backingType != null)
                builder.Append(" : ").Append(backingType);
            return new Scope(this);
        }

        public CodeFile EnumValue(string name, int? value = null)
        {
            ApplyIndent();
            builder.Append(name);
            if (value != null)
                builder.Append(" = ").Append(value);
            builder.AppendLine(",");
            return this;
        }

        public void SaveToDisk()
        {
            // Add trailing newline because I feel like it
            Space();

            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            File.WriteAllText(FullPath, builder.ToString());

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }


        public StringBuilder GetStringBuilder() { return builder; }


        [Flags]
        public enum Modifiers
        {
            None = 0,
            Private = 1 << 0,
            Public = 1 << 1,
            Protected = 1 << 2,
            Static = 1 << 3,
            Virtual = 1 << 4,
            Abstract = 1 << 5,
            Partial = 1 << 6,
        }

        public class Scope : IDisposable
        {
            private bool isDisposed;
            private CodeFile cf;
            private bool endWithSemicolon;

            public Scope(CodeFile codeFile, bool endWithSemicolon = false)
            {
                cf = codeFile;
                cf.builder.AppendLine();
                cf.ApplyIndent().AppendLine("{");
                cf.IncreaseIndent();
                this.endWithSemicolon = endWithSemicolon;
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!isDisposed)
                {
                    if (disposing)
                    {
                        // End the scope
                        //cf.builder.AppendLine();
                        cf.DecreaseIndent();
                        if (endWithSemicolon)
                            cf.ApplyIndent().AppendLine("};");
                        else
                            cf.ApplyIndent().AppendLine("}");
                    }

                    isDisposed = true;
                }
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
