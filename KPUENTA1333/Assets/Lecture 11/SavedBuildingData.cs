using System;
using System.Text;
using UnityEngine;

public interface ISaveData
{
    // Serialize the data for storage
    public string OnSave(ISaveData dataToSerialize);

    // Deserialize the data into an object
    public ISaveData OnLoad(string serializedData);
}

[Serializable()]
public class SavedBuildingData : ISaveData
{
    public int CurrentHp;
    public int BuildingLevel;
    public int OwnerId;
    public Vector3 Position;
    public BuildingType KindOfType;

    public string OnSave(ISaveData dataToSerialize)
    {
        string deserialized = "";
        // todo
        return deserialized;
    }

    public ISaveData OnLoad(string serializedData)
    {
        SavedBuildingData dataFromSave = new SavedBuildingData();
        // todo
        return dataFromSave;
    }
}

[Serializable]
public class SaveHpToText : ISaveData
{
    public int CurrentHp;

    public SaveHpToText(int hp)
    {
        CurrentHp = hp;
    }

    public string OnSave(ISaveData dataToSerialize)
    {
        string serialized = "";
        if (dataToSerialize is SaveHpToText hpObj)
        {
            serialized = hpObj.CurrentHp.ToString();
            // should be "3" or equivilant
        }
        return serialized;
    }

    public ISaveData OnLoad(string serializedData)
    {
        SaveHpToText deserialized = null;
        if (int.TryParse(serializedData, out int num))
        {
            deserialized = new SaveHpToText(num);
        }
        return deserialized;
    }

}

[Serializable]
public class SaveHpToBytes
{
    public int CurrentHp;

    public byte[] OnSave(ISaveData dataToSerialize)
    {
        byte[] serialized = new byte[0];
        if (dataToSerialize is SaveHpToText hpObj)
        {
            serialized = BitConverter.GetBytes(hpObj.CurrentHp);
            // should be "3" or equivilant
        }
        return serialized;
    }

    public ISaveData OnLoad(string serializedData)
    {
        SaveHpToText deserialized = null;
        if (int.TryParse(serializedData, out int num))
        {
            deserialized = new SaveHpToText(num);
        }
        return deserialized;
    }

}