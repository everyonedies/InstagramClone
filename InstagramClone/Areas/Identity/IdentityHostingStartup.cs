using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(InstagramClone.Areas.Identity.IdentityHostingStartup))]
namespace InstagramClone.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}