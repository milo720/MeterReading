using DotNet.Testcontainers.Builders;
using FluentAssertions;
using MeterReadingsApi.Models.Response;
using MeterReadingsDatabase;
using MeterReadingsDatabase.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System.Data.Common;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using Testcontainers.MsSql;

namespace MeterReadingApiIntergrationTests
{
    internal class IntergrationTests : IntergrationTestBase
    {
       



        
        [Test]
        public async Task Test_UploadTestData_ShouldGiveTheCorrectNumbers()
        {
            using var apiClient = appFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/MeterReadingUploads");
            MultipartFormDataContent multipartContent = new MultipartFormDataContent();

            StreamContent content = new StreamContent(File.OpenRead(@"Meter_Reading 2.csv"));
            content.Headers.Add("Content-Type", MediaTypeNames.Text.Csv);
            multipartContent.Add(content, "FileDetails", "Test-REadings.csv");

            request.Content = multipartContent;


            var response = await apiClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseObject = JsonSerializer.Deserialize<MeterReadingUploadResponse>(responseContent, options);
            responseObject.SuccessfullCount.Should().Be(24); 
            responseObject.UnccessfullCount.Should().Be(11); 

            context.MeterReadings.Should().HaveCount(24);
        }
    }
}