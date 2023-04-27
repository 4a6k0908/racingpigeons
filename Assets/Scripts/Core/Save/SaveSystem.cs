using System.IO;
using UnityEngine;

namespace Core.Save
{
    public class SaveSystem
    {
        // 儲存到 persistentDataPath
        public void Save<T>(T saveData, string saveName) where T : class
        {
            string savePath = Path.Combine(Application.persistentDataPath, saveName);

            File.WriteAllText(savePath, JsonUtility.ToJson(saveData));
        }

        /// <summary>
        /// 讀取 local 的文件，如果不存在會回傳 null
        /// </summary>
        public T Load<T>(string fileName) where T : class
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);

            var content = "";

            if (!File.Exists(filePath))
                return null;
            
            content = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(content);

        }

        // 刪除文件
        public void Delete(string fileName)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);

            if (IsExist(fileName))
            {
                File.Delete(filePath);
            }
        }

        // 檢查文件是否存在
        public bool IsExist(string fileName)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);

            return File.Exists(filePath);
        }
    }
}