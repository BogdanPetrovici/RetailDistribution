using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RetailDistribution.Client.UI.Test
{
	public class TestBase
	{
		public readonly string ExpectedGetDistrictsResultString = "[{'DistrictId':1,'DistrictName':'District1','Vendors':null,'PrimaryVendor':{'VendorId':2,'VendorName':'Vendor2','Districts':null,'IsPrimary':false}},{'DistrictId':2,'DistrictName':'District2','Vendors':null,'PrimaryVendor':{'VendorId':2,'VendorName':'Vendor2','Districts':null,'IsPrimary':false}},{'DistrictId':3,'DistrictName':'District3','Vendors':null,'PrimaryVendor':{'VendorId':2,'VendorName':'Vendor2','Districts':null,'IsPrimary':false}}]";
		public readonly string ExpectedGetVendorsResultString = "[{'VendorId':1,'VendorName':'Vendor3','Districts':null,'IsPrimary':false},{'VendorId':2,'VendorName':'Vendor2','Districts':null,'IsPrimary':true},{'VendorId':3,'VendorName':'Vendor1','Districts':null,'IsPrimary':false}]";
		public readonly string ExpectedGetShopsResultString = "[{'ShopId':1,'ShopName':'Shop1','District':null},{'ShopId':3,'ShopName':'Shop3','District':null}]";
		public HttpClient GetMockedHttpClient(string expectedResponseContent)
		{
			// ARRANGE
			var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			handlerMock
			   .Protected()
			   // Setup the PROTECTED method to mock
			   .Setup<Task<HttpResponseMessage>>(
				  "SendAsync",
				  ItExpr.IsAny<HttpRequestMessage>(),
				  ItExpr.IsAny<CancellationToken>()
			   )
			   // prepare the expected response of the mocked http call
			   .ReturnsAsync(new HttpResponseMessage()
			   {
				   StatusCode = HttpStatusCode.OK,
				   Content = new StringContent(expectedResponseContent, Encoding.UTF8, "application/json"),
			   })
			   .Verifiable();

			// use real http client with mocked handler here
			var httpClient = new HttpClient(handlerMock.Object)
			{
				BaseAddress = new Uri("http://test.com/"),
			};

			return httpClient;
		}
	}
}
