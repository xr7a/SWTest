namespace Application.Exceptions.Employee;

public class EmployeeAlreadyExistException(string phone): Exception($"Сотрудник с телефоном {phone} уже существует");