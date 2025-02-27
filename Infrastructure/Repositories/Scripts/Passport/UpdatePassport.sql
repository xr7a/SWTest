update passports
set type = coalesce(@type, type),
    number = coalesce(@number, number)
where id = @passportId