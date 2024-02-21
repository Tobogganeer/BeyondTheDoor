using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

namespace BeyondTheDoor.SaveSystem
{
    public class SaveState
    {
        private ByteBuffer savedData;

        /// <summary>
        /// Creates an empty SaveState ready for saving.
        /// </summary>
        public SaveState()
        {
            savedData = new ByteBuffer();
            SaveEmptyState();
        }

        /// <summary>
        /// Creates a SaveState with save data ready for loading.
        /// </summary>
        /// <param name="savedData"></param>
        public SaveState(ByteBuffer savedData)
        {
            this.savedData = savedData;
        }

        public void AddDataTo(ByteBuffer buffer)
        {
            buffer.AddBuffer(savedData);
        }

        public void SaveEmptyState()
        {
            SaveEmptyCabin(savedData);
            SaveEmptyDays(savedData);
            SaveEmptyCharacters(savedData);
            SaveEmptyLines(savedData);
        }

        public void SaveCurrentState()
        {
            SaveCabin(savedData);
            SaveDays(savedData);
            SaveCharacters(savedData);
            SaveLines(savedData);
        }

        public void Load()
        {
            // Load everything in the same order
            LoadCabin(savedData);
            LoadDays(savedData);
            LoadCharacters(savedData);
            LoadLines(savedData);
        }

        #region Empty Saving
        private static void SaveEmptyCabin(ByteBuffer buf)
        {
            buf.Add(true); // Has Shotgun
            buf.Add(false); // Has Car
        }

        private static void SaveEmptyDays(ByteBuffer buf)
        {
            buf.Add(1); // Day 1
            buf.Add(Stage.SpeakingWithParty); // First stage
        }

        private static void SaveEmptyCharacters(ByteBuffer buf)
        {
            buf.Add(0); // No characters
        }

        private static void SaveEmptyLines(ByteBuffer buf)
        {
            buf.Add(0); // No opened lines
        }
        #endregion

        #region Saving
        private static void SaveCabin(ByteBuffer buf)
        {
            buf.Add(Cabin.HasShotgun);
            buf.Add(Cabin.HasCar);
        }

        private static void SaveDays(ByteBuffer buf)
        {
            buf.Add(Day.DayNumber);
            buf.Add(Day.Stage);
        }

        private static void SaveCharacters(ByteBuffer buf)
        {
            // Write all characters, even if they might not have changed
            // TODO: Check how many HistoryEvents and maybe don't write characters with none?
            // They could still be changed though...
            buf.Add(Character.All.Count);
            foreach(Character ch in Character.All.Values)
            {
                AddCharacter(buf, ch);
            }
        }

        private static void SaveLines(ByteBuffer buf)
        {
            // Find out what lines we need to save
            List<Line> changedLines = new List<Line>();
            foreach (Line line in Line.All.Values)
            {
                if (line.timesOpened > 0)
                    changedLines.Add(line);
            }

            buf.Add(changedLines.Count);
            foreach (Line changedLine in changedLines)
            {
                // Write how many times the line's been opened
                buf.Add(changedLine.ID);
                buf.Add(changedLine.timesOpened);
            }
        }
        #endregion

        #region Loading
        private static void LoadCabin(ByteBuffer buf)
        {
            Cabin.HasShotgun = buf.Read<bool>();
            Cabin.HasCar = buf.Read<bool>();
        }

        private static void LoadDays(ByteBuffer buf)
        {
            Day.DayNumber = buf.Read<int>();
            Day.Stage = buf.Read<Stage>();
        }

        private static void LoadCharacters(ByteBuffer buf)
        {
            Character.ResetAll();
            int count = buf.Read<int>();
            for (int i = 0; i < count; i++)
            {
                LoadCharacter(buf);
            }
        }

        private static void LoadLines(ByteBuffer buf)
        {
            int changedLines = buf.Read<int>();
            for (int i = 0; i < changedLines; i++)
            {
                LineID lineID = buf.Read<LineID>();
                if (Line.All.TryGetValue(lineID, out Line line))
                {
                    // All that we store is the times these lines were opened
                    line.timesOpened = buf.Read<int>();
                }
                else
                {
                    throw new SaveSystemException("Could not find line with ID " + lineID);
                }
            }
        }
        #endregion

        private static void AddCharacter(ByteBuffer buf, Character c)
        {
            buf.Add(c.ID);
            buf.AddString(c.Name);
            buf.Add(c.Status);
            buf.Add(c.HistoryEvents.Count);
            foreach (var historyEvent in c.HistoryEvents)
                buf.AddStruct(new BufCharacterHistoryEvent(historyEvent));
        }

        private static void LoadCharacter(ByteBuffer buf)
        {
            CharacterID id = buf.Read<CharacterID>();
            if (Character.All.TryGetValue(id, out Character ch))
            {
                // All characters have already been reset by here
                ch.Name = buf.Read();
                ch.ChangeStatus(buf.Read<CharacterStatus>(), false);
                int numHistoryEvents = buf.Read<int>();
                ch.HistoryEvents = new List<CharacterHistoryEvent>(numHistoryEvents);
                for (int i = 0; i < numHistoryEvents; i++)
                    ch.HistoryEvents.Add(buf.ReadStruct<BufCharacterHistoryEvent>().ToHistoryEvent());
            }
            else
            {
                throw new SaveSystemException("Could not find character with ID " + id);
            }
        }

        // Gotta avoid circular dependencies with assemblies
        private class BufCharacterHistoryEvent : IBufferStruct
        {
            private CharacterStatus oldStatus;
            private CharacterStatus newStatus;
            private int day;
            private Stage stage;

            public BufCharacterHistoryEvent() { }

            public BufCharacterHistoryEvent(CharacterHistoryEvent ev)
            {
                oldStatus = ev.OldStatus;
                newStatus = ev.NewStatus;
                day = ev.Day;
                stage = ev.Stage;
            }

            public CharacterHistoryEvent ToHistoryEvent()
            {
                return new CharacterHistoryEvent(oldStatus, newStatus, day, stage);
            }

            public void Serialize(ByteBuffer buf)
            {
                buf.Add(oldStatus);
                buf.Add(newStatus);
                buf.Add(day);
                buf.Add(stage);
            }

            public void Deserialize(ByteBuffer buf)
            {
                oldStatus = buf.Read<CharacterStatus>();
                newStatus = buf.Read<CharacterStatus>();
                day = buf.Read<int>();
                stage = buf.Read<Stage>();
            }
        }
    }
}
