# Testing Neodynamic ZPL to PNG conversion

Replace license information in the placeholders:

```csharp
private const string licenseOwner = "CHANGE ME";
private const string licenseKey = "CHANGE ME";
```

Run the tests in Docker container:

```bash
docker build -t zpl2png-test .
```

Run the tests locally:

```bash
dotnet test ZplRenderingTests
```

When running tests locally, the resulting label PNGs
are generated in the build output directory.
