using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiquityPackageChallenge.Domain.Models
{
    public class Package
    {
        public double WeightLimit { get; private set; }
        public IEnumerable<Item> Items { get; private set; }

        public Package(double weightLimit, IEnumerable<Item> items)
        {
            WeightLimit = weightLimit;
            Items = items;
        }
    }
}
