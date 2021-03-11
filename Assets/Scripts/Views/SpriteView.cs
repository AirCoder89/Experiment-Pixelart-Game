using System;
using AirCoder.Core;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Views
{
    public class SpriteView : GameView
    {
        public Color Color
        {
            get => SpriteRenderer.color;
            set => SpriteRenderer.color = value;
        }
        public string SortingLayer { get; }
        private SpriteRenderer _sR;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_sR == null) _sR = gameObject.GetComponent<SpriteRenderer>();
                return _sR;
            }
        }
        
        public SpriteView(Application inApp, string inName, string inLayerName) : base(inApp, inName)
        {
            // --------------------------------------------------------------------------------
            // Add SpriteRenderer & Set Sorting Layer
            // --------------------------------------------------------------------------------
            _sR = gameObject.AddComponent<SpriteRenderer>();
            SortingLayer = inLayerName;
            SetSortingLayer(SortingLayer);
        }

        public void UpdateSpriteRendererInstance()
        {
            _sR = gameObject.GetComponent<SpriteRenderer>();
        }

        public void SetSortingLayer(string inLayerName) => SpriteRenderer.sortingLayerName = inLayerName;
        
        public void SetSprite(Sprite inSprite)
        {
            SpriteRenderer.sprite = inSprite;
        }

        public Vector2 GetSize()
        {
            return new Vector2(SpriteRenderer.sprite.bounds.size.x, SpriteRenderer.sprite.bounds.size.y);
        }
        public void SetOrderInLayer(int inOrder)
        {
            SpriteRenderer.sortingOrder = inOrder;
        }
        public void SetOrderInLayerUnder(SpriteView inOtherSpView)
        {
            SetOrderInLayer(inOtherSpView.SpriteRenderer.sortingOrder - 1);
        }
        public void SetOrderInLayerAbove(SpriteView inOtherSpView)
        {
            SetOrderInLayer(inOtherSpView.SpriteRenderer.sortingOrder + 1);
        }
    }
}