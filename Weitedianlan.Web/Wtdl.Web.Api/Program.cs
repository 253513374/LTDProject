using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using System.Text;
using Senparc.Weixin.AspNet;
using Senparc.Weixin.MP;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ��������֤����
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "apiissuer",
            ValidAudience = "apiaudience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a94c36eaf8bf49f68099d8db3e28372e"))
        };
    });

builder.Services.AddMemoryCache();
////Senparc.Weixin ע�ᣨ���룩
builder.Services.AddSenparcWeixinServices(builder.Configuration);

// ��� MVC ����
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();//��СAPI ֧��
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    //ʹ�� AddSecurityDefinition �������һ����Ϊ "Bearer" �İ�ȫ���壬
    //������Ϊ JWT ��Ȩ��ͷʹ�� Bearer ������
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "ʹ�� Bearer ������ JWT ��Ȩ��ͷ������,ע��ո�: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    //ʹ�� AddSecurityRequirement ������Ӱ�ȫҪ�󣬲�����ǰ�涨��� "Bearer" ��ȫ����
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

#region ����΢������

var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;

//����΢�����ã����룩
var registerService = app.UseSenparcWeixin(app.Environment,
    null /* ��Ϊ null �򸲸� appsettings  �е� SenpacSetting ����*/,
    null /* ��Ϊ null �򸲸� appsettings  �е� SenpacWeixinSetting ����*/,
    register => { /* CO2NET ȫ������ */ },
    (register, weixinSetting) =>
    {
        //ע�ṫ�ں���Ϣ������ִ�ж�Σ�ע�������ںţ�
        register.RegisterMpAccount(weixinSetting, "��ʢ������С���֡����ں�");
    });

#endregion ����΢������

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // c.RoutePrefix = string.Empty;
        // c.OAuthClientId("<ClientId>");
        //c.OAuthAppName("My API - OAuth2 Implicit Grant");
        // c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    });
}

app.UseHttpsRedirection();//�ض���https

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();