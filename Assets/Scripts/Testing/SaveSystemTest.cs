using BeyondTheDoor.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemTest : MonoBehaviour
{
    void Start()
    {
        RunTest(BufferCopy, nameof(BufferCopy));
        RunTest(SaveLoad, nameof(SaveLoad));
    }

    void RunTest(Action test, string testName)
    {
        try
        {
            test?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"'{testName}' failed: {ex}");
        }
    }

    void BufferCopy()
    {
        ByteBuffer write = new ByteBuffer();
        write.AddString("Buffer Copy - ");
        write.Add(5);
        ByteBuffer intermediate = new ByteBuffer();
        intermediate.AddBuffer(write);
        ByteBuffer read = intermediate.GetBuffer();
        Debug.Log(read.Read() + read.Read<int>());
    }

    void SaveLoad()
    {
        ByteBuffer save = new ByteBuffer();
        save.AddString("Save Load - ");
        save.Add(18);
        save.Add(DateTime.Now.Ticks);
        SaveSystem.SaveBuffer(save, 9);

        ByteBuffer load = SaveSystem.LoadBuffer(9);
        Debug.Log(load.Read() + load.Read<int>());
        Debug.Log("Time: " + new DateTime(load.Read<long>(), DateTimeKind.Local));
    }
}
