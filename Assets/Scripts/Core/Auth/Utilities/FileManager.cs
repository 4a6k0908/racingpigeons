using System;
using System.IO;
using UnityEngine;

public static class FileManager
{
   static string _Path="";

   public static void setPath(string path)
   {
        _Path = path;
   }

   public static bool WriteToFile(string a_FileName, string a_FileContents)
   {
      var fullPath = Path.Combine(_Path, a_FileName);

      try
      {
         File.WriteAllText(fullPath, a_FileContents);
         return true;
      }
      catch (Exception e)
      {
         Debug.LogError($"Failed to write to {fullPath} with exception {e}");
         return false;
      }
   }

   public static bool LoadFromFile(string a_FileName, out string result)
   {
      var fullPath = Path.Combine(_Path, a_FileName);

      try
      {
         result = File.ReadAllText(fullPath);
         return true;
      }
      catch (Exception e)
      {
         Debug.LogWarning($"Failed to read from {fullPath}, file should be created after a successful login, can probably ignore. Exception {e}.");
         result = "";
         return false;
      }
   }
}