using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace Jumpin.Services
{
    public class RouteService : IRouteService
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IConfiguration configuration;
        private readonly IErrorProviderService errorProviderService;
        public CodeStatus error = new CodeStatus() { ErrorStatus = false };
        public CodeStatus defaultError = new CodeStatus() { ErrorStatus = true, Name = "The property must not be null" };

        public RouteService(ApplicationDbContext dbContext, IConfiguration _configuration, IErrorProviderService _errorProviderService) 
        {
            DbContext = dbContext;
            configuration = _configuration;
            errorProviderService = _errorProviderService;
        }
        public async Task<CodeStatus> AddRoute(dtoUserRoute route)
        {
            if (route == null)
                return defaultError;

            var email = route.Email;

            if (route.SeatsNumber <= 0)
            {
                return error = await errorProviderService.GetCodeStatus("Number of seats must be minimum one!", true);
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                return error = await errorProviderService.GetCodeStatus("User not found!", true);
            }

            var newRoute = new TheRoute()
            {
                Name = route.Name,
                Coordinates = route.Coordinates,
                Description = route.Description,
                SeatsNumber = route.SeatsNumber,
                DateAndTime = route.DateAndTime,
                Price = route.Price,
                Type = route.Type,
                Status = "Active"
            };


            await DbContext.Routes.AddAsync(newRoute);
            await DbContext.SaveChangesAsync();

            var newUserRoute = new UserRoute()
            {
                User = userFromDatabase,
                Route = newRoute
            };


            await DbContext.UserRoutes.AddAsync(newUserRoute);
            await DbContext.SaveChangesAsync();


            return error = await errorProviderService.GetCodeStatus("Successfully created!", false);
        }


        public async Task<CodeStatus> DeleteRoute([FromRoute] int routeId)
        {
            if (routeId < 0)
            {
                return error = await errorProviderService.GetCodeStatus("Invalid ID", true);
            }

            var routeFromDatabase = await DbContext.UserRoutes.FirstOrDefaultAsync(x => x.Route.Id == routeId);
            var onlyRoute = await DbContext.Routes.Include(x => x.Coordinates).FirstOrDefaultAsync(x => x.Id == routeId);

            if (routeFromDatabase == null || onlyRoute == null) 
            {
                return error = await errorProviderService.GetCodeStatus("Route not found!", true);
            }

            DbContext.UserRoutes.Remove(routeFromDatabase);
            DbContext.Routes.Remove(onlyRoute);
            await DbContext.SaveChangesAsync();

            return error = await errorProviderService.GetCodeStatus("Successfully removed!", false);
        }


        public async Task<(CodeStatus, UserRoute)> UpdateRoute([FromBody] dtoUserRoute route, [FromRoute] int routeId)
        {
            if (route == null)
            {
                return (defaultError, null);
            }

            if (routeId < 0)
            {
                error = await errorProviderService.GetCodeStatus("Invalid ID", true);
                return (error, null);
            }

            var routeFromDatabase = await DbContext.UserRoutes.Include(x=>x.User).Include(x=>x.Route).FirstOrDefaultAsync(x=>x.Route.Id == routeId);

            var onlyRoute = await DbContext.Routes.FirstOrDefaultAsync(x => x.Id == routeId);

            if (routeFromDatabase == null || onlyRoute == null) 
            {
                error = await errorProviderService.GetCodeStatus("Route not found!", true);
                return (error, null);
            }

            onlyRoute.Name = route.Name != null ? route.Name : onlyRoute.Name;
            onlyRoute.SeatsNumber = route.SeatsNumber >= 0 ? route.SeatsNumber : onlyRoute.SeatsNumber;
            onlyRoute.DateAndTime = route.DateAndTime != null ? route.DateAndTime : onlyRoute.DateAndTime;
            onlyRoute.Price = route.Price >= 0 ? route.Price : onlyRoute.Price;
            onlyRoute.Description = route.Description != null ? route.Description : onlyRoute.Description;
            onlyRoute.Type = route.Type != null ? route.Type : onlyRoute.Type;

            DbContext.Routes.Update(onlyRoute);
            DbContext.UserRoutes.Update(routeFromDatabase);
            await DbContext.SaveChangesAsync();

            return (error, routeFromDatabase);
        }

        public async Task<(CodeStatus, List<UserRoute>)> GetRoutes()
        {
            var routes = await DbContext.UserRoutes.Include(x=>x.Route.Coordinates).Include(x => x.User).Include(x => x.Route).Where(x => x.Route.Status == "Active").ToListAsync();

            if(routes == null || routes.Count == 0)
            {
                error = await errorProviderService.GetCodeStatus("No routes created!", true);
                return (error, null);
            }

            return (error, routes);
        }

        public async Task<(CodeStatus, List<UserRoute>)> GetUsersRoutes(string email)
        {
            var routes = await DbContext.UserRoutes.Where(x=> x.User.Email == email).Include(x => x.Route.Coordinates).Include(x => x.User).Include(x => x.Route).ToListAsync();

            if (routes == null || routes.Count == 0)
            {
                error = await errorProviderService.GetCodeStatus("No routes created!", true);
                return (error, null);
            }

            return (error, routes);
        }

        public async Task<(CodeStatus, UserRoute)> GetRouteDetailsById(int routeId)
        {
            if (routeId < 0)
            {
                error = await errorProviderService.GetCodeStatus("Invalid ID", true);   
                return (error, null);
            }

            var routeDetails = await DbContext.UserRoutes
                .Include(x => x.User) 
                .Include(x => x.Route)
                .Include(x => x.Route.Coordinates)
                .FirstOrDefaultAsync(x => x.Route.Id == routeId);

            if (routeDetails == null)
            {
                error = await errorProviderService.GetCodeStatus("Route not found!", true);
                return (error, null);
            }
 
            return (error, routeDetails);
        }
    }
}
