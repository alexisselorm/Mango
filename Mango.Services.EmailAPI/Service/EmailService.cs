using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.DTO;
using Mango.Services.EmailAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Service
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDTO cartDTO)
        {
            StringBuilder message = new();
            message.AppendLine("<br/>Cart Email Requested");
            message.AppendLine("<br/>Total" + cartDTO.CartHeader.TotalAmount);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDTO.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");

            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDTO.CartHeader.Email);
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = message = "User Registration successful. <br/>Email:" + email;
            await LogAndEmail(message, "agbeckor2@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new()
                {
                    Email = email,
                    Message = message,
                    EmailSent = DateTime.Now,
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLogger);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
