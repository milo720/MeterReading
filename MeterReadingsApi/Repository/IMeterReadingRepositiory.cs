using MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels;
using MeterReadingsApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingsDatabase.Repository
{
    public interface IMeterReadingRepositiory
    {
        public (IEnumerable<Error> errors, int addedRecords) ValidateAgainstExitingDataAndStoreMeterReading(IEnumerable<MeterReadingCsvDataLine> meterReadings);

    }
}
