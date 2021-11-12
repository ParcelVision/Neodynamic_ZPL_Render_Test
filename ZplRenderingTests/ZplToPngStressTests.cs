using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Neodynamic.SDK.ZPLPrinter;
using Xunit;

namespace ZplRenderingTests
{
    using System.Collections.Generic;
    using System.Text;

    public class ZplToPngStressTests
    {
        private const string licenseOwner = "ParcelVision Limited-Team License";
        private const string licenseKey = "EUVB5CEFN3DUWB48Y79PZVNHFCZEYUV8EDJ793HFKKAKYUDW9KXQ";

        [Theory]
        [InlineData("./hermesLabel.txt")]
        [InlineData("./landmarkLabel.txt")]
        public async Task SequentialGenerationsWithSameInput_ShouldProduceSameResult(string base64LabelFile)
        {
            var testId = Guid.NewGuid().ToString("N");

            var labelZplBase64 = await File.ReadAllTextAsync(base64LabelFile);
            var labelZpl = Convert.FromBase64String(labelZplBase64);

            byte[] labelSingleRun = Array.Empty<byte>();

            using var zplPrinter = new ZPLPrinter(licenseOwner, licenseKey)
            {
                RenderOutputRotation = RenderOutputRotation.Rot90Clockwise,AntiAlias = true,ForceLabelHeight = true, ForceLabelWidth = true
            };

            var pages = zplPrinter.ProcessCommands(labelZpl);

            labelSingleRun = pages.First();
            await File.WriteAllBytesAsync($"label-{testId}-sequential-0.png", labelSingleRun);

            var labels = new List<byte[]>();

            for (var n = 1; n <= 10; n++)
            {
                var pagesN = zplPrinter.ProcessCommands(labelZpl);
                var labelN = pagesN.First();

                await File.WriteAllBytesAsync($"label-{testId}-sequential-{n}.png", labelN);

                labels.Add(labelN);
            }

            foreach (var label in labels)
                label.SequenceEqual(labelSingleRun).Should().BeTrue();
        }

        [Theory(Skip = "Disabled until sequential test pass")]
        [InlineData("./hermesLabel.txt")]
        [InlineData("./landmarkLabel.txt")]
        public void ParallelGenerationsWithSameInput_ShouldProduceSameResult(string base64LabelFile)
        {
            var testId = Guid.NewGuid().ToString("N");

            var labelZplBase64 = File.ReadAllText(base64LabelFile);
            var labelZpl = Convert.FromBase64String(labelZplBase64);

            using var zplPrinter = new ZPLPrinter(licenseOwner, licenseKey)
            {
                RenderOutputRotation = RenderOutputRotation.Rot90Clockwise
            };
            var pages = zplPrinter.ProcessCommands(labelZpl);

            var labelSingleRun = pages.First();
            File.WriteAllBytes($"label-{testId}-parallel-0.png", labelSingleRun);

            Parallel.ForEach(Enumerable.Range(1, 10), n =>
            {
                using var zplPrinterN = new ZPLPrinter(licenseOwner, licenseKey)
                {
                    RenderOutputRotation = RenderOutputRotation.Rot90Clockwise
                };

                var pagesN = zplPrinterN.ProcessCommands(labelZpl);
                var labelN = pagesN.First();

                File.WriteAllBytes($"label-{testId}-parallel-{n}.png", labelN);

                labelN.SequenceEqual(labelSingleRun).Should().BeTrue();
            });
        }
    }
}

