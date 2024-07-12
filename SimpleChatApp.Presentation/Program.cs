using Microsoft.EntityFrameworkCore;
using SimpleChatApp.BusinessLogic.Services;
using SimpleChatApp.DataAccess.Repositories;
using SimpleChatApp.DataAccess;
using static SimpleChatApp.Presentation.Controllers.ChatsController;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������� � ���������.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); // ��������� ��������� SignalR

// ������������ �������
builder.Services.AddSingleton<ChatHub>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IChatEventNotifier, SignalRChatEventNotifier>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ����������� �������� ��������� HTTP-��������.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// ������ ����������� � SignalR ���.
app.MapControllers();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/hubs/chat"); // ������ ��� SignalR
});

app.Run();
