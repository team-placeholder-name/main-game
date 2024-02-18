using UnityEngine;
using System.Collections.Generic;
using System;
using Scrtwpns.Mixbox;
using Unity.VisualScripting;
using System.Linq;

namespace Prismatic
{
    [System.Serializable]
    public class HueMix : IEquatable<HueMix>
    {
        // private Dictionary<Color, int> colors = new Dictionary<Color, int>();
        [SerializeField]
        private List<Color> colors = new List<Color>();
        [SerializeField]
        private List<int> weights = new List<int>();

        /// <summary>
        /// The approximated mix of all input colors.
        /// </summary>
        public Color Color { get; private set; } = Color.white;

        public HueMix(List<Color> colors, List<int> weights)
        {
            this.colors = colors;
            this.weights = weights;
            
            Mix();
        }

        // Equality overrides
        public override bool Equals(object other) => this.Equals(other as HueMix);
        public bool Equals(HueMix other)
        {
            if(other == null) return false;
            if(object.ReferenceEquals(this, other)) return true;
            if(this.GetType() != other.GetType()) return false;
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


        

        /// <summary>
        /// Adds a color to the color mix. If that color already exists in the mix, its weight in the mix is increased.
        /// </summary>
        /// <param name="color">The color added to the mix.</param>
        public void AddColor(Color color)
        {
            if(colors.Contains(color))
            {
                weights[colors.IndexOf(color)]++;
            }
            else
            {
                colors.Add(color);
                weights.Add(1);
            }

            Mix();
        }

        /// <summary>
        /// Adds a all colors from a different HueMix to the color mix. If that color already exists in the mix, its weight in the mix is increased.
        /// </summary>
        /// <param name="hueMix">The hueMix whos colors are added to the mix.</param>
        public void AddColor(HueMix hueMix)
        {
            for (int i = 0; i < hueMix.colors.Count; i++ )
            {
                for(int j = 0;j<hueMix.weights[i];j++)
                    AddColor(hueMix.colors[i]);
            }

        }
        /// <summary>
        /// Removes a color from the mix. If that color already exists in the mix, its weight in the mix will be reduced.
        /// </summary>
        /// <param name="color">The color removed from the mix.</param>
        public void RemoveColor(Color color)
        {
            if(!colors.Contains(color)) return;

            if (weights[colors.IndexOf(color)] == 1)
            {
                colors.Remove(color);
                weights.RemoveAt(colors.IndexOf(color));
            }
            else
            {
                weights[colors.IndexOf(color)]--;
            }

            Mix();
        }

        /// <summary>
        /// Mixes all Color objects in the colors dictionary into a single color using Mixbox.
        /// </summary>
        private void Mix()
        {
            if(weights.Sum() == 0)
            {
                Color = Color.white;
                return;
            }

            MixboxLatent latentMix = new MixboxLatent();
            for(int i = 0; i < colors.Count; i++)
            {
                float concentration = weights[i] / (float)weights.Sum();
                latentMix += Mixbox.RGBToLatent(colors[i]) * concentration;
            }

            Color = Mixbox.LatentToRGB(latentMix);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(colors, Color);
        }
    }
}