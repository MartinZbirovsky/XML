using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ukol.Models;

namespace Ukol
{
    public class CarViewModel : BaseCar
    {
        public double PriceNoDPH { get; set; }
        public double PriceWithDph { get; set; }
    }
}
