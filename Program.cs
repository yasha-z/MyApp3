using Session1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TASK 3.5 — Register IBookService as Scoped (one instance per HTTP request).
builder.Services.AddScoped<IBookService, BookService>();

// TASK 3.7 — ProblemDetails + handler so GET /api/books/999 returns 404 application/problem+json.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<BookNotFoundExceptionHandler>();

var app = builder.Build();

InMemoryStore.SeedData();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// TASK 2.4 — Register request logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Must run before MapControllers so unhandled BookNotFoundException becomes 404 ProblemDetails.
app.UseExceptionHandler();

app.MapControllers();

app.Run();
