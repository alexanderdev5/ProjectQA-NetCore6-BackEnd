using Aplicacion.Cursos;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using WebAPI.Middleware;

namespace WebAPI;

public class Startup
{

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<CursosOnlineContext>(opt =>
        {
            opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        });


        services.AddMediatR(typeof(Consulta.Manejador).Assembly);
        


        services.AddControllers().AddFluentValidation( cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());
    }


    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {

        app.UseMiddleware<ManejadorErrorMiddleware>();
        if (env.IsDevelopment())
        {
           // app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseDeveloperExceptionPage();

        // If Authorization is used, it must be added explicitely unlike ASP.NET Core 2.2
        // Needs to be added after UseRouting()
        // app.UseAuthorization(); 

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}