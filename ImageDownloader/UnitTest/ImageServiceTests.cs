using HtmlAgilityPack;
using ImageDownloader.DbContexts;
using ImageDownloader.Services;
using Moq;
using System.Net;
using Xunit;

public class ImageServiceTests
{
    [Fact]
    public async Task FetchImagesAsync_ShouldFetchImages()
    {
        // Arrange
        // Mocking the ApplicationDbContext
        var mockContext = new Mock<ApplicationDbContext>();

        // Creating an instance of the ImageService with the mocked context
        var imageService = new ImageService(mockContext.Object);

        // Setting up test parameters
        var query = "cute kittens";
        var maxImages = 5;

        // Mocking HtmlAgilityPack behavior
        var mockWeb = new Mock<HtmlWeb>();
        mockWeb.Setup(web => web.LoadFromWebAsync(It.IsAny<string>()))
            .ReturnsAsync(new HtmlDocument());

        // Mocking HttpClient behavior
        var mockHttpClient = new Mock<HttpClient>();
        var mockHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        mockHttpResponseMessage.Content = new ByteArrayContent(new byte[] { 1, 2, 3 }); // Mock image content
        mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(mockHttpResponseMessage);

        // Mocking IHttpClientFactory behavior
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(mockHttpClient.Object);

        // Act
        // Calling the method under test
        var images = await imageService.FetchImages(query, maxImages);

        // Assert
        // Verifying that the result is not null
        Assert.NotNull(images);

        // Verifying that the number of fetched images matches the expected count
        Assert.Equal(maxImages, images.Count);
    }
}
