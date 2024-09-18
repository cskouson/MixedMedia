using Microsoft.EntityFrameworkCore;
using MixedMedia.Data;
using MixedMedia.Domain.Services.Interfaces;
using MixedMedia.Domain.Services;
using MixedMedia.Data.Repositories.Interfaces;
using MixedMedia.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
var congfiguration = builder.Configuration;

//add dbcontext
builder.Services.AddDbContext<MixedDbContext>
    (options => options.UseNpgsql(congfiguration.GetConnectionString("DefaultConnection")));

//add repos and services
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<IVideoService, VideoService>();

// Add coontrollers and mapper to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
