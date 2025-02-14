using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Jumpin.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlarmSystem.Services;
using AlarmSystem.Models;
using AlarmSystem.Services.Interfaces;
using AlarmSystem.Models.DTO;

namespace Jumpin.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IConfiguration Configuration;
        private readonly IErrorProviderService errorProviderService; 
        private readonly IEmailService emailService;
        private readonly SmsService smsService;

        public CodeStatus error = new CodeStatus() { ErrorStatus = false };
        public CodeStatus defaultError = new CodeStatus() { ErrorStatus = true, Name = "The property must not be null" };
        public CodeStatus invalidData = new CodeStatus() { ErrorStatus = true, Name = "Invalid data!" };

        public UserService(ApplicationDbContext _context, IConfiguration _configuration, IErrorProviderService _errorProviderService, IEmailService _emailService, SmsService _smsService) 
        {
            DbContext = _context;
            Configuration = _configuration;
            errorProviderService = _errorProviderService;
            emailService = _emailService;
            smsService = _smsService;
        }

        public async Task<List<User>> GetUsers()
        {
            return await DbContext.Users.ToListAsync();       
        }

        public async Task<CodeStatus> UserRegistration(dtoUserRegistration user)
        {
            if (user == null)
                return defaultError;

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == user.Email.ToLower());

            if (userFromDatabase != null)
            {
                return error = new CodeStatus()
                {
                    ErrorStatus = true,
                    Name = "Email already used!"
                };
            }

            string passwordHash = BCrypt.Net.BCrypt.HashString(user.Password);

            var newUser = new User()
            {
                Email = user.Email.ToLower(),
                PasswordHash = passwordHash,
                FirstName = user.FirstName.ToLower(),
                LastName = user.LastName.ToLower(),
                PhoneNumber = user.PhoneNumber,
                Status = "Active",
                EmailConfirmed = false,
                Role = user.Role,
                AccountType = "Classic"
            };

            await DbContext.Users.AddAsync(newUser);
            await DbContext.SaveChangesAsync();
            var token = CreateToken(newUser);

            var generatedCode = await GenerateCode();
            var userCode = new UserCode()
            {
                Email = user.Email,
                Code = generatedCode,
                CreatedDate = DateTime.Now,
            };
            await DbContext.UserCodes.AddAsync(userCode);
            await DbContext.SaveChangesAsync();
            await emailService.SendEmailWithCode(generatedCode, user.Email, "email");

            return error = new CodeStatus()
            {
                ErrorStatus = false,
                Name = "User registered!"
            };

        }

        public async Task<CodeStatus> AcceptUserCode(dtoUserCode userCode)
        {
            if (userCode == null)
                return defaultError;

            var codeFromDatabase = (await DbContext.UserCodes
                                .Where(x => x.Email == userCode.Email && x.Code == userCode.Code)
                                .ToListAsync())
                                .DistinctBy(x => x.CreatedDate)
                                .FirstOrDefault();


            if (codeFromDatabase == null)
            {
                return error = new CodeStatus()
                {
                    ErrorStatus = true,
                    Name = "Invalid code!"
                };
            }

            if (codeFromDatabase.Code == userCode.Code && userCode.CodeType == "Email")
            {
                var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == userCode.Email);
                if (userFromDatabase == null) 
                {
                    return error = new CodeStatus()
                    {
                        ErrorStatus = true,
                        Name = "User not found!"
                    };
                }
                userFromDatabase.EmailConfirmed = true;
                await DbContext.SaveChangesAsync();

                return error = new CodeStatus()
                {
                    ErrorStatus = false,
                    Name = "Code accepted!"
                };
            }
            else if(codeFromDatabase.Code == userCode.Code && userCode.CodeType == "Phone")
            {
                var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == userCode.Email);
                if (userFromDatabase == null)
                {
                    return error = new CodeStatus()
                    {
                        ErrorStatus = true,
                        Name = "User not found!"
                    };
                }
                userFromDatabase.PhoneConfirmed = true;
                await DbContext.SaveChangesAsync();

                return error = new CodeStatus()
                {
                    ErrorStatus = false,
                    Name = "Code accepted!"
                };
            }

            return error = new CodeStatus()
            {
                ErrorStatus = true,
                Name = "Invalid code!"
            };
        }

        public async Task<CodeStatus> ResendCode(string email, string type)
        {
            if (string.IsNullOrEmpty(email))
                return defaultError;

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if(userFromDatabase == null)
                return invalidData;

            var generatedCode = await GenerateCode();
            var userCode = new UserCode();
            if (type == "email" || type == "2fa")
            {
                userCode = new UserCode()
                {
                    Email = userFromDatabase.Email,
                    Code = generatedCode,
                    CreatedDate = DateTime.Now,
                    CodeType = "Email"
                };
            }
            else
            {
                userCode = new UserCode()
                {
                    Email = userFromDatabase.Email,
                    Code = generatedCode,
                    CreatedDate = DateTime.Now,
                    CodeType = "Phone"
                };
            }
            await DbContext.UserCodes.AddAsync(userCode);
            await DbContext.SaveChangesAsync();
            await emailService.SendEmailWithCode(generatedCode, userFromDatabase.Email, type);

            return error = new CodeStatus()
            {
                ErrorStatus = false,
                Name = "Code resent"
            };
        }

        public async Task<CodeStatus> SendEmailForPasswordReset(string email)
        {
            if (string.IsNullOrEmpty(email))
                return defaultError;

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userFromDatabase == null)
                return error = await errorProviderService.GetCodeStatus("User not found!", true);

            await emailService.SendEmailWithCode("000000", email, "password");

            return await errorProviderService.GetCodeStatus("Email sent!", false);

        }

        public async Task<CodeStatus> SendSmsForPhoneVerification(string email)
        {
            if (string.IsNullOrEmpty(email))
                return defaultError;

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userFromDatabase == null)
                return error = await errorProviderService.GetCodeStatus("User not found!", true);

            var generatedCode = await GenerateCode();
            var userCode = new UserCode()
            {
                Email = userFromDatabase.Email,
                Code = generatedCode,
                CreatedDate = DateTime.Now,
                CodeType = "Phone"
            };
            await DbContext.UserCodes.AddAsync(userCode);
            await DbContext.SaveChangesAsync();

            await smsService.SendVerificationCode(userFromDatabase.PhoneNumber, generatedCode);

            return await errorProviderService.GetCodeStatus("Sms sent!", false);

        }

        public async Task<CodeStatus> ResetPassword(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return defaultError;

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userFromDatabase == null)
                return error = await errorProviderService.GetCodeStatus("User not found!", true);

            string passwordHash = BCrypt.Net.BCrypt.HashString(password);

            userFromDatabase.PasswordHash = passwordHash;

            await DbContext.SaveChangesAsync();

            return error = await errorProviderService.GetCodeStatus("Password changed!", false);
        }

        public async Task<(CodeStatus, string)> UserLogin(dtoUserLogin user)
        {
            if (user == null)
                return (defaultError, null);

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == user.Email.ToLower());

            if (userFromDatabase == null)
            {
                return (invalidData, null);
            }

            if(!userFromDatabase.EmailConfirmed)
            {
                error = new CodeStatus()
                {
                    ErrorStatus = true,
                    Name = "Email not confirmed!"
                };

                return (error, null);
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, userFromDatabase.PasswordHash);
            if (!isPasswordValid)
            {
                return (invalidData, null);
            }

            var token = CreateToken(userFromDatabase);

            if(userFromDatabase.Status == "Banned")
            {
                error = new CodeStatus()
                {
                    ErrorStatus = true,
                    Name = "Your account has been banned."
                };

                return (error, null);
            }


            var generatedCode = await GenerateCode();
            var userCode = new UserCode()
            {
                Email = userFromDatabase.Email,
                Code = generatedCode,
                CreatedDate = DateTime.Now,
            };
            await DbContext.UserCodes.AddAsync(userCode);
            await DbContext.SaveChangesAsync();
            await emailService.SendEmailWithCode(generatedCode, userFromDatabase.Email, "2fa");

            return (error, token);
        }


        private string CreateToken(User user)
        {
            List<Claim> _claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    claims: _claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


        public async Task<(CodeStatus, User)> GetUserInformations(string email)
        {
            if (string.IsNullOrEmpty(email))
                return (defaultError, null);

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if(userFromDatabase == null)
            {
                error = await errorProviderService.GetCodeStatus("User not found!", true);
                return (error, null);
            }

            return (error, userFromDatabase);

        }

        public async Task<CodeStatus> UpdateUserInformations(dtoUserInformations dtoUserInformations)
        {
            if(dtoUserInformations == null)
            {
                return defaultError;
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == dtoUserInformations.Email);
            if(userFromDatabase == null)
            {
                return error = await errorProviderService.GetCodeStatus("User not found!", true);
            }

            userFromDatabase.FirstName = dtoUserInformations.FirstName != null ? dtoUserInformations.FirstName : userFromDatabase.FirstName;
            userFromDatabase.LastName = dtoUserInformations.LastName != null ? dtoUserInformations.LastName : userFromDatabase.LastName;    
            userFromDatabase.PhoneNumber = dtoUserInformations.PhoneNumber != null ? dtoUserInformations.PhoneNumber : userFromDatabase.PhoneNumber;
            userFromDatabase.ImageUrl = dtoUserInformations.ImageUrl != null ? dtoUserInformations.ImageUrl : userFromDatabase.ImageUrl;

            DbContext.Users.Update(userFromDatabase);
            await DbContext.SaveChangesAsync();
            return error = await errorProviderService.GetCodeStatus("Successfully changed!", false);

        }

        public async Task<CodeStatus> UpgradeUsersAccount(string email)
        {
            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if(userFromDatabase == null)
                return error = await errorProviderService.GetCodeStatus("User not found!", true);

            userFromDatabase.AccountType = "VIP";
            await DbContext.SaveChangesAsync();

            return error = await errorProviderService.GetCodeStatus("Successfully upgraded!", false);
        }


        public async Task<CodeStatus> ChangeUserStatus(int status, string email)
        {
            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userFromDatabase == null)
                return error = await errorProviderService.GetCodeStatus("User not found!", true);

            if(status ==  0)
                userFromDatabase.Status = "Banned";
            else if(status == 1)
                userFromDatabase.Status = "Active";
            else
                return error = await errorProviderService.GetCodeStatus("Invalid data!", true);

            await DbContext.SaveChangesAsync();
            return error = await errorProviderService.GetCodeStatus("Status changed!", false);
        }

        #region Private functions
        private async Task<string> GenerateCode()
        {
            Random random = new Random();
            return random.Next(100000, 1000000).ToString();
        }

        #endregion
    }
}
