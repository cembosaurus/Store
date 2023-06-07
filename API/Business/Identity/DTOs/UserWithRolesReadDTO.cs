namespace Business.Identity.DTOs
{
    public class UserWithRolesReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
