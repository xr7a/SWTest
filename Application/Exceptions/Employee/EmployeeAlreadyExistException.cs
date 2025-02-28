namespace Application.Exceptions.Employee;

public class EmployeeAlreadyExistException(string phone): AppException($"Сотрудник с телефоном {phone} уже существует", 409);