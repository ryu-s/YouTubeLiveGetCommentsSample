using System;

namespace YouTubeLiveGetCommentsSample
{
    class Program
    {
        static void Main(string[] _)
        {
            var vid = Console.ReadLine();
            new YouTubeLiveCommentGetter().ShowComments(vid);
        }
    }
}
