using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiquityPackageChallenge.Domain.Models
{
    internal class Package
    {
        public double WeightLimit { get; private set; }
        public IEnumerable<Item> Ite { get; private set; }

        public Package(double weightLimit, IEnumerable<Item> ite)
        {
            WeightLimit = weightLimit;
            Ite = ite;
        }
    }
}
