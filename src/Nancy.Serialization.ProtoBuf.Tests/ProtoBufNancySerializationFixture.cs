namespace Nancy.Serialization.ProtoBuf.Tests
{
    using System.IO;
    using Nancy.Serialization.ProtoBuf.Demo.Model;
    using Nancy.Testing;
    using Nancy.Tests;
    using Xunit;
    using ProtoBufSerializer = global::ProtoBuf.Serializer;

    public class ProtoBufNancySerializationFixture
    {
        private const string UserName = "testUser";
        private const int UserAge = 29;

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

        private static void CheckResponse(BrowserResponse response, string name, int age)
        {
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
            response.Context.Response.ContentType.ShouldEqual(Constants.ProtoBufContentType);
            response.Context.Response.ShouldBeOfType(typeof(ProtoBufResponse));
            
            using (var memStream = new MemoryStream())
            {
                response.Context.Response.Contents(memStream);
                               
                // try and deserialize the response body stream with protobuf
                memStream.Position = 0;
                var returnedUser = ProtoBufSerializer.Deserialize<User>(memStream);

                returnedUser.ShouldNotBeNull();
                returnedUser.Name.ShouldEqual(name);
                returnedUser.Age.ShouldEqual(age);
            }
        }
    }
}
