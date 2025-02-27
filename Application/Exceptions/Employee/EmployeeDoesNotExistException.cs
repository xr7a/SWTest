namespace Application.Exceptions.Employee;

public class EmployeeDoesNotExistException(int id): Exception($"Сотрудника с id {id} не существует");