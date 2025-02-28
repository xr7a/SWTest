namespace Application.Exceptions.Employee;

public class EmployeeDoesNotExistException(int id): AppException($"Сотрудника с id {id} не существует", 404);