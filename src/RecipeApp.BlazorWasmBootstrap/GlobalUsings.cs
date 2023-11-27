global using System;
global using System.Collections.Generic;
global using System.Net.Http;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Threading.Tasks;

global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.JSInterop;

global using RecipeApp.BlazorWasmBootstrap.Features.Shared.Enums;
global using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
global using RecipeApp.Shared.Features.Details;
global using RecipeApp.Shared.Features.Ingredient;
global using RecipeApp.Shared.Features.Instruction;
global using RecipeApp.Shared.Features.Introduction;
global using RecipeApp.Shared.Features.Session;
global using RecipeApp.Shared.MessageHandlers;
global using RecipeApp.Shared.Models;

global using Refit;

global using Tbd.Shared.ApiResult;
