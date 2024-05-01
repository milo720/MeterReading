using MeterReadingsApi.Models.Response;

namespace MeterReadingsApi.Services
{
    public interface IMeterReadingUploadService
    {
        MeterReadingUploadResponse ProcessMeterReadingCsv(IFormFile file);
    }
}