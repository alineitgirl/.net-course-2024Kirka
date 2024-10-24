using System.Globalization;
using CsvHelper;
using Newtonsoft.Json;

namespace ExportTool;

public class ExportService
{
    public void ExportDataToCsvFile<T>(ICollection<T> data, string filePath)
    {
        using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                using (var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    writer.WriteRecords(data);
                    writer.Flush();
                }
            }
        }
    }

    public ICollection<T> ImportClientFromCsvFile<T>(string filePath)
    {

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        using (var fileStream = new FileStream(filePath, FileMode.Open))
        {
            using (var reader = new StreamReader(fileStream))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var data = csv.GetRecords<T>().ToList();
                    return data;
                }
            }
        }
    }
    
    public void ExportDataToJsonFile<T>(T data, string filePath)
    {
        using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            using (var textWriter = new StreamWriter(fileStream))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(textWriter, data);
            }
        }
    }

    public ICollection<T> ImportDataFromJsonFile<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }
        using (var fileStream = new FileStream(filePath, FileMode.Open))
        {
            using (var textReader = new StreamReader(fileStream))
            {
                var info = textReader.ReadToEnd();
                return JsonConvert.DeserializeObject<ICollection<T>>(info);
            }
        }
        
    }

}