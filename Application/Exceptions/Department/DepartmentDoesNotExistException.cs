namespace Application.Exceptions.Department;

public class DepartmentDoesNotExistException(int id) : Exception($"Отдел с id {id} не существует");
