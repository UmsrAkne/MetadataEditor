using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetadataEditor.Models;
using MetadataEditor.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png.Chunks;

namespace MetadataEditor.Core
{
    public class MetadataWriter
    {
        public static void Write(string imagePath, string metadataText, IEnumerable<Diff> diffs)
        {
            try
            {
                LogWriter.Write($"Write metadata: {Path.GetFileName(imagePath)}");

                using var image = Image.Load(imagePath);
                var pngMeta = image.Metadata.GetPngMetadata();

                var enumerable = diffs.Where(d => d.Enabled).ToList();
                if (enumerable.Any())
                {
                    foreach (var diff in enumerable)
                    {
                        metadataText = metadataText.Replace(diff.Key, diff.Value);
                    }
                }

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