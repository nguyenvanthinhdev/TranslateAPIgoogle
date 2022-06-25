namespace TranslateAPIgoogle.entities
{
    public class User
    {
        public int UserID { get; set; }
        public int AddressID { get; set; }
        public string UserName { get; set; }//tên ng dùng
        public DateTime CreateTimeUser { get; set; }
        public int? NumberOfuses { get; set; }//so lan da dung,
        public int? Active { get; set; }//cho phép dùng hay ko

        public virtual Address Address { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }
    }
}
