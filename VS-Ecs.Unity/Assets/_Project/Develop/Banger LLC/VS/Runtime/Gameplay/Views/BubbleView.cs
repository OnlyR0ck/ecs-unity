using System;
using System.Collections.Generic;
using UnityEngine;
using VS.Runtime.Services;

namespace VS.Runtime.Core.Views
{
    public class BubbleView : EcsView
    {
        [field: SerializeField] public SpriteRenderer Sprite { get; private set; }
        [field: SerializeField] public EBubbleColor Color { get; private set; }

        public void OnRelease() => 
            gameObject.SetActive(false);

        public void SetSprite(Sprite sprite) => Sprite.sprite = sprite;
        public void SetColor(EBubbleColor colorType)
        {
            Color = colorType;
            Sprite.color = BubbleExtensions.GetColor(colorType);
        }
    }

    public static class BubbleExtensions
    {
        private static readonly int ColorRange = Enum.GetValues(typeof(EBubbleColor)).Length;
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
            (EBubbleColor)RandomService.Current.Next(1, ColorRange);

        public static EBubbleColor GetRandomColor(HashSet<EBubbleColor> fieldColors)
        {
            if (fieldColors == null || fieldColors.Count == 0)
                return GetRandomColor();
            var index = RandomService.Current.Next(0, fieldColors.Count);
            var i = 0;
            foreach (var color in fieldColors)
            {
                if (i == index) return color;
                i++;
            }
            return GetRandomColor();
        }
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