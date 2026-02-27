using System;
using System.IO;
using MetadataEditor.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png.Chunks;

namespace MetadataEditor.Core
{
    public class MetadataWriter
    {
        public static void Write(string imagePath, string metadataText)
        {
            try
            {
                LogWriter.Write($"Write metadata: {Path.GetFileName(imagePath)}");

                using var image = Image.Load(imagePath);
                var pngMeta = image.Metadata.GetPngMetadata();

                // 既存 テキストを全削除
                pngMeta.TextData.Clear();

                // 新しい parameters を追加
                pngMeta.TextData.Add(new PngTextData(
                    "parameters",
                    metadataText,
                    null,
                    null));

                image.Save(imagePath);
            }
            catch (Exception ex)
            {
                LogWriter.Write($"Metadata write failed: {imagePath}");
                LogWriter.Write(ex.ToString());
                throw;
            }
        }
    }
}