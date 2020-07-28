using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedUserAsync(StoreContext context, ILoggerFactory logger)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!context.ProductBrands.Any())
                {
                    var brandsData =
                      File.ReadAllText(path.Replace("/API/bin/Debug/netcoreapp3.1", "") + @"/Infrastructure/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }
                    await context.SaveChangesAsync();

                }
                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText(path.Replace("/API/bin/Debug/netcoreapp3.1", "") + @"/Infrastructure/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText(path.Replace("/API/bin/Debug/netcoreapp3.1", "") + @"/Infrastructure/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<ProductSeedModel>>(productsData);
                    foreach (var item in products)
                    {
                        var pictureFileName = item.PictureUrl.Substring(16);

                        var product = new Product
                        {
                            Name = item.Name,
                            Description = item.Description,
                            Price = item.Price,
                            NetPrice = item.NetPrice,
                            Quantity = item.Quantity,
                            ProductBrandId = item.ProductBrandId,
                            ProductTypeId = item.ProductTypeId,
                            IsActive = item.IsActive
                        };
                        product.AddPhoto(item.PictureUrl, pictureFileName);
                        context.Products.Add(product);
                    }
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception err)
            {
                var log = logger.CreateLogger<StoreContextSeed>();
                log.LogError(err.Message);
            }

        }
    }
}