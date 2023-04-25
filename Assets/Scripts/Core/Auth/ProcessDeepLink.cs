using System;
using UnityEngine;

public class ProcessDeepLink : MonoBehaviour
{
   public static ProcessDeepLink Instance { get; private set; }

   private string deeplinkURL;
   // private LoginProc _uiInputManager;

   private void onDeepLinkActivated(string url)
   {
      // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
      deeplinkURL = url;

      // _uiInputManager = FindObjectOfType<LoginProc>();
      // _uiInputManager.ProcessDeepLink(deeplinkURL);
   }

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;

         Application.deepLinkActivated += onDeepLinkActivated;

         if (!String.IsNullOrEmpty(Application.absoluteURL))
         {
            onDeepLinkActivated(Application.absoluteURL);
         }
         else
         {
            deeplinkURL = "NONE";
         }

         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public string GetDeepLink()
   {
      return deeplinkURL;
   }
}