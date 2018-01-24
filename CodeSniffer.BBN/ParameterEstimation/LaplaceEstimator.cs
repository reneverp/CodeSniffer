using CodeSniffer.BBN.Discretization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.BBN.ParameterEstimation
{
    class LaplaceEstimator
    {
        public LaplaceEstimator(IList<Bin> bins, int decisionRow )
        {
            int countTrue = 0;
            int countFalse = 0;

            foreach(var bin in bins)
            {
                foreach(var row in bin.Rows)
                {
                    if(row.Field<bool>(decisionRow))
                    {
                        countTrue++;
                    }
                    else
                    {
                        countFalse++;
                    }
                }
            }


        }
    }
}
