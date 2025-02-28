namespace Application.Exceptions.Department;

public class DepartmentAlreadyExistException(string phone) : AppException($"Отдел с телефоном {phone} уже сушествует", 409);