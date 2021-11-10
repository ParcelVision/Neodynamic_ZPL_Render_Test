using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Neodynamic.SDK.ZPLPrinter;
using Xunit;

namespace ZplRenderingTests
{
    public class ZplToPngStressTests
    {
        private const string licenseOwner = "CHANGE ME";
        private const string licenseKey = "CHANGE ME";

        [Theory]
        [InlineData("./hermesLabel.txt")]
        [InlineData("./landmarkLabel.txt")]
        public async Task SequentialGenerationsWithSameInput_ShouldProduceSameResult(string base64LabelFile)
        {
            var testId = Guid.NewGuid().ToString("N");

            var labelZplBase64 = await File.ReadAllTextAsync(base64LabelFile);
            var labelZpl = Convert.FromBase64String(labelZplBase64);

            byte[] labelSingleRun = Array.Empty<byte>();
            using (var zplPrinter = new ZPLPrinter(licenseOwner, licenseKey)
            {
                RenderOutputRotation = RenderOutputRotation.Rot90Clockwise
            })
            {
                var pages = zplPrinter.ProcessCommands(labelZpl);

                labelSingleRun = pages.First();
                await File.WriteAllBytesAsync($"label-{testId}-sequential-0.png", labelSingleRun);
            }

            for (var n = 1; n <= 10; n++)
            {
                using var zplPrinterN = new ZPLPrinter(licenseOwner, licenseKey)
                {
                    RenderOutputRotation = RenderOutputRotation.Rot90Clockwise
                };
                var pagesN = zplPrinterN.ProcessCommands(labelZpl);
                var labelN = pagesN.First();

                await File.WriteAllBytesAsync($"label-{testId}-sequential-{n}.png", labelN);

                labelN.SequenceEqual(labelSingleRun).Should().BeTrue();
            }
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
            File.WriteAllBytes($"label-{testId}-sequential-0.png", labelSingleRun);

            Parallel.ForEach(Enumerable.Range(1, 10), n =>
            {
                using var zplPrinterN = new ZPLPrinter(licenseOwner, licenseKey)
                {
                    RenderOutputRotation = RenderOutputRotation.Rot90Clockwise
                };

                var pagesN = zplPrinterN.ProcessCommands(labelZpl);
                var labelN = pagesN.First();

                File.WriteAllBytes($"label-{testId}-sequential-{n}.png", labelN);

                labelN.SequenceEqual(labelSingleRun).Should().BeTrue();
            });
        }
    }
}

