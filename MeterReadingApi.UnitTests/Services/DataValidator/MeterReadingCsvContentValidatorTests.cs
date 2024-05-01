﻿using FakeItEasy;
using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Services.MeterUploadService.CsvReading;
using MeterReadingsApi.Services.MeterUploadService.DataValidator;
using MeterReadingsApi.Services;
using MeterReadingsDatabase.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using FluentAssertions;


namespace MeterReadingApi.UnitTests.Services.DataValidator
{
    internal class MeterReadingCsvContentValidatorTests
    {

        [Test]
        public void Test_VlaidtesAgiantValidator_ReturningTheCorrectErrorsAndValidRecords()
        {


            AbstractValidator<MeterReadingCsvDataLine> fakeValidator = A.Fake<AbstractValidator<MeterReadingCsvDataLine>>();
            var sut = new MeterReadingCsvContentValidator(fakeValidator);

            MeterReadingCsvDataLine testMeterReadingCsvDataLine1 = new MeterReadingCsvDataLine();
            MeterReadingCsvDataLine testMeterReadingCsvDataLine2 = new MeterReadingCsvDataLine();
            MeterReadingCsvDataLine testMeterReadingCsvDataLine3 = new MeterReadingCsvDataLine();
            MeterReadingCsvDataLine testMeterReadingCsvDataLine4 = new MeterReadingCsvDataLine();

            const string testError1Message = "Error 1";
            const string testError2Message = "Error 2";
            ValidationResult testValidationResult1 = new ValidationResult() { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorMessage = testError1Message } } };
            ValidationResult testValidationResult2 = new ValidationResult() { };
            ValidationResult testValidationResult3 = new ValidationResult() { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorMessage = testError2Message } } };
            ValidationResult testValidationResult4 = new ValidationResult();
            A.CallTo(() => fakeValidator.Validate(A<ValidationContext<MeterReadingCsvDataLine>>.That.Matches(c => c.InstanceToValidate == testMeterReadingCsvDataLine1))).Returns(testValidationResult1);
            A.CallTo(() => fakeValidator.Validate(A<ValidationContext<MeterReadingCsvDataLine>>.That.Matches(c => c.InstanceToValidate == testMeterReadingCsvDataLine2))).Returns(testValidationResult2);
            A.CallTo(() => fakeValidator.Validate(A<ValidationContext<MeterReadingCsvDataLine>>.That.Matches(c => c.InstanceToValidate == testMeterReadingCsvDataLine3))).Returns(testValidationResult3);
            A.CallTo(() => fakeValidator.Validate(A<ValidationContext<MeterReadingCsvDataLine>>.That.Matches(c => c.InstanceToValidate == testMeterReadingCsvDataLine4))).Returns(testValidationResult4);


            var result = sut.ValidateCsvData(new List<MeterReadingCsvDataLine> { testMeterReadingCsvDataLine1, testMeterReadingCsvDataLine2, testMeterReadingCsvDataLine3, testMeterReadingCsvDataLine4 });

            result.csvData.Count().Should().Be(2);
            result.errors.Count().Should().Be(2);
            result.csvData.First().Should().BeSameAs(testMeterReadingCsvDataLine2);
            result.csvData.Last().Should().BeSameAs(testMeterReadingCsvDataLine4);
            result.errors.First().Message.Should().BeSameAs(testError1Message);
            result.errors.Last().Message.Should().BeSameAs(testError2Message);

        }


        
    }
}
