using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Calculator.Dto
{
    public class CalcDto
    {
        [DisplayName("Expression")]
        [Required(ErrorMessage = "Enter expression")]
        [MaxLength(200, ErrorMessage = "Expression size must be no more than {1} symbols")]
        [RegularExpression(@"^[\d+,.*\/^() -]+$", ErrorMessage = "Wrong math expression")]
        public string Expression { get; set; }

        [DisplayName("Result")]
        public double? Result { get; set; }

        public string ResultString => Result?.ToString("F2");
    }
}