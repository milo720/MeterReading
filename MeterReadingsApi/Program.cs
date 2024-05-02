using FluentValidation;
using MeterReadingsApi.Models.Reqest.FileRequestModels;
using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Repository;
using MeterReadingsApi.Services;
using MeterReadingsApi.Services.MeterUploadService.CsvReading;
using MeterReadingsApi.Services.MeterUploadService.CurrentDataValidator;
using MeterReadingsApi.Services.MeterUploadService.DataValidator;
using MeterReadingsDatabase;
using MeterReadingsDatabase.Repository;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MeterReadingDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("MeterReadingDB")));

builder.Services.AddTransient<IMeterReadingCsvDataValidator, MeterReadingCsvContentValidator>();
builder.Services.AddTransient<IMeterReadingCsvReader, MeterReadingCsvReader>();
builder.Services.AddTransient< AbstractValidator < MeterReadingCsvDataLine > , MeterReadingCsvDataLineValidator >();
builder.Services.AddTransient< AbstractValidator <FileRequestModel> , FileRequestModelValidator >();
builder.Services.AddTransient<IMeterReadingUploadService, MeterReadingUploadService>();
builder.Services.AddTransient<IMeterReadingRepositiory ,MeterReadingRepository>();
builder.Services.AddTransient<IDatabaseDataValidator ,DatabaseDataValidator>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
