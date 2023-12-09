using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

await builder
    .AddServices()
    .BuildApplication()
    .AddSwagger()
    .AddApplicationServices()
    .RunAppAsync();