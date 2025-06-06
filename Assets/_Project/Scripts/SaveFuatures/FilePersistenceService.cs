using System.IO;
using UnityEngine;
using _Project.Scripts.Interfaces;

namespace _Project.Scripts.SaveFuatures
{
    public class FilePersistenceService : IPersistenceService
    {
        private readonly string _rootPath;

        public FilePersistenceService()
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