namespace VisumCRM
{
    public class Core
    {

        public static long? ConvertDateTimeToLong(DateTime? Value)
        {
            long? result = null;
            if (Value != null)
            {
                var t = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}", Value.Value.Year, Value.Value.Month, Value.Value.Date, Value.Value.Hour, Value.Value.Minute, Value.Value.Second );
                long l = 0;
                if (long.TryParse(t, out l))
                {
                    result = l;
                }
            }
            return result;
        }
    }
}
