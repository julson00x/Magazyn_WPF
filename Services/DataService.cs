using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Magazyn_WPF.Models;

namespace Magazyn_WPF.Services
{
    public class DataService
    {
        private readonly string _filePath;

        public DataService()
        {
            // Plik zapisywany obok .exe
            _filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "magazyn_data.json");
        }

        public List<Produkt> LoadProducts()
        {
            if (!File.Exists(_filePath))
                return new List<Produkt>();

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Produkt>>(json)
                       ?? new List<Produkt>();
            }
            catch
            {
                return new List<Produkt>();
            }
        }

        public void SaveProducts(IEnumerable<Produkt> produkty)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(produkty, options);
            File.WriteAllText(_filePath, json);
        }
    }
}