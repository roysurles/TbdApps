global using Asp.Versioning;

global using Dapper;

global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Primitives;
global using Microsoft.OpenApi;

global using RecipeApp.Shared.Extensions;

global using Swashbuckle.AspNetCore.SwaggerGen;
global using Swashbuckle.AspNetCore.SwaggerUI;

global using System;
global using System.Collections.Generic;
global using System.Data;
global using System.Data.SqlClient;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Net.Http;
global using System.Net.Http.Headers;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Threading;
global using System.Threading.Tasks;

global using Tbd.Shared.ApiLog;
global using Tbd.Shared.ApiResult;
global using Tbd.Shared.Constants;
global using Tbd.Shared.Extensions;
global using Tbd.Shared.Options;
global using Tbd.Shared.Pagination;
global using Tbd.WebApi.Shared.Controllers;
global using Tbd.WebApi.Shared.Extensions;
global using Tbd.WebApi.Shared.Repositories;
