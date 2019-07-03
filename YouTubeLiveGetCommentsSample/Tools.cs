using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace YouTubeLiveGetCommentsSample
{
    static class Tools
    {
        /// <summary>
        /// 指定されたURLのHTMLソースを取得する
        /// </summary>
        /// <param name="url">取得したいWebページのURL</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            var client = new WebClient { Encoding = Encoding.UTF8 };
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36");
            return client.DownloadString(url);
        }
        /// <summary>
        /// チャット欄のHTMLソースからytInitialDataを抽出する
        /// </summary>
        /// <param name="livePageHtml">チャット欄のHTMLソース</param>
        /// <returns>ytInitialData</returns>
        public static string ExtractYtInitialData(string liveChatHtml)
        {
            var match = Regex.Match(liveChatHtml, "window\\[\"ytInitialData\"\\] = ({.+});\\s*</script>", RegexOptions.Singleline);
            var ytInitialData = match.Groups[1].Value;
            return ytInitialData;
        }
    }
}
