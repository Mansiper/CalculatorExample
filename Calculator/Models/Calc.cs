using System;

namespace Calculator.Models
{
    public class Calc
    {
        public int Id { get; set; }

        public string Expression { get; set; }

        public double Result { get; set; }

        public DateTime CalcDate { get; set; }

        public long CalculationTime { get; set; }

        public string ClientIp { get; set; }

        public string ResultString() => Result.ToString("F2");
    }
}