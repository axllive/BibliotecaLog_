using BibliotecaLog.Data;
using BibliotecaLog.Repository;
using BibliotecaLog.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// Add services to the container.
//código autogerado
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//Adicionando o contexto do identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//Adicionando o contexto da model
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));
//código autogerado
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//identity carregada com serviços padrão sem detalhamento
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
// Add Dependency
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ILoanRepository, BookLoanRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

//execução da biuld
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
