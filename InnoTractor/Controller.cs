using InnoTractor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapGet("/fizzbuzz", async () =>
{
    List<FizzBuzzDto> result= new List<FizzBuzzDto>();
    for(int i = 0; i < 31; i++)
    {
        var resultstring = "";
        if(i % 3 == 0)
        {
            resultstring += "Fizz";
        }
        if(i % 5 == 0)
        {
            resultstring += "Buzz";
        }
        if(resultstring == "")
        {
            result.Add(new FizzBuzzDto { Input = i, Output = i.ToString() });
        }
        else
        {
            result.Add(new FizzBuzzDto { Input = i, Output = resultstring });
        }
    }
    return Results.Ok(result);
});
app.MapGet("/values", async () =>
{
    Variables variables = new Variables();
    var result = 0;
    foreach (var variable in variables.Values())
    {
        if (variable == null)
        {
            continue;
        }
        else if (variable.Contains('e'))
        {
            var splitvar = variable.Split('e');
            result += (int)(Convert.ToDouble(splitvar[0]) * Math.Pow(10, Convert.ToDouble(splitvar[1])));
        }
        else if (variable.Contains('.'))
        {
            var splitvar = variable.Split('.');
            result += (int)(Convert.ToDouble(splitvar[0]) * Math.Pow(10, Convert.ToDouble(variable.Length-2))) + int.Parse(splitvar[1]);
        }
        else
        {
            result += int.Parse(variable);
        }
    }
    return Results.Ok(result);
});
app.MapPost("/runningtotal", async (int input) =>
{
    var result = 0;
    string path = @"C:\Users\Public\runningTotal.txt";
    if(!File.Exists(path))
    {
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine(input);
        }
        result = input;
        return Results.Ok(result);
    }
    else
    {
        var totalString = File.ReadAllText(path);
        var total = totalString.Split("\r\n");
        if(total.Length == 2)
        {
            result = int.Parse(total[0]) + input;
        }
        else if(total.Length == 3)
        {
            result = int.Parse(total[0]) + int.Parse(total[1]) + input;
        }
        else
        {
            result = int.Parse(total[total.Length - 3]) + int.Parse(total[total.Length-2]) + input;
        }
        File.AppendAllText(path, input.ToString() + Environment.NewLine);
        return Results.Ok(result);
    }
});

app.Run();