using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlockchainWallet.Data;
using BlockchainWallet.Data.Repos;
using BlockchainWallet.Models.Dto;

namespace BlockchainWallet
{
    using global::AutoMapper;
    using Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<NodeData>(Configuration.GetSection("NodeData"));
            services.Configure<NodeData>(options =>
            {
                var urls = Configuration.GetSection("NodeData:Url").Get<string[]>();
                var endPoint = Configuration.GetSection("NodeData:Endpoint").Get<NodeData.Endpoint>();
                var maxBlocksInQuery = Configuration.GetSection("NodeData:MaxBlocksInQuery").Get<int>();
                var startingPage = Configuration.GetSection("NodeData:StartingPage").Get<int>();

                options.Url = urls;
                options.Endpoints = endPoint;
                options.MaxBlocksInQuery = maxBlocksInQuery;
                options.StartingPage = startingPage;
            });


            services.AddDbContext<BlockchainDbContext>(options => options.UseInMemoryDatabase());
            //options.UseSqlServer(Configuration.GetConnectionString("BlockchainDbConnection")));

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); 
            services.AddTransient<IHttpRequestService, HttpRequestService>();
            services.AddTransient<IBalanceCalculator, BalanceCalculator>();
            services.AddTransient<AddressService, AddressService>();
            services.AddTransient<MnemonicService, MnemonicService>();
            services.AddTransient<ITransactionManager, TransactionManager>();
            services.AddTransient<IHistoryExtractor, HistoryExtractor>();

            services.AddAutoMapper();
            services.AddMemoryCache();
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
