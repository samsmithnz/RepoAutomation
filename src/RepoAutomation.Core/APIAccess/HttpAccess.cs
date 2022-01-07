//namespace RepoAutomation.Core.APIAccess
//{
//    public static class HttpAccess
//    {
//        public static async Task DownloadFileTaskAsync(this HttpClient client,
//            Uri uri,
//            string FileName)
//        {
//            using (Stream? s = await client.GetStreamAsync(uri))
//            {
//                using (FileStream? fs = new FileStream(FileName, FileMode.CreateNew))
//                {
//                    await s.CopyToAsync(fs);
//                }
//            }
//        }
//    }
//}