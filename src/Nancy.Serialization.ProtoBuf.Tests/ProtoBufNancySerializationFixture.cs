namespace Nancy.Serialization.ProtoBuf.Tests
{
    using System.IO;
    using Nancy.Testing;
    
    using Xunit;
    
    using ProtoBufSerializer = global::ProtoBuf.Serializer;
    using global::ProtoBuf.Meta;
    
    public class ProtoBufNancySerializationFixture
    {
        private const string UserName = "testUser";
        private const int UserAge = 29;
		private const string VendorContentType = "application/vnd.company.product.v1+x-protobuf";

        static ProtoBufNancySerializationFixture()
        {
            RuntimeTypeModel.Default.Add(typeof (User), false).Add("Name", "Age");
        }

        [Fact]
        public void TestGet()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);
            var url = string.Format("/getProtoBuf/{0}/{1}", UserName, UserAge);

            // When
            var response = browser.Get(url, with =>
            {
                with.HttpRequest(); 
                with.Accept(Constants.ProtoBufContentType);
            });

            // Then
            CheckResponse(response, UserName, UserAge);
        }

		[Fact]
		public void TestGetWithVendorContentType()
		{
			// Given
			var bootstrapper = new DefaultNancyBootstrapper();
			var browser = new Browser(bootstrapper);
			var url = string.Format("/getProtoBuf/{0}/{1}", UserName, UserAge);

			// When
			var response = browser.Get(url, with =>
			{
				with.HttpRequest();
				
				with.Accept(VendorContentType);
			});

			// Then
			CheckResponse(response, UserName, UserAge);
		}

        [Fact]
        public void TestPost()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var response = browser.Post("/postProtoBuf", with =>
            {
                with.HttpRequest();
                with.FormValue("Name", UserName);
                with.FormValue("Age", UserAge.ToString());
                with.Accept(Constants.ProtoBufContentType);
            });

            // Then
            CheckResponse(response, UserName, UserAge);
        }

		[Fact]
		public void TestPostWithVendorContentType()
		{
			// Given
			var bootstrapper = new DefaultNancyBootstrapper();
			var browser = new Browser(bootstrapper);

			// When
			var response = browser.Post("/postProtoBuf", with =>
			{
				with.HttpRequest();
				with.FormValue("Name", UserName);
				with.FormValue("Age", UserAge.ToString());
				with.Accept(VendorContentType);
			});

			// Then
			CheckResponse(response, UserName, UserAge);
		}

        private static void CheckResponse(BrowserResponse response, string name, int age)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Constants.ProtoBufContentType, response.Context.Response.ContentType);
            Assert.IsType<ProtoBufResponse>(response.Context.Response);
            
            using (var memStream = new MemoryStream())
            {
                response.Context.Response.Contents(memStream);
                               
                // try and deserialize the response body stream with protobuf
                memStream.Position = 0;
                var returnedUser = ProtoBufSerializer.Deserialize<User>(memStream);

                Assert.NotNull(returnedUser);
                Assert.Equal(name, returnedUser.Name);
                Assert.Equal(age, returnedUser.Age);
            }
        }
    }
}
