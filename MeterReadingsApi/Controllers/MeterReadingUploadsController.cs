using FluentValidation;
using MeterReadingsApi.Models.Reqest.FileRequestModels;
using MeterReadingsApi.Models.Response;
using MeterReadingsApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeterReadingsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingUploadsController : ControllerBase
    {
        private readonly IMeterReadingUploadService meterReadingUploadServicecs;

        public MeterReadingUploadsController(AbstractValidator<FileRequestModel> validator, IMeterReadingUploadService meterReadingUploadServicecs)
        {
            Validator = validator;
            this.meterReadingUploadServicecs = meterReadingUploadServicecs;
        }

        public AbstractValidator<FileRequestModel> Validator { get; }

        [HttpPost]
        [ProducesResponseType(typeof(MeterReadingUploadResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        public IActionResult UploadMeterReading([FromForm] FileRequestModel fileRquestModel)
        {
           var modelValidation =  Validator.Validate(fileRquestModel);
            if (!modelValidation.IsValid)
            {
                return new BadRequestObjectResult(new ErrorResponseModel()
                {
                    Errors = modelValidation.Errors.Select(c => new Error()
                    {
                        Source = c.PropertyName,
                        Message = c.ErrorMessage,
                        Data = c.AttemptedValue
                    })
                });
            }
            var result = meterReadingUploadServicecs.ProcessMeterReadingCsv(fileRquestModel.FileDetails);
            return Ok(result);
        }

    }
}
