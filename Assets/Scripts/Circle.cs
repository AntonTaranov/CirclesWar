using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CirclesWar.Data;

namespace CirclesWar
{
    public class Circle : MonoBehaviour
    {
        private const float CIRCLE_Z = -1;
        CircleData data;
        SpriteRenderer spriteRenderer;
        float radius = -1;

        public void SetData(CircleData data)
        {
            this.data = data;
            if (spriteRenderer == null)
                InitializeSprite(data.GetColor());
            this.radius = data.GetRadius();
            UpdateRadius();
            UpdatePosition();
        }

        private void InitializeSprite(Color color)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            var texture = Resources.Load<Sprite>("white_circle");
            if (texture != null)
            {
                spriteRenderer.sprite = texture;
                spriteRenderer.color = color;
            }
        }

        private void UpdateRadius()
        {
            if (data.GetRadius() != radius)
            {
                radius = data.GetRadius();
                transform.localScale = new Vector3(radius, radius, 1);
            }
        }

        private void UpdatePosition()
        {
            var position = data.GetPosition();
            transform.localPosition = new Vector3(position.x, position.y, CIRCLE_Z);
        }

        // Update is called once per frame
        void Update()
        {
            if (data.IsAlive())
            {
                UpdateRadius();
                UpdatePosition();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
