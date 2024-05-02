using CsvHelper.Configuration;

namespace MeterReadingsApi.Models.Reqest.FileRequestModels.CsvDataModels
{
    public class MeterReadingCsvDataLineMap : ClassMap<MeterReadingCsvDataLine>
    {
        public MeterReadingCsvDataLineMap()
        {
            Map(m => m.MeterReadingDateTime).Name("MeterReadingDateTime").TypeConverterOption.Format("dd/MM/yyyy HH:mm");
            Map(m => m.AccountId).Name("AccountId");
            Map(m => m.MeterReadValue).Name("MeterReadValue");
        }

    }
}
