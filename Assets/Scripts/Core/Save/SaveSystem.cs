using System.IO;
using UnityEngine;

namespace Core.Save
{
    public class SaveSystem
    {
        public void Save<T>(T saveData, string saveName) where T : class
        {
            string savePath = Path.Combine(Application.persistentDataPath, saveName);

            File.WriteAllText(savePath, JsonUtility.ToJson(saveData));
        }

        public T Load<T>(string fileName) where T : class
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);

            var content = "";

            if (!File.Exists(filePath))
                return null;
            
            content = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(content);

        }

        public void Delete(string fileName)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);

            if (IsExist(fileName))
            {
                File.Delete(filePath);
            }
        }

        public bool IsExist(string fileName)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);

            return File.Exists(filePath);
        }
    }
}