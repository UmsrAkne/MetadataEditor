using System.Windows;
using MetadataEditor.Utils;

namespace MetadataEditor.Core
{
    using System;
    using System.Text.RegularExpressions;

    public static class PromptExtractor
    {
        /// <summary>
        /// メタデータから特定のプロンプトを抽出してクリップボードにコピーします
        /// </summary>
        /// <param name="metadata">メタデータ全文</param>
        /// <param name="targetIsPrompt">true の場合は "prompt"、 false の場合は "negative prompt" を対象に動作します。</param>
        public static void ExtractAndCopy(string metadata, bool targetIsPrompt)
        {
            if (string.IsNullOrWhiteSpace(metadata))
            {
                LogWriter.Write("メタデータが空白またはnullです。コピー処理をスキップします。");
                return;
            }

            var result = string.Empty;

            if (targetIsPrompt)
            {
                // "Negative prompt:" が現れるまでの文字列を取得
                var index = metadata.IndexOf("Negative prompt:", StringComparison.Ordinal);
                if (index != -1)
                {
                    result = metadata.Substring(0, index).Trim();
                }
            }
            else
            {
                // 正規表現で "Negative prompt:" から "Steps:" までの間を抜き出す
                var match = Regex.Match(metadata, @"Negative prompt:(.*?)(?=Steps:|$)", RegexOptions.Singleline);
                if (match.Success)
                {
                    result = match.Groups[1].Value.Trim();
                }
            }

            if (!string.IsNullOrEmpty(result))
            {
                Clipboard.SetText(result);
                LogWriter.Write($"{targetIsPrompt} をコピーしました:\n{result}");
            }
            else
            {
                LogWriter.Write("該当する項目が見つかりませんでした。");
            }
        }
    }
}