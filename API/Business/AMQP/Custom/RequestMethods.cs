namespace Business.AMQP.Custom
{
    public static class RequestMethods
    {
        public enum Item
        {
            None,
            GetAll,
            Get,
            GetById,
            GetByName,
            Add,
            Update,
            Remove,
            ExistById,
            ExistByName
        }

    }
}
