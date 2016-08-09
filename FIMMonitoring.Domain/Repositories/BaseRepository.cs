namespace FIMMonitoring.Domain
{
    public class BaseRepository
    {
        protected SoftLogsContext SoftLogsContext = new SoftLogsContext();

        protected FIMContext FimContext = new FIMContext();
    }
}
