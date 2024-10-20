using System.Globalization;
using BankSystem.Data.DbContext;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using CsvHelper;
using OfficeOpenXml;

namespace ExportTool;

public class ExportService
{
    private string _pathToDirectory;
    private string _csvFileName;

    public ExportService(string pathToDirectory, string csvFileName)
    {
        _pathToDirectory = pathToDirectory;
        _csvFileName = csvFileName;
    }

    public void ExportClientToCsvFile(ICollection<Client> clients)
    {
        var dirInfo = new DirectoryInfo(_pathToDirectory);
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
        var csvFilePath = Path.Combine(_pathToDirectory, _csvFileName);
        using (var fileStream = new FileStream(csvFilePath, FileMode.OpenOrCreate))
        {
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                using (var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    writer.WriteField(nameof(Client.Id));
                    writer.WriteField(nameof(Client.FirstName));
                    writer.WriteField(nameof(Client.LastName));
                    writer.WriteField(nameof(Client.DateOfBirth));
                    writer.WriteField(nameof(Client.Adress));
                    writer.WriteField(nameof(Client.Passport));
                    writer.WriteField(nameof(Client.PhoneNumber));
                    writer.WriteField(nameof(Client.CreatedOn));
                    writer.WriteField(nameof(Client.Age));
                    
                    writer.NextRecord();

                    foreach (var client in clients)
                    {
                        writer.WriteField(client.Id);
                        writer.WriteField(client.FirstName);
                        writer.WriteField(client.LastName);
                        writer.WriteField(client.DateOfBirth.ToString("yyyy-MM-dd"));
                        writer.WriteField(client.Adress);
                        writer.WriteField(client.Passport);
                        writer.WriteField(client.PhoneNumber);
                        writer.WriteField(client.CreatedOn.ToString("yyyy-MM-dd"));
                        writer.WriteField(client.Age);
                        writer.NextRecord();
                    }
                    
                    writer.Flush();
                }
            }
        }
    }

    public ICollection<Client> ImportClientFromFile(string filePath)
    {
        var clients = new List<Client>();

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
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var client = new Client
                        {
                            Id = csv.GetField<Guid>("Id"),
                            FirstName = csv.GetField<string>("FirstName"),
                            LastName = csv.GetField<string>("LastName"),
                            DateOfBirth = DateTime.Parse(csv.GetField("DateOfBirth")).ToUniversalTime(),
                            Adress = csv.GetField("Adress"),
                            Passport = csv.GetField("Passport"),
                            PhoneNumber = csv.GetField("PhoneNumber"),
                            CreatedOn = DateTime.Parse(csv.GetField("CreatedOn")).ToUniversalTime(),
                            Age = int.Parse(csv.GetField("Age"))
                        };

                        clients.Add(client);
                    }
                }
            }
        }

        return clients;

    }
}