using Calculator.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Db
{
    public class CalcContext : DbContext
    {
        public CalcContext(DbContextOptions<CalcContext> options)
            : base(options)
        {
        }

        public DbSet<Calc> Calcs { get; set; }
    }
}