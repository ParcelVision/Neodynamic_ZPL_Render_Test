using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Neodynamic.SDK.Printing;
using Newtonsoft.Json;
using Xunit;

namespace ZplRenderingTests
{
    public class ZplGenerationStressTests
    {
        [Fact]
        public async Task ZplGenerations_WithValidData_ShouldProduceResult()
        {

            var labelTemplate = await File.ReadAllTextAsync("hermesLabelTemplate.txt");
            var labelData = await File.ReadAllTextAsync("hermesLabelData.json");
            var data = JsonConvert.DeserializeObject<HermesLabelDataSource>(labelData);

            var label = new ThermalLabel(UnitType.Inch, 6, 4) {GapLength = 0.2};
            label.LoadJsonTemplate(labelTemplate);
            label.DataSource = new List<object> {data};

            using var job = new PrintJob {ThermalLabel = label, ProgrammingLanguage = ProgrammingLanguage.ZPL};
            var zplBytes = job.GetBinaryNativePrinterCommands();

            zplBytes.Should().NotBeEmpty();
        }

        public class HermesLabelDataSource
        {
            public string ClientName { get; set; }
            public string CarrierLogo { get; set; }
            public string Hermes2DBarcode { get; set; }
            public string PlayLogo { get; set; }
            public string Service1Text { get; set; }
            public string Service2Text { get; set; }
            public string Service3Text { get; set; }
            public string Service4Text { get; set; }
            public string ParcelshopName { get; set; }
            public string SortLevel1CodeAndDescription { get; set; }
            public string DlyMethodDescription { get; set; }
            public string SortLevel2TypeAndDescription { get; set; }
            public string SortLevel3TypeAndDescription { get; set; }
            public string SortLevel4TypeAndDescription { get; set; }
            public string SortLevel5TypeAndDescription { get; set; }
            public string DestinationAddressLine1 { get; set; }
            public string DestinationAddressLine2 { get; set; }
            public string DestinationAddressLine3 { get; set; }
            public string DestinationAddressLine4 { get; set; }
            public string DestinationAddressLine5 { get; set; }
            public string DestinationAddressLine6 { get; set; }
            public string DestinationAddressLine7 { get; set; }
            public string DestinationAddressLine8 { get; set; }
            public string CurrentDate { get; set; }
            public string Weight { get; set; }
            public string CustomerRef1 { get; set; }
            public string CustomerRef2 { get; set; }
            public string HermesBarcode { get; set; }
            public string HermesBarcodeDisplay { get; set; }
        }
    }
}
