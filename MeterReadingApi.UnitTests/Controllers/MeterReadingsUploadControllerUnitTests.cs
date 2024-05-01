using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MeterReadingsApi.Controllers;
using MeterReadingsApi.Models.Reqest.FileRequestModels;
using MeterReadingsApi.Models.Response;
using MeterReadingsApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingApi.UnitTests.Controllers
{
    internal class MeterReadingsUploadControllerUnitTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test_WhenInvalidRequestSent_400IsReturnedWithAppopiateValidationMessage()
        {
            var fakeVlaidator = A.Fake<AbstractValidator<FileRequestModel>>();
            var fakeUplaodService = A.Fake<IMeterReadingUploadService>();
            var Sut = new MeterReadingUploadsController(fakeVlaidator, fakeUplaodService);
            FileRequestModel testFileRequest = new FileRequestModel(){};

            const string testErrorMessage = "Test Error Message";
            const string testPropertyName = "Test Property Name";
            const string testValue = "Test Value";
            A.CallTo(() => fakeVlaidator.Validate(A<ValidationContext<FileRequestModel>>.Ignored)).Returns(new ValidationResult()
            {
                Errors =
                {
                    new ValidationFailure()
                    {
                        ErrorMessage = testErrorMessage,
                        PropertyName = testPropertyName,
                        AttemptedValue = testValue
                    }
                }
            });


           var result = Sut.UploadMeterReading(testFileRequest);


           var resultObject = result as BadRequestObjectResult;
           resultObject.StatusCode.Should().Be(400);
           var errorObject = resultObject.Value as ErrorResponseModel;
           errorObject.Errors.Count().Should().Be(1);
           errorObject.Errors.First().Message.Should().Be(testErrorMessage);
           errorObject.Errors.First().Source.Should().Be(testPropertyName);
           errorObject.Errors.First().Data.Should().Be(testValue);

        }


        [Test]
        public void Test_WhenValidCsvSent_ValidFileIsPassedOntoServiceAndResultMetadataReturned()
        {
            var fakeVlaidator = A.Fake<AbstractValidator<FileRequestModel>>();
            var fakeUplaodService = A.Fake<IMeterReadingUploadService>();
            var Sut = new MeterReadingUploadsController(fakeVlaidator,fakeUplaodService);
            IFormFile fakeFormFile = A.Fake<IFormFile>();
            FileRequestModel testFileRequest = new FileRequestModel()

            {
                FileDetails = fakeFormFile
            };

            A.CallTo(() => fakeVlaidator.Validate(A<ValidationContext<FileRequestModel>>.Ignored)).Returns(new ValidationResult());
            MeterReadingUploadResponse testResponse = new MeterReadingUploadResponse();
            A.CallTo(() => fakeUplaodService.ProcessMeterReadingCsv(fakeFormFile)).Returns(testResponse);


            var result = Sut.UploadMeterReading(testFileRequest);
            var resultObject = result as OkObjectResult;
            var responseObject = resultObject.Value as MeterReadingUploadResponse;
            responseObject.Should().BeSameAs(testResponse);


        }
    }
}
