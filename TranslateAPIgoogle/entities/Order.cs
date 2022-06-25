namespace TranslateAPIgoogle.entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string inpLanguage { get; set; }
        public string outLanguage { get; set; }
        public string Input { get; set; }
        public string Result { get; set; }
        public DateTime TimeOrder { get; set; }
        public virtual User User { get; set; }
    }
}
