namespace Application.Exceptions.Employee;

public class PhoneIsAlreadyUsedException(string phone) : AppException($"Телефон {phone} уже используется", 409);