namespace Application.Exceptions.Department;

public class DepartmentDoesNotExistException(int id) : AppException($"Отдел с id {id} не существует", 404);
