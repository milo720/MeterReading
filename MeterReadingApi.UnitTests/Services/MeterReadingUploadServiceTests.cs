using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using MeterReadingsApi.Controllers;
using MeterReadingsApi.Models.Reqest.FileRequestModels;
using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Models.Response;
using MeterReadingsApi.Services;
using MeterReadingsApi.Services.MeterUploadService.CsvReading;
using MeterReadingsApi.Services.MeterUploadService.CurrentDataValidator;
using MeterReadingsApi.Services.MeterUploadService.DataValidator;
using MeterReadingsDatabase.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingApi.UnitTests.Services
{
    internal class MeterReadingUploadServiceTests
    {

        [Test]
        public void Test_ParsesValidatesAndStores_ReturningTheCorrectNumberOfErrors()
        {

            IMeterReadingCsvDataValidator fakeMeterReadingValidator = A.Fake<IMeterReadingCsvDataValidator>();
            IMeterReadingCsvReader fakeMeterReadingCsvReader = A.Fake<IMeterReadingCsvReader>();
            IMeterReadingRepositiory fakeMeterReadingRepositiory = A.Fake<IMeterReadingRepositiory>();
            IDatabaseDataValidator fakeDatabaseDataValidator = A.Fake<IDatabaseDataValidator>();
            var sut = new MeterReadingUploadService(fakeMeterReadingValidator, fakeMeterReadingCsvReader, fakeDatabaseDataValidator, fakeMeterReadingRepositiory);
            IFormFile fakeFormFile = A.Fake<IFormFile>();

            List<Error> testParseErrors = new List<Error>()
            {
                new Error()
                {
                    Message = "Test1"
                }
            };
            List<MeterReadingCsvDataLine> testParsedData = new List<MeterReadingCsvDataLine>();
            A.CallTo(() => fakeMeterReadingCsvReader.ReadCsv(fakeFormFile)).Returns((testParsedData, testParseErrors, 30));

            List<Error> testValdiateErrors = new List<Error>()
            {
                new Error()
                {
                    Message = "Test2"
                }
            };
            List<MeterReadingCsvDataLine> testValidatedData = new List<MeterReadingCsvDataLine>();
            A.CallTo(() => fakeMeterReadingValidator.ValidateCsvData(testParsedData)).Returns((testValidatedData, testValdiateErrors));

            List<Error> validationAgainstExisitngDataErrors = new List<Error>()
            {
                new Error()
                {
                    Message = "Test3"
                }
            };
            List<MeterReadingCsvDataLine> testValidatedAgainstDB = new List<MeterReadingCsvDataLine>()
            {
                new MeterReadingCsvDataLine(),
                new MeterReadingCsvDataLine(),
            };
            A.CallTo(() => fakeDatabaseDataValidator.ValidateAgianstExitingData(testValidatedData)).Returns((testValidatedAgainstDB, validationAgainstExisitngDataErrors));
            var totalAdded = 10;




            var result = sut.ProcessMeterReadingCsv(fakeFormFile);
            A.CallTo(() => fakeMeterReadingRepositiory.UploadRedings(testValidatedAgainstDB)).MustHaveHappened();
            result.Errors.Count().Should().Be(3);
            result.Errors.First().Message.Should().Be("Test1");
            result.Errors.Skip(1).First().Message.Should().Be("Test2");
            result.Errors.Skip(2).First().Message.Should().Be("Test3");
            result.SuccessfullCount.Should().Be(2);
            result.UnccessfullCount.Should().Be(28);


        }
    }
}
