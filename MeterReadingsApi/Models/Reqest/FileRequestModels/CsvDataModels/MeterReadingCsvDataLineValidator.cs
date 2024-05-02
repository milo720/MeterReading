using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;

namespace MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels
{
    public class MeterReadingCsvDataLineValidator : AbstractValidator<MeterReadingCsvDataLine>
    {

            public MeterReadingCsvDataLineValidator()
            {
            RuleFor(x => x.MeterReadValue).Matches(@"^\d\d\d\d\d$").WithMessage("Meter readings must be in the form NNNNN");              
            }
        
    }
}
