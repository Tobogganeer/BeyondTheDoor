using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;
using System;

namespace BeyondTheDoor.SaveSystem
{
    public class SaveState
    {
        public DateTime SaveTime { get; private set; }
        public int GameDay { get; private set; }
        public Stage GameStage { get; private set; }

        private ByteBuffer data;

        /// <summary>
        /// Creates an empty SaveState ready for saving.
        /// </summary>
        public SaveState()
        {
            data = new ByteBuffer();
        }

        /// <summary>
        /// Creates a SaveState with save data ready for loading.
        /// </summary>
        /// <param name="savedData"></param>
        public SaveState(ByteBuffer savedData)
        {
            this.data = savedData;
            SaveTime = new DateTime(savedData.Read<long>());
            GameDay = savedData.Read<int>();
            GameStage = savedData.Read<Stage>();
        }

        public void AddDataTo(ByteBuffer buffer)
        {
            buffer.AddBuffer(data, false);
        }

        public void SaveEmptyState(bool playTutorial)
        {
            data.Add(DateTime.Now.Ticks); // Current save time
            SaveEmptyDay(playTutorial);
            SaveEmptyCabin();
            SaveEmptyCharacters();
            SaveEmptyLines();
        }

        public void SaveCurrentState()
        {
            data.Add(DateTime.Now.Ticks); // Current save time
            SaveDay();
            SaveCabin();
            SaveCharacters();
            SaveLines();
        }

        public void Load()
        {
            // Load everything in the same order
            LoadDay();
            LoadCabin();
            LoadCharacters();
            LoadLines();
        }

        #region Empty Saving
        private void SaveEmptyDay(bool playTutorial)
        {
            data.Add(playTutorial ? 0 : 1); // Day 0/1 depending on whether we are doing the tutorial
            data.Add(Stage.SpeakingWithParty); // First stage
        }

        private void SaveEmptyCabin()
        {
            data.Add(true); // Has Shotgun
            data.Add(false); // Has Car
            data.Add(false); // Has Successfully Scavenged
        }

        private void SaveEmptyCharacters()
        {
            data.Add(0); // No characters
        }

        private void SaveEmptyLines()
        {
            data.Add(0); // No opened lines
        }
        #endregion

        #region Saving
        private void SaveDay()
        {
            data.Add(Day.DayNumber);
            data.Add(Day.Stage);
        }

        private void SaveCabin()
        {
            data.Add(Cabin.HasShotgun);
            data.Add(Cabin.HasCar);
            data.Add(Cabin.HasScavengedSuccessfully);
        }

        private void SaveCharacters()
        {
            // Write all characters, even if they might not have changed
            // TODO: Check how many HistoryEvents and maybe don't write characters with none?
            // They could still be changed though...
            data.Add(Character.All.Count);
            foreach(Character ch in Character.All.Values)
            {
                AddCharacter(data, ch);
            }
        }

        private void SaveLines()
        {
            // Find out what lines we need to save
            List<Line> changedLines = new List<Line>();
            foreach (Line line in Line.All.Values)
            {
                if (line.timesOpened > 0)
                    changedLines.Add(line);
            }

            data.Add(changedLines.Count);
            foreach (Line changedLine in changedLines)
            {
                // Write how many times the line's been opened
                data.Add(changedLine.ID);
                data.Add(changedLine.timesOpened);
            }
        }
        #endregion

        #region Loading
        private void LoadDay()
        {
            //Day.DayNumber = buf.Read<int>();
            //Day.Stage = buf.Read<Stage>();
            Day.DayNumber = GameDay;
            Day.Stage = GameStage;
        }

        private void LoadCabin()
        {
            Cabin.HasShotgun = data.Read<bool>();
            Cabin.HasCar = data.Read<bool>();
            Cabin.HasScavengedSuccessfully = data.Read<bool>();
        }

        private void LoadCharacters()
        {
            Character.ResetAll();
            int count = data.Read<int>();
            for (int i = 0; i < count; i++)
            {
                LoadCharacter(data);
            }
        }

        private void LoadLines()
        {
            int changedLines = data.Read<int>();
            for (int i = 0; i < changedLines; i++)
            {
                LineID lineID = data.Read<LineID>();
                if (Line.All.TryGetValue(lineID, out Line line))
                {
                    // All that we store is the times these lines were opened
                    line.timesOpened = data.Read<int>();
                }
                else
                {
                    throw new SaveSystemException("Could not find line with ID " + lineID);
                }
            }
        }
        #endregion

        private void AddCharacter(ByteBuffer buf, Character c)
        {
            buf.Add(c.ID);
            buf.AddString(c.Name);
            buf.Add(c.Status);
            buf.Add(c.HistoryEvents.Count);
            foreach (var historyEvent in c.HistoryEvents)
                buf.AddStruct(new BufCharacterHistoryEvent(historyEvent));
        }

        private void LoadCharacter(ByteBuffer buf)
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
