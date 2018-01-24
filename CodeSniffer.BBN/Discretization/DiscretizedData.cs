using System;
using System.Collections.Generic;

namespace CodeSniffer.BBN.Discretization
{
    public class DiscretizedData
    {
        private IList<Bin> _bins;

        public DiscretizedData(IList<Bin> bins)
        {
            _bins = bins;
        }

        public Bin Discretize(double value)
        {
            foreach(var bin in _bins)
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
