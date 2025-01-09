using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace VS.Runtime.Core.Views
{
    public class BubbleView : View
    {
        [field: SerializeField] public SpriteRenderer Sprite { get; private set; }
        
        public void OnRelease() => 
            gameObject.SetActive(false);

        public void SetSprite(Sprite sprite) => Sprite.sprite = sprite;
        public void SetColor(EBubbleColor colorType) => Sprite.color = BubbleExtensions.GetColor(colorType);
    }

    public class BubbleExtensions
    {
        private static int? _colorRange;
        private static Random _random;

        private static Random Random => _random ??= new Random();
        private static int ColorRange => _colorRange ??= Enum.GetValues(typeof(EBubbleColor)).Length;  
        private static readonly Dictionary<EBubbleColor, Color> BubbleColorDictionary = new()
        {
            { EBubbleColor.None, Color.clear },
            { EBubbleColor.Yellow, Color.yellow },
            { EBubbleColor.Red, Color.red },
            { EBubbleColor.Blue, Color.blue },
            { EBubbleColor.Green, Color.green },
            { EBubbleColor.Purple, new Color(0.5f, 0f, 0.5f) }, 
            { EBubbleColor.Pink, new Color(1f, 0.41f, 0.71f) }
        };
        
        public static Color GetColor(EBubbleColor colorType) => 
            BubbleColorDictionary[colorType];

        public static EBubbleColor GetRandomColor() =>
            (EBubbleColor)Random.Next(ColorRange);
    }

    public enum EBubbleColor
    {
        None = 0,
        Yellow = 1,
        Red = 2,
        Blue = 3,
        Green = 4,
        Purple = 5,
        Pink = 6,
    }
}