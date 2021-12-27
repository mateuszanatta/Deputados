using System.Collections.Generic;

namespace Congressperson.Models.DTO
{
    public class CongresspersonStatistics
    {
        public IEnumerable<ExpensensByYear> ExpensesByYear { get; set; }
        public IEnumerable<ExpensensByMonth> ExpensesByMonth { get; set; }
    }

    public class ExpensensByYear
    {
        public int Year { get; set; }
        public decimal Value { get; set; }
    }

    public class ExpensensByMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Value { get; set; }
    }
}
