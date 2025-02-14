using AlarmSystem.Services.Interfaces;
using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Jumpin.Services
{
    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IErrorProviderService errorProviderService;
        private readonly IEmailService emailService;

        public CodeStatus error = new CodeStatus() { ErrorStatus = false };
        public CodeStatus defaultError = new CodeStatus() { ErrorStatus = true, Name = "The property must not be null" };

        public RequestService(ApplicationDbContext _context, IErrorProviderService _errorProviderService, IEmailService _emailService)
        {
            DbContext = _context;
            errorProviderService = _errorProviderService;
            emailService = _emailService;
        }
        public async Task<CodeStatus> AcceptOrDeclineRequest(dtoRequestAcceptOrDecline dtoRequest)
        {
            if (dtoRequest == null)
            {
                return defaultError;
            }

            if(dtoRequest.RequestType == "Route")
            {
                var response = await AcceptOrDeclineRouteRequest(dtoRequest);
                return response;
            }
            else if (dtoRequest.RequestType == "Car")
            {
                var response = await AcceptOrDeclineCarRequest(dtoRequest);
                return response;
            }
            else if (dtoRequest.RequestType == "Flat")
            {
                var response = await AcceptOrDeclineFlatRequest(dtoRequest);
                return response;
            }

            return defaultError;

        }

        public async Task<CodeStatus> SendRequest(dtoRequest dtoRequest)
        {
            if (dtoRequest == null)
            {
                return defaultError;
            }
           
            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x=>x.Email == dtoRequest.PassengerEmail);

            if (userFromDatabase == null)
            {
                return error = await errorProviderService.GetCodeStatus("User not found!", true);
            }

            if (dtoRequest.ObjectType == "Route")
            {
                error = await RouteRequest(dtoRequest);
            }
            else if (dtoRequest.ObjectType == "Car")
            {
                error = await CarRequest(dtoRequest);
            }
            else if (dtoRequest.ObjectType == "Flat")
            {
                error = await FlatRequest(dtoRequest);
            }

            return error;
            
        }

        #region Routes
        public async Task<(CodeStatus, List<RouteRequest>)> GetRouteRequestsRecived(string email)
        {
            if(email == null)
            {
                return (defaultError, null);
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("User not found!", true);
                return (error, null);
            }

            var requestsFromDatabase = await DbContext.RouteRequests.Include(x => x.UserRoute).Include(x => x.UserRoute.User).Include(x => x.UserRoute.Route)
                                                                                                   .Where(x => x.UserRoute.User.Email == email).ToListAsync();

            if(requestsFromDatabase.Count == 0 || requestsFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("No recived requests!", true);
                return (error, null);
            }


            return (error, requestsFromDatabase);
        }

        public async Task<(CodeStatus, List<RouteRequest>)> GetRouteSentRequests(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return (defaultError, null);
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("User not found!", true);
                return (error, null);
            }

            var requestsFromDatabase = await DbContext.RouteRequests
                .Include(x => x.UserRoute)
                .Include(x => x.UserRoute.User)
                .Include(x => x.UserRoute.Route)
                .Where(x => x.PassengerEmail == email)
                .ToListAsync();

            if (requestsFromDatabase == null || requestsFromDatabase.Count == 0)
            {
                error = await errorProviderService.GetCodeStatus("No sent requests!", true);
                return (error, null);
            }

            return (error, requestsFromDatabase);
        }


        #endregion

        #region Cars

        public async Task<(CodeStatus, List<CarRequest>)> GetCarRequestsRecived(string email)
        {
            if (email == null)
            {
                return (defaultError, null);
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("User not found!", true);
                return (error, null);
            }

            var requestsFromDatabase = await DbContext.CarRequests.Include(x => x.UserCar).Include(x => x.UserCar.User).Include(x => x.UserCar.Car)
                                                                                                   .Where(x => x.UserCar.User.Email == email).ToListAsync();

            if (requestsFromDatabase.Count == 0 || requestsFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("No recived requests!", true);
                return (error, null);
            }


            return (error, requestsFromDatabase);
        }

        public async Task<(CodeStatus, List<CarRequest>)> GetCarSentRequests(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return (defaultError, null);
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("User not found!", true);
                return (error, null);
            }

            var requestsFromDatabase = await DbContext.CarRequests.Include(x => x.UserCar).Include(x => x.UserCar.User).Include(x => x.UserCar.Car)
                                                                                                    .Where(x => x.PassengerEmail == email).ToListAsync();

            if (requestsFromDatabase == null || requestsFromDatabase.Count == 0)
            {
                error = await errorProviderService.GetCodeStatus("No sent requests!", true);
                return (error, null);
            }

            return (error, requestsFromDatabase);
        }

        #endregion

        #region Flats

        public async Task<(CodeStatus, List<FlatRequest>)> GetFlatRequestsRecived(string email)
        {
            if (email == null)
            {
                return (defaultError, null);
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("User not found!", true);
                return (error, null);
            }

            var requestsFromDatabase = await DbContext.FlatRequests.Include(x => x.UserFlat).Include(x => x.UserFlat.User).Include(x => x.UserFlat.Flat)
                                                                                                   .Where(x => x.UserFlat.User.Email == email).ToListAsync();

            if (requestsFromDatabase.Count == 0 || requestsFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("No recived requests!", true);
                return (error, null);
            }


            return (error, requestsFromDatabase);
        }

        public async Task<(CodeStatus, List<FlatRequest>)> GetFlatSentRequests(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return (defaultError, null);
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("User not found!", true);
                return (error, null);
            }

            var requestsFromDatabase = await DbContext.FlatRequests.Include(x => x.UserFlat).Include(x => x.UserFlat.User).Include(x => x.UserFlat.Flat)
                                                                                                    .Where(x => x.PassengerEmail == email).ToListAsync();

            if (requestsFromDatabase == null || requestsFromDatabase.Count == 0)
            {
                error = await errorProviderService.GetCodeStatus("No sent requests!", true);
                return (error, null);
            }

            return (error, requestsFromDatabase);
        }

        #endregion

        #region Private Functions

        private async Task<CodeStatus> FlatRequest(dtoRequest dtoRequest)
        {

            var isRequestExist = await DbContext.FlatRequests.Include(x => x.UserFlat).FirstOrDefaultAsync(x => x.PassengerEmail == dtoRequest.PassengerEmail
                                                                                    && x.UserFlat.Id == dtoRequest.ObjectId && (x.Status == "Pending" || x.Status == "Accepted"));

            if (isRequestExist != null)
            {
                return error = await errorProviderService.GetCodeStatus("Already sent request", true);
            }

            var flatFromDatabase = await DbContext.UserFlats.Include(x => x.User).Include(x => x.Flat).FirstOrDefaultAsync(x => x.Id == dtoRequest.ObjectId);


            if (flatFromDatabase == null)
            {
                return error = await errorProviderService.GetCodeStatus("Car not found!", true);
            }

            if (flatFromDatabase.User.Email == dtoRequest.PassengerEmail)
            {
                return error = await errorProviderService.GetCodeStatus("You can't request your ad!", true);
            }

            var newRequest = new FlatRequest()
            {
                PassengerEmail = dtoRequest.PassengerEmail,
                Status = dtoRequest.Status,
                Description = dtoRequest.Description,
                UserFlat = flatFromDatabase
            };

            await emailService.SendPdfEmail(newRequest);

            await DbContext.FlatRequests.AddAsync(newRequest);
            await DbContext.SaveChangesAsync();

            return error = await errorProviderService.GetCodeStatus("Successfully sent!", false);
        }


        private async Task<CodeStatus> CarRequest(dtoRequest dtoRequest)
        {
            var isRequestExist = await DbContext.CarRequests.Include(x => x.UserCar).FirstOrDefaultAsync(x => x.PassengerEmail == dtoRequest.PassengerEmail
                                                                                    && x.UserCar.Id == dtoRequest.ObjectId && (x.Status == "Pending" || x.Status == "Accepted"));

            if (isRequestExist != null)
            {
                return error = await errorProviderService.GetCodeStatus("Already sent request", true);
            }

            var carFromDatabase = await DbContext.UserCars.Include(x => x.User).Include(x => x.Car).FirstOrDefaultAsync(x => x.Id == dtoRequest.ObjectId);


            if (carFromDatabase == null)
            {
                return error = await errorProviderService.GetCodeStatus("Car not found!", true);
            }

            if (carFromDatabase.User.Email == dtoRequest.PassengerEmail)
            {
                return error = await errorProviderService.GetCodeStatus("You can't request your ad!", true);
            }

            var newRequest = new CarRequest()
            {
                PassengerEmail = dtoRequest.PassengerEmail,
                Status = dtoRequest.Status,
                Description = dtoRequest.Description,
                UserCar = carFromDatabase
            };

            await emailService.SendPdfEmail(newRequest);

            await DbContext.CarRequests.AddAsync(newRequest);
            await DbContext.SaveChangesAsync();

            return error = await errorProviderService.GetCodeStatus("Successfully sent!", false);
        }

        private async Task<CodeStatus> RouteRequest(dtoRequest dtoRequest)
        {
            var isRequestExist = await DbContext.RouteRequests.Include(x => x.UserRoute).FirstOrDefaultAsync(x => x.PassengerEmail == dtoRequest.PassengerEmail
                                                                                    && x.UserRoute.Id == dtoRequest.ObjectId && (x.Status == "Pending" || x.Status == "Accepted"));

            if (isRequestExist != null)
            {
                return error = await errorProviderService.GetCodeStatus("Already sent request", true);
            }

            var routeFromDatabase = await DbContext.UserRoutes.Include(x => x.User).Include(x => x.Route).FirstOrDefaultAsync(x => x.Id == dtoRequest.ObjectId);


            if (routeFromDatabase == null)
            {
                return error = await errorProviderService.GetCodeStatus("Route not found!", true);
            }

            if (routeFromDatabase.User.Email == dtoRequest.PassengerEmail)
            {
                return error = await errorProviderService.GetCodeStatus("You can't request your ad!", true);
            }

            var newRequest = new RouteRequest()
            {
                PassengerEmail = dtoRequest.PassengerEmail,
                Status = dtoRequest.Status,
                Description = dtoRequest.Description,
                UserRoute = routeFromDatabase
            };

            await emailService.SendPdfEmail(newRequest);

            await DbContext.RouteRequests.AddAsync(newRequest);
            await DbContext.SaveChangesAsync();

            return error = await errorProviderService.GetCodeStatus("Successfully sent!", false);
        }

        private async Task<CodeStatus> AcceptOrDeclineRouteRequest(dtoRequestAcceptOrDecline dtoRequest)
        {
            var requestFromDatabase = await DbContext.RouteRequests.Include(x => x.UserRoute.User).Include(x => x.UserRoute.Route)
                                                                            .FirstOrDefaultAsync(x => x.Id == dtoRequest.RequestId);

            if (requestFromDatabase == null)
                return error = await errorProviderService.GetCodeStatus("User not found!", true);

            if (dtoRequest.Decision == 0)
            {
                requestFromDatabase.Status = "Accepted";
                requestFromDatabase.UserRoute.Route.SeatsNumber -= 1;

                if (requestFromDatabase.UserRoute.Route.SeatsNumber == 0)
                {
                    requestFromDatabase.UserRoute.Route.Status = "Inactive";
                }
                DbContext.RouteRequests.Update(requestFromDatabase);
                await DbContext.SaveChangesAsync();

                await emailService.SendPdfEmail(requestFromDatabase, dtoRequest.Decision);

                return error = await errorProviderService.GetCodeStatus("Request accepted!", false);
            }
            else if (dtoRequest.Decision == 1)
            {
                requestFromDatabase.Status = "Declined";

                DbContext.RouteRequests.Update(requestFromDatabase);
                await DbContext.SaveChangesAsync();

                await emailService.SendPdfEmail(requestFromDatabase, dtoRequest.Decision);

                return error = await errorProviderService.GetCodeStatus("Request declined!", false);
            }
            else
            {
                return error = await errorProviderService.GetCodeStatus("Invalid decision", true);
            }
        }

        private async Task<CodeStatus> AcceptOrDeclineCarRequest(dtoRequestAcceptOrDecline dtoRequest)
        {
            var requestFromDatabase = await DbContext.CarRequests.Include(x => x.UserCar.User).Include(x => x.UserCar.Car)
                                                                            .FirstOrDefaultAsync(x => x.Id == dtoRequest.RequestId);

            if (requestFromDatabase == null)
                return error = await errorProviderService.GetCodeStatus("User not found!", true);

            if (dtoRequest.Decision == 0)
            {
                requestFromDatabase.Status = "Accepted";
                requestFromDatabase.UserCar.Car.Status = "Inactive";

                await emailService.SendPdfEmail(requestFromDatabase, dtoRequest.Decision);
                DbContext.CarRequests.Update(requestFromDatabase);
                await DbContext.SaveChangesAsync();

                return error = await errorProviderService.GetCodeStatus("Request accepted!", false);
            }
            else if (dtoRequest.Decision == 1)
            {
                requestFromDatabase.Status = "Declined";
                await emailService.SendPdfEmail(requestFromDatabase, dtoRequest.Decision);

                DbContext.CarRequests.Update(requestFromDatabase);
                await DbContext.SaveChangesAsync();

                return error = await errorProviderService.GetCodeStatus("Request declined!", false);
            }
            else
            {
                return error = await errorProviderService.GetCodeStatus("Invalid decision", true);
            }
        }

        private async Task<CodeStatus> AcceptOrDeclineFlatRequest(dtoRequestAcceptOrDecline dtoRequest)
        {
            var requestFromDatabase = await DbContext.FlatRequests.Include(x => x.UserFlat.User).Include(x => x.UserFlat.Flat)
                                                                            .FirstOrDefaultAsync(x => x.Id == dtoRequest.RequestId);

            if (requestFromDatabase == null)
                return error = await errorProviderService.GetCodeStatus("User not found!", true);
                

            if (dtoRequest.Decision == 0)
            {
                requestFromDatabase.Status = "Accepted";
                requestFromDatabase.UserFlat.Flat.Status = "Inactive";
                await emailService.SendPdfEmail(requestFromDatabase, dtoRequest.Decision);

                DbContext.FlatRequests.Update(requestFromDatabase);
                await DbContext.SaveChangesAsync();

                return error = await errorProviderService.GetCodeStatus("Request accepted!", false);
            }
            else if (dtoRequest.Decision == 1)
            {
                requestFromDatabase.Status = "Declined";
                await emailService.SendPdfEmail(requestFromDatabase, dtoRequest.Decision);

                DbContext.FlatRequests.Update(requestFromDatabase);
                await DbContext.SaveChangesAsync();

                return error = await errorProviderService.GetCodeStatus("Request declined!", false);
            }
            else
            {
                return error = await errorProviderService.GetCodeStatus("Invalid decision", true);
            }
        }


        #endregion
    }
}
