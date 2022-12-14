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
        
        [Test]
        public void Query_Cross_Month()
        {
            GivenMonthAndAmount(new List<Budget>
            {
                new Budget
                {
                    YearMonth = new DateTime(2022, 10, 1),
                    Amount = 3100
                },
                new Budget
                {
                    YearMonth = new DateTime(2022, 11, 1),
                    Amount = 300
                },
            });

            var result = _service.Query(new DateTime(2022, 10, 31), new DateTime(2022, 11, 2));

            result.Should().Be(100 + 20);
        }
        
        [Test]
        public void Query_Cross_Three_Month()
        {
            GivenMonthAndAmount(new List<Budget>
            {
                new Budget
                {
                    YearMonth = new DateTime(2022, 11, 1),
                    Amount = 3000
                },
                new Budget
                {
                    YearMonth = new DateTime(2022, 12, 1),
                    Amount = 310
                },
                new Budget
                {
                    YearMonth = new DateTime(2023, 1, 1),
                    Amount = 31
                },
            });

            var result = _service.Query(new DateTime(2022, 11, 29), new DateTime(2023, 1, 3));

            result.Should().Be(200 + 310 + 3);
        }

        private ConfiguredCall GivenMonthAndAmount(List<Budget> budgets)
        {
            return _budgetRepo.GetAll().Returns(budgets);
        }
    }
}