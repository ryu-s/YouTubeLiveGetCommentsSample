using Codeplex.Data;
using System;
using System.Threading;

namespace YouTubeLiveGetCommentsSample
{
    /// <summary>
    /// YouTubeLiveのコメントを取得しコンソールに表示するクラス
    /// </summary>
    class YouTubeLiveCommentGetter
    {
        /// <summary>
        /// 指定されたvidのコメントを取得してコンソールに表示する
        /// </summary>
        /// <param name="vid"></param>
        public void ShowComments(string vid)
        {
            var liveChatUrl = $"https://www.youtube.com/live_chat?v={vid}&is_popout=1";
            var liveChatHtml = Tools.HttpGet(liveChatUrl);
            var ytInitialData = Tools.ExtractYtInitialData(liveChatHtml);
            var parsedYtInitialData = DynamicJson.Parse(ytInitialData);
            if (parsedYtInitialData.contents.liveChatRenderer.IsDefined("actions"))
            {
                foreach(var action in parsedYtInitialData.contents.liveChatRenderer.actions)
                {
                    if (action.IsDefined("addChatItemAction") && action.addChatItemAction.item.IsDefined("liveChatTextMessageRenderer"))
                    {
                        string message;
                        if (action.addChatItemAction.item.liveChatTextMessageRenderer.message.IsDefined("runs"))
                        {
                            message = (string)action.addChatItemAction.item.liveChatTextMessageRenderer.message.runs[0].text;
                        }
                        else
                        {
                            message = (string)action.addChatItemAction.item.liveChatTextMessageRenderer.message.simpleText;
                        }
                        Console.WriteLine(message);
                    }
                }
            }

            string continuation;
            if (parsedYtInitialData.contents.liveChatRenderer.continuations[0].IsDefined("invalidationContinuationData"))
            {
                continuation = (string)parsedYtInitialData.contents.liveChatRenderer.continuations[0].invalidationContinuationData.continuation;
            }
            else
            {
                continuation = (string)parsedYtInitialData.contents.liveChatRenderer.continuations[0].timedContinuationData.continuation;
            }
            while (true)
            {
                var getLiveChatUrl = $"https://www.youtube.com/live_chat/get_live_chat?continuation={System.Web.HttpUtility.UrlEncode(continuation)}&pbj=1";
                var getLiveChat = Tools.HttpGet(getLiveChatUrl);
                var parsed = DynamicJson.Parse(getLiveChat);
                if (parsed.response.continuationContents.liveChatContinuation.IsDefined("actions"))
                {
                    foreach (var action in parsed.response.continuationContents.liveChatContinuation.actions)
                    {
                        if (action.IsDefined("addChatItemAction") && action.addChatItemAction.item.IsDefined("liveChatTextMessageRenderer"))
                        {
                            string message;
                            if (action.addChatItemAction.item.liveChatTextMessageRenderer.message.IsDefined("runs"))
                            {
                                message = (string)action.addChatItemAction.item.liveChatTextMessageRenderer.message.runs[0].text;
                            }
                            else
                            {
                                message = (string)action.addChatItemAction.item.liveChatTextMessageRenderer.message.simpleText;
                            }
                            Console.WriteLine(message);
                        }
                    }
                }
                if (parsed.response.continuationContents.liveChatContinuation.continuations[0].IsDefined("invalidationContinuationData"))
                {
                    continuation = (string)parsed.response.continuationContents.liveChatContinuation.continuations[0].invalidationContinuationData.continuation;
                }
                else
                {
                    continuation = (string)parsed.response.continuationContents.liveChatContinuation.continuations[0].timedContinuationData.continuation;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
