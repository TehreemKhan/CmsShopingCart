using CmsShopingCart.Services.Models;

namespace CmsShopingCart.Services.Services
{
    public interface IEmailService
    {
        void SendMail(Message message);
    }
}
