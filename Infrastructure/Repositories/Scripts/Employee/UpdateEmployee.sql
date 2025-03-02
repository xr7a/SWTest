update employees
set name = coalesce(@name, name),
    surname = coalesce(@surname, surname),
    phone = coalesce(@phone, phone),
    company_id = coalesce(@companyId, company_id),
    department_id = coalesce(@departmentId, department_id)
where id = @id
returning name, surname, phone, company_id as "CompanyId", department_id as "DepartmentId"