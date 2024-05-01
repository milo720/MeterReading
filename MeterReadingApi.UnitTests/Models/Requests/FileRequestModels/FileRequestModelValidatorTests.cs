using FakeItEasy;
using FluentAssertions;
using MeterReadingsApi.Models.Reqest.FileRequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingApi.UnitTests.Models.Requests.FileRequestModels
{
    internal class FileRequestModelValidatorTests
    {
        [Test]
        public void Test_WhenCalledValidateWithInncorectContentType_ReturnsVlaidationError() {

            var sut = new FileRequestModelValidator();
            IFormFile fakeFormFile = A.Fake<IFormFile>();
            A.CallTo(() => fakeFormFile.ContentType).Returns(MediaTypeNames.Text.JavaScript);
            A.CallTo(() => fakeFormFile.Length).Returns(1000);
            var result = sut.Validate(new FileRequestModel()
            {
                FileDetails = fakeFormFile
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("Please ensure the file uploaded in a CSV");
        }

        [Test]
        public void Test_WhenCalledValidateWithCorrectFile_ReturnsNoVlaidationError()
        {

            var sut = new FileRequestModelValidator();
            IFormFile fakeFormFile = A.Fake<IFormFile>();
            A.CallTo(() => fakeFormFile.ContentType).Returns(MediaTypeNames.Text.Csv);
            A.CallTo(() => fakeFormFile.Length).Returns(1000);
            var result = sut.Validate(new FileRequestModel()
            {
                FileDetails = fakeFormFile
            });

            result.Errors.Should().HaveCount(0);
        }
    }
}
