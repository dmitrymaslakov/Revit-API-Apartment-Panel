using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentPanel.Utility.Interfaces
{
    public interface IDeepClone<T>
    {
        /// <summary>
        /// Carries out a deep copy of the current instance.
        /// </summary>
        /// <returns>A deep copy of an instance of T</returns>
        T Clone();
    }
}
