using UnityEngine;
using System.Collections.Generic;
using System;
using Scrtwpns.Mixbox;

namespace Prismatic
{
    [System.Serializable]
    public class HueMix : IEquatable<HueMix>
    {
        private Dictionary<Color, int> colors = new Dictionary<Color, int>();
        private int numMixedColors = 0;

        /// <summary>
        /// The approximated mix of all input colors.
        /// </summary>
        public Color Color { get; private set; }

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
            if(colors.ContainsKey(color))
            {
                colors[color]++;
            }
            else
            {
                colors.Add(color, 1);
            }

            numMixedColors++;

            Mix();
        }

        /// <summary>
        /// Adds a all colors from a different HueMix to the color mix. If that color already exists in the mix, its weight in the mix is increased.
        /// </summary>
        /// <param name="hueMix">The hueMix whos colors are added to the mix.</param>
        public void AddColor(HueMix hueMix)
        {
            foreach(KeyValuePair<Color,int> color in hueMix.colors)
            {
                //AddColor
            }

        }
        /// <summary>
        /// Removes a color from the mix. If that color already exists in the mix, its weight in the mix will be reduced.
        /// </summary>
        /// <param name="color">The color removed from the mix.</param>
        public void RemoveColor(Color color)
        {
            if(!colors.ContainsKey(color)) return;

            if (colors[color] == 1)
            {
                colors.Remove(color);
            }
            else
            {
                colors[color]--;
            }

            numMixedColors--;

            Mix();
        }

        /// <summary>
        /// Mixes all Color objects in the colors dictionary into a single color using Mixbox.
        /// </summary>
        private void Mix()
        {
            if(numMixedColors == 0)
            {
                Color = Color.white;
                return;
            }

            MixboxLatent latentMix = new MixboxLatent();
            foreach(KeyValuePair<Color, int> pair in colors)
            {
                float concentration = pair.Value / (float)numMixedColors;
                latentMix += Mixbox.RGBToLatent(pair.Key) * concentration;
            }

            Color = Mixbox.LatentToRGB(latentMix);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(colors, numMixedColors, Color);
        }
    }
}