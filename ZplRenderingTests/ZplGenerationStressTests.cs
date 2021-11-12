using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Neodynamic.SDK.Printing;
using Neodynamic.SDK.ZPLPrinter;
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
            public HermesLabelDataSource(
                string clientName,
                string hermes2DBarcode,
                string carrierLogo,
                string playLogo,
                string service1Text,
                string service2Text,
                string service3Text,
                string service4Text,
                string parcelshopName,
                string sortLevel1CodeAndDescription,
                string dlyMethodDescription,
                string sortLevel2TypeAndDescription,
                string sortLevel3TypeAndDescription,
                string sortLevel4TypeAndDescription,
                string sortLevel5TypeAndDescription,
                string destinationAddressLine1,
                string destinationAddressLine2,
                string destinationAddressLine3,
                string destinationAddressLine4,
                string destinationAddressLine5,
                string destinationAddressLine6,
                string destinationAddressLine7,
                string destinationAddressLine8,
                string currentDate,
                string weight,
                string customerRef1,
                string customerRef2,
                string hermesBarcode,
                string hermesBarcodeDisplay
            )
            {
                ClientName = clientName;
                Hermes2DBarcode = hermes2DBarcode;
                CarrierLogo = carrierLogo;
                PlayLogo = playLogo;
                Service1Text = service1Text;
                Service2Text = service2Text;
                Service3Text = service3Text;
                Service4Text = service4Text;
                ParcelshopName = parcelshopName;
                SortLevel1CodeAndDescription = sortLevel1CodeAndDescription;
                DlyMethodDescription = dlyMethodDescription;
                SortLevel2TypeAndDescription = sortLevel2TypeAndDescription;
                SortLevel3TypeAndDescription = sortLevel3TypeAndDescription;
                SortLevel4TypeAndDescription = sortLevel4TypeAndDescription;
                SortLevel5TypeAndDescription = sortLevel5TypeAndDescription;
                DestinationAddressLine1 = destinationAddressLine1;
                DestinationAddressLine2 = destinationAddressLine2;
                DestinationAddressLine3 = destinationAddressLine3;
                DestinationAddressLine4 = destinationAddressLine4;
                DestinationAddressLine5 = destinationAddressLine5;
                DestinationAddressLine6 = destinationAddressLine6;
                DestinationAddressLine7 = destinationAddressLine7;
                DestinationAddressLine8 = destinationAddressLine8;
                CurrentDate = currentDate;
                Weight = weight;
                CustomerRef1 = customerRef1;
                CustomerRef2 = customerRef2;
                HermesBarcode = hermesBarcode;
                HermesBarcodeDisplay = hermesBarcodeDisplay;
            }

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
