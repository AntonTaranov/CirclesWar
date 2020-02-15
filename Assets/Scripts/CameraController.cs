using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CirclesWar
{
    public class CameraController : MonoBehaviour
    {
        GameObject gameFieldGameObject;
        Camera mainCamera;

        int width;
        int height;

        bool dirty = false;

        public void SetTargetGameObject(GameObject value)
        {
            gameFieldGameObject = value;
            mainCamera = Camera.main;
            dirty = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (mainCamera != null)
            {
                if (dirty || mainCamera.pixelHeight != height || mainCamera.pixelWidth != width)
                {
                    width = mainCamera.pixelWidth;
                    height = mainCamera.pixelHeight;

                    var fieldSize = gameFieldGameObject.transform.localScale;
                    var fieldMaxSize = fieldSize.x > fieldSize.y ? fieldSize.x : fieldSize.y;
                    if (height > width)
                    {
                        mainCamera.orthographicSize = fieldMaxSize * height / width * 0.5f;
                    }
                    else
                    {
                        mainCamera.orthographicSize = fieldMaxSize * 0.5f;
                    }
                    dirty = false;
                }
            }
        }
    }
}