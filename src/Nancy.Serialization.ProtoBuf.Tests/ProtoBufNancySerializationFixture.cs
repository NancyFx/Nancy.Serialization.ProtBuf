using System.IO;
using Nancy.Serialization.ProtoBuf.Demo.Model;
using Nancy.Testing;
using Nancy.Tests;
using Xunit;
using ProtoBufSerializer = ProtoBuf.Serializer;

namespace Nancy.Serialization.ProtoBuf.Tests
{    
    public class ProtoBufNancySerializationFixture
    {
        private const string UserName = "testUser";
        private const int UserAge = 29;

        [Fact]
        public void TestGet()
        {
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            var url = string.Format("/getProtoBuf/{0}/{1}", UserName, UserAge);
            var response = browser.Get(url, with => with.HttpRequest());

            CheckResponse(response, UserName, UserAge);
        }

        [Fact]
        public void TestPost()
        {
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // get the serialized bytes to the service
            var response = browser.Post(
                "/postProtoBuf",
                with =>
                {
                    with.HttpRequest();
                    with.FormValue("Name", UserName);
                    with.FormValue("Age", UserAge.ToString());
                });

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
