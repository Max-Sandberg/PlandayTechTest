using Planday.Schedule.Entities;

namespace Planday.Schedule.Services
{
    public class AssignEmployeeToShiftResult
    {
        public ResultType Type { get; }

        public Shift? Shift { get; }

        public Employee? Employee { get; }

        public IEnumerable<Shift>? OverlappingShifts { get; }

        public enum ResultType
        {
            Success,

            ShiftDoesNotExist,

            EmployeeDoesNotExist,

            ShiftConflict
        }

        private AssignEmployeeToShiftResult(ResultType resultType, Shift? shift = null, Employee? employee = null, IEnumerable<Shift>? overlappingShifts = null)
        {
            Type = resultType;
            Shift = shift;
            Employee = employee;
            OverlappingShifts = overlappingShifts;
        }

        public static AssignEmployeeToShiftResult Success(Shift shift, Employee employee)
            => new AssignEmployeeToShiftResult(ResultType.Success, shift, employee);

        public static AssignEmployeeToShiftResult EmployeeDoesNotExist()
            => new AssignEmployeeToShiftResult(ResultType.EmployeeDoesNotExist);

        public static AssignEmployeeToShiftResult ShiftDoesNotExist()
            => new AssignEmployeeToShiftResult(ResultType.ShiftDoesNotExist);

        public static AssignEmployeeToShiftResult ShiftConflict(Shift shift, Employee employee, IEnumerable<Shift> overlappingShifts)
            => new AssignEmployeeToShiftResult(ResultType.ShiftConflict, shift, employee, overlappingShifts);
    }
}