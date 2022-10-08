using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Primitives;

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
            var totalAmount = 0m;

            var dateRange = new DateRange(startDate, endDate);

            while (dateRange.HasNextMonth())
            {
                var budget= _budgetRepo.GetAll().FirstOrDefault(date =>
                {
                    return date.YearMonth.Year == dateRange.CurrentDate.Year &&
                           date.YearMonth.Month == dateRange.CurrentDate.Month;
                }) ?? new Budget();
                
                var daysInMonth = DateTime.DaysInMonth(budget.YearMonth.Year, budget.YearMonth.Month);

                totalAmount += budget.Amount / daysInMonth * dateRange.GetDays();
                
                dateRange.NextMonth();
            }

            return totalAmount;
        }
    }

    public class DateRange
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private DateTime _currentDate;

        public DateTime CurrentDate
        {
            private set => _currentDate = value;
            get => _currentDate;
        }
        
        public DateRange(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            CurrentDate = new DateTime(_startDate.Year, _startDate.Month, 1);
        }

        public bool HasNextMonth()
        {
            return CurrentDate <= _endDate;
        }

        public void NextMonth()
        {
            CurrentDate = CurrentDate.AddMonths(1);
        }

        public int GetDays()
        {
            var daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);

            if (CurrentDate.Month == _endDate.Month && CurrentDate.Year == _endDate.Year)
            {
                return _endDate.Day;
            }
            else if (CurrentDate.Month == _startDate.Month && CurrentDate.Year == _startDate.Year)
            {
                return daysInMonth - _startDate.Day + 1;
            }
            else
            {
                return daysInMonth;
            }
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