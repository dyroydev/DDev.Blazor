# DDevBlazor Common sample utilities
Common utilities for sample applications.

## Usage
1. Add a project-reference to the common project	
1. Add the following to `Program.cs`
   ```chsarp
   builder.Services.AddSampleUtilitiesForBlazorServerSide("MyAppName");
   ```
   or  
   ```chsarp
   builder.Services.AddSampleUtilitiesForBlazorWebAssembly("MyAppName");
   ```
   respectively.

## Utilities

# IKeyValueDb
An extremely simple storage service for persistent application state.
