using TranslateAPIgoogle.entities;

namespace TranslateAPIgoogle.Interface
{
    public interface IOrderService
    {
        IQueryable<User> LayHoaDon(int? userID);
        Manager updateManager(Manager manager, int numberIPsys,int NumberOfUsersSystem, int NumberOfusesSystem);
        Address addIP(Address ip, string AddressIP, int NumberOfUses);
        User AddUser(User User, int lstorder, int addressID);
        Task<string> LstOrderResult(List<Order> orders, int idUser);
        Task<string> OrderTranslate(User User);

        Task<string> translate(string? InLanguage = null, string? outLanguage = null, string? Translate = null);
        string ShowResult(string addressIp, int? NumberOfUsers, int? NumberOfusesip, string UserName, int? userNumberOfuses, string res);

    }
}
