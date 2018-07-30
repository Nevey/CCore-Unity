using System;
using CCore.Utilities;
using UnityEngine;

namespace CCore
{
	/// <summary>
	/// CCore's Monobehaviour extension
	/// </summary>
	public class MonoBehaviour : UnityEngine.MonoBehaviour
	{
		[SerializeField] private bool loggingEnabled = true;

		[SerializeField] private Color loggingColor = Color.white;

		private void DoLog(
            Action<string> logCall,
            string str,
            params object[] args)
        {
            // TODO: Check if "!Debug.isDebugBuild" is needed
			if (!loggingEnabled || !Debug.isDebugBuild)
			{
				return;
			}

#if !UNITY_PRO_LICENSE
			loggingColor = Color.black;
#endif

			string colorHex = Converter.ColorToHex(loggingColor);
			
			logCall(string.Format(
					"<color="+ colorHex +"FF>[{0}] {1} </color>",
					this.GetType().Name,
					string.Format(str, args))
			);
        }

        public void Log(string str, params object[] args)
        {
            DoLog(Debug.Log, str, args);
        }

        public void LogWarning(string str, params object[] args)
        {
            DoLog(Debug.LogWarning, str, args);
        }

        public void LogError(string str, params object[] args)
        {
            DoLog(Debug.LogError, str, args);
        }
	}
}
