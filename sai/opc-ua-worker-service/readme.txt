configure worker service as windows service
https://medium.com/@adebanjoemmanuel01/running-a-worker-service-as-a-windows-service-c1d12a28a73c

package required to configure worker service as windows service
Microsoft.Extensions.Hosting.WindowsServices

add below line in program.cs

builder.Services.AddWindowsService();
