using FakeItEasy;
using FluentAssertions;
using MeterReadingsApi.Models.Reqest.FileRequestModels;
using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingApi.UnitTests.Models.Requests.FileRequestModels.CsvDataModels
{
    internal class MeterReadingCsvDataLineValidatorTests
    {

        [Test]
        public void Test_WhenCalledValidateWithInncorectMeterReading_ReturnsVlaidationError()
        {

            var sut = new MeterReadingCsvDataLineValidator();

            var result = sut.Validate(new MeterReadingCsvDataLine() { MeterReadValue = "0000x"});

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("Meter readings must be in the form NNNNN");
        }


        [Test]
        public void Test_WhenCalledValidateWithCorectMeterReading_ReturnsNoVlaidationError()
        {

            var sut = new MeterReadingCsvDataLineValidator();

            var result = sut.Validate(new MeterReadingCsvDataLine() { MeterReadValue = "00000" });

            result.Errors.Should().HaveCount(0);

        }
    }
}
