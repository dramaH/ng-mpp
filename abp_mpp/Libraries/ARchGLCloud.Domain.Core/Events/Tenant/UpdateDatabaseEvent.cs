namespace ARchGLCloud.Domain.Core.Events
{
    public class UpdateDatabaseEvent : Event
    {
        public UpdateDatabaseEvent(string sql)
        {
            Sql = sql;
        }

        public string Sql { get; set; }
    }
}
