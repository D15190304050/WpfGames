using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    public interface ICollide
    {
        bool Collide(double x, double y);

        void MoveDown(double speed);
    }
}
