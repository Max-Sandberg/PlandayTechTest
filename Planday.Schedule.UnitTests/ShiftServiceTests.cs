using Moq;
using NUnit.Framework;
using Planday.Schedule.Entities;
using Planday.Schedule.Queries;
using Planday.Schedule.Services;

namespace Planday.Schedule.Tests.Services
{
    [TestFixture]
    public class ShiftServiceTests
    {
        private Mock<IAddShiftQuery> _mockAddShiftQuery;
        private Mock<IGetAllShiftsQuery> _mockGetAllShiftsQuery;
        private Mock<IGetShiftByIdQuery> _mockGetShiftByIdQuery;
        private Mock<IGetEmployeeByIdQuery> _mockGetEmployeeByIdQuery;
        private Mock<IGetShiftsByEmployeeQuery> _mockGetShiftsByEmployeeQuery;
        private Mock<IUpdateShiftQuery> _mockUpdateShiftQuery;
        private ShiftService _shiftService;

        [SetUp]
        public void SetUp()
        {
            _mockAddShiftQuery = new Mock<IAddShiftQuery>();
            _mockGetAllShiftsQuery = new Mock<IGetAllShiftsQuery>();
            _mockGetShiftByIdQuery = new Mock<IGetShiftByIdQuery>();
            _mockGetEmployeeByIdQuery = new Mock<IGetEmployeeByIdQuery>();
            _mockGetShiftsByEmployeeQuery = new Mock<IGetShiftsByEmployeeQuery>();
            _mockUpdateShiftQuery = new Mock<IUpdateShiftQuery>();

            _shiftService = new ShiftService(
                _mockAddShiftQuery.Object,
                _mockGetAllShiftsQuery.Object,
                _mockGetShiftByIdQuery.Object,
                _mockGetEmployeeByIdQuery.Object,
                _mockGetShiftsByEmployeeQuery.Object,
                _mockUpdateShiftQuery.Object
            );
        }

        #region GetAllShiftsAsync Tests
        [Test]
        public async Task GetAllShiftsAsync_ReturnsAllShifts()
        {
            // Arrange
            var shifts = new List<Shift>
            {
                new Shift(1, 1, DateTime.Today.AddHours(12), DateTime.Today.AddHours(14)),
                new Shift(2, 2, DateTime.Today.AddHours(14), DateTime.Today.AddHours(16)),
            };
            _mockGetAllShiftsQuery.Setup(x => x.QueryAsync()).ReturnsAsync(shifts);

            // Act
            var result = await _shiftService.GetAllShiftsAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(1));
        }
        #endregion

