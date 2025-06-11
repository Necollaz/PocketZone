namespace _Project.Scripts.Common.Interfaces
{
    public interface IDataStorageService
    {
        public void SaveToFile(string fileName, string json);
        
        public string LoadFromFile(string fileName);
        
        public bool FileExists(string fileName);
    }
}