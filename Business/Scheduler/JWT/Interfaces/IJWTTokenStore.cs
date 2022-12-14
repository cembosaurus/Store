namespace Business.Scheduler.JWT.Interfaces
{
    public interface IJWTTokenStore
    {
        bool IsExipred { get; }
        string Token { get; set; }
    }
}
