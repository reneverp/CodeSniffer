using System;
using System.Collections.Generic;

namespace CodeSniffer.BBN.Discretization
{
    public class DiscretizedData
    {
        public IList<Bin> Bins { get; private set; }

        public DiscretizedData(IList<Bin> bins)
        {
            Bins = bins;
        }

        public Bin Discretize(double value)
        {
            foreach(var bin in Bins)
            {
                if (value >= bin.LowerBoundary && value <= bin.UpperBoundary)
                {
                    return bin;
                }
            }

            return null;
        }
    }
}
