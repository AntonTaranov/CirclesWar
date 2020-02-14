using System;
using UnityEngine;

using CirclesWar.Data;

namespace CirclesWar
{
    public class ConfigLoader : MonoBehaviour
    {
		private void Awake()
		{
            var asset = Resources.Load<TextAsset>("GameConfig");

            var configMap = JsonUtility.FromJson<ConfigMap>(asset.text);

            if (configMap != null)
            {
                var gameConfig = configMap.GameConfig;
                Debug.Log("config load success");           
            }
            else
            {
                Debug.LogError("config load error");
            }
		}

        [Serializable]
        class ConfigMap
        {
            public GameConfig GameConfig;
        }
	}
}
