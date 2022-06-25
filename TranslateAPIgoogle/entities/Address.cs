namespace TranslateAPIgoogle.entities
{
    public class Address
    {
        public int AddressID { get; set; }
        public string AddressIP { get; set; }//địa chỉ ip
        public DateTime CreateTimeIP { get; set; }//thời gian tạo
        public int? NumberOfUsers { get; set; }//số người dùng
        public int? NumberOfUses { get; set; }//tổng số lượt dùng
        public int? Active { get; set; }//quyền dùng ở hệ thống(1 là ok 0 là bị chặn)
        public virtual IEnumerable<User> Users { get; set; }
    }
}
