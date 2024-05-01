using FluentValidation;
using System.Net.Mime;

namespace MeterReadingsApi.Models.Reqest.FileRequestModels
{
    public class FileRequestModelValidator : AbstractValidator<FileRequestModel>
    {
        public FileRequestModelValidator()
        {
            RuleFor(x => x.FileDetails).NotNull().WithMessage("Please provide a file for processing");
            RuleFor(x => x.FileDetails.Length).GreaterThan(0).WithMessage("Please provide file content of more than 0 bytes");
            RuleFor(x => x.FileDetails.ContentType).Equal(MediaTypeNames.Text.Csv).WithMessage("Please ensure the file uploaded in a CSV");
        }
    }
}
