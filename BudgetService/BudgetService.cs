using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetService
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime startDate, DateTime endDate)
        {
            var days = (endDate - startDate).Days +1;
            var budget= _budgetRepo.GetAll().First();

            var daysInMonth = DateTime.DaysInMonth(budget.YearMonth.Year, budget.YearMonth.Month);

            return budget.Amount / daysInMonth * days;

        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public DateTime YearMonth { get; set; }
        public decimal Amount { get; set; }
    }
}