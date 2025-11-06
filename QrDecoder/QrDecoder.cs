using System;
using System.Drawing;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace QrDecoder;

public class QrDecoder
{
    /// <summary>
    /// Decodes a QR code from a base64 encoded PNG image using SkiaSharp (cross-platform)
    /// </summary>
    /// <param name="qrBase64">Base64 encoded PNG image string</param>
    /// <returns>Decoded QR code string, or null if decoding fails</returns>
    public static string DecodeQRFromBase64(string qrBase64)
    {
        try
        {
            // Convert base64 string to byte array
            byte[] imageBytes = Convert.FromBase64String(qrBase64);

            // Load image using SkiaSharp
            using (var stream = new System.IO.MemoryStream(imageBytes))
            using (var bitmap = SKBitmap.Decode(stream))
            {
                if (bitmap == null)
                {
                    Console.WriteLine("Failed to decode image from base64");
                    return null;
                }

                // Convert SKBitmap to pixel data
                var width = bitmap.Width;
                var height = bitmap.Height;
                var pixels = bitmap.Pixels;

                Console.WriteLine($"Image size: {width}x{height}");
                Console.WriteLine($"Pixel count: {pixels.Length}");

                // For black and white QR codes, convert to grayscale luminance
                var luminances = new byte[width * height];
                for (int i = 0; i < pixels.Length; i++)
                {
                    var pixel = pixels[i];
                    // Simple grayscale conversion
                    luminances[i] = (byte)((pixel.Red + pixel.Green + pixel.Blue) / 3);
                }

                // Use PlanarYUVLuminanceSource for grayscale data
                var luminanceSource = new PlanarYUVLuminanceSource(
                    luminances,
                    width,
                    height,
                    0,
                    0,
                    width,
                    height,
                    false
                );

                // Try both binarizers
                BinaryBitmap binaryBitmap = null;
                Result result = null;

                // Set up hints
                var hints = new System.Collections.Generic.Dictionary<DecodeHintType, object>
                    {
                        { DecodeHintType.TRY_HARDER, true },
                        { DecodeHintType.POSSIBLE_FORMATS, new[] { BarcodeFormat.QR_CODE } }
                    };

                var reader = new MultiFormatReader();

                // Try with HybridBinarizer first
                try
                {
                    Console.WriteLine("Trying HybridBinarizer...");
                    binaryBitmap = new BinaryBitmap(new HybridBinarizer(luminanceSource));
                    result = reader.decode(binaryBitmap, hints);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"HybridBinarizer failed: {ex.Message}");
                }

                // If that fails, try GlobalHistogramBinarizer
                if (result == null)
                {
                    try
                    {
                        Console.WriteLine("Trying GlobalHistogramBinarizer...");
                        binaryBitmap = new BinaryBitmap(new GlobalHistogramBinarizer(luminanceSource));
                        result = reader.decode(binaryBitmap, hints);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"GlobalHistogramBinarizer failed: {ex.Message}");
                    }
                }

                return result?.Text;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error decoding QR code: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return null;
        }
    }
}
