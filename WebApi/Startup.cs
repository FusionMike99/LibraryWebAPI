using Business;
using Business.Interfaces;
using Business.Services;
using Data;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(connection));
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IReaderRepository, ReaderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutomapperProfile));
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IReaderService, ReaderService>();
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}