using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public async Task TestMethod1()
        {
            var client = new NetCoreIdentityHttpClient.NetCoreIdentityHttpClient(null);
            var response = await client.GetAllUsers();
            response.Should().NotBeNull();
            response.IsFailure.Should().BeFalse(response.Error);
            response.Value.TotalCount.Should().BeGreaterThan(0);
            response.Value.Items.Length.Should().BeGreaterThan(0);
        }
    }
}
