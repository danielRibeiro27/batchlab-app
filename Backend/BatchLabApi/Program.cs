using System.Text.Json;
using BatchLabApi.Dto;
using BatchLabApi.Service.Implementation;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/jobs", () => "Hello World!");
app.MapPost("/jobs", async (string? jobsJson) =>
{
    if(string.IsNullOrEmpty(jobsJson))
        return Results.BadRequest("Job data is required.");
    
    //TO-DO: Inject service
    //TO-DO: Add error handling
    //TO-DO: Validate job data
    //TO-DO: Return proper response with job id or status
    //TO-DO: Log the request and response
    //TO-DO: Convert dto to domain model if needed
    JobApplicationService jobService = new();
    JobDto job = JsonSerializer.Deserialize<JobDto>(jobsJson)!;
    var result = await jobService.CreateAsync(job);
    if(!result)
        return Results.StatusCode(500);

    return Results.Created("/jobs", new { status =  result});
});

app.Run();