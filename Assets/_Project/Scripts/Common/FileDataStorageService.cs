using System.IO;
using UnityEngine;
using _Project.Scripts.Common.Interfaces;

namespace _Project.Scripts.Common
{
    public class FileDataStorageService : IDataStorageService
    {
        private readonly string _rootPath;

        public FileDataStorageService()
        {
            _rootPath = Application.persistentDataPath;
        }

        public void SaveToFile(string fileName, string json)
        {
            string path = Path.Combine(_rootPath, fileName);
            
            File.WriteAllText(path, json);
        }

        public string LoadFromFile(string fileName)
        {
            string path = Path.Combine(_rootPath, fileName);
            
            return File.Exists(path) ? File.ReadAllText(path) : "";
        }

        public bool FileExists(string fileName)
        {
            string path = Path.Combine(_rootPath, fileName);
            
            return File.Exists(path);
        }
    }
}