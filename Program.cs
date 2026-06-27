using Session1;

var builder = WebApplication.CreateBuilder(args); // building the web application

builder.Services.AddControllers();// tellls that we are using controllers in our application
//iske begair api/books/ ka route kaam nahi karega
builder.Services.AddEndpointsApiExplorer(); //dsicover endpoints for swagger
builder.Services.AddSwaggerGen();

// TASK 3.5 — Register IBookService as Scoped 
builder.Services.AddScoped<IBookService, BookService>();// this tells whnvr iBookSevice is requested, give BookService instance
// scoped means one instance per HTTP request 
// (jaise ek request aayegi to ek instance create hoga, aur dusri request aayegi to dusra instance create hoga)

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

app.UseHttpsRedirection();// this will redirect http request to https request

// TASK 2.4 — Register request logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Must run before MapControllers so unhandled BookNotFoundException becomes 404 ProblemDetails.
app.UseExceptionHandler();

app.MapControllers();//maps url to controller action methods 
//so that when we hit api/books/ it will go to BooksController

app.Run();
