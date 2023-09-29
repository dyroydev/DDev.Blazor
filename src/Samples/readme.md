# Sampes for DDev.Blazor
This folder contains sample applications showcasing real usage of the components from [DDev.Blazor](https://github.com/dyroydev/DDev.Blazor).  

## Sample applications
- [ScheduleApp](https://github.com/dyroydev/DDev.Blazor/blob/main/src/Samples/ScheduleApp)

## Common
The `Common`-project contains code that simplifies the sample logic.  

### Data storage
Some sample applications work best when they can store persistant data.  
`IKeyValueDb` from `Common` stores data has json-files on your desktop folder.  
This is to make removing the data as easy as possible and encouraged.  
  
The folder created on your desktop, `ddev/`, contains all the data stored by every sample app.

## Make a new sample app
1. Create a new application in the `src/Samples/` folder.
1. Add a project-reference to the `src/DDev.Blazor`-project.
1. Add a project-reference to the `src/Samples/Common`-project.
1. Add the following to `Program.cs`:  
   ```chsarp
   builder.Services.AddSampleUtilitiesForBlazorServerSide("MyAppName");
   builder.Services.AddSampleUtilitiesForBlazorServerSide("MyAppName");
   ```
   *If you are using `WebAssembly`, then use `AddSampleUtilitiesForBlazorWebAssembly` instead.*
1. Add the following to `App.razor`:
   ```html
   <DDevBlazor />
   ```
