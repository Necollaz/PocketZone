namespace _Project.Scripts.Interfaces
{
    public interface IPersistenceService
    {
        public void SaveToFile(string fileName, string json);
        
        public string LoadFromFile(string fileName);
        
        public bool FileExists(string fileName);
    }
}