        #region GetShiftByIdAsync Tests
        [Test]
        public async Task GetShiftByIdAsync_ReturnsShift_WhenShiftExists()
        {
            // Arrange
            var shift = new Shift(1, 1, DateTime.Today.AddHours(12), DateTime.Today.AddHours(16));
            _mockGetShiftByIdQuery.Setup(x => x.QueryAsync(1)).ReturnsAsync(shift);

            // Act
            var result = await _shiftService.GetShiftByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetShiftByIdAsync_ReturnsNull_WhenShiftDoesNotExist()
        {
            // Arrange
            _mockGetShiftByIdQuery.Setup(x => x.QueryAsync(It.IsAny<long>())).ReturnsAsync((Shift)null);

            // Act
            var result = await _shiftService.GetShiftByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region CreateOpenShiftAsync Tests
        [Test]
        public async Task CreateOpenShiftAsync_ReturnsStartAfterEndError_WhenStartTimeIsAfterEndTime()
        {
            // Act
            var result = await _shiftService.CreateOpenShiftAsync(DateTime.Today.AddHours(12), DateTime.Today.AddHours(11));

            // Assert
            Assert.That(result.Type, Is.EqualTo(CreateOpenShiftResult.ResultType.StartAfterEnd));
        }

        [Test]
        public async Task CreateOpenShiftAsync_ReturnsDifferentDaysError_WhenStartAndEndTimesAreOnDifferentDays()
        {
            // Act
            var result = await _shiftService.CreateOpenShiftAsync(DateTime.Today.AddHours(12), DateTime.Today.AddDays(1).AddHours(12));

            // Assert
            Assert.That(result.Type, Is.EqualTo(CreateOpenShiftResult.ResultType.StartAndEndDifferentDays));
        }

        [Test]
        public async Task CreateOpenShiftAsync_CreatesShift_WhenValid()
        {
            // Arrange
            var shift = new Shift(null, null, DateTime.Today.AddHours(12), DateTime.Today.AddHours(16));
            _mockAddShiftQuery.Setup(x => x.QueryAsync(It.IsAny<Shift>())).ReturnsAsync(1L);

            // Act
            var result = await _shiftService.CreateOpenShiftAsync(shift.Start, shift.End);

            // Assert
            Assert.That(result.Type, Is.EqualTo(CreateOpenShiftResult.ResultType.Success));
            Assert.That(result.Shift.Id, Is.EqualTo(1));
        }
        #endregion

        #region AssignEmployeeToShiftAsync Tests
        [Test]
        public async Task AssignEmployeeToShiftAsync_ReturnsEmployeeDoesNotExist_WhenEmployeeNotFound()
        {
            // Arrange
            _mockGetEmployeeByIdQuery.Setup(x => x.QueryAsync(It.IsAny<long>())).ReturnsAsync((Employee)null);

            // Act
            var result = await _shiftService.AssignEmployeeToShiftAsync(1, 1);

            // Assert
            Assert.That(result.Type, Is.EqualTo(AssignEmployeeToShiftResult.ResultType.EmployeeDoesNotExist));
        }

        [Test]
        public async Task AssignEmployeeToShiftAsync_ReturnsShiftDoesNotExist_WhenShiftNotFound()
        {
            // Arrange
            var employee = new Employee(1, "John Smith");

            _mockGetShiftByIdQuery.Setup(x => x.QueryAsync(It.IsAny<long>())).ReturnsAsync((Shift)null);
            _mockGetEmployeeByIdQuery.Setup(x => x.QueryAsync(It.IsAny<long>())).ReturnsAsync(employee);

            // Act
            var result = await _shiftService.AssignEmployeeToShiftAsync(1, 1);

            // Assert
            Assert.That(result.Type, Is.EqualTo(AssignEmployeeToShiftResult.ResultType.ShiftDoesNotExist));
        }

        [Test]
        public async Task AssignEmployeeToShiftAsync_ReturnsShiftConflict_WhenOverlappingShiftsExist()
        {
            // Arrange
            var shift = new Shift(1, null, DateTime.Today.AddHours(12), DateTime.Today.AddHours(16));
            var employee = new Employee(1, "John Smith");
            var overlappingShift = new Shift(2, 1, DateTime.Today.AddHours(9), DateTime.Today.AddHours(13));

            _mockGetEmployeeByIdQuery.Setup(x => x.QueryAsync(1)).ReturnsAsync(employee);
            _mockGetShiftByIdQuery.Setup(x => x.QueryAsync(1)).ReturnsAsync(shift);
            _mockGetShiftsByEmployeeQuery.Setup(x => x.QueryAsync(1))
                .ReturnsAsync(new List<Shift> { overlappingShift });

            // Act
            var result = await _shiftService.AssignEmployeeToShiftAsync(1, 1);

            // Assert
            Assert.That(result.Type, Is.EqualTo(AssignEmployeeToShiftResult.ResultType.ShiftConflict));
        }

        [Test]
        public async Task AssignEmployeeToShiftAsync_AssignsEmployee_WhenNoConflictsExist()
        {
            // Arrange
            var shift = new Shift(1, null, DateTime.Today.AddHours(12), DateTime.Today.AddHours(16));
            var employee = new Employee(1, "John Smith");

            _mockGetEmployeeByIdQuery.Setup(x => x.QueryAsync(1)).ReturnsAsync(employee);
            _mockGetShiftByIdQuery.Setup(x => x.QueryAsync(1)).ReturnsAsync(shift);
            _mockGetShiftsByEmployeeQuery.Setup(x => x.QueryAsync(1)).ReturnsAsync(new List<Shift>());
            _mockUpdateShiftQuery.Setup(x => x.QueryAsync(shift)).Returns(Task.CompletedTask);

            // Act
            var result = await _shiftService.AssignEmployeeToShiftAsync(1, 1);

            // Assert
            Assert.That(result.Type, Is.EqualTo(AssignEmployeeToShiftResult.ResultType.Success));
            _mockUpdateShiftQuery.Verify(x => x.QueryAsync(It.IsAny<Shift>()), Times.Once);
        }
        #endregion
    }
}
