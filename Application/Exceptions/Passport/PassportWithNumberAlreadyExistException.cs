namespace Application.Exceptions.Passport;

public class PassportWithNumberAlreadyExistException(string number)
    : AppException($"Паспорт с номером {number} уже существует", 409);