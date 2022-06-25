using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using TranslateAPIgoogle.Context;
using TranslateAPIgoogle.entities;
using TranslateAPIgoogle.Interface;
using xNet;
using HttpRequest = xNet.HttpRequest;

namespace TranslateAPIgoogle.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext appDbContext;
        public OrderService() { appDbContext = new AppDbContext(); }

        public IQueryable<User> LayHoaDon(int? userID)
        {
            var query = appDbContext.users.Include(x => x.Orders).AsQueryable();//.Include(x => x.Users).ThenInclude(x => x.Orders).AsQueryable();
            if (userID.HasValue) { query = query.Where(x=>x.UserID == userID); }
            return query;
        }

        /// <summary>
        /// hiển thị 
        /// </summary>
        /// <param name = "addressIp" > địa chỉ IP</param>
        /// <param name = "NumberOfUsers" > số người dùng ở IP</param>
        /// <param name = "NumberOfusesip" > số lần đã dùng ở IP</param>
        ///    / <param name = "UserName" > tên người dùng</param>
        /// <param name = "userNumberOfuses" > số lần người dùng này đã dùng</param>
        /// <param name = "res" > kết quả của dịch</param>
        ///   / <returns></returns>
        /// 
        public string ShowResult(string addressIp, int? NumberOfUsers, int? NumberOfusesip,
                                 string UserName, int? userNumberOfuses, string res)
        {

            return $"ip {addressIp} co {NumberOfUsers} nguoi dung ; ip nay da dung {NumberOfusesip} lan\n" +
                   $"user ten {UserName} da dung {userNumberOfuses} lan \n" +
                   $"{res}";
        }
        public Manager updateManager(Manager manager,int numberIPsys,int NumberOfUsersSystem,int NumberOfusesSystem) 
        {
            manager.NumberIpSystem += numberIPsys;
            manager.NumberOfUsersSystem += NumberOfUsersSystem;
            manager.NumberOfusesSystem += NumberOfusesSystem;
            return manager;
        }
        public Address addIP(Address ip, string AddressIP, int NumberOfUses)
        {
            ip.AddressIP = AddressIP;
            ip.NumberOfUses = NumberOfUses;
            ip.NumberOfUsers = 1;
            ip.CreateTimeIP = DateTime.Now;
            ip.Active = 1;
            return ip;
        }
        /// <summary>
        /// add id của địa chỉ ip; số lượng dùng ban đầu,
        /// </summary>
        /// <param name = "User" > user </ param >
        /// < param name="lstorder">số lượng order</param>
        /// <param name = "addressID" > đia chỉ ip(ID)</param>
        public User AddUser(User User, int lstorder, int addressID)
        {
            User.NumberOfuses = lstorder;
            User.AddressID = addressID;
            User.CreateTimeUser = DateTime.Now;
            User.Active = 1;
            appDbContext.Add(User);
            appDbContext.SaveChanges();
            return User;
        }

        /// <summary>
        /// lấy kết quả từ order(đầu vào và đầu ra của dịch)
        /// </summary>
        /// <param name = "orders" > order </ param >
        /// < param name="idUser">id user</param>
        /// <returns>result</returns>
        public async Task<string> LstOrderResult(List<Order> orders, int idUser)
        {
            string res = "";
            foreach (var Order in orders)
            {
                Order.UserID = idUser;
                Order.TimeOrder = DateTime.Now;
                Order.Result = await translate(Order.inpLanguage, Order.outLanguage, Order.Input);
                res += Order.Input + " => " + Order.Result + "\n"+"thoi gian order : "+Order.TimeOrder+"\n";
            }
            return res;
        }

        /// <summary>
        /// dịch ngôn ngữ api của google dùng xNet để call
        /// </summary>
        /// <param name = "InLanguage" > ngôn ngữ</param>
        /// <param name = "outLanguage" > ngôn ngữ cần dịch</param>
        /// <param name = "Translate" > chữ muốn dịch</param>
        /// <returns>kết quả dịch</returns>
        public async Task<string> translate(string? InLanguage = null, string? outLanguage = null, string? Translate = null)
        {
            if (string.IsNullOrEmpty(InLanguage) || string.IsNullOrEmpty(outLanguage) || string.IsNullOrEmpty(Translate))
            { throw new Exception("thieu"); }

            HttpRequest http = new HttpRequest();
            string html = http.Get($"https://translate.googleapis.com/translate_a/single?client=gtx&sl={InLanguage}&tl={outLanguage}&dt=t&q={Translate}").ToString();
            Regex regex = new Regex(@$"\[\[\[\"".*?(.*?)\""{Translate}\""");
            string[] res = regex.Match(html).ToString().Split("\"");
            return res[1];
        }

        /// <summary>
        /// check ip và add user vào db and update số lần dùng...
        /// </summary> 
        /// <param name = "User" > (tên user) => user </ param >
        /// < returns ></ returns >
        public async Task<string> OrderTranslate(User User)
        {
            using (var trans = appDbContext.Database.BeginTransaction())
            {
                var manager = appDbContext.managers.FirstOrDefault();
                // get ip user
                string IP = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
                var lstOrder = User.Orders.ToList();
                User.Orders = null;
                string res = "";
                int NumberOfuses = lstOrder.Count();
                //check ip ton tai chua
                var addressIp = appDbContext.addresses.FirstOrDefault(x => x.AddressIP == IP);
                if (addressIp != null)
                {
                    if (addressIp.Active == 0) { return $"ip : {IP} đã bị block : v"; }
                    //check ip có bị block ko
                    // kiem tra xem user da ton tai chua
                    var userdb = appDbContext.users.FirstOrDefault(x => x.UserName.ToLower().Contains(User.UserName.ToLower()));
                    //neu ton tai thi add lst order va cap nhap so lan da dung
                    addressIp.NumberOfUses += NumberOfuses;//update số lần dùng ở ip
                        //nếu tồn tại và ko bị block thì update                   
                    if (userdb != null)
                    {
                        if (userdb.Active == 0) { return $"user {userdb.UserName} đã bị block : v"; }
                        appDbContext.Update(updateManager(manager,0,0, NumberOfuses));
                        userdb.NumberOfuses += NumberOfuses;
                        appDbContext.Update(userdb);
                        res = await LstOrderResult(lstOrder, userdb.UserID);
                        await appDbContext.AddRangeAsync(lstOrder);
                        appDbContext.SaveChanges();
                        trans.Commit();
                        return ShowResult(addressIp.AddressIP, addressIp.NumberOfUsers, addressIp.NumberOfUses,
                                              User.UserName, userdb.NumberOfuses, res);
                    }
                    else//add new user
                    {
                        addressIp.NumberOfUsers += 1;
                        appDbContext.Update(addressIp);
                        appDbContext.Update(updateManager(manager,0,1, NumberOfuses));
                        //ko tồn tại user thì add user đó vào: v
                        AddUser(User, lstOrder.Count(), addressIp.AddressID);
                        // add trả ra kết quả dịch
                        res = await LstOrderResult(lstOrder, User.UserID);
                        appDbContext.AddRange(lstOrder);
                        await appDbContext.SaveChangesAsync();
                        trans.Commit();
                        return ShowResult(addressIp.AddressIP, addressIp.NumberOfUsers, addressIp.NumberOfUses,
                                          User.UserName, User.NumberOfuses, res);
                    }                 
                }
                //khong ton tai thi add new ip
                Address IPnew = new Address();
                appDbContext.addresses.Add(addIP(IPnew, IP, lstOrder.Count()));
                appDbContext.SaveChanges();
                //add mew user
                AddUser(User, lstOrder.Count(), IPnew.AddressID);
                //update manager
                appDbContext.Update(updateManager(manager,1,1, lstOrder.Count()));
                res = await LstOrderResult(lstOrder, User.UserID);
                appDbContext.AddRange(lstOrder);
                await appDbContext.SaveChangesAsync();
                trans.Commit();
                return res;
            }
        }

    }
}
