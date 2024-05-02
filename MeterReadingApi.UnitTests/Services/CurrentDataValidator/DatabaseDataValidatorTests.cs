using FakeItEasy;
using FluentAssertions;
using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Services.MeterUploadService.CurrentDataValidator;
using MeterReadingsDatabase.Models;
using MeterReadingsDatabase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingApi.UnitTests.Services.CurrentDataValidator
{
    internal class DatabaseDataValidatorTests
    {
        [Test]
        public void Test_WhenTheAccountDoesNotExist_ReturnsError()
        {
            IMeterReadingRepositiory fakeMeterReadingRepositiory = A.Fake<IMeterReadingRepositiory>();
            var sut = new DatabaseDataValidator(fakeMeterReadingRepositiory);
            List<MeterReadingCsvDataLine> testCsvData = new List<MeterReadingCsvDataLine>()
            {
                new MeterReadingCsvDataLine()
                {
                    AccountId = 2
                }
            };
            A.CallTo(() => fakeMeterReadingRepositiory.GetAccounts(A<IEnumerable<int>>.That.Matches(c => c.FirstOrDefault() == 1 && c.Count() == 1))).Returns(new List<Account>()
            {
                new Account()
                {
                    AccountId = 1,
                }
            });

            var result = sut.ValidateAgianstExitingData(testCsvData);


            result.csvData.Count().Should().Be(0);
            result.errors.Count().Should().Be(1);
            result.errors.First().Message.Should().Be("There is no account with this ID");

        }

        [Test]
        public void Test_AccountAlreadyHasReadingAtSameTIme_ReturnsError()
        {
            IMeterReadingRepositiory fakeMeterReadingRepositiory = A.Fake<IMeterReadingRepositiory>();
            var sut = new DatabaseDataValidator(fakeMeterReadingRepositiory);
            List<MeterReadingCsvDataLine> testCsvData = new List<MeterReadingCsvDataLine>()
            {
                new MeterReadingCsvDataLine()
                {
                    AccountId = 1,
                    MeterReadingDateTime = new DateTime(2020,01,05),
                }
            };
            A.CallTo(() => fakeMeterReadingRepositiory.GetAccounts(A<IEnumerable<int>>.That.Matches(c => c.FirstOrDefault() == 1 && c.Count() == 1))).Returns(new List<Account>()
            {
                new Account()
                {
                    AccountId = 1,
                    MeterReadings = new []
                    {
                        new MeterReading()
                        {
                            MeterReadingDateTime = new DateTime(2020,01,05),
                            AccountId = 1,
                        }
                    }
                }
            });

            var result = sut.ValidateAgianstExitingData(testCsvData);


            result.csvData.Count().Should().Be(0);
            result.errors.Count().Should().Be(1);
            result.errors.First().Message.Should().Be("A reading already exists for this user at the same time");

        }

        [Test]
        public void Test_AccountAlreadyHasNewerReading_ReturnsError()
        {
            IMeterReadingRepositiory fakeMeterReadingRepositiory = A.Fake<IMeterReadingRepositiory>();
            var sut = new DatabaseDataValidator(fakeMeterReadingRepositiory);
            List<MeterReadingCsvDataLine> testCsvData = new List<MeterReadingCsvDataLine>()
            {
                new MeterReadingCsvDataLine()
                {
                    AccountId = 1,
                    MeterReadingDateTime = new DateTime(2020,01,05),
                }
            };
            A.CallTo(() => fakeMeterReadingRepositiory.GetAccounts(A<IEnumerable<int>>.That.Matches(c => c.FirstOrDefault() == 1 && c.Count() == 1))).Returns(new List<Account>()
            {
                new Account()
                {
                    AccountId = 1,
                    MeterReadings = new []
                    {
                        new MeterReading()
                        {
                            MeterReadingDateTime = new DateTime(2022,01,05),
                            AccountId = 1,
                        }
                    }
                }
            });

            var result = sut.ValidateAgianstExitingData(testCsvData);


            result.csvData.Count().Should().Be(0);
            result.errors.Count().Should().Be(1);
            result.errors.First().Message.Should().Be("A reading already exists after this reading for the user");

        }
    }
}
