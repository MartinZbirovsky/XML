using System;
using Ukol.Models;

namespace Ukol
{
    public class Car :BaseCar
    {
        public DateTime DateOfSale { get; private set; }
        public double Price { get; private set; }
        public Car(string ModelName, DateTime DateOfSale, double Price, double DPH)
        {
            this.ModelName = ModelName;
            this.DateOfSale = DateOfSale;
            this.Price = Price;
            this.DPH = DPH;
        }
    }
}
