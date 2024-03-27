using System;
using System.IO;
using System.Text.Json;

namespace ScanCode.WPF;

public class ObjectFileStorage
{
    private readonly string _rootFolder;

    public ObjectFileStorage()
    {
        //  this.rootFolder = Directory.GetCurrentDirectory(); //rootFolder;

        this._rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "CustomFolder");

        if (!Directory.Exists(_rootFolder))
        {
            Directory.CreateDirectory(_rootFolder);
        }
    }

    public void Save<T>(T obj, string fileName)
    {
        string json = JsonSerializer.Serialize(obj);
        string folderPath = Path.Combine(_rootFolder, DateTime.Now.ToString("yyyy-MM-dd"));

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        File.WriteAllText(Path.Combine(folderPath, fileName), json);
    }

    public T? Load<T>(string fileName)
    {
        var date = DateTime.Now.ToString("yyyy-MM-dd");
        string folderPath = Path.Combine(_rootFolder, date);
        string filePath = Path.Combine(folderPath, fileName);

        if (!File.Exists(filePath))
        {
            return default;
            //throw new FileNotFoundException("File not found", filePath);
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }
}