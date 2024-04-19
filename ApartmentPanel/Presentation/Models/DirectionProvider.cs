using ApartmentPanel.Presentation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.Models
{
    internal class DirectionProvider
    {
        public DirectionProvider(Key arrow) => Arrow = arrow;

        internal Key Arrow { get; set; }

        internal Direction GetDirection()
        {
            switch (Arrow)
            {
                case Key.Left:
                    return Direction.West;
                case Key.Up:
                    return Direction.North;
                case Key.Right:
                    return Direction.East;
                case Key.Down:
                    return Direction.South;
                default: return Direction.None;
            }
        }
    }
}
