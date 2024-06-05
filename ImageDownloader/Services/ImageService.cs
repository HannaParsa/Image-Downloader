using ImageDownloader.DbContexts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ImageDownloader.Services
{
    public class ImageService
    {
        private readonly ApplicationDbContext _context;

        public ImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DownloadAndStoreImages(string query, int maxNum)
        {
            var imageUrls = await FetchImages(query, maxNum);
            foreach (var imageUrl in imageUrls)
            {
                // Store image
                await StoreImage(query, imageUrl);
            }
        }

        public async Task<List<string>> FetchImages(string query, int maxNum)
        {
            var imageUrls = new List<string>();
            var url = $"https://www.google.com/search?q={query}&tbm=isch";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes("//img[@src]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var imageUrl = node.GetAttributeValue("src", null);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        imageUrls.Add(imageUrl);
                    }

                    if (imageUrls.Count >= maxNum)
                    {
                        break;
                    }
                }
            }

            return imageUrls;
        }

        public async Task StoreImage(string name, string imageUrl)
        {
            var image = new ImageData { name = name, url = imageUrl };
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
        }
    }
}
