using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiquityPackageChallenge.Domain.Models
{
    public class Item
    {
        public int Index { get; private set; }
        public double Weight { get; private set; }
        public Cost Cost { get; private set; }

        public Item(int index, double weight, Cost cost)
        {
            Index = index;
            Weight = weight;
            Cost = cost;
        }
    }
}
