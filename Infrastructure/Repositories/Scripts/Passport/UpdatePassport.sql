update passports
set type = coalesce(@type, type),
    number = coalesce(@number, number)
where employee_id = @employeeId
returning type, number