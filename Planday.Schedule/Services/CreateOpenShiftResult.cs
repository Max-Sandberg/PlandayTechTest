using Planday.Schedule.Entities;

namespace Planday.Schedule.Services
{
    public class CreateOpenShiftResult
    {
        public ResultType Type { get; }

        public Shift? Shift { get; }

        public enum ResultType
        {
            Success,

            StartAfterEnd,

            StartAndEndDifferentDays
        }

        private CreateOpenShiftResult(ResultType resultType, Shift? shift = null)
        {
            Type = resultType;
            Shift = shift;
        }

        public static CreateOpenShiftResult Success(Shift shift)
            => new CreateOpenShiftResult(ResultType.Success, shift);

        public static CreateOpenShiftResult StartAfterEnd()
            => new CreateOpenShiftResult(ResultType.StartAfterEnd);

        public static CreateOpenShiftResult StartAndEndDifferentDays()
            => new CreateOpenShiftResult(ResultType.StartAndEndDifferentDays);
    }
}