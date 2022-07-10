using System;
using System.IO;
using System.Text.Json;


public class SettingCopy
{
    public string[] Sources { get; set; }
    public string Target { get; set; }

    public SettingCopy()
    {
        Target = "";
        Sources = new string[0];
    }
    public SettingCopy(string target, string[] sources)
    {
        Target = target;
        Sources = new string[sources.Length];
        for (int i = 0; i < sources.Length; i++)
        {
            Sources[i] = sources[i];
        }
    }
}


public class Program
{
    public static void Main()
    {
        SaveJsonFile(); //создание файла настроек json
        SettingCopy setting = ReadJsonFile(); //копирование настроек из файла настроек json

        for (int i = 0; i < setting.Sources.Length; i++)
        {
            string[] files = Directory.GetFiles(setting.Sources[i]);
            if (!Directory.Exists(setting.Target)) 
            {
                Directory.CreateDirectory(setting.Target);
            }
            for (int y = 0; y < files.Length; y++)
            {
                File.Copy(files[y], Path.Combine(setting.Target, Path.GetFileName(files[y])), true);
            }
        }
    }

    /// <summary>
    /// Создание Json файла c нужными ссылками
    /// </summary>
    public static void SaveJsonFile() 
    {
        SettingCopy setting = new SettingCopy("SaveFile", new string[] { "source1", "source2" });

        string fileName = "SettingSave.json";
        string jsonString = JsonSerializer.Serialize(setting);
        File.WriteAllText(fileName, jsonString);

        Console.WriteLine(File.ReadAllText(fileName));
    }

    /// <summary>
    /// чтение json файла с классом settingCopy
    /// </summary>
    /// <returns>класс settingCopy</returns>
    public static SettingCopy ReadJsonFile()
    {
        string fileName = "SettingSave.json";
        var jsonString = File.ReadAllText(fileName);

        Console.WriteLine(jsonString);
        SettingCopy setting = JsonSerializer.Deserialize<SettingCopy>(jsonString);
        return setting;
    }
}