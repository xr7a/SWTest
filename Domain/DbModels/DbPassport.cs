﻿namespace Domain.DbModels;

public class DbPassport
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Type { get; set; }

    public string Number { get; set; }
}