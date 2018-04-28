using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    /// <summary>
    /// The GenerationInterval class represents the frame intervals for each kind of enemies to generate.
    /// </summary>
    public class GenerationInterval
    {
        /// <summary>
        /// Gets the frame interval for small enemy to generate.
        /// </summary>
        public int SmallEnemyGenerationInterval { get; }

        /// <summary>
        /// Gets the frame interval for middle enemy to generate.
        /// </summary>
        public int MiddleEnemyGenerationInterval { get; }

        /// <summary>
        /// Gets the frame interval for large enemy to generate.
        /// </summary>
        public int LargeEnemyGenerationInterval { get; }

        /// <summary>
        /// Initializes a new instance of the GenerationInterval class with given intervals.
        /// </summary>
        /// <param name="sInterval">The frame interval for small enemy to generate.</param>
        /// <param name="mInterval">The frame interval for middle enemy to generate.</param>
        /// <param name="lInterval">The frame interval for large enemy to generate.</param>
        public GenerationInterval(int sInterval, int mInterval, int lInterval)
        {
            this.SmallEnemyGenerationInterval = sInterval;
            this.MiddleEnemyGenerationInterval = mInterval;
            this.LargeEnemyGenerationInterval = lInterval;
        }
    }
}
