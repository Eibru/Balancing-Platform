using BalancingPlatform.Logic;
using BalancingPlatform.Logic.Models.Runtime;
using BalancingPlatform.WEB;
using BalancingPlatform.WEB.Components;
using BalancingPlatform.WEB.ViewModels;
using MudBlazor.Services;
using OpenCvSharp;
using SystBalancingPlatform.Logicem.Models.Runtime;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddHttpClient();

var cvParams = Utility.ReadCvParams();
var pidParams = Utility.ReadPidParams();
var servoParams = Utility.ReadServoParams();
var cvRuntime = new CvRuntime();
var pidRuntime = new PidRuntime();
var servoRuntime = new ServoRuntime();
var cvController = new CvController(cvParams, cvRuntime, pidParams);
var pidController = new PidController(pidParams, pidRuntime, cvRuntime);
var servoController = new ServoController(servoParams, servoRuntime, pidRuntime);

builder.Services.AddSingleton(cvParams);
builder.Services.AddSingleton(pidParams);
builder.Services.AddSingleton(servoParams);
builder.Services.AddSingleton(cvRuntime);
builder.Services.AddSingleton(pidRuntime);
builder.Services.AddSingleton(servoRuntime);

builder.Services.AddSingleton<MainWindowViewModel>();

var cancellationToken = new CancellationToken();

Thread CvThread;
Thread PidThread;
Thread KinematicsThread;

CvThread = new Thread(async delegate () {
    await cvController.Run(cancellationToken);
});

PidThread = new Thread(async delegate () {
    await pidController.Run(cancellationToken);
});

KinematicsThread = new Thread(async delegate () {
    await servoController.Run(cancellationToken);
});

CvThread.Start();
PidThread.Start();
KinematicsThread.Start();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapGet("/cvruntime/{img}", async (string img, HttpContext context, CvRuntime cvRuntime) => {
    if (img != "src" && img != "hsv" && img != "mask")
        return Results.NotFound();

    context.Response.StatusCode = 200;
    context.Response.ContentType = "multipart/x-mixed-replace; boundary=--frame";

    var boundary = "\r\n--frame\r\n";
    var ct = context.RequestAborted;

    while (!ct.IsCancellationRequested) {
        byte[] frame = null;
        if (img == "src")
            frame = cvRuntime.SrcFrame;
        else if (img == "hsv")
            frame = cvRuntime.HsvFrame;
        else if (img == "mask")
            frame = cvRuntime.MaskFrame;

        if (frame != null) {
            await context.Response.WriteAsync(boundary, ct);
            await context.Response.WriteAsync($"Content-Type: image/jpeg\r\nContent-Length: {frame.Length}\r\n\r\n", ct);
            await context.Response.Body.WriteAsync(frame, ct);
            await context.Response.Body.FlushAsync(ct);
        }

        // Control frame rate (e.g., ~25 fps)
        await Task.Delay(40, ct);
    }

    return Results.Ok();
});

app.Run();