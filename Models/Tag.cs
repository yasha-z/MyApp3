namespace Session1
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // tried this to avoid the null reference warning
    }
}