namespace SmartX.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () =>
            {
                return "Smart-X Iot Mesh Getaway is running.";
            }); 

            app.Run();
        }
    }
}
