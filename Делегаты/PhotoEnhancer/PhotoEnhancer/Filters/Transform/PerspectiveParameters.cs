using System;

namespace PhotoEnhancer
{
    public class PerspectiveParameters : IParameters
    {
        public double N { get; set; }

        public ParameterInfo[] GetDescription()
        {
            return new[]
            {
                new ParameterInfo() 
                {
                    Name = "Сужение в %",
                    MinValue = 0,
                    MaxValue = 100,
                    DefailtValue = 100,
                    Increment = 5
                }
            };
        }

        public void SetValues(double[] values)
        {
            N = values[0];
        }
    }
}
