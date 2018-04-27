using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    public class GenerationInterval
    {
        public int SmallEnemyGenerationInterval { get; }
        public int MiddleEnemyGenerationInterval { get; }
        public int LargeEnemyGenerationInterval { get; }

        public GenerationInterval(int sInterval, int mInterval, int lInterval)
        {
            this.SmallEnemyGenerationInterval = sInterval;
            this.MiddleEnemyGenerationInterval = mInterval;
            this.LargeEnemyGenerationInterval = lInterval;
        }
    }
}
