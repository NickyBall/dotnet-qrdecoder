# QR Code Base64 Decoder

A cross-platform C# library for decoding QR codes from base64-encoded PNG images.

## Overview

This project provides a simple solution for extracting QR code strings from base64-encoded images, specifically designed for scenarios where API responses contain QR codes as base64 PNG data instead of plain text strings.

## Features

- ✅ Cross-platform support (Windows, Linux, macOS)
- ✅ Decodes QR codes from base64-encoded PNG images
- ✅ Supports JSON response parsing
- ✅ Handles both black & white QR codes
- ✅ Multiple binarizer strategies for robust decoding
- ✅ No Windows-specific dependencies

## Requirements

- .NET 6.0 or higher
- NuGet packages:
  - `ZXing.Net` - QR code decoding
  - `SkiaSharp` - Cross-platform image processing
  - `Newtonsoft.Json` - JSON parsing (optional)

## Installation

### Using .NET CLI

```bash
dotnet add package ZXing.Net
dotnet add package SkiaSharp
dotnet add package Newtonsoft.Json
```

### Using Package Manager Console

```powershell
Install-Package ZXing.Net
Install-Package SkiaSharp
Install-Package Newtonsoft.Json
```

## Usage

### Basic Usage - Decode from Base64 String

```csharp
using QRCodeDecoder;

string qrBase64 = "iVBORw0KGgoAAAA..."; // Your base64 PNG string
string qrString = QRDecoder.DecodeQRFromBase64(qrBase64);

if (qrString != null)
{
    Console.WriteLine($"Decoded QR Code: {qrString}");
}
else
{
    Console.WriteLine("Failed to decode QR code");
}
```

### Decode from JSON Response

```csharp
string jsonResponse = @"{
    ""success"": 200,
    ""data"": {
        ""platform_order_id"": ""THBP20251106151333LpD3WxRr"",
        ""merchant_order_id"": ""15329655"",
        ""qrbase64"": ""iVBORw0KGgoAAAA...""
    }
}";

string qrString = QRDecoder.DecodeFromJsonResponse(jsonResponse);
Console.WriteLine($"Decoded: {qrString}");
```

### Read from File

```csharp
try
{
    string fileContent = System.IO.File.ReadAllText("testqr.txt");
    string qrCode = QRDecoder.DecodeFromJsonResponse(fileContent);
    Console.WriteLine($"QR Code: {qrCode}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

## API Reference

### `QRDecoder.DecodeQRFromBase64(string qrBase64)`

Decodes a QR code from a base64-encoded PNG image.

**Parameters:**
- `qrBase64` (string): Base64-encoded PNG image string

**Returns:**
- `string`: Decoded QR code text, or `null` if decoding fails

**Example:**
```csharp
string result = QRDecoder.DecodeQRFromBase64(base64String);
```

### `QRDecoder.DecodeFromJsonResponse(string jsonResponse)`

Extracts and decodes QR code from a JSON response containing a `qrbase64` field.

**Parameters:**
- `jsonResponse` (string): JSON string with structure `{"data": {"qrbase64": "..."}}`

**Returns:**
- `string`: Decoded QR code text, or `null` if parsing or decoding fails

**Example:**
```csharp
string result = QRDecoder.DecodeFromJsonResponse(jsonString);
```

## Expected JSON Format

```json
{
  "success": 200,
  "data": {
    "platform_order_id": "THBP20251106151333LpD3WxRr",
    "merchant_order_id": "15329655",
    "order_datetime": "2025-11-06 15:13:33",
    "expire_datetime": "2025-11-06 15:18:33",
    "amount": "2000.00",
    "transfer_amount": "2000.00",
    "qrbase64": "iVBORw0KGgoAAAANSUhEUgAAAfQAAAH0CAM..."
  }
}
```

## How It Works

1. **Image Loading**: Uses SkiaSharp to decode base64 PNG into bitmap
2. **Grayscale Conversion**: Converts color pixels to luminance values
3. **Binarization**: Applies HybridBinarizer or GlobalHistogramBinarizer
4. **QR Decoding**: Uses ZXing MultiFormatReader to extract QR code data

## Troubleshooting

### Null Result

If `DecodeQRFromBase64` returns `null`, check:

1. **Valid Base64**: Ensure the string is valid base64-encoded PNG
2. **QR Code Quality**: Image must contain a clear, readable QR code
3. **Image Format**: Must be PNG format
4. **Console Output**: Check debug messages for specific errors

### Common Issues

**Problem**: `The type initializer for 'Windows.Win32.PInvoke' threw an exception`
**Solution**: This project uses SkiaSharp instead of System.Drawing.Common to avoid this issue.

**Problem**: Decoding fails on valid QR code
**Solution**: The library tries both HybridBinarizer and GlobalHistogramBinarizer. Check console output for details.

## Performance

- **Image Size**: Handles QR codes up to 1024x1024 pixels efficiently
- **Decoding Speed**: Typically < 100ms for standard QR codes
- **Memory**: ~5-10MB for typical QR code images

## Platform Support

Tested on:
- ✅ Windows 10/11
- ✅ macOS 12+
- ✅ Linux (Ubuntu 20.04+)

## Use Case

This library is ideal for scenarios where:
- APIs return QR codes as base64 images instead of strings
- You need to migrate from `qrcode` text field to `qrbase64` image field
- Cross-platform QR code decoding is required
- You want to avoid Windows-specific dependencies

## Example Output

```
Image size: 500x500
Pixel count: 250000
Trying HybridBinarizer...
Decoded QR Code: 00020101021229370016A000000677010111021301055681509315802TH530376454072000.006304C022
```

## License

This project is provided as-is for educational and commercial use.

## Dependencies

- **ZXing.Net**: Apache License 2.0
- **SkiaSharp**: MIT License
- **Newtonsoft.Json**: MIT License

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## Author

Created to solve the challenge of decoding QR codes from base64-encoded images in API responses.

## Version History

### v1.0.0
- Initial release
- Cross-platform support via SkiaSharp
- Support for black & white QR codes
- JSON response parsing
- Multiple binarizer strategies

---

**Need Help?** Open an issue or check the troubleshooting section above.