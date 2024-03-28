using UnityEngine;
using System.Collections.Generic;
using System;
using Scrtwpns.Mixbox;


namespace Prismatic
{
    [System.Serializable]
    public class HueMix : IEquatable<HueMix>
    {
        public List<ColorWeight> Colors { get => colorWeights; }

        // private Dictionary<Color, int> colors = new Dictionary<Color, int>();
        [SerializeField]
        private List<ColorWeight> colorWeights = new List<ColorWeight>();


        /// <summary>
        /// The approximated mix of all input colors.
        /// </summary>
        public Color Color { get => Mix(); }

        public HueMix() { }
        public HueMix(List<ColorWeight> colors)
        {
            this.colorWeights = colors;

            Mix();
        }

        public HueMix Clone()
        {
            List<ColorWeight> clonedColors = new List<ColorWeight>();
            foreach(ColorWeight cw in colorWeights)
            {
                clonedColors.Add(cw.Clone());
            }
            return new HueMix(clonedColors);
        }

        // Equality overrides
        public override bool Equals(object other) => this.Equals(other as HueMix);
        public bool Equals(HueMix other)
        {
            if(other == null) return false;
            if(object.ReferenceEquals(this, other)) return true;
            if(this.GetType() != other.GetType()) return false;
            Debug.Log(Color + " == " + other.Color + "="+ (Color == other.Color));
            return Color == other.Color;
        }
        public static bool operator ==(HueMix left, HueMix right)
        {
            if (left is null)
            {
                if (right is null) return true;
                return false;
            }
            return left.Equals(right);
        }
        public static bool operator !=(HueMix left, HueMix right) => !(left == right);


        public List<Color> GetColors()
        {
            List<Color> colors = new List<Color>();
            foreach (ColorWeight colorWeight in colorWeights)
            {
                colors.Add(colorWeight.color);
            }
            return colors;
        }
        public int GetColorCount()
        {
            int weightSum = 0;
            foreach (ColorWeight colorWeight in colorWeights)
            {
                weightSum += colorWeight.weight;
            }
            return weightSum;
        }

        /// <summary>
        /// Adds a color to the color mix. If that color already exists in the mix, its weight in the mix is increased.
        /// </summary>
        /// <param name="color">The color added to the mix.</param>
        public void AddColor(Color color)
        {
            ColorWeight containedColor = null;
            foreach (ColorWeight colorWeight in colorWeights)
            {
                if (colorWeight.color == color)
                {
                    containedColor = colorWeight;
                    break;
                }
            }
            if (containedColor != null)
            {
                containedColor.weight++;
            }
            else
            {
                colorWeights.Add(new ColorWeight(color, 1));
            }
        }

        /// <summary>
        /// Adds a all colors from a different HueMix to the color mix. If that color already exists in the mix, its weight in the mix is increased.
        /// </summary>
        /// <param name="hueMix">The hueMix whos colors are added to the mix.</param>
        public void AddColor(HueMix hueMix)
        {
            for (int i = 0; i < hueMix.colorWeights.Count; i++ )
            {
                for(int j = 0;j<hueMix.colorWeights[i].weight;j++)
                    AddColor(hueMix.colorWeights[i].color);
            }

        }
        /// <summary>
        /// Removes a color from the mix. If that color already exists in the mix, its weight in the mix will be reduced.
        /// </summary>
        /// <param name="color">The color removed from the mix.</param>
        public void RemoveColor(Color color)
        {
            ColorWeight containedColor = null;
            foreach (ColorWeight colorWeight in colorWeights)
            {
                if (colorWeight.color == color)
                {
                    containedColor = colorWeight;
                    break;
                }
            }
            if (containedColor == null) return;

            if (containedColor.weight == 1)
            {
                colorWeights.Remove(containedColor);
            }
            else
            {
                containedColor.weight--;
            }
        }

        /// <summary>
        /// Mixes all Color objects in the colors dictionary into a single color using Mixbox.
        /// </summary>
        private Color Mix()
        {
            int weightSum=0;
            foreach (ColorWeight colorWeight in colorWeights)
            {
                weightSum+= colorWeight.weight;   
            }
            if(weightSum==0)
            {
                return Color.white;
            }

            MixboxLatent latentMix = new MixboxLatent();
            foreach (ColorWeight colorWeight in colorWeights)
            {
                float concentration = colorWeight.weight / (float)weightSum;
                latentMix += Mixbox.RGBToLatent(colorWeight.color) * concentration;
            }

            return  Mixbox.LatentToRGB(latentMix);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(colorWeights, Color);
        }
    }
    [System.Serializable]
    public class ColorWeight
    {
        public Color color = Color.white;
        public int weight = 1;
        public ColorWeight(Color color, int weight)
        {
            this.color = color;
            this.weight = weight;
        }

        public ColorWeight Clone()
        {
            return new ColorWeight(color, weight);
        }
    

    }
}