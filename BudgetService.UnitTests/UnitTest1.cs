using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;

namespace BudgetService.Tests
{
    public class BudgetServiceTests
    {
        private BudgetService _service;
        private IBudgetRepo _repo;
        private IBudgetRepo _budgetRepo;

        [SetUp]
        public void Setup()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
            _service = new BudgetService(_budgetRepo);
        }

        [Test]
        public void Query_One_Month()
        {
            GivenMonthAndAmount(new List<Budget>
            {
                new Budget
                {
                    YearMonth = new DateTime(2022, 10, 1),
                    Amount = 3100
                }
            });

            var result = _service.Query(new DateTime(2022, 10, 1), new DateTime(2022, 10, 31));

            result.Should().Be(3100);
        } 
        [Test]
        public void Query_One_Day()
        {
            GivenMonthAndAmount(new List<Budget>
            {
                new Budget
                {
                    YearMonth = new DateTime(2022, 10, 1),
                    Amount = 3100
                }
            });

            var result = _service.Query(new DateTime(2022, 10, 1), new DateTime(2022, 10, 1));

            result.Should().Be(100);
        }

        private ConfiguredCall GivenMonthAndAmount(List<Budget> budgets)
        {
            return _budgetRepo.GetAll().Returns(budgets);
        }
    }
}