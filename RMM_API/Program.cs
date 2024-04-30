using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.Authority = "https://localhost:5443";
        o.Audience = "weatherapi";
        
        o.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        o.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // Log the exception~
                // Log.Error($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },

            OnForbidden =  context =>
            {
                // Log.Error($"Authentication failed: {context.Principal.}");
                return Task.CompletedTask;
            },
            
            OnChallenge = context =>
            {
                if (!context.Handled)
                {
                    string error1 = string.Empty;
                    // Log.Warning($"Authentication challenge triggered: {error1}");
                }
                return Task.CompletedTask;
            },
            // ... other event handlers ...
        };
        o.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true
        };
    });
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapForecastEndpoints();

app.Run();
