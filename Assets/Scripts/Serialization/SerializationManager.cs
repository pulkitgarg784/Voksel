using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SerializationManager
{
    public static bool Save(string path, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Create(path);
        formatter.Serialize(file,saveData);
        file.Close();
        return true;
    }

    public static object Load(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        try
        {
            object save = formatter.Deserialize(file);
            file.Close();
            return save;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at{0}", path);
            file.Close();
            return null;
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();
        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
        ColorSerializationSurrogate colorSurrogate = new ColorSerializationSurrogate();
        selector.AddSurrogate(typeof(Vector3),new StreamingContext(StreamingContextStates.All),vector3Surrogate);
        selector.AddSurrogate(typeof(Color),new StreamingContext(StreamingContextStates.All),colorSurrogate);

        formatter.SurrogateSelector = selector;
        return formatter;
    }
}
