update departments
set name = coalesce(@name, name),
    phone = coalesce(@phone, phone)
where id = @id
returning id, name, phone