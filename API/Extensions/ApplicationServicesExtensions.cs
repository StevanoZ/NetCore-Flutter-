using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions {
    public static class ApplicationServicesExtensions {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services) {
            services.AddScoped<ITokenRepository, TokenRepository> ();
            services.AddScoped<IUnitOfWork, UnitOfWork> ();
            services.AddScoped<IPhotoRepository, PhotoRepository> ();
            services.AddScoped<IBasketRepository, BasketRepository> ();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped (typeof (IGenericRepository<>), typeof (GenericRepository<>));
            services.AddScoped<IResponseCacheRepository, ResponseCacheRepository>();
            services.Configure<ApiBehaviorOptions> (options => {
                options.InvalidModelStateResponseFactory = ActionContext => {
                    var errors = ActionContext.ModelState.Where (e => e.Value.Errors.Count > 0)
                        .SelectMany (x => x.Value.Errors)
                        .Select (x => x.ErrorMessage).ToArray ();
                    var errorResponse = new ApiValidationErrorResponse {
                        Errors = errors
                    };
                    return new BadRequestObjectResult (errorResponse);
                };
            });
            return services;
        }
    }
}